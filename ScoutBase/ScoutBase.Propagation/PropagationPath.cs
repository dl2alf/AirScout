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
using System.Data;
using System.Data.SQLite;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ScoutBase.Core;
using ScoutBase.Stations;

namespace ScoutBase.Propagation
{

    /// <summary>
    /// Holds the elevation tile information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class PropagationPathDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the tbale name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "PropagationPath";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public double Bearing12
        {
            get
            {
                return LatLon.Bearing(Lat1, Lon1, Lat2, Lon2);
            }
        }

        public double Bearing21
        {
            get
            {
                return LatLon.Bearing(Lat2, Lon2, Lat1, Lon1);
            }
        }


        public double Distance
        {
            get
            {
                return LatLon.Distance(Lat1, Lon1, Lat2, Lon2);
            }
        }

        public double Lat1 { get; set; }
        public double Lon1 { get; set; }
        public double h1 { get; set; }

        public double Lat2 { get; set; }
        public double Lon2 { get; set; }
        public double h2 { get; set; }

        public double QRG { get; set; }
        public double Radius { get; set; }
        public double F1_Clearance { get; set; }
        public double StepWidth { get; set; }

        public double Eps1_Min { get; set; }
        public double Eps2_Min { get; set; }

        // MEMBERS ONLY TO STORE STATUS TEMPORARLY --> NOT STORED IN DATABASE
        // path status: valid / invalid
        public bool Valid = false;
        // path status: selected / not selected
        public bool Selected = false;
        // path status local obstructed / not obstructed
        public bool LocalObstructed = false;
        public double LocalObstruction;
        // associated location info
        public LocationDesignator Location1 = new LocationDesignator();
        public LocationDesignator Location2 = new LocationDesignator();
        // assoicated qrv info
        public QRVDesignator QRV1 = new QRVDesignator();
        public QRVDesignator QRV2 = new QRVDesignator();

        public PropagationPathDesignator()
        {
            Lat1 = 0;
            Lon1 = 0;
            h1 = 0;
            Lat2 = 0;
            Lon2 = 0;
            h2 = 0;
            QRG = 0;
            Radius = 0;
            F1_Clearance = 0;
            StepWidth = 0;
            Eps1_Min = 0;
            Eps2_Min = 0;
            LocalObstruction = 0;
        }

        public PropagationPathDesignator(DataRow row, double localobstruction) : this()
        {
            FillFromDataRow(row);
            Valid = true;
            LocalObstruction = localobstruction;
            if (Eps1_Min < LocalObstruction)
                LocalObstructed = true;
        }

        public PropagationPathDesignator(IDataRecord record, double localobstruction) : this()
        {
            FillFromDataRecord(record);
            Valid = true;
            LocalObstruction = localobstruction;
            if (Eps1_Min < LocalObstruction)
                LocalObstructed = true;
        }

        public PropagationPathDesignator(double lat1, double lon1, double h1, double lat2, double lon2, double h2, double qrg, double radius, double f1_clearance, double stepwidth, double eps1_min, double eps2_min, double localobstruction) :
            this(lat1, lon1, h1, lat2, lon2, h2, qrg, radius, f1_clearance, stepwidth, eps1_min, eps2_min, DateTime.UtcNow, localobstruction){ }
        public PropagationPathDesignator(double lat1, double lon1, double h1, double lat2, double lon2, double h2, double qrg, double radius, double f1_clearance, double stepwidth, double eps1_min, double eps2_min, DateTime lastupdated, double localobstruction)
        {
            Lat1 = lat1;
            Lon1 = lon1;
            this.h1 = h1;
            Lat2 = lat2;
            Lon2 = lon2;
            this.h2 = h2;
            QRG = qrg;
            Radius = radius;
            F1_Clearance = f1_clearance;
            Eps1_Min = eps1_min;
            Eps2_Min = eps2_min;
            StepWidth = stepwidth;
            LastUpdated = lastupdated;
            LocalObstruction = localobstruction;
            if (Eps1_Min < LocalObstruction)
                LocalObstructed = true;
        }

        /// <summary>
        /// Returns an array of 1km-stepped info points for diagram/map view
        /// </summary>
        /// <returns>Array of info points.</returns>
        public PropagationPoint[] GetInfoPoints()
        {
            if (StepWidth <= 0)
                return null;
            PropagationPoint[] d = new PropagationPoint[(int)Distance + 2];
            // chek against localobstruction , if any
            double eps1_min = Math.Max(Eps1_Min, LocalObstruction);
            for (int i = 0; i < d.Length; i++)
            {
                d[i] = GetInfoPoint(i);
            }
            return d;
        }

        /// <summary>
        /// Returns a single info point in a given distance from location 1
        /// </summary>
        /// <param name="lat">The latitude of the plane.</param>
        /// <returns>The info point.</returns>
        public PropagationPoint GetInfoPoint(double dist)
        {
            if (StepWidth <= 0)
                return null;
            // chek against localobstruction , if any
            double eps1_min = Math.Max(Eps1_Min, LocalObstruction);
            LatLon.GPoint p = LatLon.DestinationPoint(Lat1, Lon1, Bearing12, dist);
            return new PropagationPoint(
                p.Lat, 
                p.Lon, 
                ScoutBase.Core.Propagation.HeightFromEpsilon(h1, dist, eps1_min, Radius), 
                ScoutBase.Core.Propagation.HeightFromEpsilon(h2, Distance - dist, Eps2_Min, Radius), 
                ScoutBase.Core.Propagation.F1Radius(QRG, Distance, Distance - dist));
        }


        /// <summary>
        /// Gets the mid point of the propagation path
        /// </summary>
        /// <returns>The mid point of the path.</returns>
        /// <summary>
        public LatLon.GPoint GetMidPoint()
        {
            return LatLon.MidPoint(Lat1, Lon1, Lat2, Lon2);
        }

        /// <summary>
        /// Gets an intersection point of a plane (lat, lon, bearing) with the propagation path in a suitable altitude
        /// </summary>
        /// <param name="lat">The latitude of the plane.</param>
        /// <param name="lon">The longitude of the plane.</param>
        /// <param name="bearing">The bearing of the plane.</param>
        /// <param name="maxdistance">The maximum allowed distance to return a valid intersection (-1: no limits, 0: automatcally adjuts to path distance /2).</param>
        /// <returns>The intersection point, if any. Null if no intersection point exists or plane does not have a suitable altitude at intersection point</returns>
        /// <summary>
        public IntersectionPoint GetIntersectionPoint(double lat, double lon, double bearing, double maxdistance)
        {
            // get an interscetion point with the propagation path
            LatLon.GPoint p = LatLon.IntersectionPoint(Lat1, Lon1, Bearing12, lat, lon, bearing);
            if (p == null)
                // if not, return null
                return null;
            // get the minimum altitude a plane must have at intersection point
            // get both distances to intersection point
            double dist1 = LatLon.Distance(Lat1, Lon1, p.Lat, p.Lon);
            double dist2 = LatLon.Distance(Lat2, Lon2, p.Lat, p.Lon);
            // chek against localobstruction , if any
            double eps1_min = Math.Max(Eps1_Min, LocalObstruction);
            // get minimal altitude
            double min_H = Math.Max(ScoutBase.Core.Propagation.HeightFromEpsilon(h1, dist1, eps1_min, Radius), ScoutBase.Core.Propagation.HeightFromEpsilon(h2, dist2, Eps2_Min, Radius));
            double qrb = LatLon.Distance(p.Lat, p.Lon, lat, lon);
            if (maxdistance < 0)
                return new IntersectionPoint(p.Lat, p.Lon, qrb, min_H, dist1, dist2);
            if ((maxdistance == 0) && (qrb > Distance / 2))
                return null;
            if (qrb < maxdistance)
                return null;
            return new IntersectionPoint(p.Lat, p.Lon, qrb, min_H, dist1, dist2);
        }


        /// Writes an elevation path into a csv
        /// </summary>
        /// <param name="filename">The filename to be created.</param>
        /// <returns>Nothing.</returns>
        public void ToCSV(string filename)
        {
            try
            {
                string separator = SupportFunctions.GetCSVSeparator();
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("Distance[km]" +
                        "Latitude[deg]" + separator +
                        "Longitude[deg]" + separator +
                        "Elevation[m]" + separator +
                        "Eps1_Min[deg]" + separator +
                        "Eps2_Min[m]" + separator +
                        "Min_H1[m]" + separator +
                        "Min_H2[m]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while writing elevation path [" + filename + "]: " + ex.ToString());
            }
        }
    }

}
