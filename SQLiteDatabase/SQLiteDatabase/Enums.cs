using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.SQLite
{
    [FlagsAttribute]
    public enum DATABASESTATUS : short
    {
        UNDEFINED = 0,
        EMPTY = 1,
        COMPLETE = 2,
        UPDATING = 4,
        UPTODATE = 8,
        ERROR = 128
    }

    public enum AUTOVACUUMMODE : short
    {
        NONE = 0,
        FULL = 1,
        INCREMENTAL = 2
    }
}
