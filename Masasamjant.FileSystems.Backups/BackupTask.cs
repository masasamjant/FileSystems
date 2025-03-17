using Masasamjant.FileSystems.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
