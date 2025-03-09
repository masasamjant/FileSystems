namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Represents information of an drive.
    /// </summary>
    public interface IDriveInfo
    {
        /// <summary>
        /// Gets the <see cref="IDriveOperations"/> used to obtain drive info.
        /// </summary>
        IDriveOperations DriveOperations { get; }

        /// <summary>
        /// Gets whether or not drive can be accessed.
        /// </summary>
        bool CanAccess { get; }

        /// <summary>
        /// Gets the available free space in bytes.
        /// </summary>
        long AvailableFreeSpace { get; }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        string DriveFormat { get; }

        /// <summary>
        /// Gets the drive type.
        /// </summary>
        DriveType DriveType { get; }

        /// <summary>
        /// Gets whether or not drive is ready.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Gets the name of drive.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the total amount of free space available in bytes.
        /// </summary>
        long TotalFreeSpace { get; }

        /// <summary>
        /// Gets the total size of drive in bytes.
        /// </summary>
        long TotalSize { get; }

        /// <summary>
        /// Gets the volume label.
        /// </summary>
        string VolumeLabel { get; }

        /// <summary>
        /// Gets the <see cref="IDirectoryInfo"/> of root directory.
        /// </summary>
        IDirectoryInfo RootDirectory { get; }
    }
}
