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

namespace AirScout.Aircrafts
{
    /// <summary>
    /// Holds the airport information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class AirportDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tabale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "Airports";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string ICAO { get; set; }
        public string IATA { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Alt { get; set; }
        public string Airport { get; set; }
        public string Country { get; set; }

        public AirportDesignator()
        {
            ICAO = "";
            IATA = "";
            Lat = 0;
            Lon = 0;
            Alt = 0;
            Airport = "";
            Country = "";
            LastUpdated = DateTime.MinValue;
        }

        public AirportDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public AirportDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public AirportDesignator(string icao, string iata) : this(icao, iata, 0,0,0, "", "", DateTime.UtcNow) { }
        public AirportDesignator(string icao, string iata, double lat, double lon, double alt, string airport, string country) : this(icao, iata, lat, lon, alt, airport, country, DateTime.UtcNow) { }
        public AirportDesignator(string icao, string iata, double lat, double lon, double alt, string airport, string country, DateTime lastupdated) : this()
        {
            ICAO = icao;
            IATA = iata;
            Lat = lat;
            Lon = lon;
            Alt = alt;
            Airport = airport;
            Country = country;
            LastUpdated = lastupdated;
        }

    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableAirports : DataTable
    {
        public DataTableAirports()
            : base()
        {
            // set table name
            TableName = "Airports";
            // create all specific columns
            DataColumn IATA = this.Columns.Add("ICAO", typeof(string));
            DataColumn ICAO = this.Columns.Add("IATA", typeof(string));
            DataColumn Lat = this.Columns.Add("Lat", typeof(double));
            DataColumn Lon = this.Columns.Add("Lon", typeof(double));
            DataColumn Alt = this.Columns.Add("Alt", typeof(double));
            DataColumn Airport = this.Columns.Add("Airport", typeof(string));
            DataColumn Country = this.Columns.Add("Country", typeof(string));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[2];
            keys[0] = IATA;
            keys[1] = ICAO;
            this.PrimaryKey = keys;
        }

        public DataTableAirports(List<AirportDesignator> pds)
            : this()
        {
            foreach (AirportDesignator pd in pds)
            {
                DataRow row = this.NewRow();
                row[0] = pd.ICAO;
                row[1] = pd.IATA;
                row[2] = pd.Lat;
                row[3] = pd.Lon;
                row[4] = pd.Alt;
                row[5] = pd.Airport;
                row[6] = pd.Country;
                row[7] = pd.LastUpdated;
                this.Rows.Add(row);
            }
        }

    }
}
