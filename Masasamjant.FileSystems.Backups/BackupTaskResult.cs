namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents results of backup task.
    /// </summary>
    public sealed class BackupTaskResult
    {
        /// <summary>
        /// Initializes new instance of the <see cref="BackupTaskResult"/> class.
        /// </summary>
        /// <param name="backupMode">The final backup mode.</param>
        /// <param name="properties">The backup properties.</param>
        /// <param name="backupDirectoryPath">The full path to backup directory or empty string.</param>
        internal BackupTaskResult(BackupMode backupMode, BackupProperties properties, string backupDirectoryPath)
        {
            BackupMode = backupMode;
            Properties = properties;
            BackupDirectoryPath = backupDirectoryPath;
        }

        /// <summary>
        /// Gets the actual backup mode that was made.
        /// </summary>
        public BackupMode BackupMode { get; }

        /// <summary>
        /// Gets the backup properties.
        /// </summary>
        public BackupProperties Properties { get; }
    
        /// <summary>
        /// Gets the final state of backup task.
        /// </summary>
        public BackupTaskState FinalState { get; private set; }

        /// <summary>
        /// Gets the full path to backup directory.
        /// </summary>
        public string BackupDirectoryPath { get; }

        /// <summary>
        /// Gets whether or not backup was created.
        /// </summary>
        public bool BackupCreated
        {
            get { return FinalState == BackupTaskState.Completed && !string.IsNullOrEmpty(BackupDirectoryPath); }
        }

        internal void SetFinalState(BackupTaskState finalState)
        {
            FinalState = finalState;
        }
    }
}
