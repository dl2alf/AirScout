using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace ScoutBase.Data
{
    // create a singleton object for common access to ScoutBase Database
    public class ScoutData
    {
        static ScoutBaseDatabase database = new ScoutBaseDatabase();
        public static ScoutBaseDatabase Database
        {
            get
            {
                return database;
            }
        }
    }

    public partial class ScoutBaseDatabase : Object
    {
        private System.Data.SQLite.SQLiteDatabase db;
        private string DBPath;
        private VersionInfo versioninfo;
        public CallsignDictionary callsigns;
        private HorizonDictionary horizons;
        private DataTableCallsigns Callsigns = new DataTableCallsigns();

        public string VersionInfo
        {
            get
            {
                return versioninfo.Version;
            }
        }

        public ScoutBaseDatabase()
        {
            try
            {
                // create dictionaries
                versioninfo = new VersionInfo();
                callsigns = new CallsignDictionary();
                horizons = new HorizonDictionary();
                // check if database path exists --> create if not
                if (!Directory.Exists(DefaultDatabaseDirectory()))
                    Directory.CreateDirectory(DefaultDatabaseDirectory());
                // check if database is already on disk 
                DBPath = Path.Combine(DefaultDatabaseDirectory(), "ScoutBase.db3");
                if (!File.Exists(DBPath))
                {
                    // create one on disk
                    System.Data.SQLite.SQLiteDatabase dbn = new System.Data.SQLite.SQLiteDatabase(DBPath);
                    dbn.Open();
                    dbn.CreateTable(new DataTableVersionInfo());
                    dbn.CreateTable(new DataTableCallsigns());
                    dbn.CreateTable(new DataTableHorizons());
                    dbn.CreateTable(new DataTableGLOBE());
                    dbn.CreateTable(new DataTableSRTM3());
                    dbn.CreateTable(new DataTableSRTM1());
                    if (String.IsNullOrEmpty(versioninfo.Version))
                    {
                        // no version info
                        // initally create one and store in database
                        versioninfo.Version = System.Reflection.Assembly.GetAssembly(typeof(ScoutBaseDatabase)).GetName().Version.ToString();
                        versioninfo.LastUpdated = DateTime.UtcNow;
                        dbn.InsertOrReplaceTable(versioninfo.WriteToTable());
                    }
                    dbn.Close();
                }
                if (Properties.Settings.Default.Database_InMemory)
                    db = System.Data.SQLite.SQLiteDatabase.CreateInMemoryDB(DBPath);
                else
                {
                    db = new System.Data.SQLite.SQLiteDatabase(DBPath);
                    db.Open();
                }
                // check version
                if (String.Compare(System.Reflection.Assembly.GetAssembly(typeof(ScoutBaseDatabase)).GetName().Version.ToString(), versioninfo.Version) > 0)
                {
                    // do any upgrade stuff here if necessary
                }
                // read data tables and initialize dictionaries
                ReadDataTables();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initilalizing database: " + ex.Message);
                throw new TypeInitializationException(this.GetType().Name, ex);
            }

        }

        ~ScoutBaseDatabase()
        {
            // save in-memory database to disk
            if (db.IsInMemory)
                BackupDatabase();
        }

        public void BackupDatabase()
        {
            db.BackupDatabase(DBPath);
        }

        public void Close()
        {
            if (db != null)
                db.Close();
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
            return Path.Combine(dir, "Database");
        }

        public DataTable Select(string sql) 
        {
            return db.Select(sql);
        }

        public void CreateTable (DataTable dt)
        {
            db.CreateTable(dt);
        }

        public void ClearTable(string tableName)
        {
            db.ClearTable(tableName);
        }

        public void InsertRow(string tableName, DataRow row)
        {
            db.InsertDataRow(tableName, row);
        }

        public DataTableCallsigns ReadDataTableCallsigns()
        {
            DataTableCallsigns dtc = new DataTableCallsigns();
            DataTable dt = db.Select("SELECT * FROM " + dtc.TableName);
            lock (dt)
            {
                foreach (DataRow row in dt.Rows)
                    dtc.ImportRow(row);
            }
            return dtc;
        }

        public void ReadDataTables()
        {
            // initialize and load tables from database into directories
            DataTable dt;
            dt = db.Select("SELECT * FROM '" + new DataTableCallsigns().TableName + "'");
            if ((dt != null) && (dt.Rows.Count > 0))
                callsigns = new CallsignDictionary(dt);
            else
                callsigns = new CallsignDictionary();
            dt = db.Select("SELECT * FROM " + new DataTableHorizons().TableName);
            if ((dt != null) && (dt.Rows.Count > 0))
                horizons = new HorizonDictionary(dt);
            else
                horizons = new HorizonDictionary();
        }

        public void UpdateFromDataTables(bool clear = true)
        {
            // update from tables from database into dictionaries
            // create empty dictionaries if not already created
            if (callsigns == null)
                callsigns = new CallsignDictionary();
            if (horizons == null)
                horizons = new HorizonDictionary();
            // clear dictionaries
            if (clear)
            {
                callsigns.Clear();
                horizons.Clear();
            }
            // fill dictionaries
            DataTable dt;
            dt = db.Select("SELECT * FROM " + new DataTableCallsigns().TableName);
            callsigns.FromTable(dt);
            dt = db.Select("SELECT * FROM " + new DataTableHorizons().TableName);
            horizons.FromTable(dt);
        }

        public void WriteDataTableCallsigns(DataTableCallsigns dt)
        {
            if (dt != null)
            {
                db.ClearTable(dt.TableName);
                db.InsertOrReplaceTable(dt);
            }
        }

        public void WriteDataTables()
        {
            // save all directories to database
            if ((callsigns != null) && (callsigns.Calls.Count > 0) && callsigns.Changed)
            {
                db.ClearTable(new DataTableCallsigns().TableName);
                db.InsertOrReplaceTable(callsigns.ToTable());
            }
            if ((horizons != null) && (horizons.Horizons.Count > 0) && horizons.Changed)
            {
                db.ClearTable(new DataTableHorizons().TableName);
                db.InsertOrReplaceTable(horizons.ToTable());
            }
        }

        public string HorizonssWriteToJSON()
        {
            if (horizons != null)
            {
                return horizons.WriteToJSON();
            }
            return null;
        }

        public string[][] HorizonssToArray()
        {
            if (horizons != null)
            {
                return horizons.ToArray();
            }
            return null;
        }

        public void WriteToJSON(string datapath)
        {
            string json = callsigns.ToJSONArray();
            if (!String.IsNullOrEmpty(json))
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(datapath, callsigns.Name + ".json")))
                {
                    sw.WriteLine(json);
                }
            }
            json = horizons.WriteToJSON();
            if (!String.IsNullOrEmpty(json))
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(datapath, horizons.Name + ".json")))
                {
                    sw.WriteLine(json);
                }
            }
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

        public void InsertOrUpdateTable(DataTable dt)
        {
            db.InsertOrReplaceTable(dt);
        }

        public void CallsignsFromTXT(string filename)
        {
            if (callsigns == null)
                return;
            DataTable dt = callsigns.FromTXT(filename);
//            db.InsertOrReplaceTable(dt);
//            callsigns.Clear();
//            dt = db.Select("SELECT * FROM " + new DataTableCallsigns().TableName);
            callsigns.FromTable(dt);
        }

        public void CallsignsFromJSONArray(string filename)
        {
            if (callsigns == null)
                return;
            DataTable dt = callsigns.FromJSONArray(filename);
            db.InsertOrReplaceTable(dt);
//            callsigns.Clear();
//            dt = db.Select("SELECT * FROM " + new DataTableCallsigns().TableName);
            callsigns.FromTable(dt);
        }

        public string CallsignsToJSONArray()
        {
            if (callsigns == null)
                return "";
            return callsigns.ToJSONArray();
        }
    }
}
