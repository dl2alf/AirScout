using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.CAT
{
    public class RigCommand
    {
        // what to send
        public byte[] Code = new byte[0];
        public ParamValue Value = new ParamValue();

        // what to wait for
        public byte[] ReplyEnd = new byte[0];
        public int ReplyLength = 0;
        public BitMask Validation = new BitMask();

        // what to extract
        public List<ParamValue> Values = new List<ParamValue>();
        public List<BitMask> Flags = new List<BitMask>();

        public RigCommand()
        {
        }
    }
}
