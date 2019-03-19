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
    /// Holds the elevation path information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class ElevationHorizonDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tbale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "ElevationHorizon";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public double Lat { get; private set; }
        public double Lon { get; private set; }

        public double Distance { get; private set; }
        public double StepWidth { get; private set; }
        public int Count { get; private set; }

        public short[][] Paths { get; private set; }

        // MEMBERS ONLY TO STORE STATUS TEMPORARLY --> NOT STORED IN DATABASE
        // path status: valid / invalid
        public bool Valid = true;

        public ElevationHorizonDesignator()
        {
            Lat = 0;
            Lon = 0;
            Distance = 0;
            StepWidth = 0;
            Count = 0;
            Paths = new short[360][];
            for (int i = 0; i < 360; i++)
                Paths[i] = new short[0];
        }

        public ElevationHorizonDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public ElevationHorizonDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }


        public ElevationHorizonDesignator(double lat, double lon, double distance, double stepwidth) :
            this(lat, lon, distance, stepwidth, null, DateTime.UtcNow) { }
        public ElevationHorizonDesignator(double lat, double lon, double distance, double stepwidth, short[][] paths) :
            this(lat, lon, distance, stepwidth, paths, DateTime.UtcNow) { }
        public ElevationHorizonDesignator(double lat, double lon, double distance, double stepwitdh, short[][] paths, DateTime lastupdated) : this()
        {
            Lat = lat;
            Lon = lon;
            Distance = distance;
            StepWidth = stepwitdh;
            LastUpdated = lastupdated;
            Count = (int)(Distance * 1000.0 / StepWidth) + 1;
            if (paths != null)
                Paths = paths;
            else
            {
                Paths = new short[360][];
                for (int i = 0; i < 360; i++)
                    Paths[i] = new short[0];
            }
        }

        public short[] this[int azimuth]
        {
            get
            {
                if ((Paths != null) && (azimuth >= 0) && (azimuth < 360))
                    return Paths[azimuth];
                return null;
            }
            set
            {
                if ((Paths != null) && (azimuth >= 0) && (azimuth < 360))
                    Paths[azimuth] = value;
            }
        }

        /// <summary>
        /// Writes an elevation array into a csv
        /// </summary>
        /// <param name="filename">The filename to be created.</param>
        /// <returns>Nothing.</returns>
        public void ToCSV(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("Azimiuth[deg];Distance[km];Elevation[m]");
                    for (int j = 0; j < 360; j++)
                    {
                        for (int i = 0; i < Paths[j].Length; i++)
                        {
                            sw.WriteLine(j.ToString() + ";" + ((double)i * this.StepWidth / 1000.0).ToString("F8", CultureInfo.InvariantCulture) + ";" + Paths[j][i].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

}
