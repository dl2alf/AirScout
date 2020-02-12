using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LibADSB
{
    // contains all relevant ADSB-Info
    public class ADSBInfo
    {
        public DateTime Timestamp = DateTime.UtcNow;
        public string ICAO24 = "";
        public string Call = "";
        public double Lat = double.NaN;
        public double Lon = double.NaN;
        public int Alt = int.MinValue;
        public int Heading = int.MinValue;
        public int Speed = int.MinValue;
        public AirbornePositionMsg LastEvenAirborne = null;
        public AirbornePositionMsg LastOddAirborne = null;
        public DateTime LastEvenTimestamp = DateTime.MinValue;
        public DateTime LastOddTimestamp = DateTime.MinValue;
        public bool NICSupplementA = false;

    }

    public class ADSBDecoder : Object
    {
        [Browsable(false)]
        [DescriptionAttribute("Count of objects currently in list")]
        public int Count
        {
            get
            {
                return adsbinfos.Count;
            }
        }

        // time to live --> remove object after TTL is over
        private int ttl;

        private DateTime lastsend = DateTime.UtcNow;

        private Dictionary<string, ADSBInfo> adsbinfos;

        public ADSBDecoder()
            : this(5)
        {
        }

        public ADSBDecoder(int TTL)
        {
            ttl = TTL;
            adsbinfos = new Dictionary<string, ADSBInfo>();
        }

        private string DecodeIdentificationMsg(ModeSReply msg, DateTime timestamp)
        {
            IdentificationMsg ident = (IdentificationMsg)msg;
            if (msg.ICAO24 == null)
                return "IdentifyMsg: No ICAO24 found.";
            string icao24 = BitConverter.ToString(msg.ICAO24).Replace("-", String.Empty);
            // check if ICAO is already stored in lookup table
            ADSBInfo info = null;
            if (!adsbinfos.TryGetValue(icao24, out info))
            {
                // no --> add new entry
                info = new ADSBInfo();
                info.ICAO24 = icao24;
                adsbinfos.Add(icao24, info);
            }
            // add call sign
            info.Call = ident.getIdentity();
            return "[" + info.ICAO24 + "] IdentificationMsg: Call=" + info.Call;
        }

        private string DecodeAirbornePositionMsg(ModeSReply msg, DateTime timestamp)
        {
                // Airborne position message --> we need subsequent messages to decode
            AirbornePositionMsg pos = (AirbornePositionMsg)msg;
            if (msg.ICAO24 == null)
                return "AirbornePositionMsg: No ICAO24 found.";
            string icao24 = BitConverter.ToString(msg.ICAO24).Replace("-", String.Empty);
            // check if ICAO is already stored in lookup table
            ADSBInfo info = null;
            if (!adsbinfos.TryGetValue(icao24, out info))
            {
                // no --> add new entry
                info = new ADSBInfo();
                info.ICAO24 = icao24;
                adsbinfos.Add(icao24, info);
            }
            // adsbinfo found --> update information and calculate position
            // contains valid position?
            if (!pos.HasValidPosition)
            {
                // no --> return error meesage
                return "[" + info.ICAO24 + "] AirbornePositionMsg: No valid position found.";
            }
            info.ICAO24 = icao24;
            info.NICSupplementA = pos.NICSupplementA;
            // position calculated before
            if (!double.IsNaN(info.Lat) & !double.IsNaN(info.Lon))
            {
                try
                {
                    // use local CPR
                    double[] localpos = pos.getLocalPosition(info.Lat, info.Lon);
                    // we have a pos --> store in info and update timestamp
                    info.Lat = localpos[0];
                    info.Lon = localpos[1];
                    if (pos.HasValidAltitude)
                        info.Alt = pos.Altitude;
                    info.Timestamp = timestamp;
                    return "[" + info.ICAO24 + "] AirbornePositionMsg: Lat= " + info.Lat.ToString("F8") + ", Lon=" + info.Lon.ToString("F8") + ", Alt= " + info.Alt.ToString() + ", Time= " + info.Timestamp.ToString("HH:mm:ss.fff");
                }
                catch (Exception ex)
                {
                }
            }
            // no position calculated before    
            if (pos.IsOddFormat)
            {
                try
                {
                    // odd message
                    info.LastOddAirborne = pos;
                    info.LastOddTimestamp = DateTime.UtcNow;
                    // check if even message was received before and not older than 10secs--> calculate global CPR
                    if ((info.LastEvenAirborne != null) && ((info.LastOddTimestamp - info.LastOddTimestamp).TotalSeconds <= 10))
                    {
                        try
                        {
                            double[] globalpos = pos.getGlobalPosition(info.LastEvenAirborne);
                            // we have a position --> store in info and update timestamp
                            info.Lat = globalpos[0];
                            info.Lon = globalpos[1];
                            if (pos.HasValidAltitude)
                                info.Alt = pos.Altitude;
                            //                        info.Timestamp = timestamp;
                            return "[" + info.ICAO24 + "] AirbornePositionMsg: Lat= " + info.Lat.ToString("F8") + ", Lon=" + info.Lon.ToString("F8") + ", Alt= " + info.Alt.ToString() + ", Time= " + info.Timestamp.ToString("HH:mm:ss.fff");
                        }
                        catch
                        {
                            return "[" + info.ICAO24 + "] AirbornePositionMsg: Error while decoding position";
                        }
                    }
                    return "[" + info.ICAO24 + "] AirbornePositionMsg: No decoding possible yet";
                }
                catch (Exception ex)
                {
                }
            }
            // even message
            info.LastEvenAirborne = pos;
            info.LastEvenTimestamp = DateTime.UtcNow;
            // check if odd message was received before and not older than 10secs --> calculate global CPR
            if ((info.LastOddAirborne != null) && ((info.LastEvenTimestamp-info.LastOddTimestamp).TotalSeconds <= 10))
            {
                try
                {
                    double[] globalpos = pos.getGlobalPosition(info.LastOddAirborne);
                    // we have a position --> store in info and update timestamp
                    info.Lat = globalpos[0];
                    info.Lon = globalpos[1];
                    if (pos.HasValidAltitude)
                        info.Alt = pos.Altitude;
//                    info.Timestamp = timestamp;
                    return "[" + info.ICAO24 + "] AirbornePositionMsg: Lat= " + info.Lat.ToString("F8") + ", Lon=" + info.Lon.ToString("F8") + ", Alt= " + info.Alt.ToString() + ", Time= " +info.Timestamp.ToString("HH:mm:ss.fff");
                }
                catch
                {
                    return "[" + info.ICAO24 + "] AirbornePositionMsg: Error while decoding position";
                }
            }
            else
            {
                return "[" + info.ICAO24 + "] AirbornePositionMsg:No decoding possible yet";
            }
        }

        private string DecodeAirspeedHeadingMsg(ModeSReply msg, DateTime timestamp)
        {
            AirspeedHeadingMsg heading = (AirspeedHeadingMsg)msg;
            if (msg.ICAO24 == null)
                return "AirspeedHeadingMsg: No ICAO24 found.";
            string icao24 = BitConverter.ToString(msg.ICAO24).Replace("-", String.Empty);
            // check if ICAO is already stored in lookup table
            ADSBInfo info = null;
            if (!adsbinfos.TryGetValue(icao24, out info))
            {
                // no --> add new entry
                info = new ADSBInfo();
                info.ICAO24 = icao24;
                adsbinfos.Add(icao24, info);
            }
            // add information
            if (heading.HasValidAirspeed)
                info.Speed = heading.Airspeed;
            if (heading.HasValidHeading)
                info.Heading = heading.Heading;
            return "[" + info.ICAO24 + "] AirspeedHeadingMsg: Speed=" + info.Speed.ToString() + ", Heading= " + info.Heading.ToString();
        }

        private string DecodeVelocityOverGroundMsg(ModeSReply msg, DateTime timestamp)
        {
            VelocityOverGroundMsg velocity = (VelocityOverGroundMsg)msg;
            if (msg.ICAO24 == null)
                return "VelocityOverGroundMsg: No ICAO24 found.";
            string icao24 = BitConverter.ToString(msg.ICAO24).Replace("-", String.Empty);
            // check if ICAO is already stored in lookup table
            ADSBInfo info = null;
            if (!adsbinfos.TryGetValue(icao24, out info))
            {
                // no --> add new entry
                info = new ADSBInfo();
                info.ICAO24 = icao24;
                adsbinfos.Add(icao24, info);
            }
            // add information
            if (velocity.HasValidVelocity)
                info.Speed = velocity.Velocity;
            if (velocity.HasValidHeading)
                info.Heading = velocity.Heading;
            return "[" + info.ICAO24 + "] VelocityOverGroundMsg: Speed=" + info.Speed.ToString() + ", Heading= " + info.Heading.ToString();
        }

        public string DecodeMessage(string raw_msg, DateTime timestamp)
        {
            // decode an ADS-B message and add information to list
            // cut off first and last character
            raw_msg = raw_msg.Substring(1, raw_msg.Length - 2);
            // do generic decoding first
            ModeSReply msg = LibADSB.Decoder.GenericDecoder(raw_msg);
            if (!msg.CheckParity)
            {
                return ("Parity error, no decode.");
            }
            // parity is OK, let's start to sort the messages and calculate
            //                                        Console.WriteLine(msg.ToString());
            // lock adsbinfolist
            lock (adsbinfos)
            {
                try
                {
                    if (msg.GetType() == typeof(IdentificationMsg))
                    {
                        // Identification message;
                        return DecodeIdentificationMsg(msg, timestamp);
                    }
                    else if (msg.GetType() == typeof(AirbornePositionMsg))
                    {
                        // Airborne position message
                        return DecodeAirbornePositionMsg(msg, timestamp);
                    }
                    else if (msg.GetType() == typeof(VelocityOverGroundMsg))
                    {
                        // Velocity over ground message
                        return DecodeVelocityOverGroundMsg(msg, timestamp);
                    }
                    else if (msg.GetType() == typeof(AirspeedHeadingMsg))
                    {
                        // Airspeed heading message
                        return DecodeAirspeedHeadingMsg(msg, timestamp);
                    }
                }
                catch (Exception ex)
                {
                    string s = msg.GetType().ToString();
                    return "Error while decoding " + s + ": " + ex.Message;
                }
            }
            return ("Unknown message.");
        }

        public ArrayList GetPlanes()
        {
            ArrayList list = new ArrayList();
            // return a list of ADSBInfos
            foreach (KeyValuePair<string, ADSBInfo> info in adsbinfos)
            {
                // check for old entries
                if (info.Value.Timestamp > lastsend)
                {
                    // check if entry is complete
                    if ((!String.IsNullOrEmpty(info.Value.ICAO24)) &&
                        (!String.IsNullOrEmpty(info.Value.Call)) &&
                        (info.Value.Lat != double.NaN) &&
                        (info.Value.Lon != double.NaN) &&
                        (info.Value.Alt != int.MinValue) &&
                        (info.Value.Speed != int.MinValue) &&
                        (info.Value.Heading != int.MinValue)
                        )
                    {
                        list.Add(info.Value);
                    }
                }
            }
            lastsend = DateTime.UtcNow;
            return list;
        }
    }
}
