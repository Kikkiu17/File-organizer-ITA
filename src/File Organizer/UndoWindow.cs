namespace File_Organizer
{
    public partial class UndoWindow : Form
    {
        string[] files = new string[MainWindow.operation.Split(";").Length-1];
        public UndoWindow()
        {
            InitializeComponent();
            int j = 0;

            foreach (string op in MainWindow.operation.Split(";"))
            {
                j++;
                if (op != "")
                {
                    files[j] = op;
                    if(op.Contains("spostato in"))
                    {
                        string file = files[j].Split(" spostato in ")[0];
                        string toadd = file + " verrà spostato in " + MainWindow.start_folder;
                        listBox1.Items.Add(toadd);
                    } else if(op.Contains("copiato in"))
                    {
                        string file = files[j].Split(" copiato in ")[0];
                        string toadd = file + " verrà ELIMINATO";
                        listBox1.Items.Add(toadd);
                    }
                }
            }
        }

        //evento di click del bottone undo
        private void DoUndo_Click(object sender, EventArgs e)
        {
            int total_files = files.Length;
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != "" && files[i] != null)
                {
                    //se il file è stato spostato, lo risposta alla posizione originale
                    //se è stato copiato, lo elimina
                    if (files[i].Contains("spostato in"))
                    {
                        string folder = files[i].Split(" spostato in ")[1];
                        string file = files[i].Split(" spostato in ")[0];
                        try
                        {
                            Directory.Move(folder+"\\"+file, MainWindow.start_folder+"\\"+file);
                        } catch
                        {
                            MessageBox.Show("C'è stato un errore; "+file+" ignorato", "Errore", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    else if (files[i].Contains("copiato in"))
                    {
                        string folder = files[i].Split(" copiato in ")[1];
                        string file = files[i].Split(" copiato in ")[0];
                        Console.WriteLine("DELETE "+folder+"\\"+file);
                        try
                        {
                            File.Delete(folder+"\\"+file);
                        }
                        catch
                        {
                            MessageBox.Show("C'è stato un errore; "+file+" ignorato", "Errore", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }

            MainWindow.operation = "";
            MainWindow.undodone = true;

            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DontShowAgain_CheckedChanged(object sender, EventArgs e)
        {
            if ((bool)Properties.Settings.Default["DontShowUndoAgain"] == false)
            {
                Properties.Settings.Default["DontShowUndoAgain"] = true;
                Properties.Settings.Default.Save();
            } else
            {
                Properties.Settings.Default["DontShowUndoAgain"] = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
