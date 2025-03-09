using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Adapters
{
    /// <summary>
    /// Represents <see cref="IDirectoryInfo"/> that adapts <see cref="DirectoryInfo"/> instance.
    /// </summary>
    public sealed class DirectoryInfoAdapter : IDirectoryInfo
    {
        /// <summary>
        /// Initializes new instance of the <see cref="DirectoryInfoAdapter"/> class.
        /// </summary>
        /// <param name="directory">The <see cref="DirectoryInfo"/> to adapt.</param>
        /// <param name="directoryOperations">The <see cref="IDirectoryOperations"/>.</param>
        internal DirectoryInfoAdapter(DirectoryInfo directory, IDirectoryOperations directoryOperations)
        {
            Directory = directory;
            DirectoryOperations = directoryOperations;
        }

        /// <summary>
        /// Gets the <see cref="IDirectoryOperations"/> used to obtain directory info.
        /// </summary>
        public IDirectoryOperations DirectoryOperations { get; }

        /// <summary>
        /// Gets the adapted <see cref="DirectoryInfo"/>.
        /// </summary>
        private DirectoryInfo Directory { get; }

        /// <summary>
        /// Gets the name of directory.
        /// </summary>
        public string Name
        {
            get { return Directory.Name; }
        }

        /// <summary>
        /// Gets the full name of directory.
        /// </summary>
        public string FullName
        {
            get { return Directory.FullName; }
        }

        /// <summary>
        /// Gets whether or not directory exists.
        /// </summary>
        public bool Exists
        {
            get { return Directory.Exists; }
        }

        /// <summary>
        /// Gets the <see cref="FileAttributes"/> of directory.
        /// </summary>
        public FileAttributes Attributes
        {
            get { return Directory.Attributes; }
        }

        /// <summary>
        /// Gets local creation time.
        /// </summary>
        public DateTime CreationTime
        {
            get { return Directory.CreationTime; }
        }

        /// <summary>
        /// Gets UTC creation time.
        /// </summary>
        public DateTime CreationTimeUtc
        {
            get { return Directory.CreationTimeUtc; }
        }

        /// <summary>
        /// Gets last local write time.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return Directory.LastWriteTime; }
        }

        /// <summary>
        /// Gets last UTC write time.
        /// </summary>
        public DateTime LastWriteTimeUtc
        {
            get { return Directory.LastWriteTimeUtc; }
        }

        /// <summary>
        /// Gets last local access time.
        /// </summary>
        public DateTime LastAccessTime
        {
            get { return Directory.LastAccessTime; }
        }

        /// <summary>
        /// Gets last UTC access time.
        /// </summary>
        public DateTime LastAccessTimeUtc
        {
            get { return Directory.LastAccessTimeUtc; }
        }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of parent directory.
        /// </summary>
        public IDirectoryInfo? Parent
        {
            get
            {
                if (Directory.Parent == null)
                    return null;

                return new DirectoryInfoAdapter(Directory.Parent, DirectoryOperations);
            }
        }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of root directory.
        /// </summary>
        public IDirectoryInfo Root
        {
            get { return new DirectoryInfoAdapter(Directory.Root, DirectoryOperations); }
        }

        /// <summary>
        /// Create subdirectory or subdirectories on specified path. The path can be relative to current directory.
        /// </summary>
        /// <param name="path">The specified path.</param>
        /// <returns>A <see cref="IDirectoryInfo"/> of last directory of specified path.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is empty or only white-space.</exception>
        public IDirectoryInfo CreateSubDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path), "The directory path cannot be empty or only white-space.");

            return new DirectoryInfoAdapter(Directory.CreateSubdirectory(path), DirectoryOperations);
        }

        /// <summary>
        /// Gets subdirectories of current directory.
        /// </summary>
        /// <returns>A subdirectories.</returns>
        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            return GetDirectoriesImplementation(null, null, null);
        }

        /// <summary>
        /// Gets subdirectories of current directory that match specified search pattern.
        /// </summary>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <returns>A subdirectories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<IDirectoryInfo> GetDirectories(string searchPattern)
        {
            ValidateSearchPattern(searchPattern);
            return GetDirectoriesImplementation(searchPattern, null, null);
        }

        /// <summary>
        /// Gets subdirectories of current directory that match specified search pattern and option.
        /// </summary>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search option.</param>
        /// <returns>A subdirectories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<IDirectoryInfo> GetDirectories(string searchPattern, SearchOption searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetDirectoriesImplementation(searchPattern, Enum.IsDefined(searchOption) ? searchOption : SearchOption.TopDirectoryOnly, null);
        }

        /// <summary>
        /// Gets subdirectories of current directory that match specified search pattern and option.
        /// </summary>
        /// <param name="searchPattern">The directory search pattern.</param>
        /// <param name="searchOption">The directory search option.</param>
        /// <returns>A subdirectories.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<IDirectoryInfo> GetDirectories(string searchPattern, EnumerationOptions searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetDirectoriesImplementation(searchPattern, null, searchOption);
        }

        /// <summary>
        /// Gets files of current directory.
        /// </summary>
        /// <returns>A files.</returns>
        public IEnumerable<IFileInfo> GetFiles()
        {
            return GetFilesImplementation(null, null, null);
        }

        /// <summary>
        /// Gets files of current directory that match specified search pattern.
        /// </summary>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <returns>A files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<IFileInfo> GetFiles(string searchPattern)
        {
            ValidateSearchPattern(searchPattern);
            return GetFilesImplementation(searchPattern, null, null);
        }

        /// <summary>
        /// Gets files of current directory that match specified search pattern or option.
        /// </summary>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<IFileInfo> GetFiles(string searchPattern, SearchOption searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetFilesImplementation(searchPattern, Enum.IsDefined(searchOption) ? searchOption : SearchOption.TopDirectoryOnly, null);
        }

        /// <summary>
        /// Gets files of current directory that match specified search pattern and option.
        /// </summary>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <param name="searchOption">The file search option.</param>
        /// <returns>A files.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchPattern"/> is empty or only white-space.</exception>
        public IEnumerable<IFileInfo> GetFiles(string searchPattern, EnumerationOptions searchOption)
        {
            ValidateSearchPattern(searchPattern);
            return GetFilesImplementation(searchPattern, null, searchOption);
        }

        /// <summary>
        /// Set <see cref="FileAttributes"/> of current directory.
        /// </summary>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        public void SetAttributes(FileAttributes attributes)
        {
            Directory.Attributes = attributes;
        }

        private IEnumerable<IDirectoryInfo> GetDirectoriesImplementation(string? searchPattern, SearchOption? searchOption, EnumerationOptions? enumerationOptions)
        {
            DirectoryInfo[] directories;

            if (searchPattern != null && searchOption.HasValue)
                directories = Directory.GetDirectories(searchPattern, searchOption.Value);
            else if (searchPattern != null && enumerationOptions != null)
                directories = Directory.GetDirectories(searchPattern, enumerationOptions);
            else if (searchPattern != null)
                directories = Directory.GetDirectories(searchPattern);
            else
                directories = Directory.GetDirectories();

            return directories.Select(directory => new DirectoryInfoAdapter(directory, DirectoryOperations));
        }

        private IEnumerable<IFileInfo> GetFilesImplementation(string? searchPattern, SearchOption? searchOption, EnumerationOptions? enumerationOptions)
        {
            FileInfo[] files;

            if (searchPattern != null && searchOption.HasValue)
                files = Directory.GetFiles(searchPattern, searchOption.Value);
            else if (searchPattern != null && enumerationOptions != null)
                files = Directory.GetFiles(searchPattern, enumerationOptions);
            else if (searchPattern != null)
                files = Directory.GetFiles(searchPattern);
            else
                files = Directory.GetFiles();

            return files.Select(file => new FileInfoAdapter(file, DirectoryOperations.FileSystem.FileOperations));
        }

        private static void ValidateSearchPattern(string searchPattern)
        {
            if (string.IsNullOrWhiteSpace(searchPattern))
                throw new ArgumentNullException(nameof(searchPattern), "The search pattern cannot be empty or only white-space.");
        }
    }
}
