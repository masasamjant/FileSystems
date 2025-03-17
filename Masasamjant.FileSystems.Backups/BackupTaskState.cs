namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Defines states of backup task.
    /// </summary>
    public enum BackupTaskState : int
    {
        /// <summary>
        /// Task is just created.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Task is preparing to be executed.
        /// </summary>
        Preparing = 1,

        /// <summary>
        /// Task is prepared to be executed.
        /// </summary>
        Prepared = 2,

        /// <summary>
        /// Task execution starts.
        /// </summary>
        PreExecuting = 3,

        /// <summary>
        /// Task is executing.
        /// </summary>
        Executing = 4,

        /// <summary>
        /// Task execution ends.
        /// </summary>
        PostExecuting = 5,

        /// <summary>
        /// Task has failed.
        /// </summary>
        Failed = 6,

        /// <summary>
        /// Task is canceled.
        /// </summary>
        Canceled = 7,

        /// <summary>
        /// Task is completed.
        /// </summary>
        Completed = 8
    }
}
