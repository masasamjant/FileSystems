namespace Masasamjant.BackupManager.Dialogs
{
    partial class ErrorDialog
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
            labelMessage = new Label();
            textMessage = new TextBox();
            labelState = new Label();
            textState = new TextBox();
            labelDirectory = new Label();
            textDirectory = new TextBox();
            labelFile = new Label();
            textFile = new TextBox();
            checkBoxCancel = new CheckBox();
            buttonIgnore = new Button();
            buttonOk = new Button();
            SuspendLayout();
            // 
            // labelMessage
            // 
            labelMessage.AutoSize = true;
            labelMessage.Location = new Point(31, 46);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(35, 15);
            labelMessage.TabIndex = 0;
            labelMessage.Text = "Error:";
            // 
            // textMessage
            // 
            textMessage.Location = new Point(104, 43);
            textMessage.Name = "textMessage";
            textMessage.ReadOnly = true;
            textMessage.Size = new Size(637, 23);
            textMessage.TabIndex = 1;
            // 
            // labelState
            // 
            labelState.AutoSize = true;
            labelState.Location = new Point(31, 93);
            labelState.Name = "labelState";
            labelState.Size = new Size(36, 15);
            labelState.TabIndex = 2;
            labelState.Text = "State:";
            // 
            // textState
            // 
            textState.Location = new Point(104, 90);
            textState.Name = "textState";
            textState.ReadOnly = true;
            textState.Size = new Size(361, 23);
            textState.TabIndex = 3;
            // 
            // labelDirectory
            // 
            labelDirectory.AutoSize = true;
            labelDirectory.Location = new Point(31, 131);
            labelDirectory.Name = "labelDirectory";
            labelDirectory.Size = new Size(58, 15);
            labelDirectory.TabIndex = 4;
            labelDirectory.Text = "Directory:";
            // 
            // textDirectory
            // 
            textDirectory.Location = new Point(104, 128);
            textDirectory.Name = "textDirectory";
            textDirectory.ReadOnly = true;
            textDirectory.Size = new Size(637, 23);
            textDirectory.TabIndex = 5;
            // 
            // labelFile
            // 
            labelFile.AutoSize = true;
            labelFile.Location = new Point(31, 169);
            labelFile.Name = "labelFile";
            labelFile.Size = new Size(28, 15);
            labelFile.TabIndex = 6;
            labelFile.Text = "File:";
            // 
            // textFile
            // 
            textFile.Location = new Point(104, 166);
            textFile.Name = "textFile";
            textFile.ReadOnly = true;
            textFile.Size = new Size(637, 23);
            textFile.TabIndex = 7;
            // 
            // checkBoxCancel
            // 
            checkBoxCancel.AutoSize = true;
            checkBoxCancel.Location = new Point(104, 230);
            checkBoxCancel.Name = "checkBoxCancel";
            checkBoxCancel.Size = new Size(104, 19);
            checkBoxCancel.TabIndex = 8;
            checkBoxCancel.Text = "&Cancel Backup";
            checkBoxCancel.UseVisualStyleBackColor = true;
            checkBoxCancel.CheckedChanged += checkBoxCancel_CheckedChanged;
            // 
            // buttonIgnore
            // 
            buttonIgnore.Location = new Point(666, 284);
            buttonIgnore.Name = "buttonIgnore";
            buttonIgnore.Size = new Size(75, 23);
            buttonIgnore.TabIndex = 9;
            buttonIgnore.Text = "&Ignore";
            buttonIgnore.UseVisualStyleBackColor = true;
            buttonIgnore.Click += buttonIgnore_Click;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(569, 284);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 10;
            buttonOk.Text = "&OK";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // ErrorDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(794, 338);
            Controls.Add(buttonOk);
            Controls.Add(buttonIgnore);
            Controls.Add(checkBoxCancel);
            Controls.Add(textFile);
            Controls.Add(labelFile);
            Controls.Add(textDirectory);
            Controls.Add(labelDirectory);
            Controls.Add(textState);
            Controls.Add(labelState);
            Controls.Add(textMessage);
            Controls.Add(labelMessage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ErrorDialog";
            ShowInTaskbar = false;
            Text = "Error";
            Load += ErrorDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelMessage;
        private TextBox textMessage;
        private Label labelState;
        private TextBox textState;
        private Label labelDirectory;
        private TextBox textDirectory;
        private Label labelFile;
        private TextBox textFile;
        private CheckBox checkBoxCancel;
        private Button buttonIgnore;
        private Button buttonOk;
    }
}