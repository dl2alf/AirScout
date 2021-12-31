using ScoutBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirScout.Core;
using System.IO;

namespace AirScout.Aircrafts
{
    public class PlaneInfoCache : SortedDictionary<string, PlaneInfo>
    {

        public int InsertOrUpdateIfNewer (PlaneInfo plane)
        {
            int i = 0;
            if (plane == null)
                return i;
            lock (this)
            { 
                PlaneInfo oldplane = null;
                if (!this.TryGetValue(plane.Hex, out oldplane))
                {
                    // not found --> add plane
                    this.Add(plane.Hex, plane);
                    i = 1;
                }
                else
                {
                    // plane already in cache --> check time and update if newer
                    if (plane.Time > oldplane.Time)
                    {
                        // keep old values
                        oldplane.OldTime = oldplane.Time;
                        oldplane.OldLat = oldplane.Lat;
                        oldplane.OldLon = oldplane.Lon;
                        oldplane.OldAlt = oldplane.Alt;
                        oldplane.OldSpeed = oldplane.Speed;
                        oldplane.OldTrack = oldplane.Track;

                        // update values
                        oldplane.Alt = plane.Alt;
                        oldplane.AltDiff = plane.AltDiff;
                        oldplane.Angle = plane.Angle;
                        oldplane.Call = plane.Call;
                        oldplane.Category = plane.Category;
                        oldplane.Comment = plane.Comment;
                        oldplane.Eps1 = plane.Eps1;
                        oldplane.Eps2 = plane.Eps2;
                        oldplane.Theta1 = plane.Theta1;
                        oldplane.Theta2 = plane.Theta2;
                        oldplane.IntPoint = plane.IntPoint;
                        oldplane.IntQRB = plane.IntQRB;
                        oldplane.Lat = plane.Lat;
                        oldplane.Lon = plane.Lon;
                        oldplane.Manufacturer = plane.Manufacturer;
                        oldplane.Model = plane.Model;
                        oldplane.Potential = plane.Potential;
                        oldplane.Reg = plane.Reg;
                        oldplane.SignalStrength = plane.SignalStrength;
                        oldplane.Speed = plane.Speed;
                        oldplane.Squint = plane.Squint;
                        oldplane.Time = plane.Time;
                        oldplane.Track = plane.Track;
                        oldplane.Type = plane.Type;
                        i = 1;
                    }
                }
            }
            return i;
        }

        public int BulkInsertOrUpdateIfNewer (List<PlaneInfo> planes)
        {
            int i = 0;
            if (planes == null)
                return i;
            if (planes.Count == 0)
                return i;
            lock (this)
            {
                foreach (PlaneInfo plane in planes)
                {
                    int j = InsertOrUpdateIfNewer(plane);
                    i = i + j;
                }
            }
            return i;
        }

        public int Delete (PlaneInfo plane)
        {
            int i = 0;
            PlaneInfo oldplane = null;
            if (this.TryGetValue(plane.Hex, out oldplane))
            {
                this.Remove(plane.Hex);
                i = 1;
            }
            return i;
        }

        public int BulkDelete (List<PlaneInfo> planes)
        {
            int i = 0;
            if (planes == null)
                return i;
            if (planes.Count == 0)
                return i;
            lock (this)
            {
                foreach (PlaneInfo plane in planes)
                {
                    int j = Delete(plane);
                    i = i + j;
                }
            }
            return i;
        }

        public PlaneInfo Get(string hex, DateTime at, int ttl)
        {
            PlaneInfo plane = null;
            DateTime to = at;
            DateTime from = to - new TimeSpan(0, ttl, 0);
            // return null if not found
            if (!this.TryGetValue(hex, out plane))
                return null;
            // return null if not in timespan
            if ((plane.Time < from) || (plane.Time > to))
                return null;
            // create new plane info
            PlaneInfo info = new PlaneInfo(plane);
            // estimate new values
            double alt = 0;
            double speed = 0;
            double speed_kmh = 0;
            double track = 0;
            double dist = 0;
            LatLon.GPoint newpos;
            // use stored old values if available
            if (plane.OldTime != DateTime.MinValue)
            {
                double oldtimediff = (plane.Time - plane.OldTime).TotalSeconds;
                double newtimediff = (at - plane.Time).TotalSeconds;

                // adjust values if there is a valid timespan in history
                if ((oldtimediff > 0) && (newtimediff > 0))
                {
                    alt = (plane.Alt - plane.OldAlt) / oldtimediff * newtimediff + plane.Alt;
                    track = (plane.Track - plane.OldTrack) / oldtimediff * newtimediff + plane.Track;
                    speed = (plane.Speed - plane.OldSpeed) / oldtimediff * newtimediff + plane.Speed;
                }
                else
                {
                    // do nothing
                }
            }
            else
            {
                // no stored values available 
            }

            // --> estimate new position using speed and track

            // do plausibility check of calculated new absolute values
            if ((alt > 0) && (alt <= 50000) &&
                (track > 0) && (track <= 360) &&
                (speed > 0) && (speed <= 700))
            {
                // change speed to km/h
                speed_kmh = UnitConverter.kts_kmh(speed);
                // calculate distance after timespan
                dist = speed_kmh * (at - info.Time).TotalHours;
                // estimate new position 
                newpos = LatLon.DestinationPoint(info.Lat, info.Lon, track, dist);

                // check resulting motion vector against last reported track
                // should be well inside the bounds
                // a plane cannot move to a position to where it is not pointing to
                double calctrack = LatLon.Bearing(info.Lat, info.Lon, newpos.Lat, newpos.Lon);
                if (Math.Abs(info.Track - calctrack) < 45)
                {
                    // valid --> use the calculated values
                    info.Lat = newpos.Lat;
                    info.Lon = newpos.Lon;
                    info.Alt = alt;
                    info.Track = track;
                    info.Speed = speed;
                    info.Time = at;

                    // return calculated info
                    return info;
                }
            }

            // one of plausibility checks failed --> use last reported constant values for estimation
            // change speed to km/h
            speed_kmh = info.Speed_kmh;
            // calculate distance after timespan
            dist = speed_kmh * (at - info.Time).TotalHours;
            // estimate new position 
            newpos = LatLon.DestinationPoint(info.Lat, info.Lon, info.Track, dist);
            info.Lat = newpos.Lat;
            info.Lon = newpos.Lon;
            info.Time = at;

            // return calculated info
            return info;
        }

        public List<PlaneInfo> GetAll(DateTime at, int ttl)
        {
            string filename = "positions.csv";
            string call = "CSN464";

            List<PlaneInfo> l = new List<PlaneInfo>();
            DateTime to = at;
            DateTime from = to - new TimeSpan(0, ttl, 0);
            lock (this)
            {
                foreach (PlaneInfo plane in this.Values)
                {
                    if ((plane.Time < from) || (plane.Time > to))
                        continue;
                    // create new plane info
                    PlaneInfo info = new PlaneInfo(plane);
                    // estimate new values
                    // use stored old values if available
                    if (plane.OldTime != DateTime.MinValue)
                    {
                        double oldtimediff = (plane.Time - plane.OldTime).TotalSeconds;
                        double newtimediff = (at - plane.Time).TotalSeconds;

                        // adjust values if there is a valid timespan in history
                        if ((oldtimediff > 0) && (newtimediff > 0))
                        {
                            double newalt = (plane.Alt - plane.OldAlt) / oldtimediff * newtimediff + plane.Alt;
                            double newtrack = (plane.Track - plane.OldTrack) / oldtimediff * newtimediff + plane.Track;
                            double newspeed = (plane.Speed - plane.OldSpeed) / oldtimediff * newtimediff + plane.Speed;

                            if (plane.Call == call)
                            {
                                File.AppendAllText(filename, oldtimediff.ToString() + ";" +
                                        newtimediff.ToString() + ";" +
                                        plane.OldAlt.ToString("F8") + ";" +
                                        plane.Alt.ToString("F8") + ";" +
                                        newalt.ToString("F8") + ";" +
                                        plane.OldTrack.ToString("F8") + ";" +
                                        plane.Track.ToString("F8") + ";" +
                                        newtrack.ToString("F8") + ";" +
                                        plane.OldSpeed.ToString("F8") + ";" +
                                        plane.Speed.ToString("F8") + ";" +
                                        newspeed.ToString("F8") + ";" +
                                        Environment.NewLine);
                            }
                            // do plausibility check of calculated values
                            if ((newalt > 0) && (newalt < 50000) &&
                                (newtrack > 0) && (newtrack < 360) &&
                                (newspeed > 0) && (newspeed < 700))
                            {
                                info.Alt = newalt;
                                info.Track = newtrack;
                                info.Speed = newspeed;
                            }
                            else
                            {
                                // do nothing
                                if (plane.Call == call)
                                {
                                    File.AppendAllText(filename, oldtimediff.ToString() + ";" +
                                        newtimediff.ToString() + ";" +
                                        plane.OldAlt.ToString("F8") + ";" +
                                        plane.Alt.ToString("F8") + ";" +
                                        newalt.ToString("F8") + ";" +
                                        plane.OldTrack.ToString("F8") + ";" +
                                        plane.Track.ToString("F8") + ";" +
                                        newtrack.ToString("F8") + ";" +
                                        plane.OldSpeed.ToString("F8") + ";" +
                                        plane.Speed.ToString("F8") + ";" +
                                        newspeed.ToString("F8") + ":" +
                                        "invalid values!" +
                                        Environment.NewLine);
                                }
                            }
                        }
                        else
                        {
                            // do nothing
                            if (plane.Call == call)
                            {
                                File.AppendAllText(filename, oldtimediff.ToString() + ";" + newtimediff.ToString() + "invalid timediff!" + Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        // no stored values available 
                    }

                    // --> estimate new position using speed and track
                    // change speed to km/h
                    double speed = info.Speed_kmh;
                    // calculate distance after timespan
                    double dist = speed * (at - info.Time).TotalHours;
                    // estimate new position 
                    LatLon.GPoint newpos = LatLon.DestinationPoint(info.Lat, info.Lon, info.Track, dist);
                    info.Lat = newpos.Lat;
                    info.Lon = newpos.Lon;
                    info.Time = at;
                    l.Add(info);
                }
            }
            return l;
        }
    }
}
