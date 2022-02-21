namespace File_Organizer
{
    partial class Form1
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
            this.SelectStartFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.StartPath = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.DestPath = new System.Windows.Forms.TextBox();
            this.SelectDestFolder = new System.Windows.Forms.Button();
            this.MoveFilesButton = new System.Windows.Forms.Button();
            this.CopyFilesButton = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
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
            this.SuspendLayout();
            // 
            // SelectStartFolder
            // 
            this.SelectStartFolder.Location = new System.Drawing.Point(452, 27);
            this.SelectStartFolder.Name = "SelectStartFolder";
            this.SelectStartFolder.Size = new System.Drawing.Size(75, 23);
            this.SelectStartFolder.TabIndex = 0;
            this.SelectStartFolder.Text = "Sfoglia...";
            this.SelectStartFolder.UseVisualStyleBackColor = true;
            this.SelectStartFolder.Click += new System.EventHandler(this.SelectStartFolder_Click);
            // 
            // StartPath
            // 
            this.StartPath.Location = new System.Drawing.Point(12, 27);
            this.StartPath.Name = "StartPath";
            this.StartPath.Size = new System.Drawing.Size(434, 23);
            this.StartPath.TabIndex = 1;
            this.StartPath.Text = "Percorso...";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(12, 204);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(515, 259);
            this.listBox1.TabIndex = 2;
            // 
            // DestPath
            // 
            this.DestPath.Location = new System.Drawing.Point(12, 81);
            this.DestPath.Name = "DestPath";
            this.DestPath.Size = new System.Drawing.Size(434, 23);
            this.DestPath.TabIndex = 3;
            this.DestPath.Text = "Percorso...";
            // 
            // SelectDestFolder
            // 
            this.SelectDestFolder.Location = new System.Drawing.Point(452, 81);
            this.SelectDestFolder.Name = "SelectDestFolder";
            this.SelectDestFolder.Size = new System.Drawing.Size(75, 23);
            this.SelectDestFolder.TabIndex = 4;
            this.SelectDestFolder.Text = "Sfoglia...";
            this.SelectDestFolder.UseVisualStyleBackColor = true;
            this.SelectDestFolder.Click += new System.EventHandler(this.SelectDestFolder_Click);
            // 
            // MoveFilesButton
            // 
            this.MoveFilesButton.Location = new System.Drawing.Point(570, 26);
            this.MoveFilesButton.Name = "MoveFilesButton";
            this.MoveFilesButton.Size = new System.Drawing.Size(98, 23);
            this.MoveFilesButton.TabIndex = 5;
            this.MoveFilesButton.Text = "Sposta tutto";
            this.MoveFilesButton.UseVisualStyleBackColor = true;
            this.MoveFilesButton.Click += new System.EventHandler(this.MoveFilesButton_Click);
            // 
            // CopyFilesButton
            // 
            this.CopyFilesButton.Location = new System.Drawing.Point(570, 55);
            this.CopyFilesButton.Name = "CopyFilesButton";
            this.CopyFilesButton.Size = new System.Drawing.Size(98, 23);
            this.CopyFilesButton.TabIndex = 9;
            this.CopyFilesButton.Text = "Copia tutto";
            this.CopyFilesButton.UseVisualStyleBackColor = true;
            this.CopyFilesButton.Click += new System.EventHandler(this.CopyFilesButton_Click);
            // 
            // listBox2
            // 
            this.listBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.HorizontalScrollbar = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(533, 204);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(515, 259);
            this.listBox2.TabIndex = 10;
            // 
            // SelectBackupFolder
            // 
            this.SelectBackupFolder.Enabled = false;
            this.SelectBackupFolder.Location = new System.Drawing.Point(452, 135);
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
            this.BackupPath.Location = new System.Drawing.Point(12, 135);
            this.BackupPath.Name = "BackupPath";
            this.BackupPath.Size = new System.Drawing.Size(434, 23);
            this.BackupPath.TabIndex = 12;
            this.BackupPath.Text = "Percorso...";
            // 
            // CreateBackupButton
            // 
            this.CreateBackupButton.AutoSize = true;
            this.CreateBackupButton.Location = new System.Drawing.Point(674, 29);
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
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "Cartella da riordinare";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Cartella di destinazione";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "Cartella di backup";
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(570, 113);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(98, 23);
            this.UpdateButton.TabIndex = 20;
            this.UpdateButton.Text = "Aggiorna";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // DeleteBackups
            // 
            this.DeleteBackups.Location = new System.Drawing.Point(570, 84);
            this.DeleteBackups.Name = "DeleteBackups";
            this.DeleteBackups.Size = new System.Drawing.Size(98, 23);
            this.DeleteBackups.TabIndex = 21;
            this.DeleteBackups.Text = "Elimina backup";
            this.DeleteBackups.UseVisualStyleBackColor = true;
            this.DeleteBackups.Click += new System.EventHandler(this.DeleteBackups_Click);
            // 
            // Preview
            // 
            this.Preview.AutoSize = true;
            this.Preview.Location = new System.Drawing.Point(674, 58);
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
            this.STATE.Location = new System.Drawing.Point(12, 466);
            this.STATE.Name = "STATE";
            this.STATE.Size = new System.Drawing.Size(43, 15);
            this.STATE.TabIndex = 25;
            this.STATE.Text = "Pronto";
            // 
            // MODE
            // 
            this.MODE.AutoSize = true;
            this.MODE.Location = new System.Drawing.Point(12, 481);
            this.MODE.Name = "MODE";
            this.MODE.Size = new System.Drawing.Size(322, 15);
            this.MODE.TabIndex = 26;
            this.MODE.Text = "MODALITA\' NORMALE: Verranno apportate modifiche ai file";
            // 
            // Help
            // 
            this.Help.Location = new System.Drawing.Point(674, 84);
            this.Help.Name = "Help";
            this.Help.Size = new System.Drawing.Size(98, 23);
            this.Help.TabIndex = 27;
            this.Help.Text = "Aiuto";
            this.Help.UseVisualStyleBackColor = true;
            this.Help.Click += new System.EventHandler(this.Help_Click);
            // 
            // Form1
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 503);
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
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.CopyFilesButton);
            this.Controls.Add(this.MoveFilesButton);
            this.Controls.Add(this.SelectDestFolder);
            this.Controls.Add(this.DestPath);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.StartPath);
            this.Controls.Add(this.SelectStartFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "File Organizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button SelectStartFolder;
        private FolderBrowserDialog folderBrowserDialog1;
        private TextBox StartPath;
        private ListBox listBox1;
        private TextBox DestPath;
        private Button SelectDestFolder;
        private Button MoveFilesButton;
        private Button CopyFilesButton;
        private ListBox listBox2;
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
    }
}