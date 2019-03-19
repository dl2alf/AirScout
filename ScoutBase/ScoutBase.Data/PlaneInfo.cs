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
using SphericalEarth;
using AirScout.Database.Aircrafts;
using System.Text.RegularExpressions;

namespace AirScout.Database.Core
{
    public class PlaneInfo
    {
        public DateTime Time = DateTime.UtcNow;
        public string Call = "";
        public string Reg = "";
        public string Hex = "";
        public double Lat = 0;
        public double Lon = 0;
        public int Alt = 0;
        public int Alt_m = 0;
        public int Track = 0;
        public int Speed = 0;
        public int Squawk = 0;
        public string Radar = "";
        public string Type = "";
        public string Manufacturer = "";
        public string Model = "";
        public string Category = "";

        public LatLon.GPoint IntPoint = null;
        public double IntQRB = double.MaxValue;
        public double AltDiff = 0;
        public int Potential = 0;
        public double Eps1 = 0;
        public double Eps2 = 0;
        public double Squint = 0;
        public double SignalStrength = double.MinValue;


        public PlaneInfo()
        {
            Time = DateTime.UtcNow;
        }

        public PlaneInfo(DateTime time, string call, string reg, string hex, double lat, double lon, int track, int alt, int speed, int squawk, string radar, string type, string manufacturer, string model, string category)
        {
            Time = time.ToUniversalTime();
            Call = call;
            Reg = reg;
            Hex = hex;
            Lat = lat;
            Lon = lon;
            Track = track;
            Alt = alt;
            Alt_m = (int)((double)Alt / 3.28084);
            Speed = speed;
            Squawk = squawk;
            Radar = radar;
            Type = type;
            Manufacturer = manufacturer;
            Model = model;
            Category = category;
        }

        public override string ToString()
        {
            return "PlaneInfo\n" +
                "Hex:   " + this.Hex + "\n" +
                "Reg:   " + this.Reg + "\n" +
                "Call:  " + this.Call + "\n" +
                "Type:  " + this.Type + "\n" +
                "Lat:   " + this.Lat.ToString("F8") + "\n" +
                "Lon:   " + this.Lon.ToString("F8") + "\n" +
                "Alt_m: " + this.Alt_m.ToString() + "\n" +
                "Speed: " + this.Speed.ToString();
        }
    }
    public class PlaneInfoConverter
    {

        public PlaneInfoConverter()
        {
        }

        private bool IsHex(string s)
        {
            bool b;
            try
            {
                b = !s.ToCharArray().Any(c => !"0123456789abcdefABCDEF".Contains(c));
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }

        private bool IsInt(string s)
        {
            bool b;
            try
            {
                b = !s.ToCharArray().Any(c => !"0123456789".Contains(c));
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }

        private bool IsDouble(string s)
        {
            bool b;
            try
            {
                // double should contain only following chars
                b = !s.ToCharArray().Any(c => !"+-.,E0123456789".Contains(c));
                // double must contain a decimal separator
                if (b)
                {
                    b = s.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }

        private bool IsOct(string s)
        {
            bool b;
            try
            {
                b = !s.ToCharArray().Any(c => !"01234567".Contains(c));
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }


        public DateTime To_UTC(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            // UTC must be a 10 character value
            if (s.Length < 10)
                return DateTime.MinValue;
            // try to convert UNIX time first
            if (IsInt(s))
            {
                try
                {
                    // try to convert to UTC timestamp
                    long l = System.Convert.ToInt64(s);
                    DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0,DateTimeKind.Utc);
                    timestamp = timestamp.AddSeconds(l);
                    return timestamp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
                }
            }
            // check for Standard DateTime notation
            try
            {
                // try to convert to UTC timestamp
                DateTime timestamp;
                if (DateTime.TryParse(s, out timestamp))
                    return timestamp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return DateTime.MinValue;
        }

        public string To_Hex(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            // Hex must be a 6 character value 
            if (s.Length != 6)
                return null;
            if (!IsHex(s))
                return null;
            try
            {
                // try to convert to Hex value
                long hex = System.Convert.ToInt64(s, 16);
                // check boundaries
                if ((hex < 0) || (hex > 16777215))
                    return null;
                return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Reg(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            if (s.Length < AircraftRegistrationDirectory.Min_Length+1)
                return null;
            if (!s.Contains('-') && !s.StartsWith("N"))
                return null;
            try
            {
                // try to find the registration string in aircraft registration database
                AircraftRegistrationDesignator reg = AircraftRegistrationDirectory.Find(s);
                if (reg != null)
                    return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Flight(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            if (s.Length < 3)
                return null;
            string iata = s.Substring(0, 2);
            string flightnumber = s.Substring(2);
            if (!IsInt(flightnumber))
                return null;
            try
            {
                // try to find the string in IATA airline database
                AirlineDesignator airline = AirlineDirectory.FindByIATA(iata);
                if (airline != null)
                    return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Type(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            if (s.Length < AircraftTypeDirectory.Min_Length_ICAO)
                return null;
            try
            {
                // try to find the string in aircraft registration database
                AircraftTypeDesignator type = AircraftTypeDirectory.FindByICAO(s);
                if (type != null)
                    return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Radar(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            if (s.Length < 3)
                return null;
            try
            {
                // Radars will have a letter in first char and "-" as second char
                if (Char.IsLetter(s[0]) && (s[1] == '-'))
                    return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Route(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            // Routes will have a "-" between two IATA airport codes
            if (s.Length < 9)
                return null;
            int index = s.IndexOf('-');
            if ((index < 3) || (index > s.Length-3))
                return null;
            string from = s.Substring(0, index - 1);
            string to = s.Substring(index + 1);
            try
            {
                if (AirportDirectory.FindByICAO(from) == null)
                    return null;
                if (AirportDirectory.FindByICAO(to) == null)
                    return null;
                return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Call(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            // check length
            if (s.Length < 4)
                return null;
            if (s.Contains('-'))
                return null;
            // check for numeric flight number
            string icao = s.Substring(0, 3);
            try
            {
                // try to find the string in aircraft registration database
                AirlineDesignator airline = AirlineDirectory.FindByICAO(icao);
                if ( airline != null)
                    return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public int To_Squawk(string s)
        {
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            // check length
            if (s.Length != 4)
                return int.MinValue;
            if (!IsOct(s))
                return int.MinValue;
            try
            {
                // check for octal number
                // try to convert to Hex value
                long oct = System.Convert.ToInt64(s, 8);
                // check boundaries
                if ((oct >= 0) && (oct <= 4095))
                    return (int)oct;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return int.MinValue;
        }

        public double To_Lat(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            if (s.Length < 3)
                return double.MinValue;
            // double Lon must contain a decimal separator
            if (s.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) < 0)
                return double.MinValue;
            try
            {
                // try to convert to double
                double d = System.Convert.ToDouble(s);
                // check bounds
                if ((d >= -90.0) && (d <= 90.0))
                    return d;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return double.MinValue;
        }


        public double To_Lon(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            if (s.Length < 3)
                return double.MinValue;
            // double Lon must contain a decimal separator
            if (s.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) < 0)
                return double.MinValue;
            try
            {
                // try to convert to double
                double d = System.Convert.ToDouble(s);
                // check bounds
                if ((d >= -180.0) && (d <= 180.0))
                    return d;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return double.MinValue;
        }


        public int To_Alt(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            if (s.Length > 5)
                return int.MinValue;
            if (!IsInt(s))
                return int.MinValue;
            try
            {
                // try to convert to integer
                long alt = System.Convert.ToInt64(s);
                // check bounds
                if ((alt >= 0) && (alt <= 100000))
                    return (int)alt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return int.MinValue;
        }

        public int To_Track(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            if (s.Length > 3)
                return int.MinValue;
            if (!IsInt(s))
                return int.MinValue;
            try
            {
                // try to convert to integer
                long track = System.Convert.ToInt64(s);
                // check bounds
                if ((track >= 0) && (track < 360))
                    return (int)track;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return int.MinValue;
        }

        public int To_Speed(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            if (s.Length > 3)
                return int.MinValue;
            if (!IsInt(s))
                return int.MinValue;
            try
            {
                // try to convert to integer
                long speed = System.Convert.ToInt64(s);
                // check bounds
                if ((speed >= 0) && (speed <= 800))
                    return (int)speed;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return int.MinValue;
        }

        public static bool Check(ref PlaneInfo info)
        {
            // checks a PlaneInfo object
            // tries to complete missing info from aircraft database
            // returns TRUE if contains all mandantory data
            if ((info.Time == null) ||
                (info.Time == DateTime.MinValue) ||
                (info.Time == DateTime.MaxValue) ||
                (info.Lat == double.MinValue) ||
                (info.Lat == double.MaxValue) ||
                (info.Lon == double.MinValue) ||
                (info.Lon == double.MaxValue) ||
                (info.Alt == int.MinValue) ||
                (info.Alt == int.MaxValue) ||
                (info.Track == int.MinValue) ||
                (info.Track == int.MaxValue) ||
                (info.Speed == int.MinValue) ||
                (info.Speed == int.MaxValue) ||
                String.IsNullOrEmpty(info.Call)
            )
                return false;
            return true;
        }
       
    }
}