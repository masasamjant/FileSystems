using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents backup task that creates continuous backup based on specified <see cref="BackupProperties"/>.
    /// </summary>
    public sealed class ContinuousBackupTask : BackupTask
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ContinuousBackupTask"/> class.
        /// </summary>
        /// <param name="properties">The <see cref="BackupProperties"/>.</param>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <exception cref="ArgumentException">If <see cref="BackupProperties.BackupMode"/> of <paramref name="properties"/> is not <see cref="BackupMode.Continuous"/>.</exception>
        public ContinuousBackupTask(BackupProperties properties, IFileSystem fileSystem)
            : this(properties, null, fileSystem)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="ContinuousBackupTask"/> class.
        /// </summary>
        /// <param name="properties">The <see cref="BackupProperties"/>.</param>
        /// <param name="fullBackupDirectory">The <see cref="IDirectoryInfo"/> of base full backup or <c>null</c>, if there is no base full backup.</param>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <exception cref="ArgumentException">If <see cref="BackupProperties.BackupMode"/> of <paramref name="properties"/> is not <see cref="BackupMode.Continuous"/>.</exception>
        public ContinuousBackupTask(BackupProperties properties, IDirectoryInfo? fullBackupDirectory, IFileSystem fileSystem)
            : base(properties, fileSystem)
        {
            if (properties.BackupMode != BackupMode.Continuous)
                throw new ArgumentException("The properties are not for continuous backup.", nameof(properties));

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
                return new BackupTaskResult(BackupMode.Continuous, Properties, string.Empty);

            var sourceDirectory = DirectoryOperations.GetDirectory(Properties.SourceDirectoryPath);
            string? fullBackupDirectoryPath;

            // If there is full backup directory, then we continue using that.
            // Otherwise will create new directory.
            if (FullBackupDirectory != null)
                fullBackupDirectoryPath = FullBackupDirectory.FullName;
            else
            {
                var destinationDirectory = DirectoryOperations.GetDirectory(Properties.DestinationDirectoryPath);
                fullBackupDirectoryPath = string.Format(FullBackupDirectoryPathFormat, sourceDirectory.Name, GetTimestampString());
                fullBackupDirectoryPath = Path.Combine(destinationDirectory.FullName, fullBackupDirectoryPath);
            }

            // Backup root directory.
            BackupDirectory(sourceDirectory, fullBackupDirectoryPath);

            // Check if canceled. But do not delete backup folder since we continue use that.
            if (IsCanceled)
                return new BackupTaskResult(BackupMode.Continuous, Properties, string.Empty);

            return new BackupTaskResult(BackupMode.Continuous, Properties, fullBackupDirectoryPath);
        }

        protected override void PostExecute()
        {
            return;
        }

        private void BackupDirectory(IDirectoryInfo sourceDirectory, string backupDirectoryPath)
        {
            if (IsCanceled)
                return;

            bool createDirectory = !DirectoryOperations.Exists(backupDirectoryPath);
            var history = BackupHistory.Load(backupDirectoryPath, FileOperations);
            bool noBackupHistory = history.Count == 0;
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

                    // Create backup if no history or file has changed since added to history.
                    bool createBackup = noBackupHistory || !history.Contains(sourceFile.FullName) || sourceFileHash != history.Get(sourceFile.FullName);

                    if (createBackup)
                    {
                        // Create the actual backup.
                        CreateBackup(sourceFile, destinationFilePath, backupDirectoryPath, ref createDirectory);

                        // Store backup history.
                        history.Set(sourceFile.FullName, sourceFileHash);

                        // Raise event to notify that backup was done.
                        OnFileBackup(new BackupTaskFileEventArgs(Properties, backupDirectoryPath, destinationFilePath, CurrentDirectoryPath, CurrentFilePath));
                    }
                }
                catch (Exception exception)
                {
                    var args = HandleBackupFileError(exception);

                    // Check if error was handled and if should cancel or continue.
                    // If not handler then throw exception and base class will either cancel or flag as error.
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
                        var childBAckupDirectoryPath = Path.Combine(backupDirectoryPath, childDirectory.Name);
                        BackupDirectory(childDirectory, childBAckupDirectoryPath);
                    }
                }
            }
        }
    }
}
