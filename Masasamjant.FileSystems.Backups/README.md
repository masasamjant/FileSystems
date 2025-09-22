# Masasamjant File Systems Backups

Class library to create backups from directories. Classes in this class library can be used to perform directory backups. There is implementation for 4 different backups:

1. Full backup of selected source folder. Implemented in **FullBackupTask** class.
2. Differential backup of selected source folder. This means that it will create backup from all created or modified files since latest full backup. Implemented in **DifferentialBackupTask** class.
3. Incremental backup of selected source folder. This means that it will create backup from all created or modified files since latest incremental backup. Implemented in **IncrementalBackupTask** class.
4. Continuous backup of selected source folder. This means that full backup is created from source, but compared to full backup this use single destination folder and overwrites previous backup. Implemented in **ContinuousBackupTask** class. This optimizes the disk space usage, but backup history is lost.

All backup tasks support backing up selected source folder and, optionally, sub folders. How backup is performed are defined with **BackupProperties** class.

Following code demonstrates the most simple usage:

---
_var sourceDirectoryPath = @"D:\My\Files";_ 

_var destinationDirectoryPath = @"M:\Backups";_

_bool includeSubDirectories = true;_

_var properties = new BackupProperties("Demo", BackupMode.Full, sourceDirectoryPath, destinationDirectoryPath, includeSubDirectories);_

_var backupTask = new FullBackupTask(properties, new Masasamjant.FileSystems.FileSystem());_

_var backupTaskResult = backupTask.Run();_

---
To see full application using this class library please check the github repository at https://github.com/masasamjant/FileSystems
