namespace File_Organizer
{
    partial class UndoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UndoForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.DoUndo = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.DontShowAgain = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vuoi davvero annullare l\'ultima operazione fatta?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Verranno fatte le seguenti modifiche:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(12, 42);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(524, 259);
            this.listBox1.TabIndex = 2;
            // 
            // DoUndo
            // 
            this.DoUndo.Location = new System.Drawing.Point(12, 307);
            this.DoUndo.Name = "DoUndo";
            this.DoUndo.Size = new System.Drawing.Size(98, 23);
            this.DoUndo.TabIndex = 30;
            this.DoUndo.Text = "Fai l\'undo";
            this.DoUndo.UseVisualStyleBackColor = true;
            this.DoUndo.Click += new System.EventHandler(this.DoUndo_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(116, 307);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(98, 23);
            this.Cancel.TabIndex = 31;
            this.Cancel.Text = "Annulla";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // DontShowAgain
            // 
            this.DontShowAgain.AutoSize = true;
            this.DontShowAgain.Location = new System.Drawing.Point(220, 310);
            this.DontShowAgain.Name = "DontShowAgain";
            this.DontShowAgain.Size = new System.Drawing.Size(119, 19);
            this.DontShowAgain.TabIndex = 32;
            this.DontShowAgain.Text = "Non mostrare più";
            this.DontShowAgain.UseVisualStyleBackColor = true;
            this.DontShowAgain.CheckedChanged += new System.EventHandler(this.DontShowAgain_CheckedChanged);
            // 
            // UndoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 338);
            this.Controls.Add(this.DontShowAgain);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.DoUndo);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UndoForm";
            this.Text = "Undo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private ListBox listBox1;
        private Button DoUndo;
        private Button Cancel;
        private CheckBox DontShowAgain;
    }
}