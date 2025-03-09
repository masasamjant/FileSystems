using Masasamjant.FileSystems.Abstractions;
using Masasamjant.FileSystems.Adapters;
using System.Diagnostics.CodeAnalysis;

namespace Masasamjant.FileSystems
{
    /// <summary>
    /// Provides file system operations performed to files.
    /// </summary>
    public sealed class FileOperations : FileSystemOperations, IFileOperations
    {
        /// <summary>
        /// Initializes new instance of the <see cref="FileOperations"/> class.
        /// </summary>
        /// <param name="fileSystem">The target <see cref="IFileSystem"/>.</param>
        public FileOperations(IFileSystem fileSystem)
            : base(fileSystem) 
        { }

        /// <summary>
        /// Copy source file to destination file.
        /// </summary>
        /// <param name="sourceFilePath">The path to source file.</param>
        /// <param name="destinationFilePath">The path to destination file.</param>
        /// <param name="overwrite"><c>true</c> to overwrite destination file if exists; <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="sourceFilePath"/> or <paramref name="destinationFilePath"/> is empty or white-space.</exception>
        /// <exception cref="ArgumentException">If <paramref name="sourceFilePath"/> and <paramref name="destinationFilePath"/> are same.</exception>
        public void Copy(string sourceFilePath, string destinationFilePath, bool overwrite = false)
        {
            CopyOrMove(true, sourceFilePath, destinationFilePath, overwrite);
        }

        /// <summary>
        /// Delete file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        public void Delete(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            File.Delete(filePath);
        }

        /// <summary>
        /// Check if file specified by path exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if file exists; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        public bool Exists(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            return File.Exists(filePath);
        }

        /// <summary>
        /// Gets <see cref="FileAttributes"/> of file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A <see cref="FileAttributes"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        public FileAttributes GetAttributes(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "The file path cannot be empty or only white-space.");

            return File.GetAttributes(filePath);
        }

        /// <summary>
        /// Gets <see cref="IFileInfo"/> of file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A <see cref="IFileInfo"/> of file specified by path.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or white-space.</exception>
        public IFileInfo GetFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "The file path cannot be empty or only white-space.");

            var file = new FileInfo(filePath);
            return new FileInfoAdapter(file, this);
        }

        /// <summary>
        /// Move source file to destination file.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="overwrite"><c>true</c> to overwrite destination file if exists; <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="sourceFilePath"/> or <paramref name="destinationFilePath"/> is empty or only white-space.</exception>
        /// <exception cref="ArgumentException">If <paramref name="sourceFilePath"/> and <paramref name="destinationFilePath"/> are same.</exception>
        public void Move(string sourceFilePath, string destinationFilePath, bool overwrite = false)
        {
            CopyOrMove(false, sourceFilePath, destinationFilePath, overwrite);
        }

        /// <summary>
        /// Set <see cref="FileAttributes"/> of file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="attributes">The <see cref="FileAttributes"/> to set.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="filePath"/> is empty or only white-space.</exception>
        public void SetAttributes(string filePath, FileAttributes attributes)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            File.SetAttributes(filePath, attributes);
        }

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
        public Stream GetStream(string filePath, FileMode fileMode, FileAccess fileAccess)
        {
            return GetFileStream(filePath, fileMode, fileAccess, null);
        }

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
        public Stream GetStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return GetFileStream(filePath, fileMode, fileAccess, fileShare);
        }

        /// <summary>
        /// Try get stream to access file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileMode">The <see cref="FileMode"/>.</param>
        /// <param name="fileAccess">The <see cref="FileAccess"/>.</param>
        /// <param name="stream">The <see cref="Stream"/> if return <c>true</c>; <c>null</c> otherwise.</param>
        /// <returns><c>true</c> if stream is get; <c>false</c> otherwise.</returns>
        public bool TryGetStream(string filePath, FileMode fileMode, FileAccess fileAccess, [MaybeNullWhen(false)] out Stream stream)
        {
            try
            {
                stream = GetFileStream(filePath, fileMode, fileAccess, null);
                return stream != null;
            }
            catch (Exception)
            {
                stream = null;
                return false;
            }
        }

        /// <summary>
        /// Try get stream to access file specified by path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileMode">The <see cref="FileMode"/>.</param>
        /// <param name="fileAccess">The <see cref="FileAccess"/>.</param>
        /// <param name="fileShare">The <see cref="FileShare"/>.</param>
        /// <param name="stream">The <see cref="Stream"/> if return <c>true</c>; <c>null</c> otherwise.</param>
        /// <returns><c>true</c> if stream is get; <c>false</c> otherwise.</returns>
        public bool TryGetStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, [MaybeNullWhen(false)] out Stream stream)
        {
            try
            {
                stream = GetFileStream(filePath, fileMode, fileAccess, fileShare);
                return stream != null;
            }
            catch (Exception)
            {
                stream = null;
                return false;
            }
        }

        private static void CopyOrMove(bool copy, string sourceFilePath, string destinationFilePath, bool overwrite)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentNullException(nameof(sourceFilePath), "The source file path cannot be empty or only white-space.");

            if (string.IsNullOrWhiteSpace(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath), "The destination file path cannot be empty or only white-space.");

            if (string.Equals(sourceFilePath, destinationFilePath, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("The destination file path cannot be same as source file path.", nameof(destinationFilePath));

            if (copy)
                File.Copy(sourceFilePath, destinationFilePath, overwrite);
            else
                File.Move(sourceFilePath, destinationFilePath, overwrite);
        }

        private static FileStream GetFileStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare? fileShare)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "The file path cannot be empty or only white-space.");

            if (!Enum.IsDefined(fileMode))
                throw new ArgumentException("The value is not defined.", nameof(fileMode));

            if (!Enum.IsDefined(fileAccess))
                throw new ArgumentException("The value is not defined.", nameof(fileAccess));

            if (fileShare.HasValue && !Enum.IsDefined(fileShare.Value))
                throw new ArgumentException("The value is not defined.", nameof(fileShare));

            if (fileShare.HasValue)
                return new FileStream(filePath, fileMode, fileAccess, fileShare.Value);
            else
                return new FileStream(filePath, fileMode, fileAccess);
        }
    }
}
