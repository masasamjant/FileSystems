using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Adapters
{
    /// <summary>
    /// Represents <see cref="IDriveInfo"/> that adapts <see cref="DriveInfo"/> instance.
    /// </summary>
    public sealed class DriveInfoAdapter : IDriveInfo
    {
        private bool? canAccess;

        /// <summary>
        /// Initializes new instance of the <see cref="DriveInfoAdapter"/> class.
        /// </summary>
        /// <param name="drive">The <see cref="DriveInfo"/> to adapt.</param>
        /// <param name="driveOperations">The <see cref="IDriveOperations"/>.</param>
        internal DriveInfoAdapter(DriveInfo drive, IDriveOperations driveOperations)
        {
            Drive = drive;
            DriveOperations = driveOperations;
        }

        /// <summary>
        /// Gets the <see cref="IDriveOperations"/> used to obtain drive info.
        /// </summary>
        public IDriveOperations DriveOperations { get; }

        /// <summary>
        /// Gets the adapted <see cref="DriveInfo"/>.
        /// </summary>
        private DriveInfo Drive { get; }

        /// <summary>
        /// Gets whether or not drive can be accessed.
        /// </summary>
        public bool CanAccess
        {
            get
            {
                if (!canAccess.HasValue)
                {
                    try
                    {
                        long tmp = Drive.AvailableFreeSpace;
                        canAccess = true;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        canAccess = false;
                    }
                }

                return canAccess.Value;
            }
        }

        /// <summary>
        /// Gets the available free space in bytes.
        /// </summary>
        public long AvailableFreeSpace
        {
            get { return Drive.AvailableFreeSpace; }
        }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string DriveFormat
        {
            get { return Drive.DriveFormat; }
        }

        /// <summary>
        /// Gets the drive type.
        /// </summary>
        public DriveType DriveType
        {
            get { return Drive.DriveType; }
        }

        /// <summary>
        /// Gets whether or not drive is ready.
        /// </summary>
        public bool IsReady
        {
            get { return Drive.IsReady; }
        }

        /// <summary>
        /// Gets the name of drive.
        /// </summary>
        public string Name
        {
            get { return Drive.Name; }
        }

        /// <summary>
        /// Gets the total amount of free space available in bytes.
        /// </summary>
        public long TotalFreeSpace
        {
            get { return Drive.TotalFreeSpace; }
        }

        /// <summary>
        /// Gets the total size of drive in bytes.
        /// </summary>
        public long TotalSize
        {
            get { return Drive.TotalSize; }
        }

        /// <summary>
        /// Gets the volume label.
        /// </summary>
        public string VolumeLabel
        {
            get { return Drive.VolumeLabel; }
        }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of root directory.
        /// </summary>
        public IDirectoryInfo RootDirectory
        {
            get { return new DirectoryInfoAdapter(Drive.RootDirectory, DriveOperations.FileSystem.DirectoryOperations); }
        }
    }
}
