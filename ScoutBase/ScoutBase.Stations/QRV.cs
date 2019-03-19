using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ScoutBase.Core;

namespace ScoutBase.Stations
{

    /// <summary>
    /// Holds the station QRV information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    [Serializable]
    public class QRVDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // individual static SQL strings per class will be created on first use
        // add a "new" statement on each derived class to confirm hiding of the base class members
        // update the tbale name to the table name according to the class
        // update the PrimaryKeys collection according to the class, crreate an empty list if no primary key
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "QRV";


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Call { get; set; }
        public string Loc { get; set; }
        public BAND Band { get; set; }
        public double AntennaHeight { get; set; }
        public double AntennaGain { get; set; }
        public double Power { get; set; }

        public QRVDesignator()
        {
            Call = "";
            Loc = "";
            Band = BAND.BNONE;
            AntennaHeight = 0;
            AntennaGain = 0;
            Power = 0;
            LastUpdated = DateTime.MinValue;
        }

        public QRVDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public QRVDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public QRVDesignator(string call, string loc, BAND band) : this(call, loc, band, 0, 0, 0) { }

        public QRVDesignator(string call, string loc, BAND band, double antennaheight, double antennagain, double power) : this(call, loc, band, antennaheight, antennagain, power, DateTime.UtcNow) { }

        public QRVDesignator(string call, string loc, BAND band, double antennaheight, double antennagain, double power, DateTime lastupdated) : this()
        {
            Call = call.ToUpper().Trim();
            Loc = loc.ToUpper().Trim().Substring(0, 6);
            Band = band;
            AntennaHeight = antennaheight;
            AntennaGain = antennagain;
            Power = power;
            LastUpdated = lastupdated;
        }

    }
}
