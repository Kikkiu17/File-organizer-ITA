using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Collections;
using System.Net;
using System.Text;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.Notifications;

namespace File_Organizer
{
    public partial class MainWindow : Form
    {
        bool exist_start_folder = false;
        bool exist_dest_folder = false;
        bool backups_active = false;
        bool is_preview = false;

        public static string start_folder = "";
        public static string operation = "";
        public static string dest_folder = "";
        public static bool undodone = false;
        string backup_folder = "";
        string get_version = "";
        private int optype = 0;
        bool isoperating = false;
        bool destfolder_locked = false;

        private ContextMenuStrip listboxContextMenu;

        private Algorithm _alg = new Algorithm();
        public MainWindow()
        {
            InitializeComponent();

            _alg.OnProgressUpdate += t1_OnProgressUpdate;

            //reset delle impostazioni in fase di debug
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Properties.Settings.Default.Reset();
                Preview.Checked = true;
            }

            //controlla se c'è un nuovo aggiornamento
            string urlAddress = "https://github.com/Kikkiu17/File-organizer-ITA/tags";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //scarica l'aggiornamento e avvia il nuovo programma
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader? readStream = null;
                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream,
                        Encoding.GetEncoding(response.CharacterSet));
                get_version = readStream.ReadToEnd().Split("/Kikkiu17/File-organizer-ITA/releases/tag/v")[1].Split("\"")[0];
                int get_raw_version = int.Parse(get_version.Replace(".", ""));
                string currname = Environment.CurrentDirectory + "\\" + Process.GetCurrentProcess().ProcessName+".exe";
                int curr_version = int.Parse(FileVersionInfo.GetVersionInfo(currname).ProductVersion.Replace(".", ""));
                response.Close();
                readStream.Close();

                if (curr_version < get_raw_version)
                {
                    var diagresult = MessageBox.Show("C'è una nuova versione del programma. Vuoi scaricarla e avviarla?", "Nuova versione",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Information);

                    if (diagresult == DialogResult.Yes)
                    {
                        ShowNotification(2, "Download dell'aggiornamento in corso", "potrebbe volerci un po' di tempo...");
                        string path = Path.GetDirectoryName(Application.ExecutablePath);
                        if (!File.Exists(path+"\\"+"Organizer-v"+get_version+".exe"))
                        {
                            using (var client = new WebClient())
                            {
                                client.DownloadFile("https://github.com/Kikkiu17/File-organizer-ITA/releases" +
                                    "/download/v"+get_version+"/Organizer-v"+get_version+".exe", path+"\\"+"Organizer-v"+get_version+".exe");
                            }
                        }

                        ShowNotification(2, "Download completato", "apertura della nuova applicazione...");

                        System.Diagnostics.Process.Start(path+"\\"+"Organizer-v"+get_version+".exe");
                        if (System.Windows.Forms.Application.MessageLoop)
                        {
                            // WinForms app
                            System.Windows.Forms.Application.Exit();
                        }
                        else
                        {
                            // Console app
                            System.Environment.Exit(1);
                        }
                    }
                }
            }

            //controlla se è il primo avvio
            if ((bool)Properties.Settings.Default["FirstRun"] == true)
            {
                Properties.Settings.Default["FirstRun"] = false;
                Properties.Settings.Default.Save();
                MessageBox.Show("Sembra che sia la prima volta che apri l'applicazione. Questa è una piccola introduzione" +
                    " al suo utilizzo.", "Benvenuto",
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

            //crea un contextMenu per la listbox
            listboxContextMenu = new ContextMenuStrip();
            listboxContextMenu.Opening +=new CancelEventHandler(listboxContextMenu_Opening);
            listBox2.ContextMenuStrip = listboxContextMenu;
            listboxContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(contexMenu_ItemClicked);

        }

        private void listboxContextMenu_Opening(object sender, CancelEventArgs e)
        {
            listboxContextMenu.Items.Clear();
            listboxContextMenu.Items.Add("Apri");
            listboxContextMenu.Items.Add("Rinomina");
            listboxContextMenu.Items.Add("Elimina");
            listboxContextMenu.Items.Add("Tipi di file collegati");
        }

        //evento di click del bottone della cartella iniziale
        private void SelectStartFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((bool)Properties.Settings.Default["FirstFolderSelect"] == true)
                {
                    Properties.Settings.Default["FirstFolderSelect"] = false;
                    Properties.Settings.Default.Save();

                    MessageBox.Show("Una volta che avrai spostato o copiato i primi file, cliccando \"aggiorna\" o riaprendo il programma," +
                        " potrai modificare il nome delle cartelle di destinazione cliccandoci sopra col tasto destro nella lista di destra.",
                        "Informazione",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                StartPath.Text = folderBrowserDialog1.SelectedPath;
                exist_start_folder = true;
                exist_dest_folder = true;
                string checkeddestfolder = CheckDestPath();
                if(checkeddestfolder != "")
                {
                    if(!destfolder_locked)
                    {
                        DestPath.Text = checkeddestfolder;
                    }
                } else
                {
                    if(!destfolder_locked)
                    {
                        DestPath.Text = folderBrowserDialog1.SelectedPath + "\\File Sistemati";
                    }
                }
                BackupPath.Text = folderBrowserDialog1.SelectedPath + "\\Backups";
                start_folder = folderBrowserDialog1.SelectedPath;
                dest_folder = DestPath.Text;
                backup_folder = BackupPath.Text;
                STATE.Text = "Pronto";
                BackgroundUpdate.RunWorkerAsync(argument: 0);
            }
        }

        private string CheckDestPath()
        {
            LockFolderCheckBox.Enabled = true;
            string destfolder = "";
            foreach (string folder in Directory.GetDirectories(folderBrowserDialog1.SelectedPath))
            {
                try
                {
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        if (file.Split("\\")[file.Split("\\").Length-1] == ".fileorg")
                        {
                            destfolder = folder;
                            break;
                        }
                    }
                } catch
                {
                    continue;
                }
            }

            return destfolder;
        }

        //evento di click del bottone della cartella finale
        private void SelectDestFolder_Click(object sender, EventArgs e)
        {
            LockFolderCheckBox.Enabled = true;
            exist_dest_folder = true;
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DestPath.Text = folderBrowserDialog1.SelectedPath;
                dest_folder = folderBrowserDialog1.SelectedPath;
                STATE.Text = "Pronto";
                BackgroundUpdate.RunWorkerAsync(argument: 1);
            }
        }

        //evento di click del bottone dello spostamento dei file
        private void MoveFilesButton_Click(object sender, EventArgs e)
        {
            if (exist_dest_folder && exist_start_folder)
            {
                if(dest_folder != start_folder)
                {
                    if (!is_preview)
                    {
                        string backup_string = "";
                        if (backups_active)
                        {
                            backup_string = " e backup";
                        }
                        ShowNotification(2, "Spostamento"+backup_string+" dei file in corso", "potrebbe volerci un po' di tempo...");
                    }

                    STATE.Text = "Operazione in corso...";
                    foreach (Control c in this.Controls)
                    {
                        c.Enabled = false;
                    }

                    Help.Enabled = true;
                    isoperating = true;

                    BackgroundUpdate.RunWorkerAsync(argument: 2);
                    optype = 100;
                } else
                {
                    MessageBox.Show("La cartella di destinazione e quella da riordinare non possono essere uguali.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            }
        }

        //evento di click del bottone della copia dei file
        private void CopyFilesButton_Click(object sender, EventArgs e)
        {
            if (exist_dest_folder && exist_start_folder)
            {
                if(dest_folder != start_folder)
                {
                    if (!is_preview)
                    {
                        ShowNotification(2, "Copia dei file in corso", "potrebbe volerci un po' di tempo...");
                    }

                    STATE.Text = "Operazione in corso...";
                    foreach (Control c in this.Controls)
                    {
                        c.Enabled = false;
                    }

                    Help.Enabled = true;
                    isoperating = true;

                    BackgroundUpdate.RunWorkerAsync(argument: 2);
                    optype = 200;
                } else
                {
                    MessageBox.Show("La cartella di destinazione e quella da riordinare non possono essere uguali.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
            } else
            {
                MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            }
        }

        //evento di click del bottone per la creazione di backup
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

        //evento di click del bottone per selezionare la cartella di backup
        private void SelectBackupFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                BackupPath.Text = folderBrowserDialog1.SelectedPath;
                backup_folder = BackupPath.Text;
            }
        }

        //evento di click del bottone per aggiornare la lista di file
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            BackgroundUpdate.RunWorkerAsync(argument: 1);
        }

        //messaggio costante per la messagebox
        private string GetMessage()
        {
            string message = "Si sta per eliminare la seguente cartella: "+backup_folder+"\nContinuare?";
            return message;
        }

        //evento di click del bottone per eliminare la cartella di backup
        private void DeleteBackups_Click(object sender, EventArgs e)
        {
            if(backups_active)
            {
                if(backup_folder == "")
                {
                    MessageBox.Show("Devi prima selezionare una cartella.", "Attenzione",
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
                            MessageBox.Show("Cartella di backup eliminata.", "Avviso",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Information);
                        }

                        STATE.Text = "Operazione completata";
                    }
                    else
                    {
                        MessageBox.Show("La cartella di backup da eliminare non esiste.", "Errore",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error);
                    }
                }
            } else
            {
                MessageBox.Show("Devi prima abilitare il backup.", "Attenzione",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Warning);
            }
        }

        //evento di click del bottone della modalità anteprima
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

        //evento di click del bottone di aiuto
        private void Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "https://github.com/Kikkiu17/File-organizer-ITA/wiki";
            process.Start();
        }

        //evento di click del tasto destro del mouse sulla listbox
        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if(listBox2.SelectedItem != null && listBox2.SelectedItem.ToString() != "")
            {
                int location = listBox2.IndexFromPoint(e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    listBox2.SelectedIndex = location;                //Index selected
                    listboxContextMenu.Show(PointToScreen(e.Location));
                }
            }
        }

        //evento di click di un'opzione del menu di tasto destro
        private void contexMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string selected_item = item.Text;
            if (listBox2.SelectedItem != null && !listBox2.SelectedItem.ToString().Contains("spostato") &&
                !listBox2.SelectedItem.ToString().Contains("copiato"))
            {
                string folder = listBox2.SelectedItem.ToString();
                listboxContextMenu.Hide();

                if (selected_item == "Elimina")
                {
                    var result = MessageBox.Show("Verranno eliminati TUTTI i contenuti della cartella e la cartella stessa.\nProcedere?", "Attenzione",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        Directory.Delete(dest_folder+"\\"+folder, true);
                        BackgroundUpdate.RunWorkerAsync(argument: 3);
                        MessageBox.Show("Cartella eliminata.", "Avviso",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Information);
                    }
                }
                else if (selected_item == "Rinomina")
                {
                    var result = Interaction.InputBox("Rinomina la cartella selezionata.\nIl nome non può contenere i seguenti caratteri:" +
                        "\n\\ / : * ? \" < > |", "Rinomina", listBox2.SelectedItem.ToString());

                    if (result != "")
                    {
                        try
                        {
                            Directory.Move(dest_folder+"\\"+folder, dest_folder+"\\"+result);
                            BackgroundUpdate.RunWorkerAsync(argument: 3);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Non è stato possibile rinominare la cartella. Forse ne esiste già una con lo stesso nome, oppure ci" +
                                " sono caratteri non consentiti?" +
                                "\n\nErrore:\n"+ex.Message, "Errore",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else if (selected_item == "Tipi di file collegati")
                {
                    string file_types_raw = "";
                    ArrayList old_file_types = new ArrayList();
                    foreach (string file in Directory.GetFiles(dest_folder+"\\"+folder))
                    {
                        string f = file.Split("\\")[file.Split("\\").Length-1];
                        if (f.StartsWith("."))
                        {
                            file_types_raw = file_types_raw + f + ", ";
                            old_file_types.Add(f);
                        }
                    }

                    int index = file_types_raw.LastIndexOf(", ");
                    string file_types = file_types_raw[..index];

                    var result = Interaction.InputBox("Cambia i tipi di file collegati. Separa le estensioni da una virgola" +
                        " E uno spazio.\nQueste sono le estensioni collegate:\n"+file_types,
                        "Tipi di file collegati", file_types);

                    if (result != "")
                    {
                        ArrayList new_file_types = new ArrayList();
                        ArrayList f_types_folders = new ArrayList();
                        string existing_f_types = "";
                        string f_types = "";
                        foreach (string f_type in result.Split(", "))
                        {
                            new_file_types.Add(f_type);
                            f_types = f_types + f_type + " ";
                        }
                        foreach (string subfolder in Directory.GetDirectories(dest_folder))
                        {
                            foreach(string file_type in new_file_types)
                            {
                                if(!old_file_types.Contains(file_type))
                                {
                                    if (File.Exists(subfolder+"\\"+file_type))
                                    {
                                        if (existing_f_types == "")
                                        {
                                            existing_f_types = file_type;
                                        }
                                        else
                                        {
                                            existing_f_types = existing_f_types + ", " + file_type;
                                        }
                                        f_types_folders.Add(subfolder);
                                    }
                                }
                            }
                        }

                        if(existing_f_types != "")
                        {
                            var msgboxresult = MessageBox.Show("I file con estensione "+existing_f_types+" sono già associati ad un'altra cartella." +
                                " Spostare i file dall'altra cartella a quella attuale?", "Attenzione",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if(msgboxresult == DialogResult.Yes)
                            {
                                foreach(string subfolder in f_types_folders)
                                {
                                    foreach(string file in Directory.GetFiles(subfolder))
                                    {
                                        string filename = file.Split("\\")[file.Split("\\").Length-1];
                                        if (!filename.StartsWith("."))
                                        {
                                            File.Move(file, dest_folder+"\\"+folder+"\\"+filename);
                                        }
                                    }

                                    Directory.Delete(subfolder, true);
                                }
                            }
                        }

                        foreach (string file_type in old_file_types)
                        {
                            File.Delete(dest_folder+"\\"+folder+"\\"+file_type);
                        }

                        foreach (string file_type in new_file_types)
                        {
                            File.WriteAllTextAsync(dest_folder+"\\"+folder+"\\"+file_type, file_type);
                            File.SetAttributes(dest_folder+"\\"+folder+"\\"+file_type, FileAttributes.Hidden);
                        }

                        MessageBox.Show("File collegati: "+result, "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        BackgroundUpdate.RunWorkerAsync(argument: 1);
                    }
                } else if(selected_item == "Apri")
                {
                    if(listBox2.SelectedItem != null && listBox2.SelectedItem.ToString() != "")
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.Arguments = dest_folder+"\\"+listBox2.SelectedItem.ToString();
                        startInfo.FileName = "explorer.exe";

                        Process.Start(startInfo);
                    }
                }
            }
        }

        //evento di click del bottone delle opzioni della lista di file
        private void FileListOptions_Click(object sender, EventArgs e)
        {
            new FileListOptionsForm().Show();
        }

        //evento di update da parte di un thread
        private void t1_OnProgressUpdate(int value)
        {
            base.Invoke((Action)delegate
            {
                progressBar1.Value = value;
            });
        }

        //evento di work del backgroundworker (sposta file)
        private void DoMove(object sender, DoWorkEventArgs e)
        {
            string[] file_list = new string[listBox1.Items.Count];
            listBox1.Items.CopyTo(file_list, 0);
            for (int i = 0; i < file_list.Length; i++)
            {
                file_list[i] = start_folder+"\\"+file_list[i];
            }

            var tuples = _alg.CopyMove(dest_folder, file_list, "MOVE", backups_active, backup_folder, is_preview);

            string[] operated_files_names = tuples.Item1.Split(";");
            int ignored_files = tuples.Item2;
            int created_folders = tuples.Item3;
            string created_folders_names = tuples.Item4;

            listBox1.Items.Clear();
            foreach (string created_folder in created_folders_names.Split(";"))
            {
                if (created_folder != "")
                {
                    listBox1.Items.Add(created_folder);
                }
            }

            if (operated_files_names[0] != null)
            {
                listBox2.Items.Clear();

                for (int i = 0; i < operated_files_names.Length; i++)
                {
                    if (operated_files_names[i] != null)
                    {
                        listBox2.Items.Add(operated_files_names[i]);
                    }
                }

                int moved_files = operated_files_names.Length-1;

                if (moved_files < 0)
                {
                    moved_files = 0;
                }

                if (is_preview)
                {
                    var x = (created_folders == 1) ? label1.Text = "Verrà creata 1" +
                        " cartella" : label1.Text = "Verranno create "+created_folders+" cartelle";

                    x = (moved_files == 1) ? label2.Text = "Verrà spostato 1 file ("+ignored_files+
                        " file ignorato/i)" : label2.Text = label2.Text =
                        "Verranno spostati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                }
                else
                {
                    var x = (created_folders == 1) ? label1.Text = "È stata creata 1" +
                        " cartella" : label1.Text = "Sono state create "+created_folders+" cartelle";

                    x = (moved_files == 1) ? label2.Text = "È stato spostato 1 file ("+ignored_files+
                        " file ignorato/i)" : label2.Text = label2.Text =
                        "Sono stati spostati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                }
            }
            else
            {
                if (is_preview)
                {
                    var x = (created_folders == 1) ? label1.Text = "Verrà creata 1" +
                        " cartella" : label1.Text = "Verranno create "+created_folders+" cartelle";

                    label2.Text = "Non verrà spostato nessun file ("+ignored_files+" file ignorato/i)";
                    listBox2.Items.Clear();
                    listBox2.Items.Add("Non verrà spostato nessun file");
                }
                else
                {
                    var x = (created_folders == 1) ? label1.Text = "È stata creata 1" +
                        " cartella" : label1.Text = "Sono state create "+created_folders+" cartelle";

                    label1.Text = "Sono state create "+created_folders+" cartelle";
                    label2.Text = "Non è stato spostato nessun file ("+ignored_files+" file ignorato/i)";
                    listBox2.Items.Clear();
                    listBox2.Items.Add("Non è stato spostato nessun file");
                }
            }

            if (is_preview)
            {
                foreach (string created_folder in created_folders_names.Split(";"))
                {
                    if (created_folder != "")
                    {
                        Directory.Delete(created_folder, true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    operation = operation + listBox2.Items[i].ToString() + ";";
                }

                ShowNotification(1, "Spostamento dei file completato", "");
            }

            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }

            if (backups_active)
            {
                BackupPath.Enabled = true;
                SelectBackupFolder.Enabled = true;
                CopyFilesButton.Enabled = false;
            }
            else
            {
                BackupPath.Enabled = false;
                SelectBackupFolder.Enabled = false;
                CopyFilesButton.Enabled = true;
            }

            if (destfolder_locked)
            {
                SelectDestFolder.Enabled = false;
                DestPath.Enabled = false;
            }

            STATE.Text = "Operazione completata";

            if (!is_preview)
            {
                if ((bool)Properties.Settings.Default["FirstOp"] == true)
                {
                    Properties.Settings.Default["FirstOp"] = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("È stato creato un file \".fileorg\" nella cartella di destinazione principale. Serve al programma" +
                        " a riconoscere quest'ultima, quindi non rimuoverla.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //evento di work del backgroundworker (copia file)
        private void DoCopy(object sender, DoWorkEventArgs e)
        {
            string[] file_list = new string[listBox1.Items.Count];
            listBox1.Items.CopyTo(file_list, 0);
            for (int i = 0; i < file_list.Length; i++)
            {
                file_list[i] = start_folder+"\\"+file_list[i];
            }
            var tuples = _alg.CopyMove(dest_folder, file_list, "COPY", backups_active, backup_folder, is_preview);

            string[] operated_files_names = tuples.Item1.Split(";");
            int ignored_files = tuples.Item2;
            int created_folders = tuples.Item3;
            string created_folders_names = tuples.Item4;

            listBox1.Items.Clear();
            foreach (string created_folder in created_folders_names.Split(";"))
            {
                if (created_folder != "")
                {
                    listBox1.Items.Add(created_folder);
                }
            }

            if (operated_files_names[0] != null)
            {
                listBox2.Items.Clear();

                for (int i = 0; i < operated_files_names.Length; i++)
                {
                    if (operated_files_names[i] != null)
                    {
                        listBox2.Items.Add(operated_files_names[i]);
                    }
                }

                int moved_files = operated_files_names.Length-1;

                if(moved_files < 0)
                {
                    moved_files = 0;
                }

                if (is_preview)
                {
                    var x = (created_folders == 1) ? label1.Text = "Verrà creata 1" +
                        " cartella" : label1.Text = "Verranno create "+created_folders+" cartelle";

                    x = (moved_files == 1) ? label2.Text = "Verrà copiato 1 file ("+ignored_files+
                        " file ignorato/i)" : label2.Text = label2.Text =
                        "Verranno copiati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                }
                else
                {
                    var x = (created_folders == 1) ? label1.Text = "È stata creata 1" +
                        " cartella" : label1.Text = "Sono state create "+created_folders+" cartelle";

                    x = (moved_files == 1) ? label2.Text = "È stato copiato 1 file ("+ignored_files+
                        " file ignorato/i)" : label2.Text = label2.Text =
                        "Sono stati copiati "+moved_files+" file ("+ignored_files+" file ignorato/i)";
                }
            }
            else
            {
                if (is_preview)
                {
                    var x = (created_folders == 1) ? label1.Text = "Verrà creata 1" +
                        " cartella" : label1.Text = "Verranno create "+created_folders+" cartelle";

                    label2.Text = "Non verrà copiato nessun file ("+ignored_files+" file ignorato/i)";
                    listBox2.Items.Clear();
                    listBox2.Items.Add("Non verrà copiato nessun file");
                }
                else
                {
                    var x = (created_folders == 1) ? label1.Text = "È stata creata 1" +
                        " cartella" : label1.Text = "Sono state create "+created_folders+" cartelle";

                    label1.Text = "Sono state create "+created_folders+" cartelle";
                    label2.Text = "Non è stato copiato nessun file ("+ignored_files+" file ignorato/i)";
                    listBox2.Items.Clear();
                    listBox2.Items.Add("Non è stato copiato nessun file");
                }
            }

            if (is_preview)
            {
                foreach (string created_folder in created_folders_names.Split(";"))
                {
                    if (created_folder != "")
                    {
                        Directory.Delete(created_folder, true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    operation = operation + listBox2.Items[i].ToString() + ";";
                }

                ShowNotification(1, "Copia dei file completata", "");
            }

            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }

            if (backups_active)
            {
                BackupPath.Enabled = true;
                SelectBackupFolder.Enabled = true;
                CopyFilesButton.Enabled = false;
            }
            else
            {
                BackupPath.Enabled = false;
                SelectBackupFolder.Enabled = false;
                CopyFilesButton.Enabled = true;
            }

            if (destfolder_locked)
            {
                SelectDestFolder.Enabled = false;
                DestPath.Enabled = false;
            }

            STATE.Text = "Operazione completata";

            if (!is_preview)
            {
                if ((bool)Properties.Settings.Default["FirstOp"] == true)
                {
                    Properties.Settings.Default["FirstOp"] = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("È stato creato un file \".fileorg\" nella cartella di destinazione principale. Serve al programma" +
                        " a riconoscere quest'ultima, quindi non rimuoverla.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //evento di work del backgroundworker (aggiorna lista)
        private void DoUpdate(object sender, DoWorkEventArgs e)
        {
            int type = (int)e.Argument;

            if (backups_active)
            {
                BackupPath.Enabled = true;
                SelectBackupFolder.Enabled = true;
                CopyFilesButton.Enabled = false;
            }
            else
            {
                BackupPath.Enabled = false;
                SelectBackupFolder.Enabled = false;
                if(!isoperating)
                {
                    CopyFilesButton.Enabled = true;
                }
            }

            progressBar1.Value = 0;

            if (type != 3)
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();

                if (dest_folder == "")
                {
                    MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Error);
                }
                else
                {

                    if(start_folder != "")
                    {
                        int i = 0;
                        foreach (string file in Directory.EnumerateFiles(StartPath.Text, "*"))
                        {
                            string clean_file = file.Split("\\")[file.Split("\\").Length-1];
                            if (clean_file != "desktop.ini" && clean_file != Environment.ProcessPath.Split("\\")
                                [Environment.ProcessPath.Split("\\").Length-1])
                            {
                                i++;
                                listBox1.Items.Add(file.Split("\\")[file.Split("\\").Length-1]);
                            }
                        }
                    }

                    if (type != 2)
                    {
                        if (Directory.Exists(dest_folder))
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

                        var x = (listBox1.Items.Count == 1) ? label1.Text =
                            "È stato trovato "+listBox1.Items.Count.ToString()+" file in "+StartPath.Text : label1.Text =
                            "Sono stati trovati "+listBox1.Items.Count.ToString()+" file in "+StartPath.Text;
                    }
                }
            }
            else
            {
                listBox2.Items.Clear();
                if (Directory.Exists(dest_folder))
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

                var x = (listBox1.Items.Count == 1) ? label1.Text =
                    "È stato trovato "+listBox1.Items.Count.ToString()+" file in "+StartPath.Text : label1.Text =
                    "Sono stati trovati "+listBox1.Items.Count.ToString()+" file in "+StartPath.Text;
            }
        }

        //evento dii completamento dell'aggiornamento della lista
        private void UpdateComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if(optype == 100)
            {
                optype = 0;
                listBox2.Items.Clear();
                operation = "";
                backgroundWorker1.RunWorkerAsync();
            } else if(optype == 200)
            {
                optype = 0;
                listBox2.Items.Clear();
                operation = "";
                backgroundWorker2.RunWorkerAsync();
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (operation != "")
            {
                if ((bool)Properties.Settings.Default["DontShowUndoAgain"] == false)
                {
                    new UndoForm().Show();
                }
                else
                {

                    for (int i = 0; i < operation.Split(";").Length; i++)
                    {
                        if (operation.Split(";")[i] != "" && operation.Split(";")[i] != null)
                        {
                            if (operation.Split(";")[i].Contains("spostato in"))
                            {
                                string folder = operation.Split(";")[i].Split(" spostato in ")[1];
                                string file = operation.Split(";")[i].Split(" spostato in ")[0];

                                Directory.Move(folder+"\\"+file, start_folder+"\\"+file);
                            }
                            else if (operation.Split(";")[i].Contains("copiato in"))
                            {
                                string folder = operation.Split(";")[i].Split(" copiato in ")[1];
                                string file = operation.Split(";")[i].Split(" copiato in ")[0];

                                File.Delete(folder+"\\"+file);
                            }
                        }
                    }
                    operation = "";
                    undodone = true;
                }
            }
            else
            {
                MessageBox.Show("Non è stata fatta nessuna operazione, non è possibile fare l'undo.", "Errore", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            BackgroundUpdate.RunWorkerAsync(argument: 1);

            if (undodone)
            {
                undodone = false;

                ShowNotification(1, "Undo completato", "");

                STATE.Text = "Undo completato";
            }
        }

        private void ShowNotification(int textnum, string text1, string text2)
        {
            if (textnum == 1)
            {
                try
                {
                    //Windows >= 10.0.17763.0
                    new ToastContentBuilder()
                    .AddText(text1)
                    .Show();
                }
                catch
                {
                    Console.WriteLine("Windows < 10.0.17763");
                }
            } else if (textnum == 2)
            {
                try
                {
                    //Windows >= 10.0.17763.0
                    new ToastContentBuilder()
                    .AddText(text1)
                    .AddText(text2)
                    .Show();
                }
                catch
                {
                    Console.WriteLine("Windows < 10.0.17763");
                }
            }
        }

        private void LockFolderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(destfolder_locked)
            {
                destfolder_locked = false;
                DestPath.Enabled = true;
                SelectDestFolder.Enabled = true;
            } else
            {
                destfolder_locked = true;
                DestPath.Enabled = false;
                SelectDestFolder.Enabled = false;

                if ((bool)Properties.Settings.Default["FirstFolderLock"] == true)
                {
                    Properties.Settings.Default["FirstFolderLock"] = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("CARTELLA DI DESTINAZIONE BLOCCATA\nLa cartella di destinazione non cambierà scegliendo una diversa " +
                        "cartella da riordinare.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //bottone che apre la finestra per trovare i file e cartelle più pesanti
        private void FileSizeButton_Click(object sender, EventArgs e)
        {
            if(dest_folder != "")
            {
                if(File.Exists(dest_folder+"\\.fileorg"))
                {
                    FileSizesForm form4 = new FileSizesForm();
                    form4.Show();
                } else
                {
                    MessageBox.Show("Questa funzione è attualmente supportata solo per le cartelle riordinate dal programma.",
                        "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("Non è stata selezionata nessuna cartella di destinazione.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
