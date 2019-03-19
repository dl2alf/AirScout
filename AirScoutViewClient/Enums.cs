using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScoutViewClient
{
    public enum VIEWCLIENTSTATUS
    {
        NONE = 0,
        INIT = 1,
        CONNECTING = 2,
        CONNECTED = 4,
        ERROR = 128
    }

}
