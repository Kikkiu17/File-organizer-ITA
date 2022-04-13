namespace File_Organizer
{
    partial class FileManagerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileManagerWindow));
            this.TotalFiles = new System.Windows.Forms.Label();
            this.ViewType = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cartella = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalFolders = new System.Windows.Forms.Label();
            this.TotalSize = new System.Windows.Forms.Label();
            this.SelectedItems = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectAll = new System.Windows.Forms.CheckBox();
            this.DeleteSelectedFiles = new System.Windows.Forms.Button();
            this.SelectedSize = new System.Windows.Forms.Label();
            this.OpenFile = new System.Windows.Forms.Button();
            this.OpenFilePath = new System.Windows.Forms.Button();
            this.CurrentFolder = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchInput = new System.Windows.Forms.TextBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // TotalFiles
            // 
            this.TotalFiles.AutoSize = true;
            this.TotalFiles.Location = new System.Drawing.Point(12, 403);
            this.TotalFiles.Name = "TotalFiles";
            this.TotalFiles.Size = new System.Drawing.Size(84, 15);
            this.TotalFiles.TabIndex = 2;
            this.TotalFiles.Text = "Caricamento...";
            // 
            // ViewType
            // 
            this.ViewType.Location = new System.Drawing.Point(0, 0);
            this.ViewType.Name = "ViewType";
            this.ViewType.Size = new System.Drawing.Size(100, 23);
            this.ViewType.TabIndex = 35;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Cartella,
            this.Column2});
            this.dataGridView1.Location = new System.Drawing.Point(12, 37);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(694, 328);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyUp);
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "File";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 400;
            // 
            // Cartella
            // 
            this.Cartella.HeaderText = "Cartella";
            this.Cartella.Name = "Cartella";
            this.Cartella.ReadOnly = true;
            this.Cartella.Width = 158;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Dimensione";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 75;
            // 
            // TotalFolders
            // 
            this.TotalFolders.AutoSize = true;
            this.TotalFolders.Location = new System.Drawing.Point(12, 388);
            this.TotalFolders.Name = "TotalFolders";
            this.TotalFolders.Size = new System.Drawing.Size(84, 15);
            this.TotalFolders.TabIndex = 5;
            this.TotalFolders.Text = "Caricamento...";
            // 
            // TotalSize
            // 
            this.TotalSize.AutoSize = true;
            this.TotalSize.Location = new System.Drawing.Point(12, 418);
            this.TotalSize.Name = "TotalSize";
            this.TotalSize.Size = new System.Drawing.Size(84, 15);
            this.TotalSize.TabIndex = 6;
            this.TotalSize.Text = "Caricamento...";
            // 
            // SelectedItems
            // 
            this.SelectedItems.AutoSize = true;
            this.SelectedItems.Location = new System.Drawing.Point(12, 482);
            this.SelectedItems.Name = "SelectedItems";
            this.SelectedItems.Size = new System.Drawing.Size(161, 15);
            this.SelectedItems.TabIndex = 7;
            this.SelectedItems.Text = "Nessun elemento selezionato";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 467);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "File selezionati:";
            // 
            // SelectAll
            // 
            this.SelectAll.AutoSize = true;
            this.SelectAll.Location = new System.Drawing.Point(12, 12);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(104, 19);
            this.SelectAll.TabIndex = 9;
            this.SelectAll.Text = "Seleziona tutto";
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.CheckedChanged += new System.EventHandler(this.SelectAll_CheckedChanged);
            // 
            // DeleteSelectedFiles
            // 
            this.DeleteSelectedFiles.Location = new System.Drawing.Point(552, 433);
            this.DeleteSelectedFiles.Name = "DeleteSelectedFiles";
            this.DeleteSelectedFiles.Size = new System.Drawing.Size(156, 23);
            this.DeleteSelectedFiles.TabIndex = 33;
            this.DeleteSelectedFiles.Text = "Elimina file selezionati";
            this.DeleteSelectedFiles.UseVisualStyleBackColor = true;
            this.DeleteSelectedFiles.Click += new System.EventHandler(this.DeleteSelectedFiles_Click);
            // 
            // SelectedSize
            // 
            this.SelectedSize.AutoSize = true;
            this.SelectedSize.Location = new System.Drawing.Point(12, 433);
            this.SelectedSize.Name = "SelectedSize";
            this.SelectedSize.Size = new System.Drawing.Size(140, 15);
            this.SelectedSize.TabIndex = 34;
            this.SelectedSize.Text = "Dimensioni selezione: 0 B";
            // 
            // OpenFile
            // 
            this.OpenFile.Location = new System.Drawing.Point(552, 375);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(156, 23);
            this.OpenFile.TabIndex = 36;
            this.OpenFile.Text = "Apri file";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // OpenFilePath
            // 
            this.OpenFilePath.Location = new System.Drawing.Point(552, 404);
            this.OpenFilePath.Name = "OpenFilePath";
            this.OpenFilePath.Size = new System.Drawing.Size(156, 23);
            this.OpenFilePath.TabIndex = 37;
            this.OpenFilePath.Text = "Apri percorso file";
            this.OpenFilePath.UseVisualStyleBackColor = true;
            this.OpenFilePath.Click += new System.EventHandler(this.OpenFilePath_Click);
            // 
            // CurrentFolder
            // 
            this.CurrentFolder.AutoSize = true;
            this.CurrentFolder.Location = new System.Drawing.Point(12, 373);
            this.CurrentFolder.Name = "CurrentFolder";
            this.CurrentFolder.Size = new System.Drawing.Size(84, 15);
            this.CurrentFolder.TabIndex = 38;
            this.CurrentFolder.Text = "Caricamento...";
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(652, 7);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(24, 24);
            this.SearchButton.TabIndex = 39;
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchInput
            // 
            this.SearchInput.Location = new System.Drawing.Point(122, 8);
            this.SearchInput.Name = "SearchInput";
            this.SearchInput.Size = new System.Drawing.Size(524, 23);
            this.SearchInput.TabIndex = 40;
            this.SearchInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchInput_KeyDown);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(682, 6);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(24, 24);
            this.RefreshButton.TabIndex = 41;
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // FileManagerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 506);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SearchInput);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.CurrentFolder);
            this.Controls.Add(this.OpenFilePath);
            this.Controls.Add(this.OpenFile);
            this.Controls.Add(this.SelectedSize);
            this.Controls.Add(this.DeleteSelectedFiles);
            this.Controls.Add(this.SelectAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectedItems);
            this.Controls.Add(this.TotalSize);
            this.Controls.Add(this.TotalFolders);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ViewType);
            this.Controls.Add(this.TotalFiles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileManagerWindow";
            this.Text = "Gestore file";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label TotalFiles;
        private Label ViewType;
        private DataGridView dataGridView1;
        private Label TotalFolders;
        private Label TotalSize;
        private Label SelectedItems;
        private Label label1;
        private CheckBox SelectAll;
        private Button DeleteSelectedFiles;
        private Label SelectedSize;
        private Button OpenFile;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Cartella;
        private DataGridViewTextBoxColumn Column2;
        private Button OpenFilePath;
        private Label CurrentFolder;
        private Button SearchButton;
        private TextBox SearchInput;
        private Button RefreshButton;
    }
}