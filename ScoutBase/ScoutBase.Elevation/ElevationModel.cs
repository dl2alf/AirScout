using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ScoutBase.Elevation
{
    [Description("Elevation Model")]
    public enum ELEVATIONMODEL
    {
        [Description("None")]
        NONE = 0,
        [Description("GLOBE")]
        GLOBE = 1,
        [Description("SRTM3")]
        SRTM3 = 2,
        [Description("SRTM1")]
        SRTM1 = 3,
        [Description("ASTER3")]
        ASTER3 = 4,
        [Description("ASTER1")]
        ASTER1 = 5
    }
}
