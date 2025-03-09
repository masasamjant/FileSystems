using System.Diagnostics.CodeAnalysis;

namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Provides file system operations performed to files.
    /// </summary>
    public interface IFileOperations : IFileSystemOperations
    {
        /// <summary>
        /// Check if file specified by path exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if file exists; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        bool Exists(string filePath);

        /// <summary>
        /// Delete file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        void Delete(string filePath);

        /// <summary>
        /// Gets <see cref="IFileInfo"/> of file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A <see cref="IFileInfo"/> of file specified by path.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        IFileInfo GetFile(string filePath);

        /// <summary>
        /// Gets <see cref="FileAttributes"/> of file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A <see cref="FileAttributes"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        FileAttributes GetAttributes(string filePath);

        /// <summary>
        /// Set <see cref="FileAttributes"/> of file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or only white-space.</exception>
        void SetAttributes(string filePath, FileAttributes attributes);

        /// <summary>
        /// Copy source file to destination file.
        /// </summary>
        /// <param name="sourceFilePath">The path to source file.</param>
        /// <param name="destinationFilePath">The path to destination file.</param>
        /// <param name="overwrite"><c>true</c> to overwrite destination file if exists; <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="sourceFilePath"/> or <paramref name="destinationFilePath"/> is empty or white-space.</exception>
        /// <exception cref="ArgumentException">If <paramref name="sourceFilePath"/> and <paramref name="destinationFilePath"/> are same.</exception>
        void Copy(string sourceFilePath, string destinationFilePath, bool overwrite = false);

        /// <summary>
        /// Move source file to destination file.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="overwrite"><c>true</c> to overwrite destination file if exists; <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="sourceFilePath"/> or <paramref name="destinationFilePath"/> is empty or only white-space.</exception>
        /// <exception cref="ArgumentException">If <paramref name="sourceFilePath"/> and <paramref name="destinationFilePath"/> are same.</exception>
        void Move(string sourceFilePath, string destinationFilePath, bool overwrite = false);

        /// <summary>
        /// Get stream to acceess file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileMode">The <see cref="FileMode"/>.</param>
        /// <param name="fileAccess">The <see cref="FileAccess"/>.</param>
        /// <returns>A <see cref="Stream"/> to access file.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or only white-space.</exception>
        /// <exception cref="ArgumentException">
        /// If value of <paramref name="fileMode"/> is not defined in <see cref="FileMode"/>.
        /// -or-
        /// If value of <paramref name="fileAccess"/> is not defined in <see cref="FileAccess"/>.
        /// </exception>
        Stream GetStream(string filePath, FileMode fileMode, FileAccess fileAccess);

        /// <summary>
        /// Get stream to acceess file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileMode">The <see cref="FileMode"/>.</param>
        /// <param name="fileAccess">The <see cref="FileAccess"/>.</param>
        /// <param name="fileShare">The <see cref="FileShare"/>.</param>
        /// <returns>A <see cref="Stream"/> to access file.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or only white-space.</exception>
        /// <exception cref="ArgumentException">
        /// If value of <paramref name="fileMode"/> is not defined in <see cref="FileMode"/>.
        /// -or-
        /// If value of <paramref name="fileAccess"/> is not defined in <see cref="FileAccess"/>.
        /// -or-
        /// If value of <paramref name="fileShare"/> is not defined in <see cref="FileShare"/>.
        /// </exception>
        Stream GetStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare);

        /// <summary>
        /// Try get stream to access file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileMode">The <see cref="FileMode"/>.</param>
        /// <param name="fileAccess">The <see cref="FileAccess"/>.</param>
        /// <param name="stream">The <see cref="Stream"/> if return <c>true</c>; <c>null</c> otherwise.</param>
        /// <returns><c>true</c> if stream is get; <c>false</c> otherwise.</returns>
        bool TryGetStream(string filePath, FileMode fileMode, FileAccess fileAccess, [MaybeNullWhen(false)] out Stream stream);

        /// <summary>
        /// Try get stream to access file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileMode">The <see cref="FileMode"/>.</param>
        /// <param name="fileAccess">The <see cref="FileAccess"/>.</param>
        /// <param name="fileShare">The <see cref="FileShare"/>.</param>
        /// <param name="stream">The <see cref="Stream"/> if return <c>true</c>; <c>null</c> otherwise.</param>
        /// <returns><c>true</c> if stream is get; <c>false</c> otherwise.</returns>
        bool TryGetStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, [MaybeNullWhen(false)] out Stream stream);
    }
}
