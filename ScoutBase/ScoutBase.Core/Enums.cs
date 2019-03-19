using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.Core
{
    /// <summary>
    /// Holds the source information a geographic location is from
    /// </summary>
    public enum GEOSOURCE
    {
        UNKONWN = 0,
        FROMLOC = 1,
        FROMUSER = 2,
        FROMBEST = 3
    }

    /// <summary>
    /// Gives the start options for a background updater thread
    /// </summary>
    public enum BACKGROUNDUPDATERSTARTOPTIONS
    {
        NONE = 0,
        FIRSTRUN = 1,
        RUNONCE = 2,
        RUNPERIODICALLY = 3
    }
}
