using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AirScout
{
    // keeps tracking values
    public class TrackValues
    {
        public DateTime Timestamp = DateTime.UtcNow;
        public string Hex = "";

        public double MyAzimuth = double.NaN;
        public double MyElevation = double.NaN;
        public double MyDistance = double.NaN;
        public double MySlantRange = double.NaN;
        public double MyDoppler = 0;

        public double DXAzimuth = double.NaN;
        public double DXElevation = double.NaN;
        public double DXDistance = double.NaN;
        public double DXSlantRange = double.NaN;
        public double DXDoppler = 0;

        public long RXFrequency = 0;
        public long TXFrequency = 0;
    }
}
