using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;

namespace RainScout.Radars
{
    public class RadarNone : RainScout.Radars.GenericRadar
    {
        public RadarNone()
        {
        }

        public override Bitmap GetRadarImage()
        {
            return null;
        }

        public override int[,] GetRadarLayer(RADARLAYER layer)
        {
            return null;
        }
    }

}
