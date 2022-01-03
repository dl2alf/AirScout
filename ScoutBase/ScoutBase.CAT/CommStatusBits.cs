using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScoutBase.CAT
{
    public class CommStatusBits
    {
        public bool Cts { get; set; } = false;
        public bool Dsr { get; set; } = false;

        public CommStatusBits()
        {

        }

        public CommStatusBits(bool cts, bool dsr)
        {
            Cts = cts;
            Dsr = dsr;
        }
    }
}
