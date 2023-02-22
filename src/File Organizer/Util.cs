using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace File_Organizer
{
    public class Util
    {
        readonly static string desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";

        public string CreateTempFolder(string temp_folder_name)
        {
            string temp_path = Path.GetTempPath();
            if (!Directory.Exists(temp_path+@"\"+temp_folder_name))
                Directory.CreateDirectory(temp_path+@"\"+temp_folder_name);

            return temp_path+@"\"+temp_folder_name;
        }

        public string CheckForUpdates()
        {
            string get_version = "";
            string urlAddress = "https://github.com/Kikkiu17/File-organizer-ITA/tags";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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

                if (curr_version.ToString().Length == 2)
                    curr_version *= 10;

                if (get_raw_version.ToString().Length == 2)
                    get_raw_version *= 10;

                string hum_version = string.Join(".", get_raw_version.ToString().ToArray());

                response.Close();
                readStream.Close();

                if (curr_version < get_raw_version)
                {
                    var diagresult = MessageBox.Show("C'è una nuova versione del programma ("+ hum_version +"). Vuoi scaricarla e avviarla?", "Nuova versione",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Information);
                    if (diagresult == DialogResult.Yes)
                        return get_version;
                    else
                        return "-1";
                }
                else
                    return "-1";
            }
            else
            {
                MessageBox.Show("Impossibile controllare gli aggiornamenti", "Errore",
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
                return "-2";
            }
        }

        private Tuple<ArrayList, ArrayList> GetDesktopIconsNames()
        {
            ArrayList icons_and_shortcuts = new();
            ArrayList icons_list = new();

            IntPtr vHandle = FindWindow("Progman", "Program Manager");
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "FolderView");

            int vItemCount = SendMessage(vHandle, LVM_GETITEMCOUNT, 0, 0);
            uint vProcessId;
            GetWindowThreadProcessId(vHandle, out vProcessId);
            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ |
                PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

            try
            {
                for (int j = 0; j < vItemCount; j++)
                {
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = j;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));
                    uint vNumberOfBytesRead = 0;
                    WriteProcessMemory(vProcess, vPointer,
                        Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                        Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(vHandle, LVM_GETITEMW, j, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);
                    string vText = Encoding.Unicode.GetString(vBuffer, 0,
                        (int)vNumberOfBytesRead);
                    string IconName = vText;
                    icons_list.Add(IconName);
                }
            }

            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }

            dynamic app = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
            var windows = app.Windows;

            const int SWC_DESKTOP = 8;
            const int SWFO_NEEDDISPATCH = 1;
            var hwnd = 0;
            var disp = windows.FindWindowSW(Type.Missing, Type.Missing, SWC_DESKTOP, ref hwnd, SWFO_NEEDDISPATCH);

            var sp = (IServiceProvider)disp;
            var SID_STopLevelBrowser = new Guid("4c96be40-915c-11cf-99d3-00aa004ae837");

            var browser = (IShellBrowser)sp.QueryService(SID_STopLevelBrowser, typeof(IShellBrowser).GUID);
            var view = (IFolderView)browser.QueryActiveShellView();

            view.Items(SVGIO.SVGIO_ALLVIEW, typeof(IShellItemArray).GUID, out var items);
            if (items is IShellItemArray array)
            {
                for (var i = 0; i < array.GetCount(); i++)
                {
                    var item = array.GetItemAt(i);
                    icons_and_shortcuts.Add(item.GetDisplayName(SIGDN.SIGDN_NORMALDISPLAY));
                }
            }

            return Tuple.Create(icons_list, icons_and_shortcuts);
        }

        private Tuple<ArrayList, ArrayList> GetDesktopIconsPosition()
        {
            ArrayList names = new();
            ArrayList locations = new();

            IntPtr vHandle = FindWindow("Progman", "Program Manager");
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "FolderView");

            int vItemCount = SendMessage(vHandle, LVM_GETITEMCOUNT, 0, 0);
            uint vProcessId;
            GetWindowThreadProcessId(vHandle, out vProcessId);
            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ |
                PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

            try
            {
                for (int j = 0; j < vItemCount; j++)
                {
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = j;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));
                    uint vNumberOfBytesRead = 0;

                    WriteProcessMemory(vProcess, vPointer,
                        Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                        Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(vHandle, LVM_GETITEMW, j, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);

                    string vText = Encoding.Unicode.GetString(vBuffer, 0,
                        (int)vNumberOfBytesRead);
                    string IconName = vText;

                    SendMessage(vHandle, LVM_GETITEMPOSITION, j, vPointer.ToInt32());
                    Point[] vPoint = new Point[1];
                    ReadProcessMemory(vProcess, vPointer,
                        Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0),
                        Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);
                    string IconLocation = vPoint[0].ToString();
                    names.Add(IconName);
                    locations.Add(IconLocation);
                }

            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }

            return Tuple.Create(names, locations);
        }

        private static IntPtr MakeLParam(int wLow, int wHigh)
        {
            return (IntPtr)(((short)wHigh << 16) | (wLow & 0xffff));
        }

        public void SetDesktopIconsPosition(int idx, int x, int y)
        {
            IntPtr vHandle = FindWindow("Progman", "Program Manager");
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "FolderView");
            SendMessage(vHandle, LVM_SETITEMPOSITION, idx, (int)MakeLParam(x, y));
        }

        public Tuple<ArrayList, ArrayList, ArrayList, ArrayList> GetDesktopShortcutsFullInfo()
        {
            ArrayList x = new();
            ArrayList y = new();
            ArrayList icon_names = new();
            ArrayList idx = new();

            Tuple<ArrayList, ArrayList> unsorted_icon_names = GetDesktopIconsNames();
            Tuple<ArrayList, ArrayList> names_locations = GetDesktopIconsPosition(); // item1: name, item2: location
            ArrayList files = GetDesktopFilesNames();
            ArrayList folders = GetDesktopFoldersNames();

            for (int i = 0; i < names_locations.Item1.Count; i++)
            {
                string name2 = names_locations.Item1[i].ToString();
                for (int j = 0; j < unsorted_icon_names.Item2.Count; j++)
                {
                    string name1 = unsorted_icon_names.Item2[j].ToString();
                    if (name2.StartsWith(name1) && !files.Contains(name1) && !folders.Contains(name1))
                    {
                        idx.Add(j);
                        icon_names.Add(name1);
                        x.Add(names_locations.Item2[j].ToString().Split(",")[0].Replace("{X=", ""));
                        y.Add(names_locations.Item2[j].ToString().Split(",")[1].Replace("}", "").Replace("Y=", ""));
                        break;
                    }
                }
            }

            return Tuple.Create(icon_names, x, y, idx);
        }

        private Tuple<ArrayList, ArrayList, ArrayList, ArrayList> GetDesktopIconsNameAndPositions()
        {
            ArrayList x = new();
            ArrayList y = new();
            ArrayList icon_names = new();
            ArrayList idx = new();

            Tuple<ArrayList, ArrayList> unsorted_icon_names = GetDesktopIconsNames();
            Tuple<ArrayList, ArrayList> names_locations = GetDesktopIconsPosition(); // item1: name, item2: location

            for (int i = 0; i < names_locations.Item1.Count; i++)
            {
                string name2 = names_locations.Item1[i].ToString();
                for (int j = 0; j < unsorted_icon_names.Item1.Count; j++)
                {
                    string name1 = unsorted_icon_names.Item1[j].ToString();
                    if (name2.StartsWith(name1))
                    {
                        idx.Add(j);
                        icon_names.Add(name1);
                        break;
                    }
                }
            }

            for (int i = 0; i < unsorted_icon_names.Item1.Count; i++)
            {
                string local_x = names_locations.Item2[i].ToString().Split(",")[0].Replace("{", "").Replace("X=", "");
                string local_y = names_locations.Item2[i].ToString().Split(",")[1].Replace("}", "").Replace("Y=", "");
                x.Add(local_x);
                y.Add(local_y);
            }

            return Tuple.Create(icon_names, x, y, idx);
        }

        private ArrayList GetDesktopFilesNames()
        {
            ArrayList list = new();
            var desktoplist = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)).GetFiles()
       .Concat(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)).GetFiles())
       .Distinct();
            foreach (var file in desktoplist)
                list.Add(file.Name);
            return list;
        }

        private ArrayList GetDesktopFoldersNames()
        {
            ArrayList list = new();
            foreach (string item in Directory.EnumerateDirectories(desktop_path))
                list.Add(item.Split(@"\")[^1]);
            return list;
        }

        public int GetDesktopNumberOfItems()
        {
            int num1 = GetDesktopFoldersNames().Count;
            int num2 = GetDesktopFilesNames().Count;
            return num1 + num2;
        }

        public Tuple<ArrayList, ArrayList, ArrayList, ArrayList> GetDesktopFoldersFullInfo()
        {
            ArrayList real_files = new();
            ArrayList real_folders = new();
            ArrayList files_x = new();
            ArrayList files_y = new();
            ArrayList folders_x = new();
            ArrayList folders_y = new();
            ArrayList files_idx = new();
            ArrayList folders_idx = new();

            Tuple<ArrayList, ArrayList, ArrayList, ArrayList> icons = GetDesktopIconsNameAndPositions(); // tuple: name, x, y
            ArrayList desktop_files = GetDesktopFilesNames();
            ArrayList desktop_folders = GetDesktopFoldersNames();

            for (int i = 0; i < desktop_files.Count; i++)
            {
                for (int j = 0; j < icons.Item1.Count; j++)
                {
                    string desktop_full_filename = desktop_files[i].ToString().Split(@"\")[^1];

                    if (icons.Item1[j].ToString().StartsWith(desktop_full_filename))
                    {
                        real_files.Add(desktop_files[i]);
                        files_x.Add(icons.Item2[j]);
                        files_y.Add(icons.Item3[j]);
                        files_idx.Add(j);
                        break;
                    }
                }
            }

            for (int i = 0; i < desktop_folders.Count; i++)
            {
                if (!real_files.Contains(desktop_folders[i].ToString()))
                {
                    for (int j = 0; j < icons.Item1.Count; j++)
                    {
                        //string desktop_foldername = desktop_folders[i].ToString().Split(@"\")[^1];

                        if (icons.Item1[j].ToString().StartsWith(desktop_folders[i].ToString()) && !files_idx.Contains(j))
                        {
                            real_folders.Add(desktop_folders[i]);
                            folders_x.Add(icons.Item2[j]);
                            folders_y.Add(icons.Item3[j]);
                            folders_idx.Add(j);
                            break;
                        }
                    }
                }
            }

            return Tuple.Create(real_folders, folders_x, folders_y, folders_idx);
        }

        public Tuple<ArrayList, ArrayList, ArrayList, ArrayList> GetDesktopFilesFullInfo()
        {
            ArrayList real_files = new();
            ArrayList files_x = new();
            ArrayList files_y = new();
            ArrayList files_idx = new();

            Tuple<ArrayList, ArrayList, ArrayList, ArrayList> icons = GetDesktopIconsNameAndPositions(); // tuple: name, x, y
            ArrayList desktop_files = GetDesktopFilesNames();

            for (int i = 0; i < desktop_files.Count; i++)
            {
                for (int j = 0; j < icons.Item1.Count; j++)
                {
                    string desktop_full_filename = desktop_files[i].ToString().Split(@"\")[^1];

                    if (icons.Item1[j].ToString().StartsWith(desktop_full_filename))
                    {
                        real_files.Add(desktop_files[i]);
                        files_x.Add(icons.Item2[j]);
                        files_y.Add(icons.Item3[j]);
                        files_idx.Add(j);
                        break;
                    }
                }
            }

            return Tuple.Create(real_files, files_x, files_y, files_idx);
        }

        public Tuple<ArrayList, ArrayList, ArrayList> GetAllItems()
        {
            Tuple<ArrayList, ArrayList, ArrayList, ArrayList> shortcuts = GetDesktopShortcutsFullInfo();
            Tuple<ArrayList, ArrayList, ArrayList, ArrayList> folders = GetDesktopFoldersFullInfo();
            Tuple<ArrayList, ArrayList, ArrayList, ArrayList> files = GetDesktopFilesFullInfo();

            ArrayList idx = new();
            ArrayList x = new();
            ArrayList y = new();

            idx.AddRange(shortcuts.Item4);
            idx.AddRange(folders.Item4);
            idx.AddRange(files.Item4);
            x.AddRange(shortcuts.Item2);
            x.AddRange(folders.Item2);
            x.AddRange(files.Item2);
            y.AddRange(shortcuts.Item3);
            y.AddRange(folders.Item3);
            y.AddRange(files.Item3);

             return Tuple.Create(x, y, idx);
        }

        public static void RefreshDesktop()
        {
            ToggleDesktopIcons();
            //Thread.Sleep(10);
            ToggleDesktopIcons();
            Thread.Sleep(450);
        }

        private static void ToggleDesktopIcons()
        {
            var toggleDesktopCommand = new IntPtr(0x7402);
            IntPtr hWnd = IntPtr.Zero;
            if (Environment.OSVersion.Version.Major < 6 || Environment.OSVersion.Version.Minor < 2) //7 and -
                hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
            else
            {
                var ptrs = FindWindowsWithClass("WorkerW");
                int i = 0;
                while (hWnd == IntPtr.Zero && i < ptrs.Count())
                {
                    hWnd = FindWindowEx(ptrs.ElementAt(i), IntPtr.Zero, "SHELLDLL_DefView", null);
                    i++;
                }
            }
            SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        private const int WM_COMMAND = 0x111;

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size++ > 0)
            {
                var builder = new StringBuilder(size);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        public static IEnumerable<IntPtr> FindWindowsWithClass(string className)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                StringBuilder cl = new StringBuilder(256);
                GetClassName(wnd, cl, cl.Capacity);
                if (cl.ToString() == className && (GetWindowText(wnd) == "" || GetWindowText(wnd) == null))
                {
                    windows.Add(wnd);
                }
                return true;
            },
                        IntPtr.Zero);

            return windows;
        }

        [Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryService([MarshalAs(UnmanagedType.LPStruct)] Guid service, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
        }

        // note: for the following interfaces, not all methods are defined as we don't use them here
        [Guid("000214E2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellBrowser
        {
            void _VtblGap1_12(); // skip 12 methods https://stackoverflow.com/a/47567206/403671

            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryActiveShellView();
        }

        [Guid("cde725b0-ccc9-4519-917e-325d72fab4ce"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFolderView
        {
            void _VtblGap1_5(); // skip 5 methods

            [PreserveSig]
            int Items(SVGIO uFlags, Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object items);
        }

        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object BindToHandler(System.Runtime.InteropServices.ComTypes.IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid bhid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);

            IShellItem GetParent();

            [return: MarshalAs(UnmanagedType.LPWStr)]
            string GetDisplayName(SIGDN sigdnName);
            // 2 other methods to be defined
        }

        [Guid("b63ea76d-1f85-456f-a19c-48159efa858b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItemArray
        {
            void _VtblGap1_4(); // skip 4 methods

            int GetCount();
            IShellItem GetItemAt(int dwIndex);
        }

        private enum SIGDN
        {
            SIGDN_NORMALDISPLAY,
            SIGDN_PARENTRELATIVEPARSING,
            SIGDN_DESKTOPABSOLUTEPARSING,
            SIGDN_PARENTRELATIVEEDITING,
            SIGDN_DESKTOPABSOLUTEEDITING,
            SIGDN_FILESYSPATH,
            SIGDN_URL,
            SIGDN_PARENTRELATIVEFORADDRESSBAR,
            SIGDN_PARENTRELATIVE,
            SIGDN_PARENTRELATIVEFORUI
        }

        private enum SVGIO
        {
            SVGIO_BACKGROUND,
            SVGIO_SELECTION,
            SVGIO_ALLVIEW,
            SVGIO_CHECKED,
            SVGIO_TYPE_MASK,
            SVGIO_FLAG_VIEWORDER
        }

        private const uint LVM_FIRST = 0x1000;
        private const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        private const uint LVM_GETITEMW = LVM_FIRST + 75;
        private const uint LVM_SETITEMPOSITION = LVM_FIRST + 15;
        private const uint LVM_GETITEMPOSITION = LVM_FIRST + 16;
        private const uint PROCESS_VM_OPERATION = 0x0008;
        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_VM_WRITE = 0x0020;
        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RELEASE = 0x8000;
        private const uint MEM_RESERVE = 0x2000;
        private const uint PAGE_READWRITE = 4;
        private const int LVIF_TEXT = 0x0001;

        [DllImport("kernel32.dll")]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
            uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
           uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
           IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
           IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess,
            bool bInheritHandle, uint dwProcessId);

        [DllImport("user32.DLL")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.DLL")]
        private static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

        [DllImport("user32.DLL")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent,
            IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd,
            out uint dwProcessId);

        private struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            public IntPtr pszText; // string
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;

        }
    }
}
