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
    /// Holds the aircraft registration information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class AircraftRegistrationDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tabale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "AircraftRegistrations";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Prefix { get; set; }
        public string Country { get; set; }
        public string Remarks { get; set; }

        public AircraftRegistrationDesignator()
        {
            Prefix = "";
            Country = "";
            Remarks = "";
            LastUpdated = DateTime.MinValue;
        }

        public AircraftRegistrationDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public AircraftRegistrationDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public AircraftRegistrationDesignator(string prefix) : this(prefix, "", "", DateTime.UtcNow) { }
        public AircraftRegistrationDesignator(string prefix, string country, string remarks) : this(prefix, country, remarks, DateTime.UtcNow) { }
        public AircraftRegistrationDesignator(string prefix, string country, string remarks, DateTime lastupdated) : this()
        {
            Prefix = prefix;
            Country = country;
            Remarks = remarks;
            LastUpdated = lastupdated;
        }

    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableAircraftRegistrations : DataTable
    {
        public DataTableAircraftRegistrations()
            : base()
        {
            // set table name
            TableName = "AircraftRegistrations";
            // create all specific columns
            DataColumn Prefix = this.Columns.Add("Prefix", typeof(string));
            DataColumn Country = this.Columns.Add("Country", typeof(string));
            DataColumn Remarks = this.Columns.Add("Remarks", typeof(string));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = Prefix;
            this.PrimaryKey = keys;
        }

        public DataTableAircraftRegistrations(List<AircraftRegistrationDesignator> ads)
            : this()
        {
            foreach (AircraftRegistrationDesignator ad in ads)
            {
                DataRow row = this.NewRow();
                row[0] = ad.Prefix;
                row[1] = ad.Country;
                row[2] = ad.Remarks;
                row[3] = ad.LastUpdated;
                this.Rows.Add(row);
            }
        }

    }
}
