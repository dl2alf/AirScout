using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;
using ScoutBase.Core;

namespace ScoutBase.Database
{
    public class ScoutBaseDatabase
    {
        protected System.Data.SQLite.SQLiteDatabase db;
        protected string DBPath;
        protected static LogWriter Log = LogWriter.Instance;
        protected int UserVersion = 1;
        public string Name = "ScoutBase Database Name";
        public string Description = "ScoutBase Database Description";

        public Dictionary<string, string> TableDescriptions = new Dictionary<string, string>();

        protected System.Data.SQLite.SQLiteDatabase OpenDatabase(string name, string defaultdatabasedirectory, bool inmemory)
        {
            System.Data.SQLite.SQLiteDatabase db = null;
            try
            {
                // check if database path exists --> create if not
                if (!Directory.Exists(defaultdatabasedirectory))
                    Directory.CreateDirectory(defaultdatabasedirectory);
                // check if database is already on disk 
                DBPath = defaultdatabasedirectory;
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
                if (inmemory)
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

        protected void CloseDatabase(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return;
            // save in-memory database to disk
            if (db.IsInMemory)
                db.BackupDatabase(db.DBLocation);
            //            else
            //                db.Close();
        }

        public void BackupDatabase(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return;
            // save in-memory database to disk
            if (db.IsInMemory)
                db.BackupDatabase(db.DiskFileName);
            else
                db.Close();
        }

        public bool IsInMemory(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            return db.IsInMemory;
        }

        public SQLiteConnection GetDBConnection(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return null;
            return db.DBConnection;
        }
        public string GetDBLocation(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return "";
            return db.DBLocation;
        }

        public double GetDBSize(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return 0;
            return db.DBSize;
        }

        public DataTable GetTableList(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return null;
            return db.GetTableList();
        }

        public string GetTableDescription(string tablename, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            string desc;
            if (TableDescriptions.TryGetValue(tablename, out desc))
                return desc;
            return "[not found]";
        }

        public long GetTableRowCount (string tablename, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                db = this.db;
            if (db == null)
                return 0;
            return db.TableRowCount(tablename);
        }

        public void ClearTable(string tablename, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                db = this.db;
            if (db == null)
                return;
            db.ClearTable(tablename);
        }
        public DATABASESTATUS GetDBStatus(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return DATABASESTATUS.UNDEFINED;
            return db.Status;
        }

        public void SetDBStatus(DATABASESTATUS status, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db != null)
                db.Status = status;
        }

        public bool GetDBStatusBit(DATABASESTATUS statusbit, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db != null)
                return (((int)db.Status) & ((int)statusbit)) > 0;
            return false;
        }

        public void SetDBStatusBit(DATABASESTATUS statusbit, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db != null)
                db.Status |= statusbit;
        }

        public void ResetDBStatusBit(DATABASESTATUS statusbit, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db != null)
                db.Status &= ~statusbit;
        }

        public void BeginTransaction(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return;
            db.BeginTransaction();
        }

        public void Commit(System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return;
            db.Commit();
        }

        public DataTable Select(string sql, System.Data.SQLite.SQLiteDatabase db = null)
        {
            if (db == null)
                db = this.db;
            if (db == null)
                return null;
            return db.Select(sql);
        }


        protected bool IsValid(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() == typeof(DBNull))
                return false;
            return true;
        }

    }


}
