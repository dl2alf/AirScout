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

    // holds the complete aircraft info for JSON history data
    public class HistJSON
    {
        public string id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double track { get; set; }
        public double altitude { get; set; }
        public double speed { get; set; }
        public int icon { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public bool onGround { get; set; }
        public string callsign { get; set; }
        public string squawk { get; set; }
        public int dataSource { get; set; }
        public ScheduleInfo schedule { get; set; }
        public EMSInfo ems { get; set; }

        public HistJSON()
        {
            id = "";
            latitude = 0;
            longitude = 0;
            track = 0;  
            altitude = 0;
            speed = 0;
            icon = 0;
            status = 0;
            timestamp = 0;
            onGround = false;
            callsign = "";
            squawk = null;
            dataSource = 0;
            schedule = null;
            ems = null;
        }

    }

    public class ScheduleInfo
    {
        public string std { get; set; }
        public string etd { get; set; }
        public string atd { get; set; }
        public string sta { get; set; }
        public string eta { get; set; }
        public string ata { get; set; }
    }

    public class EMSInfo
    {
        public object agps { get; set; }
        public object ias { get; set; }
        public object mach { get; set; }
        public object oat { get; set; }
        public object tas { get; set; }
        public object windSpeed { get; set; }
        public object windDirection { get; set; }
    }
}