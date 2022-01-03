using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScout.CAT
{
    public class SupportedRig
    {
        public string Type { get; set; } = "";
        public string Model { get; set; } = "";
        public CATENGINE CATEngine { get; set; } = CATENGINE.NONE;
    }
}
