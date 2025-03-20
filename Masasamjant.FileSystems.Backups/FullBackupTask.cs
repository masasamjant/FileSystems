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
        /// <exception cref="ArgumentException">If <see cref="BackupProperties.BackupMode"/> of <paramref name="properties"/> is <see cref="BackupMode.Continuous"/>.</exception>
        public FullBackupTask(BackupProperties properties, IFileSystem fileSystem) 
            : base(properties, fileSystem)
        {
            if (properties.BackupMode == BackupMode.Continuous)
                throw new ArgumentException("There is not need for full backup when using continuous backup mode.", nameof(properties));

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
                    DirectoryOperations.Delete(rootBackupDirectoryPath, Properties.IncludeSubDirectories);

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

            var history = new BackupHistory();

            foreach (var sourceFile in sourceFiles)
            {
                try
                {
                    CurrentDirectoryPath = sourceFile.DirectoryName;
                    CurrentFilePath = sourceFile.FullName;

                    // Create destination file path.
                    var destinationFileName = sourceFile.Name;
                    var destinationFilePath = Path.Combine(backupDirectoryPath, destinationFileName);
                    
                    // Compute source file hash for history.
                    var sourceFileHash = ComputeFileHash(sourceFile);
                    
                    // Create the actual backup.
                    CreateBackup(sourceFile, destinationFilePath, backupDirectoryPath, ref createDirectory);
                    
                    // Store backup history.
                    history.Set(sourceFile.FullName, sourceFileHash);
                    
                    // Raise event to notify that backup was done.
                    OnFileBackup(new BackupTaskFileEventArgs(backupDirectoryPath, destinationFilePath, CurrentDirectoryPath, CurrentFilePath));
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

            // Save backup history. This is needed if differential/incremental backup is started to create based on this backup.
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
                        BackupDirectory(childDirectory, childBackupDirectoryPath);
                    }
                }
            }
        }
    }
}
