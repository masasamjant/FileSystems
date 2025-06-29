using Masasamjant.FileSystems.Backups;

namespace Masasamjant.BackupManager.Dialogs
{
    public partial class ErrorDialog : Form
    {
        public ErrorDialog()
        {
            InitializeComponent();
        }

        internal ErrorDialog(BackupTaskErrorEventArgs error)
            : this()
        {
            Error = error;
        }

        private BackupTaskErrorEventArgs? Error { get; }

        private void ErrorDialog_Load(object sender, EventArgs e)
        {
            if (Error != null)
                FillFromError(Error);
        }

        private void FillFromError(BackupTaskErrorEventArgs error)
        {
            textMessage.Text = error.Error.Message;
            textState.Text = error.CurrentState.ToString();

            if (!string.IsNullOrWhiteSpace(error.CurrentDirectoryPath))
                textDirectory.Text = error.CurrentDirectoryPath;

            if (!string.IsNullOrWhiteSpace(error.CurrentFilePath))
                textFile.Text = error.CurrentFilePath;

            checkBoxCancel.Enabled = error.CanCancel;
        }

        private void checkBoxCancel_CheckedChanged(object sender, EventArgs e)
        {
            if (Error != null)
                Error.Cancel = Error.CanCancel && checkBoxCancel.Checked;
        }

        private void buttonIgnore_Click(object sender, EventArgs e)
        {
            if (Error != null)
                Error.Handled = false;

            DialogResult = DialogResult.No;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (Error != null)
                Error.Handled = true;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
