namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Represents information of an directory.
    /// </summary>
    public interface IDirectoryInfo
    {
        /// <summary>
        /// Gets the <see cref="IDirectoryOperations"/> used to obtain directory info.
        /// </summary>
        IDirectoryOperations DirectoryOperations { get; }

        /// <summary>
        /// Gets the name of directory.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the full name of directory.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets whether or not directory exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets the <see cref="FileAttributes"/> of directory.
        /// </summary>
        FileAttributes Attributes { get; }

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
        /// Gets the <see cref="IDirectoryInfo"/> of parent directory.
        /// </summary>
        IDirectoryInfo? Parent { get; }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of root directory.
        /// </summary>
        IDirectoryInfo Root { get; }

        /// <summary>
        /// Gets subdirectories of current directory.
        /// </summary>
        /// <returns>A subdirectories.</returns>
        IEnumerable<IDirectoryInfo> GetDirectories();

        /// <summary>
        /// Gets subdirectories of current directory that match specified search pattern.
        /// </summary>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <returns>A subdirectories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<IDirectoryInfo> GetDirectories(string searchPattern);

        /// <summary>
        /// Gets subdirectories of current directory that match specified search pattern and option.
        /// </summary>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search option.</param>
        /// <returns>A subdirectories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<IDirectoryInfo> GetDirectories(string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Gets subdirectories of current directory that match specified search pattern and option.
        /// </summary>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search option.</param>
        /// <returns>A subdirectories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<IDirectoryInfo> GetDirectories(string searchPattern, EnumerationOptions searchOption);

        /// <summary>
        /// Gets files of current directory.
        /// </summary>
        /// <returns>A files.</returns>
        IEnumerable<IFileInfo> GetFiles();

        /// <summary>
        /// Gets files of current directory that match specified search pattern.
        /// </summary>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <returns>A files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<IFileInfo> GetFiles(string searchPattern);

        /// <summary>
        /// Gets files of current directory that match specified search pattern or option.
        /// </summary>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<IFileInfo> GetFiles(string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Gets files of current directory that match specified search pattern and option.
        /// </summary>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<IFileInfo> GetFiles(string searchPattern, EnumerationOptions searchOption);

        /// <summary>
        /// Create subdirectory or subdirectories on specified path. The path can be relative to current directory.
        /// </summary>
        /// <param name="path">The specified path.</param>
        /// <returns>A <see cref="IDirectoryInfo"/> of last directory of specified path.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is empty or only white-space.</exception>
        IDirectoryInfo CreateSubDirectory(string path);

        /// <summary>
        /// Set <see cref="FileAttributes"/> of current directory.
        /// </summary>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        void SetAttributes(FileAttributes attributes);
    }
}
