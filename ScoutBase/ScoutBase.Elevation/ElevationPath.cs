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
    [Serializable]
    public class ElevationPathDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tbale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "ElevationPath";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public double Bearing12
        {
            get
            {
                return LatLon.Bearing(Lat1, Lon1, Lat2, Lon2);
            }
        }

        [JsonIgnore]
        public double Bearing21
        {
            get
            {
                return LatLon.Bearing(Lat2, Lon2, Lat1, Lon1);
            }
        }

        [JsonIgnore]

        public double Distance
        {
            get
            {
                return LatLon.Distance(Lat1, Lon1, Lat2, Lon2);
            }
        }

        public double Lat1 { get; set; }
        public double Lon1 { get; set; }

        public double Lat2 { get; set; }
        public double Lon2 { get; set; }

        public double StepWidth { get; set; }
        public int Count { get; set; }

        public short[] Path { get; set; }

        // MEMBERS ONLY TO STORE STATUS TEMPORARLY --> NOT STORED IN DATABASE
        // path status: valid / invalid
        public bool Valid = true;
        // path status: selected / not selected
        public bool Selected = false;
        // associated location info
        public LocationDesignator Location1 = new LocationDesignator();
        public LocationDesignator Location2 = new LocationDesignator();
        // assoicated qrv info
        public QRVDesignator QRV1 = new QRVDesignator();
        public QRVDesignator QRV2 = new QRVDesignator();

        public ElevationPathDesignator()
        {
            Lat1 = 0;
            Lon1 = 0;
            Lat2 = 0;
            Lon2 = 0;
            Count = 0;
            Path = new short[0];

        }

        public ElevationPathDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public ElevationPathDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public ElevationPathDesignator(double lat1, double lon1, double lat2, double lon2, double stepwidth) :
            this(lat1, lon1, lat2, lon2, stepwidth, null, DateTime.UtcNow) { }
        public ElevationPathDesignator(double lat1, double lon1, double lat2, double lon2, double stepwidth, short[] path) :
            this(lat1, lon1, lat2, lon2, stepwidth, path, DateTime.UtcNow) { }
        public ElevationPathDesignator(double lat1, double lon1, double lat2, double lon2, double stepwitdh, short[] path, DateTime lastupdated) : this()
        {
            Lat1 = lat1;
            Lon1 = lon1;
            Lat2 = lat2;
            Lon2 = lon2;
            StepWidth = stepwitdh;
            LastUpdated = lastupdated;
            Count = (int)(Distance * 1000.0 / StepWidth) + 1;
            if (path != null)
                Path = path;
            else
            {
                Path = new short[Count];
                for (int i = 0; i < Count; i++)
                    Path[i] = ElevationData.Database.ElvMissingFlag;
            }
        }

 
        /// <summary>
        /// Returns an array of 1km-stepped info points for diagram view
        /// </summary>
        /// <returns>Array of info points.</returns>
        public short[] GetInfoPoints()
        {
            if (Path == null)
                return null;
            if (StepWidth <= 0)
                return null;
            if (StepWidth > 1000.0)
                throw new InvalidOperationException("Current path stepwidth must be less or equal 1km: " + (StepWidth * 1000.0).ToString("F3"));
            int infocount = (int)Distance + 1;
            short[] d = new short[infocount];
            for (int i = 0; i < d.Length; i++)
                d[i] = Path[(int)((double)i * (double)Count / Distance)];
            return d;
        }

        /// <summary>
        /// Writes an elevation path into a csv
        /// </summary>
        /// <param name="filename">The filename to be created.</param>
        /// <returns>Nothing.</returns>
        public void ToCSV(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("Distance[km];Elevation[m]");
                    for (int i = 0; i < this.Count; i++)
                    {
                        sw.WriteLine(((double)i * this.StepWidth / 1000.0).ToString("F8", CultureInfo.InvariantCulture) + ";" + Path[i].ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

}
