using Masasamjant.FileSystems.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents history of backup for single folder.
    /// </summary>
    internal sealed class BackupHistory
    {
        private const string BackupHistoryFileName = "__BackupHistory.txt";
        private const string SHAPrefix = "SHA-";

        private Dictionary<string, string> Items { get; } = new Dictionary<string, string>();

        internal BackupHistory(string backupDirectoryPath)
        {
            BackupDirectoryPath = backupDirectoryPath;
        }

        /// <summary>
        /// Gets the backup directory path;
        /// </summary>
        public string BackupDirectoryPath { get; }

        /// <summary>
        /// Gets the count of items in backup history.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Check if contains hash for file specified by name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns><c>true</c> if contains file hash for <paramref name="fileName"/>; <c>false</c> otherwise.</returns>
        public bool Contains(string fileName) => Items.ContainsKey(fileName);

        /// <summary>
        /// Gets file hash for file specified by name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>A file hash.</returns>
        /// <exception cref="BackupException">If does not have hash for <paramref name="fileName"/>.</exception>
        public string Get(string fileName)
        {
            if (Items.TryGetValue(fileName, out var fileHash))
                return fileHash;
            else
                throw new BackupException($"History does not contain file of {fileName}.");
        }

        /// <summary>
        /// Sets file hash for file specified by name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileHash">The file hash.</param>
        public void Set(string fileName, string fileHash) => Items[fileName] = fileHash;

        /// <summary>
        /// Load backup history from specified backup directory.
        /// </summary>
        /// <param name="backupDirectoryPath">The backup directory path.</param>
        /// <param name="fileOperations">The file operations.</param>
        /// <returns>A loaded or new <see cref="BackupHistory"/>.</returns>
        public static BackupHistory Load(string backupDirectoryPath, IFileOperations fileOperations)
        {
            string backupHistoryFilePath = Path.Combine(backupDirectoryPath, BackupHistoryFileName);

            BackupHistory history = new BackupHistory(backupHistoryFilePath);

            if (!fileOperations.Exists(backupHistoryFilePath))
                return new BackupHistory(backupDirectoryPath);

            BackupTask.RemoveHiddenReadOnlyAttribute(backupHistoryFilePath, fileOperations);
            
            using (var stream = fileOperations.GetStream(backupHistoryFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                StringBuilder sb = new StringBuilder();
                string? line;
                string? check = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(SHAPrefix))
                        check = line.Remove(0, SHAPrefix.Length);
                    else
                    {
                        sb.Append(line);
                        var parts = line.Split('|');
                        if (parts != null && parts.Length == 2)
                            history.Set(parts[0], parts[1]);
                    }
                }

                var contentCheck = ComputeContentHash(sb.ToString());

                if (check == null || !string.Equals(check, contentCheck, StringComparison.Ordinal))
                    throw new BackupException("Backup history file is corrupted.");
            }

            return history;
        }

        /// <summary>
        /// Save backup history.
        /// </summary>
        /// <param name="history">The backup history.</param>
        /// <param name="fileOperations">The file operations.</param>
        public static void Save(BackupHistory history, IFileOperations fileOperations)
        {
            if (history.Count == 0)
                return;

            var backupHistoryFilePath = Path.Combine(history.BackupDirectoryPath, BackupHistoryFileName);

            if (fileOperations.Exists(backupHistoryFilePath))
                BackupTask.RemoveHiddenReadOnlyAttribute(backupHistoryFilePath, fileOperations);

            using (var stream = fileOperations.GetStream(backupHistoryFilePath, FileMode.Open, FileAccess.Read))
            using (var writer = new StreamWriter(stream))
            {
                var sb = new StringBuilder();

                foreach (var keyValue in history.Items)
                {
                    var line = string.Join('|', keyValue.Key, keyValue.Value);
                    writer.WriteLine(line);
                    sb.Append(line);
                }

                var check = ComputeContentHash(sb.ToString());
                writer.WriteLine(SHAPrefix + check);
                writer.Flush();
            }

            BackupTask.AddHiddenReadOnlyAttribute(backupHistoryFilePath, fileOperations);
        }

        private static string ComputeContentHash(string content)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(content);
            byte[] hash = SHA256.HashData(buffer);
            return Convert.ToBase64String(hash);
        }
    }
}
