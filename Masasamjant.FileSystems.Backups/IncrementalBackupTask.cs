using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents backup task that creates incremental backup based on specified <see cref="BackupProperties"/>.
    /// </summary>
    public sealed class IncrementalBackupTask : BackupTask
    {
        /// <summary>
        /// Initializes new instance of the <see cref="DifferentialBackupTask"/> class.
        /// </summary>
        /// <param name="properties">The <see cref="BackupProperties"/>.</param>
        /// <param name="fullBackupDirectory">The <see cref="IDirectoryInfo"/> of base full backup or <c>null</c>, if there is no base full backup.</param>
        /// <param name="incrementalBackupDirectories">The incremental backup directories.</param>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <exception cref="ArgumentException">If <see cref="BackupProperties.BackupMode"/> of <paramref name="properties"/> is not <see cref="BackupMode.Incremental"/>.</exception>
        public IncrementalBackupTask(BackupProperties properties, IDirectoryInfo? fullBackupDirectory, IEnumerable<IDirectoryInfo> incrementalBackupDirectories, IFileSystem fileSystem)
            : base(properties, fileSystem)
        {
            if (properties.BackupMode != BackupMode.Incremental)
                throw new ArgumentException("The properties are not for incremental backup.", nameof(properties));

            FullBackupDirectory = fullBackupDirectory;
            IncrementalBackupDirectories = incrementalBackupDirectories;
        }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of the base full backup.
        /// </summary>
        public IDirectoryInfo? FullBackupDirectory { get; }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/>s of the previous incremental backups.
        /// </summary>
        public IEnumerable<IDirectoryInfo> IncrementalBackupDirectories { get; }

        protected override void PreExecute()
        {
            CheckDisposed();
            if (IsCanceled)
                return;
        }

        protected override BackupTaskResult Execute()
        {
            CheckDisposed();
            
            if (IsCanceled)
                return new BackupTaskResult(BackupMode.Full, Properties, string.Empty);

            // First load complete history from previous full backup and all incremental backups done after that.
            var previousBackupHistories = LoadCompleteBackupHistory();

            // For incremental backup there must be full backup or previous incremental backups. If not then create new full backup.
            if (FullBackupDirectory == null || !FullBackupDirectory.Exists || previousBackupHistories.Count == 0)
            {
                using (var task = new FullBackupTask(Properties, FileSystem))
                {
                    try
                    {
                        task.Error += OnFullBackupTaskError;
                        task.StateChanged += OnFullBackupTaskStateChanged;
                        task.FileBackup += OnFullBackupTaskFileBackup;
                        return task.Run();
                    }
                    finally
                    {
                        task.Error -= OnFullBackupTaskError;
                        task.StateChanged -= OnFullBackupTaskStateChanged;
                        task.FileBackup -= OnFullBackupTaskFileBackup;
                    }
                }
            }
            else
            {
                // Create incremental backup root directory path.
                var sourceDirectory = DirectoryOperations.GetDirectory(Properties.SourceDirectoryPath);
                var destinationDirectory = DirectoryOperations.GetDirectory(Properties.DestinationDirectoryPath);
                var incrementalBackupDirectoryName = string.Format(IncrementalBackupDirectoryPathFormat, sourceDirectory.Name, GetFullBackupTimestamp(FullBackupDirectory), GetTimestampString(), GetIncrementalCount());
                var incrementalBackupDirectoryPath = Path.Combine(destinationDirectory.FullName, incrementalBackupDirectoryName);

                // Backup root directory.
                BackupDirectory(sourceDirectory, incrementalBackupDirectoryPath, previousBackupHistories);

                // Check if operation was canceled. If so, then try delete destination folder.
                if (IsCanceled)
                {
                    if (DirectoryOperations.Exists(incrementalBackupDirectoryPath))
                        DirectoryOperations.TryDelete(incrementalBackupDirectoryPath, Properties.IncludeSubDirectories);

                    return new BackupTaskResult(BackupMode.Full, Properties, string.Empty);
                }

                return new BackupTaskResult(BackupMode.Incremental, Properties, incrementalBackupDirectoryPath);
            }
        }

        protected override void PostExecute()
        {
            return;
        }

        private void BackupDirectory(IDirectoryInfo sourceDirectory, string backupDirectoryPath, BackupHistory previousBackupHistory)
        {
            if (IsCanceled)
                return;

            if (DirectoryOperations.Exists(backupDirectoryPath))
                throw new BackupException($"Cannot backup to existing folder {backupDirectoryPath}.");

            bool createDirectory = true;
            bool noPreviousBackupHistory = previousBackupHistory.Count == 0;
            var history = new BackupHistory();
            var sourceFiles = sourceDirectory.GetFiles();

            foreach (var sourceFile in sourceFiles)
            {
                try
                {
                    CurrentDirectoryPath = sourceFile.DirectoryName;
                    CurrentFilePath = sourceFile.FullName;

                    // Create destination file path.
                    var destinationFileName = sourceFile.Name;
                    var destinationFilePath = Path.Combine(backupDirectoryPath, destinationFileName);

                    // Compute hash used to verify if file needs backup or not.
                    var sourceFileHash = ComputeFileHash(sourceFile);

                    // Create backup when:
                    // - there is no previous history at all
                    // - file is not present in history
                    // - file hash is present in history but hashes mismatch
                    bool createBackup = noPreviousBackupHistory ||
                        !previousBackupHistory.Contains(sourceFile.FullName) ||
                        sourceFileHash != previousBackupHistory.Get(sourceFile.FullName);

                    if (createBackup)
                    {
                        // Create the actual backup.
                        CreateBackup(sourceFile, destinationFilePath, backupDirectoryPath, ref createDirectory);

                        // Store backup history.
                        history.Set(sourceFile.FullName, sourceFileHash);

                        // Raise event to notify that backup was done.
                        OnFileBackup(new BackupTaskFileEventArgs(backupDirectoryPath, destinationFilePath, CurrentDirectoryPath, CurrentFilePath));
                    }
                }
                catch (Exception exception)
                {
                    var args = HandleBackupFileError(exception);
                    if (args.Handled)
                    {
                        if (args.ErrorBehavior == BackupTaskErrorBehavior.Cancel)
                        {
                            IsCanceled = true;
                            return;
                        }
                        else
                            continue;
                    }
                    else
                        throw new BackupException($"Error backing up {CurrentFilePath}.", exception);
                }
                finally
                {
                    CurrentDirectoryPath = null;
                    CurrentFilePath = null;
                }
            }

            // Save backup history.
            BackupHistory.Save(history, backupDirectoryPath, FileOperations);

            // If sub directories should be included in backup, then iterate all child directories.
            if (Properties.IncludeSubDirectories)
            {
                var childDirectories = sourceDirectory.GetDirectories();

                if (childDirectories.Any())
                {
                    // Since there are some child directories, lets ensure that parent backup directory is created.
                    EnsureCreateDirectory(backupDirectoryPath, ref createDirectory);

                    // Backup each child directory and eventually the whole tree.
                    foreach (var childDirectory in childDirectories)
                    {
                        var childBackupDirectoryPath = Path.Combine(backupDirectoryPath, childDirectory.Name);
                        BackupDirectory(childDirectory, childBackupDirectoryPath, previousBackupHistory);
                    }
                }
            }
        }

        private void OnFullBackupTaskFileBackup(object? sender, BackupTaskFileEventArgs e)
        {
            OnFileBackup(e);
        }

        private void OnFullBackupTaskStateChanged(object? sender, BackupTaskStateChangedEventArgs e)
        {
            OnStateChanged(e);
        }

        private void OnFullBackupTaskError(object? sender, BackupTaskErrorEventArgs e)
        {
            OnError(e);

            if (e.Handled && e.ErrorBehavior == BackupTaskErrorBehavior.Cancel)
                IsCanceled = true;
            else
                throw e.Error;
        }

        private int GetIncrementalCount() => IncrementalBackupDirectories.Count() + 1;

        private BackupHistory LoadCompleteBackupHistory()
        {
            var histories = new List<BackupHistory>();

            // Load history in full backup folder.
            if (FullBackupDirectory != null && FullBackupDirectory.Exists)
            {
                histories.Add(BackupHistory.Load(FullBackupDirectory.FullName, FileOperations));

                if (Properties.IncludeSubDirectories)
                    LoadBackupHistories(FullBackupDirectory.GetDirectories(), histories);
            }

            // Then get incremental backup folders and load history.
            foreach (var incrementalBackupDirectory in IncrementalBackupDirectories)
            { 
                histories.Add(BackupHistory.Load(incrementalBackupDirectory.FullName, FileOperations));
                if (Properties.IncludeSubDirectories)
                    LoadBackupHistories(incrementalBackupDirectory.GetDirectories(), histories);
            }

            return BackupHistory.Merge(histories);
        }

        private void LoadBackupHistories(IEnumerable<IDirectoryInfo> directories, List<BackupHistory> histories)
        {
            foreach (var directory in directories)
            {
                var history = BackupHistory.Load(directory.FullName, FileOperations);
                if (history.Count > 0)
                    histories.Add(history);
                LoadBackupHistories(directory.GetDirectories(), histories);
            }
        }
    }
}
