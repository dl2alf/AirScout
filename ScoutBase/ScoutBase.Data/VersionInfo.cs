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
using ScoutBase.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScoutBase.Data
{

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableVersionInfo : DataTable
    {
        public DataTableVersionInfo()
            : base()
        {
            // set table name
            TableName = "VersionInfo";
            // create all specific columns
            DataColumn Version = this.Columns.Add("Version", typeof(string));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = Version;
            this.PrimaryKey = keys;
        }
    }

    public class VersionInfo : Object
    {
        public string Version = "";
        public DateTime LastUpdated = DateTime.UtcNow;

        public void ReadFromTable(DataTable dt)
        {
            if (dt.Rows.Count <= 0)
                return;
            Version = dt.Rows[0]["Version"].ToString();
            try
            {
                LastUpdated = System.Convert.ToDateTime(dt.Rows[0]["LastUpdated"].ToString(), CultureInfo.InvariantCulture);
            }
            catch
            {
                LastUpdated = DateTime.UtcNow;
            }
        }

        public DataTable WriteToTable()
        {
            DataTableVersionInfo dt = new DataTableVersionInfo();
            DataRow row = dt.NewRow();
            row["Version"] = Version;
            row["LastUpdated"] = LastUpdated.ToString("u");
            dt.Rows.Add(row);
            return dt;
        }

    }
}


 
