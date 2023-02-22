using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualBasic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System;
using System.Runtime.InteropServices;

namespace File_Organizer
{
    public partial class MainWindow : Form
    {
        List<string> folders_names = new();
        List<int> folders_x = new();
        List<int> folders_y = new();
        List<string> shortcuts_names = new();
        List<int> shortcuts_x = new();
        List<int> shortcuts_y = new();
        readonly Util ut = new();
        readonly IconMover im = new();
        readonly string desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        string get_version = "";
        string update_url = "";
        string update_dest_path = "";
        bool exist_start_folder = false;
        bool exist_dest_folder = false;
        bool backups_active = false;
        bool is_preview = false;
        
        public static string start_folder = "";
        public static string operation = "";
        public static string dest_folder = "";
        public static bool undodone = false;
        private string backup_folder = "";
        private int optype = 0;
        private bool isoperating = false;
        private bool destfolder_locked = false;
        private bool backfolder_locked = false;

        private ContextMenuStrip listboxContextMenu;
        private ContextMenuStrip UnOperatedFilesContextMenu;

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

            string response = ut.CheckForUpdates();
            get_version = response;
            if (response != "-1")
            {
                string temp_path = ut.CreateTempFolder("organizer");
                if (!File.Exists(temp_path+"\\"+"Organizer-v"+get_version+".zip"))
                {
                    update_url = "https://github.com/Kikkiu17/File-organizer-ITA/releases" +
                            "/download/v"+get_version+"/Organizer-v"+get_version+".zip";
                    update_dest_path = temp_path+"\\"+"Organizer-v"+get_version+".zip";
                    UpdateProgram();
                }
            }

            //controlla se è il primo avvio
            if ((bool)Properties.Settings.Default["FirstRun"] == true)
            {
                Properties.Settings.Default["FirstRun"] = false;
                Properties.Settings.Default.Save();
                var result = MessageBox.Show("Sembra che sia la prima volta che apri l'applicazione. Questa è una piccola introduzione" +
                    " al suo utilizzo.\n\nSe hai aggiornato il programma, puoi saltare l'introduzione. Vuoi saltarla?", "Benvenuto",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
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

            //crea un contextMenu per la listbox dei file sistemati
            listboxContextMenu = new ContextMenuStrip();
            listboxContextMenu.Opening +=new CancelEventHandler(listboxContextMenu_Opening);
            OperatedFiles.ContextMenuStrip = listboxContextMenu;
            listboxContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(contexMenu_ItemClicked);

            //crea un contextMenu epr la listbox dei file non sistemati
            UnOperatedFilesContextMenu = new ContextMenuStrip();
            UnOperatedFilesContextMenu.Opening +=new CancelEventHandler(UnOperatedFilesContextMenu_Opening);
            UnOperatedFiles.ContextMenuStrip = UnOperatedFilesContextMenu;
            UnOperatedFilesContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(UnOperatedFilesContextMenu_ItemClicked);
        }

        private void listboxContextMenu_Opening(object sender, CancelEventArgs e)
        {
            listboxContextMenu.Items.Clear();
            listboxContextMenu.Items.Add("Apri");
            //listboxContextMenu.Items.Add("Apri percorso file");
            listboxContextMenu.Items.Add("Rinomina");
            listboxContextMenu.Items.Add("Elimina");
            listboxContextMenu.Items.Add("Tipi di file collegati");
        }

        private void UnOperatedFilesContextMenu_Opening(object sender, CancelEventArgs e)
        {
            UnOperatedFilesContextMenu.Items.Clear();
            UnOperatedFilesContextMenu.Items.Add("Apri");
            UnOperatedFilesContextMenu.Items.Add("Elimina");
        }

        private void UnOperatedFilesContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string selected_item = item.Text;
            if (UnOperatedFiles.SelectedItem != null)
            {
                string listbox_item = UnOperatedFiles.SelectedItem.ToString();
                if (selected_item == "Apri")
                {
                    string filename = start_folder+"\\"+listbox_item;
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = filename;
                    psi.UseShellExecute = true;
                    Process.Start(psi);
                }
                else if (selected_item == "Elimina")
                {
                    var result = MessageBox.Show("Eliminare definitivamente il file?", "Avviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(start_folder+"\\"+UnOperatedFiles.SelectedItem);
                        BackgroundUpdate.RunWorkerAsync(argument: 2);
                    }
                }
            }
        }

        private void UnOperatedFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (UnOperatedFiles.SelectedItem != null && UnOperatedFiles.SelectedItem.ToString() != "")
            {
                int location = UnOperatedFiles.IndexFromPoint(e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    UnOperatedFiles.SelectedIndex = location;                //Index selected
                    UnOperatedFilesContextMenu.Show(UnOperatedFiles.PointToScreen(e.Location));
                }
            }
        }

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
                        "Avviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                StartPath.Text = folderBrowserDialog1.SelectedPath;
                exist_start_folder = true;
                exist_dest_folder = true;
                string checkeddestfolder = CheckDestPath();
                if (checkeddestfolder != "")
                {
                    if (!destfolder_locked)
                    {
                        DestPath.Text = checkeddestfolder;
                    }

                    if (!backfolder_locked)
                    {
                        int index = checkeddestfolder.LastIndexOf("\\");
                        string backuppath = checkeddestfolder[..index];
                        BackupPath.Text = backuppath+"\\Backup";
                    }
                }
                else
                {
                    if (!destfolder_locked)
                    {
                        DestPath.Text = folderBrowserDialog1.SelectedPath + "\\File Sistemati";
                    }

                    if (!backfolder_locked)
                    {
                        BackupPath.Text = folderBrowserDialog1.SelectedPath + "\\Backup";
                    }
                }
                start_folder = folderBrowserDialog1.SelectedPath;
                dest_folder = DestPath.Text;
                backup_folder = BackupPath.Text;
                STATE.Text = "Pronto";
                BackgroundUpdate.RunWorkerAsync(argument: 0);
            }
        }

        private string CheckDestPath()
        {
            LockDestFolderCheckBox.Enabled = true;
            LockBackFolderCheckBox.Enabled = true;
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
                }
                catch
                {
                    continue;
                }
            }

            return destfolder;
        }

        private void SelectDestFolder_Click(object sender, EventArgs e)
        {
            LockDestFolderCheckBox.Enabled = true;
            LockBackFolderCheckBox.Enabled = true;
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

        private void MoveFilesButton_Click(object sender, EventArgs e)
        {
            if (exist_dest_folder && exist_start_folder)
            {
                if (dest_folder != start_folder)
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
                }
                else
                {
                    MessageBox.Show("La cartella di destinazione e quella da riordinare non possono essere uguali.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
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
                if (dest_folder != start_folder)
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
                }
                else
                {
                    MessageBox.Show("La cartella di destinazione e quella da riordinare non possono essere uguali.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            }
        }

        private void CreateBackupButton_CheckedChanged(object sender, EventArgs e)
        {
            if (backups_active)
            {
                backups_active = false;
                BackupPath.Enabled = false;
                SelectBackupFolder.Enabled = false;
                CopyFilesButton.Enabled = true;
                STATE.Text = "Backup DISABILITATO";
            }
            else
            {
                backups_active = true;
                if (!backfolder_locked)
                {
                    BackupPath.Enabled = true;
                    SelectBackupFolder.Enabled = true;
                }
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

        //messaggio costante per la messagebox
        private string GetMessage()
        {
            string message = "Si sta per eliminare la seguente cartella: "+backup_folder+"\nContinuare?";
            return message;
        }

        private void DeleteBackups_Click(object sender, EventArgs e)
        {
            if (backups_active)
            {
                if (backup_folder == "")
                {
                    MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
                }
                else
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

                        STATE.Text = "Operazione completata";
                    }
                    else
                    {
                        MessageBox.Show("La cartella di backup da eliminare non esiste", "Errore",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Backup disattivato. Abilitare prima il backup", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            }
        }

        private void Preview_CheckedChanged(object sender, EventArgs e)
        {
            if (is_preview)
            {
                is_preview = false;
                MODE.Text = "MODALITA' NORMALE: Verranno apportate modifiche ai file";
            }
            else
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

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (OperatedFiles.SelectedItem != null && OperatedFiles.SelectedItem.ToString() != "")
            {
                int location = OperatedFiles.IndexFromPoint(e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    OperatedFiles.SelectedIndex = location;                //Index selected
                    listboxContextMenu.Show(OperatedFiles.PointToScreen(e.Location));
                }
            }
        }

        private void contexMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string selected_item = item.Text;
            if (OperatedFiles.SelectedItem != null && !OperatedFiles.SelectedItem.ToString().Contains("spostato") &&
                !OperatedFiles.SelectedItem.ToString().Contains("copiato"))
            {
                string folder = OperatedFiles.SelectedItem.ToString();
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
                    }
                }
                else if (selected_item == "Rinomina")
                {
                    var result = Interaction.InputBox("Rinomina la cartella selezionata.\nIl nome non può contenere i seguenti caratteri:" +
                        "\n\\ / : * ? \" < > |", "Rinomina", OperatedFiles.SelectedItem.ToString());

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
                    ArrayList old_file_types = new();
                    foreach (string file in Directory.GetFiles(dest_folder+"\\"+folder))
                    {
                        string f = file.Split("\\")[^1];
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
                        ArrayList new_file_types = new();
                        ArrayList f_types_folders = new();
                        string existing_f_types = "";
                        string f_types = "";
                        foreach (string f_type in result.Split(", "))
                        {
                            new_file_types.Add(f_type);
                            f_types = f_types + f_type + " ";
                        }
                        foreach (string subfolder in Directory.GetDirectories(dest_folder))
                        {
                            foreach (string file_type in new_file_types)
                            {
                                if (!old_file_types.Contains(file_type))
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

                        if (existing_f_types != "")
                        {
                            var msgboxresult = MessageBox.Show("I file con estensione "+existing_f_types+" sono già associati ad un'altra cartella." +
                                " Spostare i file dall'altra cartella a quella attuale?", "Attenzione",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if (msgboxresult == DialogResult.Yes)
                            {
                                foreach (string subfolder in f_types_folders)
                                {
                                    foreach (string file in Directory.GetFiles(subfolder))
                                    {
                                        string filename = file.Split("\\")[^1];
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

                        MessageBox.Show("File collegati: "+result, "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        BackgroundUpdate.RunWorkerAsync(argument: 1);
                    }
                }
                else if (selected_item == "Apri")
                {
                    if (OperatedFiles.SelectedItem != null && OperatedFiles.SelectedItem.ToString() != "")
                    {
                        ProcessStartInfo startInfo = new()
                        {
                            Arguments = dest_folder+"\\"+OperatedFiles.SelectedItem.ToString(),
                            FileName = "explorer.exe"
                        };

                        Process.Start(startInfo);
                    }
                }
                /*else if (selected_item == "Apri percorso file")
                {
                    if (OperatedFiles.SelectedItem != null && OperatedFiles.SelectedItem.ToString() != "")
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.Arguments = dest_folder+"\\"+OperatedFiles.SelectedItem.ToString().Split(" spostato")[0];
                        startInfo.FileName = "explorer.exe";

                        Process.Start(startInfo);
                    }
                }*/
            }
        }

        private void FileListOptions_Click(object sender, EventArgs e)
        {
            if (UnOperatedFiles.Items.Count > 0)
            {
                new FileListOptionsWindow().Show();
            }
            else
            {
                MessageBox.Show("La lista dei file è vuota", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            string[] file_list = new string[UnOperatedFiles.Items.Count];
            UnOperatedFiles.Items.CopyTo(file_list, 0);
            for (int i = 0; i < file_list.Length; i++)
            {
                file_list[i] = start_folder+"\\"+file_list[i];
            }

            var tuples = _alg.CopyMove(dest_folder, file_list, "MOVE", backups_active, backup_folder, is_preview);

            string[] operated_files_names = tuples.Item1.Split(";");
            int ignored_files = tuples.Item2;
            int created_folders = tuples.Item3;
            string created_folders_names = tuples.Item4;

            UnOperatedFiles.Items.Clear();
            foreach (string created_folder in created_folders_names.Split(";"))
            {
                if (created_folder != "")
                {
                    UnOperatedFiles.Items.Add(created_folder);
                }
            }

            int moved_files = operated_files_names.Length-1;
            bool remaining_files = false;

            if (tuples.Item5.Count > 0 && !is_preview)
            {
                foreach (string filename in tuples.Item5)
                {
                    var result = MessageBox.Show($"Esiste già un file con il nome {filename.Split(@"\")[^1]} al percorso {filename}. Sostituire il file?",
                        "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(filename);
                        if (backups_active)
                            File.Copy(start_folder+@"\"+filename.Split(@"\")[^1], tuples.Item6.ToString()+@"\"+filename.Split(@"\")[^1]);
                        File.Move(start_folder+@"\"+filename.Split(@"\")[^1], filename);
                        OperatedFiles.Items.Add($"{start_folder+@"\"+filename.Split(@"\")[^1]}" +
                            $" è stato spostato in {filename.Replace(filename.Split(@"\")[^1], "")}");
                    }
                    else
                        remaining_files = true;
                }
            }

            if (moved_files != 0)
            {
                OperatedFiles.Items.Clear();

                for (int i = 0; i < operated_files_names.Length; i++)
                {
                    if (operated_files_names[i] != null)
                    {
                        OperatedFiles.Items.Add(operated_files_names[i]);
                    }
                }

                if (moved_files < 0)
                {
                    moved_files = 0;
                }

                if (is_preview)
                {
                    if (tuples.Item5.Count > 0 && !remaining_files)
                    {
                        foreach (string filename in tuples.Item5)
                            OperatedFiles.Items.Add($"{filename} esiste già");
                    }
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
                    OperatedFiles.Items.Clear();
                    if (tuples.Item5.Count > 0 && !remaining_files)
                    {
                        foreach (string filename in tuples.Item5)
                            OperatedFiles.Items.Add($"{filename} esiste già");
                    }
                    else
                        OperatedFiles.Items.Add("Non verrà spostato nessun file");
                }
                else
                {
                    var x = (created_folders == 1) ? label1.Text = "È stata creata 1" +
                        " cartella" : label1.Text = "Sono state create "+created_folders+" cartelle";

                    label1.Text = "Sono state create "+created_folders+" cartelle";
                    if (tuples.Item5.Count == 0 || remaining_files)
                    {
                        OperatedFiles.Items.Clear();
                        label2.Text = "Non è stato spostato nessun file ("+ignored_files+" file ignorato/i)";
                        OperatedFiles.Items.Add("Non è stato spostato nessun file");
                    }
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
                for (int i = 0; i < OperatedFiles.Items.Count; i++)
                {
                    operation = operation + OperatedFiles.Items[i].ToString() + ";";
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

            if (backfolder_locked)
            {
                SelectBackupFolder.Enabled = false;
                BackupPath.Enabled = false;
            }

            STATE.Text = "Operazione completata";

            if (!is_preview)
            {
                if ((bool)Properties.Settings.Default["FirstOp"] == true)
                {
                    Properties.Settings.Default["FirstOp"] = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("È stato creato un file \".fileorg\" nella cartella di destinazione principale. Serve al programma" +
                        " per riconoscere quest'ultima, quindi non rimuoverla.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //evento di work del backgroundworker (copia file)
        private void DoCopy(object sender, DoWorkEventArgs e)
        {
            string[] file_list = new string[UnOperatedFiles.Items.Count];
            UnOperatedFiles.Items.CopyTo(file_list, 0);
            for (int i = 0; i < file_list.Length; i++)
            {
                file_list[i] = start_folder+"\\"+file_list[i];
            }
            var tuples = _alg.CopyMove(dest_folder, file_list, "COPY", backups_active, backup_folder, is_preview);

            string[] operated_files_names = tuples.Item1.Split(";");
            int ignored_files = tuples.Item2;
            int created_folders = tuples.Item3;
            string created_folders_names = tuples.Item4;

            UnOperatedFiles.Items.Clear();
            foreach (string created_folder in created_folders_names.Split(";"))
            {
                if (created_folder != "")
                {
                    UnOperatedFiles.Items.Add(created_folder);
                }
            }

            int moved_files = operated_files_names.Length-1;

            if (tuples.Item5.Count > 0 && !is_preview)
            {
                foreach (string filename in tuples.Item5)
                {
                    var result = MessageBox.Show($"Esiste già un file con il nome {filename.Split(@"\")[^1]} al percorso {filename}. Sostituire il file?",
                        "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(filename);
                        File.Copy(start_folder+@"\"+filename.Split(@"\")[^1], filename);
                        OperatedFiles.Items.Add($"{start_folder+@"\"+filename.Split(@"\")[^1]}" +
                            $" è stato copiato in {filename.Replace(filename.Split(@"\")[^1], "")}");
                    }
                }
            }

            if (moved_files != 0)
            {
                OperatedFiles.Items.Clear();

                for (int i = 0; i < operated_files_names.Length; i++)
                {
                    if (operated_files_names[i] != null)
                    {
                        OperatedFiles.Items.Add(operated_files_names[i]);
                    }
                }

                if (moved_files < 0)
                {
                    moved_files = 0;
                }

                if (is_preview)
                {
                    if (tuples.Item5.Count > 0)
                    {
                        foreach (string filename in tuples.Item5)
                            OperatedFiles.Items.Add($"{filename} esiste già");
                    }
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
                    OperatedFiles.Items.Clear();
                    if (tuples.Item5.Count > 0)
                    {
                        foreach (string filename in tuples.Item5)
                            OperatedFiles.Items.Add($"{filename} esiste già");
                    }
                    else
                        OperatedFiles.Items.Add("Non verrà copiato nessun file");
                }
                else
                {
                    var x = (created_folders == 1) ? label1.Text = "È stata creata 1" +
                        " cartella" : label1.Text = "Sono state create "+created_folders+" cartelle";

                    label1.Text = "Sono state create "+created_folders+" cartelle";
                    if (tuples.Item5.Count == 0)
                    {
                        OperatedFiles.Items.Clear();
                        label2.Text = "Non è stato copiato nessun file ("+ignored_files+" file ignorato/i)";
                        OperatedFiles.Items.Add("Non è stato copiato nessun file");
                    }
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
                for (int i = 0; i < OperatedFiles.Items.Count; i++)
                {
                    operation = operation + OperatedFiles.Items[i].ToString() + ";";
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

            if (backfolder_locked)
            {
                SelectBackupFolder.Enabled = false;
                BackupPath.Enabled = false;
            }

            STATE.Text = "Operazione completata";

            if (!is_preview)
            {
                if ((bool)Properties.Settings.Default["FirstOp"] == true)
                {
                    Properties.Settings.Default["FirstOp"] = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("È stato creato un file \".fileorg\" nella cartella di destinazione principale. Serve al programma" +
                        " a riconoscere quest'ultima, quindi non rimuoverla.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (!isoperating)
                {
                    CopyFilesButton.Enabled = true;
                }
            }

            progressBar1.Value = 0;

            if (type != 3)
            {
                UnOperatedFiles.Items.Clear();

                if (dest_folder == "")
                {
                    MessageBox.Show("Nessuna cartella selezionata", "Errore",
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Error);
                }
                else
                {

                    if (start_folder != "")
                    {
                        int i = 0;
                        foreach (string file in Directory.EnumerateFiles(StartPath.Text, "*"))
                        {
                            string clean_file = file.Split("\\")[^1];
                            if (clean_file != "desktop.ini" && clean_file != Environment.ProcessPath.Split("\\")[^1])
                            {
                                i++;
                                UnOperatedFiles.Items.Add(file.Split("\\")[^1]);
                            }
                        }
                    }

                    if (type != 2)
                    {
                        OperatedFiles.Items.Clear();
                        if (Directory.Exists(dest_folder))
                        {
                            string[] sub_folders = Directory.GetDirectories(DestPath.Text);
                            foreach (string folder in sub_folders)
                            {
                                OperatedFiles.Items.Add(folder.Split("\\")[^1]);
                            }
                            label2.Text = "Cartelle in "+DestPath.Text+":";
                        }
                        else
                        {
                            label2.Text = "La cartella "+DestPath.Text+" non esiste ancora";
                        }

                        var x = (UnOperatedFiles.Items.Count == 1) ? label1.Text =
                            "È stato trovato "+UnOperatedFiles.Items.Count.ToString()+" file in "+StartPath.Text : label1.Text =
                            "Sono stati trovati "+UnOperatedFiles.Items.Count.ToString()+" file in "+StartPath.Text;
                    }
                }
            }
            else
            {
                OperatedFiles.Items.Clear();
                if (Directory.Exists(dest_folder))
                {
                    string[] sub_folders = Directory.GetDirectories(DestPath.Text);
                    foreach (string folder in sub_folders)
                    {
                        OperatedFiles.Items.Add(folder.Split("\\")[^1]);
                    }
                    label2.Text = "Cartelle in "+DestPath.Text+":";
                }
                else
                {
                    label2.Text = "La cartella "+DestPath.Text+" non esiste ancora";
                }

                var x = (UnOperatedFiles.Items.Count == 1) ? label1.Text =
                    "È stato trovato "+UnOperatedFiles.Items.Count.ToString()+" file in "+StartPath.Text : label1.Text =
                    "Sono stati trovati "+UnOperatedFiles.Items.Count.ToString()+" file in "+StartPath.Text;
            }
        }

        //evento dii completamento dell'aggiornamento della lista
        private void UpdateComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (optype == 100)
            {
                optype = 0;
                OperatedFiles.Items.Clear();
                operation = "";
                backgroundWorker1.RunWorkerAsync();
            }
            else if (optype == 200)
            {
                optype = 0;
                OperatedFiles.Items.Clear();
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
                    new UndoWindow().Show();
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
                MessageBox.Show("Non è stata fatta nessuna operazione, non è possibile fare l'undo", "Errore", MessageBoxButtons.OK,
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
                    .AddText(text1).Show();
                }
                catch
                {
                    Console.WriteLine("Windows < 10.0.17763");
                }
            }
            else if (textnum == 2)
            {
                try
                {
                    //Windows >= 10.0.17763.0
                    new ToastContentBuilder()
                    .AddText(text1)
                    .AddText(text2).Show();
                }
                catch
                {
                    Console.WriteLine("Windows < 10.0.17763");
                }
            }
        }

        private void LockFolderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (destfolder_locked)
            {
                destfolder_locked = false;
                DestPath.Enabled = true;
                SelectDestFolder.Enabled = true;
            }
            else
            {
                destfolder_locked = true;
                DestPath.Enabled = false;
                SelectDestFolder.Enabled = false;

                if ((bool)Properties.Settings.Default["FirstFolderLock"] == true)
                {
                    Properties.Settings.Default["FirstFolderLock"] = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("CARTELLA DI DESTINAZIONE BLOCCATA\nLa cartella di destinazione non cambierà scegliendo una diversa " +
                        "cartella da riordinare.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void FileSizeButton_Click(object sender, EventArgs e)
        {
            if (dest_folder != "")
            {
                if (File.Exists(dest_folder+"\\.fileorg"))
                {
                    FileManagerWindow form4 = new FileManagerWindow();
                    form4.Show();
                }
                else
                {
                    MessageBox.Show("Questa funzione è attualmente supportata solo per le cartelle riordinate dal programma.",
                        "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nessuna cartella di destinazione selezionata.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OperatedFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (OperatedFiles.SelectedItem != null && OperatedFiles.SelectedItem.ToString() != "")
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.Arguments = dest_folder+"\\"+OperatedFiles.SelectedItem.ToString();
                    startInfo.FileName = "explorer.exe";

                    Process.Start(startInfo);
                }
            }
        }

        private void OperatedFiles_KeyUp(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;

            if (key == Keys.Enter)
            {
                if (OperatedFiles.SelectedItem != null && OperatedFiles.SelectedItem.ToString() != "")
                {
                    string filename = start_folder+"\\"+UnOperatedFiles.SelectedItem.ToString();
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = filename;
                    psi.UseShellExecute = true;
                    Process.Start(psi);
                }
            }
        }

        private void UnOperatedFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (UnOperatedFiles.SelectedItem != null && UnOperatedFiles.SelectedItem.ToString() != "")
                {
                    string filename = start_folder+"\\"+UnOperatedFiles.SelectedItem.ToString();
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = filename;
                    psi.UseShellExecute = true;
                    Process.Start(psi);
                }
            }
        }

        private void UnOperatedFiles_KeyUp(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;

            if (key == Keys.Enter)
            {
                if (UnOperatedFiles.SelectedItem != null && UnOperatedFiles.SelectedItem.ToString() != "")
                {
                    string filename = start_folder+"\\"+UnOperatedFiles.SelectedItem.ToString();
                    Process.Start("explorer.exe", @"/select,"+filename);

                }
            }
            else if (key == Keys.Delete)
            {
                if (UnOperatedFiles.SelectedItem != null && UnOperatedFiles.SelectedItem.ToString() != "")
                {
                    var result = MessageBox.Show("Eliminare definitivamente il file?", "Avviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(start_folder+"\\"+UnOperatedFiles.SelectedItem);
                        BackgroundUpdate.RunWorkerAsync(argument: 2);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BackgroundUpdate.RunWorkerAsync(argument: 1);
        }

        private void LockBackFolderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (backfolder_locked)
            {
                backfolder_locked = false;
                if (backups_active)
                {
                    BackupPath.Enabled = true;
                    SelectBackupFolder.Enabled = true;
                }
            }
            else
            {
                backfolder_locked = true;
                BackupPath.Enabled = false;
                SelectBackupFolder.Enabled = false;

                if ((bool)Properties.Settings.Default["FirstFolderLock"] == true)
                {
                    Properties.Settings.Default["FirstFolderLock"] = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("CARTELLA BLOCCATA\nLa cartella bloccata non cambierà scegliendo una diversa " +
                        "cartella da riordinare.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public async void UpdateProgram()
        {
            STATE.Text = "DOWNLOAD AGGIORNAMENTO IN CORSO...";
            foreach (Control ctrl in Controls)
                ctrl.Enabled = false;
            STATE.Enabled = true;
            int i = 0;

            var p = new Progress<float>(m =>
            {
                progressBar1.Value = (int)(m*100);
                if ((int)(m*100) == 100)
                {
                    if (i == 0)
                    {
                        i++;
                        string temp_path = ut.CreateTempFolder("organizer");
                        string program_path = Path.GetDirectoryName(Application.ExecutablePath)+@"\";
                        string zip_file_path = Directory.GetFiles(temp_path)[0];
                        string new_program_name = zip_file_path.Split(@"\")[^1].Replace(".zip", ".exe");
                        STATE.Text = "ESTRAZIONE IN CORSO...";
                        ZipFile.ExtractToDirectory(zip_file_path, temp_path);
                        if (!File.Exists(program_path + new_program_name))
                            File.Move(temp_path + new_program_name, program_path + new_program_name);
                        Directory.Delete(temp_path, true);
                        Process.Start(program_path + new_program_name);
                        if (Application.MessageLoop)
                            Application.Exit();
                        else
                            Environment.Exit(1);
                    }
                }
            });

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                using (var file = new FileStream(update_dest_path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    CancellationToken ct = CancellationToken.None;
                    await client.DownloadAsync(update_url, file, p, ct);
                }
            }
        }

        private void CheckUpdates_Click(object sender, EventArgs e)
        {
            string response = ut.CheckForUpdates();
            get_version = response;
            if (response != "-1")
            {
                string temp_path = ut.CreateTempFolder("organizer");
                if (!File.Exists(temp_path+"\\"+"Organizer-v"+get_version+".zip"))
                {
                    update_url = "https://github.com/Kikkiu17/File-organizer-ITA/releases" +
                            "/download/v"+get_version+"/Organizer-v"+get_version+".zip";
                    update_dest_path = temp_path+"\\"+"Organizer-v"+get_version+".zip";
                    UpdateProgram();
                }
            }
            else if (response == "-1")
            {
                MessageBox.Show("Il programma è aggiornato", "Avviso",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Information);
            }
        }

        private void OrganizeIcons_Click(object sender, EventArgs e)
        {
            IconsOrganizerWindow win = new IconsOrganizerWindow();
            win.Show();
            folders_names.Clear();
            folders_x.Clear();
            folders_y.Clear();
            shortcuts_names.Clear();
            shortcuts_x.Clear();
            shortcuts_y.Clear();
            Util.RefreshDesktop();
            // name, x, y, idx
            var folders = ut.GetDesktopFoldersFullInfo();
            for (int i = 0; i < folders.Item2.Count; i++)
            {
                int x = int.Parse(folders.Item2[i].ToString());
                int y = int.Parse(folders.Item3[i].ToString());
                folders_x.Add(x);
                folders_y.Add(y);
                folders_names.Add(folders.Item1[i].ToString());
            }
            var topright = im.OrganizeTopRight(folders, false);
            Util.RefreshDesktop();
            var shortcuts = ut.GetDesktopShortcutsFullInfo();
            for (int i = 0; i < shortcuts.Item2.Count; i++)
            {
                int x = int.Parse(shortcuts.Item2[i].ToString());
                int y = int.Parse(shortcuts.Item3[i].ToString());
                shortcuts_x.Add(x);
                shortcuts_y.Add(y);
                shortcuts_names.Add(shortcuts.Item1[i].ToString());
            }
            var topleft = im.OrganizeTopLeft(shortcuts, false);

            win.DrawIcons(topright, topleft);
        }

        private void UndoIcons_Click(object sender, EventArgs e)
        {
            Util.RefreshDesktop();
            var shortcuts = ut.GetDesktopShortcutsFullInfo();
            for (int i = 0; i < shortcuts_names.Count; i++)
            {
                for (int j = 0; j < shortcuts.Item1.Count; j++)
                {
                    if (shortcuts_names[i] == shortcuts.Item1[j].ToString())
                        ut.SetDesktopIconsPosition((int)shortcuts.Item4[i], shortcuts_x[i], shortcuts_y[i]);
                }
            }
            var folders = ut.GetDesktopFoldersFullInfo();
            for (int i = 0; i < folders_names.Count; i++)
            {
                for (int j = 0; j < folders.Item1.Count; j++)
                {
                    if (folders_names[i] == folders.Item1[j].ToString())
                        ut.SetDesktopIconsPosition((int)folders.Item4[i], folders_x[i], folders_y[i]);
                }
            }
        }
    }

    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress = null, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }

    public static class HttpClientExtensions
    {
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default)
        {
            // Get the http headers first to examine the content length
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                var contentLength = response.Content.Headers.ContentLength;

                using (var download = await response.Content.ReadAsStreamAsync())
                {

                    // Ignore progress reporting when no progress reporter was 
                    // passed or when the content length is unknown
                    if (progress == null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination);
                        return;
                    }

                    // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                    var relativeProgress = new Progress<long>(totalBytes => progress.Report((float)totalBytes / contentLength.Value));
                    // Use extension method to report progress while downloading
                    await download.CopyToAsync(destination, 81920, relativeProgress, cancellationToken);
                    progress.Report(1);
                }
            }
        }
    }
}
