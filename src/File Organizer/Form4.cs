using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Organizer
{
    public partial class FileSizesForm : Form
    {
        private string dest_folder = MainWindow.dest_folder;
        private int start_index = 0;
        private int end_index = 0;
        private bool sel_all = false;
        ArrayList selected_files = new ArrayList();
        ArrayList totalfiles = new ArrayList();
        ArrayList selected_size = new ArrayList();
        ArrayList sizes = new ArrayList();
        ArrayList files = new ArrayList();
        private int number_of_files = 0;

        private ContextMenuStrip dataViewContextMenu;

        public FileSizesForm()
        {
            InitializeComponent();

            UpdateList();
            CurrentFolder.Text = "Cartella: "+dest_folder;

            dataViewContextMenu = new ContextMenuStrip();
            dataViewContextMenu.Opening +=new CancelEventHandler(dataViewContextMenu_Opening);
            dataGridView1.ContextMenuStrip = dataViewContextMenu;
            dataViewContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(contexMenu_ItemClicked);
        }

        private void dataViewContextMenu_Opening(object sender, CancelEventArgs e)
        {
            dataViewContextMenu.Items.Clear();
            dataViewContextMenu.Items.Add("Apri");
            dataViewContextMenu.Items.Add("Apri percorso file");
            dataViewContextMenu.Items.Add("Elimina");
        }

        private void contexMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string selected_item = item.Text;
            
            if(selected_item == "Apri")
            {
                OpenFile.PerformClick();
            } else if(selected_item == "Apri percorso file")
            {
                OpenFilePath.PerformClick();
            } else if(selected_item == "Elimina")
            {
                DeleteSelectedFiles.PerformClick();
            }
        }

            private void UpdateList()
        {
            long long_total_size = 0;
            int total_rows = 0;
            int folder_num = 0;

            dest_folder = MainWindow.dest_folder;
            start_index = 0;
            end_index = 0;
            sel_all = false;
            selected_files.Clear();
            totalfiles.Clear();
            sizes.Clear();
            files.Clear();
            number_of_files = 0;

            dataGridView1.Rows.Clear();

            if(Directory.Exists(dest_folder))
            {
                foreach (string folder in Directory.GetDirectories(dest_folder))
                {
                    foreach (string file_path in Directory.GetFiles(folder))
                    {
                        string file = file_path.Split("\\")[file_path.Split("\\").Length - 1];
                        string splitted_folder = folder.Split("\\")[folder.Split("\\").Length - 1];

                        if (!file.StartsWith("."))
                        {
                            FileInfo fileatt = new FileInfo(file_path);
                            totalfiles.Add(file_path);
                            long int_size = fileatt.Length;
                            string size = "";
                            if (int_size < 999)
                            {
                                size = int_size.ToString() + " B";
                            }
                            else if (int_size > 999 && int_size < 999999)
                            {
                                size = (int_size/1000).ToString() + " KB";
                            }
                            else if (int_size > 999999 && int_size < 999999999)
                            {
                                size = (int_size / 1000000).ToString() + " MB";
                            }
                            else
                            {
                                size = (int_size / 1000000000).ToString() + " GB";
                            }
                            files.Add(file+"SIZESEPARATOR"+size+"SIZESEPARATOR"+int_size.ToString()+"SIZESEPARATOR"+splitted_folder);
                            sizes.Add(int_size.ToString());
                            total_rows++;
                            long_total_size += int_size;
                        }
                    }
                    folder_num++;
                }

                var sortedList = sizes.Cast<string>().OrderBy(item => long.Parse(item)).Reverse();

                int tot_rows = 0;
                foreach (string size in sortedList)
                {
                    foreach (string file in files)
                    {
                        long filesize = long.Parse(file.Split("SIZESEPARATOR")[2]);
                        if (long.Parse(size) == filesize)
                        {
                            string folder = file.Split("SIZESEPARATOR")[3];
                            dataGridView1.Rows.Add(file.Split("SIZESEPARATOR")[0]);
                            dataGridView1.Rows[tot_rows].Cells[2].Value = file.Split("SIZESEPARATOR")[1];
                            dataGridView1.Rows[tot_rows].Cells[1].Value = folder;
                            tot_rows++;
                        }
                    }
                }

                string tot_size = "";
                if (long_total_size < 999)
                {
                    tot_size = long_total_size.ToString() + " B";
                }
                else if (long_total_size > 999 && long_total_size < 999999)
                {
                    tot_size = (long_total_size/1000).ToString() + " KB";
                }
                else if (long_total_size > 999999 && long_total_size < 999999999)
                {
                    tot_size = (long_total_size / 1000000).ToString() + " MB";
                }
                else
                {
                    tot_size = (long_total_size / 1000000000).ToString() + " GB";
                }

                TotalFiles.Text = "File totali: " + total_rows;
                TotalFolders.Text = "Cartelle totali: " + folder_num;
                TotalSize.Text = "Dimensioni totali: " + tot_size;
            }
        }

        //ottiene gli index degli elementi selezionati
        //start index è quando si tiene premuto il tasto sinitro, quindi si va all'end index quando
        //si rilascia il tasto
        //se si seleziona solo 1 elemento, i due index sono uguali
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var ht = dataGridView1.HitTest(e.X, e.Y);

                if (ht.Type == DataGridViewHitTestType.RowHeader)
                {
                    start_index = ht.RowIndex;
                }
                else if (ht.Type == DataGridViewHitTestType.Cell)
                {
                    start_index = ht.RowIndex;
                }

                SelectAll.Checked = false;
            }
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            //'#See if the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                var ht = dataGridView1.HitTest(e.X, e.Y);

                if (ht.Type == DataGridViewHitTestType.RowHeader)
                {
                    end_index = ht.RowIndex;
                } else if(ht.Type == DataGridViewHitTestType.Cell)
                {
                    end_index = ht.RowIndex;
                }

                if(start_index == 0 && end_index == dataGridView1.Rows.Count -2)
                {
                    SelectAll.Checked = true;
                }

                selected_files.Clear();
                selected_size.Clear();

                if (start_index == end_index && end_index < dataGridView1.Rows.Count -1)
                {
                    SelectedItems.Text = dataGridView1.Rows[start_index].Cells[0].Value.ToString() + " (1)";
                    selected_files.Add(dataGridView1.Rows[start_index].Cells[0].Value.ToString());
                    number_of_files = 1;
                } else
                {
                    if (end_index > start_index)
                    {
                        if(end_index < dataGridView1.Rows.Count -1)
                        {
                            number_of_files = end_index - start_index + 1;
                            SelectedItems.Text = "Da file numero "+start_index+" a file numero "+end_index+" ("+number_of_files+" file)";

                            for (int i = start_index; i < end_index+1; i++)
                            {
                                selected_files.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                            }
                        }
                    } else
                    {
                        if (end_index < dataGridView1.Rows.Count -1)
                        {
                            number_of_files = start_index - end_index + 1;
                            SelectedItems.Text = "Da file numero "+end_index+" a file numero "+start_index+" ("+number_of_files+" file)";

                            for (int i = end_index; i < start_index+1; i++)
                            {
                                selected_files.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                            }
                        }
                    }
                }

                GetTotalSize();
            }
        }

        bool KeyA = false;
        bool KeyCTRL = false;

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            var kcode = e.KeyCode;

            switch(kcode)
            {
                case Keys.A:
                    KeyA = true;
                    break;
                case Keys.ControlKey:
                    KeyCTRL = true;
                    KeyA = false;
                    break;
                case Keys.Delete:
                    DeleteFiles();
                    break;
            }

            if (KeyCTRL && KeyA)
            {
                SelectAll.Checked = true;
            }
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            var kcode = e.KeyCode;

            switch (kcode)
            {
                case Keys.A:
                    KeyA = false;
                    break;
                case Keys.ControlKey:
                    KeyCTRL = false;
                    break;
                case Keys.Enter:
                    OpenFile.PerformClick();
                    break;
            }
        }

        private void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if(sel_all)
            {
                sel_all = false;
            } else
            {
                sel_all = true;

                start_index = 0;
                end_index = dataGridView1.Rows.Count -2;

                number_of_files = end_index - start_index + 1;
                SelectedItems.Text = "Da file numero "+start_index+" a file numero "+end_index+" ("+number_of_files+" file)";

                selected_files.Clear();

                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    selected_files.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }

                GetTotalSize();
            }
        }

        private void GetTotalSize()
        {
            long long_total_size = 0;

            foreach (string string_size in selected_files)
            {
                string filename = string_size;

                foreach(string file in files)
                {
                    if(file.Contains(filename))
                    {
                        long long_size = long.Parse(file.Split("SIZESEPARATOR")[2]);
                        long_total_size += long_size;
                    }
                }
            }

            string tot_size = "";
            if (long_total_size < 999)
            {
                tot_size = long_total_size.ToString() + " B";
            }
            else if (long_total_size > 999 && long_total_size < 999999)
            {
                tot_size = (long_total_size/1000).ToString() + " KB";
            }
            else if (long_total_size > 999999 && long_total_size < 999999999)
            {
                tot_size = (long_total_size / 1000000).ToString() + " MB";
            }
            else
            {
                tot_size = (long_total_size / 1000000000).ToString() + " GB";
            }

            SelectedSize.Text = "Dimensioni file selezionati: "+tot_size;
        }

        private void DeleteSelectedFiles_Click(object sender, EventArgs e)
        {
            DeleteFiles();
        }

        private void DeleteFiles()
        {
            if(selected_files.Count > 0)
            {
                var result = MessageBox.Show("Vuoi davvero eliminare definitivamente "+number_of_files+" file?", "Attenzione",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < selected_files.Count; i++)
                    {
                        foreach (string file in totalfiles)
                        {
                            string filename = selected_files[i].ToString();
                            if (file.Contains(filename))
                            {
                                File.Delete(file);
                            }
                        }
                    }

                    MessageBox.Show(number_of_files+" file eliminati.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                UpdateList();
            } else
            {
                MessageBox.Show("Nessun file selezionato.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            if(selected_files.Count > 1)
            {
                MessageBox.Show("Non puoi aprire più di un file alla volta. Selezionane solo uno.", "Errore", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                if(selected_files.Count > 0)
                {
                    for (int i = 0; i < selected_files.Count; i++)
                    {
                        foreach (string file in totalfiles)
                        {
                            string filename = selected_files[i].ToString();
                            if (file.Contains(filename))
                            {
                                ProcessStartInfo psi = new ProcessStartInfo();
                                psi.FileName = file;
                                psi.UseShellExecute = true;
                                Process.Start(psi);
                            }
                        }
                    }
                } else
                {
                    MessageBox.Show("Nessun file selezionato.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenFilePath_Click(object sender, EventArgs e)
        {
            if (selected_files.Count > 1)
            {
                MessageBox.Show("Non puoi aprire più di un file alla volta. Selezionane solo uno.", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (selected_files.Count > 0)
                {
                    for (int i = 0; i < selected_files.Count; i++)
                    {
                        foreach (string file in totalfiles)
                        {
                            string filename = selected_files[i].ToString();
                            if (file.Contains(filename))
                            {
                                Process.Start("explorer.exe", @"/select,"+file);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nessun file selezionato.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (selected_files.Count > 1)
                {
                    MessageBox.Show("Non puoi aprire più di un file alla volta. Selezionane solo uno.", "Errore",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (selected_files.Count > 0)
                    {
                        for (int i = 0; i < selected_files.Count; i++)
                        {
                            foreach (string file in totalfiles)
                            {
                                string filename = selected_files[i].ToString();
                                if (file.Contains(filename))
                                {
                                    ProcessStartInfo psi = new ProcessStartInfo();
                                    psi.FileName = file;
                                    psi.UseShellExecute = true;
                                    Process.Start(psi);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nessun file selezionato.", "Errore",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
