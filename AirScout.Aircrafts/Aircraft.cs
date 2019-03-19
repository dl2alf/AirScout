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
    /// Holds the aircraft information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class AircraftDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tabale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "Aircrafts";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Hex { get; set; }
        public string Call { get; set; }
        public string Reg { get; set; }
        public string TypeCode { get; set; }

        public AircraftDesignator()
        {
            Hex = "";
            Call = "";
            Reg = "";
            TypeCode = "";
            LastUpdated = DateTime.MinValue;
        }

        public AircraftDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public AircraftDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }


        public AircraftDesignator(string hex) : this(hex, "", "", "", DateTime.UtcNow) { }
        public AircraftDesignator(string hex, string call, string reg, string typecode) : this(hex, call, reg, typecode, DateTime.UtcNow) { }
        public AircraftDesignator(string hex, string call, string reg, string typecode, DateTime lastupdated) : this()
        {
            Hex = hex;
            Call = call;
            Reg = reg;
            TypeCode = typecode;
            LastUpdated = lastupdated;
        }

    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableAircrafts : DataTable
    {
        public DataTableAircrafts()
            : base()
        {
            // set table name
            TableName = "Aircrafts";
            // create all specific columns
            DataColumn Hex = this.Columns.Add("Hex", typeof(string));
            DataColumn Call = this.Columns.Add("Call", typeof(string));
            DataColumn Reg = this.Columns.Add("Reg", typeof(string));
            DataColumn TypeCode = this.Columns.Add("TypeCode", typeof(string));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = Hex;
            this.PrimaryKey = keys;
        }

        public DataTableAircrafts(List<AircraftDesignator> ads)
            : this()
        {
            foreach (AircraftDesignator ad in ads)
            {
                DataRow row = this.NewRow();
                row[0] = ad.Hex;
                row[1] = ad.Call;
                row[2] = ad.Reg;
                row[3] = ad.TypeCode;
                row[4] = ad.LastUpdated;
                this.Rows.Add(row);
            }
        }

    }
}
