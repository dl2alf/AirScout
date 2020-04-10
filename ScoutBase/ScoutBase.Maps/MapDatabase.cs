using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using SQLiteDatabase;
using System.IO;
using ScoutBase.Core;
using ScoutBase.Database;
using System.Data;

namespace ScoutBase.Maps
{
    public class DefaultDatabaseDirectory
    {
        public new string ToString()
        {
            // create default database directory name
            string dir = Properties.Settings.Default.Database_Directory;
            if (!String.IsNullOrEmpty(dir))
            {
                return dir;
            }
            // empty settings -_> create standard path
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GMap.NET", "TileDBv5", "en");
        }
    }


    public class MapData
    {
        static MapDatabase map = new MapDatabase();
        public static MapDatabase Database
        {
            get
            {
                return map;
            }
        }

    }

    /// <summary>
    /// Holds the Map information in a database structure.
    /// </summary>
    public class MapDatabase : ScoutBaseDatabase
    {

        public string TilesTableName = "Tiles";
        public string TilesDataTableName = "TilesData";

        public MapDatabase()
        {
            UserVersion = 5;
            Name = "ScoutBase Map Database (GMaps.NET)";
            Description = "The Scoutbase Map Database is containing map tiles and is serviced mainly from GMaps.NET library.\n" +
                "This object provides an interface for direct reading from / writing to database.";
            // add table description manually
            TableDescriptions.Add(TilesTableName, "Holds map tile inoformation.");
            TableDescriptions.Add(TilesDataTableName, "Holds map tile data.");
            db = OpenDatabase("Data.gmdb", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
            // create tables with schemas if not exist
            if (!TilesTableExists())
                TilesCreateTable();
            if (!TilesDataTableExists())
                TilesDataCreateTable();
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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GMap.NET", "TileDBv5", "en");
        }


        #region Tiles

        public bool TilesTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = TilesTableName;
            return db.TableExists(tn);
        }

        public bool TilesDataTableExists(string tablename = "")
        {
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = TilesDataTableName;
            return db.TableExists(tn);
        }

        public void TilesCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = TilesTableName;
                db.DBCommand.CommandText = "CREATE TABLE Tiles (id INTEGER NOT NULL PRIMARY KEY, X INTEGER NOT NULL, Y INTEGER NOT NULL, Zoom INTEGER NOT NULL, Type UNSIGNED INTEGER  NOT NULL, CacheTime DATETIME)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public void TilesDataCreateTable(string tablename = "")
        {
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = TilesDataTableName;
                db.DBCommand.CommandText = "CREATE TABLE TilesData (id INTEGER NOT NULL PRIMARY KEY CONSTRAINT fk_Tiles_id REFERENCES Tiles(id) ON DELETE CASCADE, Tile BLOB NULL)";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long TilesCount()
        {
            long count = (long)db.ExecuteScalar("SELECT COUNT(*) FROM " + TilesTableName);
            if (count <= 0)
                return 0;
            return count;
        }

        public long TilesDataCount()
        {
            long count = (long)db.ExecuteScalar("SELECT COUNT(*) FROM " + TilesDataTableName);
            if (count <= 0)
                return 0;
            return count;
        }

        public bool TileExists(int x, int y, int z, int type)
        {
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT CacheTime FROM " + TilesTableName + " WHERE X = @x AND Y = @y AND Zoom = @z AND Type = @type)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(new SQLiteParameter("@x", x));
                db.DBCommand.Parameters.Add(new SQLiteParameter("@y", y));
                db.DBCommand.Parameters.Add(new SQLiteParameter("@z", z));
                db.DBCommand.Parameters.Add(new SQLiteParameter("@type", type));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public MapTileInfo TileFind(int id)
        {
            db.DBCommand.CommandText = "SELECT * FROM " + TilesTableName + " WHERE id = @id";
            db.DBCommand.Parameters.Clear();
            db.DBCommand.Parameters.Add(new SQLiteParameter("@id", id));
            DataTable dt = db.Select(db.DBCommand);
            if (dt != null)
            {
                DataRow row = dt.Rows[0];
                int _id = System.Convert.ToInt32(row[0]);
                int _x = System.Convert.ToInt32(row[1]);
                int _y = System.Convert.ToInt32(row[2]);
                int _zoom = System.Convert.ToInt32(row[3]);
                int _type = System.Convert.ToInt32(row[4]);
                DateTime _cachetime = ((DateTime)row[5]).ToLocalTime();
                MapTileInfo m = new MapTileInfo(_id, _x, _y, _zoom, _type, _cachetime);
                return m;
            }
            return null;
        }

        public MapTileInfo TileFind(int x, int y, int z, int type)
        {
            db.DBCommand.CommandText = "SELECT * FROM " + TilesTableName + " WHERE X = @x AND Y = @y AND Zoom = @z AND Type = @type";
            db.DBCommand.Parameters.Clear();
            db.DBCommand.Parameters.Add(new SQLiteParameter("@x", x));
            db.DBCommand.Parameters.Add(new SQLiteParameter("@y", y));
            db.DBCommand.Parameters.Add(new SQLiteParameter("@z", z));
            db.DBCommand.Parameters.Add(new SQLiteParameter("@type", type));
            DataTable dt = db.Select(db.DBCommand);
            if (dt != null)
            {
                DataRow row = dt.Rows[0];
                int _id = System.Convert.ToInt32(row[0]);
                int _x = System.Convert.ToInt32(row[1]);
                int _y = System.Convert.ToInt32(row[2]);
                int _zoom = System.Convert.ToInt32(row[3]);
                int _type = System.Convert.ToInt32(row[4]);
                DateTime _cachetime = ((DateTime)row[5]).ToLocalTime();
                MapTileInfo m = new MapTileInfo(_id, _x, _y, _zoom, _type, _cachetime);
                return m;
            }
            return null;
        }

        public MapTileData TileDataFind(int id)
        {
            db.DBCommand.CommandText = "SELECT * FROM " + TilesDataTableName + " WHERE id = @id";
            db.DBCommand.Parameters.Clear();
            db.DBCommand.Parameters.Add(new SQLiteParameter("@id", id));
            DataTable dt = db.Select(db.DBCommand);
            if (dt != null)
            {
                MapTileData m = new MapTileData(System.Convert.ToInt32(dt.Rows[0][0]), (byte[])dt.Rows[0][1]);
                return m;
            }
            return null;
        }

        public int TileInsert(int x, int y, int z, int type, DateTime cachetime, byte[] tile)
        {
            int id = -1;
//            db.BeginTransaction();
            try
            {
                id = System.Convert.ToInt32(db.ExecuteScalar("SELECT MAX(id) FROM " + TilesTableName)) + 1;
                lock (db.DBCommand)
                {
                    db.DBCommand.CommandText = "INSERT INTO " + TilesTableName + " (id, X, Y, Zoom, Type, CacheTime) VALUES (@id, @x, @y, @z, @type, @cachetime)";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@id", id));
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@x", x));
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@y", y));
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@z", z));
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@type", type));
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@cachetime", cachetime));
                    db.ExecuteNonQuery(db.DBCommand);
                    db.DBCommand.CommandText = "INSERT INTO " + TilesDataTableName + " (id, Tile) VALUES (@id, @tile)";
                    db.DBCommand.Parameters.Clear();
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@id", id));
                    db.DBCommand.Parameters.Add(new SQLiteParameter("@tile", tile));
                    db.ExecuteNonQuery(db.DBCommand);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
//            db.Commit();
            return id;
        }

        public int TileDeleteAll()
        {
            db.BeginTransaction();
            int result;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + TilesTableName;
                db.DBCommand.Parameters.Clear();
                result = db.ExecuteNonQuery(db.DBCommand);
                db.DBCommand.CommandText = "DELETE FROM " + TilesDataTableName;
                db.DBCommand.Parameters.Clear();
                result = db.ExecuteNonQuery(db.DBCommand);
            }
            db.Commit();
            return result;
        }

        public int TileDelete(int id)
        {
            db.BeginTransaction();
            int result;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + TilesTableName + " WHERE id = " + id.ToString();
                db.DBCommand.Parameters.Clear();
                result = db.ExecuteNonQuery(db.DBCommand);
                db.DBCommand.CommandText = "DELETE FROM " + TilesDataTableName + " WHERE id = " + id.ToString();
                db.DBCommand.Parameters.Clear();
                result = db.ExecuteNonQuery(db.DBCommand);
            }
            db.Commit();
            return result;
        }

        public List<MapTileInfo> TilesGetAll()
        {
            List<MapTileInfo> l = new List<MapTileInfo>();
            DataTable dt = db.Select("SELECT * FROM " + TilesTableName);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int _id = System.Convert.ToInt32(row[0]);
                    int _x = System.Convert.ToInt32(row[1]);
                    int _y = System.Convert.ToInt32(row[2]);
                    int _zoom = System.Convert.ToInt32(row[3]);
                    int _type = System.Convert.ToInt32(row[4]);
                    DateTime _cachetime = ((DateTime)row[5]).ToLocalTime();
                    MapTileInfo m = new MapTileInfo(_id, _x, _y, _zoom, _type, _cachetime);
                    l.Add(m);
                }
            }
            return l;
        }
        #endregion

    }
}

