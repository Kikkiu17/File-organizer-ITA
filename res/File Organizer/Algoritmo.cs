using System.Diagnostics;
using System.IO;
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

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
    internal static class Algorithm
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

        public static string[] CopyMove(string dest_folder, string[] file_list, string action, bool backups_active, string backup_folder, bool is_preview)
        {
            string created_folders1 = CheckDestFolder(dest_folder);
            string[] repeated_file_types = ClassificateFiles.GetTypeName(file_list, dest_folder);
            //string[] perceived_types = Algoritmo.ClassificateFiles.GetPerceivedType(file_list, dest_folder, file_types.Length);

            var tuples = CreateGroups(file_list, repeated_file_types);

            string[,] file_groups = tuples.Item1;
            string[] file_types = tuples.Item2;

            string[] created_folders2 = CheckFinalDirs(file_types, dest_folder);

            int number_created_folders = 0;

            foreach (string created_folder in created_folders2)
            {
                if (created_folder != "")
                {
                    number_created_folders++;
                }
            }

            if (created_folders1 != "")
            {
                number_created_folders++;
            }

            int i = 0;
            int ignored_files = 0;
            int operated_files = 0;
            int arr_len = file_list.Length;
            bool backup_folder_exists = false;
            string backfolder = "";
            arr_len += 2;

            string[] operated_files_names = new string[arr_len];

                operated_files_names[arr_len-1] = number_created_folders.ToString();

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

                if(!backup_folder_exists)
                {
                    Directory.CreateDirectory(backup_folder);
                }

                string data = DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss").Replace(":", "-");

                Directory.CreateDirectory(backup_folder+"\\"+data);
                backfolder = backup_folder+"\\"+data+"\\";
            }

            foreach (string file_type in file_types)
            {
                string final_folder = dest_folder+"\\"+file_type;
                for (int j = 0; j < file_list.Length; j++)
                {
                    if (file_groups[i, j] != "" && file_groups[i, j] != null)
                    {
                        bool exists = false;
                        string filename = "\\"+file_groups[i, j].Split("\\")[file_groups[i, j].Split("\\").Length-1];

                        int index = file_groups[i, j].LastIndexOf("\\");
                        string start_dir = file_groups[i, j][..index];

                        foreach (string file in Directory.EnumerateFiles(final_folder, "*"))
                        {
                            if(file == final_folder+filename)
                            {
                                exists = true;
                                ignored_files++;
                            }
                        }

                        if(!exists)
                        {
                            if (action == "MOVE")
                            {
                                if (backups_active)
                                {
                                    try
                                    {
                                        if(!is_preview)
                                        {
                                            File.Copy(file_groups[i, j], backfolder+filename);
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show("C'è stato un errore durante l'esecuzione\nOperazione completata comunque");
                                    }
                                }
                                try
                                {
                                    if(!is_preview)
                                    {
                                        File.Move(file_groups[i, j], final_folder+filename);
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("C'è stato un errore durante l'esecuzione\nOperazione completata comunque");
                                }

                                if(is_preview)
                                {
                                    operated_files_names[operated_files] = filename.Split("\\")[1]+" verrà spostato in "+final_folder;
                                    operated_files++;
                                } else
                                {
                                    operated_files_names[operated_files] = filename.Split("\\")[1]+" spostato in "+final_folder;
                                    operated_files++;
                                }
                            }
                            else if (action == "COPY" && !backups_active)
                            {
                                try
                                {
                                    if(!is_preview)
                                    {
                                        File.Copy(file_groups[i, j], final_folder+filename);
                                    }
                                } catch
                                {
                                    MessageBox.Show("C'è stato un errore durante l'esecuzione\nOperazione completata comunque");
                                }

                                if(is_preview)
                                {
                                    operated_files_names[operated_files] = filename.Split("\\")[1]+" verràaaa copiato in "+final_folder;
                                    operated_files++;
                                } else
                                {
                                    operated_files_names[operated_files] = filename.Split("\\")[1]+" copiato in "+final_folder;
                                    operated_files++;
                                }
                            }
                        }
                    }
                }
                i++;
            }
            operated_files_names[arr_len-2] = ignored_files.ToString();
            return operated_files_names;
        }

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
                }
            }

            if(!exists)
            {
                Directory.CreateDirectory(dest_folder);
                created_folder = dest_folder;
            }

            return created_folder;
        }

        private static class ClassificateFiles
        {
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

        private static (string[,], string[]) CreateGroups(string[] file_list, string[] file_types)
        {
            string[] extensions = new string[file_list.Length];
            for (int i = 0; i < file_list.Length; i++)
            {
                FileInfo fi = new FileInfo(file_list[i]);
                extensions[i] = fi.Extension;
            }

            string non_repeated_file_types = "";

            foreach (string type in file_types)
            {
                bool exists = false;
                if (non_repeated_file_types == "")
                {
                    non_repeated_file_types = type;
                }
                else
                {
                    string[] f_types_split = non_repeated_file_types.Split("\n");
                    for (int i = 0; i < f_types_split.Length; i++)
                    {
                        if (f_types_split[i] == type)
                        {
                            exists = true;
                        }
                    }

                    if (!exists)
                    {
                        non_repeated_file_types = non_repeated_file_types + "\n" + type;
                    }
                }
            }

            string[,] file_groups = new string[non_repeated_file_types.Split("\n").Length, file_list.Length];
            int j = 0;
            foreach (string group in non_repeated_file_types.Split("\n"))
            {
                int l = 0;
                for (int i = 0; i < file_types.Length; i++)
                {
                    if (group == file_types[i])
                    {
                        file_groups[j, l] = file_list[i];
                        l++;
                    }
                }

                while (l < file_list.Length)
                {
                    file_groups[j, l] = "";
                    l++;
                }
                j++;
            }

            /*
             * non_repeated_file_types.Split("\n")[i]: nome della raccolta
             * file_groups[i, j]: elemento nella raccolta
             */

            return (file_groups, non_repeated_file_types.Split("\n"));
        }

        private static string[] CheckFinalDirs(string[] file_types, string dest_folder)
        {
            int number_created_folders = 0;
            string[] created_folders_names = new string[file_types.Length];
            foreach (string file_type in file_types)
            {
                string[] folders = Directory.GetDirectories(dest_folder);
                bool exists = false;

                foreach (string folder in folders)
                {
                    string foldername = folder.Split("\\")[folder.Split("\\").Length-1];
                    if(foldername.ToLower() == file_type.ToLower())
                    {
                        exists = true;
                        created_folders_names[number_created_folders] = "";
                        number_created_folders++;
                    }
                }

                if(!exists)
                {
                    Directory.CreateDirectory(dest_folder+"\\"+file_type);
                    created_folders_names[number_created_folders] = dest_folder+"\\"+file_type;
                    number_created_folders++;
                }
            }

            return created_folders_names;
        }
    }
}