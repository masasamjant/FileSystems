using System.Reflection;

namespace Masasamjant.BackupManager.Dialogs
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void AboutDialog_Load(object sender, EventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            
            if (assemblyName != null && assemblyName.Version != null)
            {
                labelVersion.Text = "Version: " + assemblyName.Version.ToString(3);
            }
        }
    }
}
