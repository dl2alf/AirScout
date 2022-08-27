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

    // holds the complete aircraft info for JSON serialization
    public class PlaneJSON
    {
        public string Hex { get; set; }
        public string Call { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public double? Alt { get; set; }
        public double? Track { get; set; }
        public double? Speed { get; set; }
        public string Type { get; set; }
        public int? Category { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Reg { get; set; }
        public int? Time { get; set; }

        public PlaneJSON()
        {
            Time = SupportFunctions.DateTimeToUNIXTime(DateTime.UtcNow);
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
            Category = (int)PLANECATEGORY.NONE;
        }

        public PlaneJSON(string hex, string call, double lat, double lon, double alt, double track, double speed, string type, PLANECATEGORY category, string manufacturer, string model, string reg, DateTime time)
        {
            Hex = hex;
            Call = call;
            Lat = lat;
            Lon = lon;
            Alt = alt;
            Track = track;
            Speed = speed;
            Type = type;
            Category = (int)category;
            Manufacturer = manufacturer;
            Model = model;
            Reg = reg;
            Time = SupportFunctions.DateTimeToUNIXTime(time.ToUniversalTime());
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

}