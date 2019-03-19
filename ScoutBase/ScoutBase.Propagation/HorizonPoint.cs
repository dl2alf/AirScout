using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using System.Data.SQLite;
using ScoutBase.Core;
using ScoutBase.Elevation;
using Newtonsoft.Json;

namespace ScoutBase.Propagation
{
    [Serializable]
    public class HorizonPoint
    {
        public double Dist { get; set; }
        public double Epsmin { get; set; }
        public short Elv { get; set; }

        public HorizonPoint()
        {
            Dist = double.MinValue;
            Epsmin = double.MinValue;
            Elv = short.MinValue;
        }

        public HorizonPoint(double dist, double epsmin, short elv)
        {
            Dist = dist;
            Epsmin = epsmin;
            Elv = elv;
        }
    }

}
