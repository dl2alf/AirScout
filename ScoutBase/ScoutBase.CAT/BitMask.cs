using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ScoutBase.CAT
{
    public class BitMask
    {
        public byte[] Mask = new byte[0];                  // do bitwise AND with this mask
        public byte[] Flags = new byte[0];                 // compare result to these bits
        public RigParam Param = RigParam.pmNone;    // report this param if bits match
  
        public BitMask()
        {
        }
    }
}
