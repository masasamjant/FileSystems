namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents arguments of <see cref="BackupTask.StateChanged"/> event.
    /// </summary>
    public sealed class BackupTaskStateChangedEventArgs : BackupTaskEventArgs
    {
        /// <summary>
        /// Initializes new instance of the <see cref="BackupTaskStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="currentState">The current backup task state.</param>
        /// <param name="currentDirectoryPath">The current directory path or <c>null</c>, if not processing directory.</param>
        /// <param name="currentFilePath">The current file path or <c>null</c>, if not processing file.</param>
        /// <param name="canCancel"><c>true</c> if task can be canceled in this event; <c>false</c> otherwise.</param>
        internal BackupTaskStateChangedEventArgs(BackupTaskState currentState, string? currentDirectoryPath, string? currentFilePath, bool canCancel)
            : base(currentDirectoryPath, currentFilePath, canCancel)
        {
            CurrentState = currentState;
        }

        /// <summary>
        /// Gets the current backup task state.
        /// </summary>
        public BackupTaskState CurrentState { get; }
    }
}
