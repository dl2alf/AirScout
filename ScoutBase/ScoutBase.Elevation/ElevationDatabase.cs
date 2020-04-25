using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Data.SQLite;
using System.Windows;
using ScoutBase.Core;
using ScoutBase.Database;
using Newtonsoft.Json;

namespace ScoutBase.Elevation
{

    public class ElevationPoint
    {
        public double Dist { get; set; }
        public short Elv { get; set; }

        public ElevationPoint(double dist, short elv)
        {
            Dist = dist;
            Elv = elv;
        }
    }

    public class ElevationData
    {
        static ElevationDatabase elevation = new ElevationDatabase();
        public static ElevationDatabase Database
        {
            get
            {
                return elevation;
            }
        }

    }

    /// <summary>
    /// Holds the Digital Elevation Model (DEM) in a database structure.
    /// Returns the elevation of any point referenced with latitude and longitude. 
    /// </summary>
    /// <param name="Latitude">The latitude of point.</param>
    /// <param name="Longitude">The The longitude of point.</param>
    /// <returns>An elevation value according to lat/lon.</returns>
    public class ElevationDatabase : ScoutBaseDatabase
    {
        /// <summary>
        /// Value of elevation data missing flag
        /// </summary>
        [Description("Value of elevation data missing flag")]
        [DefaultValue(-32768)]
        public short ElvMissingFlag
        {
            get
            {
                return -32768;
            }
        }

        /// <summary>
        /// Value of elevation tile missing flag
        /// </summary>
        [Description("Value of elevation tile missing flag")]
        [DefaultValue(-32767)]
        public short TileMissingFlag
        {
            get
            {
                return -32767;
            }
        }

        // Elevation database bounds
        public double MinLat { get; set; }
        public double MinLon { get; set; }
        public double MaxLat { get; set; }
        public double MaxLon { get; set; }

        System.Data.SQLite.SQLiteDatabase globe;
        System.Data.SQLite.SQLiteDatabase srtm3;
        System.Data.SQLite.SQLiteDatabase srtm1;
        System.Data.SQLite.SQLiteDatabase aster3;
        System.Data.SQLite.SQLiteDatabase aster1;

        // tile cache
        private ElevationTileDesignator globe_tile = null;
        private ElevationTileDesignator srtm3_tile = null;
        private ElevationTileDesignator srtm1_tile = null;
        private ElevationTileDesignator aster3_tile = null;
        private ElevationTileDesignator aster1_tile = null;
        private OrderedDictionary globe_cache = new OrderedDictionary();
        private OrderedDictionary srtm3_cache = new OrderedDictionary();
        private OrderedDictionary srtm1_cache = new OrderedDictionary();
        private OrderedDictionary aster3_cache = new OrderedDictionary();
        private OrderedDictionary aster1_cache = new OrderedDictionary();
        private int globe_tile_size = 138;
        private int srtm3_tile_size = 10038;
        private int srtm1_tile_size = 90038;
        private int aster3_tile_size = 10038;
        private int aster1_tile_size = 90038;
        private int globe_cache_count = 0;
        private int srtm3_cache_count = 0;
        private int srtm1_cache_count = 0;
        private int aster3_cache_count = 0;
        private int aster1_cache_count = 0;

        public ElevationDatabase()
        {
            UserVersion = 1;
            Name = "ScoutBase Elevation Database";
            Description = "The Scoutbase Elevation Database is containing various elevation information.\n" +
                "The basic elevation information is kept unique per  6digit-Maidenhead Locator and is updated periodically from a global web resource.\n" +
                "The path and horizon information are unique per one oder between two geographical locations.\n" +
                "These values are (pre-)calculated and stored at runtime.\n" +
                "All values are based on a distinct elevation model either GLOBE, SRTM3, SRTM1 or ASTER.";
            // add table description manually
            TableDescriptions.Add(ElevationTileDesignator.TableName, "Holds elevation information per 6digit Maidenhead Locator.");
            TableDescriptions.Add(ElevationPathDesignator.TableName, "Holds elevation path information between two locations.");
            TableDescriptions.Add(ElevationHorizonDesignator.TableName, "Holds elevation horizon information for one location.");
            TableDescriptions.Add(LocalObstructionDesignator.TableName, "Holds local obstruction information for one location.");
            // initialize cache sizes
            CacheSetBounds();
            // open databases
            globe = OpenDatabase("globe.db3", DefaultDatabaseDirectory(), false);
            srtm3 = OpenDatabase("srtm3.db3", DefaultDatabaseDirectory(), false);
            srtm1 = OpenDatabase("srtm1.db3", DefaultDatabaseDirectory(), false);
            aster3 = OpenDatabase("aster3.db3", DefaultDatabaseDirectory(), false);
            aster1 = OpenDatabase("aster1.db3", DefaultDatabaseDirectory(), false);
            // create tables with schemas if not exist
            if (!ElevationTileTableExists(ELEVATIONMODEL.GLOBE))
                ElevationTileCreateTable(ELEVATIONMODEL.GLOBE);
            if (!ElevationTileTableExists(ELEVATIONMODEL.SRTM3))
                ElevationTileCreateTable(ELEVATIONMODEL.SRTM3);
            if (!ElevationTileTableExists(ELEVATIONMODEL.SRTM1))
                ElevationTileCreateTable(ELEVATIONMODEL.SRTM1);
            if (!ElevationTileTableExists(ELEVATIONMODEL.ASTER3))
                ElevationTileCreateTable(ELEVATIONMODEL.ASTER3);
            if (!ElevationTileTableExists(ELEVATIONMODEL.ASTER1))
                ElevationTileCreateTable(ELEVATIONMODEL.ASTER1);
            if (!ElevationPathTableExists(ELEVATIONMODEL.GLOBE))
                ElevationPathCreateTable(ELEVATIONMODEL.GLOBE);
            if (!ElevationPathTableExists(ELEVATIONMODEL.SRTM3))
                ElevationPathCreateTable(ELEVATIONMODEL.SRTM3);
            if (!ElevationPathTableExists(ELEVATIONMODEL.SRTM1))
                ElevationPathCreateTable(ELEVATIONMODEL.SRTM1);
            if (!ElevationPathTableExists(ELEVATIONMODEL.ASTER3))
                ElevationPathCreateTable(ELEVATIONMODEL.ASTER3);
            if (!ElevationPathTableExists(ELEVATIONMODEL.ASTER1))
                ElevationPathCreateTable(ELEVATIONMODEL.ASTER1);
            if (!ElevationHorizonTableExists(ELEVATIONMODEL.GLOBE))
                ElevationHorizonCreateTable(ELEVATIONMODEL.GLOBE);
            if (!ElevationHorizonTableExists(ELEVATIONMODEL.SRTM3))
                ElevationHorizonCreateTable(ELEVATIONMODEL.SRTM3);
            if (!ElevationHorizonTableExists(ELEVATIONMODEL.SRTM1))
                ElevationHorizonCreateTable(ELEVATIONMODEL.SRTM1);
            if (!ElevationHorizonTableExists(ELEVATIONMODEL.ASTER3))
                ElevationHorizonCreateTable(ELEVATIONMODEL.ASTER3);
            if (!ElevationHorizonTableExists(ELEVATIONMODEL.ASTER1))
                ElevationHorizonCreateTable(ELEVATIONMODEL.ASTER1);
            if (!LocalObstructionTableExists(ELEVATIONMODEL.GLOBE))
                LocalObstructionCreateTable(ELEVATIONMODEL.GLOBE);
            if (!LocalObstructionTableExists(ELEVATIONMODEL.SRTM3))
                LocalObstructionCreateTable(ELEVATIONMODEL.SRTM3);
            if (!LocalObstructionTableExists(ELEVATIONMODEL.SRTM1))
                LocalObstructionCreateTable(ELEVATIONMODEL.SRTM1);
            if (!LocalObstructionTableExists(ELEVATIONMODEL.ASTER3))
                LocalObstructionCreateTable(ELEVATIONMODEL.ASTER3);
            if (!LocalObstructionTableExists(ELEVATIONMODEL.ASTER1))
                LocalObstructionCreateTable(ELEVATIONMODEL.ASTER1);
            // set default bounds
            MinLat = double.MinValue;
            MinLon = double.MinValue;
            MaxLat = double.MaxValue;
            MaxLon = double.MaxValue;
        }

        ~ElevationDatabase()
        {
            CloseDatabase(globe);
            CloseDatabase(srtm3);
            CloseDatabase(srtm1);
            CloseDatabase(aster3);
            CloseDatabase(aster1);
        }

        private void UpgradeToV1(System.Data.SQLite.SQLiteDatabase db)
        {
            if (db == null)
                return;
            // upgrades database to V1
            if (MessageBox.Show("A major database upgrade is necessary to run this version of AirScout. Older versions of AirScout are not compatible anymore and will cause errors. \n\nPress >OK< to start upgrade now (this will take some minutes). \nPress >Cancel< to leave.", "Database Upgrade of " + Path.GetFileName(db.DBLocation), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                Environment.Exit(-1);                 //  exit immediately
            // set savepoint
            string savepointname = "UpgradeToV1";
            db.Execute("SAVEPOINT " + savepointname);
            try
            {
                // drop version info table --> maintain PRAGMA user_version instead
                db.DropTable("VersionInfo");
                // change Elevation table
                Stopwatch st = new Stopwatch();
                st.Start();
                Log.WriteMessage("Database " + db.DBLocation + " is being converted...");
                UpgradeTableToV1(db, ElevationTileDesignator.TableName);
                UpgradeTableToV1(db, ElevationPathDesignator.TableName);
                st.Stop();
                Log.WriteMessage("Database " + db.DBLocation + " is converted successfully [" + st.ElapsedMilliseconds / 1000 + "ms]");
                // set new database version
                db.SetUserVerion(1);
                // release savepoint
                db.Execute("RELEASE " + savepointname);
            }
            catch (Exception ex)
            {
                // fatal error, can't upgrade database and can't rollback --> write log
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
                Log.WriteMessage("Application crashed and will close immediately.");
                Log.FlushLog();
                // try to rollback database
                db.Execute("ROLLBACK TO " + savepointname);
                //  exit immediately
                Environment.Exit(-1);
            }
        }

        public System.Data.SQLite.SQLiteDatabase GetElevationDatabase(ELEVATIONMODEL model)
        {
            switch (model)
            {
                case ELEVATIONMODEL.GLOBE: return globe;
                case ELEVATIONMODEL.SRTM3: return srtm3;
                case ELEVATIONMODEL.SRTM1: return srtm1;
                case ELEVATIONMODEL.ASTER3: return aster3;
                case ELEVATIONMODEL.ASTER1: return aster1;
                default: return null;
            }
        }

        public string DefaultDatabaseDirectory()
        {
            // create default database directory name
            // fully qualify path and adjust it to Windows/Linux notation
            // create directory if not exists
            // return directory string if needed
            string dir = Properties.Settings.Default.Database_Directory;
            // set default value if empty
            if (String.IsNullOrEmpty(dir))
                dir = "ElevationData";
            // fully qualify path if not rooted
            if (!System.IO.Path.IsPathRooted(dir))
            {
                // empty or incomplete settings --> create fully qulified standard path
                // collect entry assembly info
                Assembly ass = Assembly.GetExecutingAssembly();
                string company = "";
                string product = "";
                object[] attribs;
                attribs = ass.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
                if (attribs.Length > 0)
                {
                    company = ((AssemblyCompanyAttribute)attribs[0]).Company;
                }
                attribs = ass.GetCustomAttributes(typeof(AssemblyProductAttribute), true);
                if (attribs.Length > 0)
                {
                    product = ((AssemblyProductAttribute)attribs[0]).Product;
                }
                // create database path
                string rootdir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (!String.IsNullOrEmpty(company))
                    rootdir = Path.Combine(rootdir, company);
                if (!String.IsNullOrEmpty(product))
                    rootdir = Path.Combine(rootdir, product);
                dir = Path.Combine(rootdir, dir);
            }
            // replace Windows/Linux directory spearator chars
            dir = dir.Replace('\\', Path.DirectorySeparatorChar);
            dir = dir.Replace('/', Path.DirectorySeparatorChar);
            // create directory if not exists
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public string DefaultDatabaseDirectory (ELEVATIONMODEL model)
        {
            // get root directory
            string dir = DefaultDatabaseDirectory();
            // get elevation specific directory
            switch (model)
            {
                case ELEVATIONMODEL.GLOBE:
                    dir = Path.Combine(dir, Properties.Settings.Default.Elevation_GLOBE_DataPath);
                    break;
                case ELEVATIONMODEL.SRTM3:
                    dir = Path.Combine(dir, Properties.Settings.Default.Elevation_SRTM3_DataPath);
                    break;
                case ELEVATIONMODEL.SRTM1:
                    dir = Path.Combine(dir, Properties.Settings.Default.Elevation_SRTM1_DataPath);
                    break;
                case ELEVATIONMODEL.ASTER3:
                    dir = Path.Combine(dir, Properties.Settings.Default.Elevation_ASTER3_DataPath);
                    break;
                case ELEVATIONMODEL.ASTER1:
                    dir = Path.Combine(dir, Properties.Settings.Default.Elevation_ASTER1_DataPath);
                    break;
            }
            // replace Windows/Linux directory spearator chars
            dir = dir.Replace('\\', Path.DirectorySeparatorChar);
            dir = dir.Replace('/', Path.DirectorySeparatorChar);
            // create directory if not exists
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public string UpdateURL(ELEVATIONMODEL model)
        {
            // get elevation specific update URL
            switch (model)
            {
                case ELEVATIONMODEL.GLOBE:
                    return Properties.Settings.Default.Elevation_GLOBE_UpdateURL;
                case ELEVATIONMODEL.SRTM3:
                    return Properties.Settings.Default.Elevation_SRTM3_UpdateURL;
                case ELEVATIONMODEL.SRTM1:
                    return Properties.Settings.Default.Elevation_SRTM1_UpdateURL;
                case ELEVATIONMODEL.ASTER3:
                    return Properties.Settings.Default.Elevation_ASTER3_UpdateURL;
                case ELEVATIONMODEL.ASTER1:
                    return Properties.Settings.Default.Elevation_ASTER1_UpdateURL;
            }
            return "";
        }

        public string JSONFile(ELEVATIONMODEL model)
        {
            // get elevation specific JSON cache file 
            switch (model)
            {
                case ELEVATIONMODEL.GLOBE:
                    return Properties.Settings.Default.Elevation_GLOBE_JSONFile;
                case ELEVATIONMODEL.SRTM3:
                    return Properties.Settings.Default.Elevation_SRTM3_JSONFile;
                case ELEVATIONMODEL.SRTM1:
                    return Properties.Settings.Default.Elevation_SRTM1_JSONFile;
                case ELEVATIONMODEL.ASTER3:
                    return Properties.Settings.Default.Elevation_ASTER3_JSONFile;
                case ELEVATIONMODEL.ASTER1:
                    return Properties.Settings.Default.Elevation_ASTER1_JSONFile;
            }
            return "";

        }

        public string GetDBLocation(ELEVATIONMODEL model)
        {
            return this.GetDBLocation(GetElevationDatabase(model));
        }

        public double GetDBSize(ELEVATIONMODEL model)
        {
            return this.GetDBSize(GetElevationDatabase(model));
        }

        public DATABASESTATUS GetDBStatus(ELEVATIONMODEL model)
        {
            return this.GetDBStatus(GetElevationDatabase(model));
        }

        public void SetDBStatus(ELEVATIONMODEL model, DATABASESTATUS status)
        {
            this.SetDBStatus(status, GetElevationDatabase(model));
        }

        public bool GetDBStatusBit(ELEVATIONMODEL model, DATABASESTATUS statusbit)
        {
            return this.GetDBStatusBit(statusbit, GetElevationDatabase(model));
        }

        public void SetDBStatusBit(ELEVATIONMODEL model, DATABASESTATUS statusbit)
        {
            this.SetDBStatusBit(statusbit, GetElevationDatabase(model));
        }

        public void ResetDBStatusBit(ELEVATIONMODEL model, DATABASESTATUS statusbit)
        {
            this.ResetDBStatusBit(statusbit, GetElevationDatabase(model));
        }

        public void BeginTransaction(ELEVATIONMODEL model)
        {
            this.BeginTransaction(GetElevationDatabase(model));
        }

        public void Commit(ELEVATIONMODEL model)
        {
            this.Commit(GetElevationDatabase(model));
        }
        private DataTable Select(ELEVATIONMODEL model, string sql)
        {
            return this.Select(sql, GetElevationDatabase(model));
        }

        /// <summary>
        /// Sets the elevation tile cache size according to the amount of memory available at the system.
        /// </summary>
        public void CacheSetBounds()
        {
            // sets the elevation tile cache bounds according to available memory size
            // this function gets the currently available memory size of the system which can change dynamically by influence other applications 
            // this function is called once at initialization of this object
            // to adjust values while running, subsequent calls of this function are necessary
            // get the available memory size of the system in Bytes
            // asssume a 64bit configuration with no limits
            // assume 1GB by default, sometimes exception occurs and memory counter cannnot be cereated
            double avmem = 1;
            try
            { 
                avmem = SupportFunctions.MemoryCounter.GetAvailable() * 1024.0 * 1024.0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not create memory counter: " + ex.ToString());
            }
            // do not use more than 10% of available memory for tile cache
            avmem = avmem / 10.0;
            // reduce memory when running in a 32bit configuration (use not more than 25% of max. 2GB per application)
            if (!SupportFunctions.Is64BitConfiguration() && (avmem > 2 * 1024.0 * 1024.0))
                avmem = 2 * 1024.0 * 1024.0 * 0.25;
            // calculate cache sizes
            globe_cache_count = (int)((avmem / globe_tile_size < int.MaxValue) ? avmem / globe_tile_size : int.MaxValue);
            srtm3_cache_count = (int)((avmem / srtm3_tile_size < int.MaxValue) ? avmem / srtm3_tile_size : int.MaxValue);
            srtm1_cache_count = (int)((avmem / srtm1_tile_size < int.MaxValue) ? avmem / srtm1_tile_size : int.MaxValue);
            aster3_cache_count = (int)((avmem / aster3_tile_size < int.MaxValue) ? avmem / aster3_tile_size : int.MaxValue);
            aster1_cache_count = (int)((avmem / aster1_tile_size < int.MaxValue) ? avmem / aster1_tile_size : int.MaxValue);
        }

        /// <summary>
        /// Clears all elevation tile caches.
        /// </summary>
        public void CacheClearAll()
        {
            globe_cache.Clear();
            srtm3_cache.Clear();
            srtm1_cache.Clear();
            aster3_cache.Clear();
            aster1_cache.Clear();
        }

        private int GetCacheSize(ELEVATIONMODEL model)
        {
            // get elevation specific JSON cache file 
            switch (model)
            {
                case ELEVATIONMODEL.GLOBE:
                    return globe_cache_count;
                case ELEVATIONMODEL.SRTM3:
                    return srtm3_cache_count;
                case ELEVATIONMODEL.SRTM1:
                    return srtm1_cache_count;
                case ELEVATIONMODEL.ASTER3:
                    return aster3_cache_count;
                case ELEVATIONMODEL.ASTER1:
                    return aster1_cache_count;
            }
            return 0;
        }
        public Bitmap DrawElevationBitmap(double minlat, double minlon, double maxlat, double maxlon, int width, int height, ELEVATIONMODEL model)
        {
            int minelv = 0;
            int maxelv = 3000;
            // minimum stepwidth according to SRTM1 resolution in degrees
            double minstep = 1.0 / width;
            // calculate bitmap dimensions
            // get the x/y ratio first
            double xyratio = (maxlat - minlat) / (maxlon - minlon);
            if (height <= 0)
                height = (int)(width * xyratio);
            double stepx = (maxlon - minlon) / width;
            double stepy = (maxlat - minlat) / height;
            DEMColorPalette palette = new DEMColorPalette();
            Bitmap bm = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                // System.Console.WriteLine(i);
                for (int j = 0; j < height; j++)
                {
                    int e1 = this[minlat + j * stepy, (minlon + i * stepx), model, false];
                    if (e1 != ElvMissingFlag)
                    {
                        double e = (double)(e1 - minelv) / (double)(maxelv - minelv) * 100.0;
                        if (e < 0)
                            e = 0;
                        if (e > 100)
                            e = 100;
                        bm.SetPixel(i, height - j - 1, palette.GetColor(e));
                    }
                    else
                    {
                        bm.SetPixel(i, height - j - 1, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return bm;
        }

        /// <summary>
        /// Gets an array of all Maidenhead loactors included in a rect.
        /// Precision is 1..3 giving 2-digit, 4-digit or 6-digit locator strings.
        /// </summary>
        /// <param name="minlat">The minimum latitude of rect.</param>
        /// <param name="minlon">The minumum longitude of rect.</param>
        /// <param name="maxlat">The maximum latitude of rect.</param>
        /// <param name="maxlon">The maximum longitude of rect.</param>
        /// <param name="precision">The precision (1..3, default = 3).</param>
        /// <returns>The array of included Maidenhead locators as 6-digit strings.</returns>
        public List<string> GetLocsFromRect(double minlat, double minlon, double maxlat, double maxlon, int precision = 3)
        {
            List<string> a = new List<string>();
            if ((maxlat <= minlat) || (maxlon <= minlon))
                return a;
            double steplat = 0;
            double steplon = 0;
            switch (precision)
            {
                case 1:
                    steplat = 10.0;
                    steplon = 20.0;
                    break;
                case 2:
                    steplat = 1.0;
                    steplon = 2.0;
                    break;
                case 3:
                    steplat = 1 / 24.0;
                    steplon = 2 / 24.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Illegal precision value (1..3): " + precision.ToString());
            }
            for (double lat = minlat; lat <= maxlat; lat += steplat)
            {
                for (double lon = minlon; lon <= maxlon; lon += steplon)
                {
                    a.Add(MaidenheadLocator.LocFromLatLon(lat + 0.02, lon + 0.04, false, precision));
                }
            }
            return a;
        }

        public ElvMinMaxInfo GetMaxElvLoc(string loc, ELEVATIONMODEL model, bool setinvalidtozero = true)
        {
            // return null on invalid locator
            if (!MaidenheadLocator.Check(loc))
                return null;
            // return null if loc <> 6 digits
            if (loc.Length != 6)
                return null;
            return this.ElevationTileFindMinMaxInfo(new ElevationTileDesignator(loc.ToUpper()), model);
        }

        private OrderedDictionary GetElevationDictionary(ELEVATIONMODEL model)
        {
            if (model == ELEVATIONMODEL.GLOBE)
                return globe_cache;
            if (model == ELEVATIONMODEL.SRTM3)
                return srtm3_cache;
            if (model == ELEVATIONMODEL.SRTM1)
                return srtm1_cache;
            if (model == ELEVATIONMODEL.ASTER3)
                return aster3_cache;
            if (model == ELEVATIONMODEL.ASTER1)
                return aster1_cache;
            return null;
        }

        private ElevationTileDesignator GetElevationTile(ELEVATIONMODEL model)
        {
            if (model == ELEVATIONMODEL.GLOBE)
                return globe_tile;
            if (model == ELEVATIONMODEL.SRTM3)
                return srtm3_tile;
            if (model == ELEVATIONMODEL.SRTM1)
                return aster1_tile;
            if (model == ELEVATIONMODEL.ASTER3)
                return aster3_tile;
            if (model == ELEVATIONMODEL.ASTER1)
                return aster1_tile;
            return null;
        }

        private void SetElevationTile(ELEVATIONMODEL model, ElevationTileDesignator tile)
        {
            if (model == ELEVATIONMODEL.GLOBE)
                globe_tile = tile;
            if (model == ELEVATIONMODEL.SRTM3)
                srtm3_tile = tile;
            if (model == ELEVATIONMODEL.SRTM1)
                srtm1_tile = tile;
            if (model == ELEVATIONMODEL.ASTER3)
                aster3_tile = tile;
            if (model == ELEVATIONMODEL.ASTER1)
                aster1_tile = tile;
        }

        private ElevationTileDesignator GetElevationTile(ELEVATIONMODEL model, double lat, double lon)
        {
            // set cache according to elevation model
            // check for subsequent calls first --> return last tile cached immediately
            ElevationTileDesignator tile = GetElevationTile(model);
            if ((tile != null) && (lat >= tile.Bounds.MinLat) && (lat <= tile.Bounds.MaxLat) && (lon >= tile.Bounds.MinLon) && (lon <= tile.Bounds.MaxLon))
                return tile;
            // get cache directory
            OrderedDictionary cache = GetElevationDictionary(model);
            // load new tile from cache or database
            string loc = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
            ElevationTileDesignator td = null;
            lock (cache)
            {
                // try to find tile in cache first
                td = (ElevationTileDesignator)cache[loc];
                if (td == null)
                {
                    // try to find tile in database
                    td = this.ElevationTileFind(loc, model);
                    if (td != null)
                    {
                        // maintain cache size --> remove 
                        if (cache.Count > GetCacheSize(model))
                        {
                            cache.RemoveAt(0);
                        }
                        cache.Add(loc, td);
                    }
                }
            }
            // keep tile in cache
            SetElevationTile(model, td);
            // return tile even if null
            return td;
        }

        private short GetElevation(ELEVATIONMODEL model, double lat, double lon, bool setinvalidtozero = true)
        {
            // check bounds
            if ((lat < -90) || (lat > 90))
                throw new ArgumentException("Invalid latitude: " + lat.ToString("F8"));
            if ((lon < -180) || (lon > 180))
                throw new ArgumentException("Invalid longitude: " + lon.ToString("F8"));
            short e = ElvMissingFlag;
            ElevationTileDesignator tile = GetElevationTile(model, lat, lon);
            if (tile != null)
                e = tile.GetElevation(lat, lon);
            else
                e = TileMissingFlag;
                if (setinvalidtozero && (e <= TileMissingFlag))
                    e = 0;
            return e;
        }

        public short this[double lat, double lon, ELEVATIONMODEL model, bool setinvalidtozero = true]
        {
            get
            {
                return GetElevation(model, lat, lon, setinvalidtozero);
            }
        }

        public ElevationPoint[] GetElevationPathLatLon(double lat1, double lon1, double lat2, double lon2, ELEVATIONMODEL model)
        {
            double distance = LatLon.Distance(lat1, lon1, lat2, lon2);
            double bearing = LatLon.Bearing(lat1, lon1, lat2, lon2);
            return GetElevationPathBearing(lat1, lon1, bearing, distance, model);
        }

        public ElevationPoint[] GetElevationPathBearing(double lat1, double lon1, double bearing, double distance, ELEVATIONMODEL model)
        {
            // get default stepwidth in [km]
            double stepwidth = GetDefaultStepWidth(model) / 1000.0;
            double d = 0;
            int count = (int)(distance / stepwidth);
            ElevationPoint[] a = new ElevationPoint[count];
            try
            {
                for (int i = 0; i < count; i++)
                {
                    LatLon.GPoint p = LatLon.DestinationPoint(lat1, lon1, bearing, d);
                    a[i] = new ElevationPoint(d, this[p.Lat, p.Lon, model]);
                    d += stepwidth;
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return a;
        }

        public double GetDefaultStepWidth(ELEVATIONMODEL model)
        {
            if (model == ELEVATIONMODEL.SRTM1)
                return 30;
            if (model == ELEVATIONMODEL.SRTM3)
                return 90;
            if (model == ELEVATIONMODEL.ASTER1)
                return 30;
            if (model == ELEVATIONMODEL.ASTER3)
                return 90;
            return 1000;
        }

        #region ElevationTile

        public bool ElevationTileTableExists(ELEVATIONMODEL model, string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = ElevationTileDesignator.TableName;
            return GetElevationDatabase(model).TableExists(tn);
        }

        public void ElevationTileCreateTable(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = ElevationTileDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE " + tn + " (TileIndex TEXT NOT NULL DEFAULT '', MinElv INT32, MinLat DOUBLE, MinLon DOUBLE, MaxElv INT32, MaxLat DOUBLE, MaxLon DOUBLE, Rows INT32, Columns INT32, Elv BLOB, LastUpdated INT32, PRIMARY KEY (TileIndex))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public int ElevationTileInsert(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + ElevationTileDesignator.TableName + " (TileIndex, MinElv, MinLat, MinLon, MaxElv, MaxLat, MaxLon, Rows, Columns, Elv, LastUpdated) VALUES (@TileIndex, @MinElv, @MinLat, @MinLon, @MaxElv, @MaxLat, @MaxLon, @Rows, @Columns, @Elv, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(tile.AsString("TileIndex"));
                db.DBCommand.Parameters.Add(tile.AsInt32("MinElv"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MinLat"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MinLon"));
                db.DBCommand.Parameters.Add(tile.AsInt32("MaxElv"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MaxLat"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MaxLon"));
                db.DBCommand.Parameters.Add(tile.AsInt32("Rows"));
                db.DBCommand.Parameters.Add(tile.AsInt32("Columns"));
                db.DBCommand.Parameters.Add(tile.AsBinary("Elv"));
                db.DBCommand.Parameters.Add(tile.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationTileBulkInsert(List<ElevationTileDesignator> tiles, ELEVATIONMODEL model)
        {
            int errors = 0;
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            try
            {
                db.BeginTransaction();
                foreach (ElevationTileDesignator tile in tiles)
                {
                    try
                    {
                        this.ElevationTileInsert(tile, model);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage("Error inserting tile [" + tile.TileIndex + "]: " + ex.ToString());
                        errors++;
                    }
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return -errors;
        }

        public int ElevationTileDelete(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + ElevationTileDesignator.TableName + " WHERE TileIndex = @TileIndex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(tile.AsString("TileIndex"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationTileUpdate(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + ElevationTileDesignator.TableName + " SET TileIndex = @TileIndex, MinElv = @MinElv, MinLat = @MinLat, MinLon = @MinLon, MaxElv = @MaxElv, MaxLat = @MaxLat, MaxLon = @MaxLon, Rows = @Rows, Columns = @Columns, Elv = @Elv, LastUpdated = @LastUpdated WHERE TileIndex = @TileIndex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(tile.AsString("TileIndex"));
                db.DBCommand.Parameters.Add(tile.AsInt32("MinElv"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MinLat"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MinLon"));
                db.DBCommand.Parameters.Add(tile.AsInt32("MaxElv"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MaxLat"));
                db.DBCommand.Parameters.Add(tile.AsDouble("MaxLon"));
                db.DBCommand.Parameters.Add(tile.AsInt32("Rows"));
                db.DBCommand.Parameters.Add(tile.AsInt32("Columms"));
                db.DBCommand.Parameters.Add(tile.AsBinary("Elv"));
                db.DBCommand.Parameters.Add(tile.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public void ElevationTileInsertOrUpdateIfNewer(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            DateTime dt = this.ElevationTileFindLastUpdated(tile,model);
            if (dt == DateTime.MinValue)
                this.ElevationTileInsert(tile, model);
            else if (dt < tile.LastUpdated)
                this.ElevationTileUpdate(tile, model);
        }

        public ElevationTileDesignator ElevationTileFind(string tileindex, ELEVATIONMODEL model)
        {
            ElevationTileDesignator tile = new ElevationTileDesignator(tileindex);
            return this.ElevationTileFind(tile, model);
        }

        public ElevationTileDesignator ElevationTileFind(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + ElevationTileDesignator.TableName + " WHERE TileIndex = @TileIndex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(tile.AsString("TileIndex"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                {
                    return new ElevationTileDesignator(Result.Rows[0]);
                }
                return null;
            }
        }

        public bool ElevationTileExists(string tileindex, ELEVATIONMODEL model)
        {
            ElevationTileDesignator tile = new ElevationTileDesignator(tileindex);
            return this.ElevationTileExists(tile, model);
        }

        public bool ElevationTileExists(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + ElevationTileDesignator.TableName + " WHERE TileIndex = @TileIndex)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(tile.AsString("TileIndex"));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public DateTime ElevationTileFindLastUpdated(string tileindex, ELEVATIONMODEL model)
        {
            ElevationTileDesignator tile = new ElevationTileDesignator(tileindex);
            return this.ElevationTileFindLastUpdated(tile, model);
        }

        public DateTime ElevationTileFindLastUpdated(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + ElevationTileDesignator.TableName + " WHERE TileIndex = @TileIndex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(tile.AsString("TileIndex"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (result != null)
                    return (SQLiteEntry.UNIXTimeToDateTime((int)result));
            }
            return DateTime.MinValue;
        }

        public ElvMinMaxInfo ElevationTileFindMinMaxInfo(string tileindex, ELEVATIONMODEL model)
        {
            ElevationTileDesignator tile = new ElevationTileDesignator(tileindex);
            return this.ElevationTileFindMinMaxInfo(tile, model);
        }

        public ElvMinMaxInfo ElevationTileFindMinMaxInfo(ElevationTileDesignator tile, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT MinElv, MinLat, MInLon, MaxElv, MaxLat, MaxLon FROM " + ElevationTileDesignator.TableName + " WHERE TileIndex = @TileIndex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(tile.AsString("TileIndex"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                {
                    short minelv = System.Convert.ToInt16(Result.Rows[0][0]);
                    double minlat = System.Convert.ToDouble(Result.Rows[0][1]);
                    double minlon = System.Convert.ToDouble(Result.Rows[0][2]);
                    short maxelv = System.Convert.ToInt16(Result.Rows[0][3]);
                    double maxlat = System.Convert.ToDouble(Result.Rows[0][4]);
                    double maxlon = System.Convert.ToDouble(Result.Rows[0][5]);
                    return new ElvMinMaxInfo(minelv, minlat, minlon, maxelv, maxlat, maxlon);
                }
            }
            return null;
        }

        public List<ElevationTileDesignator> ElevationTileGetAll(ELEVATIONMODEL model)
        {
            List<ElevationTileDesignator> l = new List<ElevationTileDesignator>();
            DataTable Result = GetElevationDatabase(model).Select("SELECT * FROM " + ElevationTileDesignator.TableName);
            if ((Result == null) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new ElevationTileDesignator(row));
            return l;
        }

        public List<ElevationTileDesignator> ElevationTileGetAll(BackgroundWorker caller, ELEVATIONMODEL model)
        {
            List<ElevationTileDesignator> l = new List<ElevationTileDesignator>();
            // gets all aircraftpositions from database
            // supports abort calculation if called from background worker and cancellation requested
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(GetElevationDatabase(model).DBConnection);
            cmd.CommandText = "SELECT * FROM " + ElevationTileDesignator.TableName;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ElevationTileDesignator tile = new ElevationTileDesignator((IDataRecord)reader);
                l.Add(tile);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<ElevationTileDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting tile " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public List<ElevationTileDesignator> ElevationTileGetAll(ELEVATIONMODEL model, double minlat, double minlon, double maxlat, double maxlon)
        {
            List<ElevationTileDesignator> l = new List<ElevationTileDesignator>();
            DataTable Result = GetElevationDatabase(model).Select("SELECT * FROM Elevation");
            if ((Result == null) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
            {
                ElevationTileDesignator tile = new ElevationTileDesignator(row);
                if ((tile.BaseLat >= minlat) && (tile.BaseLat <= maxlat) && ((tile.BaseLon >= minlon) && (tile.BaseLon <= maxlon)) ||
                    (tile.BaseLat + 1 / 24.0 >= minlat) && (tile.BaseLat + 1 / 24.0 <= maxlat) && ((tile.BaseLon + 2 / 24.0 >= minlon) && (tile.BaseLon + 2 / 24.0 <= maxlon)))
                    l.Add(tile);
            }
            return l;
        }

        public List<ElevationTileDesignator> ElevationTileGetAll(BackgroundWorker caller, ELEVATIONMODEL model, double minlat, double minlon, double maxlat, double maxlon)
        {
            List<ElevationTileDesignator> l = new List<ElevationTileDesignator>();
            // gets all elevation tiles from database
            // supports abort calculation if called from background worker and cancellation requested
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(GetElevationDatabase(model).DBConnection);
            cmd.CommandText = "SELECT * FROM " + ElevationTileDesignator.TableName;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ElevationTileDesignator tile = new ElevationTileDesignator((IDataRecord)reader);
                if ((tile.BaseLat >= minlat) && (tile.BaseLat <= maxlat) && ((tile.BaseLon >= minlon) && (tile.BaseLon <= maxlon)) ||
                    (tile.BaseLat + 1 / 24.0 >= minlat) && (tile.BaseLat + 1 / 24.0 <= maxlat) && ((tile.BaseLon + 2 / 24.0 >= minlon) && (tile.BaseLon + 2 / 24.0 <= maxlon)))
                {
                    l.Add(tile);
                    i++;
                }
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<ElevationTileDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting tile " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public short ElevationTilesMaxElevation (ELEVATIONMODEL model, double minlat, double minlon, double maxlat, double maxlon)
        {
            List<string> locs = ElevationData.Database.GetLocsFromRect(minlat, minlon, maxlat, maxlon, 3);
            short max = short.MinValue;
            foreach(string loc in locs)
            {
                ElvMinMaxInfo minmax = this.ElevationTileFindMinMaxInfo(loc, model);
                if ((minmax != null) && (minmax.MaxElv > max))
                    max = minmax.MaxElv;
            }
            return max;
        }

        public long ElevationTileCount(ELEVATIONMODEL model)
        {
            long count = (long)GetElevationDatabase(model).ExecuteScalar("SELECT COUNT(*) FROM " + ElevationTileDesignator.TableName);
            if (count <= 0)
                return 0;
            return count;
        }

        #endregion

        #region ElevationPath

        public int GetElevationPathAveragePeriod(ELEVATIONMODEL model)
        {
            if (model == ELEVATIONMODEL.ASTER1)
                return 5;
            if (model == ELEVATIONMODEL.ASTER3)
                return 5;
            return 0;

        }

        public bool ElevationPathTableExists(ELEVATIONMODEL model, string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = ElevationPathDesignator.TableName;
            return GetElevationDatabase(model).TableExists(tn);
        }

        public void ElevationPathCreateTable(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = ElevationPathDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE " + tn + " (Lat1 DOUBLE NOT NULL DEFAULT 0, Lon1 DOUBLE NOT NULL DEFAULT 0, Lat2 DOUBLE NOT NULL DEFAULT 0, Lon2 DOUBLE NOT NULL DEFAULT 0, StepWidth DOUBLE NOT NULL DEFAULT 0, Count INT32, Path BLOB, LastUpdated INT32, PRIMARY KEY(Lat1, Lon1, Lat2, Lon2, StepWidth))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public bool ElevationPathExists(double lat1, double lon1, double lat2, double lon2, double stepwidth, ELEVATIONMODEL model)
        {
            ElevationPathDesignator path = new ElevationPathDesignator(lat1, lon1, lat2, lon2, stepwidth);
            return this.ElevationPathExists(path, model);
        }

        public bool ElevationPathExists(ElevationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + ElevationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND StepWidth = @StepWidth)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public ElevationPathDesignator ElevationPathFind(double lat1, double lon1, double lat2, double lon2, double stepwidth, ELEVATIONMODEL model)
        {
            ElevationPathDesignator path = new ElevationPathDesignator(lat1, lon1, lat2, lon2, stepwidth);
            return this.ElevationPathFind(path, model);
        }

        public ElevationPathDesignator ElevationPathFind(ElevationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + ElevationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                    return new ElevationPathDesignator(Result.Rows[0]);
            }
            return null;
        }

        public DateTime ElevationPathFindLastUpdated(double lat1, double lon1, double lat2, double lon2, double stepwidth, ELEVATIONMODEL model)
        {
            ElevationPathDesignator path = new ElevationPathDesignator(lat1, lon1, lat2, lon2, stepwidth);
            return this.ElevationPathFindLastUpdated(path, model);
        }

        public DateTime ElevationPathFindLastUpdated(ElevationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + ElevationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (result != null)
                    return SQLiteEntry.UNIXTimeToDateTime((int)result);
            }
            return DateTime.MinValue;
        }

        public int ElevationPathInsert(ElevationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + ElevationPathDesignator.TableName + " (Lat1, Lon1, Lat2, Lon2, StepWidth, Count, Path, LastUpdated) VALUES (@Lat1, @Lon1, @Lat2, @Lon2, @StepWidth, @Count, @Path, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(path.AsInt32("Count"));
                db.DBCommand.Parameters.Add(path.AsBinary("Path"));
                db.DBCommand.Parameters.Add(path.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationPathDelete(ElevationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + ElevationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationPathDeleteAll(ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + ElevationPathDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationPathUpdate(ElevationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + ElevationPathDesignator.TableName + " SET Lat1 = @Lat1, Lon1 = @Lon1, Lat2 = @Lat2, Lon2 = @Lon2, StepWidth = @StepWidth, Count = @Count, Path = @Path, LastUpdated = @LastUpdated WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(path.AsInt32("Count"));
                db.DBCommand.Parameters.Add(path.AsBinary("Path"));
                db.DBCommand.Parameters.Add(path.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }
        public void ElevationPathInsertOrUpdateIfNewer(ElevationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            DateTime dt = this.ElevationPathFindLastUpdated(path, model);
            if (dt == DateTime.MinValue)
                this.ElevationPathInsert(path, model);
            else if (dt < path.LastUpdated)
                this.ElevationPathUpdate(path, model);
        }

        public ElevationPathDesignator ElevationPathCreateFromBearing(BackgroundWorker caller, double lat1, double lon1, double bearing, double distance, double stepwidth, ELEVATIONMODEL model)
        {
            LatLon.GPoint gp = LatLon.DestinationPoint(lat1, lon1, bearing, distance);
            return ElevationPathCreateFromLatLon(caller, lat1, lon1, gp.Lat, gp.Lon, stepwidth, model);
        }



        // simple moving average for elevation path
        // the resulting average array is (periods - 1)  shorter than the source array
        private short[] MovingAverage(short[] values, int periods)
        {
            // check for sufficient count of values
            if (values.Length < periods)
                return null;
            short[] averages = new short[values.Length - periods + 1];
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
                if (i < periods)
                {
                    sum += values[i];
                    //                    averages[i] = (short)((i == periods - 1) ? sum / (double)periods : 0);
                    averages[0] = (short)((i == periods - 1) ? sum / (double)periods : 0);
                }
                else
                {
                    sum = sum - values[i - periods] + values[i];
                    averages[i - periods + 1] = (short)(sum / (double)periods);
                }
            return averages;
        }

        public ElevationPathDesignator ElevationPathCreateFromLatLon(BackgroundWorker caller, double lat1, double lon1, double lat2, double lon2, double stepwidth, ELEVATIONMODEL model, bool savetodatabase = true)
        {
            // calculate new elevation path
            // supports abort calculation if called from background worker and cancellation requested
            // report of status messages and single data points not needed so far
            ElevationPathDesignator ep = new ElevationPathDesignator(lat1, lon1, lat2, lon2, stepwidth);
            bool tilemissing = false;
            // convert stepwidth to [km]
            stepwidth = stepwidth / 1000.0;
            bool complete = this.GetDBStatusBit(model, DATABASESTATUS.COMPLETE) & !this.GetDBStatusBit(model, DATABASESTATUS.ERROR);
            int avperiod = GetElevationPathAveragePeriod(model);
            // check for any averaging
            if (avperiod == 0)
            {
                // no averaging --> create path direct into ep
                double d = 0;
                // check if elevation database is complete before trying to retrieve elevation path
                for (int i = 0; i < ep.Count; i++)
                {
                    LatLon.GPoint gp = LatLon.DestinationPoint(lat1, lon1, ep.Bearing12, d);
                    // get elevation point with status
                    // tile will be cached locally for subsequent use
                    short e = GetElevation(model, gp.Lat, gp.Lon, false);
                    // set elevation point if valid, else set it to 0
                    if (e > TileMissingFlag)
                    {
                        ep.Path[i] = e;
                    }
                    else
                    {
                        ep.Path[i] = 0;
                        // set the tilemissing flag
                        if (e == TileMissingFlag)
                            tilemissing = true;
                    }
                    d += stepwidth;
                    // abort calculation if called from background worker and cancellation pending
                    if (caller != null)
                    {
                        if (caller.CancellationPending)
                            return null;
                    }
                }
            }
            else
            {
                // create raw elevation buffer first and copy the average to ep
                short[] raw = new short[ep.Path.Length + avperiod - 1];
                // put the start value at the half of avperiod back in opposite direction
                double d = -(avperiod / 2) * stepwidth;
                // check if elevation database is complete before trying to retrieve elevation path
                for (int i = 0; i < raw.Length; i++)
                {
                    LatLon.GPoint gp = LatLon.DestinationPoint(lat1, lon1, ep.Bearing12, d);
                    // get elevation point with status
                    // tile will be cached locally for subsequent use
                    short e = GetElevation(model, gp.Lat, gp.Lon, false);
                    // set elevation point if valid, else set it to 0
                    if (e > TileMissingFlag)
                    {
                        raw[i] = e;
                    }
                    else
                    {
                        raw[i] = 0;
                        // set the tilemissing flag
                        if (e == TileMissingFlag)
                            tilemissing = true;
                    }
                    d += stepwidth;
                    // abort calculation if called from background worker and cancellation pending
                    if (caller != null)
                    {
                        if (caller.CancellationPending)
                            return null;
                    }
                }
                // calculate average and assign it to ep.Path
                ep.Path = MovingAverage(raw, avperiod);
            }
            
            // check if database is still complete, could have benn changed during background calculation
            if (complete)
                complete = GetDBStatusBit(model, DATABASESTATUS.COMPLETE) & !GetDBStatusBit(model, DATABASESTATUS.ERROR);
            // validate path according to completeness of database
            ep.Valid = complete;
            // oops, tile is missing --> check if database status
            // COMPLETE: check boounds --> path complete inside bounds --> assuming that missing tile is a "wet square" --> keep path valid
            // NOT COMPLETE: assuming that tile is missing --> invalidate path 
            if (tilemissing)
            { 
                if (complete)
                {
                    // check bounds --> invalidate if out of bounds
                    if ((lat1 < MinLat) || (lat1 > MaxLat) ||
                        (lon1 < MinLon) || (lon1 > MaxLon) ||
                        (lat2 < MinLat) || (lat2 > MaxLat) ||
                        (lon2 < MinLon) || (lon2 > MaxLon))
                        ep.Valid = false;

                }
            }
            // store path in database if valid
            if ((ep != null) && ep.Valid && savetodatabase)
                this.ElevationPathInsertOrUpdateIfNewer(ep, model);
            return ep;
        }

        public ElevationPathDesignator ElevationPathFindOrCreateFromBearing(BackgroundWorker caller, double lat1, double lon1, double bearing, double distance, double stepwidth, ELEVATIONMODEL model, bool savetodatabase = true)
        {
            LatLon.GPoint gp = LatLon.DestinationPoint(lat1, lon1, bearing, distance);
            return ElevationPathFindOrCreateFromLatLon(caller, lat1, lon1, gp.Lat, gp.Lon, stepwidth, model,savetodatabase);
        }

        public ElevationPathDesignator ElevationPathFindOrCreateFromLatLon(BackgroundWorker caller, double lat1, double lon1, double lat2, double lon2, double stepwidth, ELEVATIONMODEL model, bool savetodatabase = true)
        {
            ElevationPathDesignator ep = this.ElevationPathFind(lat1, lon1, lat2, lon2, stepwidth, model);
            if (ep == null)
            {
                ep = this.ElevationPathCreateFromLatLon(caller, lat1, lon1, lat2, lon2, stepwidth, model, savetodatabase);
            }
            return ep;
        }

        #endregion

        #region ElevationHorizon

        public bool ElevationHorizonTableExists(ELEVATIONMODEL model, string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = ElevationHorizonDesignator.TableName;
            return GetElevationDatabase(model).TableExists(tn);
        }

        public void ElevationHorizonCreateTable(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = ElevationHorizonDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE " + tn + " (Lat DOUBLE NOT NULL DEFAULT 0, Lon DOUBLE NOT NULL DEFAULT 0, Distance DOUBLE NOT NULL DEFAULT 0, StepWidth DOUBLE NOT NULL DEFAULT 0, Count INT32, Paths BLOB, LastUpdated INT32, PRIMARY KEY(Lat, Lon, Distance, StepWidth))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public bool ElevationHorizonExists(double lat, double lon, double distance, double stepwidth, ELEVATIONMODEL model)
        {
            ElevationHorizonDesignator path = new ElevationHorizonDesignator(lat, lon, distance, stepwidth);
            return this.ElevationHorizonExists(path, model);
        }

        public bool ElevationHorizonExists(ElevationHorizonDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + ElevationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND Distance = @Distance AND StepWidth = @StepWidth)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(path.AsDouble("Distance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public ElevationHorizonDesignator ElevationHorizonFind(double lat, double lon, double distance, double stepwidth, ELEVATIONMODEL model)
        {
            ElevationHorizonDesignator path = new ElevationHorizonDesignator(lat, lon, distance, stepwidth);
            return this.ElevationHorizonFind(path, model);
        }

        public ElevationHorizonDesignator ElevationHorizonFind(ElevationHorizonDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + ElevationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND Distance = @Distance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(path.AsDouble("Distance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                    return new ElevationHorizonDesignator(Result.Rows[0]);
            }
            return null;
        }

        public DateTime ElevationHorizonFindLastUpdated(double lat, double lon, double distance, double stepwidth, ELEVATIONMODEL model)
        {
            ElevationHorizonDesignator path = new ElevationHorizonDesignator(lat, lon, distance, stepwidth);
            return this.ElevationHorizonFindLastUpdated(path, model);
        }

        public DateTime ElevationHorizonFindLastUpdated(ElevationHorizonDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + ElevationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND Distance = @Distance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(path.AsDouble("Distance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (result != null)
                    return SQLiteEntry.UNIXTimeToDateTime((int)result);
            }
            return DateTime.MinValue;
        }

        public int ElevationHorizonInsert(ElevationHorizonDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + ElevationHorizonDesignator.TableName + " (Lat, Lon, Distance, StepWidth, Count, Paths, LastUpdated) VALUES (@Lat, @Lon, @Distance, @StepWidth, @Count, @Paths, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(path.AsDouble("Distance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(path.AsInt32("Count"));
                db.DBCommand.Parameters.Add(path.AsBinary("Paths"));
                db.DBCommand.Parameters.Add(path.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationHorizonDelete(ElevationHorizonDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + ElevationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND Distance = @Distance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(path.AsDouble("Distance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationHorizonDeleteAll(ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + ElevationHorizonDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int ElevationHorizonUpdate(ElevationHorizonDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + ElevationHorizonDesignator.TableName + " SET Lat = @Lat, Lon = @Lon, Distance = @Distance, StepWidth = @StepWidth, Count = @Count, Paths = @Paths, LastUpdated = @LastUpdated WHERE Lat = @Lat AND Lon = @Lon AND Distance = @Distance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(path.AsDouble("Distance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(path.AsInt32("Count"));
                db.DBCommand.Parameters.Add(path.AsBinary("Paths"));
                db.DBCommand.Parameters.Add(path.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }
        public void ElevationHorizonInsertOrUpdateIfNewer(ElevationHorizonDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            DateTime dt = this.ElevationHorizonFindLastUpdated(path, model);
            if (dt == DateTime.MinValue)
                this.ElevationHorizonInsert(path, model);
            else if (dt < path.LastUpdated)
                this.ElevationHorizonUpdate(path, model);
        }
        public ElevationHorizonDesignator ElevationHorizonCreate(BackgroundWorker caller, double lat, double lon, double distance, double stepwidth, ELEVATIONMODEL model, bool savetodatabase = true)
        {
            ElevationHorizonDesignator hd = this.ElevationHorizonFind(lat, lon, distance, stepwidth, model);
            // create an ElevationPath per degree and save it to horizon
            // supports abort of calculation if called from a background thread and cancellation is pending
            for (int j = 0; j < 360; j++)
            {
                // get full elevation path
                ElevationPathDesignator ep = ElevationPathFindOrCreateFromBearing(caller, lat, lon, j, distance, stepwidth, model, false);
                // return null if elevation path is null for whatever reason
                if (ep == null)
                    return null;
                // find biggest prominence
                double eps_max = double.MinValue;
                int index = 0;
                for (int i = 0; i < ep.Count; i++)
                {
                    // assume an antenna height of 100m and a standard K-Factor of 1.33
                    double eps = Propagation.EpsilonFromHeights(ep.Path[0] + 100.0, (double)i * ep.StepWidth / 1000.0, ep.Path[i], 1.33 * LatLon.Earth.Radius);
                    if (eps > eps_max)
                    {
                        eps_max = eps;
                        index = i;
                    }
                    if (caller != null)
                    {
                        // abort calculation if cancellation pending
                        if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                            return null;
                    }
                }
                // calculate endpoint
                // cut path to biggest prominence to save space
                short[] a = ep.Path;
                Array.Resize(ref a, index);
                hd[j] = a;
                // reset valid flag if invalid
                if (!ep.Valid)
                    hd.Valid = false;
                // store horizon in database if valid
                if ((hd != null) && hd.Valid && savetodatabase)
                    this.ElevationHorizonInsertOrUpdateIfNewer(hd, model);
            }
            return hd;
        }

        public ElevationHorizonDesignator ElevationHorizonFindOrCreate(BackgroundWorker caller, double lat, double lon, double distance, double stepwidth, ELEVATIONMODEL model, bool savetodatabase = true)
        {
            ElevationHorizonDesignator hd = this.ElevationHorizonFind(lat, lon, distance, stepwidth, model);
            if (hd == null)
                hd = ElevationHorizonCreate(caller, lat, lon, distance, stepwidth, model, savetodatabase);
            return hd;
        }

        #endregion


        #region LocalObstruction

        public bool LocalObstructionTableExists(ELEVATIONMODEL model, string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = LocalObstructionDesignator.TableName;
            return GetElevationDatabase(model).TableExists(tn);
        }

        public void LocalObstructionCreateTable(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = LocalObstructionDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE " + tn + " (Lat DOUBLE NOT NULL DEFAULT 0, Lon DOUBLE NOT NULL DEFAULT 0, Distance BLOB, Height BLOB, LastUpdated INT32, PRIMARY KEY(Lat, Lon))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public bool LocalObstructionExists(double lat, double lon, ELEVATIONMODEL model)
        {
            LocalObstructionDesignator obstr = new LocalObstructionDesignator(lat, lon);
            return this.LocalObstructionExists(obstr, model);
        }

        public bool LocalObstructionExists(LocalObstructionDesignator obstr, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + LocalObstructionDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lon"));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public LocalObstructionDesignator LocalObstructionFind(double lat, double lon, ELEVATIONMODEL model)
        {
            LocalObstructionDesignator obstr = new LocalObstructionDesignator(lat, lon);
            return this.LocalObstructionFind(obstr, model);
        }

        public LocalObstructionDesignator LocalObstructionFindOrCreateDefault(double lat, double lon, ELEVATIONMODEL model, bool savetodatabase = true)
        {
            LocalObstructionDesignator obstr = this.LocalObstructionFind(lat, lon, model);
            if (obstr == null)
            {
                obstr = new LocalObstructionDesignator(lat, lon);
                if (savetodatabase)
                    LocalObstructionInsert(obstr, model);
            }
            return obstr;
        }

        public LocalObstructionDesignator LocalObstructionFind(LocalObstructionDesignator obstr, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + LocalObstructionDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lon"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                    return new LocalObstructionDesignator(Result.Rows[0]);
            }
            return null;
        }

        public DateTime LocalObstructionFindLastUpdated(double lat, double lon, ELEVATIONMODEL model)
        {
            LocalObstructionDesignator obstr = new LocalObstructionDesignator(lat, lon);
            return this.LocalObstructionFindLastUpdated(obstr, model);
        }

        public DateTime LocalObstructionFindLastUpdated(LocalObstructionDesignator obstr, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + LocalObstructionDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lon"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (result != null)
                    return SQLiteEntry.UNIXTimeToDateTime((int)result);
            }
            return DateTime.MinValue;
        }

        public int LocalObstructionInsert(LocalObstructionDesignator obstr, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + LocalObstructionDesignator.TableName + " (Lat, Lon, Distance, Height, LastUpdated) VALUES (@Lat, @Lon, @Distance, @Height, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(obstr.AsBinary("Distance"));
                db.DBCommand.Parameters.Add(obstr.AsBinary("Height"));
                db.DBCommand.Parameters.Add(obstr.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int LocalObstructionDelete(LocalObstructionDesignator obstr, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + LocalObstructionDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lon"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int LocalObstructionDeleteAll(ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + LocalObstructionDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int LocalObstructionUpdate(LocalObstructionDesignator obstr, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + LocalObstructionDesignator.TableName + " SET Lat = @Lat, Lon = @Lon, Distance = @Distance, Height = @Height, LastUpdated = @LastUpdated WHERE Lat = @Lat AND Lon = @Lon";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(obstr.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(obstr.AsBinary("Distance"));
                db.DBCommand.Parameters.Add(obstr.AsBinary("Height"));
                db.DBCommand.Parameters.Add(obstr.AsUNIXTime("LastUpdated"));
                int result = db.ExecuteNonQuery(db.DBCommand);
                return result;
            }
        }
        public void LocalObstructionInsertOrUpdateIfNewer(LocalObstructionDesignator obstr, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetElevationDatabase(model);
            DateTime dt = this.LocalObstructionFindLastUpdated(obstr, model);
            if (dt == DateTime.MinValue)
                this.LocalObstructionInsert(obstr, model);
            else if (dt < obstr.LastUpdated)
                this.LocalObstructionUpdate(obstr, model);
        }
        #endregion

        #region ElevationCatalogue

        public ElevationCatalogue ElevationCatalogueCreateCheckBoundsAndLastModified(BackgroundWorker caller, ELEVATIONMODEL model, double minlat, double minlon, double maxlat, double maxlon)
        {
            ElevationCatalogue ec;
            string jsonfile = Path.Combine(DefaultDatabaseDirectory(model), JSONFile(model));
            // try to read cached catalogue first
            ec = ElevationCatalogue.FromJSONFileCheckBoundsAndLastModified(jsonfile, minlat, minlon, maxlat, maxlon);
            // create and save new catalogue if not found or not matching
            if (ec == null)
            {
                ec = new ElevationCatalogue(caller, UpdateURL(model), DefaultDatabaseDirectory(model), minlat, minlon, maxlat, maxlon);
                if (ec.Files.Count > 0)
                {
                    ec.ToJSONFile(jsonfile);
                    File.SetLastWriteTimeUtc(jsonfile, ec.LastModified);
                }
            }
            return ec;
        }
        #endregion

        #region Upgrade

        public void UpgradeTableToV1(System.Data.SQLite.SQLiteDatabase db, string tablename)
        {
            int count = db.TableRowCount(tablename);
            // copy rows, if any
            // don't care about the column type --> SQLite can handle that
            if (count > 0)
            {
                db.Execute("UPDATE " + tablename + " SET LastUpdated = strftime('%s',LastUpdated)");
            }
            // change table's structure
            // BE CAREFUL HERE!!!
            int schema_version = db.GetSchemaVersion();
            db.Execute("PRAGMA writable_schema = ON");
            db.Execute("UPDATE sqlite_master SET sql = replace(sql, 'LastUpdated TEXT', 'LastUpdated INT32') WHERE type = 'table' AND name = '" + tablename + "'");
            schema_version++;
            db.SetSchemaVerion(schema_version);
            db.Execute("PRAGMA writable_schema = OFF");
        }

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


    #endregion


}
