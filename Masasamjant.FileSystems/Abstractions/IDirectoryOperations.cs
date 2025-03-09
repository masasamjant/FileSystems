namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Provides file system operations performed to directories.
    /// </summary>
    public interface IDirectoryOperations : IFileSystemOperations
    {
        /// <summary>
        /// Check if directory specified by path exists.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns><c>true</c> if directory exists; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        bool Exists(string directoryPath);

        /// <summary>
        /// Delete directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="recursive"><c>true</c> to perform recursive delete; <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        void Delete(string directoryPath, bool recursive = false);

        /// <summary>
        /// Create directory with specified path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A <see cref="IDirectoryInfo"/> of created directory.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        IDirectoryInfo CreateDirectory(string directoryPath);

        /// <summary>
        /// Gets <see cref="IDirectoryInfo"/> of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A <see cref="IDirectoryInfo"/> of directory.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        IDirectoryInfo GetDirectory(string directoryPath);

        /// <summary>
        /// Get sub directories of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A paths of sub directories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        IEnumerable<string> GetDirectories(string directoryPath);

        /// <summary>
        /// Get sub directories of directory specified by path that match specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <returns>A paths of sub directories that match search pattern.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<string> GetDirectories(string directoryPath, string searchPattern);

        /// <summary>
        /// Get sub directories of directory specified by path that match specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search option.</param>
        /// <returns>A paths of sub directories that match search pattern.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<string> GetDirectories(string directoryPath, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Get sub directories of directory specified by path that match specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search options.</param>
        /// <returns>A paths of sub directories that match search pattern.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/>, <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<string> GetDirectories(string directoryPath, string searchPattern, EnumerationOptions searchOption);

        /// <summary>
        /// Get files of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        IEnumerable<string> GetFiles(string directoryPath);

        /// <summary>
        /// Get files of directory specified by path using specified search pattern.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<string> GetFiles(string directoryPath, string searchPattern);

        /// <summary>
        /// Get files of directory specified by path using specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<string> GetFiles(string directoryPath, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Get files of directory specified by path using specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        IEnumerable<string> GetFiles(string directoryPath, string searchPattern, EnumerationOptions searchOption);

        /// <summary>
        /// Gets <see cref="IDirectoryInfo"/> of current user temp directory.
        /// </summary>
        /// <returns>A <see cref="IDirectoryInfo"/> of current user temp directory.</returns>
        IDirectoryInfo GetTempDirectory();

        /// <summary>
        /// Gets <see cref="FileAttributes"/> of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A <see cref="FileAttributes"/>.</returns>
        FileAttributes GetAttributes(string directoryPath);

        /// <summary>
        /// Set <see cref="FileAttributes"/> of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        void SetAttributes(string directoryPath, FileAttributes attributes);
    }
}
