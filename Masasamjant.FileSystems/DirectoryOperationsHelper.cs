using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems
{
    /// <summary>
    /// Provides helper and extension methods to <see cref="IDirectoryOperations"/> interface.
    /// </summary>
    public static class DirectoryOperationsHelper
    {
        /// <summary>
        /// Tries to delete folder specified by <paramref name="directoryPath"/>, but if exception occurs then
        /// that is swallowed.
        /// </summary>
        /// <param name="directoryOperations">The <see cref="IDirectoryOperations"/>.</param>
        /// <param name="directoryPath">The path of deleted folder.</param>
        /// <param name="recursive"><c>true</c> to perform recursive delete; <c>false</c> otherwise.</param>
        public static void TryDelete(this IDirectoryOperations directoryOperations, string directoryPath, bool recursive = false)
        {
            try
            {
                directoryOperations.Delete(directoryPath, recursive);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
