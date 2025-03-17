namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents base exception of backup errors.
    /// </summary>
    public class BackupException : Exception
    {
        /// <summary>
        /// Initializes new default instance of the <see cref="BackupException"/> class.
        /// </summary>
        public BackupException() 
            : this("Unexpected exception in backup process.")
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="BackupException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public BackupException(string message)
            : this(message, null)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="BackupException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public BackupException(string message, Exception? innerException)
            : base(message, innerException) 
        { }
    }
}
