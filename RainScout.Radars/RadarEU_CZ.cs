using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using RainScout.Core;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Net.Cache;
using System.IO;
using System.IO.IsolatedStorage;
using GMap.NET;
using GMap.NET.Projections;
using Newtonsoft.Json;

namespace RainScout.Radars
{
    [Serializable]
    public class RadarEU_CZ : RainScout.Radars.GenericRadar
    {
        string BaseURL = "http://www.radareu.cz/data/radar/";

        // Radar images
        private Bitmap IntensityImage = null;
        private Bitmap CloudTopsImage = null;
        private Bitmap LightningImage = null;

        // values
        private int[,] LightningValues = null;

        // Radar legend
        private ValueColorTable CloudTopsLegend = new ValueColorTable();
        private ValueColorTable IntensityLegend = new ValueColorTable();
        private ValueColorTable LightningLegend = new ValueColorTable();

        // update cylce in seconds
        private int UpdateCycle = 5 * 60;

        // map zoom level
        private readonly int MapZoom = 20;

        public RadarEU_CZ()
        {
            Name = "Radar EU";
            Source = "http://www.radareu.cz";

            Left = -14.5;
            Right = 45.25;
            Top = 72.5;
            Bottom = 31.0;

            RadarLayers.Add(RADARLAYER.INTENSITY);
            RadarLayers.Add(RADARLAYER.LIGHTNING);

            // initialize intensity dictionary
            IntensityLegend.Add(-1,Color.FromArgb(112, 0, 0, 0));
            IntensityLegend.Add(0,Color.Transparent);
            IntensityLegend.Add(1, Color.FromArgb(255, 1, 1, 1));
            IntensityLegend.Add(4,Color.FromArgb(255, 55, 1, 117));
            IntensityLegend.Add(8,Color.FromArgb(255, 7, 0, 246));
            IntensityLegend.Add(16,Color.FromArgb(255, 0, 114, 198));
            IntensityLegend.Add(20,Color.FromArgb(255, 1, 162, 1));
            IntensityLegend.Add(24,Color.FromArgb(255, 0, 195, 5));
            IntensityLegend.Add(28,Color.FromArgb(255, 50, 214, 2));
            IntensityLegend.Add(32,Color.FromArgb(255, 155, 229, 7));
            IntensityLegend.Add(36,Color.FromArgb(255, 221, 218, 0));
            IntensityLegend.Add(40,Color.FromArgb(255, 247, 181, 1));
            IntensityLegend.Add(44,Color.FromArgb(255, 254, 129, 3));
            IntensityLegend.Add(48,Color.FromArgb(255, 251, 86, 0));
            IntensityLegend.Add(52,Color.FromArgb(255, 252, 2, 6));
            IntensityLegend.Add(56,Color.FromArgb(255, 150, 6, 10));
            IntensityLegend.Add(60,Color.FromArgb(255, 255, 255, 255));

        }

        private Bitmap GetRadarImage(DateTime utc)
        {
            // gets actual radar image from url
            string imagename = "radar.anim." + utc.ToString("yyyyMMdd") + "." + utc.ToString("HHmm") + ".0.png";
            Bitmap image1 = null;
            Bitmap image2 = null;
            string url = BaseURL + imagename;
            try
            {
                var request = WebRequest.Create(url);

                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        image1 = new Bitmap(stream);
                    }
                }

                // crop image
                Rectangle crop = new Rectangle();
                crop.X = 2;
                crop.Y = 2;
                crop.Width = image1.Width - crop.X;
                crop.Height = image1.Height - crop.Y;
                image2 = new Bitmap(crop.Width, crop.Height);
                using (Graphics g = Graphics.FromImage(image2))
                {
                    g.DrawImage(image1, -crop.X, -crop.Y);
                }


                image2.Save("RadarEU_CZ.png");
                this.Timestamp = utc;
            }
            catch (Exception ex)
            {
                // do nothing
                return null;
            }

            return image2;
        }

        public void GetRadarImages()
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

            IntensityImage = bm;

            GetLightningFromJSON();

//            SaveLightningValuesAsCSV();
        }

        private void GetLightningFromJSON()
        {
            if (IntensityImage == null)
                return;


            int zoom = 5;

            GPoint dsttl = MercatorProjection.Instance.FromLatLngToPixel(new PointLatLng(this.Top, this.Left), zoom);
            GPoint dstbr = MercatorProjection.Instance.FromLatLngToPixel(new PointLatLng(this.Bottom, this.Right), zoom);
            int dstwidth = (int)dstbr.X - (int)dsttl.X;
            int dstheight = (int)dstbr.Y - (int)dsttl.Y;

            LightningValues = new int[dstwidth, dstheight];

            string baseurl = "https://map.blitzortung.org/GEOjson/getjson.php?f=s&n=";
            for (int i = 0; i <=23; i++) 
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseurl + i.ToString("00"));
                    RequestCachePolicy policy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    request.CachePolicy = policy;
                    request.CookieContainer = new CookieContainer();
                    request.Referer = "https://map.blitzortung.org/";
                    // get html page
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                            string json = reader.ReadToEnd();
                            dynamic root = JsonConvert.DeserializeObject(json);
                            foreach (dynamic entry in root)
                            {
                                double lon = entry[0];
                                double lat = entry[1];
                                DateTime time = entry[2];
                                int age = (int)(DateTime.UtcNow - time).TotalMinutes;
                                GPoint p = MercatorProjection.Instance.FromLatLngToPixel(new PointLatLng(lat,lon), zoom);
                                int x = (int)p.X - (int)dsttl.X;
                                int y = (int)p.Y - (int)dsttl.Y;

                                if ((x > 1) && (x < dstwidth - 1) && (y >= 1) && (y < dstheight - 1))
                                {
                                    LightningValues[x, y] = age;
                                    LightningValues[x, y + 1] = age;
                                    LightningValues[x + 1, y] = age;
                                    LightningValues[x + 1, y + 1] = age;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }

        public override Bitmap GetRadarImage()
        {
            // check for last update and get new images if necessary
            if (Timestamp.AddSeconds(UpdateCycle) < DateTime.UtcNow)
            {
                GetRadarImages();
            }

            return IntensityImage;
        }

        public override int[,] GetRadarLayer(RADARLAYER layer)
        {
            // check for last update and get new images if necessary
            if (Timestamp.AddSeconds(UpdateCycle) < DateTime.UtcNow)
            {
                GetRadarImages();
            }

            switch (layer)
            {
                case RADARLAYER.INTENSITY: return GetValuesFromImage(IntensityImage, IntensityLegend, NEARESTCOLORSTRATEGY.RGB);
                case RADARLAYER.LIGHTNING: return LightningValues;
            }

            return null;
        }

        private int[,] GetValuesFromImage(Bitmap image, ValueColorTable legend, NEARESTCOLORSTRATEGY strategy)
        {
            if (image == null)
                return null;


            int[,] values = new int[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color c = image.GetPixel(x, y);
                    try
                    {
                        if (c.A == 0)
                            values[x, y] = 0;
                        else if ((c.A > 0) && (c.A < 255))
                            values[x, y] = -1;
                        else
                        {
                            int v = legend.GetValueFromColor(c, strategy);
                            values[x, y] = v;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error getting color at (" + x.ToString() + "," + y.ToString() + ") - " + c.ToString() + ": " + ex.Message);
                    }
                }
            }

            return values;
        }

        private void SaveLightningValuesAsCSV()
        {
            if (LightningImage == null)
                return;

            using (StreamWriter sw = new StreamWriter(File.OpenWrite("LightningValues.csv")))
            {
                for (int x = 0; x < LightningImage.Width; x++)
                {
                    sw.Write(x.ToString() + ";");
                }
                sw.WriteLine();

                for (int x = 0; x < LightningImage.Height; x++)
                {
                    for (int y = 0; y < LightningImage.Width; y++)
                    {
                        Color c = LightningImage.GetPixel(y, x);
                        sw.Write(LightningLegend.GetValueFromColor(c, NEARESTCOLORSTRATEGY.RGB).ToString() + ";");
                    }
                    sw.WriteLine();
                }
            }
        }

    }
}
