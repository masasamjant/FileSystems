﻿using Masasamjant.FileSystems.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Masasamjant.FileSystems.Backups
{
    /// <summary>
    /// Represents history of backup for single folder.
    /// </summary>
    internal sealed class BackupHistory
    {
        private const string SHAPrefix = "SHA-";

        private Dictionary<string, string> Items { get; } = new Dictionary<string, string>();

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
            string backupHistoryFilePath = Path.Combine(backupDirectoryPath, GetHistoryFileName(backupDirectoryPath));

            BackupHistory history = new BackupHistory();

            if (!fileOperations.Exists(backupHistoryFilePath))
                return new BackupHistory();

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
        /// <param name="backupDirectoryPath">The backup directory path.</param>
        /// <param name="fileOperations">The file operations.</param>
        public static void Save(BackupHistory history, string backupDirectoryPath, IFileOperations fileOperations)
        {
            if (history.Count == 0)
                return;

            var backupHistoryFilePath = Path.Combine(backupDirectoryPath, GetHistoryFileName(backupDirectoryPath));

            if (fileOperations.Exists(backupHistoryFilePath))
                BackupTask.RemoveHiddenReadOnlyAttribute(backupHistoryFilePath, fileOperations);

            using (var stream = fileOperations.GetStream(backupHistoryFilePath, FileMode.Create, FileAccess.Write))
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

        /// <summary>
        /// Merge several <see cref="BackupHistory"/> instances in to single one.
        /// </summary>
        /// <param name="histories">The backup histories to merge.</param>
        /// <returns>A <paramref name="histories"/> merged into single <see cref="BackupHistory"/>.</returns>
        public static BackupHistory Merge(IEnumerable<BackupHistory> histories)
        {
            var merge = new BackupHistory();
            foreach (var history in histories)
                foreach (var keyValue in history.Items)
                    merge.Set(keyValue.Key, keyValue.Value);
            return merge;
        }

        private static string ComputeContentHash(string content)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(content);
            byte[] hash = SHA256.HashData(buffer);
            return Convert.ToBase64String(hash);
        }

        private static string GetHistoryFileName(string backupDirectoryPath)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(backupDirectoryPath);
            buffer = SHA1.HashData(buffer);
            var hash = Convert.ToBase64String(buffer);
            var replace = hash.Where(c => !char.IsLetter(c) && !char.IsAsciiDigit(c)).ToArray();
            foreach (var c in replace)
                hash = hash.Replace(c, '0');
            return hash.ToUpperInvariant() + ".hst";
        }
    }
}
