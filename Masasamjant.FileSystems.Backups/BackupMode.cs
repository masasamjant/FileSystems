namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Defines supported backup modes.
    /// </summary>
    public enum BackupMode : int
    {
        /// <summary>
        /// Create full backup from the source folder.
        /// </summary>
        Full = 0,

        /// <summary>
        /// Create differential backup from source folder. Backups all modified or
        /// created files since latest full backup.
        /// </summary>
        Differential = 1,

        /// <summary>
        /// Create incremental backup from source folder. Backup all modified or 
        /// created files since latest incremental backup.
        /// </summary>
        Incremental = 2,

        /// <summary>
        /// Create continuous backup from source folder. Backup all modified or 
        /// created files since last backup. Use single folder and overwrites existing files.
        /// This mode optimizes disk space usage, but file history is lost.
        /// </summary>
        Continuous = 3
    }
}
