using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RainScout.Radars
{
    [Serializable]
    public abstract class GenericRadar
    {
        public string Name = "None";
        public string Source = "None";
        public DateTime Timestamp = DateTime.MinValue;

        // dimensions in lat/lon
        public double Top = 0;
        public double Left = 0;
        public double Bottom = 0;
        public double Right = 0;

        // List of supported Radar layers
        public List<RADARLAYER> RadarLayers { get; internal set; } = new List<RADARLAYER>();

        public abstract Bitmap GetRadarImage();
        public abstract int[,] GetRadarLayer(RADARLAYER layer);

        // contains radar layer?
        public bool HasRadarLayer(RADARLAYER layer)
        {
            if (RadarLayers == null)
                return false;
            if (RadarLayers.Contains(layer))
                return true;
            return false;
        }

    }
}
