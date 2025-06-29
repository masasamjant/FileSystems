using Masasamjant.FileSystems.Abstractions;
using Masasamjant.FileSystems.Adapters;

namespace Masasamjant.FileSystems
{
    /// <summary>
    /// Provides file system operations performed to drives.
    /// </summary>
    public sealed class DriveOperations : FileSystemOperations, IDriveOperations
    {
        /// <summary>
        /// Initializes new instance of the <see cref="DriveOperations"/> class.
        /// </summary>
        /// <param name="fileSystem">The target <see cref="IFileSystem"/>.</param>
        public DriveOperations(IFileSystem fileSystem)
            : base(fileSystem) 
        { }

        /// <summary>
        /// Gets drive specified by name.
        /// </summary>
        /// <param name="name">The drive name.</param>
        /// <returns>A <see cref="IDriveInfo"/> or <c>null</c>.</returns>
        public IDriveInfo? GetDrive(string name)
        {
            return GetDrives().FirstOrDefault(s => s.Name == name);
        }

        /// <summary>
        /// Gets current drives.
        /// </summary>
        /// <returns>A current drives.</returns>
        public IEnumerable<IDriveInfo> GetDrives()
        {
            return GetDrivesImplementation(null, false);
        }

        /// <summary>
        /// Gets drives of specified <see cref="DriveType"/>.
        /// </summary>
        /// <param name="driveType">The <see cref="DriveType"/>.</param>
        /// <returns>A specified drives.</returns>
        /// <remarks>If <paramref name="driveType"/> is not defined, then gets fixed drives.</remarks>
        public IEnumerable<IDriveInfo> GetDrives(DriveType driveType)
        {
            return GetDrivesImplementation(Enum.IsDefined(driveType) ? driveType : DriveType.Fixed, false);
        }

        /// <summary>
        /// Gets current drives that are ready
        /// </summary>
        /// <returns>A current ready drives.</returns>
        public IEnumerable<IDriveInfo> GetReadyDrives()
        {
            return GetDrivesImplementation(null, true);
        }

        /// <summary>
        /// Gets current drives that are ready
        /// </summary>
        /// <param name="driveType">The <see cref="DriveType"/>.</param>
        /// <returns>A current ready drives.</returns>
        /// <remarks>If <paramref name="driveType"/> is not defined, then gets fixed drives.</remarks>
        public IEnumerable<IDriveInfo> GetReadyDrives(DriveType driveType)
        {
            return GetDrivesImplementation(Enum.IsDefined(driveType) ? driveType : DriveType.Fixed, true);
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{IDriveInfo}"/> using specified arguments.
        /// </summary>
        /// <param name="driveType">The <see cref="DriveType"/> or <c>null</c> to get all drive types.</param>
        /// <param name="onlyReady"><c>true</c> to get only ready drives; <c>false</c> otherwise.</param>
        /// <returns>A <see cref="IEnumerable{IDriveInfo}"/> of drives.</returns>
        private IEnumerable<IDriveInfo> GetDrivesImplementation(DriveType? driveType, bool onlyReady)
        {
            var drives = DriveInfo.GetDrives().AsEnumerable();

            if (driveType.HasValue)
                drives = drives.Where(d => d.DriveType == driveType.Value);

            if (onlyReady)
                drives = drives.Where(d => d.IsReady);

            return drives.Select(drive => new DriveInfoAdapter(drive, this));
        }
    }
}
