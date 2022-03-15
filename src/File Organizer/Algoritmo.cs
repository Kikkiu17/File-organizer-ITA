using System.Runtime.InteropServices;

static class Attributes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };

    public static class FILE_ATTRIBUTE
    {
        public const uint FILE_ATTRIBUTE_NORMAL = 0x80;
    }

    public static class SHGFI
    {
        public const uint SHGFI_TYPENAME = 0x000000400;
        public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
    }

    [DllImport("shell32.dll")]
    public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
}

namespace File_Organizer
{
    class Algorithm
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public delegate void ProgressUpdate(int value);
        public event ProgressUpdate OnProgressUpdate;

        //metodo di spostamento/copia file
        public (string, int, int, string) CopyMove(string dest_folder, string[] file_list, string action,
            bool backups_active, string backup_folder, bool is_preview)
        {
            int total_files = file_list.Length;
            int percentage = 0;
            int file_prog = 0;
            string created_folders1 = CheckDestFolder(dest_folder);
            string[] repeated_file_types = ClassificateFiles.GetTypeName(file_list, dest_folder);
            //string[] perceived_types = Algoritmo.ClassificateFiles.GetPerceivedType(file_list, dest_folder, file_types.Length);

            var tuples = CreateGroups(file_list, repeated_file_types);

            string[] file_types = tuples.Item1;
            string[] extensions = tuples.Item2;

            var checktuples = CheckFinalDirs(file_types, dest_folder, extensions, repeated_file_types);
            string created_folders2 = checktuples.Item1;
            file_types = checktuples.Item2;
            repeated_file_types = checktuples.Item3;

            int number_created_folders = 0;
            int operated_files = 0;

            string created_folders = created_folders2+created_folders1;

            number_created_folders = number_created_folders + created_folders2.Split(";").Length-1;

            if (created_folders1 != "")
            {
                number_created_folders++;
            }

            int ignored_files = 0;
            int arr_len = file_list.Length;
            bool backup_folder_exists = false;
            string backfolder = "";
            arr_len += 2;

            string operated_files_names = "";

            //crea e/o imposta la cartella di backup
            if (backups_active)
            {
                int index = dest_folder.LastIndexOf("\\");
                string[] folders = Directory.GetDirectories(dest_folder[..index]);
                foreach (string folder in folders)
                {
                    if (folder == backup_folder)
                    {
                        backup_folder_exists = true;
                    }
                }

                if (!backup_folder_exists)
                {
                    Directory.CreateDirectory(backup_folder);
                }

                string data = DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss").Replace(":", "-");

                Directory.CreateDirectory(backup_folder+"\\"+data);
                backfolder = backup_folder+"\\"+data+"\\";
            }

            //inizia lo spostamento/copia vero e proprio
            foreach (string file_type in file_types)
            {
                int file_index = 0;
                foreach (string repeated_file_type in repeated_file_types)
                {
                    if (repeated_file_type == file_type)
                    {
                        string file_dest_folder = dest_folder+"\\"+file_type;
                        string filename = file_list[file_index].Split("\\")[file_list[file_index].Split("\\").Length-1];

                        if (!File.Exists(file_dest_folder+"\\"+filename) && file_list[file_index] != Environment.ProcessPath)
                        {
                            file_prog++;
                            if (action == "MOVE")
                            {
                                if(!is_preview)
                                {
                                    if (backups_active)
                                    {
                                        try
                                        {
                                            File.Copy(file_list[file_index], backfolder+"\\"+filename);
                                        }
                                        catch (Exception e)
                                        {
                                            MessageBox.Show("C'è stato un errore durante l'esecuzione. Errore:\n\n"+e);
                                        }
                                    }

                                    try
                                    {
                                        File.Move(file_list[file_index], file_dest_folder+"\\"+filename);
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("C'è stato un errore durante l'esecuzione. Errore:\n\n"+e);
                                    }

                                    operated_files_names = operated_files_names + filename+" spostato in "+file_dest_folder + ";";
                                } else
                                {
                                    operated_files_names = operated_files_names + filename+" verrà spostato in "+file_dest_folder + ";";
                                }

                                operated_files++;
                            }
                            else if (action == "COPY" && !backups_active)
                            {
                                if (!is_preview)
                                {
                                    try
                                    {
                                        File.Copy(file_list[file_index], file_dest_folder+"\\"+filename);
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("C'è stato un errore durante l'esecuzione. Errore:\n\n"+e);
                                    }

                                    operated_files_names = operated_files_names + filename+" copiato in "+file_dest_folder + ";";
                                } else
                                {
                                    operated_files_names = operated_files_names + filename+" verrà copiato in "+file_dest_folder + ";";
                                }

                                operated_files++;
                            }

                        }
                        else
                        {
                            ignored_files++;
                        }

                        //aggiorna la progressbar
                        percentage = ((file_prog+ignored_files)*100)/total_files;

                        if (percentage <= 100)
                        {
                            if (OnProgressUpdate != null)
                            {
                                OnProgressUpdate(percentage);
                            }
                        }
                    }
                    file_index++;
                }
            }

            return (operated_files_names, ignored_files, number_created_folders, created_folders);
        }

        //controlla se c'è la cartella di destinazione
        //ad es: C:\Users\Kikkiu\Desktop\File Sistemati
        //controlla la presenza di File Sistemati
        private static string CheckDestFolder(string dest_folder)
        {
            string created_folder = "";
            bool exists = false;
            int index = dest_folder.LastIndexOf("\\");
            string[] folders = Directory.GetDirectories(dest_folder[..index]);
            foreach(string folder in folders)
            {
                if(folder.ToLower() == dest_folder.ToLower())
                {
                    exists = true;
                    if(!File.Exists(folder+"\\.fileorg"))
                    {
                        File.WriteAllTextAsync(folder+"\\.fileorg", folder+"\\.fileorg");
                        File.SetAttributes(folder+"\\.fileorg", FileAttributes.Hidden);
                    }
                    break;
                }
            }

            if(!exists)
            {
                Directory.CreateDirectory(dest_folder);
                File.WriteAllTextAsync(dest_folder+"\\.fileorg", dest_folder+"\\.fileorg");
                File.SetAttributes(dest_folder+"\\.fileorg", FileAttributes.Hidden);
                created_folder = dest_folder;
            }

            return created_folder;
        }

        //classe per la classificazione dei file
        private static class ClassificateFiles
        {
            //ottiene il tipo di file
            public static string[] GetTypeName(string[] file_list, string dest_folder)
            {
                int index = dest_folder.LastIndexOf("\\");
                string parent_dir = dest_folder[..index];
                string[] file_types = new string[file_list.Length];
                int i = 0;
                foreach (string file in file_list)
                {
                    Attributes.SHFILEINFO info = new Attributes.SHFILEINFO();
                    uint dwFileAttributes = Attributes.FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL;
                    uint uFlags = (uint)(Attributes.SHGFI.SHGFI_TYPENAME | Attributes.SHGFI.SHGFI_USEFILEATTRIBUTES);

                    Attributes.SHGetFileInfo(file, dwFileAttributes, ref info, (uint)Marshal.SizeOf(info), uFlags);

                    file_types[i] = info.szTypeName;
                    i++;
                }

                return file_types;
            }

            //ottiene il tipo di file confrontando più file con il registro di sistema
            /*private static string[] GetRegType(string[] extensions)
            {
                RegistryKey key = Registry.ClassesRoot;
                string[] file_types = new string[extensions.Length];
                int i = 0;

                foreach (string v in key.GetSubKeyNames())
                {
                    for (int j = 0; j < extensions.Length; j++)
                    {
                        if (v == extensions[j])
                        {
                            RegistryKey productKey = key.OpenSubKey(v);
                            if (productKey != null)
                            {
                                foreach (var value in productKey.GetValueNames())
                                {
                                    if (value == "PerceivedType")
                                    {
                                        file_types[i] = Convert.ToString(productKey.GetValue("PerceivedType")) + " " + v;
                                    }
                                }
                            }
                            i++;
                            break;
                        }
                    }
                }
                return file_types;
            }

            public static string[] GetPerceivedType(string[] file_list, string dest_folder, int arr_len)
            {
                string[] file_types = new string[arr_len];

                string[] extensions = new string[arr_len];
                for (int i = 0;i < arr_len;i++)
                {
                    FileInfo fi = new FileInfo(file_list[i]);
                    extensions[i] = fi.Extension;
                }

                file_types = GetRegType(extensions);

                return file_types;
            }*/
        }

        /*
         * creazione dei gruppi di file
         * raggruppa i file secondo il loro tipo
         */
        private static (string[], string[]) CreateGroups(string[] file_list, string[] file_types)
        {

            string[] extensions = new string[file_list.Length];

            int i = 0;
            foreach (string file in file_list)
            {
                FileInfo fi = new FileInfo(file);
                extensions[i++] = fi.Extension;
            }

            string new_file_types = "";

            i = 0;
            foreach(string file in file_types)
            {
                if (!new_file_types.Split(";").Contains(file))
                {
                    new_file_types = new_file_types + file + ";";
                }
                i++;
            }

            string[] single_file_types = new string[new_file_types.Split(";").Length];

            for (i = 0; i < new_file_types.Split(";").Length; i++)
            {
                single_file_types[i] = new_file_types.Split(";")[i];
            }

            //print dei gruppi nella console
            /*j = 0;
            foreach (string group in non_repeated_file_types.Split("\n"))
            {
                int l = 0;
                Console.WriteLine(group);
                for (int i = 0; i < file_types.Length; i++)
                {
                    if (group == file_types[i])
                    {
                        Console.WriteLine(file_groups[j, l]);
                        l++;
                    }
                }
                Console.WriteLine("\n\n");

                while (l < file_list.Length)
                {
                    file_groups[j, l] = "";
                    l++;
                }
                j++;
            }

            
             non_repeated_file_types.Split("\n")[i]: nome della raccolta
             file_groups[i, j]: elemento nella raccolta
             */

            return (single_file_types, extensions);
        }

        //controlla se le cartelle finali sono presenti
        //ad es: C:\Users\Kikkiu\Desktop\File Sistemati\File PNG
        //controlla se esiste File PNG
        private static (string, string[], string[]) CheckFinalDirs(string[] file_types, string dest_folder,
            string[] extensions, string[] repeated_file_types)
        {
            string created_folders_names = "";

            for (int i = 0; i < file_types.Length; i++)
            {
                string file_type = file_types[i];
                int file_index = 0;
                foreach(string r_file_type in repeated_file_types)
                {
                    if(r_file_type == file_type)
                    {

                        string dir_to_create = dest_folder + "\\" + file_type;
                        if (!Directory.Exists(dir_to_create))
                        {
                            bool exists = false;
                            foreach(string folder in Directory.GetDirectories(dest_folder))
                            {
                                if(File.Exists(folder+"\\"+extensions[file_index]))
                                {
                                    string folder_name = folder.Split("\\")[folder.Split("\\").Length-1];
                                    if(!file_types.Contains(folder_name))
                                    {
                                        file_types[i] = folder_name;
                                    }
                                    repeated_file_types[file_index] = folder_name;
                                    exists = true;
                                }
                            }

                            if(!exists)
                            {
                                created_folders_names = created_folders_names+dir_to_create+";";
                                Directory.CreateDirectory(dir_to_create);
                                if (extensions[file_index] != "")
                                {
                                    File.WriteAllTextAsync(dir_to_create+"\\"+extensions[file_index], extensions[file_index]);
                                    File.SetAttributes(dir_to_create+"\\"+extensions[file_index], FileAttributes.Hidden);
                                }
                            }
                        } else
                        {
                            if (!File.Exists(dir_to_create+"\\"+extensions[file_index]) && extensions[file_index] != "")
                            {
                                File.WriteAllTextAsync(dir_to_create+"\\"+extensions[file_index], extensions[file_index]);
                                File.SetAttributes(dir_to_create+"\\"+extensions[file_index], FileAttributes.Hidden);
                            }
                        }
                    }
                    file_index++;
                }
            }

            return (created_folders_names, file_types, repeated_file_types);
        }
    }
}