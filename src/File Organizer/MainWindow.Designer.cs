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
            SelectStartFolder = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            StartPath = new TextBox();
            UnOperatedFiles = new ListBox();
            DestPath = new TextBox();
            SelectDestFolder = new Button();
            MoveFilesButton = new Button();
            CopyFilesButton = new Button();
            OperatedFiles = new ListBox();
            SelectBackupFolder = new Button();
            BackupPath = new TextBox();
            CreateBackupButton = new CheckBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            DeleteBackups = new Button();
            Preview = new CheckBox();
            STATE = new Label();
            MODE = new Label();
            Help = new Button();
            FileListOptions = new Button();
            UndoButton = new Button();
            progressBar1 = new ProgressBar();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            BackgroundUpdate = new System.ComponentModel.BackgroundWorker();
            BackgroundUndo = new System.ComponentModel.BackgroundWorker();
            LockDestFolderCheckBox = new CheckBox();
            FileManagerButton = new Button();
            RefreshButton = new Button();
            LockBackFolderCheckBox = new CheckBox();
            CheckUpdates = new Button();
            OrganizeIcons = new Button();
            UndoIcons = new Button();
            SetDefault = new Button();
            SuspendLayout();
            // 
            // SelectStartFolder
            // 
            SelectStartFolder.Location = new Point(452, 30);
            SelectStartFolder.Name = "SelectStartFolder";
            SelectStartFolder.Size = new Size(75, 23);
            SelectStartFolder.TabIndex = 0;
            SelectStartFolder.Text = "Sfoglia...";
            SelectStartFolder.UseVisualStyleBackColor = true;
            SelectStartFolder.Click += SelectStartFolder_Click;
            // 
            // StartPath
            // 
            StartPath.Location = new Point(12, 30);
            StartPath.Name = "StartPath";
            StartPath.Size = new Size(434, 23);
            StartPath.TabIndex = 1;
            StartPath.Text = "Percorso...";
            // 
            // UnOperatedFiles
            // 
            UnOperatedFiles.Anchor = AnchorStyles.Top;
            UnOperatedFiles.FormattingEnabled = true;
            UnOperatedFiles.HorizontalScrollbar = true;
            UnOperatedFiles.ItemHeight = 15;
            UnOperatedFiles.Location = new Point(12, 130);
            UnOperatedFiles.Name = "UnOperatedFiles";
            UnOperatedFiles.Size = new Size(515, 259);
            UnOperatedFiles.TabIndex = 2;
            UnOperatedFiles.KeyUp += UnOperatedFiles_KeyUp;
            UnOperatedFiles.MouseDoubleClick += UnOperatedFiles_MouseDoubleClick;
            UnOperatedFiles.MouseDown += UnOperatedFiles_MouseDown;
            // 
            // DestPath
            // 
            DestPath.Location = new Point(533, 31);
            DestPath.Name = "DestPath";
            DestPath.Size = new Size(434, 23);
            DestPath.TabIndex = 3;
            DestPath.Text = "Percorso...";
            // 
            // SelectDestFolder
            // 
            SelectDestFolder.Location = new Point(973, 30);
            SelectDestFolder.Name = "SelectDestFolder";
            SelectDestFolder.Size = new Size(69, 23);
            SelectDestFolder.TabIndex = 4;
            SelectDestFolder.Text = "Sfoglia...";
            SelectDestFolder.UseVisualStyleBackColor = true;
            SelectDestFolder.Click += SelectDestFolder_Click;
            // 
            // MoveFilesButton
            // 
            MoveFilesButton.Location = new Point(12, 103);
            MoveFilesButton.Name = "MoveFilesButton";
            MoveFilesButton.Size = new Size(123, 23);
            MoveFilesButton.TabIndex = 5;
            MoveFilesButton.Text = "Sposta tutto";
            MoveFilesButton.UseVisualStyleBackColor = true;
            MoveFilesButton.Click += MoveFilesButton_Click;
            // 
            // CopyFilesButton
            // 
            CopyFilesButton.Location = new Point(141, 103);
            CopyFilesButton.Name = "CopyFilesButton";
            CopyFilesButton.Size = new Size(123, 23);
            CopyFilesButton.TabIndex = 9;
            CopyFilesButton.Text = "Copia tutto";
            CopyFilesButton.UseVisualStyleBackColor = true;
            CopyFilesButton.Click += CopyFilesButton_Click;
            // 
            // OperatedFiles
            // 
            OperatedFiles.Anchor = AnchorStyles.Top;
            OperatedFiles.FormattingEnabled = true;
            OperatedFiles.HorizontalScrollbar = true;
            OperatedFiles.ItemHeight = 15;
            OperatedFiles.Location = new Point(533, 130);
            OperatedFiles.Name = "OperatedFiles";
            OperatedFiles.Size = new Size(510, 259);
            OperatedFiles.TabIndex = 10;
            OperatedFiles.KeyUp += OperatedFiles_KeyUp;
            OperatedFiles.MouseDoubleClick += OperatedFiles_MouseDoubleClick;
            OperatedFiles.MouseDown += listBox2_MouseDown;
            // 
            // SelectBackupFolder
            // 
            SelectBackupFolder.Enabled = false;
            SelectBackupFolder.Location = new Point(452, 74);
            SelectBackupFolder.Name = "SelectBackupFolder";
            SelectBackupFolder.Size = new Size(75, 23);
            SelectBackupFolder.TabIndex = 13;
            SelectBackupFolder.Text = "Sfoglia...";
            SelectBackupFolder.UseVisualStyleBackColor = true;
            SelectBackupFolder.Click += SelectBackupFolder_Click;
            // 
            // BackupPath
            // 
            BackupPath.Enabled = false;
            BackupPath.Location = new Point(12, 74);
            BackupPath.Name = "BackupPath";
            BackupPath.Size = new Size(434, 23);
            BackupPath.TabIndex = 12;
            BackupPath.Text = "Percorso...";
            // 
            // CreateBackupButton
            // 
            CreateBackupButton.AutoSize = true;
            CreateBackupButton.Location = new Point(621, 107);
            CreateBackupButton.Name = "CreateBackupButton";
            CreateBackupButton.Size = new Size(92, 19);
            CreateBackupButton.TabIndex = 14;
            CreateBackupButton.Text = "Crea backup";
            CreateBackupButton.UseVisualStyleBackColor = true;
            CreateBackupButton.CheckedChanged += CreateBackupButton_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 399);
            label1.Name = "label1";
            label1.Size = new Size(111, 15);
            label1.TabIndex = 15;
            label1.Text = "Scegli una cartella...";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(533, 399);
            label2.Name = "label2";
            label2.Size = new Size(111, 15);
            label2.TabIndex = 16;
            label2.Text = "Scegli una cartella...";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 8);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 17;
            label3.Text = "Cartella da riordinare";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(533, 8);
            label4.Name = "label4";
            label4.Size = new Size(129, 15);
            label4.TabIndex = 18;
            label4.Text = "Cartella di destinazione";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 56);
            label5.Name = "label5";
            label5.Size = new Size(102, 15);
            label5.TabIndex = 19;
            label5.Text = "Cartella di backup";
            // 
            // DeleteBackups
            // 
            DeleteBackups.Location = new Point(719, 104);
            DeleteBackups.Name = "DeleteBackups";
            DeleteBackups.Size = new Size(110, 23);
            DeleteBackups.TabIndex = 21;
            DeleteBackups.Text = "Elimina backup";
            DeleteBackups.UseVisualStyleBackColor = true;
            DeleteBackups.Click += DeleteBackups_Click;
            // 
            // Preview
            // 
            Preview.AutoSize = true;
            Preview.Location = new Point(533, 107);
            Preview.Name = "Preview";
            Preview.Size = new Size(82, 19);
            Preview.TabIndex = 24;
            Preview.Text = "Anteprima";
            Preview.UseVisualStyleBackColor = true;
            Preview.CheckedChanged += Preview_CheckedChanged;
            // 
            // STATE
            // 
            STATE.AutoSize = true;
            STATE.Location = new Point(12, 450);
            STATE.Name = "STATE";
            STATE.Size = new Size(43, 15);
            STATE.TabIndex = 25;
            STATE.Text = "Pronto";
            // 
            // MODE
            // 
            MODE.AutoSize = true;
            MODE.Location = new Point(12, 465);
            MODE.Name = "MODE";
            MODE.Size = new Size(322, 15);
            MODE.TabIndex = 26;
            MODE.Text = "MODALITA' NORMALE: Verranno apportate modifiche ai file";
            // 
            // Help
            // 
            Help.Location = new Point(942, 104);
            Help.Name = "Help";
            Help.Size = new Size(101, 23);
            Help.TabIndex = 27;
            Help.Text = "Aiuto";
            Help.UseVisualStyleBackColor = true;
            Help.Click += Help_Click;
            // 
            // FileListOptions
            // 
            FileListOptions.Location = new Point(835, 104);
            FileListOptions.Name = "FileListOptions";
            FileListOptions.Size = new Size(101, 23);
            FileListOptions.TabIndex = 28;
            FileListOptions.Text = "Lista file";
            FileListOptions.UseVisualStyleBackColor = true;
            FileListOptions.Click += FileListOptions_Click;
            // 
            // UndoButton
            // 
            UndoButton.Location = new Point(270, 103);
            UndoButton.Name = "UndoButton";
            UndoButton.Size = new Size(123, 23);
            UndoButton.TabIndex = 29;
            UndoButton.Text = "Undo";
            UndoButton.UseVisualStyleBackColor = true;
            UndoButton.Click += UndoButton_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 424);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1030, 23);
            progressBar1.TabIndex = 30;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.DoWork += DoMove;
            // 
            // backgroundWorker2
            // 
            backgroundWorker2.DoWork += DoCopy;
            // 
            // BackgroundUpdate
            // 
            BackgroundUpdate.DoWork += DoUpdate;
            BackgroundUpdate.RunWorkerCompleted += UpdateComplete;
            // 
            // LockDestFolderCheckBox
            // 
            LockDestFolderCheckBox.AutoSize = true;
            LockDestFolderCheckBox.Enabled = false;
            LockDestFolderCheckBox.Location = new Point(755, 7);
            LockDestFolderCheckBox.Name = "LockDestFolderCheckBox";
            LockDestFolderCheckBox.Size = new Size(61, 19);
            LockDestFolderCheckBox.TabIndex = 31;
            LockDestFolderCheckBox.Text = "Blocca";
            LockDestFolderCheckBox.UseVisualStyleBackColor = true;
            LockDestFolderCheckBox.CheckedChanged += LockFolderCheckBox_CheckedChanged;
            // 
            // FileManagerButton
            // 
            FileManagerButton.Location = new Point(399, 103);
            FileManagerButton.Name = "FileManagerButton";
            FileManagerButton.Size = new Size(123, 23);
            FileManagerButton.TabIndex = 32;
            FileManagerButton.Text = "Gestore file";
            FileManagerButton.UseVisualStyleBackColor = true;
            FileManagerButton.Click += FileSizeButton_Click;
            // 
            // RefreshButton
            // 
            RefreshButton.Image = (Image)resources.GetObject("RefreshButton.Image");
            RefreshButton.Location = new Point(533, 60);
            RefreshButton.Name = "RefreshButton";
            RefreshButton.Size = new Size(82, 41);
            RefreshButton.TabIndex = 41;
            RefreshButton.UseVisualStyleBackColor = true;
            RefreshButton.Click += button1_Click;
            // 
            // LockBackFolderCheckBox
            // 
            LockBackFolderCheckBox.AutoSize = true;
            LockBackFolderCheckBox.Enabled = false;
            LockBackFolderCheckBox.Location = new Point(120, 55);
            LockBackFolderCheckBox.Name = "LockBackFolderCheckBox";
            LockBackFolderCheckBox.Size = new Size(61, 19);
            LockBackFolderCheckBox.TabIndex = 42;
            LockBackFolderCheckBox.Text = "Blocca";
            LockBackFolderCheckBox.UseVisualStyleBackColor = true;
            LockBackFolderCheckBox.CheckedChanged += LockBackFolderCheckBox_CheckedChanged;
            // 
            // CheckUpdates
            // 
            CheckUpdates.Location = new Point(835, 457);
            CheckUpdates.Name = "CheckUpdates";
            CheckUpdates.Size = new Size(205, 23);
            CheckUpdates.TabIndex = 43;
            CheckUpdates.Text = "Controlla aggiornamenti";
            CheckUpdates.UseVisualStyleBackColor = true;
            CheckUpdates.Click += CheckUpdates_Click;
            // 
            // OrganizeIcons
            // 
            OrganizeIcons.Location = new Point(621, 69);
            OrganizeIcons.Name = "OrganizeIcons";
            OrganizeIcons.Size = new Size(134, 23);
            OrganizeIcons.TabIndex = 44;
            OrganizeIcons.Text = "Sistema icone desktop";
            OrganizeIcons.UseVisualStyleBackColor = true;
            OrganizeIcons.Click += OrganizeIcons_Click;
            // 
            // UndoIcons
            // 
            UndoIcons.Location = new Point(761, 69);
            UndoIcons.Name = "UndoIcons";
            UndoIcons.Size = new Size(101, 23);
            UndoIcons.TabIndex = 45;
            UndoIcons.Text = "Undo icone";
            UndoIcons.UseVisualStyleBackColor = true;
            UndoIcons.Click += UndoIcons_Click;
            // 
            // SetDefault
            // 
            SetDefault.Location = new Point(668, 4);
            SetDefault.Name = "SetDefault";
            SetDefault.Size = new Size(81, 23);
            SetDefault.TabIndex = 46;
            SetDefault.Text = "Predefinita";
            SetDefault.UseVisualStyleBackColor = true;
            SetDefault.Click += SetDefault_Click;
            // 
            // MainWindow
            // 
            AccessibleRole = AccessibleRole.None;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1052, 492);
            Controls.Add(SetDefault);
            Controls.Add(UndoIcons);
            Controls.Add(OrganizeIcons);
            Controls.Add(CheckUpdates);
            Controls.Add(LockBackFolderCheckBox);
            Controls.Add(RefreshButton);
            Controls.Add(FileManagerButton);
            Controls.Add(LockDestFolderCheckBox);
            Controls.Add(progressBar1);
            Controls.Add(UndoButton);
            Controls.Add(FileListOptions);
            Controls.Add(Help);
            Controls.Add(MODE);
            Controls.Add(STATE);
            Controls.Add(Preview);
            Controls.Add(DeleteBackups);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(CreateBackupButton);
            Controls.Add(SelectBackupFolder);
            Controls.Add(BackupPath);
            Controls.Add(OperatedFiles);
            Controls.Add(CopyFilesButton);
            Controls.Add(MoveFilesButton);
            Controls.Add(SelectDestFolder);
            Controls.Add(DestPath);
            Controls.Add(UnOperatedFiles);
            Controls.Add(StartPath);
            Controls.Add(SelectStartFolder);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainWindow";
            Text = " ";
            ResumeLayout(false);
            PerformLayout();
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
        private CheckBox LockDestFolderCheckBox;
        private Button FileManagerButton;
        private Button RefreshButton;
        private CheckBox LockBackFolderCheckBox;
        private Button CheckUpdates;
        private Button OrganizeIcons;
        private Button UndoIcons;
        private Button SetDefault;
    }
}