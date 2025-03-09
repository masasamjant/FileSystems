namespace Masasamjant.FileSystems.Abstractions
{
    /// <summary>
    /// Provides file system operations performed to drives.
    /// </summary>
    public interface IDriveOperations : IFileSystemOperations
    {
        /// <summary>
        /// Gets current drives.
        /// </summary>
        /// <returns>A current drives.</returns>
        IEnumerable<IDriveInfo> GetDrives();

        /// <summary>
        /// Gets drives of specified <see cref="DriveType"/>.
        /// </summary>
        /// <param name="driveType">The <see cref="DriveType"/>.</param>
        /// <returns>A specified drives.</returns>
        /// <remarks>If <paramref name="driveType"/> is not defined, then gets fixed drives.</remarks>
        IEnumerable<IDriveInfo> GetDrives(DriveType driveType);

        /// <summary>
        /// Gets current drives that are ready
        /// </summary>
        /// <returns>A current ready drives.</returns>
        IEnumerable<IDriveInfo> GetReadyDrives();

        /// <summary>
        /// Gets current drives that are ready
        /// </summary>
        /// <param name="driveType">The <see cref="DriveType"/>.</param>
        /// <returns>A current ready drives.</returns>
        /// <remarks>If <paramref name="driveType"/> is not defined, then gets fixed drives.</remarks>
        IEnumerable<IDriveInfo> GetReadyDrives(DriveType driveType);
    }
}
