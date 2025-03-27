namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents arguments of <see cref="BackupTask.FileBackup"/> event.
    /// </summary>
    public sealed class BackupTaskFileEventArgs : BackupTaskEventArgs
    {
        /// <summary>
        /// Initializes new instance of the <see cref="BackupTaskFileEventArgs"/> class.
        /// </summary>
        /// <param name="properties">The properties of backup task.</param>
        /// <param name="backupDirectoryPath">The full path of backup directory.</param>
        /// <param name="backupFilePath">The full path of backup file.</param>
        /// <param name="currentDirectoryPath">The current directory path or <c>null</c>, if not processing directory.</param>
        /// <param name="currentFilePath">The current file path or <c>null</c>, if not processing file.</param>
        internal BackupTaskFileEventArgs(BackupProperties properties, string backupDirectoryPath, string backupFilePath, string? currentDirectoryPath, string? currentFilePath)
            : base(properties, currentDirectoryPath, currentFilePath, false)
        {
            BackupDirectoryPath = backupDirectoryPath;
            BackupFilePath = backupFilePath;
        }

        /// <summary>
        /// Gets the full path of backup directory.
        /// </summary>
        public string BackupDirectoryPath { get; }

        /// <summary>
        /// Gets the full path of backup file.
        /// </summary>
        public string BackupFilePath { get; }
    }
}
