using Masasamjant.FileSystems.Abstractions;

namespace Masasamjant.FileSystems.Adapters
{
    [TestClass]
    public class DriveInfoAdapterUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            FileSystem fileSystem = new FileSystem();
            DriveInfo drive = DriveInfo.GetDrives().Where(x => x.Name == "C:\\").First();
            IDriveOperations driveOperations = new DriveOperations(fileSystem);
            var adapter = new DriveInfoAdapter(drive, driveOperations);
            Assert.IsTrue(ReferenceEquals(driveOperations, adapter.DriveOperations));
            Assert.AreEqual(drive.Name, adapter.Name);
            Assert.AreEqual(drive.DriveFormat, adapter.DriveFormat);
            Assert.AreEqual(drive.DriveType, adapter.DriveType);
            Assert.AreEqual(drive.IsReady, adapter.IsReady);
            Assert.AreEqual(drive.TotalSize, adapter.TotalSize);
            Assert.AreEqual(drive.VolumeLabel, adapter.VolumeLabel);
            Assert.AreEqual(true, adapter.CanAccess);
        }

        [TestMethod]
        public void Test_RootDirectory()
        {
            FileSystem fileSystem = new FileSystem();
            DriveInfo drive = DriveInfo.GetDrives().Where(x => x.Name == "C:\\").First();
            IDriveOperations driveOperations = new DriveOperations(fileSystem);
            var adapter = new DriveInfoAdapter(drive, driveOperations);
            var rootDirectory = adapter.RootDirectory;
            Assert.IsTrue(typeof(DirectoryInfoAdapter).Equals(rootDirectory.GetType()));
        }
    }
}
