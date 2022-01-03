// Formulas courtesy of http://www.movable-type.co.uk/scripts/latlong.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SphericalEarth
{
    public class LatLon
    {

        public class Earth
        {
            public static double Radius = 6371;
        }

        public class GPoint : Object
        {
            public double Lat;
            public double Lon;

            public GPoint()
            {
                Lat = Double.NaN;
                Lon = Double.NaN;
            }

            public GPoint(double lat, double lon)
            {
                Lat = lat;
                Lon = lon;
            }
        }

        public class GPath : Object
        {
            public double Lat1;
            public double Lon1;
            public double Lat2;
            public double Lon2;

            public GPath()
            {
                Lat1 = Double.NaN;
                Lon1 = Double.NaN;
                Lat2 = Double.NaN;
                Lon2 = Double.NaN;
            }

            public GPath(double lat1, double lon1, double lat2, double lon2)
            {
                Lat1 = lat1;
                Lon1 = lon1;
                Lat2 = lat2;
                Lon2 = lon2;
            }
        }

        public static double Distance(string myloc, string loc)
        {
            return Distance(Lat(myloc), Lon(myloc), Lat(loc), Lon(loc));
        }

        public static double Distance(double mylat, double mylon, double lat, double lon)
        {
            double R = Earth.Radius;
            double dLat = (mylat - lat);
            double dLon = (mylon - lon);
            double a = Math.Sin(dLat / 180 * Math.PI / 2) * Math.Sin(dLat / 180 * Math.PI / 2) +
                    Math.Sin(dLon / 180 * Math.PI / 2) * Math.Sin(dLon / 180 * Math.PI / 2) * Math.Cos(mylat / 180 * Math.PI) * Math.Cos(lat / 180 * Math.PI);
            return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        public static double Bearing(string myloc, string loc)
        {
            return Bearing(Lat(myloc), Lon(myloc), Lat(loc), Lon(loc));
        }

        public static double Bearing(double mylat, double mylon, double lat, double lon)
        {
            double dLat = (lat - mylat);
            double dLon = (lon - mylon);
            double y = Math.Sin(dLon / 180 * Math.PI) * Math.Cos(lat / 180 * Math.PI);
            double x = Math.Cos(mylat / 180 * Math.PI) * Math.Sin(lat / 180 * Math.PI) - Math.Sin(mylat / 180 * Math.PI) * Math.Cos(lat / 180 * Math.PI) * Math.Cos(dLon / 180 * Math.PI);
            return (Math.Atan2(y, x) / Math.PI * 180 + 360) % 360;
        }

        public static GPoint MidPoint(double mylat, double mylon, double lat, double lon)
        {
            // more accurate spherical midpoint
            double aslatdiff = (mylat - lat);
            double aslondiff = (mylon - lon);
            double bx = Math.Cos(lat / 180 * Math.PI) * Math.Cos(-aslondiff / 180 * Math.PI);
            double by = Math.Cos(lat / 180 * Math.PI) * Math.Sin(-aslondiff / 180 * Math.PI);
            double latm = Math.Atan2(Math.Sin(mylat / 180 * Math.PI) + Math.Sin(lat / 180 * Math.PI), Math.Sqrt((Math.Cos(mylat / 180 * Math.PI) + bx) * (Math.Cos(mylat / 180 * Math.PI) + bx) + by * by)) / Math.PI * 180;
            double lonm = mylon + Math.Atan2(by, Math.Cos(mylat / 180 * Math.PI) + bx) / Math.PI * 180;
            return new GPoint(latm, lonm);
        }

        public static GPoint DestinationPoint(double mylat, double mylon, double bearing, double distance)
        {
            double R = Earth.Radius;
            double lat = Math.Asin(Math.Sin(mylat / 180 * Math.PI) * Math.Cos(distance / R) + Math.Cos(mylat / 180 * Math.PI) * Math.Sin(distance / R) * Math.Cos(bearing / 180 * Math.PI));
            double lon = mylon / 180 * Math.PI + Math.Atan2(Math.Sin(bearing / 180 * Math.PI) * Math.Sin(distance / R) * Math.Cos(mylat / 180 * Math.PI), Math.Cos(distance / R) - Math.Sin(mylat / 180 * Math.PI) * Math.Sin(lat));
            return new GPoint(lat / Math.PI * 180, lon / Math.PI * 180);
        }

        public static GPoint IntersectionPoint(double mylat, double mylon, double mybearing, double lat, double lon, double bearing)
        {
            double dLat = (lat - mylat);
            double dLon = (lon - mylon);
            double brng13 = mybearing / 180 * Math.PI;
            double brng23 = bearing / 180 * Math.PI;
            double brng12 = 0;
            double brng21 = 0;
            double dist12 = 2 * Math.Asin(Math.Sqrt(Math.Sin(dLat / 2 / 180 * Math.PI) * Math.Sin(dLat / 2 / 180 * Math.PI) + Math.Cos(mylat / 180 * Math.PI) * Math.Cos(lat / 180 * Math.PI) * Math.Sin(dLon / 2 / 180 * Math.PI) * Math.Sin(dLon / 2 / 180 * Math.PI)));
            if (dist12 == 0) return null;

            // initial/final bearings between points
            double brngA = Math.Acos((Math.Sin(lat / 180 * Math.PI) - Math.Sin(mylat / 180 * Math.PI) * Math.Cos(dist12)) / (Math.Sin(dist12) * Math.Cos(mylat / 180 * Math.PI)));
            // protect against rounding
            if (Double.IsNaN(brngA))
                brngA = 0;
            double brngB = Math.Acos((Math.Sin(mylat / 180 * Math.PI) - Math.Sin(lat / 180 * Math.PI) * Math.Cos(dist12)) / (Math.Sin(dist12) * Math.Cos(lat / 180 * Math.PI)));
            if (Math.Sin(lon / 180 * Math.PI - mylon / 180 * Math.PI) > 0)
            {
                brng12 = brngA;
                brng21 = 2 * Math.PI - brngB;
            }
            else
            {
                brng12 = 2 * Math.PI - brngA;
                brng21 = brngB;
            }
            double alpha1 = (brng13 - brng12 + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 2-1-3
            double alpha2 = (brng21 - brng23 + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 1-2-3

            if ((Math.Sin(alpha1) == 0) && (Math.Sin(alpha2) == 0))
                return null;  // infinite intersections
            if (Math.Sin(alpha1) * Math.Sin(alpha2) < 0)
                return null;       // ambiguous intersection

            //alpha1 = Math.abs(alpha1);
            //alpha2 = Math.abs(alpha2);
            // ... Ed Williams takes abs of alpha1/alpha2, but seems to break calculation?

            double alpha3 = Math.Acos(-Math.Cos(alpha1) * Math.Cos(alpha2) + Math.Sin(alpha1) * Math.Sin(alpha2) * Math.Cos(dist12));
            double dist13 = Math.Atan2(Math.Sin(dist12) * Math.Sin(alpha1) * Math.Sin(alpha2), Math.Cos(alpha2) + Math.Cos(alpha1) * Math.Cos(alpha3));
            double lat3 = Math.Asin(Math.Sin(mylat / 180 * Math.PI) * Math.Cos(dist13) + Math.Cos(mylat / 180 * Math.PI) * Math.Sin(dist13) * Math.Cos(brng13));
            double dLon13 = Math.Atan2(Math.Sin(brng13) * Math.Sin(dist13) * Math.Cos(mylat / 180 * Math.PI), Math.Cos(dist13) - Math.Sin(mylat / 180 * Math.PI) * Math.Sin(lat3));
            double lon3 = mylon / 180 * Math.PI + dLon13;
            lon3 = (lon3 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalise to -180..+180º

            return new GPoint(lat3 / Math.PI * 180, lon3 / Math.PI * 180);
        }

        public static double Lat(string loc)
        {
            double StepB1 = 10;
            double StepB2 = StepB1 / 10;
            double StepB3 = StepB2 / 24;
            double StartB1 = -90;
            double StartB2 = 0;
            double StartB3 = StepB3 / 2;
            try
            {
                loc = loc.ToUpper();
                if ((loc[1] < 'A') || (loc[1] > 'Z') ||
                    (loc[3] < '0') || (loc[3] > '9') ||
                    (loc[5] < 'A') || (loc[5] > 'X'))
                {
                    return -360;
                }
                return StartB1 + StepB1 * (System.Convert.ToInt16(loc[1]) - 0x41) +
                    StartB2 + StepB2 * (System.Convert.ToInt16(loc[3]) - 0x30) +
                    StartB3 + StepB3 * (System.Convert.ToInt16(loc[5]) - 0x41);
            }
            catch
            {
                // Fehler bei der Breitenberechnung
                return -360;
            }
        }

        public static double Lon(string loc)
        {
            try
            {
                double StepL1 = 20;
                double StepL2 = StepL1 / 10;
                double StepL3 = StepL2 / 24;
                double StartL1 = -180;
                double StartL2 = 0;
                double StartL3 = StepL3 / 2;
                loc = loc.ToUpper();
                if ((loc[0] < 'A') || (loc[0] > 'Z') ||
                    (loc[2] < '0') || (loc[2] > '9') ||
                    (loc[4] < 'A') || (loc[4] > 'X'))
                {
                    return -360;
                }
                return StartL1 + StepL1 * (System.Convert.ToInt16(loc[0]) - 0x41) +
                    StartL2 + StepL2 * (System.Convert.ToInt16(loc[2]) - 0x30) +
                    StartL3 + StepL3 * (System.Convert.ToInt16(loc[4]) - 0x41);
            }
            catch
            {
                // Fehler bei der Längenberechnung
                return -360;
            }
        }

        public static string Loc(double lat, double lon)
        {
            try
            {
                double StepB1 = 10;
                double StepB2 = StepB1 / 10;
                double StepB3 = StepB2 / 24;
                double StartB1 = -90;
                double StartB2 = 0;
                double StartB3 = StepB3 / 2;
                double StepL1 = 20;
                double StepL2 = StepL1 / 10;
                double StepL3 = StepL2 / 24;
                double StartL1 = -180;
                double StartL2 = 0;
                double StartL3 = StepL3 / 2;
                int i0, i1, i2, i3, i4, i5;
                char S0, S1, S2, S3, S4, S5;
                i0 = System.Convert.ToInt32(Math.Floor((lon - StartL1) / StepL1));
                S0 = System.Convert.ToChar(i0 + 0x41);
                lon = lon - i0 * StepL1 - StartL1;
                i2 = System.Convert.ToInt32(Math.Floor((lon - StartL2) / StepL2));
                S2 = System.Convert.ToChar(i2 + 0x30);
                lon = lon - i2 * StepL2 - StartL2;
                i4 = System.Convert.ToInt32((lon - StartL3) / StepL3);
                S4 = System.Convert.ToChar(i4 + 0x41);
                i1 = System.Convert.ToInt32(Math.Floor((lat - StartB1) / StepB1));
                S1 = System.Convert.ToChar(i1 + 0x41);
                lat = lat - i1 * StepB1 - StartB1;
                i3 = System.Convert.ToInt32(Math.Floor((lat - StartB2) / StepB2));
                S3 = System.Convert.ToChar(i3 + 0x30);
                lat = lat - i3 * StepB2 - StartB2;
                i5 = System.Convert.ToInt32((lat - StartB3) / StepB3);
                S5 = System.Convert.ToChar(i5 + 0x41);
                string S = System.String.Concat(S0, S1, S2, S3, S4, S5);
                return S;
            }
            catch
            {
                // Fehler bei der Locatorberechnung
                return "";
            }
        }
    }
}
