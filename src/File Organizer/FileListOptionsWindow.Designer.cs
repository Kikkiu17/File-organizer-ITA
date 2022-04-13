namespace File_Organizer
{
    partial class FileListOptionsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileListOptionsWindow));
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.FileListSaveButton = new System.Windows.Forms.Button();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.HorizontalScrollbar = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(12, 12);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(515, 259);
            this.listBox2.TabIndex = 3;
            // 
            // FileListSaveButton
            // 
            this.FileListSaveButton.Location = new System.Drawing.Point(12, 277);
            this.FileListSaveButton.Name = "FileListSaveButton";
            this.FileListSaveButton.Size = new System.Drawing.Size(98, 23);
            this.FileListSaveButton.TabIndex = 21;
            this.FileListSaveButton.Text = "Salva lista";
            this.FileListSaveButton.UseVisualStyleBackColor = true;
            this.FileListSaveButton.Click += new System.EventHandler(this.FileListSaveButton_Click);
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(116, 277);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(98, 23);
            this.UpdateButton.TabIndex = 22;
            this.UpdateButton.Text = "Aggiorna";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(429, 277);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(98, 23);
            this.Close.TabIndex = 23;
            this.Close.Text = "Chiudi";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // FileListOptionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 308);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.FileListSaveButton);
            this.Controls.Add(this.listBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FileListOptionsWindow";
            this.Text = "Opzioni lista";
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox listBox2;
        private Button FileListSaveButton;
        private Button UpdateButton;
        private new Button Close;
        private SaveFileDialog saveFileDialog1;
    }
}