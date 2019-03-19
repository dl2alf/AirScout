using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using ScoutBase.Data;
using Newtonsoft.Json;
using ScoutBase.Core;

namespace ScoutBase.Data
{

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableSRTM1 : DataTable
    {
        public DataTableSRTM1()
            : base()
        {
            // set table name
            TableName = "SRTM1";
            // create all specific columns
            DataColumn FileIndex = this.Columns.Add("FileIndex", typeof(string));
            DataColumn FileName = this.Columns.Add("Filename", typeof(string));
            DataColumn MinLat = this.Columns.Add("MinLat", typeof(double));
            DataColumn MaxLat = this.Columns.Add("MaxLat", typeof(double));
            DataColumn MinLon = this.Columns.Add("MinLon", typeof(double));
            DataColumn MaxLon = this.Columns.Add("MaxLon", typeof(double));
            DataColumn MinElv = this.Columns.Add("MinElv", typeof(int));
            DataColumn MaxElv = this.Columns.Add("MaxElv", typeof(int));
            DataColumn Rows = this.Columns.Add("Rows", typeof(int));
            DataColumn Columns = this.Columns.Add("Columns", typeof(int));
            DataColumn Version = this.Columns.Add("Version", typeof(int));
            DataColumn URL = this.Columns.Add("URL", typeof(string));
            DataColumn Status = this.Columns.Add("Status", typeof(string));
            DataColumn Local = this.Columns.Add("Local", typeof(bool));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = FileIndex;
            this.PrimaryKey = keys;
        }

        public DataRow NewRow(SRTM1TileDesignator tile)
        {
            DataRow row = this.NewRow();
            row["FileIndex"] = tile.FileIndex;
            row["Filename"] = tile.FileName;
            row["MinLat"] = tile.MinLat;
            row["MaxLat"] = tile.MaxLat;
            row["MinLon"] = tile.MinLon;
            row["MaxLon"] = tile.MaxLon;
            row["MinElv"] = tile.MinElv;
            row["MaxElv"] = tile.MaxElv;
            row["Rows"] = tile.Rows;
            row["Columns"] = tile.Columns;
            row["Version"] = tile.Version;
            row["URL"] = tile.URL;
            row["Status"] = tile.Status.ToString();
            row["Local"] = tile.Local;
            row["LastUpdated"] = tile.LastUpdated.ToString("u");
            return row;
        }
    }

    [Serializable]
    [System.ComponentModel.DesignerCategory("")]
    public class JSONArraySRTM1 : Object
    {
        public string version = "";
        public int full_count = 0;
        public List<List<string>> tiles;

        public JSONArraySRTM1()
        {
            version = "";
            full_count = 0;
            tiles = new List<List<string>>();
        }

        public JSONArraySRTM1(SRTM1Dictionary SRTM1)
        {
            version = SRTM1.Version;
            full_count = SRTM1.Tiles.Count;
            tiles = new List<List<string>>();
            for (int i = 0; i < full_count; i++)
            {
                List<string> l = new List<string>();
                SRTM1TileDesignator tile = SRTM1.Tiles.Values.ElementAt(i);
                l.Add(tile.FileIndex);
                l.Add(tile.FileName);
                l.Add(tile.MinLat.ToString("F8", CultureInfo.InvariantCulture));
                l.Add(tile.MaxLat.ToString("F8", CultureInfo.InvariantCulture));
                l.Add(tile.MinLon.ToString("F8", CultureInfo.InvariantCulture));
                l.Add(tile.MaxLon.ToString("F8", CultureInfo.InvariantCulture));
                l.Add(tile.MinElv.ToString());
                l.Add(tile.MaxElv.ToString());
                l.Add(tile.Rows.ToString());
                l.Add(tile.Columns.ToString());
                l.Add(tile.URL.ToString());
                l.Add(tile.Status.ToString());
                l.Add(tile.Local.ToString());
                l.Add(tile.LastUpdated.ToString("u"));
                tiles.Add(l);
            }
        }
    }


    public enum SRTM1TILESTATUS
    {
        UNDEFINED = 'U',
        BAD = 'B',
        GOOD = 'G',
    }

    /// <summary>
    /// Holds the SRTM1 tile information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class SRTM1TileDesignator
    {
        public string FileIndex;
        public string FileName = "";
        public double MinLat = 0;
        public double MaxLat = 0;
        public double MinLon = 0;
        public double MaxLon = 0;
        public int MinElv = int.MaxValue;
        public int MaxElv = int.MinValue;
        public int Rows = 0;
        public int Columns = 0;
        public int Version = 0;
        public string URL = "";
        public SRTM1TILESTATUS Status;
        public bool Local;
        public DateTime LastUpdated;

        public SRTM1TileDesignator(string fileindex, string filename, double minlat, double maxlat, double minlon, double maxlon, int minelv, int maxelv, int rows, int columns, int version, string url, SRTM1TILESTATUS status, bool local)
            : this(fileindex, filename, minlat, maxlat, minlon, maxlon, minelv, maxelv, rows, columns, version, url, status, local, DateTime.UtcNow) { }
        public SRTM1TileDesignator(string fileindex, string filename, double minlat, double maxlat, double minlon, double maxlon, int minelv, int maxelv, int rows, int columns, int version, string url, SRTM1TILESTATUS status, bool local, DateTime lastupdated)
        {
            FileIndex = fileindex;
            FileName = filename;
            MinLat = minlat;
            MaxLat = maxlat;
            MinLon = minlon;
            MaxLon = maxlon;
            MinElv = minelv;
            MaxElv = maxelv;
            Rows = rows;
            Columns = columns;
            Version = version;
            URL = url;
            Status = status;
            Local = local;
            LastUpdated = lastupdated;
        }
    }


    /// <summary>
    /// Holds the SRTM1 Elevation Model in a directory structure.
    /// Returns the elevation of any point referenced with latitude and longitude. 
    /// </summary>
    /// <param name="Latitude">The latitude of point.</param>
    /// <param name="Longitude">The The longitude of point.</param>
    /// <returns>An elevation value according to lat/lon.</returns>
    /// <summary>
    /// Holds a SRTM1 tile collction
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class SRTM1Dictionary : Object
    {

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetAssembly(typeof(ScoutBaseDatabase)).GetName().Version.ToString();

            }
        }

        public string Name
        {
            get
            {
                return "SRTM1";
            }
        }

        private bool changed = false;
        public bool Changed
        {
            get
            {
                return changed;
            }
        }

        public SortedDictionary<string, SRTM1TileDesignator> Tiles = new SortedDictionary<string, SRTM1TileDesignator>();
        private int elv_missing_flag = -500;


        SortedDictionary<string, SRTM1TileDesignator> LocalTiles = new SortedDictionary<string, SRTM1TileDesignator>();
        SortedDictionary<string, SRTM1TileDesignator> WebTiles = new SortedDictionary<string, SRTM1TileDesignator>();

        public SRTM1Dictionary()
        {
        }


        public SRTM1Dictionary(DataTable table)
        {
            // create dictionary from data table
            FromTable(table);
            // reset changed flag
            changed = false;
        }

        public void Clear()
        {
            // clean up tile dictionary
            if (Tiles != null)
            {
                if (Tiles.Count > 0)
                    changed = true;
                Tiles.Clear();
            }
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
            long l = 1201 * 1201 * 2;
            if (l != new System.IO.FileInfo(filename).Length)
                return false;
            return true;
        }

        public SRTM1TileDesignator GetTileFromPoint(double lat, double lon)
        {
            // get the lat/lon base values for a single tile to load
            int latbase = (int)Math.Floor(lat);
            int lonbase = (int)Math.Floor(lon);
            // calculate index
            string index = String.Concat((lat > 0) ? "N" : "S", Math.Abs(latbase).ToString("00")) +
                String.Concat((lon > 0) ? "E" : "W", Math.Abs(lonbase).ToString("000"));
            SRTM1TileDesignator tile = null;
            if (Tiles.TryGetValue(index, out tile))
            {
                if (tile.Local)
                    return tile;
            }
            return null;
        }

        public SRTM1TileDesignator GetTileInfoFromFile(string filename)
        {
            if (!IsSRTM1File(filename))
                return null;
            string fileindex = Path.GetFileNameWithoutExtension(filename).ToUpper();
            int latbase = (fileindex[0] == 'S') ? -System.Convert.ToInt32(fileindex.Substring(1, 2)) : System.Convert.ToInt32(fileindex.Substring(1, 2));
            int lonbase = (fileindex[3] == 'W') ? -System.Convert.ToInt32(fileindex.Substring(4, 3)) : System.Convert.ToInt32(fileindex.Substring(4, 3));
            return new SRTM1TileDesignator(fileindex, filename, latbase, latbase + 1, lonbase, lonbase + 1, int.MaxValue, int.MinValue, 1201, 1201, 1, "", SRTM1TILESTATUS.GOOD, true);
        }

        public void Scan()
        {
            try
            {
                GetAllTilesFromLocalDir();
                GetAllTilesFromWeb();
                // save all new tiles in a separate directory
                // reset all tiles which are not available anymore, add missing with default values
                Tiles.Clear();
                // add all local tiles
                foreach (SRTM1TileDesignator tile in LocalTiles.Values)
                {
                    Console.WriteLine("Scanning tile: " + tile.FileName + "...");
                    // search for index in webtiles and grab url if found
                    SRTM1TileDesignator webtile;
                    if (WebTiles.TryGetValue(tile.FileIndex, out webtile))
                    {
                        tile.URL = webtile.URL;
                        // remove web tile info
                        WebTiles.Remove(webtile.FileIndex);
                    }
                    Update(tile);
                }
                // add rest of web tiles
                foreach (SRTM1TileDesignator tile in WebTiles.Values)
                {
                    Update(tile);
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void GetAllTilesFromLocalDir()
        {
            // return empty dictionary if path not found
            if (!Directory.Exists(global::ScoutBase.Data.Properties.Settings.Default.SRTM1_DataPath))
                return;
            LocalTiles.Clear();
            FileInfo[] SRTM1files = new DirectoryInfo(global::ScoutBase.Data.Properties.Settings.Default.SRTM1_DataPath).GetFiles("???????.hgt");
            foreach (FileInfo file in SRTM1files)
            {
                try
                {
                    SRTM1TileDesignator tile = GetTileInfoFromFile(file.FullName);
                    if (tile != null)
                    {
                        // valid tile found, complete tile information
                        // check min/max info
                        if ((tile.MaxElv == int.MinValue) || (tile.MinElv == int.MaxValue))
                        {
                            // try to get cached min/max info
                            SRTM1TileDesignator oldtile;
                            if (Tiles.TryGetValue(tile.FileIndex, out oldtile))
                            {
                                tile.MinElv = oldtile.MinElv;
                                tile.MaxElv = oldtile.MaxElv;
                            }
                            // still no min/max info?
                            if ((tile.MaxElv == int.MinValue) || (tile.MinElv == int.MaxValue))
                            {
                                // calculate min/max elevation
                                CalcMinMaxElevation(ref tile);
                            }
                        }
                        LocalTiles.Add(tile.FileIndex, tile);
                    }
                }
                catch
                {
                }
            }
        }

        private void GetAllTilesFromWeb()
        {
            // check for valid urls
            if (string.IsNullOrEmpty(global::ScoutBase.Data.Properties.Settings.Default.SRTM1_URLs))
                return;
            // load tiles from web only once
            if (WebTiles.Count > 0)
                return;
            try
            {
                // split urls if necessary
                string[] SRTM1urls;
                if (global::ScoutBase.Data.Properties.Settings.Default.SRTM1_URLs.Contains("\r\n"))
                    SRTM1urls = global::ScoutBase.Data.Properties.Settings.Default.SRTM1_URLs.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                else
                {
                    SRTM1urls = new string[1];
                    SRTM1urls[0] = global::ScoutBase.Data.Properties.Settings.Default.SRTM1_URLs;
                }
                // create a searchable index of all available tiles from web
                SortedDictionary<string, string> urls = new SortedDictionary<string, string>();
                foreach (string url in SRTM1urls)
                {
                    HTTPDirectorySearcher s = new HTTPDirectorySearcher();
                    List<HTTPDirectoryItem> l = s.GetDirectoryInformation(url);
                    foreach (HTTPDirectoryItem item in l)
                    {
                        try
                        {
                            // find possible file names
                            if (item.Name.Contains(".zip"))
                            {
                                urls.Add(item.Name.Split('.')[0].ToUpper(), item.AbsolutePath);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                // initally generate tile info
                for (int lat = -90; lat < 90; lat++)
                {
                    for (int lon = -180; lon < 180; lon++)
                    {
                        string index = ((lat < 0) ? "S" + Math.Abs(lat).ToString("00") : "N" + Math.Abs(lat).ToString("00"))
                            + ((lon < 0) ? "W" + Math.Abs(lon).ToString("000") : "E" + Math.Abs(lon).ToString("000"));
                        SRTM1TileDesignator tile = new SRTM1TileDesignator(index, "", (double)lat, (double)lat + 1, (double)lon, (double)lon + 1, int.MaxValue, int.MinValue, 1201, 1201, 0, "", SRTM1TILESTATUS.UNDEFINED, false);
                        // search tile in web tile index
                        string url;
                        if (urls.TryGetValue(tile.FileIndex, out url))
                        {
                            // add tile if found
                            tile.URL = url;
                            WebTiles.Add(tile.FileIndex, tile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CalcMinMaxElevation(ref SRTM1TileDesignator tile)
        {
            try
            {
                // open file to get more information
                if (tile.Local && ((tile.MinElv == int.MaxValue) || (tile.MaxElv == int.MinValue)))
                {
                    using (BinaryReader br = new BinaryReader(File.OpenRead(tile.FileName)))
                    {
                        tile.MinElv = int.MaxValue;
                        tile.MaxElv = int.MinValue;
                        long l = br.BaseStream.Length / 2;
                        for (int i = 0; i < l; i++)
                        {
                            byte[] a16 = new byte[2];
                            br.Read(a16, 0, 2);
                            Array.Reverse(a16);
                            short s = BitConverter.ToInt16(a16, 0);
                            if (s < elv_missing_flag)
                                s = (short)elv_missing_flag;
                            if (s != elv_missing_flag)
                            {
                                if (s < tile.MinElv)
                                    tile.MinElv = s;
                                if (s > tile.MaxElv)
                                    tile.MaxElv = s;
                            }
                        }
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void FromTable(DataTable dt)
        {
            if (dt == null)
                return;
            this.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string index = row["FileIndex"].ToString();
                string filename = row["FileName"].ToString();
                double minlat = System.Convert.ToDouble(row["MinLat"]);
                double maxlat = System.Convert.ToDouble(row["MaxLat"]);
                double minlon = System.Convert.ToDouble(row["MinLon"]);
                double maxlon = System.Convert.ToDouble(row["MaxLon"]);
                int minelv = System.Convert.ToInt32(row["MinElv"]);
                int maxelv = System.Convert.ToInt32(row["MaxElv"]);
                int rows = System.Convert.ToInt32(row["Rows"]);
                int columns = System.Convert.ToInt32(row["Columns"]);
                int version = System.Convert.ToInt32(row["Version"]);
                string url = row["URL"].ToString();
                SRTM1TILESTATUS status = (SRTM1TILESTATUS)Enum.Parse(typeof(SRTM1TILESTATUS), row["Status"].ToString());
                bool local = System.Convert.ToBoolean(row["Local"]);
                DateTime lastupdated = DateTime.UtcNow;
                try
                {
                    try
                    {
                        lastupdated = DateTime.ParseExact(row["LastUpdated"].ToString(), "yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture);
                        lastupdated = lastupdated.ToUniversalTime();
                    }
                    catch
                    {
                    }
                    SRTM1TileDesignator tile = new SRTM1TileDesignator(index, filename, minlat, maxlat, minlon, maxlon, minelv, maxelv, rows, columns, version, url, status, local, lastupdated);
                    this.Tiles.Add(tile.FileIndex, tile);
                }
                catch
                {
                }
            }

        }

        public DataTable ToTable()
        {
            DataTableSRTM1 dt = new DataTableSRTM1();
            foreach (KeyValuePair<string, SRTM1TileDesignator> tile in Tiles)
            {
                DataRow row = dt.NewRow(tile.Value);
                dt.Rows.Add(row);
            }
            return dt;
        }

        public DataTable FromJSONArray(string filename)
        {
            if (!File.Exists(filename))
                return new DataTableSRTM1();
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    string json = sr.ReadToEnd();
                    JSONArraySRTM1 a = JsonConvert.DeserializeObject<JSONArraySRTM1>(json);
                    // check version
                    if (String.Compare(Version, a.version) != 0)
                    {
                        // do upgrade/downgrade stuff here
                    }
                    else
                    {
                        foreach (List<string> l in a.tiles)
                        {
                            string fileindex = l[0];
                            string FileName = l[1];
                            double minlat = System.Convert.ToDouble(l[2], CultureInfo.InvariantCulture);
                            double maxlat = System.Convert.ToDouble(l[3], CultureInfo.InvariantCulture);
                            double minlon = System.Convert.ToDouble(l[4], CultureInfo.InvariantCulture);
                            double maxlon = System.Convert.ToDouble(l[5], CultureInfo.InvariantCulture);
                            int minelv = System.Convert.ToInt32(l[6]);
                            int maxelv = System.Convert.ToInt32(l[7]);
                            int rows = System.Convert.ToInt32(l[8]);
                            int columns = System.Convert.ToInt32(l[9]);
                            int version = System.Convert.ToInt32(l[10]);
                            string url = l[11];
                            SRTM1TILESTATUS status = SRTM1TILESTATUS.UNDEFINED;
                            try
                            {
                                status = (SRTM1TILESTATUS)Enum.Parse(typeof(SRTM1TILESTATUS), l[12]);
                            }
                            catch
                            {
                            }
                            bool local = System.Convert.ToBoolean(l[13]);
                            DateTime lastupdated = DateTime.UtcNow;
                            try
                            {
                                lastupdated = System.Convert.ToDateTime(l[4]);
                            }
                            catch
                            {
                            }
                            Update(new SRTM1TileDesignator(fileindex, filename, minlat, maxlat, minlon, maxlon, minelv, maxelv, rows, columns, version, url, status, local, lastupdated));
                        }
                    }
                }
                return ToTable();
            }
            catch (Exception ex)
            {
            }
            return new DataTableSRTM1();
        }

        public string ToJSONArray()
        {
            JSONArraySRTM1 a = new JSONArraySRTM1(this);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Formatting.Indented;
            string json = JsonConvert.SerializeObject(a, settings);
            return json;
        }


        public void Update(SRTM1TileDesignator tile)
        {
            // return on null
            if (tile == null)
                return;
            SRTM1TileDesignator entry = null;
            lock (this)
            {
                if (Tiles.TryGetValue(tile.FileIndex, out entry))
                {
                    entry.FileIndex = tile.FileIndex;
                    entry.FileName = tile.FileName;
                    entry.MinLat = tile.MinLat;
                    entry.MaxLat = tile.MaxLat;
                    entry.MinLon = tile.MinLon;
                    entry.MaxLon = tile.MaxLon;
                    entry.MinElv = tile.MinElv;
                    entry.MaxElv = tile.MaxElv;
                    entry.Rows = tile.Rows;
                    entry.Columns = tile.Columns;
                    entry.Version = tile.Version;
                    entry.URL = tile.URL;
                    entry.Status = tile.Status;
                    entry.Local = tile.Local;
                    entry.LastUpdated = tile.LastUpdated;
                    changed = true;
                }
                else
                {
                    this.Tiles.Add(tile.FileIndex, new SRTM1TileDesignator(tile.FileIndex, tile.FileName, tile.MinLat, tile.MaxLat, tile.MinLon, tile.MaxLon, tile.MinElv, tile.MaxElv, tile.Rows, tile.Columns, tile.Version, tile.URL, tile.Status, tile.Local, tile.LastUpdated));
                    changed = true;
                }
            }
        }

        public List<SRTM1TileDesignator> GetLocalTiles()
        {
            List<SRTM1TileDesignator> l = new List<SRTM1TileDesignator>();
            foreach (SRTM1TileDesignator tile in Tiles.Values)
            {
                if (tile.Local)
                    l.Add(tile);
            }
            return l;
        }

        public List<SRTM1TileDesignator> GetLocalTiles(double minlat, double maxlat, double minlon, double maxlon)
        {
            Rect area = new Rect(minlon, minlat, maxlon - minlon, maxlat - minlat);
            List<SRTM1TileDesignator> a = GetLocalTiles();
            List<SRTM1TileDesignator> l = new List<SRTM1TileDesignator>();
            foreach (SRTM1TileDesignator tile in a)
            {
                Rect r = new Rect(tile.MinLon, tile.MinLat, tile.MaxLon - tile.MinLon, tile.MaxLat - tile.MinLat);
                if (tile.Local && area.IntersectsWith(r))
                    l.Add(tile);
            }
            return l;
        }

        public List<SRTM1TileDesignator> GetAllTiles()
        {
            List<SRTM1TileDesignator> l = new List<SRTM1TileDesignator>();
            foreach (SRTM1TileDesignator tile in Tiles.Values)
            {
                l.Add(tile);
            }
            return l;
        }

        public List<SRTM1TileDesignator> GetAvailableTiles()
        {
            List<SRTM1TileDesignator> l = new List<SRTM1TileDesignator>();
            foreach (SRTM1TileDesignator tile in Tiles.Values)
            {
                if (!String.IsNullOrEmpty(tile.URL))
                    l.Add(tile);
            }
            return l;
        }

        public List<SRTM1TileDesignator> GetAvailableTiles(double minlat, double maxlat, double minlon, double maxlon)
        {
            Rect area = new Rect(minlon, minlat, maxlon - minlon, maxlat - minlat);
            List<SRTM1TileDesignator> a = GetAvailableTiles();
            List<SRTM1TileDesignator> l = new List<SRTM1TileDesignator>();
            foreach (SRTM1TileDesignator tile in a)
            {
                Rect r = new Rect(tile.MinLon, tile.MinLat, tile.MaxLon - tile.MinLon, tile.MaxLat - tile.MinLat);
                if (!tile.Local && area.IntersectsWith(r))
                    l.Add(tile);
            }
            return l;
        }


        public void DownloadTile(SRTM1TileDesignator tile)
        {
            if (tile == null)
                return;
            if (String.IsNullOrEmpty(tile.URL))
                return;
            AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
            string filename = Path.GetFileName(tile.URL.Substring(tile.URL.LastIndexOf('/')));
            cl.DownloadFile(tile.URL, Path.Combine(Properties.Settings.Default.SRTM1_DataPath, filename));
        }
    }

}
