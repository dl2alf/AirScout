using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.Propagation
{
    /// <summary>
    /// Holds a single propagation point information
    /// Lat: The latitude
    /// Lon: The longitude
    /// H1:  The minimuum heigth an object must have at this point to be seen from location 1
    /// H2:  The minimuum heigth an object must have at this point to be seen from location 2
    /// F1:  The Fresnel Zone F1 radius at this point
    /// </summary>
    public class PropagationPoint
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double H1 { get; set; }
        public double H2 { get; set; }
        public double F1 { get; set; }

        public PropagationPoint()
        {
            Lat = 0;
            Lon = 0;
            H1 = 0;
            H2 = 0;
            F1 = 0;
        }

        public PropagationPoint (double lat, double lon, double h1, double h2, double f1)
        {
            Lat = lat;
            Lon = lon;
            H1 = h1;
            H2 = h2;
            F1 = f1;
        }
    }
}
