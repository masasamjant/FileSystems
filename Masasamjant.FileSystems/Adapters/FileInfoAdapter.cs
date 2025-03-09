using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Adapters
{
    /// <summary>
    /// Represents <see cref="IFileInfo"/> that adapts <see cref="FileInfo"/> instance.
    /// </summary>
    public sealed class FileInfoAdapter : IFileInfo
    {
        /// <summary>
        /// Initializes new instance of the <see cref="FileInfoAdapter"/> class.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo"/> to adapt.</param>
        /// <param name="fileOperations">The <see cref="IFileOperations"/>.</param>
        internal FileInfoAdapter(FileInfo file, IFileOperations fileOperations)
        {
            FileOperations = fileOperations;
            File = file;
        }

        /// <summary>
        /// Gets the <see cref="IFileOperations"/> used to obtain file info.
        /// </summary>
        public IFileOperations FileOperations { get; }

        /// <summary>
        /// Gets the adapted <see cref="FileInfo"/>.
        /// </summary>
        private FileInfo File { get; }

        /// <summary>
        /// Gets name of the file.
        /// </summary>
        public string Name
        {
            get { return File.Name; }
        }

        /// <summary>
        /// Gets full name and path of the file.
        /// </summary>
        public string FullName
        {
            get { return File.FullName; }
        }

        /// <summary>
        /// Gets whether or not file is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return File.IsReadOnly; }
        }

        /// <summary>
        /// Gets whether or not file exists.
        /// </summary>
        public bool Exists
        {
            get { return File.Exists; }
        }

        /// <summary>
        /// Gets directory name of the file.
        /// </summary>
        public string? DirectoryName
        {
            get { return File.DirectoryName; }
        }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of the file directory.
        /// </summary>
        public IDirectoryInfo? Directory
        {
            get
            {
                if (File.Directory == null)
                    return null;

                return new DirectoryInfoAdapter(File.Directory, FileOperations.FileSystem.DirectoryOperations);
            }
        }

        /// <summary>
        /// Gets size of the file in bytes.
        /// </summary>
        public long Length
        {
            get { return File.Length; }
        }

        /// <summary>
        /// Gets local creation time.
        /// </summary>
        public DateTime CreationTime
        {
            get { return File.CreationTime; }
        }

        /// <summary>
        /// Gets UTC creation time.
        /// </summary>
        public DateTime CreationTimeUtc
        {
            get { return File.CreationTimeUtc; }
        }

        /// <summary>
        /// Gets last local write time.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return File.LastWriteTime; }
        }

        /// <summary>
        /// Gets last UTC write time.
        /// </summary>
        public DateTime LastWriteTimeUtc
        {
            get { return File.LastWriteTimeUtc; }
        }

        /// <summary>
        /// Gets last local access time.
        /// </summary>
        public DateTime LastAccessTime
        {
            get { return File.LastAccessTime; }
        }

        /// <summary>
        /// Gets last UTC access time.
        /// </summary>
        public DateTime LastAccessTimeUtc
        {
            get { return File.LastAccessTimeUtc; }
        }

        /// <summary>
        /// Gets the <see cref="FileAttributes"/>.
        /// </summary>
        public FileAttributes Attributes
        {
            get { return File.Attributes; }
        }

        /// <summary>
        /// Copy current file to another file possibly overwriting destination file.
        /// </summary>
        /// <param name="destinationFilePath">The path to destination file.</param>
        /// <param name="overwrite"><c>true</c> to overwrite destination file if exists; <c>false</c> otherwise.</param>
        /// <returns>A <see cref="IFileInfo"/> of copy file.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="destinationFilePath"/> is empty string or only white-space.</exception>
        /// <exception cref="ArgumentException">If <paramref name="destinationFilePath"/> is path of current file.</exception>
        public IFileInfo CopyTo(string destinationFilePath, bool overwrite = false)
        {
            if (string.IsNullOrWhiteSpace(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath), "The value cannot be empty or only white-space.");

            if (string.Equals(destinationFilePath, FullName, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("The destination file path cannot be same as current file path.", nameof(destinationFilePath));

            var copy = File.CopyTo(destinationFilePath, overwrite);
            return new FileInfoAdapter(copy, FileOperations);
        }

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
        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return OpenStream(fileMode, fileAccess, fileShare);
        }

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
        public Stream Open(FileMode fileMode, FileAccess fileAccess)
        {
            return OpenStream(fileMode, fileAccess, null);
        }

        /// <summary>
        /// Opens stream to access current file.
        /// </summary>
        /// <param name="mode">The <see cref="FileMode"/>.</param>
        /// <returns>A <see cref="Stream"/> to access file.</returns>
        /// <exception cref="ArgumentException">If value of <paramref name="mode"/> is not defined in <see cref="FileMode"/>.</exception>
        public Stream Open(FileMode fileMode)
        {
            return OpenStream(fileMode, null, null);
        }

        /// <summary>
        /// Set <see cref="FileAttributes"/> of current file.
        /// </summary>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        public void SetAttributes(FileAttributes attributes)
        {
            File.Attributes = attributes;
        }

        private FileStream OpenStream(FileMode fileMode, FileAccess? fileAccess, FileShare? fileShare)
        {
            if (!Enum.IsDefined(fileMode))
                throw new ArgumentException("The value is not defined.", nameof(fileMode));

            if (fileAccess.HasValue && !Enum.IsDefined(fileAccess.Value))
                throw new ArgumentException("The value is not defined.", nameof(fileAccess));

            if (fileShare.HasValue && !Enum.IsDefined(fileShare.Value))
                throw new ArgumentException("The value is not defined.", nameof(fileShare));

            if (fileAccess.HasValue && fileShare.HasValue)
                return File.Open(fileMode, fileAccess.Value, fileShare.Value);
            else if (fileAccess.HasValue)
                return File.Open(fileMode, fileAccess.Value);
            else
                return File.Open(fileMode);
        }
    }
}
