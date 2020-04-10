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
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Xml;
using System.Xml.Serialization;
using ScoutBase.Core;
using Newtonsoft.Json;

namespace ScoutBase.Maps
{

    /// <summary>
    /// Holds the map tile information
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class MapTileInfo
    {
        public int ID;
        public int X;
        public int Y;
        public int Zoom;
        public int Type;
        public DateTime CacheTime;

        public MapTileInfo(int id, int x, int y, int zoom, int type, DateTime cachetime)
        {
            ID = id;
            X = x;
            Y = y;
            Zoom = zoom;
            Type = type;
            CacheTime = cachetime;
        }
    }

    /// <summary>
    /// Holds the map tile data
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class MapTileData
    {
        public int ID;
        public byte[] Tile;

        public MapTileData(int id, byte[] tile)
        {
            ID = id;
            Tile = new byte[tile.Length];
            Array.Copy(tile, Tile, tile.Length);
        }
    }
}
