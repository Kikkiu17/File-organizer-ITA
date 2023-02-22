namespace File_Organizer
{
    partial class IconsOrganizerWindow
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
            this.ConfirmLayout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConfirmLayout
            // 
            this.ConfirmLayout.Location = new System.Drawing.Point(12, 556);
            this.ConfirmLayout.Name = "ConfirmLayout";
            this.ConfirmLayout.Size = new System.Drawing.Size(138, 23);
            this.ConfirmLayout.TabIndex = 0;
            this.ConfirmLayout.Text = "Conferma layout";
            this.ConfirmLayout.UseVisualStyleBackColor = true;
            this.ConfirmLayout.Click += new System.EventHandler(this.ConfirmLayout_Click);
            // 
            // IconsOrganizerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 594);
            this.Controls.Add(this.ConfirmLayout);
            this.Name = "IconsOrganizerWindow";
            this.Text = "IconsOrganizerWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private Button ConfirmLayout;
    }
}