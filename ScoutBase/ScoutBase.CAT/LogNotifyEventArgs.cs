using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ScoutBase.CAT
{
    [Description("Log level for log notifications")]
    public enum LOGLEVEL
    {
        [Description("All")]
        llAll = 0,
        [Description("Communication")]
        llComm = 1,
        [Description("Info")]
        llInfo = 2,
        [Description("Errors")]
        llError = 3,
        [Description("Fatal")]
        llFatal = 4
    }

    public class LogNotifyEventArgs
    {
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public LOGLEVEL LogLevel { get; set; } = LOGLEVEL.llAll;
        public string Message { get; set; } = "";

        public LogNotifyEventArgs(DateTime timestamp, LOGLEVEL loglevel, string message)
        {
            TimeStamp = timestamp;
            LogLevel = loglevel;
            Message = message;
        }
    }
}
