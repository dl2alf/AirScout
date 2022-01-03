using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.CAT
{
    public class PortBits
    {
        public bool Cts { get { return (FPort != null) ? FPort.CtsBit : false; } }
        public bool Dsr { get { return (FPort != null) ? FPort.DsrBit : false; } }
        public bool Dtr { get { return (FPort != null) ? FPort.DtrBit : false; } set { if (FPort != null) FPort.DtrBit = value; } }
        public bool Rts { get { return (FPort != null) ? FPort.RtsBit : false; } set { if (FPort != null) FPort.RtsBit = value; } }

        public CommPort FPort = null;

        public bool Lock()
        {
            // not used?
            return false;
        }

        public void Unlock()
        {
            // not used?
        }
    }
}
