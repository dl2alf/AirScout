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
    /// Holds the aircraft type information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class AircraftTypeDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tabale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "AircraftTypes";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string IATA { get; set; }
        public string ICAO { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public PLANECATEGORY Category {get; set;}

        public AircraftTypeDesignator()
        {
            LastUpdated = DateTime.MinValue;
        }

        public AircraftTypeDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public AircraftTypeDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public AircraftTypeDesignator(string iata, string icao) : this(iata, icao, "", "", PLANECATEGORY.NONE, DateTime.UtcNow) { }
        public AircraftTypeDesignator(string iata, string icao, string manufacturer, string model, PLANECATEGORY cat) : this(iata, icao, manufacturer, model, cat, DateTime.UtcNow) { }
        public AircraftTypeDesignator(string iata, string icao, string manufacturer, string model, PLANECATEGORY cat, DateTime lastupdated) : this()
        {
            IATA = iata;
            ICAO = icao;
            Manufacturer = manufacturer;
            Model = model;
            Category = cat;
            LastUpdated = lastupdated;
        }

    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableAircraftTypes : DataTable
    {
        public DataTableAircraftTypes()
            : base()
        {
            // set table name
            TableName = "AircraftTypes";
            // create all specific columns
            DataColumn IATA = this.Columns.Add("IATA", typeof(string));
            DataColumn ICAO = this.Columns.Add("ICAO", typeof(string));
            DataColumn Manufacturer = this.Columns.Add("Manufacturer", typeof(string));
            DataColumn Model = this.Columns.Add("Model", typeof(string));
            DataColumn Category = this.Columns.Add("Category", typeof(PLANECATEGORY));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[2];
            keys[0] = IATA;
            keys[1] = ICAO;
            this.PrimaryKey = keys;
        }

        public DataTableAircraftTypes(List<AircraftTypeDesignator> ads)
            : this()
        {
            foreach (AircraftTypeDesignator ad in ads)
            {
                DataRow row = this.NewRow();
                row[0] = ad.IATA;
                row[1] = ad.ICAO;
                row[2] = ad.Manufacturer;
                row[3] = ad.Model;
                row[4] = ad.Category;
                row[5] = ad.LastUpdated;
                this.Rows.Add(row);
            }
        }

    }
}
