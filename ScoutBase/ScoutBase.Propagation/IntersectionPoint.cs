using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.Propagation
{
    public class IntersectionPoint
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double QRB { get; set; }
        public double Min_H { get; set; }
        public double Dist1 { get; set; }
        public double Dist2 { get; set; }


        public IntersectionPoint()
        {
            Lat = 0;
            Lon = 0;
            QRB = 0;
            Min_H = 0;
            Dist1 = 0;
            Dist2 = 0;
        }

        public IntersectionPoint (double lat, double lon, double qrb, double min_h, double dist1, double dist2)
        {
            Lat = lat;
            Lon = lon;
            QRB = qrb;
            Min_H = min_h;
            Dist1 = dist1;
            Dist2 = dist2;
        }
    }
}
