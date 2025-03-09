using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems
{
    /// <summary>
    /// Represents file system.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        /// <summary>
        /// Initializes new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        public FileSystem() 
        {
            DriveOperations = new DriveOperations(this);
            DirectoryOperations = new DirectoryOperations(this);
            FileOperations = new FileOperations(this);
        }

        /// <summary>
        /// Gets the <see cref="IDriveOperations"/>.
        /// </summary>
        public IDriveOperations DriveOperations { get; }

        /// <summary>
        /// Gets the <see cref="IDirectoryOperations"/>.
        /// </summary>
        public IDirectoryOperations DirectoryOperations { get; }

        /// <summary>
        /// Gets the <see cref="IFileOperations"/>.
        /// </summary>
        public IFileOperations FileOperations { get; }
    }
}
