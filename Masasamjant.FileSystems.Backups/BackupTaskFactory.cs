using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents factory to create backup task.
    /// </summary>
    public static class BackupTaskFactory
    {
        /// <summary>
        /// Creates correct backup task for specified backup properties.
        /// </summary>
        /// <param name="properties">The backup properties.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <returns>A backup task.</returns>
        public static BackupTask CreateBackupTask(BackupProperties properties, IFileSystem fileSystem)
        {
            switch (properties.BackupMode)
            {
                case BackupMode.Full:
                    return new FullBackupTask(properties, fileSystem);
                case BackupMode.Differential:
                    return CreateDifferentialBackupTask(properties, fileSystem);
                case BackupMode.Incremental:
                    return CreateIncrementalBackupTask(properties, fileSystem);
                case BackupMode.Continuous:
                    return CreateContinuousBackupTask(properties, fileSystem);
                default:
                    throw new NotSupportedException($"Backup mode '{properties.BackupMode}' is not supported.");
            }
        }

        private static BackupTask CreateDifferentialBackupTask(BackupProperties properties, IFileSystem fileSystem) 
        {
            var sourceDirectory = fileSystem.DirectoryOperations.GetDirectory(properties.SourceDirectoryPath);
            var destinationDirectory = fileSystem.DirectoryOperations.GetDirectory(properties.DestinationDirectoryPath);
            var latestFullBackupDirectory = GetLatestFullBackupDirectory(destinationDirectory, sourceDirectory.Name);
            return new DifferentialBackupTask(properties, latestFullBackupDirectory, fileSystem);
        }

        private static BackupTask CreateIncrementalBackupTask(BackupProperties properties, IFileSystem fileSystem)
        {
            var sourceDirectory = fileSystem.DirectoryOperations.GetDirectory(properties.SourceDirectoryPath);
            var destinationDirectory = fileSystem.DirectoryOperations.GetDirectory(properties.DestinationDirectoryPath);
            var latestFullBackupDirectory = GetLatestFullBackupDirectory(destinationDirectory, sourceDirectory.Name);

            if (latestFullBackupDirectory != null)
            { 
                var timestamp = BackupTask.GetFullBackupTimestamp(latestFullBackupDirectory);
                var directories = destinationDirectory.GetDirectories();
                var incrementalBackupDirectories = directories.Where(directory => directory.Name.Contains("_I_") && 
                    directory.Name.Contains(timestamp)).OrderBy(directory => directory.CreationTime).ToList();
                
                // If there is no incremental directories created from full backup, then stars new incremental chain.
                bool validIncrementalBackupChain = incrementalBackupDirectories.Count == 0;

                // There is incremental directories, so check that chain is not broken.
                if (!validIncrementalBackupChain)
                {
                    validIncrementalBackupChain = true;
                    int incrementalNumber = 1;
                    foreach (var incrementalDirectory in incrementalBackupDirectories)
                    {
                        var nameParts = incrementalDirectory.Name.Split('_');

                        if (nameParts.Length != 5 || nameParts[4] != incrementalNumber.ToString())
                        {
                            validIncrementalBackupChain = false;
                            break;
                        }

                        incrementalNumber++;
                    }
                }

                if (validIncrementalBackupChain)
                    return new IncrementalBackupTask(properties, latestFullBackupDirectory, incrementalBackupDirectories, fileSystem);
            }

            return new FullBackupTask(properties, fileSystem);
        }

        private static BackupTask CreateContinuousBackupTask(BackupProperties properties, IFileSystem fileSystem)
        {
            var sourceDirectory = fileSystem.DirectoryOperations.GetDirectory(properties.SourceDirectoryPath);
            var destinationDirectory = fileSystem.DirectoryOperations.GetDirectory(properties.DestinationDirectoryPath);
            var latestFullBackupDirectory = GetLatestFullBackupDirectory(destinationDirectory, sourceDirectory.Name);
            return new ContinuousBackupTask(properties, latestFullBackupDirectory, fileSystem);
        }

        private static IDirectoryInfo? GetLatestFullBackupDirectory(IDirectoryInfo destinationDirectory, string sourceDirectoryName)
        {
            IDirectoryInfo? latestFullBackupDirectory = null;
            var directories = destinationDirectory.GetDirectories();
            if (directories.Any())
                latestFullBackupDirectory = directories.Where(directory => directory.Name.Contains(sourceDirectoryName) && directory.Name.Contains("_F"))
                    .OrderByDescending(directory => directory.CreationTime).FirstOrDefault();
            return latestFullBackupDirectory;
        }
    }
}
