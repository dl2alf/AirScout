using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.CAT
{
    [Flags]
    public enum ValueFormat
    {
        vfNone,
        vfText,     // asc codes
        vfBinL,     // integer, little endian
        vfBinB,     // integer, big endian
        vfBcdLU,    // BCD little endian unsigned
        vfBcdLS,    // BCD little endian signed; sign in high byte (00 or FF)
        vfBcdBU,    // big endian
        vfBcdBS,    // big endian
        vfYaesu,    // format invented by Yaesu
                    //  Added by RA6UAZ for Icom Marine Radio NMEA Command
        vfDPIcom,   // format Decimal point by Icom
        vfTextUD    // Yaesu: text, but sign is U for + and D for -
    }
}
