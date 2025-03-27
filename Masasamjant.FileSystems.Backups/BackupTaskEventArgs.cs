namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents base class for arguments of backup task events.
    /// </summary>
    public class BackupTaskEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes new instance of the <see cref="BackupTaskEventArgs"/> class.
        /// </summary>
        /// <param name="properties">The properties of backup task.</param>
        /// <param name="currentDirectoryPath">The current directory path or <c>null</c>, if not processing directory.</param>
        /// <param name="currentFilePath">The current file path or <c>null</c>, if not processing file.</param>
        /// <param name="canCancel"><c>true</c> if task can be canceled in this event; <c>false</c> otherwise.</param>
        protected BackupTaskEventArgs(BackupProperties properties, string? currentDirectoryPath, string? currentFilePath, bool canCancel)
        {
            Properties = properties;
            CurrentDirectoryPath = currentDirectoryPath;
            CurrentFilePath = currentFilePath;
            CanCancel = canCancel;
        }

        /// <summary>
        /// Gets the properties of backup task.
        /// </summary>
        public BackupProperties Properties { get; }

        /// <summary>
        /// Gets the current directory path or <c>null</c>, if not processing folder.
        /// </summary>
        public string? CurrentDirectoryPath { get; }
    
        /// <summary>
        /// Gets the current file path or <c>null</c>, if not processing file.
        /// </summary>
        public string? CurrentFilePath { get; }
    
        /// <summary>
        /// Gets whether or not backup can be canceled at the event.
        /// </summary>
        public bool CanCancel { get; }

        /// <summary>
        /// Gets or sets whether or not backup should be canceled. 
        /// </summary>
        /// <remarks>Have no meaning if <see cref="CanCancel"/> is <c>false</c>.</remarks>
        public virtual bool Cancel { get; set; }
    }
}
