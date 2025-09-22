# Masasamjant File Systems

Class library to abstract file systems. This class library separates file system to several interfaces that can be used instead of concrete file system:

**IFileSystem** is the main interface that represents file system that provides drive, directory and file operations repsesented by interfaces: **IDriveOperations**, **IDirectoryOperations** and **IFileOperations**.

Each operations interface has own information interface that represents different file system item:

**IDriveInfo** represents information of drive.

**IDirectoryInfo** represents information of directory.

**IFileInfo** represents information of file.

The library contains implementation of these for .NET classes like _DriveInfo_, _DirectoryInfo_ and _FileInfo_.

The github repository https://github.com/masasamjant/FileSystems also has backups class library that provides example of how this library is used.
