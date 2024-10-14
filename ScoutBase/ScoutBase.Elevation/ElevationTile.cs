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

namespace ScoutBase.Elevation
{

    /// <summary>
    /// Holds the elevation tile information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class ElevationTileDesignator : SQLiteEntry
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // update the table name to the table name according to the class
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static new readonly string TableName = "Elevation";

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public double BaseLat = 0;
        [JsonIgnore]
        public double BaseLon = 0;

        [JsonIgnore]
        public double StepWidthLat = 0;
        [JsonIgnore]
        public double StepWidthLon = 0;

        [JsonIgnore]
        private string _TileIndex = "";
        public string TileIndex
        {
            get
            {
                return _TileIndex;
            }
            set
            {
                _TileIndex = value;
                BaseLat = MaidenheadLocator.LatFromLoc(value, PositionInRectangle.BottomLeft);
                BaseLon = MaidenheadLocator.LonFromLoc(value, PositionInRectangle.BottomLeft);
            }
        }

        public int MinElv { get; set; }
        public double MinLat { get; set; }
        public double MinLon { get; set; }

        public int MaxElv { get; set; }
        public double MaxLat { get; set; }
        public double MaxLon { get; set; }

        [JsonIgnore]
        private int _Rows = 50;
        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                _Rows = value;
                StepWidthLat = 1 / 24.0 / value;
            }
        }
        [JsonIgnore]
        private int _Columns = 100;
        public int Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
                StepWidthLon = 2 / 24.0 / value;
            }
        }

        public short[,] Elv { get; set; }

        // MEMBERS ONLY TO STORE VALUES TEMPORARLY --> NOT STORED IN DATABASE
        // locator bounds
        public LatLon.GRect Bounds;


        public ElevationTileDesignator()
        {
            MinElv = short.MaxValue;
            MinLat = 0;
            MinLon = 0;
            MaxElv = short.MinValue;
            MaxLat = 0;
            MaxLon = 0;
            Elv = new short[Rows, Columns];
            Bounds = new LatLon.GRect(0, 0, 0, 0);
        }

        public ElevationTileDesignator(DataRow row) : this()
        {
            FillFromDataRow(row);
            Bounds = MaidenheadLocator.BoundsFromLoc(TileIndex);
        }

        public ElevationTileDesignator(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        public ElevationTileDesignator(string index, int minelv = 0, double minlat = 0, double minlon = 0, int maxelv = 0, double maxlat = 0, double maxlon = 0, int rows = 0, int columns = 0, short[,] elv = null) :
            this(index, minelv, minlat, minlon, maxelv, maxlat, maxlon, rows, columns, elv, DateTime.UtcNow) { }

        public ElevationTileDesignator(string index, int minelv, double minlat, double minlon, int maxelv, double maxlat, double maxlon, int rows, int columns, short[,] elv, DateTime lastupdated) : this()
        {
            TileIndex = index;
            MinElv = minelv;
            MinLat = minlat;
            MinLon = minlon;
            MaxElv = maxelv;
            MaxLat = maxlat;
            MaxLon = maxlon;
            Rows = rows;
            Columns = columns;
            Elv = elv;
            LastUpdated = lastupdated;
            Bounds = MaidenheadLocator.BoundsFromLoc(index);
        }


        /// <summary>
        /// Gets the elevation from a given point (Latitude, Longitude).
        /// </summary>
        /// <param name="lat">The latitude.</param>
        /// <param name="lon">The longitude.</param>
        /// <returns>The elevation. ElvMissingFlag if not found.</returns>
        public short GetElevation (double lat, double lon)
        {

            int i = (int)(((lat - BaseLat) / StepWidthLat));
            int j = (int)(((lon - BaseLon) / StepWidthLon));
            try
            {
                return Elv[j,i];
            }
            catch
            {
                return ElevationData.Database.ElvMissingFlag;
            }
        }

        /// <summary>
        /// Gets the elevation data as a bitmap.
        /// Uses fixed color palette to create colors according to elevation.
        /// </summary>
        /// <returns>The bitmap.</returns>
        public Bitmap ToBitmap()
        {
            // set min/max elevation
            int minelv = 0;
            int maxelv = 3000;
            // create color palette
            DEMColorPalette palette = new DEMColorPalette();
            Bitmap bm = new Bitmap(this.Columns, this.Rows);
            for (int i = 0; i < this.Columns; i++)
            {
                // System.Console.WriteLine(i);
                for (int j = 0; j < this.Rows; j++)
                {
                    short e1 = Elv[i, j];
                    if (e1 != ElevationData.Database.ElvMissingFlag)
                    {
                        double e = (double)(e1 - minelv) / (double)(maxelv - minelv) * 100.0;
                        if (e < 0)
                            e = 0;
                        if (e > 100)
                            e = 100;
                        bm.SetPixel(i, bm.Height - j - 1, palette.GetColor(e));
                    }
                    else
                    {
                        bm.SetPixel(i, bm.Height - j - 1, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return bm;
        }
    }

    public class ElvMinMaxInfo
    {
        public short MinElv;
        public double MinLat;
        public double MinLon;
        public short MaxElv;
        public double MaxLat;
        public double MaxLon;

        public ElvMinMaxInfo() : this(ElevationData.Database.ElvMissingFlag, 0,0, ElevationData.Database.ElvMissingFlag, 0,0) { }
        public ElvMinMaxInfo(short minelv, double minlat, double minlon, short maxelv, double maxlat, double maxlon)
        {
            MinElv = minelv;
            MinLat = minlat;
            MinLon = minlon;
            MaxElv = maxelv;
            MaxLat = maxlat;
            MaxLon = maxlon;
        }
    }


}
