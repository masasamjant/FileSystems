namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Represents file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets the <see cref="IDriveOperations"/>.
        /// </summary>
        IDriveOperations DriveOperations { get; }

        /// <summary>
        /// Gets the <see cref="IDirectoryOperations"/>.
        /// </summary>
        IDirectoryOperations DirectoryOperations { get; }

        /// <summary>
        /// Gets the <see cref="IFileOperations"/>.
        /// </summary>
        IFileOperations FileOperations { get; }
    }
}
