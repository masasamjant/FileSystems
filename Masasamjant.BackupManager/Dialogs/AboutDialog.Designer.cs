namespace Masasamjant.BackupManager.Dialogs
{
    partial class AboutDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelTitle = new Label();
            labelDescription = new Label();
            labelVersion = new Label();
            SuspendLayout();
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitle.Location = new Point(13, 17);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(138, 21);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Backup Manager";
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Location = new Point(13, 53);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(346, 15);
            labelDescription.TabIndex = 1;
            labelDescription.Text = "Easy and simple application to create backups on local machine.";
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Location = new Point(13, 80);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(84, 15);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "Version: 0.0.0.0";
            // 
            // AboutDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(375, 184);
            Controls.Add(labelVersion);
            Controls.Add(labelDescription);
            Controls.Add(labelTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutDialog";
            ShowInTaskbar = false;
            Text = "About";
            Load += AboutDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitle;
        private Label labelDescription;
        private Label labelVersion;
    }
}