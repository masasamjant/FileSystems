namespace Masasamjant.BackupManager
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBoxTask = new GroupBox();
            buttonSave = new Button();
            checkIncludeSubFolders = new CheckBox();
            buttonRun = new Button();
            buttonBrowseDestination = new Button();
            textBoxDestination = new TextBox();
            labelDestination = new Label();
            buttonBrowseSource = new Button();
            textBoxSource = new TextBox();
            labelSource = new Label();
            labelTask = new Label();
            comboBoxTask = new ComboBox();
            textBoxName = new TextBox();
            labelName = new Label();
            browseSourceDialog = new FolderBrowserDialog();
            browseDestinationDialog = new FolderBrowserDialog();
            groupBoxFiles = new GroupBox();
            listBoxFiles = new ListBox();
            mainMenu = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            savePropertiesDialog = new SaveFileDialog();
            openPropertiesDialog = new OpenFileDialog();
            groupBoxTask.SuspendLayout();
            groupBoxFiles.SuspendLayout();
            mainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxTask
            // 
            groupBoxTask.Controls.Add(buttonSave);
            groupBoxTask.Controls.Add(checkIncludeSubFolders);
            groupBoxTask.Controls.Add(buttonRun);
            groupBoxTask.Controls.Add(buttonBrowseDestination);
            groupBoxTask.Controls.Add(textBoxDestination);
            groupBoxTask.Controls.Add(labelDestination);
            groupBoxTask.Controls.Add(buttonBrowseSource);
            groupBoxTask.Controls.Add(textBoxSource);
            groupBoxTask.Controls.Add(labelSource);
            groupBoxTask.Controls.Add(labelTask);
            groupBoxTask.Controls.Add(comboBoxTask);
            groupBoxTask.Controls.Add(textBoxName);
            groupBoxTask.Controls.Add(labelName);
            groupBoxTask.Location = new Point(12, 31);
            groupBoxTask.Name = "groupBoxTask";
            groupBoxTask.Size = new Size(847, 213);
            groupBoxTask.TabIndex = 0;
            groupBoxTask.TabStop = false;
            groupBoxTask.Text = "Properties";
            // 
            // buttonSave
            // 
            buttonSave.Enabled = false;
            buttonSave.Location = new Point(628, 167);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 12;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // checkIncludeSubFolders
            // 
            checkIncludeSubFolders.AutoSize = true;
            checkIncludeSubFolders.Location = new Point(98, 167);
            checkIncludeSubFolders.Name = "checkIncludeSubFolders";
            checkIncludeSubFolders.Size = new Size(128, 19);
            checkIncludeSubFolders.TabIndex = 11;
            checkIncludeSubFolders.Text = "Include sub-folders";
            checkIncludeSubFolders.UseVisualStyleBackColor = true;
            // 
            // buttonRun
            // 
            buttonRun.Enabled = false;
            buttonRun.Location = new Point(709, 167);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new Size(75, 23);
            buttonRun.TabIndex = 10;
            buttonRun.Text = "Run";
            buttonRun.UseVisualStyleBackColor = true;
            buttonRun.Click += buttonRun_Click;
            // 
            // buttonBrowseDestination
            // 
            buttonBrowseDestination.Location = new Point(709, 120);
            buttonBrowseDestination.Name = "buttonBrowseDestination";
            buttonBrowseDestination.Size = new Size(75, 23);
            buttonBrowseDestination.TabIndex = 9;
            buttonBrowseDestination.Text = "Browse";
            buttonBrowseDestination.UseVisualStyleBackColor = true;
            buttonBrowseDestination.Click += buttonBrowseDestination_Click;
            // 
            // textBoxDestination
            // 
            textBoxDestination.Location = new Point(98, 120);
            textBoxDestination.Name = "textBoxDestination";
            textBoxDestination.ReadOnly = true;
            textBoxDestination.Size = new Size(605, 23);
            textBoxDestination.TabIndex = 8;
            textBoxDestination.TextChanged += textBoxDestination_TextChanged;
            // 
            // labelDestination
            // 
            labelDestination.AutoSize = true;
            labelDestination.Location = new Point(22, 123);
            labelDestination.Name = "labelDestination";
            labelDestination.Size = new Size(70, 15);
            labelDestination.TabIndex = 7;
            labelDestination.Text = "Destination:";
            // 
            // buttonBrowseSource
            // 
            buttonBrowseSource.Location = new Point(709, 91);
            buttonBrowseSource.Name = "buttonBrowseSource";
            buttonBrowseSource.Size = new Size(75, 23);
            buttonBrowseSource.TabIndex = 6;
            buttonBrowseSource.Text = "Browse";
            buttonBrowseSource.UseVisualStyleBackColor = true;
            buttonBrowseSource.Click += buttonBrowseSource_Click;
            // 
            // textBoxSource
            // 
            textBoxSource.Location = new Point(98, 91);
            textBoxSource.Name = "textBoxSource";
            textBoxSource.ReadOnly = true;
            textBoxSource.Size = new Size(605, 23);
            textBoxSource.TabIndex = 5;
            textBoxSource.TextChanged += textBoxSource_TextChanged;
            // 
            // labelSource
            // 
            labelSource.AutoSize = true;
            labelSource.Location = new Point(22, 94);
            labelSource.Name = "labelSource";
            labelSource.Size = new Size(46, 15);
            labelSource.TabIndex = 4;
            labelSource.Text = "Source:";
            // 
            // labelTask
            // 
            labelTask.AutoSize = true;
            labelTask.Location = new Point(22, 63);
            labelTask.Name = "labelTask";
            labelTask.Size = new Size(49, 15);
            labelTask.TabIndex = 3;
            labelTask.Text = "Backup:";
            // 
            // comboBoxTask
            // 
            comboBoxTask.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTask.FormattingEnabled = true;
            comboBoxTask.Items.AddRange(new object[] { "Full", "Differential", "Incremental", "Continuous" });
            comboBoxTask.Location = new Point(98, 60);
            comboBoxTask.Name = "comboBoxTask";
            comboBoxTask.Size = new Size(273, 23);
            comboBoxTask.TabIndex = 2;
            comboBoxTask.SelectedIndexChanged += comboBoxTask_SelectedIndexChanged;
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(98, 29);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(686, 23);
            textBoxName.TabIndex = 1;
            textBoxName.TextChanged += textBoxName_TextChanged;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(22, 32);
            labelName.Name = "labelName";
            labelName.Size = new Size(42, 15);
            labelName.TabIndex = 0;
            labelName.Text = "Name:";
            // 
            // groupBoxFiles
            // 
            groupBoxFiles.Controls.Add(listBoxFiles);
            groupBoxFiles.Location = new Point(12, 250);
            groupBoxFiles.Name = "groupBoxFiles";
            groupBoxFiles.Size = new Size(847, 289);
            groupBoxFiles.TabIndex = 1;
            groupBoxFiles.TabStop = false;
            groupBoxFiles.Text = "Files";
            // 
            // listBoxFiles
            // 
            listBoxFiles.Dock = DockStyle.Fill;
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.Location = new Point(3, 19);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(841, 267);
            listBoxFiles.TabIndex = 0;
            // 
            // mainMenu
            // 
            mainMenu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(871, 24);
            mainMenu.TabIndex = 2;
            mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(871, 551);
            Controls.Add(groupBoxFiles);
            Controls.Add(groupBoxTask);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = "Backup Manager";
            groupBoxTask.ResumeLayout(false);
            groupBoxTask.PerformLayout();
            groupBoxFiles.ResumeLayout(false);
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxTask;
        private Label labelTask;
        private ComboBox comboBoxTask;
        private TextBox textBoxName;
        private Label labelName;
        private Label labelSource;
        private TextBox textBoxDestination;
        private Label labelDestination;
        private Button buttonBrowseSource;
        private TextBox textBoxSource;
        private Button buttonBrowseDestination;
        private Button buttonRun;
        private FolderBrowserDialog browseSourceDialog;
        private FolderBrowserDialog browseDestinationDialog;
        private CheckBox checkIncludeSubFolders;
        private GroupBox groupBoxFiles;
        private ListBox listBoxFiles;
        private Button buttonSave;
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private SaveFileDialog savePropertiesDialog;
        private OpenFileDialog openPropertiesDialog;
    }
}
