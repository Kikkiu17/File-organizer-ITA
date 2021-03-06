namespace File_Organizer
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.SelectStartFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.StartPath = new System.Windows.Forms.TextBox();
            this.UnOperatedFiles = new System.Windows.Forms.ListBox();
            this.DestPath = new System.Windows.Forms.TextBox();
            this.SelectDestFolder = new System.Windows.Forms.Button();
            this.MoveFilesButton = new System.Windows.Forms.Button();
            this.CopyFilesButton = new System.Windows.Forms.Button();
            this.OperatedFiles = new System.Windows.Forms.ListBox();
            this.SelectBackupFolder = new System.Windows.Forms.Button();
            this.BackupPath = new System.Windows.Forms.TextBox();
            this.CreateBackupButton = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.DeleteBackups = new System.Windows.Forms.Button();
            this.Preview = new System.Windows.Forms.CheckBox();
            this.STATE = new System.Windows.Forms.Label();
            this.MODE = new System.Windows.Forms.Label();
            this.Help = new System.Windows.Forms.Button();
            this.FileListOptions = new System.Windows.Forms.Button();
            this.UndoButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.BackgroundUpdate = new System.ComponentModel.BackgroundWorker();
            this.BackgroundUndo = new System.ComponentModel.BackgroundWorker();
            this.LockFolderCheckBox = new System.Windows.Forms.CheckBox();
            this.FileManagerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectStartFolder
            // 
            this.SelectStartFolder.Location = new System.Drawing.Point(452, 26);
            this.SelectStartFolder.Name = "SelectStartFolder";
            this.SelectStartFolder.Size = new System.Drawing.Size(75, 23);
            this.SelectStartFolder.TabIndex = 0;
            this.SelectStartFolder.Text = "Sfoglia...";
            this.SelectStartFolder.UseVisualStyleBackColor = true;
            this.SelectStartFolder.Click += new System.EventHandler(this.SelectStartFolder_Click);
            // 
            // StartPath
            // 
            this.StartPath.Location = new System.Drawing.Point(12, 26);
            this.StartPath.Name = "StartPath";
            this.StartPath.Size = new System.Drawing.Size(434, 23);
            this.StartPath.TabIndex = 1;
            this.StartPath.Text = "Percorso...";
            // 
            // UnOperatedFiles
            // 
            this.UnOperatedFiles.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UnOperatedFiles.FormattingEnabled = true;
            this.UnOperatedFiles.HorizontalScrollbar = true;
            this.UnOperatedFiles.ItemHeight = 15;
            this.UnOperatedFiles.Location = new System.Drawing.Point(12, 204);
            this.UnOperatedFiles.Name = "UnOperatedFiles";
            this.UnOperatedFiles.Size = new System.Drawing.Size(515, 259);
            this.UnOperatedFiles.TabIndex = 2;
            // 
            // DestPath
            // 
            this.DestPath.Location = new System.Drawing.Point(12, 84);
            this.DestPath.Name = "DestPath";
            this.DestPath.Size = new System.Drawing.Size(434, 23);
            this.DestPath.TabIndex = 3;
            this.DestPath.Text = "Percorso...";
            // 
            // SelectDestFolder
            // 
            this.SelectDestFolder.Location = new System.Drawing.Point(452, 84);
            this.SelectDestFolder.Name = "SelectDestFolder";
            this.SelectDestFolder.Size = new System.Drawing.Size(75, 23);
            this.SelectDestFolder.TabIndex = 4;
            this.SelectDestFolder.Text = "Sfoglia...";
            this.SelectDestFolder.UseVisualStyleBackColor = true;
            this.SelectDestFolder.Click += new System.EventHandler(this.SelectDestFolder_Click);
            // 
            // MoveFilesButton
            // 
            this.MoveFilesButton.Location = new System.Drawing.Point(822, 26);
            this.MoveFilesButton.Name = "MoveFilesButton";
            this.MoveFilesButton.Size = new System.Drawing.Size(110, 23);
            this.MoveFilesButton.TabIndex = 5;
            this.MoveFilesButton.Text = "Sposta tutto";
            this.MoveFilesButton.UseVisualStyleBackColor = true;
            this.MoveFilesButton.Click += new System.EventHandler(this.MoveFilesButton_Click);
            // 
            // CopyFilesButton
            // 
            this.CopyFilesButton.Location = new System.Drawing.Point(822, 55);
            this.CopyFilesButton.Name = "CopyFilesButton";
            this.CopyFilesButton.Size = new System.Drawing.Size(110, 23);
            this.CopyFilesButton.TabIndex = 9;
            this.CopyFilesButton.Text = "Copia tutto";
            this.CopyFilesButton.UseVisualStyleBackColor = true;
            this.CopyFilesButton.Click += new System.EventHandler(this.CopyFilesButton_Click);
            // 
            // OperatedFiles
            // 
            this.OperatedFiles.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.OperatedFiles.FormattingEnabled = true;
            this.OperatedFiles.HorizontalScrollbar = true;
            this.OperatedFiles.ItemHeight = 15;
            this.OperatedFiles.Location = new System.Drawing.Point(533, 204);
            this.OperatedFiles.Name = "OperatedFiles";
            this.OperatedFiles.Size = new System.Drawing.Size(515, 259);
            this.OperatedFiles.TabIndex = 10;
            this.OperatedFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox2_MouseDown);
            // 
            // SelectBackupFolder
            // 
            this.SelectBackupFolder.Enabled = false;
            this.SelectBackupFolder.Location = new System.Drawing.Point(452, 142);
            this.SelectBackupFolder.Name = "SelectBackupFolder";
            this.SelectBackupFolder.Size = new System.Drawing.Size(75, 23);
            this.SelectBackupFolder.TabIndex = 13;
            this.SelectBackupFolder.Text = "Sfoglia...";
            this.SelectBackupFolder.UseVisualStyleBackColor = true;
            this.SelectBackupFolder.Click += new System.EventHandler(this.SelectBackupFolder_Click);
            // 
            // BackupPath
            // 
            this.BackupPath.Enabled = false;
            this.BackupPath.Location = new System.Drawing.Point(12, 142);
            this.BackupPath.Name = "BackupPath";
            this.BackupPath.Size = new System.Drawing.Size(434, 23);
            this.BackupPath.TabIndex = 12;
            this.BackupPath.Text = "Percorso...";
            // 
            // CreateBackupButton
            // 
            this.CreateBackupButton.AutoSize = true;
            this.CreateBackupButton.Location = new System.Drawing.Point(938, 29);
            this.CreateBackupButton.Name = "CreateBackupButton";
            this.CreateBackupButton.Size = new System.Drawing.Size(92, 19);
            this.CreateBackupButton.TabIndex = 14;
            this.CreateBackupButton.Text = "Crea backup";
            this.CreateBackupButton.UseVisualStyleBackColor = true;
            this.CreateBackupButton.CheckedChanged += new System.EventHandler(this.CreateBackupButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Scegli una cartella...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Scegli una cartella...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "Cartella da riordinare";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Cartella di destinazione";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "Cartella di backup";
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(822, 113);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(110, 23);
            this.UpdateButton.TabIndex = 20;
            this.UpdateButton.Text = "Aggiorna";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // DeleteBackups
            // 
            this.DeleteBackups.Location = new System.Drawing.Point(822, 84);
            this.DeleteBackups.Name = "DeleteBackups";
            this.DeleteBackups.Size = new System.Drawing.Size(110, 23);
            this.DeleteBackups.TabIndex = 21;
            this.DeleteBackups.Text = "Elimina backup";
            this.DeleteBackups.UseVisualStyleBackColor = true;
            this.DeleteBackups.Click += new System.EventHandler(this.DeleteBackups_Click);
            // 
            // Preview
            // 
            this.Preview.AutoSize = true;
            this.Preview.Location = new System.Drawing.Point(938, 58);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(82, 19);
            this.Preview.TabIndex = 24;
            this.Preview.Text = "Anteprima";
            this.Preview.UseVisualStyleBackColor = true;
            this.Preview.CheckedChanged += new System.EventHandler(this.Preview_CheckedChanged);
            // 
            // STATE
            // 
            this.STATE.AutoSize = true;
            this.STATE.Location = new System.Drawing.Point(12, 495);
            this.STATE.Name = "STATE";
            this.STATE.Size = new System.Drawing.Size(43, 15);
            this.STATE.TabIndex = 25;
            this.STATE.Text = "Pronto";
            // 
            // MODE
            // 
            this.MODE.AutoSize = true;
            this.MODE.Location = new System.Drawing.Point(12, 510);
            this.MODE.Name = "MODE";
            this.MODE.Size = new System.Drawing.Size(322, 15);
            this.MODE.TabIndex = 26;
            this.MODE.Text = "MODALITA\' NORMALE: Verranno apportate modifiche ai file";
            // 
            // Help
            // 
            this.Help.Location = new System.Drawing.Point(938, 83);
            this.Help.Name = "Help";
            this.Help.Size = new System.Drawing.Size(110, 23);
            this.Help.TabIndex = 27;
            this.Help.Text = "Aiuto";
            this.Help.UseVisualStyleBackColor = true;
            this.Help.Click += new System.EventHandler(this.Help_Click);
            // 
            // FileListOptions
            // 
            this.FileListOptions.Location = new System.Drawing.Point(822, 142);
            this.FileListOptions.Name = "FileListOptions";
            this.FileListOptions.Size = new System.Drawing.Size(110, 23);
            this.FileListOptions.TabIndex = 28;
            this.FileListOptions.Text = "Opzioni lista file";
            this.FileListOptions.UseVisualStyleBackColor = true;
            this.FileListOptions.Click += new System.EventHandler(this.FileListOptions_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.Location = new System.Drawing.Point(938, 113);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(110, 23);
            this.UndoButton.TabIndex = 29;
            this.UndoButton.Text = "Undo";
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 469);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1036, 23);
            this.progressBar1.TabIndex = 30;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoMove);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoCopy);
            // 
            // BackgroundUpdate
            // 
            this.BackgroundUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoUpdate);
            this.BackgroundUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.UpdateComplete);
            // 
            // LockFolderCheckBox
            // 
            this.LockFolderCheckBox.AutoSize = true;
            this.LockFolderCheckBox.Enabled = false;
            this.LockFolderCheckBox.Location = new System.Drawing.Point(147, 66);
            this.LockFolderCheckBox.Name = "LockFolderCheckBox";
            this.LockFolderCheckBox.Size = new System.Drawing.Size(61, 19);
            this.LockFolderCheckBox.TabIndex = 31;
            this.LockFolderCheckBox.Text = "Blocca";
            this.LockFolderCheckBox.UseVisualStyleBackColor = true;
            this.LockFolderCheckBox.CheckedChanged += new System.EventHandler(this.LockFolderCheckBox_CheckedChanged);
            // 
            // FileManagerButton
            // 
            this.FileManagerButton.Location = new System.Drawing.Point(938, 141);
            this.FileManagerButton.Name = "FileManagerButton";
            this.FileManagerButton.Size = new System.Drawing.Size(110, 23);
            this.FileManagerButton.TabIndex = 32;
            this.FileManagerButton.Text = "Gestore file";
            this.FileManagerButton.UseVisualStyleBackColor = true;
            this.FileManagerButton.Click += new System.EventHandler(this.FileSizeButton_Click);
            // 
            // MainWindow
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 532);
            this.Controls.Add(this.FileManagerButton);
            this.Controls.Add(this.LockFolderCheckBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.UndoButton);
            this.Controls.Add(this.FileListOptions);
            this.Controls.Add(this.Help);
            this.Controls.Add(this.MODE);
            this.Controls.Add(this.STATE);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.DeleteBackups);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CreateBackupButton);
            this.Controls.Add(this.SelectBackupFolder);
            this.Controls.Add(this.BackupPath);
            this.Controls.Add(this.OperatedFiles);
            this.Controls.Add(this.CopyFilesButton);
            this.Controls.Add(this.MoveFilesButton);
            this.Controls.Add(this.SelectDestFolder);
            this.Controls.Add(this.DestPath);
            this.Controls.Add(this.UnOperatedFiles);
            this.Controls.Add(this.StartPath);
            this.Controls.Add(this.SelectStartFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "File Organizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button SelectStartFolder;
        private FolderBrowserDialog folderBrowserDialog1;
        private TextBox StartPath;
        private ListBox UnOperatedFiles;
        private TextBox DestPath;
        private Button SelectDestFolder;
        private Button MoveFilesButton;
        private Button CopyFilesButton;
        private ListBox OperatedFiles;
        private Button SelectBackupFolder;
        private TextBox BackupPath;
        private CheckBox CreateBackupButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button UpdateButton;
        private Button DeleteBackups;
        private CheckBox Preview;
        private Label STATE;
        private Label MODE;
        private Button Help;
        public ProgressBar PROGRESS;
        private Button FileListOptions;
        private Button UndoButton;
        private ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker BackgroundUpdate;
        private System.ComponentModel.BackgroundWorker BackgroundUndo;
        private CheckBox LockFolderCheckBox;
        private Button FileManagerButton;
    }
}