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
    /// Holds the airline information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class AirlineDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tabale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "Airlines";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string ICAO { get; set; }
        public string IATA { get; set; }
        public string Airline { get; set; }
        public string Country { get; set; }

        public AirlineDesignator()
        {
            ICAO = "";
            IATA = "";
            Airline = "";
            Country = "";
            LastUpdated = DateTime.MinValue;
        }

        public AirlineDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public AirlineDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public AirlineDesignator(string icao, string iata) : this(icao, iata,"", "", DateTime.UtcNow) { }
        public AirlineDesignator(string icao, string iata, string airline, string country) : this(icao, iata, airline, country, DateTime.UtcNow) { }
        public AirlineDesignator(string icao, string iata, string airline, string country, DateTime lastupdated) : this()
        {
            ICAO = icao;
            IATA = iata;
            Airline = airline;
            Country = country;
            LastUpdated = lastupdated;
        }

    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableAirlines : DataTable
    {
        public DataTableAirlines()
            : base()
        {
            // set table name
            TableName = "Airlines";
            // create all specific columns
            DataColumn IATA = this.Columns.Add("IATA", typeof(string));
            DataColumn ICAO = this.Columns.Add("ICAO", typeof(string));
            DataColumn Airline = this.Columns.Add("Airline", typeof(string));
            DataColumn Country = this.Columns.Add("Country", typeof(string));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[2];
            keys[0] = IATA;
            keys[1] = ICAO;
            this.PrimaryKey = keys;
        }

        public DataTableAirlines(List<AirlineDesignator> ads)
            : this()
        {
            foreach (AirlineDesignator ad in ads)
            {
                DataRow row = this.NewRow();
                row[0] = ad.ICAO;
                row[1] = ad.IATA;
                row[2] = ad.Airline;
                row[3] = ad.Country;
                row[4] = ad.LastUpdated;
                this.Rows.Add(row);
            }
        }

    }
}
