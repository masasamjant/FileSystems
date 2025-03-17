using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents backup task that creates full backup based on specified <see cref="BackupProperties"/>.
    /// </summary>
    public sealed class FullBackupTask : BackupTask
    {
        private IDirectoryInfo? sourceDirectory;
        private IDirectoryInfo? destinationDirectory;
        private string rootBackupDirectoryName;
        private string rootBackupDirectoryPath;

        /// <summary>
        /// Initializes new instance of the <see cref="FullBackupTask"/> class.
        /// </summary>
        /// <param name="properties">The <see cref="BackupProperties"/>.</param>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        public FullBackupTask(BackupProperties properties, IFileSystem fileSystem) 
            : base(properties, fileSystem)
        {
            rootBackupDirectoryName = string.Empty;
            rootBackupDirectoryPath = string.Empty;
        }

        protected override void PreExecute()
        {
            CheckDisposed();

            if (IsCanceled)
                return;

            sourceDirectory = DirectoryOperations.GetDirectory(Properties.SourceDirectoryPath);
            destinationDirectory = DirectoryOperations.GetDirectory(Properties.DestinationDirectoryPath);
            rootBackupDirectoryName = string.Format(FullBackupDirectoryPathFormat, sourceDirectory.Name, GetTimestampString());
            rootBackupDirectoryPath = Path.Combine(destinationDirectory.FullName, rootBackupDirectoryName);
        }

        protected override BackupTaskResult Execute()
        {
            CheckDisposed();

            if (IsCanceled)
                return new BackupTaskResult(BackupMode.Full, Properties, string.Empty);

            BackupDirectory(sourceDirectory!, rootBackupDirectoryPath);

            if (IsCanceled)
            {
                if (DirectoryOperations.Exists(rootBackupDirectoryPath))
                    DirectoryOperations.Delete(rootBackupDirectoryPath);

                return new BackupTaskResult(BackupMode.Full, Properties, string.Empty);
            }

            return new BackupTaskResult(BackupMode.Full, Properties, rootBackupDirectoryPath);
        }

        protected override void PostExecute()
        {
            sourceDirectory = null;
            destinationDirectory = null;
            rootBackupDirectoryName = string.Empty;
            rootBackupDirectoryPath = string.Empty;
        }

        private void BackupDirectory(IDirectoryInfo sourceDirectory, string backupDirectoryPath)
        {
            if (IsCanceled)
                return;

            if (DirectoryOperations.Exists(backupDirectoryPath))
                throw new BackupException($"Cannot backup to existing folder {backupDirectoryPath}.");

            // Flag to create directory if at least one file is backed up.
            bool createDirectory = true;

            var sourceFiles = sourceDirectory.GetFiles();

            var history = new BackupHistory(backupDirectoryPath);

            foreach (var sourceFile in sourceFiles)
            {
                try
                {
                    CurrentDirectoryPath = sourceFile.DirectoryName;
                    CurrentFilePath = sourceFile.FullName;
                    var destinationFileName = sourceFile.Name;
                    var destinationFilePath = Path.Combine(backupDirectoryPath, destinationFileName);
                    var sourceFileHash = ComputeFileHash(sourceFile);
                    CreateBackup(sourceFile, destinationFilePath, backupDirectoryPath, ref createDirectory);
                    history.Set(sourceFile.FullName, sourceFileHash);
                    OnFileBackup(new BackupTaskFileEventArgs(backupDirectoryPath, destinationFilePath, CurrentDirectoryPath, CurrentFilePath));
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
                    {
                        throw new BackupException($"Error backing up {CurrentFilePath}.", exception);
                    }
                }
                finally
                {
                    CurrentDirectoryPath = null;
                    CurrentFilePath = null;
                }
            }

            BackupHistory.Save(history, FileOperations);

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
                        BackupDirectory(childDirectory, childBackupDirectoryPath);
                    }
                }
            }
        }
    }
}
