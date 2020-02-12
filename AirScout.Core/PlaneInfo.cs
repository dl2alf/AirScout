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
using System.Text.RegularExpressions;
using System.Data;
using ScoutBase.Core;
using System.Reflection;

namespace AirScout.Core
{

    // holds the complete aircraft info
    public class PlaneInfo
    {
        public DateTime Time { get; set; }
        public string Call { get; set; }
        public string Reg { get; set; }
        public string Hex { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        private double _Alt;
        public double Alt
        {
            get
            {
                return _Alt;
            }
            set
            {
                _Alt = value;
                _Alt_m = UnitConverter.ft_m(value);
            }
        }
        private double _Alt_m;
        public double Alt_m
        {
            get
            {
                return _Alt_m;
            }
        }
        public double Track = 0;
        private double _Speed;
        public double Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
                _Speed_kmh = UnitConverter.kts_kmh(value);
            }
        }
        private double _Speed_kmh;
        public double Speed_kmh
        {
            get
            {
                return _Speed_kmh;
            }
        }
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public PLANECATEGORY Category { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int VSpeed { get; set; }

        public LatLon.GPoint IntPoint = null;
        public double IntQRB = double.MaxValue;
        public double AltDiff = 0;
        public int Potential = 0;
        public double Eps1 = 0;
        public double Eps2 = 0;
        public double Theta1 = 0;
        public double Theta2 = 0;
        public double Angle = 0;
        public double Squint = 0;
        public double SignalStrength = double.MinValue;
        public bool Ambiguous = false;
        public string Comment = "";

        public PlaneInfo()
        {
            Time = DateTime.UtcNow;
            Call = "";
            Reg = "";
            Hex = "";
            Lat = 0;
            Lon = 0;
            Alt = 0;
            Track = 0;
            Speed = 0;
            Type = "";
            Manufacturer = "";
            Model = "";
            Category = PLANECATEGORY.NONE;
            From = "";
            To = "";
            VSpeed = 0;
        }

        public PlaneInfo(DateTime time, string call, string reg, string hex, double lat, double lon, double track, double alt, double speed, string type, string manufacturer, string model, PLANECATEGORY category)
        {
            Time = time.ToUniversalTime();
            Call = call;
            Reg = reg;
            Hex = hex;
            Lat = lat;
            Lon = lon;
            Alt = alt;
            Track = track;
            Speed = speed;
            Type = type;
            Manufacturer = manufacturer;
            Model = model;
            Category = category;
        }

        // LEGACY!!!
        public PlaneInfo (DataRow row)
        {
            Time = SupportFunctions.UNIXTimeToDateTime(System.Convert.ToInt32(row[0]));
            Call = (string)row[1];
            Reg = (string)row[2];
            Hex = (string)row[3];
            Lat = (double)row[4];
            Lon = (double)row[5];
            Track = System.Convert.ToInt32(row[6]);
            Alt = System.Convert.ToInt32(row[7]);
            Speed = System.Convert.ToInt32(row[8]);
            Type = (string)row[9];
            Manufacturer = (string)row[10];
            Model = (string)row[11];
            Category = (PLANECATEGORY)row[12];
        }

        public PlaneInfo (PlaneInfo plane)
        {
            this.Ambiguous = plane.Ambiguous;
            this.Alt = plane.Alt;
            this.AltDiff = plane.AltDiff;
            this.Angle = plane.Angle;
            this.Call = plane.Call;
            this.Category = plane.Category;
            this.Comment = plane.Comment;
            this.Eps1 = plane.Eps1;
            this.Eps2 = plane.Eps2;
            this.Theta1 = plane.Theta1;
            this.Theta2 = plane.Theta2;
            this.Hex = plane.Hex;
            this.IntPoint = plane.IntPoint;
            this.IntQRB = plane.IntQRB;
            this.Lat = plane.Lat;
            this.Lon = plane.Lon;
            this.Manufacturer = plane.Manufacturer;
            this.Model = plane.Model;
            this.Potential = plane.Potential;
            this.Reg = plane.Reg;
            this.SignalStrength = plane.SignalStrength;
            this.Speed = plane.Speed;
            this.Squint = plane.Squint;
            this.Time = plane.Time;
            this.Track = plane.Track;
            this.Type = plane.Type;

        }

        public override string ToString()
        {
            string s = "";
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                if (p.PropertyType.Name.ToUpper() == "STRING")
                {
                    string v = (string)p.GetValue(this, null);
                    if (v == null)
                        v = "[null]";
                    else if (v.Length == 0)
                        v = "[empty]";
                    s = s + p.Name + ": " + v + "\n";
                }
                else if (p.PropertyType.Name.ToUpper() == "DATETIME")
                {
                    DateTime dt = (DateTime)p.GetValue(this, null);
                    s = s + p.Name + ": " + dt.ToString("yyyy-MM-dd HH:mm:ss") + "\n";
                }
                else
                {
                    object o = p.GetValue(this, null);
                    if (o == null)
                        s = s + p.Name + ": " + "[null]" + "\n";
                    else
                        s = s + p.Name + ": " + o.ToString() + "\n";
                }
            }
            return s;
        }

    }

    // List of plane infos
    public class PlaneInfoList : List<PlaneInfo>
    {

    }
}