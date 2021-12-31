using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScoutBase.CAT
{
    public static class Helpers
    {
        /// <summary>
        /// Returns true if running under Linux/Mono
        /// </summary>
        public static bool IsMono
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }


    }
}
