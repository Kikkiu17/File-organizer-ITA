namespace File_Organizer
{
    public partial class Form1 : Form
    {
        bool exist_start_folder = false;
        bool exist_dest_folder = false;
        bool backups_active = false;
        bool dest_folder_exists = false;
        bool is_preview = false;

        string start_folder = "";
        string dest_folder = "";
        string backup_folder = "";

        public Form1()
        {
            InitializeComponent();

            //reset delle impostazioni in fase di debug
            if (System.Diagnostics.Debugger.IsAttached)
                Properties.Settings.Default.Reset();

            if ((bool)Properties.Settings.Default["FirstRun"] == true)
            {
                //First application run
                //Update setting
                Properties.Settings.Default["FirstRun"] = false;
                //Save setting
                Properties.Settings.Default.Save();
                //Create new instance of Dialog you want to show
                MessageBox.Show("Sembra che sia la prima volta che apri l'applicazione. Questa è una piccola guida al suo utilizzo.", "Benvenuto",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Information);
                MessageBox.Show("Seleziona una cartella da riordinare cliccando su \"Sfoglia...\". Poi, se vuoi cambiare la cartella in cui " +
                    "verranno spostati i file, basta cliccare sul tasto \"Sfoglia...\" Subito sotto.", "Benvenuto",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.None);
                MessageBox.Show("Poi, seleziona le impostazioni desiderate, e scegli se spostare o copiare tutto.\nPer ulteriori informazioni, " +
                    "puoi visitare la pagina di GitHub cliccando sul tasto \"Aiuto\".", "Benvenuto",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.None);

            }
        }

        private void UpdateList(int type)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            if(dest_folder == "")
            {
                MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            } else
            {
                int index = dest_folder.LastIndexOf("\\");
                string[] folders = Directory.GetDirectories(dest_folder[..index]);
                foreach (string folder in folders)
                {
                    if (folder == DestPath.Text)
                    {
                        dest_folder_exists = true;
                    }
                }

                foreach (string file in Directory.EnumerateFiles(StartPath.Text, "*"))
                {
                    listBox1.Items.Add(file.Split("\\")[file.Split("\\").Length-1]);
                }

                if (dest_folder_exists)
                {
                    string[] sub_folders = Directory.GetDirectories(DestPath.Text);
                    foreach (string folder in sub_folders)
                    {
                        listBox2.Items.Add(folder.Split("\\")[folder.Split("\\").Length-1]);
                    }
                    label2.Text = "Cartelle in "+DestPath.Text+":";
                }
                else
                {
                    label2.Text = "La cartella "+DestPath.Text+" non esiste ancora";
                }

                label1.Text = "Sono stati trovati "+listBox1.Items.Count.ToString()+" file in "+StartPath.Text;
            }
        }

        private void SelectStartFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                StartPath.Text = folderBrowserDialog1.SelectedPath;
                exist_start_folder = true;
                exist_dest_folder = true;
                DestPath.Text = folderBrowserDialog1.SelectedPath + "\\File Sistemati";
                BackupPath.Text = folderBrowserDialog1.SelectedPath + "\\Backups";
                start_folder = folderBrowserDialog1.SelectedPath;
                dest_folder = DestPath.Text;
                backup_folder = BackupPath.Text;
                STATE.Text = "Pronto";
                UpdateList(0);
            }
        }

        private void SelectDestFolder_Click(object sender, EventArgs e)
        {
            exist_dest_folder = true;
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DestPath.Text = folderBrowserDialog1.SelectedPath;
                dest_folder = folderBrowserDialog1.SelectedPath;
                STATE.Text = "Pronto";
                UpdateList(1);
            }
        }

        private void MoveFilesButton_Click(object sender, EventArgs e)
        {
            if (exist_dest_folder && exist_start_folder)
            {
                STATE.Text = "Operazione in corso...";
                foreach(Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                UpdateList(1);
                listBox2.Items.Clear();
                string[] file_list = new string[listBox1.Items.Count];
                listBox1.Items.CopyTo(file_list, 0);
                for(int i = 0; i < file_list.Length; i++)
                {
                    file_list[i] = start_folder+"\\"+file_list[i];
                }
                string[] operated_files = Algorithm.CopyMove(dest_folder, file_list, "MOVE", backups_active, backup_folder, is_preview);

                if(operated_files[0] != null)
                {
                    for (int i = 0; i < operated_files.Length-2; i++)
                    {
                        if(operated_files[i] != null)
                        {
                            listBox2.Items.Add(operated_files[i]);
                        }
                    }

                    int created_folders_num = int.Parse(operated_files[operated_files.Length-1]);
                    int ignored_files = int.Parse(operated_files[operated_files.Length-2]);
                    int moved_files = (operated_files.Length-2) - ignored_files;

                    if(is_preview)
                    {
                        label1.Text = "Verranno create "+created_folders_num+" cartelle";
                        label2.Text = "Verranno spostati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                    } else
                    {
                        label1.Text = "Sono state create "+created_folders_num+" cartelle";
                        label2.Text = "Sono stati spostati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                    }
                } else
                {
                    int created_folders_num = int.Parse(operated_files[operated_files.Length-1]);

                    if(is_preview)
                    {
                        label1.Text = "Verranno create "+created_folders_num+" cartelle";
                        label2.Text = "Non verrà spostato nessun file ("+operated_files[operated_files.Length-2]+" file ignorato/i)";
                        listBox2.Items.Add("Non verrà spostato nessun file");
                    } else
                    {
                        label1.Text = "Sono state create "+created_folders_num+" cartelle";
                        label2.Text = "Non è stato spostato nessun file ("+operated_files[operated_files.Length-2]+" file ignorato/i)";
                        listBox2.Items.Add("Non è stato spostato nessun file");
                    }
                }

                listBox1.Items.Clear();

                foreach (string file in Directory.EnumerateFiles(StartPath.Text, "*"))
                {
                    listBox1.Items.Add(file.Split("\\")[file.Split("\\").Length-1]);
                }

                foreach (Control c in this.Controls)
                {
                    c.Enabled = true;
                }

                STATE.Text = "Operazione completata";
            } else
            {
                MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            }
        }

        private void CopyFilesButton_Click(object sender, EventArgs e)
        {
            if (exist_dest_folder && exist_start_folder)
            {
                STATE.Text = "Operazione in corso...";
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                UpdateList(1);
                listBox2.Items.Clear();
                string[] file_list = new string[listBox1.Items.Count];
                listBox1.Items.CopyTo(file_list, 0);
                for (int i = 0; i < file_list.Length; i++)
                {
                    file_list[i] = start_folder+"\\"+file_list[i];
                }
                string[] operated_files = Algorithm.CopyMove(dest_folder, file_list, "COPY", backups_active, backup_folder, is_preview);

                if(operated_files[0] != null)
                {
                    for (int i = 0; i < operated_files.Length-2; i++)
                    {
                        if (operated_files[i] != null)
                        {
                            listBox2.Items.Add(operated_files[i]);
                        }
                    }

                    int created_folders_num = int.Parse(operated_files[operated_files.Length-1]);
                    int ignored_files = int.Parse(operated_files[operated_files.Length-2]);
                    int moved_files = (operated_files.Length-2) - ignored_files;

                    if(is_preview)
                    {
                        label1.Text = "Verranno create "+created_folders_num+" cartelle";
                        label2.Text = "Verranno copiati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                    } else
                    {
                        label1.Text = "Sono state create "+created_folders_num+" cartelle";
                        label2.Text = "Sono stati copiati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                    }
                } else
                {
                    int created_folders_num = int.Parse(operated_files[operated_files.Length-1]);

                    if(is_preview)
                    {
                        label1.Text = "Verranno create "+created_folders_num+" cartelle";
                        label2.Text = "Non verrà copiato nessun file ("+operated_files[operated_files.Length-2]+" file ignorato/i)";
                        listBox2.Items.Add("Non verrà copiato nessun file");
                    } else
                    {
                        label1.Text = "Sono state create "+created_folders_num+" cartelle";
                        label2.Text = "Non è stato copiato nessun file ("+operated_files[operated_files.Length-2]+" file ignorato/i)";
                        listBox2.Items.Add("Non è stato copiato nessun file");
                    }
                }

                listBox1.Items.Clear();

                foreach (string file in Directory.EnumerateFiles(StartPath.Text, "*"))
                {
                    listBox1.Items.Add(file.Split("\\")[file.Split("\\").Length-1]);
                }

                foreach (Control c in this.Controls)
                {
                    c.Enabled = true;
                }

                STATE.Text = "Operazione completata";
            } else
            {
                MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            }
        }

        private void CreateBackupButton_CheckedChanged(object sender, EventArgs e)
        {
            if(backups_active)
            {
                backups_active = false;
                BackupPath.Enabled = false;
                SelectBackupFolder.Enabled = false;
                CopyFilesButton.Enabled = true;
                STATE.Text = "Backup DISABILITATO";
            } else
            {
                backups_active = true;
                BackupPath.Enabled = true;
                SelectBackupFolder.Enabled = true;
                CopyFilesButton.Enabled = false;
                STATE.Text = "Backup ABILITATO";
            }
        }

        private void SelectBackupFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                BackupPath.Text = folderBrowserDialog1.SelectedPath;
                backup_folder = BackupPath.Text;
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdateList(1);
        }

        private string GetMessage()
        {
            string message = "Si sta per eliminare la seguente cartella: "+backup_folder+"\nContinuare?";
            return message;
        }

        private void DeleteBackups_Click(object sender, EventArgs e)
        {
            if(backups_active)
            {
                if(backup_folder == "")
                {
                    MessageBox.Show("Devi prima selezionare una cartella", "Attenzione",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Warning);
                } else
                {
                    bool exists = false;
                    int index = backup_folder.LastIndexOf("\\");
                    string[] folders = Directory.GetDirectories(backup_folder[..index]);
                    foreach (string folder in folders)
                    {
                        if (folder == backup_folder)
                        {
                            exists = true;
                        }
                    }

                    if (exists)
                    {
                        const string caption = "Attenzione";
                        var result = MessageBox.Show(GetMessage(), caption,
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            Directory.Delete(backup_folder, true);
                            MessageBox.Show("Cartella di backup eliminata", "Avviso",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("La cartella di backup da eliminare non esiste", "Errore",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error);
                    }
                }
            } else
            {
                MessageBox.Show("Devi prima abilitare il backup", "Attenzione",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Warning);
            }
        }

        private void Preview_CheckedChanged(object sender, EventArgs e)
        {
            if(is_preview)
            {
                is_preview = false;
                MODE.Text = "MODALITA' NORMALE: Verranno apportate modifiche ai file";
            } else
            {
                is_preview = true;
                if ((bool)Properties.Settings.Default["FirstPreview"] == true)
                {
                    //First application run
                    //Update setting
                    Properties.Settings.Default["FirstPreview"] = false;
                    //Save setting
                    Properties.Settings.Default.Save();
                    MessageBox.Show("MODALITA' ANTEPRIMA ABILITATA\nLa modalità anteprima permette di vedere in anteprima quali file saranno " +
                        "spostati / copiati e quante cartelle verranno create, senza toccare nulla.\n\nPer procedere, " +
                        "cliccare su \"sposta tutto\" o su \"copia tutto\".", "Informazioni",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Information);
                }
                    MODE.Text = "MODALITA' ANTEPRIMA: Non verranno apportate modifiche ai file";
            }
        }

        private void Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "https://github.com/Kikkiu17/File-organizer-ITA/wiki";
            process.Start();
        }
    }
}