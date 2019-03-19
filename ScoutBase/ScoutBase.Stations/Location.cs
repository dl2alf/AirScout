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
using System.Data.SQLite;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ScoutBase.Core;

namespace ScoutBase.Stations
{

    /// <summary>
    /// Holds the station location information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    [Serializable]
    public class LocationDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tabale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "Location";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Call { get; set; }
        public string Loc { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public GEOSOURCE Source { get; set; }
        public int Hits { get; set; }

        [JsonIgnore]
        public LatLon.GPoint GeoLocation
        {
            get
            {
                return new LatLon.GPoint(Lat, Lon);
            }
        }

        // keeps elevation value, not saved in database
        public short Elevation;
        public bool BestCaseElevation;

        public LocationDesignator()
        {
            Call = "";
            Loc = "";
            Lat = 0;
            Lon = 0;
            Source = GEOSOURCE.UNKONWN;
            Hits = 0;
            Elevation = 0;
            BestCaseElevation = false;
            LastUpdated = DateTime.MinValue;
        }

        public LocationDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public LocationDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public LocationDesignator(string call) : this(call, MaidenheadLocator.LocFromLatLon(0,0, false, 3), 0, 0, ScoutBase.Core.GEOSOURCE.UNKONWN, 0, DateTime.UtcNow) { }
        public LocationDesignator(string call, string loc) : this(call, loc, MaidenheadLocator.LatFromLoc(loc), MaidenheadLocator.LonFromLoc(loc), ScoutBase.Core.GEOSOURCE.FROMLOC, 0, DateTime.UtcNow) { }
        public LocationDesignator(string call, string loc, GEOSOURCE source) : this(call, loc, MaidenheadLocator.LatFromLoc(loc), MaidenheadLocator.LonFromLoc(loc), source, 0, DateTime.UtcNow) { }
        public LocationDesignator(string call, double lat, double lon) : this(call, MaidenheadLocator.LocFromLatLon(lat, lon, false, 3), lat, lon, GEOSOURCE.FROMUSER, 0, DateTime.UtcNow) { }
        public LocationDesignator(string call, double lat, double lon, GEOSOURCE source) : this(call, MaidenheadLocator.LocFromLatLon(lat,lon, false, 3), lat, lon, source, 0, DateTime.UtcNow) { }

        public LocationDesignator(string call, string loc, double lat, double lon, GEOSOURCE source, int hits, DateTime lastupdated) : this()
        {
            Call = call.ToUpper().Trim();
            Loc = loc;
            Lat = lat;
            Lon = lon;
            Source = source;
            Hits = hits;
            Elevation = 0;
            BestCaseElevation = false;
            LastUpdated = lastupdated;
        }

    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableLocations : DataTable
    {
        public DataTableLocations()
            : base()
        {
            // set table name
            TableName = "Locations";
            // create all specific columns
            DataColumn Call = this.Columns.Add("Call", typeof(string));
            DataColumn Loc = this.Columns.Add("Loc", typeof(string));
            DataColumn Lat = this.Columns.Add("Lat", typeof(double));
            DataColumn Lon = this.Columns.Add("Lon", typeof(double));
            DataColumn Source = this.Columns.Add("Source", typeof(GEOSOURCE));
            DataColumn Hits = this.Columns.Add("Hits", typeof(int));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(DateTime));
            // create primary key
            DataColumn[] keys = new DataColumn[2];
            keys[0] = Call;
            keys[1] = Loc;
            this.PrimaryKey = keys;
        }

        public DataTableLocations(List<LocationDesignator> lds)
            : this()
        {
            foreach (LocationDesignator ld in lds)
            {
                DataRow row = this.NewRow();
                row[0] = ld.Call;
                row[1] = ld.Loc;
                row[2] = ld.Lat;
                row[3] = ld.Lon;
                row[4] = ld.Source;
                row[5] = ld.Hits;
                row[6] = ld.LastUpdated;
                this.Rows.Add(row);
            }
        }
    }

}
