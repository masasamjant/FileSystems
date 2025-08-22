using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems
{
    /// <summary>
    /// Provides helper methods to <see cref="IFileSystem"/> interface.
    /// </summary>
    public static class FileSystemHelper
    {
        /// <summary>
        /// Check if file has <see cref="FileAttributes.ReadOnly"/> attribute.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if file has <see cref="FileAttributes.ReadOnly"/> attribute; <c>false</c> otherwise.</returns>
        public static bool IsReadOnlyFile(this IFileSystem fileSystem, string filePath) => HasFileAttribute(fileSystem, filePath, FileAttributes.ReadOnly);

        /// <summary>
        /// Check if file has <see cref="FileAttributes.Hidden"/> attribute.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if file has <see cref="FileAttributes.Hidden"/> attribute; <c>false</c> otherwise.</returns>
        public static bool IsHiddenFile(this IFileSystem fileSystem, string filePath) => HasFileAttribute(fileSystem, filePath, FileAttributes.Hidden);

        /// <summary>
        /// Check if file has specified <see cref="FileAttributes"/> attribute.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="attribute">The <see cref="FileAttributes"/> attribute to check.</param>
        /// <returns><c>true</c> if file has <paramref name="attribute"/>; <c>false</c> otherwise.</returns>
        public static bool HasFileAttribute(this IFileSystem fileSystem, string filePath, FileAttributes attribute)
        {
            var attributes = fileSystem.FileOperations.GetAttributes(filePath);
            return attributes.HasFlag(attribute);
        }

        /// <summary>
        /// Check if directory has <see cref="FileAttributes.ReadOnly"/> attribute.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns><c>true</c> if directory has <see cref="FileAttributes.ReadOnly"/> attribute; <c>false</c> otherwise.</returns>
        public static bool IsReadOnlyDirectory(this IFileSystem fileSystem, string directoryPath) => HasDirectoryAttribute(fileSystem, directoryPath, FileAttributes.ReadOnly);

        /// <summary>
        /// Check if directory has <see cref="FileAttributes.Hidden"/> attribute.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns><c>true</c> if directory has <see cref="FileAttributes.Hidden"/> attribute; <c>false</c> otherwise.</returns>
        public static bool IsHiddenDirectory(this IFileSystem fileSystem, string directoryPath) => HasDirectoryAttribute(fileSystem, directoryPath, FileAttributes.Hidden);

        /// <summary>
        /// Check if directory has specified <see cref="FileAttributes"/> attribute.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/>.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="attribute">The <see cref="FileAttributes"/> attribute to check.</param>
        /// <returns><c>true</c> if directory has <paramref name="attribute"/>; <c>false</c> otherwise.</returns>
        public static bool HasDirectoryAttribute(this IFileSystem fileSystem, string directoryPath, FileAttributes attribute)
        {
            var attributes = fileSystem.DirectoryOperations.GetAttributes(directoryPath);
            return attributes.HasFlag(attribute);
        }
    }
}
