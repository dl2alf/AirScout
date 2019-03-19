using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using System.Data.SQLite;
using ScoutBase.Core;
using ScoutBase.Elevation;
using Newtonsoft.Json;

namespace ScoutBase.Propagation
{
    /// <summary>
    /// Holds the horizon information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class PropagationHorizonDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // individual static SQL strings per class will be created on first use
        // add a "new" statement on each derived class to confirm hiding of the base class members
        // update the tbale name to the table name according to the class
        // update the PrimaryKeys collection according to the class, crreate an empty list if no primary key
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "PropagationHorizon";


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public double Lat { get; set; }
        public double Lon { get; set; }
        public double h { get; set; }
        public double Dist { get; set; }
        public double StepWidth { get; set; }
        public double QRG { get; set; }
        public double F1_Clearance { get; set; }
        public double Radius { get; set; }
        public HorizonPoint[] Horizon {get; set;}

        // MEMBERS ONLY TO STORE STATUS TEMPORARLY --> NOT STORED IN DATABASE
        // horizon status: valid / invalid
        public bool Valid = true;
        // horizon status: selected / not selected
        public bool Selected = false;
        // horizon status local obstructed / not obstructed
        public bool LocalObstructed = false;
        public LocalObstructionDesignator LocalObstruction;

        public PropagationHorizonDesignator()
        {
            Lat = 0;
            Lon = 0;
            h = 0;
            Dist = 0;
            StepWidth = 0;
            QRG = 0;
            F1_Clearance = 0;
            Radius = 0;
            Horizon = new HorizonPoint[360];
            for (int i = 0; i < Horizon.Length; i++)
                Horizon[i] = new HorizonPoint(0, 0, 0);
            LastUpdated = DateTime.MinValue.ToUniversalTime();
        }

        public PropagationHorizonDesignator(DataRow row, LocalObstructionDesignator localobstruction) : this()
        {
            FillFromDataRow(row);
            LocalObstruction = localobstruction;
            if (Horizon == null)
                return;
            if (LocalObstruction == null)
                return;
            for (int i = 0; i > 360; i++)
            {
                if ((Horizon[i] != null) && (Horizon[i].Epsmin < LocalObstruction.GetObstruction(h, i)))
                    LocalObstructed = true;
            }
        }

        public PropagationHorizonDesignator(IDataRecord record, LocalObstructionDesignator localobstruction) : this()
        {
            FillFromDataRecord(record);
            LocalObstruction = localobstruction;
            if (Horizon == null)
                return;
            if (LocalObstruction == null)
                return;
            for (int i = 0; i > 360; i++)
            {
                if ((Horizon[i] != null) && (Horizon[i].Epsmin < LocalObstruction.GetObstruction(h, i)))
                    LocalObstructed = true;
            }
        }


        public PropagationHorizonDesignator(double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, LocalObstructionDesignator localobstruction) :
            this(lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth, null, DateTime.UtcNow, localobstruction) { }
        public PropagationHorizonDesignator(double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, HorizonPoint[] horizon, LocalObstructionDesignator localobstruction) :
            this(lat, lon, h, dist, qrg, radius, f1_clearance, stepwidth, horizon, DateTime.UtcNow, localobstruction) { }
        public PropagationHorizonDesignator(double lat, double lon, double h, double dist, double qrg, double radius, double f1_clearance, double stepwidth, HorizonPoint[] horizon, DateTime lastupdated, LocalObstructionDesignator localobstruction)
        {
            Lat = lat;
            Lon = lon;
            this.h = h; 
            Dist = dist;
            StepWidth = stepwidth;
            QRG = qrg;
            F1_Clearance = f1_clearance;
            Radius = radius;
            if (horizon == null)
            {
                // initialize new horizon array if null
                Horizon = new HorizonPoint[360];
                for (int i = 0; i < Horizon.Length; i++)
                    Horizon[i] = new HorizonPoint(0, 0, 0);
            }
            else
                Horizon = horizon;
            LocalObstruction = localobstruction;
            LastUpdated = lastupdated;
            if (Horizon == null)
                return;
            if (LocalObstruction == null)
                return;
            for (int i = 0; i > 360; i++)
            {
                if ((Horizon[i] != null) && (Horizon[i].Epsmin < LocalObstruction.GetObstruction(h, i)))
                    LocalObstructed = true;
            }
        }

        /// <summary>
        /// Returns the maximum elevation angle Epsilon of all objects on an elevation path are seen from an observer(h1)
        /// If the maxixum elevation angle is higher than an object with maxelv could reach the path is cut to this length
        /// </summary>
        /// <param name="h1">The height of the observer [m].</param>
        /// <param name="elv">The array with elevation data [m].</param>
        /// <param name="dist">The full path distance [km].</param>
        /// <param name="stepwidth">The stepwidth in [m].</param>
        /// <param name="freq">The frequency in [GHz].</param>
        /// <param name="f1_clearance">The Fresenl Zone F1 clearance [].</param>
        /// <param name="maxelv">The maximal elevation an object could have [m].</param>
        /// <param name="re">The equivalent earth radius [km].</param>
        /// <returns>The HorizonPoint with distance, epsmax and elevation.</returns>
        public static HorizonPoint EpsilonMaxFromPath(double h1, ref short[] elv, double dist, double stepwidth, double freq, double f1_clearance, double maxelv, double re)
        {
            HorizonPoint hp = new HorizonPoint();
            stepwidth = stepwidth / 1000.0;
            double d = 0;
            for (int i = 0; i < elv.Length; i++)
            {
                double f1 = ScoutBase.Core.Propagation.F1Radius(freq, d, dist - d);
                double h2 = elv[i] + f1 * f1_clearance;
                double eps = ScoutBase.Core.Propagation.EpsilonFromHeights(h1, d, h2, re);
                if (eps > hp.Epsmin)
                {
                    // ignore first 100m
                    if (d > 0.1)
                    {
                        hp.Epsmin = eps;
                        hp.Dist = d;
                        hp.Elv = elv[i];
                    }
                }
                if (maxelv > 0)
                {
                    double epsmax = ScoutBase.Core.Propagation.EpsilonFromHeights(h1, d, maxelv, re);
                    if (epsmax < hp.Epsmin)
                    {
                        Array.Resize<short>(ref elv, i);
                        return hp;
                    }
                }
                d += stepwidth;
            }
            return hp;
        }


        public static HorizonPoint GetHorizon(double lat, double lon, double qtf, double qrg, double k_factor, double f1_clearance, double clearance, double stepwidth, ELEVATIONMODEL model)
        {
            return null;
        }
    }
}
