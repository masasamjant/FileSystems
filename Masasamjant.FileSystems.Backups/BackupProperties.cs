using System.Text.Json.Serialization;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents properties of backup.
    /// </summary>
    public sealed class BackupProperties
    {
        /// <summary>
        /// Initializes new instance of the <see cref="BackupProperties"/> class.
        /// </summary>
        /// <param name="backupName">The unique backup name.</param>
        /// <param name="backupMode">The backup mode.</param>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        /// <param name="includeSubDirectories"><c>true</c> to include sub-directories of source directory; <c>false</c> to include only source directory.</param>
        /// <exception cref="ArgumentException">
        /// If value of <paramref name="backupMode"/> is not defined.
        /// -or-
        /// If value of <paramref name="destinationDirectoryPath"/> is same as value of <paramref name="sourceDirectoryPath"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If value of <paramref name="backupName"/>, or value of <paramref name="sourceDirectoryPath"/>, or value of <paramref name="destinationDirectoryPath"/> is empty string or contains only white-space.
        /// </exception>
        public BackupProperties(string backupName, BackupMode backupMode, string sourceDirectoryPath, string destinationDirectoryPath, bool includeSubDirectories)
        {
            if (!Enum.IsDefined(backupMode))
                throw new ArgumentException("The value is not defined.", nameof(backupMode));

            if (string.IsNullOrWhiteSpace(backupName))
                throw new ArgumentNullException(nameof(backupName), "The backup name cannot be empty or only white-space characters.");

            if (string.IsNullOrWhiteSpace(sourceDirectoryPath))
                throw new ArgumentNullException(nameof(sourceDirectoryPath), "The source directory path cannot be empty or only white-space characters.");

            if (string.IsNullOrWhiteSpace(destinationDirectoryPath))
                throw new ArgumentNullException(nameof(destinationDirectoryPath), "The destination directory path cannot be empty or only white-space characters.");

            if (string.Equals(sourceDirectoryPath, destinationDirectoryPath, StringComparison.Ordinal))
                throw new ArgumentException("The destination directory path cannot be same as source directory path.", nameof(destinationDirectoryPath));

            BackupName = backupName;
            BackupMode = backupMode;
            SourceDirectoryPath = sourceDirectoryPath;
            DestinationDirectoryPath = destinationDirectoryPath;
            IncludeSubDirectories = includeSubDirectories;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="BackupProperties"/> class.
        /// </summary>
        public BackupProperties()
        { }

        /// <summary>
        /// Gets the backup name.
        /// </summary>
        [JsonInclude]
        public string BackupName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the backup mode.
        /// </summary>
        [JsonInclude]
        public BackupMode BackupMode { get; internal set; }

        /// <summary>
        /// Gets the source directory path. The path to folder to take backup.
        /// </summary>
        [JsonInclude]
        public string SourceDirectoryPath { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the destination directory path. The path to solder to save backup.
        /// </summary>
        [JsonInclude]
        public string DestinationDirectoryPath { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets whether or not sub-directories of <see cref="SourceDirectoryPath"/> is included in backup. 
        /// If <c>false</c>, then only backup files in <see cref="SourceDirectoryPath"/>; otherwise backup
        /// also directory structure with files.
        /// </summary>
        [JsonInclude]
        public bool IncludeSubDirectories { get; internal set; }
    }
}
