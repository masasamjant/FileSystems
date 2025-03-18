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

            if (FullBackupDirectory != null)
                fullBackupDirectoryPath = FullBackupDirectory.FullName;
            else
            {
                var destinationDirectory = DirectoryOperations.GetDirectory(Properties.DestinationDirectoryPath);
                fullBackupDirectoryPath = string.Format(FullBackupDirectoryPathFormat, sourceDirectory.Name, GetTimestampString());
                fullBackupDirectoryPath = Path.Combine(destinationDirectory.FullName, fullBackupDirectoryPath);
            }

            BackupDirectory(sourceDirectory, fullBackupDirectoryPath);

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
                    var destinationFileName = sourceFile.Name;
                    var destinationFilePath = Path.Combine(backupDirectoryPath, destinationFileName);
                    var sourceFileHash = ComputeFileHash(sourceFile);

                    bool createBackup = noBackupHistory || !history.Contains(sourceFile.FullName) || sourceFileHash != history.Get(sourceFile.FullName);

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

            if (Properties.IncludeSubDirectories)
            {
                var childDirectories = sourceDirectory.GetDirectories();

                if (childDirectories.Any())
                {
                    EnsureCreateDirectory(backupDirectoryPath, ref createDirectory);

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
