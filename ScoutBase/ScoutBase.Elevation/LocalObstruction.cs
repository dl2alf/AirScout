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
using ScoutBase.Stations;

namespace ScoutBase.Elevation
{

    /// <summary>
    /// Holds the local obstruction information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class LocalObstructionDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tbale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "LocalObstruction";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public double Lat { get; private set; }
        public double Lon { get; private set; }
        public double[] Distance { get; private set; }
        public double[] Height { get; private set; }


        public LocalObstructionDesignator()
        {
            Lat = 0;
            Lon = 0;
            Distance = new double[360];
            Height = new double[360];
            for (int i = 0; i < 360; i++)
            {
                Distance[i] = 0;
                Height[i] = 0;
            }
            LastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a local obstruction designator from a data table
        /// </summary>
        /// <param name="obstr">The data table to create from</param>
        public LocalObstructionDesignator(double lat, double lon, DataTableLocalObstructions obstr) : this()
        {
            foreach (DataRow row in obstr.Rows)
            {
                int i = System.Convert.ToInt32(row["Bearing"]);
                this.Distance[i] = System.Convert.ToDouble(row["Distance"]);
                this.Height[i] = System.Convert.ToDouble(row["Height"]);
            }
            Lat = lat;
            Lon = lon;
        }

        public LocalObstructionDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public LocalObstructionDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public LocalObstructionDesignator(double lat, double lon) : this(lat, lon, null, null, DateTime.UtcNow) { }
        public LocalObstructionDesignator(double lat, double lon, double[] distance, double[] height) : this(lat, lon, distance, height, DateTime.UtcNow) { }
        public LocalObstructionDesignator(double lat, double lon, double[] distance, double[] height, DateTime lastupdated) : this()
        {
            Lat = lat;
            Lon = lon;
            if (distance != null)
                Distance = distance;
            else
            {
                Distance = new double[360];
                for (int i = 0; i < 360; i++)
                    Distance[i] = 0;
            }
            if (height != null)
                Height = height;
            else
            {
                Height = new double[360];
                for (int i = 0; i < 360; i++)
                    Distance[i] = 0;
            }
        }

        public void SetObstruction (int deg, double dist, double height)
        {

        }

        public double GetDistance (int deg)
        {
            return Distance[deg];
        }

        public double GetHeight (int deg)
        {
            return Height[deg];
        }


        /// <summary>
        /// Gets a local obstruction at bearing, if any.
        /// </summary>
        /// <param name="myheight">The bearing.</param>
        /// <param name="bearing">The bearing.</param>
        /// <returns>The local obstruction, double.MinValue if no obstruction found.</returns>
        public double GetObstruction(double myheight, double bearing)
        {
            // assume that local really means local and simply calculate epsilon using trigonometric functions
            double eps = Math.Atan((GetHeight((int)bearing) - myheight) / GetDistance((int)bearing));
            return eps;
        }


    }

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableLocalObstructions : DataTable
    {
        public DataTableLocalObstructions()
            : base()
        {
            // set table name
            TableName = "LocalObstructions";
            // create all specific columns
            DataColumn Bearing = this.Columns.Add("Bearing", typeof(int));
            DataColumn Distance = this.Columns.Add("Distance", typeof(double));
            DataColumn Height = this.Columns.Add("Height", typeof(double));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = Bearing;
            this.PrimaryKey = keys;
        }

        public DataTableLocalObstructions(LocalObstructionDesignator obstr)
            : this()
        {
            for (int i = 0; i < 360; i++)
            {
                DataRow row = this.NewRow();
                row["Bearing"] = i;
                row["Distance"] = (obstr != null) ? obstr.GetDistance(i) : 0;
                row["Height"] = (obstr != null) ? obstr.GetHeight(i) : 0;
                this.Rows.Add(row);
            }
        }
    }
}
