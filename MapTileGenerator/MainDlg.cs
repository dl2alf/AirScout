using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MapTileGenerator
{
    public partial class MainDlg : Form
    {
        public MainDlg()
        {
            InitializeComponent();
        }

        private void Say(string text)
        {
            if (tsl_Main.Text != text)
            {
                tsl_Main.Text = text;
                ss_Main.Refresh();
            }
        }

        private void btn_Database_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.CheckFileExists = true;
            Dlg.Filter = "GMap.NET database files (*.gmdb)|*.gmdb";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                tb_Database.Text = Dlg.FileName;
                Properties.Settings.Default.Save();
                // change ScoutBase database location
                ScoutBase.Maps.Properties.Settings.Default.Database_UpdateURL = Properties.Settings.Default.Database;
            }
        }

        private void btn_MapTilesRootPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                tb_MapTilesRootPath.Text = Dlg.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            List<MapTileInfo> tiles = MapData.Database.TilesGetAll();
            int count = tiles.Count();
            int i = 1;
            foreach (MapTileInfo tile in tiles)
            {
                try
                {
                    string dirname = Path.Combine(Properties.Settings.Default.MapTilesRootPath, tile.Zoom.ToString(), tile.X.ToString());
                    if (!Directory.Exists(dirname))
                    {
                        Directory.CreateDirectory(dirname);
                    }
                    string filename = Path.Combine(dirname, tile.Y.ToString()) + ".png";
                    MapTileData d = MapData.Database.TileDataFind(tile.ID);
                    if (d != null && !File.Exists(filename) || (File.GetLastWriteTime(filename) != tile.CacheTime))
                    {
                        using (BinaryWriter bw = new BinaryWriter(File.Create(filename)))
                        {
                            bw.Write(d.Tile);
                        }
                        File.SetCreationTime(filename, tile.CacheTime);
                        File.SetLastWriteTime(filename, tile.CacheTime);
                        File.SetLastAccessTime(filename, tile.CacheTime);
                    }
                    Say("Saving [" + i.ToString() + " of " + count.ToString() + "] " + filename);
                }
                catch (Exception ex)
                {
                    Say(ex.Message);
                }
                i++;
                Application.DoEvents();
            }
            Say("Ready.");
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void CombineImages(string[] files)
        {
            if (files.Count() == 0)
                return;
            //change the location to store the final image.
            string finalImage = Path.Combine(Application.ExecutablePath, "zoom.png");
            List<int> imageHeights = new List<int>();
            int nIndex = 0;
            int width = 0;
            foreach (string file in files)
            {
                Image img = Image.FromFile(file);
                imageHeights.Add(img.Height);
                width += img.Width;
                img.Dispose();
            }
            imageHeights.Sort();
            int height = imageHeights[imageHeights.Count - 1];
            Bitmap img3 = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(img3);
            g.Clear(SystemColors.AppWorkspace);
            foreach (string file in files)
            {
                Image img = Image.FromFile(file);
                if (nIndex == 0)
                {
                    g.DrawImage(img, new Point(0, 0));
                    nIndex++;
                    width = img.Width;
                }
                else
                {
                    g.DrawImage(img, new Point(width, 0));
                    width += img.Width;
                }
                img.Dispose();
            }
            g.Dispose();
            img3.Save(finalImage, System.Drawing.Imaging.ImageFormat.Png);
            img3.Dispose();
            pb_Zoom.Image = Image.FromFile(finalImage);
        }

        private void btn_CreatePNG_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            {
                if (Dlg.ShowDialog() == DialogResult.OK)
                {
                    string[] files = Directory.GetFiles(Dlg.SelectedPath, "*.png");
                    Array.Sort(files);
                    CombineImages(files);
                }
            }
        }
    }
}
