using System.Runtime.InteropServices;
using System.Collections;

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
            Application.Run(new MainWindow());
        }

        public delegate void ProgressUpdate(int value);
        public event ProgressUpdate? OnProgressUpdate;

        //metodo di spostamento/copia file
        public (string, int, int, string, ArrayList, string) CopyMove(string dest_folder, string[] file_list, string action,
            bool backups_active, string backup_folder, bool is_preview)
        {
            ArrayList ignored_files_names = new();

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
                number_created_folders++;

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
                        string filename = file_list[file_index].Split("\\")[^1];

                        if (!File.Exists(file_dest_folder+"\\"+filename) && file_list[file_index] != Environment.ProcessPath)
                        {
                            file_prog++;
                            if (action == "MOVE")
                            {
                                if (!is_preview)
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
                                }
                                else
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
                                }
                                else
                                {
                                    operated_files_names = operated_files_names + filename+" verrà copiato in "+file_dest_folder + ";";
                                }

                                operated_files++;
                            }

                        }
                        else
                        {
                            ignored_files_names.Add(file_dest_folder+"\\"+filename);
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

            return (operated_files_names, ignored_files, number_created_folders, created_folders, ignored_files_names, backfolder);
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
            foreach (string folder in folders)
            {
                if (folder.ToLower() == dest_folder.ToLower())
                {
                    exists = true;
                    if (!File.Exists(folder+"\\.fileorg"))
                    {
                        File.WriteAllTextAsync(folder+"\\.fileorg", folder+"\\.fileorg");
                        File.SetAttributes(folder+"\\.fileorg", FileAttributes.Hidden);
                    }
                    break;
                }
            }

            if (!exists)
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
                FileInfo fi = new(file);
                extensions[i++] = fi.Extension;
            }

            string new_file_types = "";

            i = 0;
            foreach (string file in file_types)
            {
                if (!new_file_types.Split(";").Contains(file))
                    new_file_types = new_file_types + file + ";";
                i++;
            }

            string[] single_file_types = new string[new_file_types.Split(";").Length];

            for (i = 0; i < new_file_types.Split(";").Length; i++)
                single_file_types[i] = new_file_types.Split(";")[i];

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
                foreach (string r_file_type in repeated_file_types)
                {
                    if (r_file_type == file_type)
                    {

                        string dir_to_create = dest_folder + "\\" + file_type;
                        if (!Directory.Exists(dir_to_create))
                        {
                            bool exists = false;
                            foreach (string folder in Directory.GetDirectories(dest_folder))
                            {
                                if (File.Exists(folder+"\\"+extensions[file_index]))
                                {
                                    string folder_name = folder.Split("\\")[folder.Split("\\").Length-1];
                                    if (!file_types.Contains(folder_name))
                                    {
                                        file_types[i] = folder_name;
                                    }
                                    repeated_file_types[file_index] = folder_name;
                                    exists = true;
                                }
                            }

                            if (!exists)
                            {
                                created_folders_names = created_folders_names+dir_to_create+";";
                                Directory.CreateDirectory(dir_to_create);
                                if (extensions[file_index] != "")
                                {
                                    File.WriteAllTextAsync(dir_to_create+"\\"+extensions[file_index], extensions[file_index]);
                                    File.SetAttributes(dir_to_create+"\\"+extensions[file_index], FileAttributes.Hidden);
                                }
                            }
                        }
                        else
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

    class IconMover
    {
        readonly Util ut = new();

        public (List<int>, List<int>, List<int>) OrganizeTopLeft(Tuple<ArrayList, ArrayList, ArrayList, ArrayList> input_tuple, bool apply)
        {
            // item_name, x, y, idx
            ArrayList name_list = new();
            ArrayList idx = new();
            name_list.AddRange(input_tuple.Item1);
            idx.AddRange(input_tuple.Item4);
            Tuple<ArrayList, ArrayList, ArrayList, ArrayList> shortcuts = ut.GetDesktopShortcutsFullInfo();
            int shortcuts_no = shortcuts.Item1.Count;
            List<string> sorted_items = new();
            sorted_items.AddRange(name_list.Cast<string>().ToList());
            sorted_items.Sort();

            for (int i = 0; i < sorted_items.Count; i++)
            {
                if (sorted_items[i].ToString().StartsWith("Cestino"))
                    sorted_items.RemoveAt(i);
            }

            int items_no = sorted_items.Count;
            double items_sqr = Math.Sqrt(items_no);
            int items_upper_int = (int)Math.Round(items_sqr);
    
            List<int> idx_list = new();
            List<int> x_level_list = new();
            List<int> y_level_list = new();
            List<int> aux_x = new();
            List<int> aux_y = new();
            List<int> aux_idx = new();
            Tuple<ArrayList, ArrayList, ArrayList> items = ut.GetAllItems(); // x, y, idx
            ArrayList itemsx = new();
            ArrayList itemsy = new();
            itemsx.AddRange(items.Item1);
            itemsy.AddRange(items.Item2);

            // spacing y 84 px
            // spacing x 76 px
            int column_limit = items_upper_int;
            int y_level = 86;
            int x_level = 22;
            for (int i = 0; i < items_upper_int + 1; i++)
            {
                for (int j = 0; j < column_limit; j++)
                {
                    for (int k = 0; k < name_list.Count; k++)
                    {
                        if (sorted_items[j] == name_list[k])
                        {
                            if (sorted_items.Count == 1)
                            {
                                idx_list.Add((int)idx[k]);
                                x_level_list.Add(x_level);
                                y_level_list.Add(2);
                                break;
                            }
                            idx_list.Add((int)idx[k]);
                            x_level_list.Add(x_level);
                            y_level_list.Add(y_level);
                            y_level += 84;
                        }
                    }

                    if (sorted_items.Count == 1)
                        break;
                }
                sorted_items = sorted_items.Skip(column_limit).ToList();
                column_limit -= (i == 0) ? 0 : 1;
                x_level += 76;
                y_level = 2;

                // controlla se ci sono oggetti lasciati da parte e li riordina
                if (i == items_upper_int)
                {
                    if (sorted_items.Count > 0)
                    {
                        x_level = 22;
                        y_level = items_upper_int * 84 + 86;

                        for (int j = 0; j < sorted_items.Count; j++)
                        {
                            for (int k = 0; k < name_list.Count; k++)
                            {
                                if (sorted_items[j] == name_list[k])
                                {
                                    idx_list.Add((int)idx[k]);
                                    x_level_list.Add(x_level);
                                    y_level_list.Add(y_level);
                                }
                            }

                            x_level += 76;
                            y_level -= 84;
                        }
                    }

                    bool can_continue = false;
                    for (int h = 0; h < idx_list.Count; h++)
                    {
                        int curx = x_level_list[h];
                        int cury = y_level_list[h];

                        // idx, x, y
                        for (int j = 0; j < items.Item3.Count; j++)
                        {
                            int curidx = int.Parse(items.Item3[j].ToString());
                            if (can_continue)
                            {
                                can_continue = false;
                                break;
                            }
                            if (curx == int.Parse(itemsx[j].ToString()) && cury == int.Parse(itemsy[j].ToString()))
                            {
                                for (int k = 0; k < 25; k++) // X
                                {
                                    if (can_continue)
                                        break;
                                    int x = 22 + 76 * k;
                                    for (int l = 0; l < 12; l++) // Y
                                    {
                                        if (can_continue)
                                            break;
                                        int y = 2 + 84 * l;
                                        if (!(x_level_list.Contains(x) && y_level_list.Contains(y)) && !(aux_x.Contains(x) && aux_y.Contains(y))
                                            && curidx != idx_list[h])
                                        {
                                            aux_idx.Add(curidx);
                                            aux_x.Add(x);
                                            aux_y.Add(y);
                                            can_continue = true;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                if (sorted_items.Count == 0)
                    break;
            }

            if (apply)
            {
                for (int j = 0; j < aux_idx.Count; j++)
                    ut.SetDesktopIconsPosition(aux_idx[j], aux_x[j], aux_y[j]);
                for (int h = 0; h < idx_list.Count; h++)
                    ut.SetDesktopIconsPosition(idx_list[h], x_level_list[h], y_level_list[h]);

                for (int i = 0; i < shortcuts_no; i++)
                {
                    if (shortcuts.Item1[i].ToString() == "Cestino")
                    {
                        ut.SetDesktopIconsPosition((int)shortcuts.Item4[i], 22, 2);
                        // il cestino è sempre in 22, 2
                    }
                }
            }

            return (idx_list, x_level_list, y_level_list);
        }

        public (List<int>, List<int>, List<int>) OrganizeTopRight(Tuple<ArrayList, ArrayList, ArrayList, ArrayList> input_tuple, bool apply)
        {
            // item_name, x, y, idx
            ArrayList name_list = new();
            ArrayList idx = new();
            name_list.AddRange(input_tuple.Item1);
            idx.AddRange(input_tuple.Item4);
            Tuple<ArrayList, ArrayList, ArrayList, ArrayList> shortcuts = ut.GetDesktopShortcutsFullInfo();
            List<string> sorted_items = new();
            sorted_items.AddRange(name_list.Cast<string>().ToList());
            sorted_items.Sort();
            sorted_items.Reverse();

            for (int i = 0; i < sorted_items.Count; i++)
            {
                if (sorted_items[i].ToString().StartsWith("Cestino"))
                    sorted_items.RemoveAt(i);
            }

            int items_no = sorted_items.Count;
            double items_sqr = Math.Sqrt(items_no);
            int items_upper_int = (int)Math.Ceiling(items_sqr) + 1;

            List<int> idx_list = new();
            List<int> x_level_list = new();
            List<int> y_level_list = new();
            List<int> aux_x = new();
            List<int> aux_y = new();
            List<int> aux_idx = new();
            Tuple<ArrayList, ArrayList, ArrayList> items = ut.GetAllItems(); // x, y, idx

            // spacing y 84 px
            // spacing x 76 px
            int column_limit = items_upper_int;
            int y_level = 2 + (items_upper_int - 1) * 84;
            int x_level = 1846;
            for (int i = 0; i < items_upper_int; i++)
            {
                if (column_limit >= sorted_items.Count)
                    column_limit = sorted_items.Count;
                if (i == 4 && column_limit == 1 && sorted_items.Count > 1)
                    column_limit = sorted_items.Count;
                y_level = 2 + (column_limit - 1) * 84;
                for (int j = 0; j < column_limit; j++)
                {
                    for (int k = 0; k < name_list.Count; k++)
                    {
                        if (sorted_items[j] == name_list[k].ToString())
                        {
                            idx_list.Add((int)idx[k]);
                            x_level_list.Add(x_level);
                            y_level_list.Add(y_level);
                        }
                    }
                    y_level -= 84;

                    /*for (int k = 0; k < name_list.Count; k++)
                    {
                        // CHECK list conTENTS
                        Console.WriteLine($"J: {j}");
                        Console.WriteLine($"K: {k}");
                        Console.WriteLine("--- SORTED ITEMS:");
                        foreach (string item in sorted_items)
                            Console.WriteLine(item);
                        Console.WriteLine("--- NAME LIST:");
                        foreach (string item in name_list)
                            Console.WriteLine(item);
                        if (sorted_items[j] == name_list[k].ToString())
                        {
                            if (sorted_items.Count == 1)
                            {
                                idx_list.Add((int)idx[k]);
                                x_level_list.Add(x_level);
                                y_level_list.Add(2);
                                break;
                            }
                            idx_list.Add((int)idx[k]);
                            x_level_list.Add(x_level);
                            y_level_list.Add(y_level);
                            y_level -= 84;
                            Console.WriteLine("2");
                        }
                    }

                    if (sorted_items.Count == 1)
                        break;*/
                }
                sorted_items = sorted_items.Skip(column_limit).ToList();
                if (sorted_items.Count == 0)
                    break;
                column_limit -= 1;
                x_level -= 76;
            }

            bool can_continue = false;
            for (int h = 0; h < idx_list.Count; h++)
            {
                int curx = x_level_list[h];
                int cury = y_level_list[h];

                // idx, x, y
                for (int j = 0; j < items.Item3.Count; j++)
                {
                    int curidx = int.Parse(items.Item3[j].ToString());
                    if (can_continue)
                    {
                        can_continue = false;
                        break;
                    }
                    if (curx == int.Parse(items.Item1[j].ToString()) && cury == int.Parse(items.Item2[j].ToString()))
                    {
                        for (int k = 0; k < 25; k++) // X
                        {
                            if (can_continue)
                                break;
                            int x = 22 + 76 * k;
                            for (int l = 0; l < 12; l++) // Y
                            {
                                if (can_continue)
                                    break;
                                int y = 2 + 84 * l;
                                bool levels_contains = false;
                                for (int g = 0; g < x_level_list.Count; g++)
                                {
                                    if (x_level_list[g] == x && y_level_list[g] == y)
                                        levels_contains = true;
                                }
                                bool aux_contains = false;
                                for (int g = 0; g < aux_x.Count; g++)
                                {
                                    if (aux_x[g] == x && aux_y[g] == y)
                                        aux_contains = true;
                                }
                                bool items_contains = false;
                                for (int g = 0; g < items.Item1.Count; g++)
                                {
                                    if (int.Parse(items.Item1[g].ToString()) == x && int.Parse(items.Item2[g].ToString()) == y)
                                        items_contains = true;
                                }
                                if (!levels_contains && !aux_contains && !items_contains && curidx != idx_list[h])
                                {
                                    aux_idx.Add(curidx);
                                    aux_x.Add(x);
                                    aux_y.Add(y);
                                    can_continue = true;
                                }
                            }
                        }
                    }
                }

            }

            if (apply)
            {
                for (int j = 0; j < aux_idx.Count; j++)
                    ut.SetDesktopIconsPosition(aux_idx[j], aux_x[j], aux_y[j]);
                for (int h = 0; h < idx_list.Count; h++)
                    ut.SetDesktopIconsPosition(idx_list[h], x_level_list[h], y_level_list[h]);
            }

            return (idx_list, x_level_list, y_level_list);
        }
    }
}
