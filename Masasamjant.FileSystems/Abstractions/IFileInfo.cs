namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Represents information of an file.
    /// </summary>
    public interface IFileInfo
    {
        /// <summary>
        /// Gets the <see cref="IFileOperations"/> used to obtain file info.
        /// </summary>
        IFileOperations FileOperations { get; }

        /// <summary>
        /// Gets name of the file.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets full name and path of the file.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets whether or not file is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets whether or not file exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets directory name of the file.
        /// </summary>
        string? DirectoryName { get; }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of the file directory.
        /// </summary>
        IDirectoryInfo? Directory { get; }

        /// <summary>
        /// Gets size of the file in bytes.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets local creation time.
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Gets UTC creation time.
        /// </summary>
        DateTime CreationTimeUtc { get; }

        /// <summary>
        /// Gets last local write time.
        /// </summary>
        DateTime LastWriteTime { get; }

        /// <summary>
        /// Gets last UTC write time.
        /// </summary>
        DateTime LastWriteTimeUtc { get; }

        /// <summary>
        /// Gets last local access time.
        /// </summary>
        DateTime LastAccessTime { get; }

        /// <summary>
        /// Gets last UTC access time.
        /// </summary>
        DateTime LastAccessTimeUtc { get; }

        /// <summary>
        /// Gets the <see cref="FileAttributes"/>.
        /// </summary>
        FileAttributes Attributes { get; }

        /// <summary>
        /// Copy current file to another file possibly overwriting destination file.
        /// </summary>
        /// <param name="destinationFilePath">The path to destination file.</param>
        /// <param name="overwrite"><c>true</c> to overwrite destination file if exists; <c>false</c> otherwise.</param>
        /// <returns>A <see cref="IFileInfo"/> of copy file.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="destinationFilePath"/> is empty string or only white-space.</exception>
        /// <exception cref="ArgumentException">If <paramref name="destinationFilePath"/> is path of current file.</exception>
        IFileInfo CopyTo(string destinationFilePath, bool overwrite = false);

        /// <summary>
        /// Opens stream to access current file.
        /// </summary>
        /// <param name="mode">The <see cref="FileMode"/>.</param>
        /// <param name="access">The <see cref="FileAccess"/>.</param>
        /// <param name="share">The <see cref="FileShare"/>.</param>
        /// <returns>A <see cref="Stream"/> to access file.</returns>
        /// <exception cref="ArgumentException">
        /// If value of <paramref name="mode"/> is not defined in <see cref="FileMode"/>.
        /// -or-
        /// If value of <paramref name="access"/> is not defined in <see cref="FileAccess"/>.
        /// -or-
        /// If value of <paramref name="share"/> is not defined in <see cref="FileShare"/>.
        /// </exception>
        Stream Open(FileMode mode, FileAccess access, FileShare share);

        /// <summary>
        /// Opens stream to access current file.
        /// </summary>
        /// <param name="mode">The <see cref="FileMode"/>.</param>
        /// <param name="access">The <see cref="FileAccess"/>.</param>
        /// <returns>A <see cref="Stream"/> to access file.</returns>
        /// <exception cref="ArgumentException">
        /// If value of <paramref name="mode"/> is not defined in <see cref="FileMode"/>.
        /// -or-
        /// If value of <paramref name="access"/> is not defined in <see cref="FileAccess"/>.
        /// </exception>
        Stream Open(FileMode mode, FileAccess access);

        /// <summary>
        /// Opens stream to access current file.
        /// </summary>
        /// <param name="mode">The <see cref="FileMode"/>.</param>
        /// <returns>A <see cref="Stream"/> to access file.</returns>
        /// <exception cref="ArgumentException">If value of <paramref name="mode"/> is not defined in <see cref="FileMode"/>.</exception>
        Stream Open(FileMode mode);

        /// <summary>
        /// Set <see cref="FileAttributes"/> of current file.
        /// </summary>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        void SetAttributes(FileAttributes attributes);
    }
}
