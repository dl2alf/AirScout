using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirScout.Core;

namespace AirScout.Aircrafts
{
    public static class PlaneInfoChecker
    {

        public static bool Check_Hex(string hex)
        {
            if (String.IsNullOrEmpty(hex))
                return false;
            hex = hex.Replace("\"", String.Empty).ToUpper().Trim();
            // Hex must be a 6 character value 
            if (hex.Length != 6)
                return false;
            if (hex.ToCharArray().Any(c => !"0123456789ABCDEF".Contains(c)))
                return false;
            try
            {
                // try to convert to Hex value
                long h = System.Convert.ToInt64(hex, 16);
                // check boundaries
                if ((h < 0) || (h > 16777215))
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + hex);
                return false;
            }
            return true;
        }

        public static bool Check_Call(string call)
        {
            if (String.IsNullOrEmpty(call))
                return false;
            if (String.IsNullOrWhiteSpace(call))
                return false;
            if (call.Contains("[unknown]"))
                return false;
            call = call.Replace("\"", String.Empty).ToUpper().Trim();
            // check length
            if (call.Length < 4)
                return false;
            if (call.Contains('-'))
                return false;
            string airline = "";
            // Type B callsign?
            if (char.IsNumber(call[2]))
            {
                airline = call.Substring(0, 2);
                if (AircraftData.Database.AirlineFindByIATA(airline) == null)
                    return false;
            }
            // Type C callsign
            if (!Char.IsLetter(call[0]) || !char.IsLetter(call[1]))
                return false;
            airline = call.Substring(0, 3);
            if (AircraftData.Database.AirlineFindByICAO(airline) == null)
                return false;
            return true;
        }

        public static bool Check_Reg(string reg)
        {
            if (String.IsNullOrEmpty(reg))
                return false;
            if (String.IsNullOrWhiteSpace(reg))
                return false;
            if (reg.Contains("[unknown]"))
                return false;
            reg = reg.Replace("\"", String.Empty).ToUpper().Trim();
            if (reg.Length < AircraftData.Database.AircraftRegistrationMinLength + 1)
                return false;
            if (!reg.Contains('-') && !reg.StartsWith("N"))
                return false;
            return true;
        }

        public static bool Check_Lat(double lat)
        {
            if (lat < -90)
                return false;
            if (lat > 90)
                return false;
            return true;
        }

        public static bool Check_Lon(double lon)
        {
            if (lon < -180)
                return false;
            if (lon > 180)
                return false;
            return true;
        }

        public static bool Check_Alt(double alt)
        {
            if (alt < 0)
                return false;
            if (alt > 100000.0)
                return false;
            return true;
        }

        public static bool Check_Track(double track)
        {
            if (track < 0)
                return false;
            if (track >= 360)
                return false;
            return true;
        }


        public static bool Check_Speed(double speed)
        {
            if (speed < 0)
                return false;
            if (speed > 800)
                return false;
            return true;
        }

        public static bool Check_Type(string type)
        {
            if (String.IsNullOrEmpty(type))
                return false;
            if (String.IsNullOrWhiteSpace(type))
                return false;
            if (type.Contains("[unknown]"))
                return false;
            type = type.Replace("\"", String.Empty).ToUpper().Trim();
            // check for alphanumeric values only
            if (!type.All(char.IsLetterOrDigit))
                return false;
            if (type.Length < AircraftData.Database.AircraftTypeICAOMinLength)
                return false;
            return true;
        }

        public static bool Check_Manufacturer(string manufacturer)
        {
            if (String.IsNullOrEmpty(manufacturer))
                return false;
            return true;
        }

        public static bool Check_Model(string model)
        {
            if (String.IsNullOrEmpty(model))
                return false;
            return true;
        }

        public static bool Check(PlaneInfo info)
        {
            // checks a PlaneInfo object
            // returns TRUE if contains all mandantory data
            // get a plane info converter for plausiblity check
            PlaneInfoConverter C = new PlaneInfoConverter();
            // check hex
            if (String.IsNullOrEmpty(C.To_Hex(info.Hex)))
                return false;
            // check call
            if (String.IsNullOrEmpty(C.To_Call(info.Call, false)))
                return false;
            if (info.Time == null)
                return false;
            if (info.Time == DateTime.MinValue)
                return false;
            if (info.Time == DateTime.MaxValue)
                return false;
            if (info.Lat > 90)
                return false;
            if (info.Lat < -90)
                return false;
            if (info.Lon > 180)
                return false;
            if (info.Lon < -180)
                return false;
            if (info.Alt < 0)
                return false;
            if (info.Alt > 100000)
                return false;
            if (info.Track > 360)
                return false;
            if (info.Track < 0)
                return false;
            if (info.Speed < 0)
                return false;
            if (info.Speed > 5000)
                return false;
            return true;
        }

    }
}
