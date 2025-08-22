using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Adapters
{
    [TestClass]
    public class DirectoryInfoAdapterUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            IFileSystem fileSystem = new FileSystem();
            DirectoryInfo directory = new DirectoryInfo(@"C:\Windows");
            IDirectoryOperations directoryOperations = new DirectoryOperations(fileSystem);
            var adapter = new DirectoryInfoAdapter(directory, directoryOperations);
            Assert.IsTrue(ReferenceEquals(directoryOperations, adapter.DirectoryOperations));
            Assert.AreEqual(directory.Name, adapter.Name);
            Assert.AreEqual(directory.FullName, adapter.FullName);
            Assert.AreEqual(directory.Exists, adapter.Exists);
            Assert.AreEqual(directory.Attributes, adapter.Attributes);
            Assert.AreEqual(directory.CreationTime, adapter.CreationTime);
            Assert.AreEqual(directory.CreationTimeUtc, adapter.CreationTimeUtc);
            Assert.AreEqual(directory.Parent!.Name, adapter.Parent!.Name);
            Assert.IsTrue(typeof(DirectoryInfoAdapter).Equals(adapter.Parent!.GetType()));
            Assert.AreEqual(directory.Root.Name, adapter.Root.Name);
            Assert.IsTrue(typeof(DirectoryInfoAdapter).Equals(adapter.Root.GetType()));
        }

        [TestMethod]
        public void Test_CreateSubDirectory()
        {
            IFileSystem fileSystem = new FileSystem();
            DirectoryInfo directory = new DirectoryInfo(@"C:\Windows\Temp");
            IDirectoryOperations directoryOperations = new DirectoryOperations(fileSystem);
            var adapter = new DirectoryInfoAdapter(directory, directoryOperations);
            var path = Path.Combine(directory.FullName, Guid.NewGuid().ToString("N").ToUpperInvariant());
            var subdirectory = adapter.CreateSubDirectory(path);
            Assert.AreEqual(Path.Combine(directory.FullName, path), subdirectory.FullName);
            adapter.DirectoryOperations.Delete(subdirectory.FullName);
        }
    }
}
