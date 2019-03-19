using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using AirScout.Aircrafts;
using ScoutBase.Core;

namespace AirScout.Aircrafts
{
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
            if (String.IsNullOrEmpty(s))
                return DateTime.MinValue;
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
                    DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
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
            if (String.IsNullOrEmpty(s))
                return null;
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
            if (String.IsNullOrEmpty(s))
                return null;
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            if (s.Length < AircraftData.Database.AircraftRegistrationMinLength + 1)
                return null;
            if (!s.Contains('-') && !s.StartsWith("N"))
                return null;
            try
            {
                // try to find the registration string in aircraft registration database
                AircraftRegistrationDesignator reg = AircraftData.Database.AircraftRegistrationFindByReg(s);
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
            if (String.IsNullOrEmpty(s))
                return null;
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
                AirlineDesignator airline = AircraftData.Database.AirlineFindByIATA(iata);
                if (airline != null)
                    return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Type(string s, bool checktype = true)
        {
            if (String.IsNullOrEmpty(s))
                return null;
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            // check for alphanumeric values only
            if (!s.All(char.IsLetterOrDigit))
                return null;
            if (s.Length < AircraftData.Database.AircraftTypeICAOMinLength)
                return null;
            try
            {
                if (!checktype)
                    return s;
                // try to find the string in aircraft registration database
                AircraftTypeDesignator type = AircraftData.Database.AircraftTypeFindByICAO(s);
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
            if (String.IsNullOrEmpty(s))
                return null;
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
            if (String.IsNullOrEmpty(s))
                return null;
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            // Routes will have a "-" between two IATA airport codes
            if (s.Length < 9)
                return null;
            int index = s.IndexOf('-');
            if ((index < 3) || (index > s.Length - 3))
                return null;
            string from = s.Substring(0, index - 1);
            string to = s.Substring(index + 1);
            try
            {
                if (AircraftData.Database.AirportFindByICAO(from) == null)
                    return null;
                if (AircraftData.Database.AirportFindByICAO(to) == null)
                    return null;
                return s;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public string To_Call(string s, bool checkairline = true)
        {
            if (String.IsNullOrEmpty(s))
                return null;
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
                if (!checkairline)
                    return s;
                // try to find the string in aircraft registration database
                AirlineDesignator airline = AircraftData.Database.AirlineFindByICAO(icao);
                if (airline != null)
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
    }
}
