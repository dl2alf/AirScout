using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using ScoutBase.Core;
using ScoutBase.Propagation;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.ComponentModel;
using AirScout.Core;

namespace AirScout.Aircrafts
{

    public class AircraftData
    {
        static AircraftDatabase aircrafts = new AircraftDatabase();
        public static AircraftDatabase Database
        {
            get
            {
                return aircrafts;
            }
        }

    }

    /// <summary>
    /// Holds the Aircraft information in a database structure.
    /// </summary>
    public class AircraftDatabase
    {
        System.Data.SQLite.SQLiteDatabase db;

        private string DBPath;

        private static LogWriter Log = LogWriter.Instance;

        public readonly int UserVersion = 1;

        public long AircraftRegistrationMinLength { get; private set; }
        public long AircraftRegistrationMaxLength { get; private set; }

        public long AircraftTypeIATAMinLength { get; private set; }
        public long AircraftTypeIATAMaxLength { get; private set; }

        public long AircraftTypeICAOMinLength { get; private set; }
        public long AircraftTypeICAOMaxLength { get; private set; }

        public AircraftDatabase()
        {
            db = OpenDatabase("aircrafts.db3");
            // set auto vacuum mode to "Full" to allow database to reduce size on disk
            // requires a vacuum command to change database layout
            AUTOVACUUMMODE mode = db.GetAutoVacuum();
            if (mode != AUTOVACUUMMODE.FULL)
            {
                if (MessageBox.Show("A major database layout change is necessary to run this version of AirScout. Older versions of AirScout are not compatible anymore and will cause errors. \n\nPress >OK< to start upgrade now (this will take some minutes). \nPress >Cancel< to leave.", "Database Upgrade of " + Path.GetFileName(db.DBLocation), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    Environment.Exit(-1);                 //  exit immediately
                db.SetAutoVacuum(AUTOVACUUMMODE.FULL);
            }
            // create tables with schemas if not exist
            if (!AircraftTableExists())
                AircraftCreateTable();
            if (!AircraftTypeTableExists())
                AircraftTypeCreateTable();
            if (!AirlineTableExists())
                AirlineCreateTable();
            if (!AirportTableExists())
                AirportCreateTable();
            if (!AircraftRegistrationTableExists())
                AircraftRegistrationCreateTable();
            // create views
            PlaneInfoCreateView();

            // get max/min lengths for AircraftRegistrations
            // due to performance reasons the maintenance is only performed at startup and during insertion (without database query)
            AircraftRegistrationMaxLength = AircraftRegistrationGetMaxLength();
            AircraftRegistrationMinLength = AircraftRegistrationGetMinLength();
            AircraftTypeIATAMaxLength = AircraftTypeIATAGetMaxLength();
            AircraftTypeIATAMinLength = AircraftTypeIATAGetMinLength();
            AircraftTypeICAOMaxLength = AircraftTypeICAOGetMaxLength();
            AircraftTypeICAOMinLength = AircraftTypeICAOGetMinLength();
        }

        ~AircraftDatabase()
        {
            CloseDatabase(db);
        }

        private System.Data.SQLite.SQLiteDatabase OpenDatabase(string name)
        {
            System.Data.SQLite.SQLiteDatabase db = null;
            try
            {
                // check if database path exists --> create if not
                if (!Directory.Exists(DefaultDatabaseDirectory()))
                    Directory.CreateDirectory(DefaultDatabaseDirectory());
                // check if database is already on disk 
                DBPath = DefaultDatabaseDirectory();
                if (!File.Exists(Path.Combine(DBPath, name)))
                {
                    // create one on disk
                    System.Data.SQLite.SQLiteDatabase dbn = new System.Data.SQLite.SQLiteDatabase(Path.Combine(DBPath, name));
                    // open database
                    dbn.Open();
                    // set user version
                    dbn.SetUserVerion(UserVersion);
                    // set auto vacuum mode to full
                    dbn.SetAutoVacuum(AUTOVACUUMMODE.FULL);
                    dbn.Close();
                }
                // check for in-memory database --> open from disk, if not
                if (Properties.Settings.Default.Database_InMemory)
                    db = System.Data.SQLite.SQLiteDatabase.CreateInMemoryDB(Path.Combine(DBPath, name));
                else
                {
                    db = new System.Data.SQLite.SQLiteDatabase(Path.Combine(DBPath, name));
                    db.Open();
                }
                // get version info
                int v = db.GetUserVersion();
                // do upgrade stuff here
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initilalizing database: " + ex.Message);
                throw new TypeInitializationException(this.GetType().Name, ex);
            }
            return db;
        }

        private void CloseDatabase(System.Data.SQLite.SQLiteDatabase db)
        {
            if (db == null)
                return;
            // save in-memory database to disk
            if (db.IsInMemory)
                db.BackupDatabase(db.DBLocation);
            //            else
            //                db.Close();
        }

        public void BackupDatabase()
        {
            if (db == null)
                return;
            // save in-memory database to disk
            if (db.IsInMemory)
                db.BackupDatabase(db.DiskFileName);
            else
                db.Close();
        }

        public bool IsInMemory()
        {
            return db.IsInMemory;
        }

        public string DefaultDatabaseDirectory()
        {
            // create default database directory name
            string dir = Properties.Settings.Default.Database_Directory;
            if (!String.IsNullOrEmpty(dir))
            {
                return dir;
            }
            // empty settings -_> create standard path
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
            dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!String.IsNullOrEmpty(company))
                dir = Path.Combine(dir, company);
            if (!String.IsNullOrEmpty(product))
                dir = Path.Combine(dir, product);
            return Path.Combine(dir, "AircraftData");
        }

        public DATABASESTATUS GetDBStatus()
        {
            if (db != null)
                return db.Status;
            return DATABASESTATUS.UNDEFINED;
        }

        public void SetDBStatus(DATABASESTATUS status)
        {
            if (db != null)
                db.Status = status;
        }

        public bool GetDBStatusBit(DATABASESTATUS statusbit)
        {
            if (db != null)
                return (((int)db.Status) & ((int)statusbit)) > 0;
            return false;
        }

        public void SetDBStatusBit(DATABASESTATUS statusbit)
        {
            if (db != null)
                db.Status |= statusbit;
        }

        public void ResetDBStatusBit(DATABASESTATUS statusbit)
        {
            if (db != null)
                db.Status &= ~statusbit;
        }

        public void BeginTransaction()
        {
            if (db == null)
                return;
            db.BeginTransaction();
        }

        public void Commit()
        {
            if (db == null)
                return;
            db.Commit();
        }

        private DataTable Select(System.Data.SQLite.SQLiteDatabase db, string sql)
        {
            return db.Select(sql);
        }

        public string GetDBLocation()
        {
            if (db == null)
                return "";
            return db.DBLocation;
        }

        public double GetDBSize()
        {
            if (db == null)
                return 0;
            return db.DBSize;
        }

        private bool IsValid(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() == typeof(DBNull))
                return false;
            return true;
        }

        #region Aircrafts

        public bool AircraftTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = AircraftDesignator.TableName;
            return db.TableExists(tn);
        }

        public void AircraftCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = AircraftDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE `" + tn + "`(Hex TEXT UNIQUE NOT NULL DEFAULT '', Call TEXT NOT NULL DEFAULT '', Reg TEXT NOT NULL DEFAULT '', TypeCode TEXT, LastUpdated INT32, PRIMARY KEY (Hex))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                // create table indices
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_Reg ON `" + tn + "` (Reg)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                // create table indices
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_Call ON `" + tn + "` (Call)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long AircraftCount()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AircraftDesignator.TableName);
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public long AircraftCountUnknownCall()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AircraftDesignator.TableName + " WHERE Call = '[unknown]'");
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public long AircraftCountUnknownHex()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AircraftDesignator.TableName + " WHERE Hex = '[unknown]'");
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public long AircraftCountUnknownType()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AircraftDesignator.TableName + " WHERE TypeCode = '[unknown]'");
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public bool AircraftExists(string hex)
        {
            AircraftDesignator ad = new AircraftDesignator(hex);
            return AircraftExists(ad);
        }

        public bool AircraftExists(AircraftDesignator ad)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + AircraftDesignator.TableName + " WHERE Hex = @Hex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ad.AsString("Hex"));
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result) && ((long)result > 0))
                    return true;
            }
            return false;
        }

        public AircraftDesignator AircraftFindByHex(string hex)
        {
            AircraftDesignator ad = new AircraftDesignator(hex);
            return AircraftFind(ad);
        }

        public AircraftDesignator AircraftFindByReg(string reg)
        {
            // returs entry by search string, latest entry if more than one entry found
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftDesignator.TableName + " WHERE Reg = '" + reg + "' ORDER BY Lastupdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AircraftDesignator AircraftFindByCall(string call)
        {
            // returs entry by search string, latest entry if more than one entry found
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftDesignator.TableName + " WHERE Call = '" + call + "' ORDER BY Lastupdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AircraftDesignator AircraftFind(AircraftDesignator ad)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftDesignator.TableName + " WHERE Hex = @Hex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ad.AsString("Hex"));
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AircraftDesignator AircraftFindAt(long index)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftDesignator.TableName + " LIMIT 1 OFFSET " + index.ToString();
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public DateTime AircraftFindlastUpdated(string hex)
        {
            AircraftDesignator ad = new AircraftDesignator(hex);
            return AircraftFindLastUpdated(ad);
        }

        public DateTime AircraftFindLastUpdated(AircraftDesignator ad)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + AircraftDesignator.TableName + " WHERE Hex = @Hex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ad.AsString("Hex"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (IsValid(result))
                    return (SQLiteEntry.UNIXTimeToDateTime((int)result));
            }
            return DateTime.MinValue;
        }

        public int AircraftInsert(AircraftDesignator ad)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + AircraftDesignator.TableName + " (Hex, Call, Reg, TypeCode, LastUpdated) VALUES (@Hex, @Call, @Reg, @TypeCode, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ad.AsString("Hex"));
                db.DBCommand.Parameters.Add(ad.AsString("Call"));
                db.DBCommand.Parameters.Add(ad.AsString("Reg"));
                db.DBCommand.Parameters.Add(ad.AsString("TypeCode"));
                db.DBCommand.Parameters.Add(ad.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftDelete(string hex)
        {
            AircraftDesignator ad = new AircraftDesignator(hex);
            return AircraftDelete(ad);
        }

        public int AircraftDelete(AircraftDesignator ad)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftDesignator.TableName + " WHERE Hex = @Hex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ad.AsString("Hex"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftUpdate(AircraftDesignator ad)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + AircraftDesignator.TableName + " SET Hex = @Hex, Call = @Call, Reg = @Reg, TypeCode = @TypeCode, LastUpdated = @LastUpdated WHERE Hex = @Hex";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ad.AsString("Hex"));
                db.DBCommand.Parameters.Add(ad.AsString("Call"));
                db.DBCommand.Parameters.Add(ad.AsString("Reg"));
                db.DBCommand.Parameters.Add(ad.AsString("TypeCode"));
                db.DBCommand.Parameters.Add(ad.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftBulkInsert(List<AircraftDesignator> ads)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftDesignator ad in ads)
                    {
                        try
                        {
                            AircraftInsert(ad);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting aircraft [" + ad.Hex + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AircraftBulkDelete(List<AircraftDesignator> ads)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftDesignator ad in ads)
                    {
                        try
                        {
                            AircraftDelete(ad);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error deleting aircraft [" + ad.Hex + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AircraftBulkInsertOrUpdateIfNewer(List<AircraftDesignator> ads)
        {
            if (ads == null)
                return 0;
            int i = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftDesignator ad in ads)
                    {
                        try
                        {
                            AircraftInsertOrUpdateIfNewer(ad);
                            i++;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    db.Commit();
                }
            }
            catch(Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return i;
        }

        public int AircraftInsertOrUpdateIfNewer(AircraftDesignator ad)
        {
            DateTime dt = AircraftFindLastUpdated(ad);
            if (dt == DateTime.MinValue)
                return AircraftInsert(ad);
            if (dt < ad.LastUpdated)
                return AircraftUpdate(ad);
            return 0;
        }

        public List<AircraftDesignator> AircraftGetAll()
        {
            List<AircraftDesignator> l = new List<AircraftDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftDesignator.TableName);
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftDesignator(row));
            return l;

        }

        public List<AircraftDesignator> AircraftGetAll(BackgroundWorker caller)
        {
            // gets all aircrafts from database
            // supports abort calculation if called from background worker and cancellation requested
            List<AircraftDesignator> l = new List<AircraftDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + AircraftDesignator.TableName;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AircraftDesignator ap = new AircraftDesignator((IDataRecord)reader);
                l.Add(ap);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<AircraftDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting aircraft " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public List<AircraftDesignator> AircraftFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<AircraftDesignator>>(json, settings);
        }

        public string AircraftToJSON()
        {
            List<AircraftDesignator> l = AircraftGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public DataTableAircrafts AircraftToDataTable()
        {
            List<AircraftDesignator> ads = AircraftGetAll();
            DataTableAircrafts dtl = new DataTableAircrafts(ads);
            return dtl;
        }

        #endregion

        #region AircraftTypes

        public bool AircraftTypeTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = AircraftTypeDesignator.TableName;
            return db.TableExists(tn);
        }

        public void AircraftTypeCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = AircraftTypeDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE `" + tn + "`(ICAO TEXT NOT NULL DEFAULT '', IATA TEXT NOT NULL DEFAULT '', Manufacturer TEXT, Model TEXT, Category INT32, LastUpdated INT32, PRIMARY KEY (ICAO, IATA))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                // create table indices
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_ICAO ON `" + tn + "` (ICAO)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_IATA ON `" + tn + "` (IATA)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long AircraftTypeCount()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AircraftTypeDesignator.TableName);
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public bool AircraftTypeExists(string icao, string iata)
        {
            AircraftTypeDesignator td = new AircraftTypeDesignator(icao, iata);
            return AircraftTypeExists(td);
        }

        public bool AircraftTypeExists(AircraftTypeDesignator td)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + AircraftTypeDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(td.AsString("ICAO"));
                db.DBCommand.Parameters.Add(td.AsString("IATA"));
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result) && ((long)result > 0))
                    return true;
            }
            return false;
        }

        private long AircraftTypeIATAGetMaxLength()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT max(length(IATA)) FROM " + AircraftTypeDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                    return (long)result;
            }
            return 0;
        }

        private long AircraftTypeIATAGetMinLength()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT min(length(IATA)) FROM " + AircraftTypeDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                    return (long)result;
            }
            return 0;
        }

        private long AircraftTypeICAOGetMaxLength()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT max(length(ICAO)) FROM " + AircraftTypeDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                    return (long)result;
            }
            return 0;
        }

        private long AircraftTypeICAOGetMinLength()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT min(length(ICAO)) FROM " + AircraftTypeDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                    return (long)result;
            }
            return 0;
        }


        public AircraftTypeDesignator AircraftTypeFindByIATA(string iata)
        {
            // returs entry by search string, latest entry if more than one entry found
            if (String.IsNullOrEmpty(iata))
                return null;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftTypeDesignator.TableName + " WHERE IATA = '" + iata + "' ORDER BY Lastupdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftTypeDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AircraftTypeDesignator AircraftTypeFindByICAO(string icao)
        {
            // returs entry by search string, latest entry if more than one entry found
            if (String.IsNullOrEmpty(icao))
                return null;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftTypeDesignator.TableName + " WHERE ICAO = '" + icao + "' ORDER BY Lastupdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftTypeDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AircraftTypeDesignator AircraftTypeFind(string icao, string iata)
        {
            AircraftTypeDesignator td = new AircraftTypeDesignator(icao, iata);
            return AircraftTypeFind(td);
        }

        public AircraftTypeDesignator AircraftTypeFind(AircraftTypeDesignator td)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftTypeDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(td.AsString("ICAO"));
                db.DBCommand.Parameters.Add(td.AsString("IATA"));
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftTypeDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AircraftTypeDesignator AircraftTypeFindAt(long index)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftTypeDesignator.TableName + " LIMIT 1 OFFSET " + index.ToString();
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftTypeDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public DateTime AircraftTypeFindlastUpdated(string icao, string iata)
        {
            AircraftTypeDesignator td = new AircraftTypeDesignator(icao, iata);
            return AircraftTypeFindLastUpdated(td);
        }

        public DateTime AircraftTypeFindLastUpdated(AircraftTypeDesignator td)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + AircraftTypeDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(td.AsString("ICAO"));
                db.DBCommand.Parameters.Add(td.AsString("IATA"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (IsValid(result))
                    return (SQLiteEntry.UNIXTimeToDateTime((int)result));
            }
            return DateTime.MinValue;
        }

        public int AircraftTypeInsert(AircraftTypeDesignator td)
        {
            // maintain min/max values
            if (td.IATA.Length < AircraftTypeIATAMinLength)
                AircraftTypeIATAMinLength = td.IATA.Length;
            if (td.IATA.Length > AircraftTypeIATAMaxLength)
                AircraftTypeIATAMaxLength = td.IATA.Length;
            if (td.ICAO.Length < AircraftTypeICAOMinLength)
                AircraftTypeICAOMinLength = td.ICAO.Length;
            if (td.ICAO.Length > AircraftTypeICAOMaxLength)
                AircraftTypeICAOMaxLength = td.ICAO.Length;

            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + AircraftTypeDesignator.TableName + " (ICAO, IATA, Manufacturer, Model, Category, LastUpdated) VALUES (@ICAO, @IATA, @Manufacturer, @Model, @Category, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(td.AsString("ICAO"));
                db.DBCommand.Parameters.Add(td.AsString("IATA"));
                db.DBCommand.Parameters.Add(td.AsString("Manufacturer"));
                db.DBCommand.Parameters.Add(td.AsString("Model"));
                db.DBCommand.Parameters.Add(td.AsInt32("Category"));
                db.DBCommand.Parameters.Add(td.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftTypeDelete(string icao, string iata)
        {
            AircraftTypeDesignator td = new AircraftTypeDesignator(iata, icao);
            return AircraftTypeDelete(td);
        }

        public int AircraftTypeDelete(AircraftTypeDesignator td)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftTypeDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(td.AsString("ICAO"));
                db.DBCommand.Parameters.Add(td.AsString("IATA"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftTypeDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftTypeDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftTypeUpdate(AircraftTypeDesignator ad)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + AircraftTypeDesignator.TableName + " SET ICAO = @ICAO, IATA = @IATA, Manufacturer = @Manufacturer, Model = @Model, Category = @Category, LastUpdated = @LastUpdated WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ad.AsString("ICAO"));
                db.DBCommand.Parameters.Add(ad.AsString("IATA"));
                db.DBCommand.Parameters.Add(ad.AsString("Manufacturer"));
                db.DBCommand.Parameters.Add(ad.AsString("Model"));
                db.DBCommand.Parameters.Add(ad.AsInt32("Category"));
                db.DBCommand.Parameters.Add(ad.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftTypeBulkInsert(List<AircraftTypeDesignator> tds)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftTypeDesignator td in tds)
                    {
                        try
                        {
                            AircraftTypeInsert(td);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting AircraftType [" + td.ICAO + ", " + td.IATA + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AircraftTypeBulkDelete(List<AircraftTypeDesignator> ads)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftTypeDesignator td in ads)
                    {
                        try
                        {
                            AircraftTypeDelete(td);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting AircraftType [" + td.ICAO + ", " + td.IATA + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AircraftTypeBulkInsertOrUpdateIfNewer(List<AircraftTypeDesignator> ads)
        {
            if (ads == null)
                return 0;
            int i = 0;
            lock (db)
            {
                db.BeginTransaction();
                foreach (AircraftTypeDesignator ad in ads)
                {
                    AircraftTypeInsertOrUpdateIfNewer(ad);
                    i++;
                }
                db.Commit();
            }
            return i;
        }

        public int AircraftTypeInsertOrUpdateIfNewer(AircraftTypeDesignator ad)
        {
            DateTime dt = AircraftTypeFindLastUpdated(ad);
            if (dt == DateTime.MinValue)
                return AircraftTypeInsert(ad);
            if (dt < ad.LastUpdated)
                return AircraftTypeUpdate(ad);
            return 0;
        }

        public List<AircraftTypeDesignator> AircraftTypeGetAll()
        {
            List<AircraftTypeDesignator> l = new List<AircraftTypeDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftTypeDesignator.TableName);
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftTypeDesignator(row));
            return l;

        }

        public List<AircraftTypeDesignator> AircraftTypeGetAll(BackgroundWorker caller)
        {
            // gets all Aircraft types from database
            // supports abort calculation if called from background worker and cancellation requested
            List<AircraftTypeDesignator> l = new List<AircraftTypeDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + AircraftTypeDesignator.TableName;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AircraftTypeDesignator ap = new AircraftTypeDesignator((IDataRecord)reader);
                l.Add(ap);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<AircraftTypeDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting Aircraft type " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public List<AircraftTypeDesignator> AircraftTypeFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<AircraftTypeDesignator>>(json, settings);
        }

        public string AircraftTypeToJSON()
        {
            List<AircraftTypeDesignator> l = AircraftTypeGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public DataTableAircraftTypes AircraftTypeToDataTable()
        {
            List<AircraftTypeDesignator> ads = AircraftTypeGetAll();
            DataTableAircraftTypes dtl = new DataTableAircraftTypes(ads);
            return dtl;
        }

        #endregion

        #region Airlines

        public bool AirlineTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = AirlineDesignator.TableName;
            return db.TableExists(tn);
        }

        public void AirlineCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = AirlineDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE `" + tn + "`(ICAO TEXT NOT NULL DEFAULT '', IATA TEXT NOT NULL DEFAULT '', Airline TEXT, Country TEXT, LastUpdated INT32, PRIMARY KEY (ICAO, IATA))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                // create table indices
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_ICAO ON `" + tn + "` (ICAO)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_IATA ON `" + tn + "` (IATA)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long AirlineCount()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AirlineDesignator.TableName);
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public bool AirlineExists(string icao, string iata)
        {
            AirlineDesignator ld = new AirlineDesignator(icao, iata);
            return AirlineExists(ld);
        }

        public bool AirlineExists(AirlineDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + AirlineDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("ICAO"));
                db.DBCommand.Parameters.Add(ld.AsString("IATA"));
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result) && ((long)result > 0))
                    return true;
            }
            return false;
        }

        public AirlineDesignator AirlineFindByICAO(string icao)
        {
            // returs entry by search string, latest entry if more than one entry found
            if (String.IsNullOrEmpty(icao))
                return null;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirlineDesignator.TableName + " WHERE ICAO = '" + icao + "' ORDER BY LastUpdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirlineDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AirlineDesignator AirlineFindByIATA(string iata)
        {
            // returs entry by search string, latest entry if more than one entry found
            if (String.IsNullOrEmpty(iata))
                return null;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirlineDesignator.TableName + " WHERE IATA = '" + iata + "' ORDER BY LastUpdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirlineDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AirlineDesignator AirlineFind(string icao, string iata)
        {
            AirlineDesignator ld = new AirlineDesignator(icao, iata);
            return AirlineFind(ld);
        }

        public AirlineDesignator AirlineFind(AirlineDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirlineDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("ICAO"));
                db.DBCommand.Parameters.Add(ld.AsString("IATA"));
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirlineDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AirlineDesignator AirlineFindAt(long index)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirlineDesignator.TableName + " LIMIT 1 OFFSET " + index.ToString();
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirlineDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public DateTime AirlineFindlastUpdated(string icao, string iata)
        {
            AirlineDesignator ld = new AirlineDesignator(icao, iata);
            return AirlineFindLastUpdated(ld);
        }

        public DateTime AirlineFindLastUpdated(AirlineDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + AirlineDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("ICAO"));
                db.DBCommand.Parameters.Add(ld.AsString("IATA"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (IsValid(result))
                    return (SQLiteEntry.UNIXTimeToDateTime((int)result));
            }
            return DateTime.MinValue;
        }

        public int AirlineInsert(AirlineDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + AirlineDesignator.TableName + " (ICAO, IATA, Airline, Country, LastUpdated) VALUES (@ICAO, @IATA, @Airline, @Country, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("ICAO"));
                db.DBCommand.Parameters.Add(ld.AsString("IATA"));
                db.DBCommand.Parameters.Add(ld.AsString("Airline"));
                db.DBCommand.Parameters.Add(ld.AsString("Country"));
                db.DBCommand.Parameters.Add(ld.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirlineDelete(string icao, string iata)
        {
            AirlineDesignator ld = new AirlineDesignator(iata, icao);
            return AirlineDelete(ld);
        }

        public int AirlineDelete(AirlineDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AirlineDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("ICAO"));
                db.DBCommand.Parameters.Add(ld.AsString("IATA"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirlineDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AirlineDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirlineUpdate(AirlineDesignator ld)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + AirlineDesignator.TableName + " SET ICAO = @ICAO, IATA = @IATA, Airline = @Airline, Country = @Country, LastUpdated = @LastUpdated WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ld.AsString("ICAO"));
                db.DBCommand.Parameters.Add(ld.AsString("IATA"));
                db.DBCommand.Parameters.Add(ld.AsString("Airline"));
                db.DBCommand.Parameters.Add(ld.AsString("Country"));
                db.DBCommand.Parameters.Add(ld.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirlineBulkInsert(List<AirlineDesignator> lds)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AirlineDesignator ld in lds)
                    {
                        try
                        {
                            AirlineInsert(ld);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting Airline [" + ld.ICAO + ", " + ld.IATA + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AirlineBulkDelete(List<AirlineDesignator> lds)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AirlineDesignator ld in lds)
                    {
                        try
                        {
                            AirlineDelete(ld);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting Airline [" + ld.ICAO + ", " + ld.IATA + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AirlineBulkInsertOrUpdateIfNewer(List<AirlineDesignator> lds)
        {
            if (lds == null)
                return 0;
            int i = 0;
            lock (db)
            { 
                db.BeginTransaction();
                foreach (AirlineDesignator ld in lds)
                {
                    AirlineInsertOrUpdateIfNewer(ld);
                    i++;
                }
                db.Commit();
            }
            return i;
        }

        public int AirlineInsertOrUpdateIfNewer(AirlineDesignator ld)
        {
            DateTime dt = AirlineFindLastUpdated(ld);
            if (dt == DateTime.MinValue)
                return AirlineInsert(ld);
            if (dt < ld.LastUpdated)
                return AirlineUpdate(ld);
            return 0;
        }

        public List<AirlineDesignator> AirlineGetAll()
        {
            List<AirlineDesignator> l = new List<AirlineDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AirlineDesignator.TableName);
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AirlineDesignator(row));
            return l;

        }

        public List<AirlineDesignator> AirlineGetAll(BackgroundWorker caller)
        {
            // gets all Airlines from database
            // supports abort calculation if called from background worker and cancellation requested
            List<AirlineDesignator> l = new List<AirlineDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + AirlineDesignator.TableName;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AirlineDesignator ap = new AirlineDesignator((IDataRecord)reader);
                l.Add(ap);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<AirlineDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting Airline " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public List<AirlineDesignator> AirlineFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<AirlineDesignator>>(json, settings);
        }

        public string AirlineToJSON()
        {
            List<AirlineDesignator> l = AirlineGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public DataTableAirlines AirlineToDataTable()
        {
            List<AirlineDesignator> lds = AirlineGetAll();
            DataTableAirlines dtl = new DataTableAirlines(lds);
            return dtl;
        }

        #endregion

        #region Airports

        public bool AirportTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = AirportDesignator.TableName;
            return db.TableExists(tn);
        }

        public void AirportCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = AirportDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE `" + tn + "`(ICAO TEXT NOT NULL DEFAULT '', IATA TEXT NOT NULL DEFAULT '', Lat DOUBLE, Lon DOUBLE, Alt DOUBLE, Airport TEXT, Country TEXT, LastUpdated INT32, PRIMARY KEY (ICAO, IATA))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                // create table indices
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_ICAO ON `" + tn + "` (ICAO)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_IATA ON `" + tn + "` (IATA)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long AirportCount()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AirportDesignator.TableName);
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public bool AirportExists(string icao, string iata)
        {
            AirportDesignator pd = new AirportDesignator(icao, iata);
            return AirportExists(pd);
        }

        public bool AirportExists(AirportDesignator pd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + AirportDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(pd.AsString("ICAO"));
                db.DBCommand.Parameters.Add(pd.AsString("IATA"));
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result) && ((long)result > 0))
                    return true;
            }
            return false;
        }

        public AirportDesignator AirportFindByICAO(string icao)
        {
            // returs entry by search string, latest entry if more than one entry found
            if (String.IsNullOrEmpty(icao))
                return null;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirportDesignator.TableName + " WHERE ICAO = '" + icao + "' ORDER BY LastUpdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirportDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AirportDesignator AirportFindByIATA(string iata)
        {
            // returs entry by search string, latest entry if more than one entry found
            if (String.IsNullOrEmpty(iata))
                return null;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirportDesignator.TableName + " WHERE IATA = '" + iata + "' ORDER BY LastUpdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirportDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AirportDesignator AirportFind(string icao, string iata)
        {
            AirportDesignator pd = new AirportDesignator(icao, iata);
            return AirportFind(pd);
        }

        public AirportDesignator AirportFind(AirportDesignator pd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirportDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(pd.AsString("ICAO"));
                db.DBCommand.Parameters.Add(pd.AsString("IATA"));
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirportDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AirportDesignator AirportFindAt(long index)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AirportDesignator.TableName + " LIMIT 1 OFFSET " + index.ToString();
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AirportDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public DateTime AirportFindlastUpdated(string icao, string iata)
        {
            AirportDesignator pd = new AirportDesignator(icao, iata);
            return AirportFindLastUpdated(pd);
        }

        public DateTime AirportFindLastUpdated(AirportDesignator pd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + AirportDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(pd.AsString("ICAO"));
                db.DBCommand.Parameters.Add(pd.AsString("IATA"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (IsValid(result))
                    return (SQLiteEntry.UNIXTimeToDateTime((int)result));
            }
            return DateTime.MinValue;
        }

        public int AirportInsert(AirportDesignator pd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + AirportDesignator.TableName + " (ICAO, IATA, Lat, Lon, Alt, Airport, Country, LastUpdated) VALUES (@ICAO, @IATA, @Lat, @Lon, @Alt, @Airport, @Country, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(pd.AsString("ICAO"));
                db.DBCommand.Parameters.Add(pd.AsString("IATA"));
                db.DBCommand.Parameters.Add(pd.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(pd.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(pd.AsDouble("Alt"));
                db.DBCommand.Parameters.Add(pd.AsString("Airport"));
                db.DBCommand.Parameters.Add(pd.AsString("Country"));
                db.DBCommand.Parameters.Add(pd.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirportDelete(string icao, string iata)
        {
            AirportDesignator pd = new AirportDesignator(iata, icao);
            return AirportDelete(pd);
        }

        public int AirportDelete(AirportDesignator pd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AirportDesignator.TableName + " WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(pd.AsString("ICAO"));
                db.DBCommand.Parameters.Add(pd.AsString("IATA"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirportDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AirportDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirportUpdate(AirportDesignator pd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + AirportDesignator.TableName + " SET ICAO = @ICAO, IATA = @IATA, Lat = @Lat, Lon = @Lon, Alt = @Alt, Airport = @Airport, Country = @Country, LastUpdated = @LastUpdated WHERE ICAO = @ICAO AND IATA = @IATA";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(pd.AsString("ICAO"));
                db.DBCommand.Parameters.Add(pd.AsString("IATA"));
                db.DBCommand.Parameters.Add(pd.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(pd.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(pd.AsDouble("Alt"));
                db.DBCommand.Parameters.Add(pd.AsString("Airport"));
                db.DBCommand.Parameters.Add(pd.AsString("Country"));
                db.DBCommand.Parameters.Add(pd.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AirportBulkInsert(List<AirportDesignator> pds)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AirportDesignator pd in pds)
                    {
                        try
                        {
                            AirportInsert(pd);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting Airport [" + pd.ICAO + ", " + pd.IATA + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AirportBulkDelete(List<AirportDesignator> pds)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AirportDesignator pd in pds)
                    {
                        try
                        {
                            AirportDelete(pd);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting Airport [" + pd.ICAO + ", " + pd.IATA + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AirportBulkInsertOrUpdateIfNewer(List<AirportDesignator> pds)
        {
            if (pds == null)
                return 0;
            int i = 0;
            lock (db)
            {
                db.BeginTransaction();
                foreach (AirportDesignator pd in pds)
                {
                    AirportInsertOrUpdateIfNewer(pd);
                    i++;
                }
                db.Commit();
            }
            return i;
        }

        public int AirportInsertOrUpdateIfNewer(AirportDesignator pd)
        {
            DateTime dt = AirportFindLastUpdated(pd);
            if (dt == DateTime.MinValue)
                return AirportInsert(pd);
            if (dt < pd.LastUpdated)
                return AirportUpdate(pd);
            return 0;
        }

        public List<AirportDesignator> AirportGetAll()
        {
            List<AirportDesignator> l = new List<AirportDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AirportDesignator.TableName);
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AirportDesignator(row));
            return l;

        }

        public List<AirportDesignator> AirportGetAll(BackgroundWorker caller)
        {
            // gets all Airports from database
            // supports abort calculation if called from background worker and cancellation requested
            List<AirportDesignator> l = new List<AirportDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + AirportDesignator.TableName;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AirportDesignator ap = new AirportDesignator((IDataRecord)reader);
                l.Add(ap);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<AirportDesignator>();
                    if (caller.WorkerSupportsCancellation && caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting Airport " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public List<AirportDesignator> AirportFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<AirportDesignator>>(json, settings);
        }

        public string AirportToJSON()
        {
            List<AirportDesignator> l = AirportGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public DataTableAirports AirportToDataTable()
        {
            List<AirportDesignator> lds = AirportGetAll();
            DataTableAirports dtl = new DataTableAirports(lds);
            return dtl;
        }

        #endregion

        #region AircraftRegistrations

        public bool AircraftRegistrationTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = AircraftRegistrationDesignator.TableName;
            return db.TableExists(tn);
        }

        public void AircraftRegistrationCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = AircraftRegistrationDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE `" + tn + "`(Prefix TEXT NOT NULL DEFAULT '', Country TEXT, Remarks TEXT, LastUpdated INT32, PRIMARY KEY (Prefix))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long AircraftRegistrationCount()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AircraftRegistrationDesignator.TableName);
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public bool AircraftRegistrationExists(string prefix)
        {
            AircraftRegistrationDesignator rd = new AircraftRegistrationDesignator(prefix);
            return AircraftRegistrationExists(rd);
        }

        public bool AircraftRegistrationExists(AircraftRegistrationDesignator rd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + AircraftRegistrationDesignator.TableName + " WHERE Prefix = @Prefix";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(rd.AsString("Prefix"));
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result) && ((long)result > 0))
                    return true;
            }
            return false;
        }

        private long AircraftRegistrationGetMaxLength()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT max(length(Prefix)) FROM " + AircraftRegistrationDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                    return (long)result;
            }
            return 0;
        }

        private long AircraftRegistrationGetMinLength()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT min(length(Prefix)) FROM " + AircraftRegistrationDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                    return (long)result;
            }
            return 0;
        }

        public AircraftRegistrationDesignator AircraftRegistrationFindByReg(string reg)
        {
            if (String.IsNullOrEmpty(reg))
                return null;
            reg = reg.ToUpper().Trim();
            // check for US registration --> insert '-' after first letter
            if (reg.StartsWith("N"))
            {
                if (Char.IsDigit(reg[1]))
                    reg = reg[0] + "-" + reg.Substring(1);
                else
                    return null;
            }
            // return null if not a registration
            if (reg.IndexOf("-") == 0)
                return null;
            // stop search at char before '-' should be clean prefix then
            int stop = reg.IndexOf("-") - 1;
            // start search two chars after '-'
            int i = stop + 2;
            while (i >= stop)
            {
                AircraftRegistrationDesignator rd = AircraftRegistrationFind(reg.Substring(0, i));
                if (rd != null)
                    return rd;
                i--;
            }
            return null;
        }
        public AircraftRegistrationDesignator AircraftRegistrationFind(string prefix)
        {
            AircraftRegistrationDesignator rd = new AircraftRegistrationDesignator(prefix);
            return AircraftRegistrationFind(rd);
        }

        public AircraftRegistrationDesignator AircraftRegistrationFind(AircraftRegistrationDesignator rd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftRegistrationDesignator.TableName + " WHERE Prefix = @Prefix";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(rd.AsString("Prefix"));
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftRegistrationDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public AircraftRegistrationDesignator AircraftRegistrationFindAt(long index)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftRegistrationDesignator.TableName + " LIMIT 1 OFFSET " + index.ToString();
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftRegistrationDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
            return null;
        }

        public DateTime AircraftRegistrationFindlastUpdated(string prefix)
        {
            AircraftRegistrationDesignator rd = new AircraftRegistrationDesignator(prefix);
            return AircraftRegistrationFindLastUpdated(rd);
        }

        public DateTime AircraftRegistrationFindLastUpdated(AircraftRegistrationDesignator rd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + AircraftRegistrationDesignator.TableName + " WHERE Prefix = @Prefix";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(rd.AsString("Prefix"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (IsValid(result))
                    return (SQLiteEntry.UNIXTimeToDateTime((int)result));
            }
            return DateTime.MinValue;
        }

        public int AircraftRegistrationInsert(AircraftRegistrationDesignator rd)
        {
            // maintain max/min lengths
            if (rd.Prefix.Length < AircraftRegistrationMinLength)
                AircraftRegistrationMinLength = rd.Prefix.Length;
            if (rd.Prefix.Length > AircraftRegistrationMaxLength)
                AircraftRegistrationMaxLength = rd.Prefix.Length;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + AircraftRegistrationDesignator.TableName + " (Prefix, Country, Remarks, LastUpdated) VALUES (@Prefix, @Country, @Remarks, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(rd.AsString("Prefix"));
                db.DBCommand.Parameters.Add(rd.AsString("Country"));
                db.DBCommand.Parameters.Add(rd.AsString("Remarks"));
                db.DBCommand.Parameters.Add(rd.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftRegistrationDelete(string prefix)
        {
            AircraftRegistrationDesignator rd = new AircraftRegistrationDesignator(prefix);
            return AircraftRegistrationDelete(rd);
        }

        public int AircraftRegistrationDelete(AircraftRegistrationDesignator rd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftRegistrationDesignator.TableName + " WHERE Prefix = @Prefix";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(rd.AsString("Prefix"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftRegistrationDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftRegistrationDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftRegistrationUpdate(AircraftRegistrationDesignator rd)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + AircraftRegistrationDesignator.TableName + " SET Prefix = @Prefix, Country = @Country, Remarks = @Remarks, LastUpdated = @LastUpdated WHERE Prefix = @Prefix";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(rd.AsString("Prefix"));
                db.DBCommand.Parameters.Add(rd.AsString("Country"));
                db.DBCommand.Parameters.Add(rd.AsString("Remarks"));
                db.DBCommand.Parameters.Add(rd.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftRegistrationBulkInsert(List<AircraftRegistrationDesignator> rds)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftRegistrationDesignator rd in rds)
                    {
                        try
                        {
                            AircraftRegistrationInsert(rd);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting AircraftRegistration [" + rd.Prefix + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AircraftRegistrationBulkDelete(List<AircraftRegistrationDesignator> rds)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftRegistrationDesignator rd in rds)
                    {
                        try
                        {
                            AircraftRegistrationDelete(rd);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error deleting AircraftRegistration [" + rd.Prefix + "]: " + ex.ToString());
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return -errors;
        }

        public int AircraftRegistrationBulkInsertOrUpdateIfNewer(List<AircraftRegistrationDesignator> rds)
        {
            if (rds == null)
                return 0;
            int i = 0;
            lock (db)
            {
                db.BeginTransaction();
                foreach (AircraftRegistrationDesignator rd in rds)
                {
                    AircraftRegistrationInsertOrUpdateIfNewer(rd);
                    i++;
                }
                db.Commit();
            }
            return i;
        }

        public int AircraftRegistrationInsertOrUpdateIfNewer(AircraftRegistrationDesignator rd)
        {
            DateTime dt = AircraftRegistrationFindLastUpdated(rd);
            if (dt == DateTime.MinValue)
                return AircraftRegistrationInsert(rd);
            if (dt < rd.LastUpdated)
                return AircraftRegistrationUpdate(rd);
            return 0;
        }

        public List<AircraftRegistrationDesignator> AircraftRegistrationGetAll()
        {
            List<AircraftRegistrationDesignator> l = new List<AircraftRegistrationDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftRegistrationDesignator.TableName);
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftRegistrationDesignator(row));
            return l;

        }

        public List<AircraftRegistrationDesignator> AircraftRegistrationGetAll(BackgroundWorker caller)
        {
            // gets all AircraftRegistrations from database
            // supports abort calculation if called from background worker and cancellation requested
            List<AircraftRegistrationDesignator> l = new List<AircraftRegistrationDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + AircraftRegistrationDesignator.TableName;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AircraftRegistrationDesignator ap = new AircraftRegistrationDesignator((IDataRecord)reader);
                l.Add(ap);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<AircraftRegistrationDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting AircraftRegistration " + i.ToString() + " of");
                }
            }
            reader.Close();
            return l;
        }

        public List<AircraftRegistrationDesignator> AircraftRegistrationFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<AircraftRegistrationDesignator>>(json, settings);
        }

        public string AircraftRegistrationToJSON()
        {
            List<AircraftRegistrationDesignator> l = AircraftRegistrationGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public DataTableAircraftRegistrations AircraftRegistrationToDataTable()
        {
            List<AircraftRegistrationDesignator> rds = AircraftRegistrationGetAll();
            DataTableAircraftRegistrations dtl = new DataTableAircraftRegistrations(rds);
            return dtl;
        }

        #endregion

        #region PlaneInfo

        public void PlaneInfoCreateView()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "CREATE VIEW IF NOT EXISTS view_PlaneInfo  AS SELECT AircraftPositions.LastUpdated, Call, Reg, AircraftPositions.Hex, Lat, Lon, Track, Alt, Speed, TypeCode, Manufacturer, Model, Category FROM AircraftPositions INNER JOIN Aircrafts ON AircraftPositions.Hex = Aircrafts.Hex INNER JOIN AircraftTypes ON AircraftTypes.ICAO = Aircrafts.TypeCode";
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteNonQuery();
            }
        }

        public int PlaneInfoBulkInsertOrUpdateIfNewer(List<PlaneInfo> planes)
        {
            if (planes == null)
                return 0;
            int i = 0;
            lock (db)
            {
                db.BeginTransaction();
                foreach (PlaneInfo plane in planes)
                {
                    try
                    {
                        // update aircraft information
                        if (PlaneInfoChecker.Check_Hex(plane.Hex) && PlaneInfoChecker.Check_Call(plane.Call) && PlaneInfoChecker.Check_Reg(plane.Reg) && PlaneInfoChecker.Check_Type(plane.Type))
                            AircraftData.Database.AircraftInsertOrUpdateIfNewer(new AircraftDesignator(plane.Hex, plane.Call, plane.Reg, plane.Type, plane.Time));
                        // update aircraft type information
                        if (!String.IsNullOrEmpty(plane.Type))
                        {
                            AircraftTypeInsertOrUpdateIfNewer(new AircraftTypeDesignator("", plane.Type, plane.Manufacturer, plane.Model, plane.Category, DateTime.UtcNow));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString());
                        return -1;
                    }
                }
                db.Commit();
            }
            return i;
        }

        public int PlaneInfoBulkInsertOrUpdateIfNewer(BackgroundWorker caller, List<PlaneInfo> planes)
        {
            if (planes == null)
                return 0;
            int i = 0;
            lock (db)
            {
                db.BeginTransaction();
                foreach (PlaneInfo plane in planes)
                {
                    try
                    {
                        // update aircraft information
                        if (!String.IsNullOrEmpty(plane.Hex) && !String.IsNullOrEmpty(plane.Reg) && !String.IsNullOrEmpty(plane.Type))
                            AircraftData.Database.AircraftInsertOrUpdateIfNewer(new AircraftDesignator(plane.Hex, plane.Call, plane.Reg, plane.Type, plane.Time));
                        // update aircraft type information
                        if (!String.IsNullOrEmpty(plane.Type))
                        {
                            AircraftTypeInsertOrUpdateIfNewer(new AircraftTypeDesignator("", plane.Type, plane.Manufacturer, plane.Model, plane.Category, DateTime.UtcNow));
                        }
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        Log.WriteMessage(ex.ToString());
                        return -1;
                    }
                    // abort if called from background worker and cancellation pending
                    if (caller != null)
                    {
                        if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        {
                            db.Rollback();
                            return -1;
                        }
                    }

                }
                db.Commit();
            }
            return i;
        }

        public List<PlaneInfo> PlaneInfoGetAll(DateTime newerthan)
        {
            List<PlaneInfo> l = new List<PlaneInfo>();
            int i = SupportFunctions.DateTimeToUNIXTime(newerthan);
            // SELECT max(AircraftPositions.Lastupdated) AS LastUpdated, Call, Reg, AircraftPositions.Hex, Lat, Lon, Track, Alt, Speed, TypeCode, Manufacturer, Model, Category FROM AircraftPositions INNER JOIN Aircrafts ON AircraftPositions.Hex = Aircrafts.Hex INNER JOIN AircraftTypes ON AircraftTypes.ICAO = Aircrafts.TypeCode WHERE AircraftPositions.LastUpdated > 1500000 GROUP BY AircraftPositions.Hex
            DataTable Result = db.Select("SELECT max(AircraftPositions.Lastupdated) AS LastUpdated, Call, Reg, AircraftPositions.Hex, Lat, Lon, Track, Alt, Speed, TypeCode, Manufacturer, Model, Category FROM AircraftPositions INNER JOIN Aircrafts ON AircraftPositions.Hex = Aircrafts.Hex INNER JOIN AircraftTypes ON AircraftTypes.ICAO = Aircrafts.TypeCode WHERE AircraftPositions.LastUpdated > " + i.ToString() + " GROUP BY AircraftPositions.Hex");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
            {
                PlaneInfo info = new PlaneInfo(row);
                try
                {
                    l.Add(info);
                }
                catch (Exception ex)
                {
                    Log.WriteMessage("Error inserting PlaneInfo[" + info.ToString() + "]: " + ex.ToString());
                }
            }
            return l;
        }

        ///<summary>
        /// Gets a list of aircraft infos at a time.
        /// Querying the latest position entry per aircraft but not older than ttl back in history
        /// and estimating the position at given time
        /// <param name="at">The given time. </param>
        /// <param name="ttl">"Time To Live": discard positions which are older than ttl [min]. </param>
        ///
        /// </summary>
        public List<PlaneInfo> PlaneInfoGetAll(DateTime at, int ttl)
        {
            List<PlaneInfo> l = new List<PlaneInfo>();
            int to = SupportFunctions.DateTimeToUNIXTime(at);
            int from = to - ttl * 60;
            DataTable Result = db.Select("SELECT max(AircraftPositions.Lastupdated) AS LastUpdated, Call, Reg, AircraftPositions.Hex, Lat, Lon, Track, Alt, Speed, TypeCode, Manufacturer, Model, Category FROM AircraftPositions INNER JOIN Aircrafts ON AircraftPositions.Hex = Aircrafts.Hex INNER JOIN AircraftTypes ON AircraftTypes.ICAO = Aircrafts.TypeCode WHERE AircraftPositions.LastUpdated >= " + from.ToString() + " AND AircraftPositions.LastUpdated <= " + to.ToString() + " GROUP BY AircraftPositions.Hex");
//            DataTable Result = db.Select("SELECT max(Lastupdated) AS LastUpdated, Call, Reg, Hex, Lat, Lon, Track, Alt, Speed, TypeCode, Manufacturer, Model, Category FROM view_PlaneInfo WHERE LastUpdated > " + from.ToString() + " AND LastUpdated <= " + to.ToString() + " GROUP BY Hex");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
            {
                PlaneInfo info = new PlaneInfo(row);
                //estimate new position
                // change speed to km/h
                double speed = info.Speed_kmh;
                // calculate distance after timespan
                double dist = speed * (at - info.Time).TotalHours;
                // estimate new position 
                LatLon.GPoint newpos = LatLon.DestinationPoint(info.Lat, info.Lon, info.Track, dist);
                info.Lat = newpos.Lat;
                info.Lon = newpos.Lon;
                info.Time = at;
                l.Add(info);
            }
            return l;

        }

        // selects all planes from a list which are in range of the midpoint of a given propagation path
        public List<PlaneInfo> GetNearestPlanes(DateTime at, PropagationPathDesignator ppath, List<PlaneInfo> planes, double maxradius, double maxdist, double maxalt)
        {
            List<PlaneInfo> l = new List<PlaneInfo>();
            // return empty list on empty list or null
            if (planes == null)
                return l;
            if (planes.Count() == 0)
                return l;
            // adjust maxradius when automatic calculation is required
            if (maxradius < 0)
                maxradius = double.MaxValue;
            if (maxradius == 0)
                maxradius = ppath.Distance / 2;
            // get midpoint value
            double midlat = ppath.GetMidPoint().Lat;
            double midlon = ppath.GetMidPoint().Lon;
            try
            {
//                StreamWriter sw = new StreamWriter("GetNearestPlanes.csv");
//                sw.WriteLine("utc;hex;call;lat;lon;alt;speed;track;maxdist");
                // get intersection info for each plane in list
                foreach (PlaneInfo info in planes)
                {
                    PlaneInfo plane;
                    try
                    {
                        // skip if plane is out of range
                        double dist = LatLon.Distance(midlat, midlon, info.Lat, info.Lon);
/*
                        sw.WriteLine(info.Time.ToString() + ";" +
                            info.Hex + ";" +
                            info.Call + ";" +
                            info.Lat.ToString("") + ";" +
                            info.Lon.ToString("") + ";" +
                            info.Alt_m.ToString("") + ";" +
                            info.Speed_kmh.ToString("") + ";" +
                            info.Track.ToString("") + ";" +
                            maxdist.ToString(""));
*/
                        if (dist > maxradius)
                            continue;

                        // clone object
                        plane = new PlaneInfo(info.Time, info.Call, info.Reg, info.Hex, info.Lat, info.Lon, info.Track, info.Alt, info.Speed, info.Type, info.Manufacturer, info.Model, info.Category);
                        //estimate new position
                        // change speed to km/h
                        double speed = info.Speed_kmh;
                        // calculate distance after timespan
                        double di = speed * (at - info.Time).TotalHours;
                        // estimate new position 
                        LatLon.GPoint newpos = LatLon.DestinationPoint(info.Lat, info.Lon, info.Track, di);
                        info.Lat = newpos.Lat;
                        info.Lon = newpos.Lon;
                        info.Time = at;


                        // calculate four possible intersections
                        // i1 -->       plane heading
                        // i2 -->       plane heading +90°
                        // i3 -->       plane heading - 90°
                        // i4 -->       opposite plane heading
                        // imin -->     intpoint with shortest distance

                        IntersectionPoint imin = null;
                        IntersectionPoint i1 = null;
                        IntersectionPoint i2 = null;
                        IntersectionPoint i3 = null;
                        IntersectionPoint i4 = null;
                        Stopwatch st = new Stopwatch();
                        i1 = ppath.GetIntersectionPoint(plane.Lat, plane.Lon, plane.Track, 0);
                        i2 = ppath.GetIntersectionPoint(plane.Lat, plane.Lon, ppath.Bearing12 - 90, 0);
                        // calculate right opposite direction only if no left intersection was found
                        if (i2 == null)
                            i3 = ppath.GetIntersectionPoint(plane.Lat, plane.Lon, ppath.Bearing12 + 90, 0);
                        // calcalute opposite direction only if no forward intersection was found
                        if (i1 == null)
                            i4 = ppath.GetIntersectionPoint(plane.Lat, plane.Lon, plane.Track - 180, 0);
                        // find the minimum distance first
                        if (i1 != null)
                            imin = i1;
                        if ((i2 != null) && ((imin == null) || (i2.QRB < imin.QRB)))
                            imin = i2;
                        if ((i3 != null) && ((imin == null) || (i3.QRB < imin.QRB)))
                            imin = i3;
                        if ((i4 != null) && ((imin == null) || (i4.QRB < imin.QRB)))
                            imin = i4;
                        // check hot planes which are very near the path first
                        if ((imin != null) && (imin.QRB <= maxdist))
                        {
                            // plane is near path
                            // use the minimum qrb info
                            plane.IntPoint = new LatLon.GPoint(imin.Lat, imin.Lon);
                            plane.IntQRB = imin.QRB;
                            plane.AltDiff = plane.Alt_m - imin.Min_H;
                            double c1 = LatLon.Bearing(plane.IntPoint.Lat, plane.IntPoint.Lon, ppath.Lat2, ppath.Lon2);
                            double c2 = plane.Track;
                            double ca = c1 - c2;
                            if (ca < 0)
                                ca = ca + 360;
                            if ((ca > 180) && (ca < 360))
                                ca = 360 - ca;
                            // save in rad 
                            plane.Angle = ca / 180.0 * Math.PI;
                            plane.Eps1 = Propagation.EpsilonFromHeights(ppath.h1, imin.Dist1, plane.Alt_m, ppath.Radius);
                            plane.Eps2 = Propagation.EpsilonFromHeights(ppath.h2, imin.Dist2, plane.Alt_m, ppath.Radius);
                            plane.Theta1 = Propagation.ThetaFromHeights(ppath.h1, imin.Dist1, plane.Alt_m, ppath.Radius);
                            plane.Theta2 = Propagation.ThetaFromHeights(ppath.h2, imin.Dist2, plane.Alt_m, ppath.Radius);
                            plane.Squint = Math.Abs(Propagation.ThetaFromHeights(ppath.h1, imin.Dist1, plane.Alt_m, ppath.Radius) - Propagation.ThetaFromHeights(ppath.h2, imin.Dist2, plane.Alt_m, ppath.Radius));
                            if (plane.AltDiff > 0)
                            {
                                // plane is high enough
                                plane.Potential = 100;
                            }
                            else if (imin.Min_H <= maxalt)
                            {
                                // plane is not high enough yet but might be in the future
                                plane.Potential = 50;
                            }
                            else
                            {
                                // minimal needed altitude is higher than Planes_MaxAlt --> no way to reach
                                // plane is not interesting
                                plane.Potential = 0;
                            }
                        }
                        else
                        {
                            // plane is far from path --> check only intersection i1 = planes moves towards path
                            if ((i1 != null) && (i1.Min_H <= maxalt))
                            {
                                plane.IntPoint = new LatLon.GPoint(i1.Lat, i1.Lon);
                                plane.IntQRB = i1.QRB;
                                plane.AltDiff = plane.Alt_m - i1.Min_H;
                                plane.Eps1 = Propagation.EpsilonFromHeights(ppath.h1, i1.Dist1, plane.Alt_m, ppath.Radius);
                                plane.Eps2 = Propagation.EpsilonFromHeights(ppath.h2, i1.Dist2, plane.Alt_m, ppath.Radius);
                                plane.Squint = Math.Abs(Propagation.ThetaFromHeights(ppath.h1, imin.Dist1, plane.Alt_m, ppath.Radius) - Propagation.ThetaFromHeights(ppath.h2, imin.Dist2, plane.Alt_m, ppath.Radius));
                                if (plane.AltDiff > 0)
                                {
                                    // plane wil cross path in a suitable altitude
                                    plane.Potential = 75;
                                }
                                else
                                {
                                    // plane wil cross path not in a suitable altitude
                                    plane.Potential = 50;
                                }
                            }
                            else
                            {
                                // plane is not interesting
                                plane.Potential = 0;
                            }
                        }

                        // add plane to list
                        l.Add(plane);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            return l;
        }

        #endregion
    }

}
