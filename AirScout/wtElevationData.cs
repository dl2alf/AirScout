using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using SphericalEarth;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AirScout
{
    public class wtElevationData
    {

        private int[,] Data = null;

        public int number_of_rows = 0;
        public int number_of_columns = 0;

        public double left_map_x = -10.00000000000;
        public double right_map_x = 20.00000000000;
        public double upper_map_y = 60.00000000000;
        public double lower_map_y = 35.00000000000;
        public double grid_size = 0.00833333333;

        public int elev_m_min = -30;
        public int elev_m_max = 4570;
        public static int elev_m_missing_flag = -500;

        static int latbase = int.MinValue;
        static int lonbase = int.MinValue;
        static string hgtfilename = "";
        static bool hgtexists = false;
        static BinaryReader strmbr = null;
        static int xysize = int.MinValue;

        static GLOBETile currenttile;
        static BinaryReader globebr = null;
        public static int globe_elv_missing = -500;

        GLOBETileCollection GLOBETiles;

        public wtElevationData()
        {
            GLOBETiles = new GLOBETileCollection(Properties.Settings.Default.Elevation_GLOBE_DataPath);
            foreach (GLOBETile tile in GLOBETiles)
            {
                Bitmap bm = DrawGLOBEBitmap(tile);ergrthrt
                bm.Save(tile.Name + ".jpg", ImageFormat.Jpeg);
            }

            // uses elevation data from the (G)lobal (L)and (O)ne-km (B)ase (E)levation
            /*
            try
            {
                // search for header file in the given directory
                string[] files = Directory.GetFiles(Application.StartupPath + "\\" + Properties.Settings.Default.Elevation_GLOBE_DataPath, "*.hdr");
                //  must be exactly 1 header file in the dir
                if (files.Length == 1)
                {
                    Properties.Settings.Default.Elevation_GLOBE_HeaderFileName = files[0];
                    Properties.Settings.Default.Elevation_GLOBE_DataFileName = Path.ChangeExtension(Properties.Settings.Default.Elevation_GLOBE_HeaderFileName, ".bin");
                    // get an EN - formatinfo to convert the doubles
                    NumberFormatInfo provider = new NumberFormatInfo();
                    provider.NumberDecimalSeparator = ".";
                    provider.NumberGroupSeparator = ",";
                    provider.NumberGroupSizes = new int[] { 3 };
                    // processing the header file first
                    using (StreamReader sr = new StreamReader(Properties.Settings.Default.Elevation_GLOBE_HeaderFileName))
                    {
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            if (s.IndexOf("left_map_x") >= 0)
                            {
                                left_map_x = System.Convert.ToDouble(s.Remove(0, s.IndexOf("=") + 1), provider);
                            }
                            else if (s.IndexOf("right_map_x") >= 0)
                            {
                                right_map_x = System.Convert.ToDouble(s.Remove(0, s.IndexOf("=") + 1), provider);
                            }
                            else if (s.IndexOf("upper_map_y") >= 0)
                            {
                                upper_map_y = System.Convert.ToDouble(s.Remove(0, s.IndexOf("=") + 1), provider);
                            }
                            else if (s.IndexOf("lower_map_y") >= 0)
                            {
                                lower_map_y = System.Convert.ToDouble(s.Remove(0, s.IndexOf("=") + 1), provider);
                            }
                            else if (s.IndexOf("number_of_rows") >= 0)
                            {
                                number_of_rows = System.Convert.ToInt32(s.Remove(0, s.IndexOf("=") + 1));
                            }
                            else if (s.IndexOf("number_of_columns") >= 0)
                            {
                                number_of_columns = System.Convert.ToInt32(s.Remove(0, s.IndexOf("=") + 1));
                            }
                            else if (s.IndexOf("grid_size") >= 0)
                            {
                                grid_size = System.Convert.ToDouble(s.Remove(0, s.IndexOf("=") + 1), provider);
                            }
                            else if (s.IndexOf("elev_m_min") >= 0)
                            {
                                elev_m_min = System.Convert.ToInt32(s.Remove(0, s.IndexOf("=") + 1));
                            }
                            else if (s.IndexOf("elev_m_max") >= 0)
                            {
                                elev_m_max = System.Convert.ToInt32(s.Remove(0, s.IndexOf("=") + 1));
                            }
                            else if (s.IndexOf("elev_m_missing_flag") >= 0)
                            {
                                elev_m_missing_flag = System.Convert.ToInt32(s.Remove(0, s.IndexOf("=") + 1));
                            }
                        }
                    }
                    Data = new int[number_of_columns, number_of_rows];
                    using (Stream stream = File.Open(Properties.Settings.Default.Elevation_GLOBE_DataFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        byte[] buf = new byte[2];
                        for (int i = 0; i < number_of_rows; i++)
                        {
                            for (int j = 0; j < number_of_columns; j++)
                            {
                                stream.Read(buf, 0, 2);
                                Data[j, number_of_rows - i - 1] = BitConverter.ToInt16(buf, 0);
                            }
                        }
                    }
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error while reading " + Properties.Settings.Default.Elevation_GLOBE_DataFileName + "\n" + e1);
            }
             */
        }

        public Bitmap DrawGLOBEBitmap(GLOBETile tile)
        {
            byte[] a16 = new byte[2];
            DEMColorPalette palette = new DEMColorPalette();
            byte[] buffer;
            short[] Data = new short[tile.Columns * tile.Rows];
            Bitmap bm = new Bitmap(tile.Columns, tile.Rows);
            int index = 0;
            using (BinaryReader br = new BinaryReader (File.OpenRead(tile.Name)))
            {
                buffer = br.ReadBytes(Data.Length * 2);
                Buffer.BlockCopy(buffer, 0, Data, 0, buffer.Length);
                index++;
            }
            for (int i = 0; i < tile.Rows; i++)
            {
                // System.Console.WriteLine(i);
                for (int j = 0; j < tile.Columns; j++)
                {
                    int e1 = Data[(i * tile.Columns + j)];
                    if (e1 != elev_m_missing_flag)
                    {
                        double e = (double)(e1 - tile.MinElv) / (double)(tile.MaxElv - tile.MinElv) * 100.0;
                        if (e < 0)
                            e = 0;
                        if (e > 100)
                            e = 100;
                        bm.SetPixel(j, i, palette.GetColor(e));
                    }
                    else
                    {
                        bm.SetPixel(j, i, Color.FromArgb(0, 0, 128));
                    }
                }
            }
            return bm;
        }

        public ElvMaxInfo GetMaxElvLoc (string loc)
        {
            if (!(WCCheck.WCCheck.IsLoc(loc) > 0))
                return null;
            double startlat = ((int)(LatLon.Lat(loc) * 24.0)) / 24.0;
            double startlon = ((int)(LatLon.Lon(loc) * 12.0)) / 12.0;
            double stoplat = startlat + 1.0/24.0;
            double stoplon = startlon + 2.0/24.0;
            double stepwidthi = (stoplat - startlat) / 100.0;
            double stepwidthj = (stoplon - startlon) / 100.0;
            ElvMaxInfo maxinfo = new ElvMaxInfo(int.MinValue, 0, 0);
            for (double i = startlat; i <= stoplat; i += stepwidthi)
            {
                for (double j = startlon; j <= stoplon; j += stepwidthj)
                {
                    int elv = this[i, j];
                    if (elv > maxinfo.Elv)
                    {
                        maxinfo.Elv = elv;
                        maxinfo.Lat = i;
                        maxinfo.Lon = j;
                    }
                }
            }
            return maxinfo;
        }

        public int this[double lat, double lon]
        {
            get
            {
                lock (this)
                {
                    int e = elev_m_missing_flag;
                    // get elevation data from SRTM1 if enabled
                    if (Properties.Settings.Default.Elevation_SRTM1_Enabled)
                    {
                        e = GetSRTMElevation(Properties.Settings.Default.Elevation_SRTM1_DataPath, lat, lon);
                        if (e != elev_m_missing_flag)
                            return e;
                    }
                    else if (Properties.Settings.Default.Elevation_SRTM3_Enabled)
                    {
                        e = GetSRTMElevation(Properties.Settings.Default.Elevation_SRTM3_DataPath, lat, lon);
                        if (e != elev_m_missing_flag)
                            return e;
                    }
                    else if (Properties.Settings.Default.Elevation_GLOBE_Enabled)
                    {
                        e = GetGLOBEElevation(Properties.Settings.Default.Elevation_GLOBE_DataPath, lat, lon);
                        if (e != elev_m_missing_flag)
                            return e;
                    }
                    if (lat < lower_map_y)
                        return 0;
                    if (lat > upper_map_y)
                        return 0;
                    if (lon < left_map_x)
                        return 0;
                    if (lon > right_map_x)
                        return 0;
                    int i = (int)((lon - left_map_x) / grid_size);
                    int j = (int)((lat - lower_map_y) / grid_size);
                    // simple data 
                    // int e = Data[i, j];
                    /*
                    // get maximum elevation around the point
                    e = 0;
                    try
                    {
                        Bitmap bm = new Bitmap(5, 3);
                        int elevmax = int.MinValue;
                        int elevmin = int.MaxValue;
                        int elevavg = 0;
                        int elevsum = 0;
                        int count = 0;
                        for (int i1 = 0; i1 < bm.Width; i1++)
                        {
                            for (int j1 = 0; j1 < bm.Height; j1++)
                            {
                                int c = Data[i1 + i - bm.Width / 2, j1 + j - bm.Height / 2];
                                if (c != elev_m_missing_flag)
                                {
                                    if (elevmin > c)
                                        elevmin = c;
                                    if (elevmax < c)
                                        elevmax = c;
                                    count++;
                                    elevsum += c;
                                }
                            }
                        }
                        if (count > 0)
                            elevavg = elevsum / count;
                        e = elevavg;
                        for (int i1 = 0; i1 < bm.Width; i1++)
                        {
                            for (int j1 = 0; j1 < bm.Height; j1++)
                            {
                                if (Data[i1 + i - bm.Width / 2, j1 + j - bm.Height / 2] != elev_m_missing_flag)
                                {
                                    int c = 0;
                                    if ((elevmax - elevmin) != 0)
                                        c = (Data[i1 + i - bm.Width / 2, j1 + j - bm.Height / 2] - elevmin) * 255 / (elevmax - elevmin);
                                    double percent = c * 100 / 255;
                                    bm.SetPixel(i1, bm.Height - j1 - 1, Color.FromArgb(60, (int)(235.0f - (percent * 2.15f)), 235));
                                }
                                else
                                {
                                    bm.SetPixel(i1, bm.Height - j1 - 1, Color.FromArgb(0, 0, 128));
                                }
                            }
                        }
                        bm.SetPixel(bm.Width / 2, bm.Height / 2, Color.FromArgb(255, 0, 0));
                        // bm.Save(WCCheck.WCCheck.Loc(lon, lat) + ".bmp");
                    }
                    catch (Exception ex)
                    {
                        // do nothing
                    }
                     */
                    // set elevation to 0 if data missing flag is set
                    if (e == elev_m_missing_flag)
                        return 0;
                    // return Elevation + Correction
                    return e;
                }
            }
            set
            {
                lock (this)
                {
                    if (lat < lower_map_y)
                        throw (new IndexOutOfRangeException());
                    if (lat > upper_map_y)
                        throw (new IndexOutOfRangeException());
                    if (lon < left_map_x)
                        throw (new IndexOutOfRangeException());
                    if (lon > right_map_x)
                        throw (new IndexOutOfRangeException());
                    int i = (int)((lon - left_map_x) / grid_size);
                    int j = (int)((lat - lower_map_y) / grid_size);
                    Data[i, j] = value;
                }
            }
        }

        public static int GetSRTMElevation(string datapath, double lat, double lon)
        {
            int x, y;
            byte[] a16 = new byte[2];
            try
            {
                // get the lat/lon base values for a single tile to load
                int newlatint = (int)Math.Floor(lat);
                int newlonint = (int)Math.Floor(lon);
                // check if right tile is already loaded --> work with cached values only
                if ((strmbr != null) && (newlatint == latbase) && (newlonint == lonbase))
                {
                    if (hgtexists)
                    {
                        // get file x/y dimensions (SRTM3: 1201x1201, SRTM1: 3601x3601)
                        x = xysize - (int)((lat - latbase) * xysize);
                        y = (int)((lon - lonbase) * xysize);
                        strmbr.BaseStream.Position = ((x - 1) * xysize + (y - 1)) * 2;
                        strmbr.Read(a16, 0, 2);
                        Array.Reverse(a16);
                        int e1 = BitConverter.ToInt16(a16, 0);
                        if (e1 == -32768)
                            e1 = elev_m_missing_flag;
                        return e1;

                    }
                    else
                        return elev_m_missing_flag;
                }
                // tile does not match --> load new tile
                // store new lat/lon for tile
                latbase = newlatint;
                lonbase = newlonint;
                // calculate datafilename
                string latstr = Math.Abs(latbase).ToString("00");
                if (lat > 0)
                {
                    latstr = "N" + latstr;
                }
                else
                {
                    latstr = "S" + latstr;

                }
                string lonstr = Math.Abs(lonbase).ToString("000");
                if (lon > 0)
                    lonstr = "E" + lonstr;
                else
                    lonstr = "W" + lonstr;
                hgtfilename = Application.StartupPath + "\\" + datapath + "\\" + latstr + lonstr + ".hgt";
                if (!File.Exists(hgtfilename))
                {
                    hgtexists = false;
                    return elev_m_missing_flag;
                }
                hgtexists = true;
                if (strmbr != null)
                    strmbr.Dispose();
                strmbr = new BinaryReader(File.OpenRead(hgtfilename));
                // get file x/y dimensions (SRTM3: 1201x1201, SRTM1: 3601x3601)
                xysize = (int)Math.Sqrt(strmbr.BaseStream.Length / 2);
                x = xysize - (int)((lat - latbase) * xysize);
                y = (int)((lon - lonbase) * xysize);
                strmbr.BaseStream.Position = ((x-1) * xysize +  (y-1)) * 2;
                strmbr.Read(a16, 0, 2);
                Array.Reverse(a16);
                int e = BitConverter.ToInt16(a16,0);
                if (e == -32768)
                    e = elev_m_missing_flag;
                return e;
            }
            catch
            {
            }
            return elev_m_missing_flag;
        }

        public int GetGLOBEElevation(string datapath, double lat, double lon)
        {
            int x, y;
            byte[] a16 = new byte[2];
            try
            {
                // get the lat/lon base values for a single tile to load
                GLOBETile tile = this.GLOBETiles.GetTileFromPoint(lat, lon);
                if (tile == null)
                    return elev_m_missing_flag; // tile is missing --> no elevation data available
                if ((globebr == null) || (tile != currenttile))
                {
                    globebr = new BinaryReader(File.OpenRead(tile.Name));
                    currenttile = tile;
                }
                x = (int)((tile.MaxLat - lat) / tile.DivLat);
                y = (int)((lon - tile.MinLon) / tile.DivLon);
                long streampos = (x * tile.Columns + y) * 2;
                globebr.BaseStream.Position = streampos;
                globebr.Read(a16, 0, 2);
                // Array.Reverse(a16);
                int e1 = BitConverter.ToInt16(a16, 0);
                if ((e1 == -32768) || (e1 == globe_elv_missing))
                    e1 = elev_m_missing_flag;
                if (e1 < tile.MinElv)
                    e1 = tile.MinElv;
                if (e1 > tile.MaxElv)
                    e1 = tile.MaxElv;
                return e1;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]:" + ex.Message);
            }
            return elev_m_missing_flag;
        }
    }

    public class ElvMaxInfo
    {
        public int Elv;
        public double Lat;
        public double Lon;

        public ElvMaxInfo(int elv, double lat, double lon)
        {
            Elv = elv;
            Lat = lat;
            Lon = lon;
        }
    }

    public class DEMColorPalette
    {
        private Dictionary<double, Color> Base;
        private Color[] Lookup;

        private int Count = 1000;

        public DEMColorPalette()
        {
            // fill initial palette
            Base = new Dictionary<double, Color>();
            this.Base.Add(0.00, Color.FromArgb(0,97,71));
            this.Base.Add(1.02, Color.FromArgb(16,122,47));
            this.Base.Add(10.20, Color.FromArgb(232,215,125));
            this.Base.Add(24.49, Color.FromArgb(161,67,0));
            this.Base.Add(34.69, Color.FromArgb(158,0,0));
            this.Base.Add(57.14, Color.FromArgb(110,110,110));
            this.Base.Add(81.63, Color.FromArgb(255,255,255));
            this.Base.Add(100.00, Color.FromArgb(255,255,255));
            Lookup = new Color[Count+1];
            Bitmap bm = new Bitmap(Count+1, 100);
            for (int i = 0; i <= Count; i++)
            {
                Color c = GetColorFromBase((double)i / (double)Count * 100.0);
                this.Lookup[i] = c;
                for (int j = 0; j < bm.Height; j++)
                    bm.SetPixel(i, j, c);
            }
            bm.Save("DEMPalette.bmp");
        }

        public Color GetColor(double percentage)
        {
            int i = (int)(percentage / 100.0 * Count);
            try
            {
                return Lookup[i];
            }
            catch
            {
                return Color.FromArgb(0,0,0);
            }
        }

        private Color GetColorFromBase(double percentage)
        {
            if ((percentage < 0) || (percentage > 100))
                return Color.FromArgb(0, 0, 0);
            for ( int i = 0; i < this.Base.Count-1; i++)
            {
                KeyValuePair<double, Color> low = this.Base.ElementAt(i);
                KeyValuePair<double, Color> high = this.Base.ElementAt(i+1);
                if ((percentage >= low.Key) && (percentage <= high.Key))
                {
                    double p = (percentage-low.Key)/(high.Key-low.Key);
                    int red = low.Value.R + (int)(p * (high.Value.R - low.Value.R));
                    int green = low.Value.G + (int)(p * (high.Value.G - low.Value.G));
                    int blue = low.Value.B + (int)(p * (high.Value.B - low.Value.B));
                    return Color.FromArgb(red, green, blue);
                }
            }
            return Color.FromArgb(0, 0, 0);
        }
    }

    public class GLOBETileCollection : List<GLOBETile>
    {

        public GLOBETileCollection(string path = "")
            : base()
        {
            if (!String.IsNullOrEmpty(path))
                ScanDir(path);
        }

        public void ScanDir(string path)
        {
            try
            {
                this.Clear();
                FileInfo[] GLOBEfiles = new DirectoryInfo(path).GetFiles("????.*");
                foreach (FileInfo file in GLOBEfiles)
                {
                    GLOBETile tile = GetTileInfoFromFile(file.FullName);
                    if (tile != null)
                        this.Add(tile);
                }
            }
            catch
            {
            }
        }

        public static bool IsGLOBEFile(string filename)
        {
            // checks for a valid filename with/witout extension
            // filename must have a 4.xxx notation
            // [0]: 'A'..'P'
            // [1],[2]: nn = version
            // [3]: 'G','B','S','T' = status
            // 'S' and 'T' are not used
            if (String.IsNullOrEmpty(filename))
                return false;
            string upperfilename = Path.GetFileNameWithoutExtension(filename).ToUpper();
            char index = upperfilename[0];
            char status = upperfilename[3];
            if (upperfilename.Length != 4)
                return false;
            if ((index < 'A')
                || (index > 'P')
                || !Char.IsDigit(upperfilename[1])
                || !Char.IsDigit(upperfilename[2])
                || ((status != 'G') && (status != 'B'))
                )
                return false;
            return true;
        }
        
        public GLOBETile GetTileFromPoint(double lat, double lon)
        {
            foreach(GLOBETile tile in this)
            {
                if ((lat >= tile.MinLat) &&
                    (lat <= tile.MaxLat) &&
                    (lon >= tile.MinLon) &&
                    (lon <= tile.MaxLon))
                    return tile;
            }
            return null;
        }

        public static GLOBETile GetTileInfoFromFile(string filename)
        {
            if (!IsGLOBEFile(filename))
                return null;
            char index = Path.GetFileNameWithoutExtension(filename).ToUpper()[0];
            char status = Path.GetFileNameWithoutExtension(filename).ToUpper()[3];
            GLOBETile tile = null;
            switch (index)
            {
                case 'A':
                    tile = new GLOBETile(filename, 'A', 50.0, 90.0, -180.0, -90.0, 1, 6098, 10800, 4800);
                    break;
                case 'B':
                    tile = new GLOBETile(filename, 'B', 50.0, 90.0, -90.0, -0.0, 1, 3940, 10800, 4800);
                    break;
                case 'C':
                    tile = new GLOBETile(filename, 'C', 50.0, 90.0, 0.0, 90.0, -30, 4010, 10800, 4800);
                    break;
                case 'D':
                    tile = new GLOBETile(filename, 'D', 50.0, 90.0, 90.0, 180.0, 1, 4588, 10800, 4800);
                    break;
                case 'E':
                    tile = new GLOBETile(filename, 'E', 0.0, 50.0, -180.0, -90.0, -84, 5443, 10800, 4800);
                    break;
                case 'F':
                    tile = new GLOBETile(filename, 'F', 0.0, 50.0, -90.0, -0.0, -40, 6085, 10800, 4800);
                    break;
                case 'G':
                    tile = new GLOBETile(filename, 'G', 0.0, 50.0, 0.0, 90.0, -407, 8752, 10800, 4800);
                    break;
                case 'H':
                    tile = new GLOBETile(filename, 'H', 0.0, 50.0, 90.0, 180.0, -63, 7491, 10800, 4800);
                    break;
                case 'I':
                    tile = new GLOBETile(filename, 'I', -50.0, 0.0, -180.0, -90.0, 1, 2732, 10800, 4800);
                    break;
                case 'J':
                    tile = new GLOBETile(filename, 'J', -50.0, 0.0, -90.0, -0.0, -127, 6798, 10800, 4800);
                    break;
                case 'K':
                    tile = new GLOBETile(filename, 'K', -50.0, 0.0, 0.0, 90.0, 1, 5825, 10800, 4800);
                    break;
                case 'L':
                    tile = new GLOBETile(filename, 'L', -50.0, 0.0, 90.0, 180.0, 1, 5179, 10800, 4800);
                    break;
                case 'M':
                    tile = new GLOBETile(filename, 'M', -90.0, -50.0, -180.0, -90.0, 1, 4009, 10800, 4800);
                    break;
                case 'N':
                    tile = new GLOBETile(filename, 'N', -90.0, -50.0, -90.0, -0.0, 1, 4743, 10800, 4800);
                    break;
                case 'O':
                    tile = new GLOBETile(filename, 'O', -90.0, -50.0, 0.0, 90.0, 1, 4039, 10800, 4800);
                    break;
                case 'P':
                    tile = new GLOBETile(filename, 'P', -90.0, -50.0, 90.0, 180.0, 1, 4363, 10800, 4800);
                    break;
                default:
                    return null;
            }
            try
            {
                int.TryParse(filename.Substring(1, 2), out tile.Version);
                tile.Status = (GLOBETileStatus)Enum.ToObject(typeof(GLOBETileStatus), filename[3]);
            }
            catch
            {
            }
            return tile;
        }
    }

    public enum GLOBETileStatus
    {
        UNDEFINED = 0,
        BAD = 1,
        GOOD = 2,
    }

    public class GLOBETile
    {
        public string Name = "";
        public char Index;
        public double MinLat = 0;
        public double MaxLat = 0;
        public double MinLon = 0;
        public double MaxLon = 0;
        public int MinElv = 0;
        public int MaxElv = 0;
        public double DivLat = 0;
        public double DivLon = 0;
        public int Columns = 0;
        public int Rows = 0;
        public int Version = 0;
        public GLOBETileStatus Status;

        public GLOBETile(string name, char index, double minlat, double maxlat, double minlon, double maxlon, int minelv, int maxelv, int columns, int rows, int version = 0, GLOBETileStatus status = GLOBETileStatus.UNDEFINED)
        {
            Name = name;
            Index = index;
            MinLat = minlat;
            MaxLat = maxlat;
            MinLon = minlon;
            MaxLon = maxlon;
            MinElv = minelv;
            MaxElv = maxelv;
            Columns = columns;
            Rows = rows;
            Version = version;
            Status = status;
            if (Rows > 0)
                DivLat = (MaxLat - MinLat) / (double)Rows;
            if (Columns > 0)
                DivLon = (MaxLon - MinLon) / (double)Columns;
        }
    }
}
