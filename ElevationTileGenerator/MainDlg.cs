using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;
using ScoutBase;
using ScoutBase.Core;
using ScoutBase.Elevation;
using SQLiteDatabase;
using Ionic.Zip;
using Newtonsoft.Json;

namespace ElevationTileGenerator
{
    public partial class MainDlg : Form
    {
        DataTableSRTM3 srtm3 = new DataTableSRTM3();

        List<string> Files1 = new List<string>();
        List<string> Files2 = new List<string>();

        ElevationCatalogue Tiles = new ElevationCatalogue();

        public MainDlg()
        {
            InitializeComponent();
        }

        private void MainDlg_Load(object sender, EventArgs e)
        {
            //            string[] a = ElevationModel.Database.GetLocsFromRect(50, 0, 51, 2);
            // fill compare files catalogues
            if (!String.IsNullOrEmpty(tb_Tile1.Text))
            {
                // check for multiple files
                if (tb_Tile1.Text.Contains(";"))
                {
                    string[] a = tb_Tile1.Text.Split(';');
                    foreach (string file in a)
                        Files1.Add(file);
                }
                else
                    Files1.Add(tb_Tile1.Text);
            }
            if (!String.IsNullOrEmpty(tb_Tile2.Text))
            {
                // check for multiple files
                if (tb_Tile2.Text.Contains(";"))
                {
                    string[] a = tb_Tile2.Text.Split(';');
                    foreach (string file in a)
                        Files2.Add(file);
                }
                else
                    Files2.Add(tb_Tile2.Text);
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            Scan();
        }

        public bool IsGLOBEFile(string filename)
        {
            // checks for a valid filename with/witout extension
            // filename must have a xxxx. notation
            // [0]: 'A'..'P'
            // [1],[2]: nn = version
            // [3]: 'G','B','S','T' = status
            // 'S' and 'T' are not used
            if (String.IsNullOrEmpty(filename))
                return false;
            string upperfilename = Path.GetFileNameWithoutExtension(filename).ToUpper();
            // return on extension not empty
            string extension = Path.GetExtension(filename);
            if (!String.IsNullOrEmpty(extension))
                return false;
            char index = upperfilename[0];
            char status = upperfilename[3];
            // check length of filename
            if (upperfilename.Length != 4)
                return false;
            // check allowed chars
            if ((index < 'A')
                || (index > 'P')
                || !Char.IsDigit(upperfilename[1])
                || !Char.IsDigit(upperfilename[2])
                || ((status != 'G') && (status != 'B'))
                )
                return false;
            //check length
            long l = 0;
            switch (index)
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                    l = 4800 * 10800 * 2;
                    break;
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                    l = 6000 * 10800 * 2;
                    break;
            }
            if (l != new System.IO.FileInfo(filename).Length)
                return false;
            return true;
        }

        public bool IsSRTM3File(string filename)
        {
            // checks for a valid SRTM filename with extension
            // filename must have a XnnYnnn.hgt notation
            // [0]:         'N' or 'S'
            // [1],[2]:     nn = Latitude
            // [3]:         'E' or 'W'
            // [4],[5],[6]: nnn = Longitude
            if (String.IsNullOrEmpty(filename))
                return false;
            if (Path.GetExtension(filename).ToUpper() != ".HGT")
                return false;
            string upperfilename = Path.GetFileNameWithoutExtension(filename).ToUpper();
            if ((upperfilename[0] != 'N') && (upperfilename[0] != 'S'))
                return false;
            if ((upperfilename[3] != 'W') && (upperfilename[3] != 'E'))
                return false;
            if (!Char.IsDigit(upperfilename[1]))
                return false;
            if (!Char.IsDigit(upperfilename[2]))
                return false;
            if (!Char.IsDigit(upperfilename[4]))
                return false;
            if (!Char.IsDigit(upperfilename[5]))
                return false;
            if (!Char.IsDigit(upperfilename[6]))
                return false;
            // check file size
            long l = 1201 * 1201 * 2;
            if (l != new System.IO.FileInfo(filename).Length)
                return false;
            return true;
        }

        public bool IsSRTM1File(string filename)
        {
            // checks for a valid SRTM filename with extension
            // filename must have a XnnYnnn.hgt notation
            // [0]:         'N' or 'S'
            // [1],[2]:     nn = Latitude
            // [3]:         'E' or 'W'
            // [4],[5],[6]: nnn = Longitude
            if (String.IsNullOrEmpty(filename))
                return false;
            if (Path.GetExtension(filename).ToUpper() != ".HGT")
                return false;
            string upperfilename = Path.GetFileNameWithoutExtension(filename).ToUpper();
            if ((upperfilename[0] != 'N') && (upperfilename[0] != 'S'))
                return false;
            if ((upperfilename[3] != 'W') && (upperfilename[3] != 'E'))
                return false;
            if (!Char.IsDigit(upperfilename[1]))
                return false;
            if (!Char.IsDigit(upperfilename[2]))
                return false;
            if (!Char.IsDigit(upperfilename[4]))
                return false;
            if (!Char.IsDigit(upperfilename[5]))
                return false;
            if (!Char.IsDigit(upperfilename[6]))
                return false;
            // check file size
            long l = 3601 * 3601 * 2;
            if (l != new System.IO.FileInfo(filename).Length)
                return false;
            return true;
        }


        private void ProcessTile(string tilename, int current, int count)
        {
            if (IsSRTM3File(tilename) || IsSRTM1File(tilename))
            {
                string srtmindex = Path.GetFileNameWithoutExtension(tilename).ToUpper();
                int latbase = (srtmindex[0] == 'S') ? -System.Convert.ToInt32(srtmindex.Substring(1, 2)) : System.Convert.ToInt32(srtmindex.Substring(1, 2));
                int lonbase = (srtmindex[3] == 'W') ? -System.Convert.ToInt32(srtmindex.Substring(4, 3)) : System.Convert.ToInt32(srtmindex.Substring(4, 3));
                string tilebase = MaidenheadLocator.LocFromLatLon(latbase, lonbase, false, 3).Substring(0,4);

                // creating dir
                string dir = Path.Combine(Properties.Settings.Default.DestDir, tilebase.Substring(0, 2), tilebase.Substring(2, 2));
                int rows;
                int cols;
                if (IsSRTM3File(tilename))
                {
                    rows = 1201;
                    cols = 1201;
                }
                else
                {
                    rows = 3601;
                    cols = 3601;
                }
                FileStream fs = File.OpenRead(tilename);
                MemoryStream ms = new MemoryStream();
                ms.SetLength(fs.Length);
                fs.Read(ms.GetBuffer(), 0, (int)fs.Length);
                using (BinaryReader br = new BinaryReader(ms))
                {
                    bool all_void = true;
                    ElevationTileDesignator tile;
                    char x = (lonbase % 2 == 0) ? 'A' : 'M';
                    for (int i = 0; i < 12; i++)
                    {
                        char y = 'A';
                        for (int j = 0; j < 24; j++)
                        {
                            Stopwatch st = new Stopwatch();
                            st.Start();
                            string filename = tilebase + x + y + ".loc";
                            if (!File.Exists(Path.Combine(dir, filename)))
                            {
                                tile = new ElevationTileDesignator();
                                tile.TileIndex = tilebase + x + y;
                                tile.Rows = rows / 24;
                                tile.Columns = cols / 12;
                                short[,] b = new short[tile.Columns, tile.Rows];
                                byte[] a16 = new byte[2];
                                for (int k = 0; k < tile.Columns; k++)
                                {
                                    for (int l = 0; l < tile.Rows; l++)
                                    {
                                        br.BaseStream.Position = ((rows - 2 - j * tile.Rows - l) * cols + i * tile.Columns + k) * 2;
                                        br.Read(a16, 0, 2);
                                        Array.Reverse(a16);
                                        short e = BitConverter.ToInt16(a16, 0);
                                        b[k, l] = e;
                                        if ((e != ElevationData.Database.ElvMissingFlag) && (e < tile.MinElv))
                                        {
                                            tile.MinElv = e;
                                            tile.MinLon = tile.BaseLon + k * tile.StepWidthLon;
                                            tile.MinLat = tile.BaseLat + l * tile.StepWidthLat;
                                        }
                                        if ((e != ElevationData.Database.ElvMissingFlag) && (e > tile.MaxElv))
                                        {
                                            tile.MaxElv = e;
                                            tile.MaxLon = tile.BaseLon + k * tile.StepWidthLon;
                                            tile.MaxLat = tile.BaseLat + l * tile.StepWidthLat;
                                        }
                                        if (e != ElevationData.Database.ElvMissingFlag)
                                            all_void = false;
                                    }
                                }
                                tile.Elv = b;
                                tile.LastUpdated = DateTime.UtcNow;
                                if (!all_void)
                                {
                                    //                                ElevationModel.Database.ElevationTileInsertOrUpdateIfNewer(tile);
                                    // create dir if not exists
                                    if (!Directory.Exists(dir))
                                        Directory.CreateDirectory(dir);
                                    // write json file
                                    ms = new MemoryStream();
                                    using (StreamWriter sw = new StreamWriter(ms))
                                    {
                                        string json = tile.ToJSON();
                                        sw.WriteLine(json);
                                        sw.Flush();
                                        ms.Position = 0;
                                        File.WriteAllBytes(Path.Combine(dir, filename), ms.ToArray());
                                    }
                                    File.SetCreationTimeUtc(Path.Combine(dir, filename), tile.LastUpdated);
                                    File.SetLastWriteTimeUtc(Path.Combine(dir, filename), tile.LastUpdated);
                                }
                            }
                            st.Stop();
                            tsl_Status.Text = "[" + Path.GetFileNameWithoutExtension(tilename) + ", " + current.ToString() + " of " + count.ToString() + "] Processing tile: " + tilebase + x + y + ", " + st.ElapsedMilliseconds.ToString() + "ms";
                            Application.DoEvents();
                            y++;
                        }
                        x++;
                    }
                }
            }
            else if (IsGLOBEFile(tilename))
            {
                char index = Path.GetFileName(tilename.ToUpper())[0];
                int latbase;
                int lonbase;
                int rows;
                int cols;
                switch (index)
                {
                    case 'A':
                        latbase = 50;
                        lonbase = -180;
                        cols = 10800;
                        rows = 4800;
                        break;
                    case 'B':
                        latbase = 50;
                        lonbase = -90;
                        cols = 10800;
                        rows = 4800;
                        break;
                    case 'C':
                        latbase = 50;
                        lonbase = 0;
                        cols = 10800;
                        rows = 4800;
                        break;
                    case 'D':
                        latbase = 50;
                        lonbase = 90;
                        cols = 10800;
                        rows = 4800;
                        break;
                    case 'E':
                        latbase = 0;
                        lonbase = -180;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'F':
                        latbase = 0;
                        lonbase = -90;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'G':
                        latbase = 0;
                        lonbase = 0;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'H':
                        latbase = 0;
                        lonbase = 90;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'I':
                        latbase = -50;
                        lonbase = -180;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'J':
                        latbase = -50;
                        lonbase = -90;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'K':
                        latbase = -50;
                        lonbase = 0;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'L':
                        latbase = -50;
                        lonbase = 90;
                        cols = 10800;
                        rows = 6000;
                        break;
                    case 'M':
                        latbase = -90;
                        lonbase = -180;
                        cols = 10800;
                        rows = 4800;
                        break;
                    case 'N':
                        latbase = -90;
                        lonbase = -90;
                        cols = 10800;
                        rows = 4800;
                        break;
                    case 'O':
                        latbase = -90;
                        lonbase = -0;
                        cols = 10800;
                        rows = 4800;
                        break;
                    case 'P':
                        latbase = -90;
                        lonbase = 90;
                        cols = 10800;
                        rows = 4800;
                        break;
                    default:
                        latbase = 0;
                        lonbase = 0;
                        cols = 0;
                        rows = 0;
                        break;
                }

                string tilebase = MaidenheadLocator.LocFromLatLon(latbase, lonbase, false, 3).Substring(0, 4);
                FileStream fs = File.OpenRead(tilename);
                MemoryStream ms = new MemoryStream();
                ms.SetLength(fs.Length);
                fs.Read(ms.GetBuffer(), 0, (int)fs.Length);
                using (BinaryReader br = new BinaryReader(ms))
                {
                    ElevationTileDesignator tile;
                    for (int i0 = 0; i0 <= 4; i0++)
                    {
                        int j0max = (rows == 6000) ? 4 : 3;
                        for (int j0 = 0; j0 <= j0max; j0++)
                        {
                            int i1max = (i0 < 4) ? 9 : 4;
                            for (int i1 = 0; i1 <= i1max; i1++)
                            {
                                for (int j1 = 0; j1 <= 9; j1++)
                                {
                                    for (int i2 = 0; i2 < 24; i2++)
                                    {
                                        for (int j2 = 0; j2 < 24; j2++)
                                        {
                                            Stopwatch st = new Stopwatch();
                                            st.Start();
                                            // generate lat/lon from tile's midpoint
                                            double lat = latbase + j0 * 10.0 + j1 * 1.0 + j2 * 1.0 / 24.0 + 0.5 / 24.0;
                                            double lon = lonbase + i0 * 20.0 + i1 * 2.0 + i2 * 2.0 / 24.0 + 1.0 / 24.0;
                                            // generate filename from lat/lon
                                            string tileindex = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
                                            string filename = tileindex + ".loc";
                                            // creating dir
                                            string dir = Path.Combine(Properties.Settings.Default.DestDir, filename.Substring(0, 2), filename.Substring(2,2));
                                            if (!File.Exists(Path.Combine(dir, filename)))
                                            {
                                                bool all_void = true;
                                                tile = new ElevationTileDesignator();
                                                tile.TileIndex = tileindex;
                                                tile.Rows = (rows == 6000) ? rows / 50 / 24 : rows / 40 / 24;
                                                tile.Columns = cols / 90 / 24 * 2;
                                                short[,] b = new short[tile.Columns, tile.Rows];
                                                for (int k = 0; k < tile.Columns; k++)
                                                {
                                                    for (int l = 0; l < tile.Rows; l++)
                                                    {
                                                        br.BaseStream.Position = ((rows - 1 - j0 * 10 * 24 * tile.Rows - j1 * 24 * tile.Rows - j2 * tile.Rows - l) * cols + i0 * 10 * 24 * tile.Columns + i1 * 24 * tile.Columns + i2 * tile.Columns + k) * 2;
                                                        short e = br.ReadInt16();
                                                        if (e <= -500)
                                                            e = ElevationData.Database.ElvMissingFlag;
                                                        b[k, l] = e;
                                                        if ((e != ElevationData.Database.ElvMissingFlag) && (e < tile.MinElv))
                                                        {
                                                            tile.MinElv = e;
                                                            tile.MinLon = tile.BaseLon + k * tile.StepWidthLon;
                                                            tile.MinLat = tile.BaseLat + l * tile.StepWidthLat;
                                                        }
                                                        if ((e != ElevationData.Database.ElvMissingFlag) && (e > tile.MaxElv))
                                                        {
                                                            tile.MaxElv = e;
                                                            tile.MaxLon = tile.BaseLon + k * tile.StepWidthLon;
                                                            tile.MaxLat = tile.BaseLat + l * tile.StepWidthLat;
                                                        }
                                                        if (e != ElevationData.Database.ElvMissingFlag)
                                                            all_void = false;
                                                    }
                                                }
                                                tile.Elv = b;
                                                tile.LastUpdated = DateTime.UtcNow;
                                                if (!all_void)
                                                {
                                                    //                                ElevationModel.Database.ElevationTileInsertOrUpdateIfNewer(tile);
                                                    // create dir if not exists
                                                    try
                                                    {
                                                        if (!Directory.Exists(dir))
                                                            Directory.CreateDirectory(dir);
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    // write json file
                                                    ms = new MemoryStream();
                                                    using (StreamWriter sw = new StreamWriter(ms))
                                                    {
                                                        string json = tile.ToJSON();
                                                        sw.WriteLine(json);
                                                        sw.Flush();
                                                        ms.Position = 0;
                                                        File.WriteAllBytes(Path.Combine(dir, filename), ms.ToArray());
                                                    }
                                                    File.SetCreationTimeUtc(Path.Combine(dir, filename), tile.LastUpdated);
                                                    File.SetLastWriteTimeUtc(Path.Combine(dir, filename), tile.LastUpdated);
                                                }
                                            }
                                            st.Stop();
                                            tsl_Status.Text = "[" + Path.GetFileNameWithoutExtension(tilename) + ", " + current.ToString() + " of " + count.ToString() + "] Processing tile: " + filename + ", " + st.ElapsedMilliseconds.ToString() + "ms";
                                            ss_Main.Refresh();
                                            Application.DoEvents();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Scan()
        {
            //            string[] files = Directory.GetFiles(Properties.Settings.Default.SourceDir);
            //            foreach (string file in files)
            //                ProcessTile(file);
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.DefaultExt = "*.loc";
            Dlg.Multiselect = true;
            Dlg.InitialDirectory = Properties.Settings.Default.SourceDir;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                int i = 1;
                foreach (string file in Dlg.FileNames)
                {
                    ProcessTile(file, i, Dlg.FileNames.Count());
                    i++;
                }
            }
        }

        private void ZIP()
        {
            int i = 1;
            char[] a = new char[2];
            char[] b = new char[2];
            for (a[0] = 'A'; a[0] <= 'R'; a[0]++)
            {
                for (a[1] = 'A'; a[1] <= 'R'; a[1]++)
                {
                    for (b[0] = '0'; b[0] <= '9'; b[0]++)
                    {
                        for (b[1] = '0'; b[1] <= '9'; b[1]++)
                        {
                            Stopwatch st = new Stopwatch();
                            st.Start();
                            string dira = Path.Combine(Properties.Settings.Default.DestDir, new string(a), new string(b));
                            string dirb = Path.Combine(Properties.Settings.Default.DestDir, new string(a));
                            if (Directory.Exists(dira))
                            {
                                string[] files = Directory.GetFiles(dira, "*.loc");
                                if (files.Length > 0)
                                {
                                    // create ZIP file and add all loc files
                                    string zipfilename = Path.Combine(dirb, new string(a) + new string(b) + ".zip");
                                    if (File.Exists(zipfilename))
                                        File.Delete(zipfilename);
                                    ZipFile zip = new ZipFile(zipfilename);
                                    zip.AddFiles(files, "");
                                    zip.Save();
                                    // sleep 5s to ensure that file is written
                                    Thread.Sleep(1000);
                                }
                                foreach (string file in files)
                                    File.Delete(file);
                                Directory.Delete(dira);
                            }

                            st.Stop();
                            tsl_Status.Text = "Processing dir: " + dira + ", " + st.ElapsedMilliseconds.ToString() + "ms.";
                            Application.DoEvents();
                            i++;
                        }
                    }
                }
            }
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void btn_SourceDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            Dlg.SelectedPath = Properties.Settings.Default.SourceDir;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.SourceDir = Dlg.SelectedPath;
            }
        }

        private void btn_DestDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            Dlg.SelectedPath = Properties.Settings.Default.DestDir;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.DestDir = Dlg.SelectedPath;
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Picture_Click(object sender, EventArgs e)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            Bitmap bm  = ElevationData.Database.DrawElevationBitmap(50, 0, 60, 20,3200, 3200, ELEVATIONMODEL.SRTM1);
            bm.Save("Elevation.tif", System.Drawing.Imaging.ImageFormat.Tiff);
            pb_Picture.Image = bm;
            st.Stop();
            tsl_Status.Text = "Drawing picture: " + st.ElapsedMilliseconds.ToString() + "ms.";
            this.Refresh();
        }

        private void btn_ZIP_Click(object sender, EventArgs e)
        {
            ZIP();
        }

        public T GetFirstInstance<T>(string propertyName, string json)
        {
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                int level = 0;

                while (jsonReader.Read())
                {
                    switch (jsonReader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            if (level != 1)
                                break;
                            if ((string)jsonReader.Value == propertyName)
                            {
                                jsonReader.Read();

                                return (T)jsonReader.Value;
                            }
                            break;

                        case JsonToken.StartArray:
                        case JsonToken.StartConstructor:
                        case JsonToken.StartObject:
                            level++;
                            break;

                        case JsonToken.EndArray:
                        case JsonToken.EndConstructor:
                        case JsonToken.EndObject:
                            level--;
                            break;
                    }

                }
                return default(T);
            }
        }
        private void ScanDir(string dir)
        {
            tsl_Status.Text = "Scanning directory: " + dir;
            this.Refresh();
            Application.DoEvents();
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string d in dirs)
                ScanDir(d);
            string[] files = Directory.GetFiles(dir, "*.loc");
            foreach (string file in files)
            {
                try
                {
                    DateTime lastupdated = File.GetCreationTimeUtc(file);
                    Tiles.Files.Add(Path.GetFileName(file), lastupdated);
                    /*
                    using (StreamReader sr = new StreamReader(File.OpenRead(file)))
                    {
                        string json = sr.ReadToEnd();
                        // ElevationTileDesignator tile = ElevationTileDesignator.FromJSON<ElevationTileDesignator>(json);
                        json = "{\"" + json.Remove(0, json.LastIndexOf("LastUpdated"));
                        lastupdated = GetFirstInstance<DateTime>("LastUpdated", json);
                        Tiles.Files.Add(Path.GetFileName(file), lastupdated);
                    }
                    */
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ScanDirZIP(string dir)
        {
            tsl_Status.Text = "Scanning directory: " + dir;
            this.Refresh();
            Application.DoEvents();
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string d in dirs)
                ScanDirZIP(d);
            string[] zipfiles = Directory.GetFiles(dir, "*.zip");
            foreach (string zipfile in zipfiles)
            {
                ZipFile zip = new ZipFile(zipfile);
                foreach (ZipEntry file in zip.Entries)
                {
                    Tiles.Files.Add(Path.GetFileName(file.FileName), file.CreationTime.ToUniversalTime());
                }
            }
        }

        private void CAT()
        {
            Tiles.Files.Clear();
            ScanDir(Properties.Settings.Default.DestDir);
            tsl_Status.Text = "Scanned: " + Tiles.Files.Count + " files.";
            this.Refresh();
            string filename = Path.Combine(Properties.Settings.Default.DestDir, "files.cat");
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("// (D)igital (E)levation (Model) tile catalogue");
                sw.WriteLine("// created " + DateTime.UtcNow.ToString("yyyy-MM-dd") + " (c) DL2ALF");
                sw.WriteLine("// using elevation data from the GLOBE and SRTM (V3) projects, voids filled with the help of ASTER data");
                sw.WriteLine("// GLOBE data homepage: https://www.ngdc.noaa.gov/mgg/topo/globe.html");
                sw.WriteLine("// SRTM3 data homepage: https://lpdaac.usgs.gov/dataset_discovery/measures/measures_products_table/srtmgl3_v003");
                sw.WriteLine("// SRTM1 data homepage: https://lpdaac.usgs.gov/dataset_discovery/measures/measures_products_table/srtmgl1_v003");
                sw.WriteLine("// ASTER GDEM is a product of NASA and METI. See links above for details.");
                sw.WriteLine();
                foreach (KeyValuePair<string, DateTime> tile in Tiles.Files)
                {
                    sw.WriteLine(tile.Key + ";" + tile.Value.ToString("yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture));
                }
            }
            string zipfilename = filename.Replace(".cat", ".zip");
            if (File.Exists(zipfilename))
                File.Delete(zipfilename);
            ZipFile zip = new ZipFile(zipfilename);
            zip.AddFile(filename, "");
            zip.Save();
        }

        private void CATFromZIP()
        {
            Tiles.Files.Clear();
            ScanDirZIP(Properties.Settings.Default.DestDir);
            tsl_Status.Text = "Scanned: " + Tiles.Files.Count + " files.";
            this.Refresh();
            Application.DoEvents();
            // string json = Tiles.ToJSON();
            string filename = Path.Combine(Properties.Settings.Default.DestDir, "files.cat");
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("// (D)igital (E)levation (Model) tile catalogue");
                sw.WriteLine("// created " + DateTime.UtcNow.ToString("yyyy-MM-dd") + " (c) DL2ALF");
                sw.WriteLine("// using elevation data from the GLOBE and SRTM (V3) projects, voids filled with the help of ASTER data");
                sw.WriteLine("// GLOBE data homepage: https://www.ngdc.noaa.gov/mgg/topo/globe.html");
                sw.WriteLine("// SRTM3 data homepage: https://lpdaac.usgs.gov/dataset_discovery/measures/measures_products_table/srtmgl3_v003");
                sw.WriteLine("// SRTM1 data homepage: https://lpdaac.usgs.gov/dataset_discovery/measures/measures_products_table/srtmgl1_v003");
                sw.WriteLine("// ASTER GDEM is a product of NASA and METI. See links above for details.");
                sw.WriteLine();
                foreach (KeyValuePair<string, DateTime> tile in Tiles.Files)
                {
                    sw.WriteLine(tile.Key + ";" + tile.Value.ToString("yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture));
                }
            }
            string zipfilename = filename.Replace(".cat", ".zip");
            if (File.Exists(zipfilename))
                File.Delete(zipfilename);
            ZipFile zip = new ZipFile(zipfilename);
            zip.AddFile(filename, "");
            zip.Save();
        }

        private void btn_CAT_Click(object sender, EventArgs e)
        {
            CAT();
        }

        private void TestCAT()
        {
            // ElevationCatalogue cat = new ElevationCatalogue("http://www.airscout.eu/downloads/ElevationData/SRTM3", Application.StartupPath, 35, -15, 60, 30);
        }

        private void btn_TestCAT_Click(object sender, EventArgs e)
        {
            TestCAT();
        }

        private void btn_CATFromZIP_Click(object sender, EventArgs e)
        {
            CATFromZIP();
        }

        private void PictureFromTiles(string[] files, bool createTIFFromTiles = false)
        {
            Bitmap bm = null;
            foreach (string file in files)
            {
                tsl_Status.Text = "Processing " + Path.GetFileName(file) + "...";
                Application.DoEvents();
                using (StreamReader sr = new StreamReader(File.OpenRead(file)))
                {
                    string json = sr.ReadToEnd();
                    ElevationTileDesignator tile = ElevationTileDesignator.FromJSON<ElevationTileDesignator>(json);
                    // create bitmap if not created
                    if (bm == null)
                    {
                        bm = new Bitmap(tile.Columns * 24, tile.Rows * 24);
                    }
                    Bitmap bt = tile.ToBitmap();
                    if (createTIFFromTiles)
                        bt.Save(file.Replace(".loc",".tif"), System.Drawing.Imaging.ImageFormat.Tiff);
                    int col = (int)(tile.TileIndex[4] - 'A') * tile.Columns;
                    int row = (int)(tile.TileIndex[5] - 'A') * tile.Rows;
                    for (int i = 0; i < tile.Columns; i++)
                    {
                        for (int j = 0; j < tile.Rows; j++)
                        {
                            bm.SetPixel(col + i, bm.Height - row - bt.Height + j, bt.GetPixel(i, j));
                        }
                    }
                    pb_Picture.Image = bm;
                }
            }
            if (bm != null)
                bm.Save("ElevationTiles.tif", System.Drawing.Imaging.ImageFormat.Tiff);
        }
    
        private void btn_PictureFromTiles_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            Dlg.SelectedPath = Properties.Settings.Default.DestDir;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(Dlg.SelectedPath, "*.loc");
            }
        }

        private void ScanDirDelete(string dir)
        {
            tsl_Status.Text = "Scanning directory: " + dir;
            this.Refresh();
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string d in dirs)
                ScanDirZIP(d);
            string[] delfiles = Directory.GetFiles(dir, "*.*");
            foreach (string delfile in delfiles)
            {
                try
                {
                    File.Delete(delfile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void Delete()
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            Dlg.SelectedPath = Properties.Settings.Default.DestDir;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                ScanDirDelete(Dlg.SelectedPath);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Delete();    
        }

        private void Say(string txt)
        {
            if (tsl_Status.Text != txt)
            {
                tsl_Status.Text = txt;
                this.Refresh();
                Application.DoEvents();
            }
        }

        private void btn_Compare_Tile1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.CheckFileExists = true;
            Dlg.Multiselect = true;
            Dlg.Filter = "Elevation Files (*.hgt)|*.hgt|Zipped Elevation Files (*.zip)|*.zip";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Files1.Clear();
                if (Dlg.FileNames.Length > 1)
                {
                    // multiple files selected
                    tb_Tile1.Text = string.Join(";", Dlg.FileNames);
                }
                else
                {
                    // unzip if zip file selected
                    if (Path.GetExtension(Dlg.FileName) == ".zip")
                    {
                        // create ZIP file and add all loc files
                        ZipFile zip = new ZipFile(Dlg.FileName);
                        foreach (string filename in zip.EntryFileNames)
                        {
                            Files1.Add(Path.Combine(Path.GetTempPath(), "Files1", filename));
                        }
                        zip.ExtractAll(Path.Combine(Path.GetTempPath(), "Files1"), ExtractExistingFileAction.OverwriteSilently);
                        bool filemissing;
                        do
                        {
                            filemissing = false;
                            foreach (String filename in Files1)
                            {
                                if (!File.Exists(filename))
                                {
                                    Application.DoEvents();
                                    filemissing = true;
                                    break;
                                }
                            }
                            Thread.Sleep(1000);
                        }
                        while (filemissing);
                    }
                    else
                    {
                        tb_Tile1.Text = Dlg.FileName;
                        Files1.Add(Dlg.FileName);
                    }
                }
            }
            Properties.Settings.Default.Save();
        }

        private void btn_Compare_Tile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.CheckFileExists = true;
            Dlg.Multiselect = true;
            Dlg.Filter = "Elevation Files (*.hgt)|*.hgt|Zipped Elevation Files (*.zip)|*.zip";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Files2.Clear();
                if (Dlg.FileNames.Length > 1)
                {
                    // multiple files selected
                    tb_Tile2.Text = string.Join(";", Dlg.FileNames);
                }
                else
                {
                    // unzip if zip file selected
                    if (Path.GetExtension(Dlg.FileName) == ".zip")
                    {
                        // create ZIP file and add all loc files
                        ZipFile zip = new ZipFile(Dlg.FileName);
                        foreach (string filename in zip.EntryFileNames)
                        {
                            Files2.Add(Path.Combine(Path.GetTempPath(), "Files2", filename));
                        }
                        zip.ExtractAll(Path.Combine(Path.GetTempPath(), "Files2"), ExtractExistingFileAction.OverwriteSilently);
                        bool filemissing;
                        do
                        {
                            filemissing = false;
                            foreach (String filename in Files2)
                            {
                                if (!File.Exists(filename))
                                {
                                    Application.DoEvents();
                                    filemissing = true;
                                    break;
                                }
                            }
                            Thread.Sleep(1000);
                        }
                        while (filemissing);
                    }
                    else
                    {
                        tb_Tile2.Text = Dlg.FileName;
                        Files2.Add(Dlg.FileName);
                    }
                }
            }
            Properties.Settings.Default.Save();
        }

        private string FindinFiles2(string filename1)
        {
            foreach(string filename2 in Files2)
            {
                if (Path.GetFileName(filename1) == Path.GetFileName(filename2))
                    return filename2;
            }
            return "";
        }

        public Bitmap DrawElevationBitmapHGT(string filename)
        {
            int minelv = 0;
            int maxelv = 0;
            long length = new System.IO.FileInfo(filename).Length / 2;
            // calculate bitmap dimensions
            // get the x/y ratio first
            int width = (int)Math.Sqrt(length);
            int height = width;
            short[,] elv = new short[width,height];
            long diff = 0;
            long voids = 0;
            long count = 0;
            long sum = 0;
            using (var filestream = File.Open(filename, FileMode.Open))
            using (var binaryStream = new BinaryReader(filestream))
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        elv[i, j] = binaryStream.ReadInt16();
                        if (elv[i, j] != 0)
                            diff++;
                        if (elv[i, j] == -512)
                            voids++;
                        if (elv[i, j] < minelv)
                            minelv = elv[i, j];
                        if (elv[i, j] > maxelv)
                            maxelv = elv[i, j];
                        count++;
                        if (count % 100000 == 0)
                            Say("Processing " + count.ToString() + " of " + length.ToString() + " points");
                    }
                }
            }
            long[] d = new long[maxelv - minelv + 1];
            for (int i = 0; i < d.Length; i++)
                d[i] = 0;
            DEMColorPalette palette = new DEMColorPalette();
            Bitmap bm = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                // System.Console.WriteLine(i);
                for (int j = 0; j < height; j++)
                {
                    double e = (double)(elv[i,j] - minelv) / (double)(maxelv - minelv) * 100.0;
                    if (e < 0)
                        e = 0;
                    if (e > 100)
                        e = 100;
                    // count occurence of difference in an array
                    d[elv[i,j] - minelv]++;
                    sum += elv[i, j];
                    bm.SetPixel(i, height - j - 1, palette.GetColor(e));
                }
            }
            using (StreamWriter sw = new StreamWriter("ElevationDifference.csv"))
            {
                sw.WriteLine("Difference[m];Count");
                for (int i = 0; i < d.Length; i++)
                {
                    sw.WriteLine((i + minelv).ToString() + ";" + d[i].ToString());
                }
            }
            lbl_Diff_Dimensions.Text = width.ToString() + "x" + height.ToString();
            lbl_Diff_Total.Text = diff.ToString();
            lbl_Diff_Voids.Text = voids.ToString();
            lbl_Diff_Min.Text = minelv.ToString();
            lbl_Diff_Max.Text = maxelv.ToString();
            lbl_Diff_Mean.Text = (sum / length).ToString("F2");
            return bm;
        }

        private string CompareFilesHGT(string filename1, string filename2)
        {
            string difffilename = Path.Combine(Application.StartupPath, Path.GetFileName(filename1).Replace(".hgt",".hgtdiff"));
            long length1 = new System.IO.FileInfo(filename1).Length / 2;
            long length2 = new System.IO.FileInfo(filename2).Length / 2;
            if (length1 != length2)
            {
                MessageBox.Show("Cannot compare files with different length: " + filename1 + "<>" + filename2);
                return "";
            }
            long l = 0;
            long diff = 0;
            // delete diff file
            try
            {
                File.Delete(difffilename);
                FileStream fs1 = File.OpenRead(filename1);
                FileStream fs2 = File.OpenRead(filename2);
                using (BinaryReader br1 = new BinaryReader(fs1))
                {
                    using (BinaryReader br2 = new BinaryReader(fs2))
                    {
                        using (BinaryWriter bw = new BinaryWriter(File.Create(difffilename)))
                        {
                            while (br1.BaseStream.Position < br1.BaseStream.Length)
                            {
                                byte[] b1 = br1.ReadBytes(2);
                                byte[] b2 = br2.ReadBytes(2);
                                Array.Reverse(b1);
                                Array.Reverse(b2);
                                short e1 = BitConverter.ToInt16(b1, 0);
                                short e2 = BitConverter.ToInt16(b2, 0);
                                short e = ElevationData.Database.ElvMissingFlag;
                                if ((e1 >= -500) && (e2 >= -500))
                                {
                                    e = (short)(e1 - e2);
                                    if (e > 1000)
                                        Console.WriteLine("Error at position:" + br1.BaseStream.Position.ToString("0X"));
                                    if (e != 0)
                                        diff++;
                                }
                                bw.Write(e);
                                if (l % 100000 == 0)
                                {
                                    Say("Comparing point " + l.ToString() + " of " + length1.ToString() + ": " + diff.ToString() + " differences");
                                }
                                l++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return difffilename;
        }

        private void CompareFiles(string filename1, string filename2)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            if (Path.GetExtension(filename1) == ".hgt")
            {
                string difffilename = CompareFilesHGT(filename1, filename2);
                tb_Diff.Text = difffilename;
                Bitmap bm = DrawElevationBitmapHGT(difffilename);
                pb_Picture.Image = bm;
                bm.Save("ElevationDifference.tif", System.Drawing.Imaging.ImageFormat.Tiff);
                pb_Picture.Image = bm;
            }
            st.Stop();
            tsl_Status.Text = "Compare files: " + st.ElapsedMilliseconds.ToString() + "ms.";
            this.Refresh();
        }

        private void btn_Compare_Click(object sender, EventArgs e)
        {
            foreach (string filename1 in Files1)
            {
                string filename2 = FindinFiles2(filename1);
                if (!String.IsNullOrEmpty(filename2))
                {
                    CompareFiles(filename1, filename2);
                }
            }
        }

        private void btn_ShowDiff_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.CheckFileExists = true;
            Dlg.Multiselect = false;
            Dlg.Filter = "Elevation Difference Files (*.hgtdiff)|*.hgtdiff";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                tb_Show_Diff.Text = Dlg.FileName;
                Bitmap bm = DrawElevationBitmapHGT(tb_Show_Diff.Text);
                pb_Diff.Image = bm;
            }
            Properties.Settings.Default.Save();
        }

        private void btn_PictureFromZIP_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.CheckFileExists = true;
            Dlg.Multiselect = false;
            Dlg.Filter = "Zipped Elevation Files (*.zip)|*.zip";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                string[] delfiles = Directory.GetFiles(Path.GetTempPath(), "*.loc");
                foreach (string file in delfiles)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // do nothing
                    }
                }
                // unzip if zip file selected
                if (Path.GetExtension(Dlg.FileName) == ".zip")
                {
                    // create ZIP file and add all loc files
                    ZipFile zip = new ZipFile(Dlg.FileName);
                    zip.ExtractAll(Path.GetTempPath(), ExtractExistingFileAction.OverwriteSilently);
                    bool filemissing;
                    do
                    {
                        filemissing = false;
                        foreach (String filename in Files1)
                        {
                            if (!File.Exists(filename))
                            {
                                Application.DoEvents();
                                filemissing = true;
                                break;
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    while (filemissing);
                    List<string> files = new List<string>();
                    foreach (string file in zip.EntryFileNames)
                        files.Add(Path.Combine(Path.GetTempPath(), file));
                    PictureFromTiles(files.ToArray());
                }
                else
                {
                    tb_Tile1.Text = Dlg.FileName;
                    Files1.Add(Dlg.FileName);
                }
            }
        }

        private short GetSRTM1Elevation(BinaryReader br, int x, int y)
        {
            int rows = 3601;
            int cols = 3601;
            long l = (x * cols + y) * 2;
            br.BaseStream.Seek(l,0);
            byte[] b = br.ReadBytes(2);
            Array.Reverse(b);
            short e = BitConverter.ToInt16(b, 0);
            return e;
        }
        private void ConvertSRTM1ToSRTM3File(string srtm1file, string srtm3file)
        {
            Say("Processing " + srtm1file);
            using (BinaryReader br = new BinaryReader(File.OpenRead(srtm1file)))
            {
                using (BinaryWriter bw = new BinaryWriter(File.Create(srtm3file)))
                {
                    short e = 0;
                    for (int i = 0; i < 1200; i++)
                    {
                        for (int j = 0; j < 1200; j++)
                        {
                            e = 0;
                            for (int k = 0; k < 3; k++)
                            {
                                for (int l = 0; l < 3; l++)
                                {
                                    short elv = GetSRTM1Elevation(br, i * 3 + k, j * 3 + l);
                                    e += elv;
                                }
                            }
                            e = (short)(e / 9);
                            bw.BaseStream.WriteByte((byte)((e >> 8) & 0xFF));
                            bw.BaseStream.WriteByte((byte)(e & 0xFF));
                        }
                        // write point #1201
                        bw.BaseStream.WriteByte((byte)((e >> 8) & 0xFF));
                        bw.BaseStream.WriteByte((byte)(e & 0xFF));
                    }
                    for (int j = 0; j < 1201; j++)
                    {
                        e= GetSRTM1Elevation(br, 3600, j * 3);
                        bw.BaseStream.WriteByte((byte)((e >> 8) & 0xFF));
                        bw.BaseStream.WriteByte((byte)(e & 0xFF));
                    }
                }
            }
            Say("Ready.");
        }

        private void btn_ConvertSRTM1ToSRTM3_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Properties.Settings.Default.SourceDir, "*.hgt");
            foreach (string srtm1file in files)
            {
                if (!IsSRTM1File(srtm1file))
                    continue;
                string srtm3file = Path.Combine(Properties.Settings.Default.DestDir, Path.GetFileName(srtm1file));
                ConvertSRTM1ToSRTM3File(srtm1file, srtm3file);
            }
        }

        private void btn_Coverage_Click(object sender, EventArgs e)
        {
            CoverageDlg Dlg = new CoverageDlg();
            Dlg.ShowDialog();
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableSRTM3 : DataTable
    {
        public DataTableSRTM3()
            : base()
        {
            // set table name
            TableName = "SRTM3Tiles";
            // create all specific columns
            DataColumn FileIndex = this.Columns.Add("FileIndex", typeof(string));
            DataColumn MinLat = this.Columns.Add("MinLat", typeof(double));
            DataColumn MaxLat = this.Columns.Add("MaxLat", typeof(double));
            DataColumn MinLon = this.Columns.Add("MinLon", typeof(double));
            DataColumn MaxLon = this.Columns.Add("MaxLon", typeof(double));
            DataColumn MinElv = this.Columns.Add("MinElv", typeof(int));
            DataColumn MaxElv = this.Columns.Add("MaxElv", typeof(int));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = FileIndex;
            this.PrimaryKey = keys;
        }

    }

}
