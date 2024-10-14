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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SQLite;
using AirScout.Core;

namespace AirScout.AircraftPositions
{

    /// <summary>
    /// Holds the aircraft information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class AircraftPositionDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tabale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "AircraftPositions";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Hex { get; set; }
        public string Call { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Alt { get; set; }
        public double Track { get; set; }
        public double Speed { get; set; }

        public AircraftPositionDesignator()
        {
            Hex = "";
            Call = "";
            Lat = 0;
            Lon = 0;
            Alt = 0;
            Track = 0;
            Speed = 0;
            LastUpdated = DateTime.MinValue;
        }

        public AircraftPositionDesignator(AircraftPositionDesignator ap)
        {
            Hex = ap.Hex;
            Call = ap.Call;
            Lat = ap.Lat;
            Lon = ap.Lon;
            Alt = ap.Alt;
            Track = ap.Track;
            Speed = ap.Speed;
            LastUpdated = ap.LastUpdated;
        }

        public AircraftPositionDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public AircraftPositionDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public AircraftPositionDesignator(string hex, DateTime lastupdated) : this(hex,"", 0,0,0,0,0, lastupdated) { }
        public AircraftPositionDesignator(string hex, string call, double lat, double lon, double alt, double track, double speed, DateTime lastupdated) : this()
        {
            Hex = hex;
            Call = call;
            Lat = lat;
            Lon = lon;
            Alt = alt;
            Track = track;
            Speed = speed;
            LastUpdated = lastupdated;
        }

    }



    [System.ComponentModel.DesignerCategory("")]
    public class DataTableAircraftPositions : DataTable
    {
        public DataTableAircraftPositions()
            : base()
        {
            // set table name
            TableName = "AircraftPositions";
            // create all specific columns
            DataColumn Hex = this.Columns.Add("Hex", typeof(string));
            DataColumn Call = this.Columns.Add("Call", typeof(string));
            DataColumn Lat = this.Columns.Add("Lat", typeof(double));
            DataColumn Lon = this.Columns.Add("Lon", typeof(double));
            DataColumn Alt = this.Columns.Add("Alt", typeof(double));
            DataColumn Track = this.Columns.Add("Track", typeof(double));
            DataColumn Speed = this.Columns.Add("Speed", typeof(double));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(DateTime));
            // create primary key
            DataColumn[] keys = new DataColumn[2];
            keys[0] = Hex;
            keys[1] = LastUpdated;
            this.PrimaryKey = keys;
        }

        public DataTableAircraftPositions(List<AircraftPositionDesignator> aps)
            : this()
        {
            foreach (AircraftPositionDesignator ad in aps)
            {
                DataRow row = this.NewRow();
                row[0] = ad.Hex;
                row[1] = ad.Hex;
                row[2] = ad.Lat;
                row[3] = ad.Lon;
                row[4] = ad.Alt;
                row[5] = ad.Track;
                row[6] = ad.Speed;
                row[7] = ad.LastUpdated;
                this.Rows.Add(row);
            }
        }


    }
}
