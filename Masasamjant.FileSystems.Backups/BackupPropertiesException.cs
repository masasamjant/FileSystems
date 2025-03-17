namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents exception because of invalid backup properties.
    /// </summary>
    public class BackupPropertiesException : BackupException
    {
        /// <summary>
        /// Initializes new instance of the <see cref="BackupPropertiesException"/> class.
        /// </summary>
        /// <param name="properties">The invalid backup properties.</param>
        public BackupPropertiesException(BackupProperties properties)
            : this("Unexpected exception using specified backup properties.", properties)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="BackupPropertiesException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="properties">The invalid backup properties.</param>
        public BackupPropertiesException(string message, BackupProperties properties)
            : this(message, properties, null)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="BackupPropertiesException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="properties">The invalid backup properties.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public BackupPropertiesException(string message, BackupProperties properties, Exception? innerException)
            : base(message, innerException)
        {
            Properties = properties;
        }

        /// <summary>
        /// Gets the invalid backup properties.
        /// </summary>
        public BackupProperties Properties { get; }
    }
}
