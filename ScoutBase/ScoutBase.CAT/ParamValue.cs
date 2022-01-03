using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScoutBase.CAT
{
    public class ParamValue
    {
        public int Start, Len;         //insert or extract bytes, Start is a 0-based index
        public ValueFormat Format;     //encode or decode according to this format
        public double Mult, Add;       //linear transformation before encoding or after decoding
        public RigParam Param;         //param to insert or to report

        public ParamValue()
        {
            Start = 0;
            Len = 0;
            Format = ValueFormat.vfNone;
            Mult = 0;
            Add = 0;
            Param = RigParam.pmNone;
        }
    }
}
