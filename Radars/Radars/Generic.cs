using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RainScout.Radars
{
    public class Generic
    {
        public double Top = 0;
        public double Left = 0;
        public double Bottom = 0;
        public double Right = 0;

        public string Source = "[none]";
        public string URL = "";
        public DateTime Timestamp = DateTime.MinValue;
    }
}
