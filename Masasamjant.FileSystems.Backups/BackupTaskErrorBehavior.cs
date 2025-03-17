namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Defines how backup task should behave in error.
    /// </summary>
    public enum BackupTaskErrorBehavior : int
    {
        /// <summary>
        /// Cancel task.
        /// </summary>
        Cancel = 0,

        /// <summary>
        /// Continue task.
        /// </summary>
        Continue = 1
    }
}
