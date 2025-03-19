using Masasamjant.FileSystems.Abstractions;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents abstract backup task.
    /// </summary>
    public abstract class BackupTask : IDisposable
    {
        /// <summary>
        /// Format of full backup directory path.
        /// </summary>
        protected const string FullBackupDirectoryPathFormat = "{0}_{1}_F";

        /// <summary>
        /// Format of diffential backup directory path.
        /// </summary>
        protected const string DifferentialBackupDirectoryPathFormat = "{0}_{1}_{2}_D";

        /// <summary>
        /// Format of incremental backup directory path.
        /// </summary>
        protected const string IncrementalBackupDirectoryPathFormat = "{0}_{1}_{2}_I_{3}";

        /// <summary>
        /// Notifies when error occurs in task.
        /// </summary>
        public event EventHandler<BackupTaskErrorEventArgs>? Error;

        /// <summary>
        /// Notifies when state of task has changed.
        /// </summary>
        public event EventHandler<BackupTaskStateChangedEventArgs>? StateChanged;

        /// <summary>
        /// Notifies when backup of file has been done.
        /// </summary>
        public event EventHandler<BackupTaskFileEventArgs>? FileBackup;

        /// <summary>
        /// Initializes new instance of the <see cref="BackupTask"/> class.
        /// </summary>
        /// <param name="properties">The backup properties.</param>
        /// <param name="fileSystem">The file system.</param>
        internal BackupTask(BackupProperties properties, IFileSystem fileSystem)
        {
            Properties = properties;
            FileSystem = fileSystem;
            State = BackupTaskState.Created;
        }

        /// <summary>
        /// Gets the backup properties.
        /// </summary>
        public BackupProperties Properties { get; }

        /// <summary>
        /// Gets the file system.
        /// </summary>
        protected IFileSystem FileSystem { get; }

        /// <summary>
        /// Gets the <see cref="IFileOperations"/> of <see cref="FileSystem"/>.
        /// </summary>
        protected IFileOperations FileOperations => FileSystem.FileOperations;

        /// <summary>
        /// Gets the <see cref="IDirectoryOperations"/> of <see cref="FileSystem"/>.
        /// </summary>
        protected IDirectoryOperations DirectoryOperations => FileSystem.DirectoryOperations;

        /// <summary>
        /// Gets the state of the backup task.
        /// </summary>
        public BackupTaskState State { get; private set; }

        /// <summary>
        /// Gets whether or not current instance is disposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets or sets full path to current directory that is backed up.
        /// </summary>
        protected string? CurrentDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets full path to current file that is backed up.
        /// </summary>
        protected string? CurrentFilePath { get; set; }

        /// <summary>
        /// Gets or sets if task is canceled.
        /// </summary>
        protected bool IsCanceled { get; set; }

        /// <summary>
        /// Run backup task.
        /// </summary>
        /// <returns>A <see cref="BackupTaskResult"/>.</returns>
        /// <exception cref="BackupPropertiesException">
        /// If source directory of <see cref="Properties"/> does not exist.
        /// -or-
        /// If destination directory of <see cref="Properties"/> does not exist.
        /// -or-
        /// If destination directory is sub-directory of the source directory.
        /// -or-
        /// If source directory is sub-directory of the destination directory.
        /// </exception>
        public BackupTaskResult Run()
        {
            ValidateBackupProperties();

            bool error = false;

            BackupTaskResult? result = null;

            try
            {
                CurrentDirectoryPath = null;
                CurrentFilePath = null;

                SetState(BackupTaskState.PreExecuting, true);

                if (!IsCanceled)
                {
                    PreExecute();

                    SetState(BackupTaskState.Executing, true);

                    if (!IsCanceled)
                    {
                        result = Execute();
                        SetState(BackupTaskState.PostExecuting, false);
                        PostExecute();
                    }
                }
            }
            catch (Exception exception)
            {
                var args = new BackupTaskErrorEventArgs(exception, State, CurrentDirectoryPath, CurrentFilePath);
                OnError(args);
                if (args.Handled && args.ErrorBehavior == BackupTaskErrorBehavior.Cancel)
                    IsCanceled = true;
                else
                    error = true;
            }
            finally
            {
                BackupTaskState finalState;

                if (IsCanceled || result == null)
                    result = new BackupTaskResult(Properties.BackupMode, Properties, string.Empty);

                if (error)
                    finalState = BackupTaskState.Failed;
                else if (IsCanceled)
                    finalState = BackupTaskState.Canceled;
                else
                    finalState = BackupTaskState.Completed;

                SetState(finalState, false);
                result.SetFinalState(finalState);
            }

            return result;
        }

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        /// <param name="disposing"><c>true</c> if disposing; <c>false</c> otherwise.</param>
        protected virtual void Dispose(bool disposing) 
        {
            if (!IsDisposed)
                IsDisposed = true;
        }

        /// <summary>
        /// Checks if <see cref="IsDisposed"/> is <c>true</c> and if so then throws <see cref="ObjectDisposedException"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If <see cref="IsDisposed"/> is <c>true</c>.</exception>
        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        /// <summary>
        /// Derived classes must override to perform pre-execution state. This is state where
        /// backup task should prepare itself.
        /// </summary>
        protected abstract void PreExecute();

        /// <summary>
        /// Derived classes must override to perform execution state. This is state where actual
        /// backup should be done.
        /// </summary>
        /// <returns>A <see cref="BackupTaskResult"/>.</returns>
        protected abstract BackupTaskResult Execute();

        /// <summary>
        /// Derived classes must override to perform post-execution state. This is state where
        /// backup task can clean itself.
        /// </summary>
        protected abstract void PostExecute();

        /// <summary>
        /// Raises <see cref="Error"/> event.
        /// </summary>
        /// <param name="args">The <see cref="BackupTaskErrorEventArgs"/>.</param>
        protected virtual void OnError(BackupTaskErrorEventArgs args)
        {
            Error?.Invoke(this, args);
        }

        /// <summary>
        /// Raises <see cref="StateChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="BackupTaskStateChangedEventArgs"/>.</param>
        protected virtual void OnStateChanged(BackupTaskStateChangedEventArgs args)
        {
            StateChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Raises <see cref="FileBackup"/> event.
        /// </summary>
        /// <param name="args">The <see cref="BackupTaskFileEventArgs"/>.</param>
        protected virtual void OnFileBackup(BackupTaskFileEventArgs args)
        {
            FileBackup?.Invoke(this, args);
        }

        /// <summary>
        /// Creates backup from source file specified by <paramref name="sourceFile"/>.
        /// </summary>
        /// <param name="sourceFile">The <see cref="IFileInfo"/> of source file.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="backupDirectoryPath">The backup directory path.</param>
        /// <param name="createDirectory"><c>true</c> to create directory of <paramref name="backupDirectoryPath"/>; <c>false</c> if directory already created.</param>
        protected void CreateBackup(IFileInfo sourceFile, string destinationFilePath, string backupDirectoryPath, ref bool createDirectory)
        {
            EnsureCreateDirectory(backupDirectoryPath, ref createDirectory);
            sourceFile.CopyTo(destinationFilePath, true);
        }

        /// <summary>
        /// Ensure that directory specified by <paramref name="backupDirectoryPath"/> is created if <paramref name="createDirectory"/> is <c>true</c>.
        /// </summary>
        /// <param name="backupDirectoryPath">The backup directory path.</param>
        /// <param name="createDirectory"><c>true</c> to create directory; <c>false</c> if already invoked for this.</param>
        protected void EnsureCreateDirectory(string backupDirectoryPath, ref bool createDirectory)
        {
            if (createDirectory)
            {
                DirectoryOperations.CreateDirectory(backupDirectoryPath);
                createDirectory = false;
            }
        }

        /// <summary>
        /// Handle file backup error.
        /// </summary>
        /// <param name="exception">The occurred exception.</param>
        /// <returns>A <see cref="BackupTaskErrorEventArgs"/>.</returns>
        protected BackupTaskErrorEventArgs HandleBackupFileError(Exception exception)
        {
            var args = new BackupTaskErrorEventArgs(exception, State, CurrentDirectoryPath, CurrentFilePath);
            OnError(args);
            return args;
        }

        /// <summary>
        /// Computes SHA-256 hash value for specified <see cref="IFileInfo"/>.
        /// </summary>
        /// <param name="fileInfo">The <see cref="IFileInfo"/>.</param>
        /// <returns>The SHA-256 for <paramref name="fileInfo"/>.</returns>
        protected static string ComputeFileHash(IFileInfo fileInfo)
            => ComputeFileHash(fileInfo.FullName, fileInfo.CreationTime, fileInfo.LastWriteTime);

        /// <summary>
        /// Gets the timestamp string of current local time.
        /// </summary>
        /// <returns>A timestamp string of current local time.</returns>
        protected static string GetTimestampString()
        {
            DateTime dt = DateTime.Now;

            return string.Concat(new string[]
            {
                dt.Year.ToString(),
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'),
                dt.Hour.ToString().PadLeft(2, '0'),
                dt.Minute.ToString().PadLeft(2, '0'),
                dt.Second.ToString().PadLeft(2, '0')
            });
        }

        internal static string GetFullBackupTimestamp(IDirectoryInfo directory)
        {
            return directory.Name.Split('_')[1];
        }

        internal static void AddHiddenReadOnlyAttribute(string filePath, IFileOperations fileOperations)
        {
            var attributes = fileOperations.GetAttributes(filePath);
            attributes |= FileAttributes.Hidden;
            attributes |= FileAttributes.ReadOnly;
            fileOperations.SetAttributes(filePath, attributes);
        }

        internal static void RemoveHiddenReadOnlyAttribute(string filePath, IFileOperations fileOperations)
        {
            var attributes = fileOperations.GetAttributes(filePath);
            attributes &= ~FileAttributes.Hidden;
            attributes &= ~FileAttributes.ReadOnly;
            fileOperations.SetAttributes(filePath, attributes);
        }

        private static string ComputeFileHash(string filePath, DateTime creationTime, DateTime listWriteTime)
        {
            string value = string.Concat(filePath.ToLowerInvariant(), creationTime.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(), listWriteTime.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
            byte[] buffer = Encoding.Unicode.GetBytes(value);
            byte[] hash = SHA256.HashData(buffer);
            return Convert.ToBase64String(hash);
        }

        private void SetState(BackupTaskState state, bool canCancel)
        {
            if (State != state)
            {
                State = state;
                var args = new BackupTaskStateChangedEventArgs(State, CurrentDirectoryPath, CurrentFilePath, canCancel);
                OnStateChanged(args);
                if (args.CanCancel && args.Cancel)
                    IsCanceled = true;
            }
        }

        private void ValidateBackupProperties()
        {
            var sourceDirectory = DirectoryOperations.GetDirectory(Properties.SourceDirectoryPath);

            if (!sourceDirectory.Exists)
                throw new BackupPropertiesException("The source directory does not exist.", Properties);

            var destinationDirectory = DirectoryOperations.GetDirectory(Properties.DestinationDirectoryPath);

            if (!destinationDirectory.Exists)
                throw new BackupPropertiesException("The destination directory does not exist.", Properties);

            foreach (var childDirectory in sourceDirectory.GetDirectories())
            {
                if (childDirectory.FullName == destinationDirectory.FullName)
                    throw new BackupPropertiesException("The destination directory cannot be sub-directory of the source directory.", Properties);
            }

            foreach (var childDirectory in destinationDirectory.GetDirectories())
            {
                if (childDirectory.FullName == sourceDirectory.FullName)
                    throw new BackupPropertiesException("The source directory cannot be sub-directory of the destination directory.", Properties);
            }
        }
    }
}
