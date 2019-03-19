using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.IO;
using System.Data;
using AirScout.Database.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScoutBase.Data
{

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableHorizons : DataTable
    {
        public DataTableHorizons()
            : base()
        {
            // set table name
            TableName = "Horizons";
            // create all specific columns
            DataColumn Model = this.Columns.Add("Model", typeof(string));
            DataColumn Lat = this.Columns.Add("Lat", typeof(double));
            DataColumn Lon = this.Columns.Add("Lon", typeof(double));
            DataColumn Height = this.Columns.Add("Height", typeof(double));
            DataColumn Clearance = this.Columns.Add("Clearance", typeof(double));
            DataColumn Horizon = this.Columns.Add("Horizon", typeof(Horizon));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[5];
            keys[0] = Model;
            keys[1] = Lat;
            keys[2] = Lon;
            keys[3] = Height;
            keys[4] = Clearance;
            this.PrimaryKey = keys;
        }
    }


    //
    // lookup table for all callsigns
    //
    public class HorizonDictionary : Object
    {
        public string Version
        {
            get
            {
                return "1.0.0.0";
            }
        }

        public string Name
        {
            get
            {
                return "Horizonss";
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

        public SortedDictionary<string, HorizonDesignator> Horizons = new SortedDictionary<string, HorizonDesignator>();

        public HorizonDictionary()
        {
        }

        public HorizonDictionary(DataTable table)
        {
            // create dictionary from data table
            FromTable(table);
            // reset changed flag
            changed = false;
        }

        public void Clear()
        {
            if (Horizons.Count > 0)
                changed = true;
            Horizons.Clear();
        }

        public void FromTable(DataTable dt)
        {
            if (dt == null)
                return;
            foreach (DataRow row in dt.Rows)
            {
                string model = row["Model"].ToString();
                double lat = (double)row["Lat"];
                double lon = (double)row["Lon"];
                double height = (double)row["Height"];
                double clearance = (double)row["Clearance"];
                Horizon horizion = (Horizon)row["Horizon"];
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
                    Update(new HorizonDesignator(model, lat, lon, height,clearance,horizion, lastupdated));
                }
                catch
                {
                }
            }
                
        }

        public string WriteToJSON()
        {
            // write json string
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Formatting.Indented;
            string json = JsonConvert.SerializeObject(Horizons, settings);
            return json;
        }

        public string[][] ToArray()
        {
            List<HorizonDesignator> l = Horizons.Values.ToList<HorizonDesignator>();
            string[][] a = new string[l.Count][];
            for (int i = 0; i < l.Count; i++)
            {
                a[i] = l[i].ToArray();
            }
            return a;
        }

        public DataTable ToTable()
        {
            DataTableHorizons dt = new DataTableHorizons();
            foreach (KeyValuePair<string, HorizonDesignator> horizon in Horizons)
            {
                DataRow row = dt.NewRow();

                row["Model"] = horizon.Value.Model;
                row["Lat"] = horizon.Value.Lat;
                row["Lon"] = horizon.Value.Lon;
                row["Height"] = horizon.Value.Height;
                row["Clearance"] = horizon.Value.Clearance;
                row["Horizon"] = horizon.Value.Horizon;
                row["LastUpdated"] = horizon.Value.LastUpdated.ToString("u");
                dt.Rows.Add(row);
            }
            return dt;
        }

        public void Update(HorizonDesignator horizon)
        {
            // return on null
            if (horizon == null)
                return;
            HorizonDesignator entry = null;
            if (!Horizons.TryGetValue(horizon.ToString(), out entry))
            {
                // key not found --> add new entry
                Horizons.Add(horizon.ToString(), new HorizonDesignator(horizon.Model, horizon.Lat,horizon.Lon,horizon.Height,horizon.Clearance,horizon.Horizon));
                changed = true;
            }
            else
            {
                // key found --> check for update
                if (horizon.LastUpdated > entry.LastUpdated)
                {
                    // new timestamp --> udpate all not empty fields
                    entry.Model = horizon.Model;
                    entry.Lat = horizon.Lat;
                    entry.Lon = horizon.Lon;
                    entry.Height = horizon.Height;
                    entry.Clearance = horizon.Clearance;
                    entry.LastUpdated = horizon.LastUpdated;
                    changed = true;
                }
            }
        }

        public HorizonDesignator Find(string model, double lat, double lon, double height, double clearance)
        {
            HorizonDesignator info = null;
            string search = new HorizonDesignator(model,lat, lon, height,clearance,null).ToString();
            Horizons.TryGetValue(search, out info);
            return info;
        }
    
        public List<HorizonDesignator> GetHorizons()
        {
            return Horizons.Values.ToList();
        }

    }

    public class HorizonDesignator
    {
        public string Model;
        public double Lat;
        public double Lon;
        public double Height;
        public double Clearance;
        public Horizon Horizon;
        public DateTime LastUpdated;

        public HorizonDesignator()
        {
            Model = "";
            Lat = 0;
            Lon = 0;
            Height = 0;
            Clearance = 0;
            Horizon = new Horizon();
            LastUpdated = DateTime.MinValue;
        }

        public HorizonDesignator(string model, double lat, double lon, double height, double clearance, Horizon horizon)
            : this(model, lat, lon, height, clearance, horizon, DateTime.UtcNow) { }
        public HorizonDesignator(string model, double lat, double lon, double height, double clearance, Horizon horizon, DateTime lastupdated)
        {
            if (!Constants.ElevationModel.IsAnyOf(model))
                throw (new ArgumentException("Unknown elevation model: " + model));
            if ((lat > 90) || (lat < -90))
                throw (new ArgumentOutOfRangeException("Latitude out of range: " + lat.ToString("F8")));
            if ((lon > 180) || (lat < -180))
                throw (new ArgumentOutOfRangeException("Longitude out of range: " + lon.ToString("F8")));
            if (height < 0)
                throw (new ArgumentOutOfRangeException("Height out of range: " + height.ToString("F8")));
            if (clearance < 0)
                throw (new ArgumentOutOfRangeException("Clearance out of range: " + clearance.ToString("F8")));
            Model = model;
            Lat = lat;
            Lon = lon;
            Height = height;
            Clearance = clearance;
            Horizon = horizon;
            LastUpdated = lastupdated;
        }

        public override string ToString()
        {
            return Model + ";" +
                Math.Round(Lat,4).ToString("F4", CultureInfo.InvariantCulture) + ";" +
                Math.Round(Lon,4).ToString("F4", CultureInfo.InvariantCulture) + ";" +
                Math.Round(Height,0).ToString("F0", CultureInfo.InvariantCulture) + ";" +
                Math.Round(Clearance,0).ToString("F0", CultureInfo.InvariantCulture) + ";" +
                Horizon.ToString();
        }

        public string[] ToArray()
        {
            string[] a = new string[6];
            a[0] = Model;
            a[1] = Lat.ToString("F8", CultureInfo.InvariantCulture);
            a[2] = Lon.ToString("F8", CultureInfo.InvariantCulture);
            a[3] = Height.ToString("F8", CultureInfo.InvariantCulture);
            a[4] = Clearance.ToString("F8", CultureInfo.InvariantCulture);
            a[5] = JsonConvert.SerializeObject(Horizon.ToArray());
            return a;
        }
    }

    [Serializable]
    public class Horizon : Object
    {
        private double[] Hor = new double[360];

        public double this[int i]
        {
            get
            {
                return Hor[i];
            }
            set
            {
                Hor[i] = value;
            }
        }

        public Horizon()
        {
            for (int i = 0; i < Hor.Length; i++)
                Hor[i] = double.MinValue;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Hor.Length; i++)
            {
                s = s + Hor[i].ToString("F8", CultureInfo.InvariantCulture);
                if (i < Hor.Length - 1)
                    s = s + ";";
            }
            return s;
        }

        public string[] ToArray()
        {
            string[] a = new string[Hor.Length];
            for (int i = 0; i < a.Length; i++)
                a[i]= Hor[i].ToString("F8", CultureInfo.InvariantCulture);
            return a;
        }
    }

}
