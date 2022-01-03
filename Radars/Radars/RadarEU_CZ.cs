using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;

namespace RainScout.Radars
{
    public class RadarEU_CZ : RainScout.Radars.Generic
    {
        string BaseURL = "http://www.radareu.cz/data/radar/";

        public RadarEU_CZ()
        {
            Left = -14.5;
            Right = 45.25;
            Top = 72.5;
            Bottom = 31.0;
            Source = "http://www.radareu.cz";
        }

        public Bitmap GetRadarImage(DateTime utc)
        {
            // gets actual radar image from url
            string imagename = "radar.anim." + utc.ToString("yyyyMMdd") + "." + utc.ToString("HHmm") + ".0.png";
            Bitmap image = null;
            URL = BaseURL + imagename;
            try
            {
                var request = WebRequest.Create(URL);

                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        image = new Bitmap(stream);
                    }
                }
                image.Save("RadarEU_CZ.png");
                this.Timestamp = utc;
            }
            catch (Exception ex)
            {
                // do nothing
                return null;
            }
            return image;
        }

        public Bitmap GetRadarImage()
        {
            // get current date and time string
            DateTime utc = DateTime.UtcNow;
            // round it to 15min interval
            if (utc.Minute < 15)
                utc = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 0, 0, DateTimeKind.Utc);
            else if (utc.Minute < 30)
                utc = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 15, 0, DateTimeKind.Utc);
            else if (utc.Minute < 45)
                utc = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 30, 0, DateTimeKind.Utc);
            else if (utc.Minute < 60)
                utc = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 45, 0, DateTimeKind.Utc);
            Bitmap bm = GetRadarImage(utc);
            if (bm == null)
            {
                // call failed --> try with 1 period earlier
                utc = utc - new TimeSpan(0, 15, 0);
                GetRadarImage(utc);
            }
            return bm;
        }
    }
}
