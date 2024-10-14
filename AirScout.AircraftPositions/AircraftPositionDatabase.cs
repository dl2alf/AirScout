using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using ScoutBase.Core;
using ScoutBase.Database;
using ScoutBase.Propagation;
using System.Data.SQLite;
using System.ComponentModel;
using System.Windows.Forms;
using Newtonsoft.Json;
using AirScout.Core;
using AirScout.Aircrafts;

namespace AirScout.AircraftPositions
{

    public class AircraftPositionData
    {
        static AircraftPositionDatabase aircraftpositions = new AircraftPositionDatabase();
        public static AircraftPositionDatabase Database
        {
            get
            {
                return aircraftpositions;
            }
        }

    }

    /// <summary>
    /// Holds the Aircraft position information in a database structure.
    /// </summary>
    public class AircraftPositionDatabase : ScoutBaseDatabase
    {
        public AircraftPositionDatabase()
        {
            UserVersion = 1;
            Name = "AirScout Aircraft Position Database";
            Description = "Holds aircraft position history.";
            db = OpenDatabase("aircraftpositions.db3", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
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
            if (!AircraftPositionTableExists())
                AircraftPositionCreateTable();
        }

        ~AircraftPositionDatabase()
        {
            CloseDatabase(db);
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

        #region AircraftPositions

        public bool AircraftPositionTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = AircraftPositionDesignator.TableName;
            return db.TableExists(tn);
        }

        public void AircraftPositionCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = AircraftPositionDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE `" + tn + "`(Hex TEXT NOT NULL DEFAULT '', Call TEXT NOT NULL DEFAULT '', Lat DOUBLE, Lon DOUBLE, Alt DOUBLE, Track DOUBLE, Speed DOUBLE, LastUpdated INT32 NOT NULL DEFAULT 0, PRIMARY KEY (Hex, LastUpdated))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                // create table indices
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_Hex ON `" + tn + "` (Hex)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
                db.DBCommand.CommandText = "CREATE INDEX idx_" + tn + "_Call ON `" + tn + "` (Call)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long AircraftPositionCount()
        {
            object count = db.ExecuteScalar("SELECT COUNT(*) FROM " + AircraftPositionDesignator.TableName);
            if (IsValid(count))
                return (long)count;
            return 0;
        }

        public bool AircraftPositionExists(string hex, DateTime lastupdated)
        {
            AircraftPositionDesignator ap = new AircraftPositionDesignator(hex, lastupdated);
            return AircraftPositionExists(ap);
        }

        public bool AircraftPositionExists(AircraftPositionDesignator ap)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + AircraftPositionDesignator.TableName + " WHERE Hex = @Hex AND LastUpdated = @LastUpdated";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ap.AsString("Hex"));
                db.DBCommand.Parameters.Add(ap.AsUNIXTime("LastUpdated"));
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result) && ((long)result > 0))
                    return true;
            }
            return false;
        }

        public AircraftPositionDesignator AircraftPositionFind(string hex, DateTime lastupdated)
        {
            AircraftPositionDesignator ad = new AircraftPositionDesignator(hex, lastupdated);
            return AircraftPositionFind(ad);
        }

        public AircraftPositionDesignator AircraftPositionFindByHex(string hex)
        {
            // returs entry by search string, latest entry if more than one entry found
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE Hex = '" + hex + "' ORDER BY Lastupdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftPositionDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            return null;
        }

        public AircraftPositionDesignator AircraftPositionFindByCall(string call )
        {
            // returs entry by search string, latest entry if more than one entry found
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE Call = '" + call + "' ORDER BY Lastupdated DESC LIMIT 1";
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftPositionDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            return null;
        }

        public AircraftPositionDesignator AircraftPositionFind(AircraftPositionDesignator ap)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE Hex = @Hex AND LastUpdated = @LastUpdated";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ap.AsString("Hex"));
                db.DBCommand.Parameters.Add(ap.AsUNIXTime("LastUpdated"));
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if (IsValid(Result) && (Result.Rows.Count > 0))
                    {
                        return new AircraftPositionDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            return null;
        }

        public AircraftPositionDesignator AircraftPositionFindAt(long index)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + AircraftPositionDesignator.TableName + " LIMIT 1 OFFSET " + index.ToString();
                db.DBCommand.Parameters.Clear();
                try
                {
                    DataTable Result = db.Select(db.DBCommand);
                    if ((Result != null) && (Result.Rows.Count > 0))
                    {
                        return new AircraftPositionDesignator(Result.Rows[0]);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            return null;
        }

        public int AircraftPositionInsert(AircraftPositionDesignator ap)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + AircraftPositionDesignator.TableName + " (Hex, Call, Lat, Lon, Alt, Track, Speed, LastUpdated) VALUES (@Hex, @Call, @Lat, @Lon, @Alt, @Track, @Speed, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ap.AsString("Hex"));
                db.DBCommand.Parameters.Add(ap.AsString("Call"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Alt"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Track"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Speed"));
                db.DBCommand.Parameters.Add(ap.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftPositionDelete(string hex, DateTime lastupdated)
        {
            AircraftPositionDesignator ap = new AircraftPositionDesignator(hex, lastupdated);
            return AircraftPositionDelete(ap);
        }

        public int AircraftPositionDelete(AircraftPositionDesignator ap)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftPositionDesignator.TableName + " WHERE Hex = @Hex AND LastUpdated = @LastUpdated";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ap.AsString("Hex"));
                db.DBCommand.Parameters.Add(ap.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftPositionDeleteAll()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftPositionDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftPositionDeleteFirst(int count)
        {
            // deletes the first n entries from data table sorted by rowid
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftPositionDesignator.TableName + " WHERE rowid IN (SELECT rowid FROM AircraftPositions ORDER BY rowid ASC LIMIT " + count.ToString() + ")";
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftPositionBulkDeleteFirst(int count)
        {
            int i = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    // deletes the first n entries from data table sorted by rowid
                    lock (db.DBCommand)
                    {
                        db.DBCommand.CommandText = "DELETE FROM " + AircraftPositionDesignator.TableName + " WHERE rowid IN (SELECT rowid FROM AircraftPositions ORDER BY rowid ASC LIMIT " + count.ToString() + ")";
                        db.DBCommand.Parameters.Clear();
                        i = db.ExecuteNonQuery(db.DBCommand);
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return i;
        }

        public int AircraftPositionDeleteOlderThan(DateTime olderthan)
        {
            // deletes all entries from data table older than xxx
            AircraftPositionDesignator ap = new AircraftPositionDesignator();
            ap.LastUpdated = olderthan;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + AircraftPositionDesignator.TableName + " WHERE LastUpdated < @LastUpdated";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ap.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftPositionBulkDeleteOlderThan(DateTime olderthan)
        {
            // deletes all entries from data table older than xxx
            AircraftPositionDesignator ap = new AircraftPositionDesignator();
            ap.LastUpdated = olderthan;
            int i = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    lock (db.DBCommand)
                    {
                        db.DBCommand.CommandText = "DELETE FROM " + AircraftPositionDesignator.TableName + " WHERE LastUpdated < @LastUpdated";
                        db.DBCommand.Parameters.Clear();
                        db.DBCommand.Parameters.Add(ap.AsUNIXTime("LastUpdated"));
                        i = db.ExecuteNonQuery(db.DBCommand);
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return i;
        }

        public int AircraftPositionUpdate(AircraftPositionDesignator ap)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + AircraftPositionDesignator.TableName + " SET Hex = @Hex, Call = @Call, Lat = @Lat, Lon = @Lon, Alt = @Alt, Track = @Track, Speed = @Speed, LastUpdated = @LastUpdated WHERE Hex = @Hex AND LastUpdated = @LastUpdated";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(ap.AsString("Hex"));
                db.DBCommand.Parameters.Add(ap.AsString("Call"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Alt"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Track"));
                db.DBCommand.Parameters.Add(ap.AsDouble("Speed"));
                db.DBCommand.Parameters.Add(ap.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int AircraftPositionBulkInsert(List<AircraftPositionDesignator> aps)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftPositionDesignator ap in aps)
                    {
                        try
                        {
                            AircraftPositionInsert(ap);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error inserting aircraft position [" + ap.Hex + "]: " + ex.ToString(), LogLevel.Error);
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return -errors;
        }

        public int AircraftPositionBulkDelete(List<AircraftPositionDesignator> aps)
        {
            int errors = 0;
            try
            {
                lock (db)
                {
                    db.BeginTransaction();
                    foreach (AircraftPositionDesignator ap in aps)
                    {
                        try
                        {
                            AircraftPositionDelete(ap);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("Error deleting aircraft position[" + ap.Hex + "]: " + ex.ToString(), LogLevel.Error);
                            errors++;
                        }
                    }
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return -errors;
        }

        public int AircraftPositionBulkInsertOrUpdateIfNewer(List<AircraftPositionDesignator> aps)
        {
            if (aps == null)
                return 0;
            int i = 0;
            lock (db)
            {
                try
                {
                    db.BeginTransaction();
                    foreach (AircraftPositionDesignator ap in aps)
                    {
                        i = i + AircraftPositionInsertOrUpdateIfNewer(ap);
                    }
                }
                catch
                {

                }
                finally
                {
                    db.Commit();
                }
            }
            return i;
        }

        public int AircraftPositionBulkInsertOrUpdateIfNewer(BackgroundWorker caller, List<AircraftPositionDesignator> aps)
        {
            if (aps == null)
                return 0;
            int i = 0;
            int count = aps.Count;
            lock (db)
            {
                db.BeginTransaction();
                foreach (AircraftPositionDesignator ap in aps)
                {
                    i = i + AircraftPositionInsertOrUpdateIfNewer(ap);
                    // abort calculation if called from background worker and cancellation pending
                    if (caller != null)
                    {
                        if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        {
                            db.Rollback();
                            return -1;
                        }
                        if (caller.WorkerReportsProgress && (i % 1000 == 0))
                            caller.ReportProgress(0, "Updating position " + i.ToString() + " of " + count.ToString());
                    }

                }
                db.Commit();
            }
            return i;
        }

        public int AircraftPositionInsertOrUpdateIfNewer(AircraftPositionDesignator ap)
        {
            AircraftPositionDesignator an = AircraftPositionFind(ap);
            if (an == null)
            {
                int result = AircraftPositionInsert(ap);
 //               Console.WriteLine("Aircraft position inserted: " + ap.Hex + "," + ap.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss") + ", " + result.ToString());
                return result;
            }
            if (ap.LastUpdated > an.LastUpdated)
                return AircraftPositionUpdate(ap);
            return 0;
        }

        public DateTime AircraftPositionOldestEntry()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT min(LastUpdated) FROM " + AircraftPositionDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                {
                    DateTime dt = SupportFunctions.UNIXTimeToDateTime(System.Convert.ToInt32(result));
                    return dt;
                }
            }
            return DateTime.MinValue;

        }

        public DateTime AircraftPositionYoungestEntry()
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT max(LastUpdated) FROM " + AircraftPositionDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                object result = db.DBCommand.ExecuteScalar();
                if (IsValid(result))
                {
                    DateTime dt = SupportFunctions.UNIXTimeToDateTime(System.Convert.ToInt32(result));
                    return dt;
                }
            }
            return DateTime.MinValue;

        }

        public List<DateTime> AircraftPositionGetAllLastUpdated()
        {
            List<DateTime> l = new List<DateTime>();
            DataTable Result = db.Select("SELECT LastUpdated FROM " + AircraftPositionDesignator.TableName + " ORDER BY LastUpdated ASC");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(SupportFunctions.UNIXTimeToDateTime((int)row[0]));
            return l;

        }

        public List<string> AircraftPositionGetAllHex()
        {
            List<string> l = new List<string>();
            DataTable Result = db.Select("SELECT Hex FROM " + AircraftPositionDesignator.TableName + " GROUP BY Hex");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(row[0].ToString());
            return l;

        }

        public List<string> AircraftPositionGetAllHex(DateTime from, DateTime to)
        {
            List<string> l = new List<string>();
            DataTable Result = db.Select("SELECT Hex FROM " + AircraftPositionDesignator.TableName + " WHERE LastUpdated >= " + SupportFunctions.DateTimeToUNIXTime(from).ToString() + " AND LastUpdated <= " + SupportFunctions.DateTimeToUNIXTime(to).ToString() + " GROUP BY Hex");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(row[0].ToString());
            return l;

        }

        public List<string> AircraftPositionGetAllCalls()
        {
            List<string> l = new List<string>();
            DataTable Result = db.Select("SELECT Call FROM " + AircraftPositionDesignator.TableName + " GROUP BY Call");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(row[0].ToString());
            return l;

        }

        public List<string> AircraftPositionGetAllCalls(DateTime from, DateTime to)
        {
            List<string> l = new List<string>();
            DataTable Result = db.Select("SELECT Call FROM " + AircraftPositionDesignator.TableName + " WHERE LastUpdated >= " + SupportFunctions.DateTimeToUNIXTime(from).ToString() + " AND LastUpdated <= " + SupportFunctions.DateTimeToUNIXTime(to).ToString() + " GROUP BY Call");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(row[0].ToString());
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionGetAll()
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftPositionDesignator.TableName + " ORDER BY LastUpdated ASC");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftPositionDesignator(row));
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionGetAll(BackgroundWorker caller)
        {
            // gets all aircraftpositions from database
            // supports abort calculation if called from background worker and cancellation requested
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            int i = 0;
            SQLiteCommand cmd = new SQLiteCommand(db.DBConnection);
            cmd.CommandText = "SELECT * FROM " + AircraftPositionDesignator.TableName + " ORDER BY LastUpdated ASC";
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AircraftPositionDesignator ap = new AircraftPositionDesignator((IDataRecord)reader);
                l.Add(ap);
                i++;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return new List<AircraftPositionDesignator>();
                    if (caller.WorkerReportsProgress && (i % 1000 == 0))
                        caller.ReportProgress(0, "Getting position " + i.ToString());
                }
            }
            reader.Close();
            return l;
        }

        public List<AircraftPositionDesignator> AircraftPositionGetAllByHex(string hex)
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE Hex = '" + hex + "' ORDER BY LastUpdated ASC");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftPositionDesignator(row));
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionGetAllByHex(string hex, DateTime from, DateTime to)
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE Hex = '" + hex + "' AND LastUpdated >= " + SupportFunctions.DateTimeToUNIXTime(from).ToString() + " AND LastUpdated <= " + SupportFunctions.DateTimeToUNIXTime(to).ToString() + " ORDER BY LastUpdated ASC");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftPositionDesignator(row));
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionGetAllByCall(string call)
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE Call = '" + call + "' ORDER BY LastUpdated ASC");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftPositionDesignator(row));
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionGetAllByCall(string call, DateTime from, DateTime to)
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE Call = '" + call + "' AND LastUpdated >= " + SupportFunctions.DateTimeToUNIXTime(from).ToString() + " AND LastUpdated <= " + SupportFunctions.DateTimeToUNIXTime(to).ToString() + " ORDER BY LastUpdated ASC");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftPositionDesignator(row));
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionGetAll(DateTime from, DateTime to)
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            DataTable Result = db.Select("SELECT * FROM " + AircraftPositionDesignator.TableName + " WHERE LastUpdated >= " + SupportFunctions.DateTimeToUNIXTime(from).ToString() + " AND LastUpdated <= " + SupportFunctions.DateTimeToUNIXTime(to).ToString() + " ORDER BY LastUpdated ASC");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftPositionDesignator(row));
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionGetAllLatest(DateTime from)
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            int i = SupportFunctions.DateTimeToUNIXTime(from);
            DataTable Result = db.Select("SELECT Hex, Call, Lat, Lon, Alt, Track, Speed, max(Lastupdated) AS LastUpdated FROM " + AircraftPositionDesignator.TableName + " WHERE LastUpdated > " + i.ToString() + " GROUP BY Hex");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
                l.Add(new AircraftPositionDesignator(row));
            return l;

        }


        ///<summary>
        /// Gets a list of aircraft positions at a time.
        /// Querying the latest position entry per aircraft but not older than ttl back in history
        /// and estimating the position at given time
        /// <param name="at">The given time. </param>
        /// <param name="ttl">"Time To Live": discard positions which are older than ttl [min]. </param>
        ///
        /// </summary>
        public List<AircraftPositionDesignator> AircraftPositionGetAllLatest(DateTime at, int ttl)
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            int to = SupportFunctions.DateTimeToUNIXTime(at);
            int from = to - ttl * 60;
            DataTable Result = db.Select("SELECT Hex, Call, Lat, Lon, Alt, Track, Speed, max(Lastupdated) AS LastUpdated FROM " + AircraftPositionDesignator.TableName + " WHERE LastUpdated >= " + from.ToString() + " AND LastUpdated <= " + to.ToString() + " GROUP BY Hex");
            if (!IsValid(Result) || (Result.Rows.Count == 0))
                return l;
            foreach (DataRow row in Result.Rows)
            {
                AircraftPositionDesignator ap = new AircraftPositionDesignator(row);
                //estimate new position
                // change speed to km/h
                double speed = ap.Speed * 1.852;
                // calculate distance after timespan
                double dist = speed * (at - ap.LastUpdated).TotalHours;
                // estimate new position 
                LatLon.GPoint newpos = LatLon.DestinationPoint(ap.Lat, ap.Lon, ap.Track, dist);
                ap.Lat = newpos.Lat;
                ap.Lon = newpos.Lon;
                l.Add(ap);
            }
            return l;

        }

        public List<AircraftPositionDesignator> AircraftPositionFromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<List<AircraftPositionDesignator>>(json, settings);
        }

        public string AircraftPositionToJSON()
        {
            List<AircraftPositionDesignator> l = AircraftPositionGetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(l, settings);
            return json;
        }

        public DataTableAircraftPositions AircraftPositionToDataTable()
        {
            List<AircraftPositionDesignator> ap = AircraftPositionGetAll();
            DataTableAircraftPositions dtl = new DataTableAircraftPositions(ap);
            return dtl;
        }

        #endregion

        #region PlaneInfo

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
                        if (PlaneInfoChecker.Check_Hex(plane.Hex) && PlaneInfoChecker.Check_Call(plane.Call))
                            AircraftPositionData.Database.AircraftPositionInsertOrUpdateIfNewer(new AircraftPositionDesignator(plane.Hex, plane.Call, plane.Lat, plane.Lon, plane.Alt, plane.Track, plane.Speed, plane.Time));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString(), LogLevel.Error);
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
                        if (PlaneInfoChecker.Check_Hex(plane.Hex) && PlaneInfoChecker.Check_Call(plane.Call))
                            AircraftPositionData.Database.AircraftPositionInsertOrUpdateIfNewer(new AircraftPositionDesignator(plane.Hex, plane.Call, plane.Lat, plane.Lon, plane.Alt, plane.Track, plane.Speed, plane.Time));
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        Log.WriteMessage(ex.ToString(), LogLevel.Error);
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
                    Log.WriteMessage("Error inserting PlaneInfo[" + info.ToString() + "]: " + ex.ToString(), LogLevel.Error);
                }
            }
            return l;
        }

        #endregion

    }

}
