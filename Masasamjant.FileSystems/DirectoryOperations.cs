using Masasamjant.FileSystems.Abstractions;
using Masasamjant.FileSystems.Adapters;

namespace Masasamjant.FileSystems
{
    /// <summary>
    /// Provides file system operations performed to directories.
    /// </summary>
    public sealed class DirectoryOperations : FileSystemOperations, IDirectoryOperations
    {
        /// <summary>
        /// Initializes new instance of the <see cref="DirectoryOperations"/> class.
        /// </summary>
        /// <param name="fileSystem">The target <see cref="IFileSystem"/>.</param>
        public DirectoryOperations(IFileSystem fileSystem)
            : base(fileSystem) 
        { }

        /// <summary>
        /// Create directory with specified path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A <see cref="IDirectoryInfo"/> of created directory.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        public IDirectoryInfo CreateDirectory(string directoryPath)
        {
            ValidateDirectoryPath(directoryPath);
            return new DirectoryInfoAdapter(Directory.CreateDirectory(directoryPath), this);
        }

        /// <summary>
        /// Delete directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="recursive"><c>true</c> to perform recursive delete; <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        public void Delete(string directoryPath, bool recursive = false)
        {
            ValidateDirectoryPath(directoryPath);
            Directory.Delete(directoryPath, recursive);
        }

        /// <summary>
        /// Check if directory specified by path exists.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns><c>true</c> if directory exists; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        public bool Exists(string directoryPath)
        {
            ValidateDirectoryPath(directoryPath);
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// Get sub directories of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A paths of sub directories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetDirectories(string directoryPath)
        {
            return GetDirectoriesImplementation(directoryPath, null, null, null);
        }

        /// <summary>
        /// Get sub directories of directory specified by path that match specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <returns>A paths of sub directories that match search pattern.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetDirectories(string directoryPath, string searchPattern)
        {
            ValidateSearchPattern(searchPattern);
            return GetDirectoriesImplementation(directoryPath, searchPattern, null, null);
        }

        /// <summary>
        /// Get sub directories of directory specified by path that match specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search option.</param>
        /// <returns>A paths of sub directories that match search pattern.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetDirectories(string directoryPath, string searchPattern, SearchOption searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetDirectoriesImplementation(directoryPath, searchPattern, Enum.IsDefined(searchOption) ? searchOption : SearchOption.TopDirectoryOnly, null);
        }

        /// <summary>
        /// Get sub directories of directory specified by path that match specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search options.</param>
        /// <returns>A paths of sub directories that match search pattern.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/>, <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetDirectories(string directoryPath, string searchPattern, EnumerationOptions searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetDirectoriesImplementation(directoryPath, searchPattern, null, searchOption);
        }

        /// <summary>
        /// Gets <see cref="IDirectoryInfo"/> of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A <see cref="IDirectoryInfo"/> of directory.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        public IDirectoryInfo GetDirectory(string directoryPath)
        {
            ValidateDirectoryPath(directoryPath);
            var directory = new DirectoryInfo(directoryPath);
            return new DirectoryInfoAdapter(directory, this);
        }

        /// <summary>
        /// Get files of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetFiles(string directoryPath)
        {
            return GetFilesImplementation(directoryPath, null, null, null);
        }

        /// <summary>
        /// Get files of directory specified by path using specified search pattern.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetFiles(string directoryPath, string searchPattern)
        {
            ValidateSearchPattern(searchPattern);
            return GetFilesImplementation(directoryPath, searchPattern, null, null);
        }

        /// <summary>
        /// Get files of directory specified by path using specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetFiles(string directoryPath, string searchPattern, SearchOption searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetFilesImplementation(directoryPath, searchPattern, Enum.IsDefined(searchOption) ? searchOption : SearchOption.TopDirectoryOnly, null);
        }

        /// <summary>
        /// Get files of directory specified by path using specified search pattern and option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A paths of files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="directoryPath"/> or <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<string> GetFiles(string directoryPath, string searchPattern, EnumerationOptions searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetFilesImplementation(directoryPath, searchPattern, null, searchOption);
        }

        /// <summary>
        /// Gets <see cref="IDirectoryInfo"/> of current user temp directory.
        /// </summary>
        /// <returns>A <see cref="IDirectoryInfo"/> of current user temp directory.</returns>
        public IDirectoryInfo GetTempDirectory()
        {
            var tempDirectoryPath = Path.GetTempPath();
            var directory = new DirectoryInfo(tempDirectoryPath);
            return new DirectoryInfoAdapter(directory, this);
        }

        /// <summary>
        /// Gets <see cref="FileAttributes"/> of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A <see cref="FileAttributes"/>.</returns>
        public FileAttributes GetAttributes(string directoryPath)
        {
            var directory = GetDirectory(directoryPath);
            return directory.Attributes;
        }

        /// <summary>
        /// Set <see cref="FileAttributes"/> of directory specified by path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        public void SetAttributes(string directoryPath, FileAttributes attributes)
        {
            var directory = GetDirectory(directoryPath);
            directory.SetAttributes(attributes);
        }

        /// <summary>
        /// Gets paths of sub directories.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The search pattern or <c>null</c>.</param>
        /// <param name="searchOption">The <see cref="SearchOption"/> or <c>null</c>.</param>
        /// <param name="enumerationOptions">The <see cref="EnumerationOptions"/> or <c>null.</c></param>
        /// <returns>A paths of sub directories.</returns>
        /// <remarks>If both <paramref name="searchOption"/> and <paramref name="enumerationOptions"/> is set, then <paramref name="searchOption"/> is used.</remarks>
        private static string[] GetDirectoriesImplementation(string directoryPath, string? searchPattern, SearchOption? searchOption, EnumerationOptions? enumerationOptions)
        {
            ValidateDirectoryPath(directoryPath);

            if (searchPattern != null && searchOption.HasValue)
                return Directory.GetDirectories(directoryPath, searchPattern, searchOption.Value);
            else if (searchPattern != null && enumerationOptions != null)
                return Directory.GetDirectories(directoryPath, searchPattern, enumerationOptions);
            else if (searchPattern != null)
                return Directory.GetDirectories(directoryPath, searchPattern);
            else
                return Directory.GetDirectories(directoryPath);
        }

        /// <summary>
        /// Gets paths of files.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The search pattern or <c>null</c>.</param>
        /// <param name="searchOption">The <see cref="SearchOption"/> or <c>null</c>.</param>
        /// <param name="enumerationOptions">The <see cref="EnumerationOptions"/> or <c>null.</c></param>
        /// <returns>A paths of files.</returns>
        /// <remarks>If both <paramref name="searchOption"/> and <paramref name="enumerationOptions"/> is set, then <paramref name="searchOption"/> is used.</remarks>
        private static string[] GetFilesImplementation(string directoryPath, string? searchPattern, SearchOption? searchOption, EnumerationOptions? enumerationOptions)
        {
            ValidateDirectoryPath(directoryPath);

            if (searchPattern != null && searchOption.HasValue)
                return Directory.GetFiles(directoryPath, searchPattern, searchOption.Value);
            else if (searchPattern != null && enumerationOptions != null)
                return Directory.GetFiles(directoryPath, searchPattern, enumerationOptions);
            else if (searchPattern != null)
                return Directory.GetFiles(directoryPath, searchPattern);
            else
                return Directory.GetFiles(directoryPath);
        }

        private static void ValidateDirectoryPath(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentNullException(nameof(directoryPath), "The directory path cannot be empty or only white-space.");
        }

        private static void ValidateSearchPattern(string searchPattern)
        {
            if (string.IsNullOrWhiteSpace(searchPattern))
                throw new ArgumentNullException(nameof(searchPattern), "The search pattern cannot be empty or only white-space.");
        }
    }
}
