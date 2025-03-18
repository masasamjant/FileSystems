using Masasamjant.FileSystems;
using Masasamjant.FileSystems.Abstractions;
using Masasamjant.FileSystems.Backups;

namespace Masasamjant.BackupManager
{
    public partial class MainForm : Form
    {
        private readonly IFileSystem fileSystem;

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
                textBoxDestination.Text= browseDestinationDialog.SelectedPath;
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            var value = comboBoxTask.SelectedItem?.ToString();

            if (Enum.TryParse<BackupMode>(value, true, out var mode)) 
            {
                var name = textBoxName.Text;
                var source = textBoxSource.Text;
                var destination = textBoxDestination.Text;
                var properties = new BackupProperties(textBoxName.Text, mode, textBoxSource.Text, textBoxDestination.Text, checkIncludeSubFolders.Checked);
                using (var task = BackupTaskFactory.CreateBackupTask(properties, fileSystem))
                {
                    try
                    {
                        task.FileBackup += OnBackupTaskFileBackup;
                        task.Run();
                    }
                    finally
                    {
                        task.FileBackup -= OnBackupTaskFileBackup;
                    }
                }
            }
        }

        private void OnBackupTaskFileBackup(object? sender, BackupTaskFileEventArgs e)
        {
            listBoxFiles.Items.Add($"{e.CurrentFilePath} > {e.BackupFilePath}");
        }
    }
}
