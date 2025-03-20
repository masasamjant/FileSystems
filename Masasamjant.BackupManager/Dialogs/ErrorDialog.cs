using Masasamjant.FileSystems.Backups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        }
    }
}
