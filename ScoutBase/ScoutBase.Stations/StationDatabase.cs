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
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using ScoutBase.Core;
using ScoutBase.Database;
using Newtonsoft.Json;

namespace ScoutBase.Stations
{

    public class StationData
    {
        static StationDatabase stations = new StationDatabase();
        public static StationDatabase Database
        {
            get
            {
                return stations;
            }
        }

    }

    /// <summary>
    /// Holds the Station information in a database structure.
    /// </summary>
    public class StationDatabase : ScoutBaseDatabase
    {
        public StationDatabase()
        {
            UserVersion = 1;
            Name = "ScoutBase Station Database";
            Description = "The Scoutbase Station Database is containing location and QRV infos. Each information has a unique key: \n" +
                "callsign and 6-digit Maidenhead locator square. \n" +
                "The database is periodically updated from a global web resource. The user can modify / add entries via user interface.";
            // add table description manually
            TableDescriptions.Add(LocationDesignator.TableName, "Holds location information.");
            TableDescriptions.Add(QRVDesignator.TableName, "Holds QRV information.");
            db = OpenDatabase("stations.db3", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
            // create tables with schemas if not exist
            if (!LocationTableExists())
                LocationCreateTable();
            if (!QRVTableExists())
                QRVCreateTable();
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
                dir = "StationData";
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

        public string UpdateURL()
        {
            return Properties.Settings.Default.Stations_UpdateURL;
        }

        #region Location

        public bool LocationTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = LocationDesignator.TableName;
            return db.TableExists(tn);
        }

        public void LocationCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = LocationDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE `" + tn + "`(Call TEXT NOT NULL DEFAULT '', Loc TEXT NOT NULL DEFAULT '', Lat REAL, Lon REAL, Source INT32, Hits INT32, LastUpdated INT32, PRIMARY KEY (Call, Loc))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long LocationCount()
        {
            long count = (long)db.ExecuteScalar("SELECT COUNT(*) FROM " + LocationDesignator.TableName);
            if (count <= 0)
                return 0;
            return count;
        }

        public bool LocationExists(string call, string loc)
        {
            LocationDesignator ld = new LocationDesignator(call, loc);
            return LocationExists(ld);
        }

        public bool LocationExists(string call)
        {
            LocationDesignator ld = new LocationDesignator(call);
            return LocationExists(ld);
        }

        public bool LocationExists(LocationDesignator ld)
        {
            if (String.IsNullOrEmpty(ld.Loc))
            {
                // Loc is empty --> search for any location for this call
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + LocationDesignator.TableName + " WHERE Call = @Call)";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(ld.AsString("Call"));
                    long result = (long)db.DBCommand.ExecuteScalar();
                    if (result > 0)
                        return true;
                }
            }
            else
            {
                // Loc is not empty --> search for distinct location for this call
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + LocationDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc)";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(ld.AsString("Call"));
                    db.DBCommand.Parameters.Add(ld.AsString("Loc"));
                    long result = (long)db.DBCommand.ExecuteScalar();
                    if (result > 0)
                        return true;
                }
            }
            return false;
        }

        public LocationDesignator LocationFindAt(long index)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + LocationDesignator.TableName + " LIMIT 1 OFFSET " + index.ToString();
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new LocationDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return null;
        }

        public List<LocationDesignator> LocationFindAll(string call)
        {
            List<LocationDesignator> l = new List<LocationDesignator>();
            if (String.IsNullOrEmpty(call))
                return l;
            LocationDesignator ld = new LocationDesignator(call);
            lock (db.DBCommand)
            {
                // Loc is empty --> search for last recent location for this call
                db.DBCommand.CommandText = "SELECT * FROM " + LocationDesignator.TableName + " WHERE Call = @Call ORDER BY LastUpdated DESC";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("Call"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                {
                    foreach (DataRow row in Result.Rows)
                    {
                        l.Add(new LocationDesignator(row));
                    }
                    return l;
                }
            }
            return null;
        }

        public LocationDesignator LocationFindLastRecent(string call)
        {
            LocationDesignator ld = new LocationDesignator(call);
            if (String.IsNullOrEmpty(call))
                return null;
            lock (db.DBCommand)
            {
                // Loc is empty --> search for last recent location for this call
                db.DBCommand.CommandText = "SELECT * FROM " + LocationDesignator.TableName + " WHERE Call = @Call ORDER BY LastUpdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("Call"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                    return new LocationDesignator(Result.Rows[0]);
            }
            return null;
        }

        public LocationDesignator LocationFindMostHit(string call)
        {
            LocationDesignator ld = new LocationDesignator(call);
            if (String.IsNullOrEmpty(call))
                return null;
            lock (db.DBCommand)
            {
                // Loc is empty --> search for last recent location for this call
                db.DBCommand.CommandText = "SELECT * FROM " + LocationDesignator.TableName + "WHERE Call = @Call ORDER BY Hits DESC LIMIT 1 ";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("Call"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                    return new LocationDesignator(Result.Rows[0]);
            }
            return null;
        }

        public LocationDesignator LocationFind(string call)
        {
            if (String.IsNullOrEmpty(call))
                return null;
            return LocationFindLastRecent(call);
        }

        public LocationDesignator LocationFind(string call, string loc)
        {
            if (String.IsNullOrEmpty(call) || String.IsNullOrEmpty(loc))
                return null;
            LocationDesignator ld = new LocationDesignator(call, loc);
            return LocationFind(ld);
        }

        public LocationDesignator LocationFind(LocationDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + LocationDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("Call"));
                db.DBCommand.Parameters.Add(ld.AsString("Loc"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                {
                    foreach (DataColumn col in Result.Columns)
                    {
 //                       Console.WriteLine(col.ColumnName + "; " + col.DataType.ToString());
                    }
                    foreach (object item in Result.Rows[0].ItemArray)
 //                       Console.WriteLine(item.ToString() + ", ");
                    return new LocationDesignator(Result.Rows[0]);
                }
            }
            return null;
        }

        public LocationDesignator LocationFindOrUpdateOrCreate(string call, double lat, double lon)
        {
            if (String.IsNullOrEmpty(call))
                return null;
            LocationDesignator ld = new LocationDesignator(call, MaidenheadLocator.LocFromLatLon(lat, lon, false, 3));
            // try to find entry with call & loc matching
            ld = LocationFind(ld);
            // if not found --> create and insert new entry
            if (ld == null)
            {
                ld = new LocationDesignator(call, lat, lon);
                this.LocationInsert(ld);
                return ld;
            }
            // entry with that loc was found --> check lat/lon
            if ((ld.Lat != lat) || (ld.Lon != lon))
            {
                // entry found --> update lat/lon and update database
                ld.Lat = lat;
                ld.Lon = lon;
                ld.LastUpdated = DateTime.UtcNow;
                LocationUpdate(ld);
                return ld;
            }
            // return unmodified ld found in database
            return ld;
        }

        public LocationDesignator LocationFindOrCreate(string call, string loc)
        {
            if (String.IsNullOrEmpty(call) || String.IsNullOrEmpty(loc))
                return null;
            LocationDesignator ld = this.LocationFind(call, loc);
            if (ld == null)
            {
                ld = new LocationDesignator(call, loc);
                this.LocationInsert(ld);
            }
            return ld;
        }

        public DateTime LocationFindlastUpdated(string call)
        {
            LocationDesignator ld = LocationFindLastRecent(call);
            if (ld != null)
                return ld.LastUpdated;
            return DateTime.MinValue;
        }

        public DateTime LocationFindlastUpdated(string call, string loc)
        {
            LocationDesignator ld = new LocationDesignator(call, loc);
            return LocationFindLastUpdated(ld);
        }

        public DateTime LocationFindLastUpdated(LocationDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + LocationDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("Call"));
                db.DBCommand.Parameters.Add(ld.AsString("Loc"));
                DataTable result = db.Select(db.DBCommand);
                if (result != null && result.Rows.Count > 0)
                    return SQLiteEntry.UNIXTimeToDateTime(Convert.ToInt32(result.Rows[0][0]));
            }
            return DateTime.MinValue;
        }

        public int LocationInsert(LocationDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + LocationDesignator.TableName + " (Call, Loc, Lat, Lon, Source, Hits, LastUpdated) VALUES (@Call, @Loc, @Lat, @Lon, @Source, @Hits, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("Call"));
                db.DBCommand.Parameters.Add(ld.AsString("Loc"));
                db.DBCommand.Parameters.Add(ld.AsSingle("Lat"));
                db.DBCommand.Parameters.Add(ld.AsSingle("Lon"));
                db.DBCommand.Parameters.Add(ld.AsInt32("Source"));
                db.DBCommand.Parameters.Add(ld.AsInt32("Hits"));
                db.DBCommand.Parameters.Add(ld.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int LocationDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + LocationDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int LocationDelete(string call)
        {
            LocationDesignator ld = new LocationDesignator(call);
            return LocationDelete(ld);
        }

        public int LocationDelete(string call, string loc)
        {
            LocationDesignator ld = new LocationDesignator(call, loc);
            return LocationDelete(ld);
        }


        public int LocationDelete(LocationDesignator ld)
        {
            if (String.IsNullOrEmpty(ld.Loc))
            {
                lock (db.DBCommand)
                {
                    // Loc is empty --> delete all locations for this call
                    db.DBCommand.CommandText = "DELETE FROM " + LocationDesignator.TableName + " WHERE Call = @Call";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(ld.AsString("Call"));
                    return db.ExecuteNonQuery(db.DBCommand);
                }
            }
            else
            {
                // loc is not emppty --> delete distinct call and loc
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "DELETE FROM " + LocationDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(ld.AsString("Call"));
                    db.DBCommand.Parameters.Add(ld.AsString("Loc"));
                    return db.ExecuteNonQuery(db.DBCommand);
                }
            }
        }

        public int LocationUpdate(LocationDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + LocationDesignator.TableName + " SET Call = @Call, Loc = @Loc, Lat = @Lat, Lon = @Lon, Source = @Source, Hits = @Hits, LastUpdated = @LastUpdated WHERE Call = @Call AND Loc = @Loc";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("Call"));
                db.DBCommand.Parameters.Add(ld.AsString("Loc"));
                db.DBCommand.Parameters.Add(ld.AsSingle("Lat"));
                db.DBCommand.Parameters.Add(ld.AsSingle("Lon"));
                db.DBCommand.Parameters.Add(ld.AsInt32("Source"));
                db.DBCommand.Parameters.Add(ld.AsInt32("Hits"));
                db.DBCommand.Parameters.Add(ld.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int LocationBulkInsert(List<LocationDesignator> lds)
        {
            int errors = 0;
            try
            {
                db.BeginTransaction();
                foreach (LocationDesignator ld in lds)
                {
                    try
                    {
                        LocationInsert(ld);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage("Error inserting location [" + ld.Call + "]: " + ex.ToString(), LogLevel.Error);
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

        public int LocationBulkDelete(List<LocationDesignator> lds)
        {
            int errors = 0;
            try
            {
                db.BeginTransaction();
                foreach (LocationDesignator ld in lds)
                {
                    try
                    {
                        LocationDelete(ld);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage("Error deleting location [" + ld.Call + "]: " + ex.ToString(), LogLevel.Error);
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

        public int LocationBulkInsertOrUpdateIfNewer(List<LocationDesignator> lds)
        {
            if (lds == null)
                return 0;
            db.BeginTransaction();
            int i = 0;
            foreach (LocationDesignator ld in lds)
            {
                LocationInsertOrUpdateIfNewer(ld);
                i++;
            }
            db.Commit();
            return i;
        }

        public int LocationInsertOrUpdateIfNewer(LocationDesignator ld)
        {
            DateTime dt = LocationFindLastUpdated(ld);
            if (dt == DateTime.MinValue)
                return LocationInsert(ld);
            if (dt < ld.LastUpdated)
                return LocationUpdate(ld);
            return 0;
        }

        public List<LocationDesignator> LocationGetAll()
        {
            List<LocationDesignator> l = new List<LocationDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + LocationDesignator.TableName + " ORDER BY Call ASC");
            if ((Result == null) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new LocationDesignator(row));
            return l;

        }

        public List<LocationDesignator> LocationGetAll(string call)
        {
            List<LocationDesignator> l = new List<LocationDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + LocationDesignator.TableName + " WHERE CALL LIKE '" + call + "' ORDER BY Call ASC");
            if ((Result == null) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new LocationDesignator(row));
            return l;

        }

        public List<LocationDesignator> LocationGetAll(BackgroundWorker caller)
        {
            // gets all locations from database
            // supports abort calculation if called from background worker and cancellation requested
            List<LocationDesignator> l = new List<LocationDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + LocationDesignator.TableName + " ORDER BY Call ASC";
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LocationDesignator ld = new LocationDesignator((IDataRecord)reader);
                l.Add(ld);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<LocationDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting location " + i.ToString());
                }
            }
            reader.Close();
            return l;
        }

        public List<LocationDesignator> LocationGetAll(double minlat, double minlon, double maxlat, double maxlon)
        {
            List<LocationDesignator> l = new List<LocationDesignator>();
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + LocationDesignator.TableName + " WHERE Lat >= @minlat AND Lat <= @maxlat AND Lon >= @minlon AND Lon <= @maxlon ORDER BY Call ASC";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.AddWithValue("@minlat", minlat);
                db.DBCommand.Parameters.AddWithValue("@maxlat", maxlat);
                db.DBCommand.Parameters.AddWithValue("@minlon", minlon);
                db.DBCommand.Parameters.AddWithValue("@maxlon", maxlon);
                DataTable Result = db.Select(db.DBCommand);
                if (Result != null)
                {
                    foreach (DataRow row in Result.Rows)
                        l.Add(new LocationDesignator(row));
                }
            }
            return l;
        }

        public List<LocationDesignator> LocationGetAll(BackgroundWorker caller, double minlat, double minlon, double maxlat, double maxlon)
        {
            // gets all locations from database
            // supports abort calculation if called from background worker and cancellation requested
            List<LocationDesignator> l = new List<LocationDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + LocationDesignator.TableName + " WHERE Lat >= @minlat AND Lat <= @maxlat AND Lon >= @minlon AND Lon <= @maxlon ORDER BY Call ASC";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@minlat", minlat);
            cmd.Parameters.AddWithValue("@maxlat", maxlat);
            cmd.Parameters.AddWithValue("@minlon", minlon);
            cmd.Parameters.AddWithValue("@maxlon", maxlon);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LocationDesignator ld = new LocationDesignator((IDataRecord)reader);
                l.Add(ld);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<LocationDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting location " + i.ToString());
                }
            }
            reader.Close();
            return l;
        }

        public List<LocationDesignator> LocationFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<LocationDesignator>>(json, settings);
        }

        public string LocationToJSON()
        {
            List<LocationDesignator> l = LocationGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public DataTableLocations LocationToDataTable()
        {
            List<LocationDesignator> ld = LocationGetAll();
            DataTableLocations dtl = new DataTableLocations(ld);
            return dtl;
        }

        public List<LocationDesignator> LocationFromTXT(string txt)
        {
            // imports a TXT with old AirScout data format
            List<LocationDesignator> l = new List<LocationDesignator>();
            try
            {
                string[] locs = txt.Split('\n');
                foreach (string s in locs)
                { 
                    if (!String.IsNullOrEmpty(s) && !s.StartsWith("//"))
                    {
                        string[] a = s.Split(';');
                        // store array values in LocationDesignator
                        LocationDesignator ld = new LocationDesignator();
                        string call = a[0];
                        double lat = System.Convert.ToDouble(a[1], CultureInfo.InvariantCulture);
                        double lon = System.Convert.ToDouble(a[2], CultureInfo.InvariantCulture);
                        int hits = 0;
                        string loc = a[3];
                        GEOSOURCE source = GEOSOURCE.UNKONWN;
                        if (MaidenheadLocator.IsPrecise(lat, lon, loc.Length / 2))
                            source = GEOSOURCE.FROMUSER;
                        else
                            source = GEOSOURCE.FROMLOC;
                        DateTime dt = DateTime.Parse(a[6], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                        dt = dt.ToUniversalTime();
                        if (GeographicalPoint.Check(lat, lon))
                        {
                            ld.Call = call;
                            ld.Lat = lat;
                            ld.Lon = lon;
                            ld.Loc = loc;
                            ld.Source = source;
                            ld.Hits = hits;
                            ld.LastUpdated = dt;
                            l.Add(ld);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return l;
        }

        public List<LocationDesignator> LocationFromV1_2(string filename)
        {
            // get list of locations from a V1.2.xxx database file
            List<LocationDesignator> l = new List<LocationDesignator>();
            try
            {
                System.Data.SQLite.SQLiteDatabase dbo;
                dbo = new System.Data.SQLite.SQLiteDatabase(filename);
                dbo.Open();
                lock (dbo.DBCommand)
                {
                    dbo.DBCommand.CommandText = "SELECT * FROM Callsigns";
                    DataTable Result = dbo.Select(dbo.DBCommand);
                    if (Result != null)
                    {
                        foreach (DataRow row in Result.Rows)
                        {
                            string call = row["Call"].ToString();
                            double lat = System.Convert.ToDouble(row["Lat"]);
                            double lon = System.Convert.ToDouble(row["Lon"]);
                            string loc = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3, false);
                            GEOSOURCE source = (GEOSOURCE)Enum.Parse(typeof(GEOSOURCE), row["Source"].ToString());
                            DateTime lastupdated = DateTime.Parse(row["LastUpdated"].ToString(),CultureInfo.InvariantCulture,DateTimeStyles.AssumeUniversal).ToUniversalTime();
                            if (Callsign.Check(call) && MaidenheadLocator.Check(loc))
                            {
                                LocationDesignator ld = new LocationDesignator(call, loc, lat, lon, source, 0, lastupdated);
                                l.Add(ld);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return l;
        }

        #endregion

        #region QRV

        public bool QRVTableExists()
        {
        return db.TableExists(QRVDesignator.TableName);
        }

        public void QRVCreateTable()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "CREATE TABLE `" + QRVDesignator.TableName + "`(Call TEXT NOT NULL DEFAULT '', Loc TEXT NOT NULL DEFAULT '', Band Int32 NOT NULL DEFAULT 0, AntennaHeight REAL, AntennaGain REAL, Power REAL, LastUpdated INT32, PRIMARY KEY (Call, Loc, Band))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long QRVCount()
        {
            long count = (long)db.ExecuteScalar("SELECT COUNT(*) FROM " + QRVDesignator.TableName);
            if (count <= 0)
                return 0;
            return count;
        }

        public bool QRVExists(string call, string loc, BAND band)
        {
            QRVDesignator qrv = new QRVDesignator(call,loc, band);
            return QRVExists(qrv);
        }

        public bool QRVExists(QRVDesignator qrv)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + QRVDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc AND Band = @Band)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                db.DBCommand.Parameters.Add(qrv.AsString("Loc"));
                db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public QRVDesignator QRVFind(string call, string loc, BAND band)
        {
            QRVDesignator qrv = new QRVDesignator(call, loc, band);
            return QRVFind(band, qrv);
        }

        public QRVDesignator QRVFindOrCreateDefault(string call, string loc, BAND band)
        {
            QRVDesignator qrv = new QRVDesignator(call, loc, band);
            qrv = QRVFind(band, qrv);
            if (qrv == null)
                qrv = new QRVDesignator(call, loc, band, QRVGetDefaultAntennaHeight(band), QRVGetDefaultAntennaGain(band), QRVGetDefaultPower(band));
            return qrv;
        }

        public List<QRVDesignator> QRVFind(string call, string loc)
        {
            QRVDesignator qrv = new QRVDesignator(call,loc, BAND.BNONE);
            return QRVFind(qrv);
        }

        public QRVDesignator QRVFind(BAND band, QRVDesignator qrv)
        {
            if (!String.IsNullOrEmpty(qrv.Loc))
            {
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "SELECT * FROM " + QRVDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc AND Band = @Band";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                    db.DBCommand.Parameters.Add(qrv.AsString("Loc"));
                    db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                        return new QRVDesignator(Result.Rows[0]);
                }
            }
            else
            {
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "SELECT * FROM " + QRVDesignator.TableName + " WHERE Call = @Call AND Band = @Band";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                    db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                        return new QRVDesignator(Result.Rows[0]);
                }
            }
            return null;
        }

        public List<QRVDesignator> QRVFind(QRVDesignator qrv)
        {
            if (!String.IsNullOrEmpty(qrv.Loc))
            {
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "SELECT * FROM " + QRVDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                    db.DBCommand.Parameters.Add(qrv.AsString("Loc"));
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        List<QRVDesignator> l = new List<QRVDesignator>();
                        foreach (DataRow row in Result.Rows)
                        {
                            QRVDesignator q = new QRVDesignator(row);
                            l.Add(q);
                        }
                        return l;
                    }
                    return null;
                }
            }
            else
            {
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "SELECT * FROM " + QRVDesignator.TableName + " WHERE Call = @Call";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                    db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        List<QRVDesignator> l = new List<QRVDesignator>();
                        foreach (DataRow row in Result.Rows)
                        {
                            QRVDesignator q = new QRVDesignator(row);
                            l.Add(q);
                        }
                        return l;
                    }
                    return null;
                }
            }
        }

        public DateTime QRVFindlastUpdated(string call, string loc, BAND band)
        {
            QRVDesignator qrv = new QRVDesignator(call,loc, band);
            return QRVFindLastUpdated(qrv);
        }

        public DateTime QRVFindLastUpdated(QRVDesignator qrv)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + QRVDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc AND Band = @Band";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                db.DBCommand.Parameters.Add(qrv.AsString("Loc"));
                db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                DataTable result = db.Select(db.DBCommand);
                if (result != null && result.Rows.Count > 0)
                    return SQLiteEntry.UNIXTimeToDateTime(Convert.ToInt32(result.Rows[0][0]));
            }
            return DateTime.MinValue;
        }

        public int QRVInsert(QRVDesignator qrv)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + QRVDesignator.TableName + " (Call, Loc, Band, AntennaHeight, AntennaGain, Power, LastUpdated) VALUES (@Call, @Loc, @Band, @AntennaHeight, @AntennaGain, @Power, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                db.DBCommand.Parameters.Add(qrv.AsString("Loc"));
                db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                db.DBCommand.Parameters.Add(qrv.AsSingle("AntennaHeight"));
                db.DBCommand.Parameters.Add(qrv.AsSingle("AntennaGain"));
                db.DBCommand.Parameters.Add(qrv.AsSingle("Power"));
                db.DBCommand.Parameters.Add(qrv.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int QRVDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + QRVDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int QRVDelete(string call, string loc, BAND band)
        {
            QRVDesignator qrv = new QRVDesignator(call,loc, band);
            return QRVDelete(qrv);
        }

        public int QRVDelete(QRVDesignator qrv)
        {
            lock (db.DBCommand)
            {
                // Handle delete of all bands
                string bandwhere = "";
                if (qrv.Band != BAND.BALL)
                    bandwhere = " AND Band = @Band";
                else
                    bandwhere = "";
                db.DBCommand.CommandText = "DELETE FROM " + QRVDesignator.TableName + " WHERE Call = @Call AND Loc = @Loc" + bandwhere;
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                db.DBCommand.Parameters.Add(qrv.AsString("Loc"));
                db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int QRVUpdate(QRVDesignator qrv)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + QRVDesignator.TableName + " SET Call = @Call, Loc = @Loc, Band = @Band, AntennaHeight = @AntennaHeight, AntennaGain = @AntennaGain, Power = @Power, LastUpdated = @LastUpdated WHERE Call = @Call AND Loc = @Loc AND Band = @Band";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(qrv.AsString("Call"));
                db.DBCommand.Parameters.Add(qrv.AsString("Loc"));
                db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                db.DBCommand.Parameters.Add(qrv.AsSingle("AntennaHeight"));
                db.DBCommand.Parameters.Add(qrv.AsSingle("AntennaGain"));
                db.DBCommand.Parameters.Add(qrv.AsSingle("Power"));
                db.DBCommand.Parameters.Add(qrv.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int QRVBulkInsert(List<QRVDesignator> qrvs)
        {
            int errors = 0;
            try
            {
                db.BeginTransaction();
                foreach (QRVDesignator qrv in qrvs)
                {
                    try
                    {
                        QRVInsert(qrv);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage("Error inserting QRV [" + qrv.Call + "]: " + ex.ToString(), LogLevel.Error);
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

        public int QRVBulkDelete(List<QRVDesignator> qrvs)
        {
            int errors = 0;
            try
            {
                db.BeginTransaction();
                foreach (QRVDesignator qrv in qrvs)
                {
                    try
                    {
                        QRVDelete(qrv);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage("Error deleting QRV [" + qrv.Call + "]: " + ex.ToString(), LogLevel.Error);
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

        public int QRVBulkInsertOrUpdateIfNewer(List<QRVDesignator> qrvs)
        {
            if (qrvs == null)
                return 0;
            db.BeginTransaction();
            int i = 0;
            foreach (QRVDesignator qrv in qrvs)
            {
                QRVInsertOrUpdateIfNewer(qrv);
                i++;
            }
            db.Commit();
            return i;
        }

        public int QRVInsertOrUpdateIfNewer(QRVDesignator qrv)
        {
            DateTime dt = QRVFindLastUpdated(qrv);
            if (dt == DateTime.MinValue)
                return QRVInsert(qrv);
            if (dt < qrv.LastUpdated)
                return QRVUpdate(qrv);
            return 0;
        }

        public List<QRVDesignator> QRVGetAll()
        {
            DataTable Result = db.Select("SELECT * FROM " + QRVDesignator.TableName + " ORDER BY Call ASC");
            if ((Result != null) && (Result.Rows.Count > 0))
            {
                List<QRVDesignator> l = new List<QRVDesignator>();
                foreach (DataRow row in Result.Rows)
                {
                    QRVDesignator q = new QRVDesignator(row);
                    l.Add(q);
                }
                return l;
            }
            return null;
        }

        public List<QRVDesignator> QRVGetAll(BackgroundWorker caller)
        {
            // gets all entries from database
            // supports abort calculation if called from background worker and cancellation requested
            List<QRVDesignator> l = new List<QRVDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + QRVDesignator.TableName + " ORDER BY Call ASC";
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                QRVDesignator q = new QRVDesignator((IDataRecord)reader);
                l.Add(q);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<QRVDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting QRV " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public List<QRVDesignator> QRVGetAll(BAND band)
        {
            QRVDesignator qrv = new QRVDesignator();
            qrv.Band = band;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + QRVDesignator.TableName + " WHERE Band = @Band ORDER BY Call ASC";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(qrv.AsInt32("Band"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                {
                    List<QRVDesignator> l = new List<QRVDesignator>();
                    foreach (DataRow row in Result.Rows)
                    {
                        QRVDesignator q = new QRVDesignator(row);
                        l.Add(q);
                    }
                    return l;
                }
                return null;
            }
        }

        public List<QRVDesignator> QRVGetAll(BackgroundWorker caller, BAND band)
        {
            // gets all entries from database
            // supports abort calculation if called from background worker and cancellation requested
            List<QRVDesignator> l = new List<QRVDesignator>();
            QRVDesignator qrv = new QRVDesignator();
            qrv.Band = band;
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + QRVDesignator.TableName + " WHERE Band = @Band ORDER BY Call ASC";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(qrv.AsInt32("Band"));
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                QRVDesignator q = new QRVDesignator((IDataRecord)reader);
                l.Add(q);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<QRVDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting QRV " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public double QRVGetDefaultAntennaHeight(BAND band)
        {
            return 10.0;
        }

        public double QRVGetDefaultAntennaGain(BAND band)
        {
            switch (band)
            {
                case BAND.B50M:
                    return 3.0;
                case BAND.B70M:
                    return 6.0;
                case BAND.B144M:
                    return 12.0;
                case BAND.B432M:
                    return 15.0;
                case BAND.B1_2G:
                    return 18.0;
                case BAND.B2_3G:
                    return 20.0;
                case BAND.B3_4G:
                    return 22.0;
                case BAND.B5_7G:
                    return 25.0;
                case BAND.B10G:
                    return 30.0;
                case BAND.B24G:
                    return 40.0;
                case BAND.B47G:
                    return 42.0;
                case BAND.B76G:
                    return 45.0;
                default:
                    return 0.0;
            }
        }

        public double QRVGetDefaultPower(BAND band)
        {
            switch (band)
            {
                case BAND.B50M:
                    return 100.0;
                case BAND.B70M:
                    return 100.0;
                case BAND.B144M:
                    return 100.0;
                case BAND.B432M:
                    return 100.0;
                case BAND.B1_2G:
                    return 50.0;
                case BAND.B2_3G:
                    return 20.0;
                case BAND.B3_4G:
                    return 15.0;
                case BAND.B5_7G:
                    return 10.0;
                case BAND.B10G:
                    return 5.0;
                case BAND.B24G:
                    return 0.2;
                case BAND.B47G:
                    return 0.05;
                case BAND.B76G:
                    return 0.01;
                default:
                    return 0.0;
            }
        }

        public List<QRVDesignator> QRVFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<QRVDesignator>>(json, settings);
        }

        public string QRVToJSON()
        {
            List<QRVDesignator> l = QRVGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public string QRVToJSON(BAND band)
        {
            List<QRVDesignator> l = QRVGetAll(band);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        #endregion
    }

}
