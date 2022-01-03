using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScout
{
    public enum TRACKSTATUS
    {
        NONE = 0,
        STOPPED = 1,
        SINGLE = 2,
        TRACKING = 3,
        ERROR = 128
    }

    public enum ROTSTATUS
    {
        NONE = 0,
        STOPPED = 1,
        TRACKING = 2,
        ERROR = 128
    }
}
