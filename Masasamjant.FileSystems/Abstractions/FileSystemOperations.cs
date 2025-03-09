namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Represents operations performed to file system.
    /// </summary>
    public abstract class FileSystemOperations : IFileSystemOperations
    {
        /// <summary>
        /// Initializes new instance of <see cref="FileSystemOperations"/> class.
        /// </summary>
        /// <param name="fileSystem">The target <see cref="IFileSystem"/>.</param>
        protected FileSystemOperations(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <summary>
        /// Gets the target file system.
        /// </summary>
        public IFileSystem FileSystem { get; }
    }
}
