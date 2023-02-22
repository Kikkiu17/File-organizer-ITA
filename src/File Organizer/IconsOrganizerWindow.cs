using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace File_Organizer
{
    public partial class IconsOrganizerWindow : Form
    {
        public IconsOrganizerWindow()
        {
            InitializeComponent();
        }

        private const int SPACING = 4;
        private const int BORDER = 4;
        private const int SQUARE_DIM = 34;
        private readonly SolidBrush lightGrayBrush = new SolidBrush(Color.LightGray);
        private readonly SolidBrush blackBrush = new SolidBrush(Color.Black);

        public void DrawIcons((List<int>, List<int>, List<int>) topright, (List<int>, List<int>, List<int>) topleft)
        {
            // idx, x, y

            // elimina gli elementi doppi
            for (int i = topleft.Item1.Count - 1; i >= 0; i--)
            {
                for (int j = topleft.Item1.Count - 1; j >= 0; j--)
                {
                    if (i != j)
                    {
                        if (topleft.Item1[i] == topleft.Item1[j])
                        {
                            topleft.Item1.RemoveAt(j);
                            topleft.Item2.RemoveAt(j);
                            topleft.Item3.RemoveAt(j);
                            if (i > j)
                                i--;
                        }
                    }
                }
            }

            // elimina gli elementi doppi
            for (int i = topright.Item1.Count - 1; i >= 0; i--)
            {
                for (int j = topright.Item1.Count - 1; j >= 0; j--)
                {
                    if (i != j)
                    {
                        if (topright.Item1[i] == topright.Item1[j])
                        {
                            topright.Item1.RemoveAt(j);
                            topright.Item2.RemoveAt(j);
                            topright.Item3.RemoveAt(j);
                            if (i > j)
                                i--;
                        }
                    }
                }
            }

            Graphics mGraphics = Graphics.FromHwnd(Handle);
            mGraphics.FillRectangle(lightGrayBrush, BORDER, BORDER, 954, 544);
            mGraphics.FillRectangle(blackBrush, BORDER + SPACING, BORDER + SPACING + 1, SQUARE_DIM, SQUARE_DIM);

            for (int i = 0; i < topright.Item1.Count; i++)
            {
                int idx = topright.Item1[i];
                int x = topright.Item2[i] / 2;
                int y = topright.Item3[i] / 2;
                mGraphics.FillRectangle(blackBrush, BORDER + SPACING + x - 11, BORDER + SPACING + y, SQUARE_DIM, SQUARE_DIM);
            }
            for (int i = 0; i < topleft.Item1.Count; i++)
            {
                int idx = topleft.Item1[i];
                int x = topleft.Item2[i] / 2;
                int y = topleft.Item3[i] / 2;
                mGraphics.FillRectangle(blackBrush, BORDER + SPACING + x - 11, BORDER + SPACING + y, SQUARE_DIM, SQUARE_DIM);
            }

            mGraphics.Dispose();
        }

        readonly Util ut = new();
        readonly IconMover im = new();

        private void ConfirmLayout_Click(object sender, EventArgs e)
        {
            Util.RefreshDesktop();
            // name, x, y, idx
            var folders = ut.GetDesktopFoldersFullInfo();
            im.OrganizeTopRight(folders, true);
            Util.RefreshDesktop();
            var shortcuts = ut.GetDesktopShortcutsFullInfo();
            im.OrganizeTopLeft(shortcuts, true);

            this.Close();
        }
    }
}
