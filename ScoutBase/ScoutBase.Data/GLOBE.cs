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
    public class DataTableGLOBE : DataTable
    {
        public DataTableGLOBE()
            : base()
        {
            // set table name
            TableName = "GLOBE";
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
    }


    [Serializable]
    [System.ComponentModel.DesignerCategory("")]
    public class JSONArrayGLOBE : Object
    {
        public string version = "";
        public int full_count = 0;
        public List<List<string>> tiles;

        public JSONArrayGLOBE()
        {
            version = "";
            full_count = 0;
            tiles = new List<List<string>>();
        }

        public JSONArrayGLOBE(GLOBEDictionary globe)
        {
            version = globe.Version;
            full_count = globe.Tiles.Count;
            tiles = new List<List<string>>();
            for (int i = 0; i < full_count; i++)
            {
                List<string> l = new List<string>();
                GLOBETileDesignator tile = globe.Tiles.Values.ElementAt(i);
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


    public enum GLOBETILESTATUS
    {
        UNDEFINED = 'U',
        BAD = 'B',
        GOOD = 'G',
    }

    /// <summary>
    /// Holds the GLOBE tile information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class GLOBETileDesignator
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
        public GLOBETILESTATUS Status;
        public bool Local;
        public DateTime LastUpdated;

        public GLOBETileDesignator(string fileindex, string filename, double minlat, double maxlat, double minlon, double maxlon, int minelv, int maxelv, int rows, int columns, int version, string url, GLOBETILESTATUS status, bool local)
            : this(fileindex, filename, minlat, maxlat, minlon, maxlon, minelv, maxelv, rows, columns, version, url, status, local, DateTime.UtcNow) { }
        public GLOBETileDesignator(string fileindex, string filename, double minlat, double maxlat, double minlon, double maxlon, int minelv, int maxelv, int rows, int columns, int version, string url, GLOBETILESTATUS status, bool local, DateTime lastupdated)
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

    public class URLInfo
    {
        public string Index;
        public int Version;
        public string Status;
        public string URL;

        public URLInfo(string index, int version, string status, string url)
        {
            Index = index;
            Version = version;
            Status = status;
            URL = url;
        }
    }

    /// <summary>
    /// Holds the GLOBE Elevation Model in a directory structure.
    /// Returns the elevation of any point referenced with latitude and longitude. 
    /// </summary>
    /// <param name="Latitude">The latitude of point.</param>
    /// <param name="Longitude">The The longitude of point.</param>
    /// <returns>An elevation value according to lat/lon.</returns>
    /// <summary>
    /// Holds a GLOBE tile collction
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class GLOBEDictionary : Object
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
                return "GLOBE";
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

        public SortedDictionary<string, GLOBETileDesignator> Tiles = new SortedDictionary<string, GLOBETileDesignator>();
        private int elv_missing_flag = -500;


        public GLOBEDictionary()
        {
        }

        public GLOBEDictionary(DataTable table)
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

        public GLOBETileDesignator GetTileFromPoint(double lat, double lon)
        {
            foreach (GLOBETileDesignator tile in this.Tiles.Values)
            {
                if ((lat > tile.MinLat) &&
                    (lat <= tile.MaxLat) &&
                    (lon >= tile.MinLon) &&
                    (lon < tile.MaxLon) && 
                    tile.Local)
                    return tile;
            }
            return null;
        }

        public GLOBETileDesignator GetTileInfoFromFile(string filename)
        {
            if (!IsGLOBEFile(filename))
                return null;
            string upperfilename = Path.GetFileNameWithoutExtension(filename).ToUpper();
            char index = upperfilename[0];
            int vresion = System.Convert.ToInt32(upperfilename.Substring(1, 2));
            char status = upperfilename[3];
            GLOBETileDesignator tile = null;
            switch (index)
            {
                case 'A':
                    tile = new GLOBETileDesignator("A", filename, 50.0, 90.0, -180.0, -90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'B':
                    tile = new GLOBETileDesignator("B", filename, 50.0, 90.0, -90.0, -0.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'C':
                    tile = new GLOBETileDesignator("C", filename, 50.0, 90.0, 0.0, 90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'D':
                    tile = new GLOBETileDesignator("D", filename, 50.0, 90.0, 90.0, 180.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'E':
                    tile = new GLOBETileDesignator("E", filename, 0.0, 50.0, -180.0, -90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'F':
                    tile = new GLOBETileDesignator("F", filename, 0.0, 50.0, -90.0, -0.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'G':
                    tile = new GLOBETileDesignator("G", filename, 0.0, 50.0, 0.0, 90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'H':
                    tile = new GLOBETileDesignator("H", filename, 0.0, 50.0, 90.0, 180.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'I':
                    tile = new GLOBETileDesignator("I", filename, -50.0, 0.0, -180.0, -90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'J':
                    tile = new GLOBETileDesignator("J", filename, -50.0, 0.0, -90.0, -0.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'K':
                    tile = new GLOBETileDesignator("K", filename, -50.0, 0.0, 0.0, 90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'L':
                    tile = new GLOBETileDesignator("L", filename, -50.0, 0.0, 90.0, 180.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'M':
                    tile = new GLOBETileDesignator("M", filename, -90.0, -50.0, -180.0, -90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'N':
                    tile = new GLOBETileDesignator("N", filename, -90.0, -50.0, -90.0, -0.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'O':
                    tile = new GLOBETileDesignator("O", filename, -90.0, -50.0, 0.0, 90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                case 'P':
                    tile = new GLOBETileDesignator("P", filename, -90.0, -50.0, 90.0, 180.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, true);
                    break;
                default:
                    return null;
            }
            try
            {
                int.TryParse(upperfilename.Substring(1, 2), out tile.Version);
                tile.Status = (GLOBETILESTATUS)Enum.ToObject(typeof(GLOBETILESTATUS), upperfilename[3]);
            }
            catch (Exception ex)
            {
                // do nothing
            }
            return tile;
        }

        public void Scan()
        {
            try
            {
                SortedDictionary<string, GLOBETileDesignator> localtiles = GetAllTilesFromLocalDir();
                SortedDictionary<string, GLOBETileDesignator> webtiles = GetAllTilesFromWeb();
                // save all new tiles in a separate directory
                // reset all tiles which are not available anymore, add missing with default values
                Tiles.Clear();
                // add all local tiles
                foreach (GLOBETileDesignator tile in localtiles.Values)
                {
                    Console.WriteLine("Scanning tile: " + tile.FileName + "...");
                    // search for index in webtiles and grab url if found
                    GLOBETileDesignator webtile;
                    if (webtiles.TryGetValue(tile.FileIndex, out webtile))
                    {
                        // same version found on web --> update url
                        if (webtile.Version == tile.Version)
                        {
                            tile.URL = webtile.URL;
                        }
                        else if (webtile.Version > tile.Version)
                        {
                            // newer tile found on web
                            // reset tile information 
                            tile.FileName = "";
                            tile.Version = webtile.Version;
                            tile.MinElv = int.MaxValue;
                            tile.MaxElv = int.MinValue;
                            tile.Status = webtile.Status;
                            tile.Local = false;
                            tile.URL = webtile.URL;
                        }
                        // remove web tile info
                        webtiles.Remove(webtile.FileIndex);
                    }
                    Update(tile);
                }
                // add rest of web tiles
                foreach (GLOBETileDesignator tile in webtiles.Values)
                {
                    Update(tile);
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private SortedDictionary<string, GLOBETileDesignator> GetAllTilesFromLocalDir()
        {
            // return empty dictionary if path not found
            if (!Directory.Exists(global::ScoutBase.Data.Properties.Settings.Default.GLOBE_DataPath))
                return new SortedDictionary<string, GLOBETileDesignator>();
            SortedDictionary<string, GLOBETileDesignator> localtiles = new SortedDictionary<string, GLOBETileDesignator>();
            FileInfo[] GLOBEfiles = new DirectoryInfo(global::ScoutBase.Data.Properties.Settings.Default.GLOBE_DataPath).GetFiles("????.*");
            foreach (FileInfo file in GLOBEfiles)
            {
                try
                {
                    GLOBETileDesignator tile = GetTileInfoFromFile(file.FullName);
                    if (tile != null)
                    {
                        // valid tile found, complete tile information
                        // check min/max info
                        GLOBETileDesignator oldtile;
                        if ((tile.MaxElv == int.MinValue) || (tile.MinElv == int.MaxValue))
                        {
                            // try to get cached min/max info
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
                        if (localtiles.TryGetValue(tile.FileIndex, out oldtile))
                        {
                            if ((tile.Status == GLOBETILESTATUS.GOOD) && (tile.Version > oldtile.Version))
                            {
                                //remove outdated tile
                                localtiles.Remove(oldtile.FileIndex);
                            }
                        }
                        localtiles.Add(tile.FileIndex, tile);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return localtiles;
        }

        private SortedDictionary<string, GLOBETileDesignator> GetAllTilesFromWeb()
        {
            // check for valid urls
            if (string.IsNullOrEmpty(global::ScoutBase.Data.Properties.Settings.Default.GLOBE_URLs))
                return new SortedDictionary<string, GLOBETileDesignator>();
            // split urls if necessary
            string[] globeurls;
            if (global::ScoutBase.Data.Properties.Settings.Default.GLOBE_URLs.Contains("\r\n"))
                globeurls = global::ScoutBase.Data.Properties.Settings.Default.GLOBE_URLs.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            else
            {
                globeurls = new string[1];
                globeurls[0] = global::ScoutBase.Data.Properties.Settings.Default.GLOBE_URLs;
            }
            // create a searchable index of all available tiles from web
            SortedDictionary<string, URLInfo> urls = new SortedDictionary<string,URLInfo>();
            foreach (string globeurl in globeurls)
            {
                HTTPDirectorySearcher s = new HTTPDirectorySearcher();
                List<HTTPDirectoryItem> l = s.GetDirectoryInformation(globeurl);
                foreach (HTTPDirectoryItem item in l)
                {
                    try
                    {
                        // find possible file names
                        if (item.Name.Contains(".zip"))
                        {
                            string fileindex = item.Name.Split('.')[0].ToUpper().Substring(0,1);
                            int version = System.Convert.ToInt32(item.Name.Split('.')[0].ToUpper().Substring(1,2));
                            string status = item.Name.Split('.')[0].ToUpper().Substring(3,1);
                            URLInfo url = new URLInfo(fileindex, version, status, item.AbsolutePath);
                            URLInfo oldurl;
                            if (urls.TryGetValue(fileindex, out oldurl))
                            {
                                if ((url.Status == "G") && (url.Version > oldurl.Version))
                                    urls.Remove(oldurl.Index);
                            }
                            urls.Add(url.Index, url);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            // initally generate tile info
            SortedDictionary<string, GLOBETileDesignator> globetiles = new SortedDictionary<string, GLOBETileDesignator>();
            GLOBETileDesignator globetile;
            globetile = new GLOBETileDesignator("A", "", 50.0, 90.0, -180.0, -90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("B", "", 50.0, 90.0, -90.0, -0.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("C", "", 50.0, 90.0, 0.0, 90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("D", "", 50.0, 90.0, 90.0, 180.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("E", "", 0.0, 50.0, -180.0, -90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("F", "", 0.0, 50.0, -90.0, -0.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("G", "", 0.0, 50.0, 0.0, 90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("H", "", 0.0, 50.0, 90.0, 180.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("I", "", -50.0, 0.0, -180.0, -90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("J", "", -50.0, 0.0, -90.0, -0.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("K", "", -50.0, 0.0, 0.0, 90.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("L", "", -50.0, 0.0, 90.0, 180.0, int.MaxValue, int.MinValue, 6000, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("M", "", -90.0, -50.0, -180.0, -90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("N", "", -90.0, -50.0, -90.0, -0.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("O", "", -90.0, -50.0, 0.0, 90.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);
            globetile = new GLOBETileDesignator("P", "", -90.0, -50.0, 90.0, 180.0, int.MaxValue, int.MinValue, 4800, 10800, 0, "", GLOBETILESTATUS.UNDEFINED, false);
            globetiles.Add(globetile.FileIndex, globetile);

            SortedDictionary<string, GLOBETileDesignator> webtiles = new SortedDictionary<string, GLOBETileDesignator>();
            foreach (GLOBETileDesignator tile in globetiles.Values)
            {
                // search tile in web tile index
                URLInfo url;
                if (urls.TryGetValue(tile.FileIndex, out url))
                {
                    // add tile if found
                    tile.URL = url.URL;
                    tile.Version = url.Version;
                    try
                    {
                        tile.Status = (GLOBETILESTATUS)url.Status[0];
                    }
                    catch
                    {
                    }
                    webtiles.Add(tile.FileIndex, tile);
                }
            }
            return webtiles;
        }


        public void CalcMinMaxElevation(ref GLOBETileDesignator tile)
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
                            short s = br.ReadInt16();
                            if (s != elv_missing_flag)
                            {
                                if (s < tile.MinElv)
                                    tile.MinElv = s;
                                if (s > tile.MaxElv)
                                    tile.MaxElv = s;
                            }
                        }
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
                GLOBETILESTATUS status = (GLOBETILESTATUS)Enum.Parse(typeof(GLOBETILESTATUS), row["Status"].ToString());
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
                    Update(new GLOBETileDesignator(index,filename,minlat,maxlat,minlon, maxlon, minelv, maxelv, rows, columns,version, url, status, local,lastupdated));
                }
                catch
                {
                }
            }

        }

        public DataTable ToTable()
        {
            DataTableGLOBE dt = new DataTableGLOBE();
            foreach (KeyValuePair<string, GLOBETileDesignator> tile in Tiles)
            {
                DataRow row = dt.NewRow();
                row["FileIndex"] = tile.Value.FileIndex;
                row["Filename"] = tile.Value.FileName;
                row["MinLat"] = tile.Value.MinLat;
                row["MaxLat"] = tile.Value.MaxLat;
                row["MinLon"] = tile.Value.MinLon;
                row["MaxLon"] = tile.Value.MaxLon;
                row["MinElv"] = tile.Value.MinElv;
                row["MaxElv"] = tile.Value.MaxElv;
                row["Rows"] = tile.Value.Rows;
                row["Columns"] = tile.Value.Columns;
                row["Version"] = tile.Value.Version;
                row["URL"] = tile.Value.URL;
                row["Status"] = tile.Value.Status.ToString();
                row["Local"] = tile.Value.Local;
                row["LastUpdated"] = tile.Value.LastUpdated.ToString("u");
                dt.Rows.Add(row);
            }
            return dt;
        }

        public DataTable FromJSONArray(string filename)
        {
            if (!File.Exists(filename))
                return new DataTableGLOBE();
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    string json = sr.ReadToEnd();
                    JSONArrayGLOBE a = JsonConvert.DeserializeObject<JSONArrayGLOBE>(json);
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
                            GLOBETILESTATUS status = GLOBETILESTATUS.UNDEFINED;
                            try
                            {
                                status = (GLOBETILESTATUS)Enum.Parse(typeof(GLOBETILESTATUS), l[12]);
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
                            Update(new GLOBETileDesignator(fileindex, filename, minlat, maxlat, minlon, maxlon, minelv, maxelv, rows, columns, version,url, status, local, lastupdated));
                        }
                    }
                }
                return ToTable();
            }
            catch (Exception ex)
            {
            }
            return new DataTableGLOBE();
        }

        public string ToJSONArray()
        {
            JSONArrayGLOBE a = new JSONArrayGLOBE(this);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Formatting.Indented;
            string json = JsonConvert.SerializeObject(a, settings);
            return json;
        }


        public void Update(GLOBETileDesignator tile)
        {
            // return on null
            if (tile == null)
                return;
            GLOBETileDesignator entry = null;
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
                    this.Tiles.Add(tile.FileIndex, new GLOBETileDesignator(tile.FileIndex, tile.FileName, tile.MinLat, tile.MaxLat, tile.MinLon, tile.MaxLon, tile.MinElv, tile.MaxElv, tile.Rows, tile.Columns, tile.Version, tile.URL, tile.Status, tile.Local, tile.LastUpdated));
                    changed = true;
                }
            }
        }

        public List<GLOBETileDesignator> GetAllTiles()
        {
            List<GLOBETileDesignator> l = new List<GLOBETileDesignator>();
            foreach (GLOBETileDesignator tile in Tiles.Values)
            {
                l.Add(tile);
            }
            return l;
        }

        public List<GLOBETileDesignator> GetLocalTiles()
        {
            List<GLOBETileDesignator> l = new List<GLOBETileDesignator>();
            foreach (GLOBETileDesignator tile in Tiles.Values)
            {
                if (tile.Local)
                    l.Add(tile);
            }
            return l;
        }

        public List<GLOBETileDesignator> GetLocalTiles(double minlat, double maxlat, double minlon, double maxlon)
        {
            Rect area = new Rect(minlon, minlat, maxlon - minlon, maxlat - minlat);
            List<GLOBETileDesignator> a = GetLocalTiles();
            List<GLOBETileDesignator> l = new List<GLOBETileDesignator>();
            foreach (GLOBETileDesignator tile in a)
            {
                Rect r = new Rect(tile.MinLon, tile.MinLat, tile.MaxLon - tile.MinLon, tile.MaxLat - tile.MinLat);
                if (tile.Local && area.IntersectsWith(r))
                    l.Add(tile);
            }
            return l;
        }

        public List<GLOBETileDesignator> GetAvailableTiles()
        {
            List<GLOBETileDesignator> l = new List<GLOBETileDesignator>();
            foreach (GLOBETileDesignator tile in Tiles.Values)
            {
                if (!String.IsNullOrEmpty(tile.URL))
                    l.Add(tile);
            }
            return l;
        }

        public List<GLOBETileDesignator> GetAvailableTiles(double minlat, double maxlat, double minlon, double maxlon)
        {
            Rect area = new Rect(minlon, minlat, maxlon - minlon, maxlat - minlat);
            List<GLOBETileDesignator> a = GetAvailableTiles();
            List<GLOBETileDesignator> l = new List<GLOBETileDesignator>();
            foreach (GLOBETileDesignator tile in a)
            {
                Rect r = new Rect(tile.MinLon, tile.MinLat, tile.MaxLon - tile.MinLon, tile.MaxLat - tile.MinLat);
                if (!tile.Local && area.IntersectsWith(r))
                    l.Add(tile);
            }
            return l;
        }

        public void DownloadTile(GLOBETileDesignator tile)
        {
            if (tile == null)
                return;
            if (String.IsNullOrEmpty(tile.URL))
                return;
            AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
            string filename = Path.GetFileName(tile.URL.Substring(tile.URL.LastIndexOf('/')));
            cl.DownloadFile(tile.URL, Path.Combine(Properties.Settings.Default.GLOBE_DataPath, filename));
        }
    }

}
