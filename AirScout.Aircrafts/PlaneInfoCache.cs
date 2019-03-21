using ScoutBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirScout.Core;

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
                    // add plane
                    this.Add(plane.Hex, plane);
                    i = 1;
                }
                else
                {
                    // plane already in cache --> check time and update if newer
                    if (plane.Time > oldplane.Time)
                    {
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
            // estimate new position
            // change speed to km/h
            double speed = info.Speed_kmh;
            // calculate distance after timespan
            double dist = speed * (at - info.Time).TotalHours;
            // estimate new position 
            LatLon.GPoint newpos = LatLon.DestinationPoint(info.Lat, info.Lon, info.Track, dist);
            info.Lat = newpos.Lat;
            info.Lon = newpos.Lon;
            info.Time = at;
            return info;
        }

        public List<PlaneInfo> GetAll(DateTime at, int ttl)
        {
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
                    // estimate new position
                    // change speed to km/h
                    double speed = info.Speed_kmh;
                    // calculate distance after timespan
                    double dist = speed * (at - info.Time).TotalHours;
                    // estimate new position 
                    LatLon.GPoint newpos = LatLon.DestinationPoint(info.Lat, info.Lon, info.Track, dist);
                    double d = LatLon.Distance(info.Lat, info.Lon, newpos.Lat, newpos.Lon);
                    Console.WriteLine(d);
                    if (d > 100)
                        Console.WriteLine("Error");
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
