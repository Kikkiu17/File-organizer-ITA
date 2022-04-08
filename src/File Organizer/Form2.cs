namespace File_Organizer
{
    public partial class FileListOptionsForm : Form
    {
        public FileListOptionsForm()
        {
            InitializeComponent();

            if(MainWindow.start_folder != "")
            {
                listBox2.Items.Clear();
                foreach (string file in Directory.GetFiles(MainWindow.start_folder))
                {
                    string clean_file = file.Split("\\")[file.Split("\\").Length-1];

                    if(clean_file != "desktop.ini" && clean_file != Environment.ProcessPath)
                    {
                        listBox2.Items.Add(file.Split("\\")[file.Split("\\").Length-1]);
                    }
                }
            } else
            {
                listBox2.Items.Clear();
                listBox2.Items.Add("La cartella da riordinare non è stata selezionata");
            }
        }

        //evento di click del bottone salva
        private void FileListSaveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "File di testo (*.txt)|*.txt";
            saveFileDialog1.DefaultExt = "File di testo (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] arr = new string [listBox2.Items.Count];
                listBox2.Items.CopyTo(arr, 0);
                string to_write = "";
                for(int i = 0; i < arr.Length; i++)
                {
                    to_write = to_write + i + " " + arr[i] + "\n";
                }
                try
                {
                    File.WriteAllTextAsync(saveFileDialog1.FileName, to_write);
                    MessageBox.Show("Lista salvata in "+saveFileDialog1.FileName, "Informazione",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex)
                {
                    MessageBox.Show("Errore durante il salvataggio del file.\n\nErrore:\n"+ex.Message, "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //evento di click del bottone aggiorna
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (MainWindow.start_folder != "")
            {
                listBox2.Items.Clear();
                foreach (string file in Directory.GetFiles(MainWindow.start_folder))
                {
                    string clean_file = file.Split("\\")[file.Split("\\").Length-1];

                    if (clean_file != "desktop.ini" && clean_file != Environment.ProcessPath)
                    {
                        listBox2.Items.Add(file.Split("\\")[file.Split("\\").Length-1]);
                    }
                }
            }
            else
            {
                listBox2.Items.Clear();
                listBox2.Items.Add("La cartella da riordinare non è stata selezionata");
            }
        }
    }
}
