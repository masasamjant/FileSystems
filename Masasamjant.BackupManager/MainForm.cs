using Masasamjant.BackupManager.Dialogs;
using Masasamjant.FileSystems;
using Masasamjant.FileSystems.Abstractions;
using Masasamjant.FileSystems.Backups;
using System.Text.Json;

namespace Masasamjant.BackupManager
{
    public partial class MainForm : Form
    {
        private readonly IFileSystem fileSystem;
        private const string FileDialogFilter = "Task (*.btask)|*.json|All (*.*)|*.*\"";

        public MainForm()
        {
            InitializeComponent();
            fileSystem = new FileSystem();
        }

        private void buttonBrowseSource_Click(object sender, EventArgs e)
        {
            browseSourceDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (browseSourceDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSource.Text = browseSourceDialog.SelectedPath;
            }
        }

        private void buttonBrowseDestination_Click(object sender, EventArgs e)
        {
            browseDestinationDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (browseDestinationDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxDestination.Text = browseDestinationDialog.SelectedPath;
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            listBoxFiles.Items.Clear();

            var value = comboBoxTask.SelectedItem?.ToString();

            if (Enum.TryParse<BackupMode>(value, true, out var mode))
            {
                var properties = new BackupProperties(textBoxName.Text, mode, textBoxSource.Text, textBoxDestination.Text, checkIncludeSubFolders.Checked);
                using (var task = BackupTaskFactory.CreateBackupTask(properties, fileSystem))
                {
                    try
                    {
                        task.FileBackup += OnBackupTaskFileBackup;
                        task.Error += OnBackupTaskError;
                        task.Run();
                    }
                    finally
                    {
                        task.FileBackup -= OnBackupTaskFileBackup;
                        task.Error -= OnBackupTaskError;
                    }
                }
            }
        }

        private void OnBackupTaskError(object? sender, BackupTaskErrorEventArgs e)
        {
            using (var dialog = new ErrorDialog(e))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    e.Handled = true;
                }
                else
                    e.Handled = false;
            }
        }

        private void OnBackupTaskFileBackup(object? sender, BackupTaskFileEventArgs e)
        {
            listBoxFiles.Items.Add($"{e.CurrentFilePath} > {e.BackupFilePath}");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var value = comboBoxTask.SelectedItem?.ToString();

            if (Enum.TryParse<BackupMode>(value, true, out var mode))
            {
                var properties = new BackupProperties(textBoxName.Text, mode, textBoxSource.Text, textBoxDestination.Text, checkIncludeSubFolders.Checked);
                var json = JsonSerializer.Serialize(properties);
                var initialDirectory = GetInitialBackupTaskFolder();
                savePropertiesDialog.InitialDirectory = initialDirectory;
                savePropertiesDialog.Filter = FileDialogFilter;

                if (savePropertiesDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = fileSystem.FileOperations.GetStream(savePropertiesDialog.FileName, FileMode.Create, FileAccess.Write))
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(json);
                        writer.Flush();
                    }

                    var directoryPath = Path.GetDirectoryName(savePropertiesDialog.FileName);

                    if (!string.IsNullOrWhiteSpace(directoryPath) && directoryPath != initialDirectory)
                    {
                        SaveInitialBackupTaskFolder(directoryPath);
                    }
                }
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            CheckValidProperties();
        }

        private void comboBoxTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckValidProperties();
        }

        private void textBoxSource_TextChanged(object sender, EventArgs e)
        {
            CheckValidProperties();
        }

        private void textBoxDestination_TextChanged(object sender, EventArgs e)
        {
            CheckValidProperties();
        }

        private void CheckValidProperties()
        {
            if (!string.IsNullOrWhiteSpace(textBoxName.Text) &&
                !string.IsNullOrWhiteSpace(textBoxSource.Text) &&
                !string.IsNullOrWhiteSpace(textBoxDestination.Text) &&
                comboBoxTask.SelectedItem != null)
            {
                buttonRun.Enabled = true;
                buttonSave.Enabled = true;
            }
            else
            {
                buttonRun.Enabled = false;
                buttonSave.Enabled = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openPropertiesDialog.InitialDirectory = GetInitialBackupTaskFolder();
            openPropertiesDialog.Filter = FileDialogFilter;

            if (openPropertiesDialog.ShowDialog() == DialogResult.OK)
            {
                string json;

                using (var stream = fileSystem.FileOperations.GetStream(openPropertiesDialog.FileName, FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }

                var properties = JsonSerializer.Deserialize(json, typeof(BackupProperties)) as BackupProperties;

                if (properties != null)
                {
                    textBoxName.Text = properties.BackupName;
                    comboBoxTask.SelectedItem = properties.BackupMode.ToString();
                    textBoxSource.Text = properties.SourceDirectoryPath;
                    textBoxDestination.Text = properties.DestinationDirectoryPath;
                    checkIncludeSubFolders.Checked = properties.IncludeSubDirectories;
                    CheckValidProperties();
                }
            }

        }

        private string GetInitialBackupTaskFolder()
        {
            var settings = Properties.Settings.Default;
            var latestBackupTaskFolder = settings.LatestBackupTaskFolder;
            if (!string.IsNullOrWhiteSpace(latestBackupTaskFolder) && fileSystem.DirectoryOperations.Exists(latestBackupTaskFolder))
                return latestBackupTaskFolder;
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private static void SaveInitialBackupTaskFolder(string directoryPath)
        {
            var settings = Properties.Settings.Default;
            settings.LatestBackupTaskFolder = directoryPath;
            settings.Save();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new AboutDialog())
                dialog.ShowDialog();
        }
    }
}
