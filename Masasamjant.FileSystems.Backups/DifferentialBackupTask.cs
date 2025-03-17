using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents backup task that creates differential backup based on specified <see cref="BackupProperties"/>.
    /// </summary>
    public sealed class DifferentialBackupTask : BackupTask
    {
        /// <summary>
        /// Initializes new instance of the <see cref="DifferentialBackupTask"/> class.
        /// </summary>
        /// <param name="properties">The <see cref="BackupProperties"/>.</param>
        /// <param name="fullBackupDirectory">The <see cref="IDirectoryInfo"/> of base full backup or <c>null</c>, if there is no base full backup.</param>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        public DifferentialBackupTask(BackupProperties properties, IDirectoryInfo? fullBackupDirectory, IFileSystem fileSystem)
            : base(properties, fileSystem)
        {
            if (properties.BackupMode != BackupMode.Differential)
                throw new ArgumentException("The properties are not for differential backup.", nameof(properties));

            FullBackupDirectory = fullBackupDirectory;
        }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of the base full backup.
        /// </summary>
        public IDirectoryInfo? FullBackupDirectory { get; }

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

            // Before differential backup can be done there must be full backup at base.
            if (FullBackupDirectory == null || !FullBackupDirectory.Exists)
            {
                FullBackupTask task = new FullBackupTask(Properties, FileSystem);

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
            else 
            {
                // There was full backup so it is possible to make differential backup.

                BackupHistory previousBackupHistory = LoadCompleteBackupHistory(FullBackupDirectory);

                var sourceDirectory = DirectoryOperations.GetDirectory(Properties.SourceDirectoryPath);
                var destinationDirectory = DirectoryOperations.GetDirectory(Properties.DestinationDirectoryPath);
                var diffBackupDirectoryName = string.Format(DifferentialBackupDirectoryPathFormat, sourceDirectory.Name, GetFullBackupTimestamp(FullBackupDirectory), GetTimestampString());
                var diffBackupDirectoryPath = Path.Combine(destinationDirectory.FullName, diffBackupDirectoryName);

                BackupDirectory(sourceDirectory, diffBackupDirectoryPath, previousBackupHistory);

                if (IsCanceled)
                {
                    if (Directory.Exists(diffBackupDirectoryPath))
                        Directory.Delete(diffBackupDirectoryPath, Properties.IncludeSubDirectories);

                    return new BackupTaskResult(BackupMode.Full, Properties, string.Empty);
                }

                return new BackupTaskResult(BackupMode.Differential, Properties, diffBackupDirectoryPath);
            }
        }

        protected override void PostExecute()
        {
            if (IsCanceled)
                return;
        }

        private void BackupDirectory(IDirectoryInfo sourceDirectory, string backupDirectoryPath, BackupHistory previousBackupHistory)
        {
            if (IsCanceled)
                return;

            if (DirectoryOperations.Exists(backupDirectoryPath))
                throw new BackupException($"Cannot backup to existing folder {backupDirectoryPath}.");

            bool createDirectory = true;
            bool noPreviousHistory = previousBackupHistory.Count == 0;
            var sourceFiles = sourceDirectory.GetFiles();
            var history = new BackupHistory();

            foreach (var sourceFile in sourceFiles)
            {
                try
                {
                    CurrentDirectoryPath = sourceFile.DirectoryName;
                    CurrentFilePath = sourceFile.FullName;
                    var destinationFileName = sourceFile.Name;
                    var destinationFilePath = Path.Combine(backupDirectoryPath, destinationFileName);

                    // Compute hash used to verify if file needs backup or not.
                    var sourceFileHash = ComputeFileHash(sourceFile);

                    // Create backup when:
                    // - there is no previous history at all
                    // - file is not present in history
                    // - file hash is present in history but mismatch
                    bool createBackup = noPreviousHistory ||
                        !previousBackupHistory.Contains(sourceFile.FullName) ||
                        sourceFileHash != previousBackupHistory.Get(sourceFile.FullName);

                    if (createBackup)
                    {
                        CreateBackup(sourceFile, destinationFilePath, backupDirectoryPath, ref createDirectory);
                        history.Set(sourceFile.FullName, sourceFileHash);
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

            BackupHistory.Save(history, backupDirectoryPath, FileOperations);

            // If sub directories should be included in backup, then iterate all child directories.
            if (Properties.IncludeSubDirectories)
            {
                var childDirectories = sourceDirectory.GetDirectories();
                
                if (childDirectories.Any())
                {
                    // Since there are some child directories, lets ensure that parent backup directory is created.
                    EnsureCreateDirectory(backupDirectoryPath, ref createDirectory);

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

        private BackupHistory LoadCompleteBackupHistory(IDirectoryInfo fullBackupDirectory)
        {
            if (Properties.IncludeSubDirectories)
            {
                List<BackupHistory> histories = new List<BackupHistory>();
                LoadBackupHistories(fullBackupDirectory, histories);
                return BackupHistory.Merge(histories);
            }
            else
            {
                return BackupHistory.Load(fullBackupDirectory.FullName, FileOperations);
            }
        }

        private void LoadBackupHistories(IDirectoryInfo directory, List<BackupHistory> histories)
        {
            var history = BackupHistory.Load(directory.FullName, FileOperations);

            if (history.Count > 0)
                histories.Add(history);

            var childDirectories = directory.GetDirectories();

            foreach (var childDirectory in childDirectories)
                LoadBackupHistories(childDirectory, histories);
        }
    }
}
