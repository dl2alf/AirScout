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
using System.Configuration;
using System.Threading;
using System.Data;
using System.Data.SQLite;
using ScoutBase.Core;
using ScoutBase.Database;
using ScoutBase.Elevation;
using SQLiteDatabase;
using Newtonsoft.Json;

namespace ScoutBase.Propagation
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
            // collect entry assembly info
            Assembly ass = Assembly.GetExecutingAssembly();
            string name = ass.GetName().Name;
            string company = "";
            object[] attribs;
            attribs = ass.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
            if (attribs.Length > 0)
            {
                company = ((AssemblyCompanyAttribute)attribs[0]).Company;
            }
            attribs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
            // create database path
            dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!String.IsNullOrEmpty(company))
                dir = Path.Combine(dir, company);
            if (!String.IsNullOrEmpty(name))
                dir = Path.Combine(dir, name);
            return Path.Combine(dir, "PropagationData");
        }
    }


    public class PropagationData
    {
        static PropagationDatabase propagation = new PropagationDatabase();
        public static PropagationDatabase Database
        {
            get
            {
                return propagation;
            }
        }

    }

    /// <summary>
    /// Holds the Station information in a database structure.
    /// </summary>
    public class PropagationDatabase : ScoutBaseDatabase
    {

        public double NearFieldSuppression { get; set; }

        System.Data.SQLite.SQLiteDatabase globe;
        System.Data.SQLite.SQLiteDatabase srtm3;
        System.Data.SQLite.SQLiteDatabase srtm1;
        System.Data.SQLite.SQLiteDatabase aster3;
        System.Data.SQLite.SQLiteDatabase aster1;

        public PropagationDatabase()
        {
            UserVersion = 1;
            Name = "ScoutBase Propagation Database";
            Description = "The Scoutbase Propagation Database is containing propagation path and horizon information.\n" +
                "The info is unique for one single or between two geographical locations, heights, frequency, F1-Clearance and calculation stepwidth.\n" +
                "All calculations are based on a distinct elevation model GLOBE, SRTM3, SRTM1 or ASTER.\n" +
                "All values are (pre-)calculated and stored at runtime.";
            // add table description manually
            TableDescriptions.Add(PropagationPathDesignator.TableName, "Holds propagation path information.");
            TableDescriptions.Add(PropagationHorizonDesignator.TableName, "Holds propagation horizon information.");
            globe = OpenDatabase("globe.db3", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
            srtm3 = OpenDatabase("srtm3.db3", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
            srtm1 = OpenDatabase("srtm1.db3", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
            aster3 = OpenDatabase("aster3.db3", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
            aster1 = OpenDatabase("aster1.db3", DefaultDatabaseDirectory(), Properties.Settings.Default.Database_InMemory);
            // create tables with schemas if not exist
            // create tables with schemas if not exist
            if (!PropagationPathTableExists(ELEVATIONMODEL.GLOBE))
                PropagationPathCreateTable(ELEVATIONMODEL.GLOBE);
            if (!PropagationPathTableExists(ELEVATIONMODEL.SRTM3))
                PropagationPathCreateTable(ELEVATIONMODEL.SRTM3);
            if (!PropagationPathTableExists(ELEVATIONMODEL.SRTM1))
                PropagationPathCreateTable(ELEVATIONMODEL.SRTM1);
            if (!PropagationPathTableExists(ELEVATIONMODEL.ASTER3))
                PropagationPathCreateTable(ELEVATIONMODEL.ASTER3);
            if (!PropagationPathTableExists(ELEVATIONMODEL.ASTER1))
                PropagationPathCreateTable(ELEVATIONMODEL.ASTER1);
            if (!PropagationHorizonTableExists(ELEVATIONMODEL.GLOBE))
                PropagationHorizonCreateTable(ELEVATIONMODEL.GLOBE);
            if (!PropagationHorizonTableExists(ELEVATIONMODEL.SRTM3))
                PropagationHorizonCreateTable(ELEVATIONMODEL.SRTM3);
            if (!PropagationHorizonTableExists(ELEVATIONMODEL.SRTM1))
                PropagationHorizonCreateTable(ELEVATIONMODEL.SRTM1);
            if (!PropagationHorizonTableExists(ELEVATIONMODEL.ASTER3))
                PropagationHorizonCreateTable(ELEVATIONMODEL.ASTER3);
            if (!PropagationHorizonTableExists(ELEVATIONMODEL.ASTER1))
                PropagationHorizonCreateTable(ELEVATIONMODEL.ASTER1);
            // set nearfield suppression to 0
            NearFieldSuppression = 0;
        }

        ~PropagationDatabase()
        {
            CloseDatabase(globe);
            CloseDatabase(srtm3);
            CloseDatabase(srtm1);
            CloseDatabase(aster3);
            CloseDatabase(aster1);
        }

        public System.Data.SQLite.SQLiteDatabase GetPropagationDatabase(ELEVATIONMODEL model)
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

        public void BackupDatabase(ELEVATIONMODEL model)
        {
            this.BackupDatabase(GetPropagationDatabase(model));
        }

        public bool IsInMemory(ELEVATIONMODEL model)
        {
            return this.IsInMemory(GetPropagationDatabase(model));
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
            return Path.Combine(dir, "PropagationData");
        }

        public DATABASESTATUS GetDBStatus(ELEVATIONMODEL model)
        {
            return this.GetDBStatus(GetPropagationDatabase(model));
        }

        public void SetDBStatus(ELEVATIONMODEL model, DATABASESTATUS status)
        {
            this.SetDBStatus(status, GetPropagationDatabase(model));
        }

        public bool GetDBStatusBit(ELEVATIONMODEL model, DATABASESTATUS statusbit)
        {
            return this.GetDBStatusBit(statusbit, GetPropagationDatabase(model));
        }

        public void SetDBStatusBit(ELEVATIONMODEL model, DATABASESTATUS statusbit)
        {
            this.SetDBStatusBit(statusbit, GetPropagationDatabase(model));
        }

        public void ResetDBStatusBit(ELEVATIONMODEL model, DATABASESTATUS statusbit)
        {
            this.ResetDBStatusBit(statusbit, GetPropagationDatabase(model));
        }

        public void BeginTransaction(ELEVATIONMODEL model)
        {
            this.BeginTransaction(GetPropagationDatabase(model));
        }

        public void Commit(ELEVATIONMODEL model)
        {
            this.BeginTransaction(GetPropagationDatabase(model));
        }

        private DataTable Select(ELEVATIONMODEL model, string sql)
        {
            return this.Select(sql, GetPropagationDatabase(model));
        }

        public string GetDBLocation(ELEVATIONMODEL model)
        {
            return this.GetDBLocation(GetPropagationDatabase(model));
        }

        public double GetDBSize(ELEVATIONMODEL model)
        {
            return this.GetDBSize(GetPropagationDatabase(model));
        }


        #region PropagationPath

        public bool PropagationPathTableExists(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = PropagationPathDesignator.TableName;
            return db.TableExists(tn);
        }

        public void PropagationPathCreateTable(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = PropagationPathDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE " + tn + " (Lat1 DOUBLE NOT NULL DEFAULT 0, Lon1 DOUBLE NOT NULL DEFAULT 0, h1 DOUBLE NOT NULL DEFAULT 0, Lat2 DOUBLE NOT NULL DEFAULT 0, Lon2 DOUBLE NOT NULL DEFAULT 0, h2 DOUBLE NOT NULL DEFAULT 0, QRG DOUBLE NOT NULL DEFAULT 0, Radius DOUBLE NOT NULL DEFAULT 0, F1_Clearance DOUBLE NOT NULL DEFAULT 0, StepWidth DOUBLE NOT NULL DEFAULT 0, Eps1_Min DOUBLE, Eps2_Min DOUBLE, LastUpdated INT32, PRIMARY KEY (Lat1, Lon1, h1, Lat2, Lon2, h2, QRG, Radius, F1_Clearance, StepWidth))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long PropagationPathCount(ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            long count = (long)db.ExecuteScalar("SELECT COUNT(*) FROM " + PropagationPathDesignator.TableName);
            if (count <= 0)
                return 0;
            return count;
        }

        public bool PropagationPathExists(double lat1, double lon1, double h1, double lat2, double lon2, double h2, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model)
        {
            PropagationPathDesignator path = new PropagationPathDesignator(lat1, lon1, h1, lat2, lon2, h2, qrg, radius, f1_clearance, stepwidth, 0, 0, double.MinValue);
            return this.PropagationPathExists(path, model);
        }

        public bool PropagationPathExists(PropagationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + PropagationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND h1 = @h1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND h2 = @h2 AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("h1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("h2"));
                db.DBCommand.Parameters.Add(path.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(path.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(path.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public PropagationPathDesignator PropagationPathFind(double lat1, double lon1, double h1, double lat2, double lon2, double h2, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, double localobstruction)
        {
            PropagationPathDesignator path = new PropagationPathDesignator(lat1, lon1, h1, lat2, lon2, h2, qrg, radius, f1_clearance, stepwidth, 0, 0, localobstruction);
            return this.PropagationPathFind(path, model);
        }

        public PropagationPathDesignator PropagationPathFind(PropagationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            // save localobstruction 
            double obstr = path.LocalObstruction;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + PropagationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND h1 = @h1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND h2 = @h2 AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("h1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("h2"));
                db.DBCommand.Parameters.Add(path.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(path.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(path.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                    return new PropagationPathDesignator(Result.Rows[0], obstr);
            }
            return null;
        }

        public DateTime PropagationPathFindLastUpdated(double lat1, double lon1, double h1, double lat2, double lon2, double h2, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model)
        {
            PropagationPathDesignator path = new PropagationPathDesignator(lat1, lon1, h1, lat2, lon2, h2, qrg, radius, f1_clearance, stepwidth, 0, 0, double.MinValue);
            return this.PropagationPathFindLastUpdated(path, model);
        }

        public DateTime PropagationPathFindLastUpdated(PropagationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + PropagationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND h1 = @h1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND h2 = @h2 AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("h1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("h2"));
                db.DBCommand.Parameters.Add(path.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(path.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(path.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (result != null)
                    return SQLiteEntry.UNIXTimeToDateTime((int)result);
            }
            return DateTime.MinValue;
        }

        public int PropagationPathInsert(PropagationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + PropagationPathDesignator.TableName + " (Lat1, Lon1, h1, Lat2, Lon2, h2, QRG, Radius, F1_Clearance, StepWidth, Eps1_Min, Eps2_Min, LastUpdated) VALUES (@Lat1, @Lon1, @h1, @Lat2, @Lon2, @h2, @QRG, @Radius, @F1_Clearance, @StepWidth, @Eps1_Min, @Eps2_Min, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("h1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("h2"));
                db.DBCommand.Parameters.Add(path.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(path.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(path.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(path.AsDouble("Eps1_Min"));
                db.DBCommand.Parameters.Add(path.AsDouble("Eps2_Min"));
                db.DBCommand.Parameters.Add(path.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int PropagationPathDelete(PropagationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + PropagationPathDesignator.TableName + " WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND h1 = @h1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND h2 = @h2 AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("h1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("h2"));
                db.DBCommand.Parameters.Add(path.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(path.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(path.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int PropagationPathDeleteAll(ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + PropagationPathDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int PropagationPathUpdate(PropagationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + PropagationPathDesignator.TableName + " SET Lat1 = @Lat1, Lon1 = @Lon1, h1 = @h1, Lat2 = @Lat2, Lon2 = @Lon2, h2 = @h2, QRG = @QRG, Radius = @Radius, F1_Clearance = @F1_Clearance, @StepWidth = @StepWidth, Eps1_Min = @Eps1_Min, Eps2_Min = @Eps2_Min, LastUpdated = @LastUpdated WHERE Lat1 = @Lat1 AND Lon1 = @Lon1 AND h1 = @h1 AND Lat2 = @Lat2 AND Lon2 = @Lon2 AND h2 = @h2 AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(path.AsDouble("Lat1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon1"));
                db.DBCommand.Parameters.Add(path.AsDouble("h1"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lat2"));
                db.DBCommand.Parameters.Add(path.AsDouble("Lon2"));
                db.DBCommand.Parameters.Add(path.AsDouble("h2"));
                db.DBCommand.Parameters.Add(path.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(path.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(path.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(path.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(path.AsDouble("Eps1_Min"));
                db.DBCommand.Parameters.Add(path.AsDouble("Eps2_Min"));
                db.DBCommand.Parameters.Add(path.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }
        public void PropagationPathInsertOrUpdateIfNewer(PropagationPathDesignator path, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            DateTime dt = this.PropagationPathFindLastUpdated(path, model);
            if (dt == DateTime.MinValue)
                this.PropagationPathInsert(path, model);
            else if (dt < path.LastUpdated)
                this.PropagationPathUpdate(path, model);
        }

        public PropagationPathDesignator PropagationPathCreateFromBearing(BackgroundWorker caller, double lat1, double lon1, double h1, double bearing, double distance, double h2, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, double localobstruction, bool savetodatabase = true)
        {
            LatLon.GPoint gp = LatLon.DestinationPoint(lat1, lon1, bearing, distance);
            return PropagationPathCreateFromLatLon(caller, lat1, lon1, h1, gp.Lat, gp.Lon, h2, qrg, radius, f1_clearance, stepwidth, model, localobstruction, savetodatabase);
        }

        public PropagationPathDesignator PropagationPathCreateFromLatLon(BackgroundWorker caller, double lat1, double lon1, double h1, double lat2, double lon2, double h2, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, double localobstruction, bool savetodatabase = true)
        {
            // calculates a propagation path, a elevation path is calculated before if needed
            // supports cancellation if calld from a background thread
            double eps1_min = double.MinValue;
            double eps2_min = double.MinValue;
            // find or create an elevation path
            ElevationPathDesignator ep = ElevationData.Database.ElevationPathFindOrCreateFromLatLon(caller, lat1, lon1, lat2, lon2, stepwidth, model, savetodatabase);
            // return null if elevation path is null for whatever reason
            if (ep == null)
                return null;
            for (int i = 0; i < ep.Count; i++)
            {
                double dist1 = i * stepwidth / 1000.0;
                double dist2 = (ep.Count - i - 1) * stepwidth / 1000.0;
                double f1c1 = ScoutBase.Core.Propagation.F1Radius(qrg, dist1, dist2) * f1_clearance;
                double f1c2 = ScoutBase.Core.Propagation.F1Radius(qrg, dist2, dist1) * f1_clearance;
                double nf = NearFieldSuppression / 1000.0;
                double eps1;
                double eps2;
                if (dist1 > nf)
                    eps1 = ScoutBase.Core.Propagation.EpsilonFromHeights(h1, dist1, ep.Path[i] + f1c1, radius);
                else
                    eps1 = ScoutBase.Core.Propagation.EpsilonFromHeights(h1, nf, ep.Path[i] + f1c1, radius);
                if (eps1 > eps1_min)
                    eps1_min = eps1;
                if (dist2 > nf)
                    eps2 = ScoutBase.Core.Propagation.EpsilonFromHeights(h2, dist2, ep.Path[i] + f1c2, radius);
                else
                    eps2 = ScoutBase.Core.Propagation.EpsilonFromHeights(h2, nf, ep.Path[i] + f1c2, radius);
                if (eps2 > eps2_min)
                    eps2_min = eps2;
                if (caller != null)
                {
                    // abort calculation if cancellation pending
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return null;
                }
            }
            PropagationPathDesignator pp = new PropagationPathDesignator(lat1, lon1, h1, lat2, lon2, h2, qrg, radius, f1_clearance, stepwidth, eps1_min, eps2_min, localobstruction);
            // take status from elevation path
            pp.Valid = ep.Valid;
            // store in database if valid
            if ((pp != null) && pp.Valid && savetodatabase)
                this.PropagationPathInsertOrUpdateIfNewer(pp, model);
            return pp;
        }

        public PropagationPathDesignator PropagationPathFindOrCreateFromBearing(BackgroundWorker caller, double lat1, double lon1, double h1, double bearing, double distance, double h2, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, double localobstruction, bool savetodatabase = true)
        {
            LatLon.GPoint gp = LatLon.DestinationPoint(lat1, lon1, bearing, distance);
            return PropagationPathFindOrCreateFromLatLon(caller, lat1, lon1, h1, gp.Lat, gp.Lon, h2, qrg, radius, f1_clearance, stepwidth, model, localobstruction, savetodatabase);
        }

        public PropagationPathDesignator PropagationPathFindOrCreateFromLatLon(BackgroundWorker caller, double lat1, double lon1, double h1, double lat2, double lon2, double h2, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, double localobstruction, bool savetodatabase = true)
        {
            PropagationPathDesignator pp = this.PropagationPathFind(lat1, lon1, h1, lat2, lon2, h2, qrg, radius, f1_clearance, stepwidth, model, localobstruction);
            if (pp == null)
                pp = this.PropagationPathCreateFromLatLon(caller, lat1, lon1, h1, lat2, lon2, h2, qrg, radius, f1_clearance, stepwidth, model, localobstruction, savetodatabase);
            return pp;
        }

        #endregion

        #region PropagationHorizon

        public bool PropagationHorizonTableExists(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            // check for table name is null or empty --> use default tablename from type instead
            string tn = tablename;
            if (String.IsNullOrEmpty(tn))
                tn = PropagationHorizonDesignator.TableName;
            return db.TableExists(tn);
        }

        public void PropagationHorizonCreateTable(ELEVATIONMODEL model, string tablename = "")
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                // check for table name is null or empty --> use default tablename from type instead
                string tn = tablename;
                if (String.IsNullOrEmpty(tn))
                    tn = PropagationHorizonDesignator.TableName;
                db.DBCommand.CommandText = "CREATE TABLE " + tn + " (Lat DOUBLE NOT NULL DEFAULT 0, Lon DOUBLE NOT NULL DEFAULT 0, h DOUBLE NOT NULL DEFAULT 0, Dist DOUBLE NOT NULL DEFAULT 0, QRG DOUBLE NOT NULL DEFAULT 0, Radius DOUBLE NOT NULL DEFAULT 0, F1_Clearance DOUBLE NOT NULL DEFAULT 0, StepWidth DOUBLE NOT NULL DEFAULT 0, Horizon BLOB, LastUpdated INT32, PRIMARY KEY (Lat, Lon, h, Dist, QRG, Radius, F1_Clearance, StepWidth))";
                db.DBCommand.Parameters.Clear();
                db.Execute(db.DBCommand);
            }
        }

        public long PropagationHorizonCount(ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            long count = (long)db.ExecuteScalar("SELECT COUNT(*) FROM " + PropagationHorizonDesignator.TableName);
            if (count <= 0)
                return 0;
            return count;
        }

        public bool PropagationHorizonExists(double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model)
        {
            PropagationHorizonDesignator path = new PropagationHorizonDesignator(lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth, null);
            return this.PropagationHorizonExists(path, model);
        }

        public bool PropagationHorizonExists(PropagationHorizonDesignator hor, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT EXISTS (SELECT LastUpdated FROM " + PropagationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND h = @h AND Dist = @Dist AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(hor.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(hor.AsDouble("h"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Dist"));
                db.DBCommand.Parameters.Add(hor.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(hor.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(hor.AsDouble("StepWidth"));
                long result = (long)db.DBCommand.ExecuteScalar();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public PropagationHorizonDesignator PropagationHorizonFind(double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, LocalObstructionDesignator localobstruction)
        {
            PropagationHorizonDesignator hor = new PropagationHorizonDesignator(lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth, localobstruction);
            return this.PropagationHorizonFind(hor, model);
        }

        public PropagationHorizonDesignator PropagationHorizonFind(PropagationHorizonDesignator hor, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            // save localobstruction 
            LocalObstructionDesignator obstr = hor.LocalObstruction;
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT * FROM " + PropagationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND h = @h AND Dist = @Dist AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(hor.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(hor.AsDouble("h"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Dist"));
                db.DBCommand.Parameters.Add(hor.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(hor.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(hor.AsDouble("StepWidth"));
                DataTable Result = db.Select(db.DBCommand);
                if ((Result != null) && (Result.Rows.Count > 0))
                    return new PropagationHorizonDesignator(Result.Rows[0], obstr);
            }
            return null;
        }

        public DateTime PropagationHorizonFindLastUpdated(double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model)
        {
            PropagationHorizonDesignator path = new PropagationHorizonDesignator(lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth,null);
            return this.PropagationHorizonFindLastUpdated(path, model);
        }

        public DateTime PropagationHorizonFindLastUpdated(PropagationHorizonDesignator hor, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "SELECT LastUpdated FROM " + PropagationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND h = @h AND Dist = @Dist AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(hor.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(hor.AsDouble("h"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Dist"));
                db.DBCommand.Parameters.Add(hor.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(hor.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(hor.AsDouble("StepWidth"));
                object result = db.ExecuteScalar(db.DBCommand);
                if (result != null)
                    return SQLiteEntry.UNIXTimeToDateTime((int)result);
            }
            return DateTime.MinValue;
        }

        public int PropagationHorizonInsert(PropagationHorizonDesignator hor, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "INSERT INTO " + PropagationHorizonDesignator.TableName + " (Lat, Lon, h, Dist, QRG, Radius, F1_Clearance, StepWidth, Horizon, LastUpdated) VALUES (@Lat, @Lon, @h, @Dist, @QRG, @Radius, @F1_Clearance, @StepWidth, @Horizon, @LastUpdated)";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(hor.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(hor.AsDouble("h"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Dist"));
                db.DBCommand.Parameters.Add(hor.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(hor.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(hor.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(hor.AsBinary("Horizon"));
                db.DBCommand.Parameters.Add(hor.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int PropagationHorizonDelete(PropagationHorizonDesignator hor, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + PropagationHorizonDesignator.TableName + " WHERE Lat = @Lat AND Lon = @Lon AND h = @h AND Dist = @Dist AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(hor.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(hor.AsDouble("h"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Dist"));
                db.DBCommand.Parameters.Add(hor.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(hor.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(hor.AsDouble("StepWidth"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int PropagationHorizonDeleteAll(ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "DELETE FROM " + PropagationHorizonDesignator.TableName;
                db.DBCommand.Parameters.Clear();
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }

        public int PropagationHorizonUpdate(PropagationHorizonDesignator hor, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            lock (db.DBCommand)
            {
                db.DBCommand.CommandText = "UPDATE " + PropagationHorizonDesignator.TableName + " SET Lat = @Lat, Lon = @Lon, h = @h, Dist = @Dist, QRG = @QRG, Radius = @Radius, F1_Clearance = @F1_Clearance, @StepWidth = @StepWidth, Horizon = @Horizon, LastUpdated = @LastUpdated WHERE Lat = @Lat AND Lon = @Lon AND h = @h AND Dist = @Dist AND QRG = @QRG AND Radius = @Radius AND F1_Clearance = @F1_Clearance AND StepWidth = @StepWidth";
                db.DBCommand.Parameters.Clear();
                db.DBCommand.Parameters.Add(hor.AsDouble("Lat"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Lon"));
                db.DBCommand.Parameters.Add(hor.AsDouble("h"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Dist"));
                db.DBCommand.Parameters.Add(hor.AsDouble("QRG"));
                db.DBCommand.Parameters.Add(hor.AsDouble("Radius"));
                db.DBCommand.Parameters.Add(hor.AsDouble("F1_Clearance"));
                db.DBCommand.Parameters.Add(hor.AsDouble("StepWidth"));
                db.DBCommand.Parameters.Add(hor.AsBinary("Horizon"));
                db.DBCommand.Parameters.Add(hor.AsUNIXTime("LastUpdated"));
                return db.ExecuteNonQuery(db.DBCommand);
            }
        }
        public void PropagationHorizonInsertOrUpdateIfNewer(PropagationHorizonDesignator hor, ELEVATIONMODEL model)
        {
            System.Data.SQLite.SQLiteDatabase db = GetPropagationDatabase(model);
            DateTime dt = this.PropagationHorizonFindLastUpdated(hor, model);
            if (dt == DateTime.MinValue)
                this.PropagationHorizonInsert(hor, model);
            else if (dt < hor.LastUpdated)
                this.PropagationHorizonUpdate(hor, model);
        }

        public PropagationHorizonDesignator PropagationHorizonCreate(BackgroundWorker caller, double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, LocalObstructionDesignator localobstruction, bool savetodatabase = true)
        {
            // calculate propagation horizon
            // report status messages and single data points if called from background thread
            HorizonPoint[] hor = new HorizonPoint[360];
            bool valid = true;
            PropagationHorizonDesignator hd = new PropagationHorizonDesignator(lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth, hor, localobstruction);
            for (int j = 0; j < 360; j++)
            {
                // report progress if called from background worker
                if (caller != null)
                {
                    if (caller.WorkerReportsProgress)
                        caller.ReportProgress(-1, "Calculating horizon " + j.ToString() + "° of 360°");
                }
                double eps_min = double.MinValue;
                double eps_dist = 0;
                short eps_elv = 0;
                // find or create elevation path
                ElevationPathDesignator ep = ElevationData.Database.ElevationPathFindOrCreateFromBearing(caller, lat, lon, j, dist, stepwidth, model, false);
                for (int i = 0; i < ep.Count; i++)
                {
                    double d = i * stepwidth / 1000.0;
                    double nf = NearFieldSuppression / 1000.0;
                    double eps;
                    if (d > nf)
                    {
                        double f1c = ScoutBase.Core.Propagation.F1Radius(qrg, d, dist) * f1_clearance;
                        eps = ScoutBase.Core.Propagation.EpsilonFromHeights(h, d, ep.Path[i] + f1c, radius);
                    }
                    else
                    {
                        double f1c = ScoutBase.Core.Propagation.F1Radius(qrg, nf, dist) * f1_clearance;
                        eps = ScoutBase.Core.Propagation.EpsilonFromHeights(h, nf, ep.Path[i] + f1c, radius);
                    }
                    if (eps > eps_min)
                    {
                        eps_min = eps;
                        eps_dist = d;
                        eps_elv = ep.Path[i];
                    }
                }
                hor[j] = new HorizonPoint(eps_dist, eps_min, eps_elv);
                // report current horizon if called from background worker
                if (caller != null)
                {
                    if (caller.WorkerReportsProgress)
                        caller.ReportProgress(j, hor[j]);
                }
                // take status from elevation path
                if (!ep.Valid)
                    valid = false;
                // abort calculation if called from background worker and cancellation pending
                if (caller != null)
                {
                    if (caller.WorkerSupportsCancellation && caller.CancellationPending)
                        return null;
                }
            }
            // copy over the horizon and status
            hd.Horizon = hor;
            hd.Valid = valid;
            // store in database if valid
            if ((hd != null) && hd.Valid && savetodatabase)
                this.PropagationHorizonInsertOrUpdateIfNewer(hd, model);
            return hd;
        }

        public PropagationHorizonDesignator PropagationHorizonFindOrCreate(BackgroundWorker caller, double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, ELEVATIONMODEL model, LocalObstructionDesignator localobstruction, bool savetodatabase = true)
        {
            PropagationHorizonDesignator hd = this.PropagationHorizonFind(lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth, model, localobstruction);
            if (hd == null)
                hd = PropagationHorizonCreate(caller, lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth, model, localobstruction, savetodatabase);
            return hd;
        }

        #endregion

    }
}
