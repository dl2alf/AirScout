using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;



namespace AirScout.Database.Core
{
    public static class Constants
    {
        public static class ElevationModel
        {
            public static string SRTM1 = "SRTM1";
            public static string SRTM3 = "SRTM3";
            public static string GLOBE = "GLOBE";

            public static bool IsAnyOf(string model)
            {
                if (String.Compare(model,SRTM1,true) >= 0)
                    return true;
                if (String.Compare(model,SRTM3,true) >= 0)
                    return true;
                if (String.Compare(model,GLOBE,true) >= 0)
                    return true;
                return false;
            }
        }
    }
}
