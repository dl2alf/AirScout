using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AirScout.CAT
{
    public enum CATENGINE
    {
        NONE = 0,
        OMNIRIGX = 1,
        SCOUTBASE = 2,
        HAMLIB = 3
    }

    [Description("Rig Status")]
    public enum RIGSTATUS
    {
        [Description("None")]
        NONE = 0,
        [Description("CAT not available")]
        NOCAT = 1,
        [Description("Port not available")]
        NOPORT = 2,
        [Description("Rig not available")]
        NORIG = 4,
        [Description("Rig not suitable")]
        NOTSUITABLE = 8,
        [Description("Rig offline")]
        OFFLINE = 16,
        [Description("Rig online")]
        ONLINE = 64,
        [Description("Error")]
        ERROR = 128
    }

    public enum RIGSPLIT
    {
        NONE = 0,
        SPLITOFF = 1,
        SPLITON = 2,
        ERROR = 128
    }

    public enum RIGRIT
    {
        NONE = 0,
        RITON = 1,
        RITOFF = 2,
        ERROR = 128
    }

    public enum RIGMODE
    {
        NONE = 0,
        CW = 1,
        CW_R = 2,
        LSB = 3,
        USB = 4,
        DIG = 5,
        DIG_R = 6,
        AM = 7,
        FM = 8,
        OTHER = 9,
        ERROR = 128
    }

    public enum RIGVFO
    {
        NONE = 0,
        A = 1,
        B = 2,
        ERROR = 128
    }

    [Description("Flow Control")]
    public enum FLOWCONTROL
    {
        [Description("Low")]
        LOW,
        [Description("High")]
        HIGH,
        [Description("Handshake")]
        HANDSHAKE
    }

    [Description("Stopbits")]
    public enum STOPBITS
    {
        [Description("1")]
        ONE,
        [Description("1,5")]
        ONE_5,
        [Description("2")]
        TWO
    }

    [Description("Parity")]
    public enum PARITY
    {
        [Description("None")]
        NONE,
        [Description("Odd")]
        ODD,
        [Description("Even")]
        EVEN,
        [Description("Mark")]
        MARK,
        [Description("Space")]
        SPACE
    }

    public enum DOPPLERSTRATEGY
    {
        DOPPLER_NONE = 0,
        DOPPLER_A = 1,
        DOPPLER_B = 2,
        DOPPLER_C = 4,
        DOPPLER_D = 8,
        ERROR = 128
    }


}
