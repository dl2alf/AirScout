using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.CAT
{
    public class QueueItem
    {
        public byte[] Code = new byte[0];
        public CommandKind Kind = CommandKind.ckInit;

        public RigParam Param = RigParam.pmNone;    // param of Set comand
        public int Number;                          // ordinal number of Init or Status command
        public object CustSender;                   // COM object that sent custom command

        public int ReplyLength = 0;
        public string ReplyEnd = "";

        public bool NeedsReply()
        {
            return (ReplyLength > 0) || (!String.IsNullOrEmpty(ReplyEnd));
        }

    }
}
