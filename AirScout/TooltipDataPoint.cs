using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;

namespace AirScout
{
    public class TooltipDataPoint : IDataPointProvider
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Tooltip { get; set; }
        public DataPoint GetDataPoint() => new DataPoint(X, Y);

        public TooltipDataPoint(double x, double y, string tooltip)
        {
            X = x;
            Y = y;
            Tooltip = tooltip;
        }
    }
}
