namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Represents operations performed to file system.
    /// </summary>
    public interface IFileSystemOperations
    {
        /// <summary>
        /// Gets the target file system.
        /// </summary>
        IFileSystem FileSystem { get; }
    }
}
