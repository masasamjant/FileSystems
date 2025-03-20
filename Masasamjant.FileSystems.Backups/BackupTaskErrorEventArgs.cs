namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents arguments of <see cref="BackupTask.Error"/> event.
    /// </summary>
    public sealed class BackupTaskErrorEventArgs : BackupTaskEventArgs
    {
        private BackupTaskErrorBehavior errorBehavior = BackupTaskErrorBehavior.Cancel;

        /// <summary>
        /// Initializes new instance of the <see cref="BackupTaskErrorEventArgs"/> class.
        /// </summary>
        /// <param name="error">The exception of the error.</param>
        /// <param name="currentState">The current state of the task.</param>
        /// <param name="currentDirectoryPath">The current directory path or <c>null</c>, if not processing directory.</param>
        /// <param name="currentFilePath">The current file path or <c>null</c>, if not processing file.</param>
        internal BackupTaskErrorEventArgs(Exception error, BackupTaskState currentState, string? currentDirectoryPath, string? currentFilePath)
            : base(currentDirectoryPath, currentFilePath, true)
        {
            Error = error;
            Handled = false;
            Cancel = true;
        }

        /// <summary>
        /// Gets <see cref="Exception"/> of the error.
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Gets or sets whether or not error was handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets how task should behave in error.
        /// </summary>
        public BackupTaskErrorBehavior ErrorBehavior
        {
            get { return errorBehavior; }
            set
            {
                if (!Enum.IsDefined(value))
                    throw new ArgumentException("The value is not defined.", nameof(ErrorBehavior));

                if (errorBehavior != value)
                {
                    errorBehavior = value;
                    Cancel = value == BackupTaskErrorBehavior.Cancel;
                }
            }
        }

        /// <summary>
        /// Gets the current state of the task.
        /// </summary>
        public BackupTaskState CurrentState { get; }

        /// <summary>
        /// Gets or sets whether or not backup should be canceled. 
        /// </summary>
        public override bool Cancel 
        { 
            get => ErrorBehavior == BackupTaskErrorBehavior.Cancel;
            set 
            {
                errorBehavior = value ? BackupTaskErrorBehavior.Cancel : BackupTaskErrorBehavior.Continue;
            } 
        }
    }
}
