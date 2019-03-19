// AirScout.Aircrafts.SignalLevelDesignator
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

namespace AirScout.Signals
{
    [DesignerCategory("")]
    public class SignalLevelDesignator : SQLiteEntry
    {
        [JsonIgnore]
        public new static readonly string TableName = "SignalLevels";

        public double Level
        {
            get;
            set;
        }

        public SignalLevelDesignator()
        {
            this.Level = -1.7976931348623157E+308;
            base.LastUpdated = DateTime.MinValue;
        }

        public SignalLevelDesignator(DataRow row)
            : this()
        {
            base.FillFromDataRow(row);
        }

        public SignalLevelDesignator(IDataRecord record)
            : this()
        {
            base.FillFromDataRecord(record);
        }

        public SignalLevelDesignator(DateTime lastupdated)
            : this(-1.7976931348623157E+308, lastupdated)
        {
        }

        public SignalLevelDesignator(double level)
            : this(level, DateTime.UtcNow)
        {
        }

        public SignalLevelDesignator(double level, DateTime lastupdated)
            : this()
        {
            this.Level = level;
            base.LastUpdated = lastupdated;
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableSignalLevels : DataTable
    {
        public DataTableSignalLevels()
            : base()
        {
            // set table name
            TableName = "SignalLevels";
            // create all specific columns
            DataColumn Hex = this.Columns.Add("Level", typeof(double));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = LastUpdated;
            this.PrimaryKey = keys;
        }

        public DataTableSignalLevels(List<SignalLevelDesignator> sds)
            : this()
        {
            foreach (SignalLevelDesignator sd in sds)
            {
                DataRow row = this.NewRow();
                row[0] = sd.Level;
                row[1] = sd.LastUpdated;
                this.Rows.Add(row);
            }
        }
    }
}

