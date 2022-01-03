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
using NGrib;
using NGrib.Grib2;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace RainScout.Radars
{
    [Serializable]
    public class RadarHD_EU : RainScout.Radars.GenericRadar
    {
        string BaseURL = "https://www.rainviewer.com/weather-radar-map-live.html";

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

        // Numeric weather prediction
        private Dictionary<int, Dictionary<Coordinate, float?>> NWP = new Dictionary<int, Dictionary<Coordinate, float?>>();

        public RadarHD_EU()
        {
            Name = "RadarHD EU";
            Source = "https://www.rainviewer.com/";

            Left = -14.5;
            Right = 45.25;
            Top = 72.5;
            Bottom = 31.0;

            RadarLayers.Add(RADARLAYER.INTENSITY);
            RadarLayers.Add(RADARLAYER.CLOUDTOPS);
            RadarLayers.Add(RADARLAYER.LIGHTNING);

            // initialize intensity dictionary
            IntensityLegend.Add(-1,Color.FromArgb(0, 0, 0, 0));
            IntensityLegend.Add(0, Color.Transparent);
            IntensityLegend.Add(1, ColorTranslator.FromHtml("#626262"));
            IntensityLegend.Add(5, ColorTranslator.FromHtml("#28EDEB"));
            IntensityLegend.Add(10, ColorTranslator.FromHtml("#19A1F0"));
            IntensityLegend.Add(15, ColorTranslator.FromHtml("#0412EF"));
            IntensityLegend.Add(20, ColorTranslator.FromHtml("#2BFE2D")); 
            IntensityLegend.Add(25, ColorTranslator.FromHtml("#1FC721"));
            IntensityLegend.Add(30, ColorTranslator.FromHtml("#149015"));
            IntensityLegend.Add(35, ColorTranslator.FromHtml("#FDFD35"));
            IntensityLegend.Add(40, ColorTranslator.FromHtml("#E3BF27"));
            IntensityLegend.Add(45, ColorTranslator.FromHtml("#FC8E22"));
            IntensityLegend.Add(50, ColorTranslator.FromHtml("#F90017"));
            IntensityLegend.Add(55, ColorTranslator.FromHtml("#D10011"));
            IntensityLegend.Add(60, ColorTranslator.FromHtml("#BE000F"));
            IntensityLegend.Add(65, ColorTranslator.FromHtml("#FA00F9"));
            IntensityLegend.Add(70, ColorTranslator.FromHtml("#9856C6"));
            IntensityLegend.Add(75, ColorTranslator.FromHtml("#EBEBEB"));

            // initialize cloud tops dictionary
            CloudTopsLegend.Add(-1, Color.FromArgb(0, 0, 0, 0));
            CloudTopsLegend.Add(0, Color.Transparent);
            CloudTopsLegend.Add(-92, ColorTranslator.FromHtml("#000000"));
            CloudTopsLegend.Add(-90, ColorTranslator.FromHtml("#181818"));
            CloudTopsLegend.Add(-88, ColorTranslator.FromHtml("#2C1C1C"));
            CloudTopsLegend.Add(-86, ColorTranslator.FromHtml("#411416"));
            CloudTopsLegend.Add(-84, ColorTranslator.FromHtml("#550C10"));
            CloudTopsLegend.Add(-82 , ColorTranslator.FromHtml("#67040C"));
            CloudTopsLegend.Add(-80, ColorTranslator.FromHtml("#7D0007"));
            CloudTopsLegend.Add(-78, ColorTranslator.FromHtml("#900008"));
            CloudTopsLegend.Add(-76, ColorTranslator.FromHtml("#A6000B"));
            CloudTopsLegend.Add(-74, ColorTranslator.FromHtml("#B8000E"));
            CloudTopsLegend.Add(-72, ColorTranslator.FromHtml("#C60010"));
            CloudTopsLegend.Add(-70, ColorTranslator.FromHtml("#DD0013"));
            CloudTopsLegend.Add(-69, ColorTranslator.FromHtml("#F10016"));
            CloudTopsLegend.Add(-68, ColorTranslator.FromHtml("#FC0018"));
            CloudTopsLegend.Add(-67, ColorTranslator.FromHtml("#FC2518"));
            CloudTopsLegend.Add(-66, ColorTranslator.FromHtml("#FC3E1A"));
            CloudTopsLegend.Add(-65, ColorTranslator.FromHtml("#FC5C1C"));
            CloudTopsLegend.Add(-64, ColorTranslator.FromHtml("#FD7B20"));
            CloudTopsLegend.Add(-63, ColorTranslator.FromHtml("#FD9924"));
            CloudTopsLegend.Add(-62, ColorTranslator.FromHtml("#FDB628"));
            CloudTopsLegend.Add(-61, ColorTranslator.FromHtml("#FEC82B"));
            CloudTopsLegend.Add(-60, ColorTranslator.FromHtml("#FEE430"));
            CloudTopsLegend.Add(-59, ColorTranslator.FromHtml("#FDFE35"));
            CloudTopsLegend.Add(-58, ColorTranslator.FromHtml("#DFFF3D"));
            CloudTopsLegend.Add(-57, ColorTranslator.FromHtml("#C2FF4F"));
            CloudTopsLegend.Add(-56, ColorTranslator.FromHtml("#AEFF5F"));
            CloudTopsLegend.Add(-55, ColorTranslator.FromHtml("#97FF74"));
            CloudTopsLegend.Add(-54, ColorTranslator.FromHtml("#80FF8A"));
            CloudTopsLegend.Add(-53, ColorTranslator.FromHtml("#68FFA2"));
            CloudTopsLegend.Add(-52, ColorTranslator.FromHtml("#53FFBA"));
            CloudTopsLegend.Add(-51, ColorTranslator.FromHtml("#53FFBA"));
            CloudTopsLegend.Add(-50, ColorTranslator.FromHtml("#32FFEB"));
            CloudTopsLegend.Add(-49, ColorTranslator.FromHtml("#2BFAFE"));
            CloudTopsLegend.Add(-48, ColorTranslator.FromHtml("#26E2FD"));
            CloudTopsLegend.Add(-47, ColorTranslator.FromHtml("#21C8FD"));
            CloudTopsLegend.Add(-46, ColorTranslator.FromHtml("#1CB2FC"));
            CloudTopsLegend.Add(-45, ColorTranslator.FromHtml("#1799FC"));
            CloudTopsLegend.Add(-44, ColorTranslator.FromHtml("#1385FC"));
            CloudTopsLegend.Add(-43, ColorTranslator.FromHtml("#0D64FB"));
            CloudTopsLegend.Add(-42, ColorTranslator.FromHtml("#0A52FB"));
            CloudTopsLegend.Add(-41, ColorTranslator.FromHtml("#0740FB"));
            CloudTopsLegend.Add(-40, ColorTranslator.FromHtml("#0520FB"));
            CloudTopsLegend.Add(-38, ColorTranslator.FromHtml("#0311E6"));
            CloudTopsLegend.Add(-36, ColorTranslator.FromHtml("#00035E"));
            CloudTopsLegend.Add(-34, ColorTranslator.FromHtml("#010794"));
            CloudTopsLegend.Add(-32, ColorTranslator.FromHtml("#EFEFEF"));
            CloudTopsLegend.Add(-30, ColorTranslator.FromHtml("#E1E1E1"));
            CloudTopsLegend.Add(-26, ColorTranslator.FromHtml("#D5D5D5"));
            CloudTopsLegend.Add(-22, ColorTranslator.FromHtml("#C9C9C9"));
            CloudTopsLegend.Add(-18, ColorTranslator.FromHtml("#BDBDBD"));
            CloudTopsLegend.Add(-14, ColorTranslator.FromHtml("#B3B3B3"));
            CloudTopsLegend.Add(-10, ColorTranslator.FromHtml("#ADADAD"));
            CloudTopsLegend.Add(-8, ColorTranslator.FromHtml("#A7A7A7"));
            CloudTopsLegend.Add(-6, ColorTranslator.FromHtml("#A1A1A1"));
            CloudTopsLegend.Add(-4, ColorTranslator.FromHtml("#9B9B9B"));
            CloudTopsLegend.Add(-2, ColorTranslator.FromHtml("#8F8F8F"));
        }

        private Bitmap GetIntensityTile(DateTime utc, int x, int y, int z, int colorscheme)
        {
            // gets tile from url
            Bitmap image1 = null;   // 1622129403
            Bitmap image2 = null;   // 1622128200
            utc = utc.AddMinutes(-1);
            DateTime imagetime = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, (int)(utc.Minute / 10) * 10, 0, DateTimeKind.Utc);
            int time = (int)(imagetime - new DateTime(1970, 1, 1)).TotalSeconds;
            string url = "https://tilecache.rainviewer.com/v2/radar/" + time.ToString() + "/256/" + z.ToString() + "/" + x.ToString() + "/" + y.ToString() + "/" + colorscheme.ToString() + "/1_1.png";
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

                this.Timestamp = utc;
            }
            catch (Exception ex)
            {
                // do nothing
                return null;
            }

            return image1;
        }

        private Bitmap GetCoverageTile(int x, int y, int z)
        {
            // gets tile from url
            Bitmap image1 = null;   // 1622129403
            Bitmap image2 = null;   // 1622128200
            // https://tilecache.rainviewer.com/v2/coverage/0/256/5/17/7.png
            string url = "https://tilecache.rainviewer.com/v2/coverage/0/256/" + z.ToString() + "/" +  "/" + x.ToString() + "/" + y.ToString() + ".png";
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

            }
            catch (Exception ex)
            {
                // do nothing
                return null;
            }

            return image1;
        }

        public Bitmap GetIntensityImage(string saveraw)
        {
            // get current date and time string
            DateTime utc = DateTime.UtcNow;

            int left = 14;
            int top = 6;
            int right = 20;
            int bottom = 13;

            Bitmap radar;
            Bitmap coverage;
            Bitmap image1 = new Bitmap((right - left) * 256, (bottom - top) * 256);
            Bitmap image2 = null;
            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    radar = GetIntensityTile(utc, x, y, 5, 6);
                    coverage = GetCoverageTile(x, y, 5);

                    using (Graphics g = Graphics.FromImage(image1))
                    {
                        try
                        {
                            if (radar != null)
                            {
                                Rectangle rect = new Rectangle((x - left) * 256, (y - top) * 256, 256, 256);
                                g.DrawImage(radar, rect);
                                g.DrawImage(coverage, rect);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                         
                    }
                }
            }

            if (image1 == null)
                return null;

            int shrinkleft = 250;
            int shrinktop = 170;
            int shrinkright = 115;
            int shrinkbottom = 30;

            Color outofrange = Color.Transparent;
            Color inrange = Color.Transparent;

            // crop image
            Rectangle crop = new Rectangle();
            crop.X = shrinkleft;
            crop.Y = shrinktop;
            crop.Width = image1.Width - shrinkleft - shrinkright;
            crop.Height = image1.Height - shrinktop - shrinkbottom;
            image2 = new Bitmap(crop.Width, crop.Height);
            using (Graphics g = Graphics.FromImage(image2))
            {
                g.DrawImage(image1, -crop.X, -crop.Y);
            }

            // save raw image if filename is not empty                        
            if (!String.IsNullOrEmpty(saveraw))
            {
                try
                {
                    image1.Save(saveraw, ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    // do nothing
                }
            }


            image1.Save("RadarHD_EU.png", ImageFormat.Png);
            
            return image2;
        }

        private Bitmap GetCloudTopsImage(string saveraw)
        {
            Bitmap image1 = null;
            Bitmap image2 = null;
            Bitmap image3 = null;
            int start = 0;
            int stop = 0;
            string search = "weather.us/images/data/cache/sat/sat_";
            try
            {
                string url = "https://weather.us/satellite/europe/top-alert-5min.html";
                RequestCachePolicy policy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.CachePolicy = policy;
                request.CookieContainer = new CookieContainer();
                string imageurl = "";
                // get html page
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        string content = reader.ReadToEnd();
                        // grab out the url of the latest image
                        start = content.IndexOf(search, start) - 13;
                        // return on no image found
                        if (start <= 0)
                            return null;
                        stop = content.IndexOf(".jpg", start) + 4;
                        if (stop < start)
                            return null;

                        // get the picture
                        try
                        {
                            imageurl = content.Substring(start, stop - start);
                            Console.WriteLine("Getting picture from web resource: " + imageurl);
                            HttpWebRequest picrequest = (HttpWebRequest)HttpWebRequest.Create(imageurl);
                            picrequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36";
                            picrequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                            picrequest.CookieContainer = new CookieContainer();
                            foreach (Cookie cookie in response.Cookies)
                            {
                                picrequest.CookieContainer.Add(cookie);
                            }

                            using (var picresponse = picrequest.GetResponse())
                            {
                                using (var picstream = picresponse.GetResponseStream())
                                {
                                    image1 = new Bitmap(picstream);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while getting picture: " + ex.Message);
                        }

                        // no picture loaded
                        if (image1 == null)
                            return null;

                        // save raw image if filename is not empty                        
                        if (!String.IsNullOrEmpty(saveraw))
                        {
                            try
                            {
                                image1.Save(saveraw, ImageFormat.Png);
                            }
                            catch (Exception ex)
                            {
                                // do nothing
                            }
                        }

                        // process image
                        try
                        {
                            // save raw image if filename is not empty                        
                            if (!String.IsNullOrEmpty(saveraw))
                            {
                                try
                                {
                                    image1.Save(saveraw, ImageFormat.Png);
                                }
                                catch (Exception ex)
                                {
                                    // do nothing
                                }
                            }

                            int zoom = 3;

                            GPoint srctl = PlateCarreeProjection.Instance.FromLatLngToPixel(new PointLatLng(this.Top, this.Left), zoom);
                            GPoint srcbr = PlateCarreeProjection.Instance.FromLatLngToPixel(new PointLatLng(this.Bottom, this.Right), zoom);
                            int srcwidth = (int)srcbr.X - (int)srctl.X;
                            int srcheight = (int)srcbr.Y - (int)srctl.Y;

                            GPoint dsttl = MercatorProjection.Instance.FromLatLngToPixel(new PointLatLng(this.Top, this.Left), zoom + 2);
                            GPoint dstbr = MercatorProjection.Instance.FromLatLngToPixel(new PointLatLng(this.Bottom, this.Right), zoom + 2);
                            int dstwidth = (int)dstbr.X - (int)dsttl.X;
                            int dstheight = (int)dstbr.Y - (int)dsttl.Y;
                            image2 = new Bitmap(dstwidth, dstheight);
                            for (int x = 0; x < dstwidth; x++)
                            {
                                for (int y = 0; y < dstheight; y++)
                                {
                                    PointLatLng p = MercatorProjection.Instance.FromPixelToLatLng(dsttl.X + x, dsttl.Y + y, zoom + 2);
                                    GPoint g = PlateCarreeProjection.Instance.FromLatLngToPixel(p, zoom);
                                    int srcx = (int)((g.X - srctl.X) * (double)image1.Width / (double)srcwidth);
                                    int srcy = (int)((g.Y - srctl.Y) * (double)image1.Height / (double)srcheight);
                                    Color c = Color.Green;
                                    if ((srcx >= 0) && (srcx < image1.Width) && (srcy >= 0) && (srcy < image1.Height))
                                    {
                                        c = image1.GetPixel(srcx, srcy);
                                    }
                                    image2.SetPixel(x, y, c);
                                }
                            }

                            // save raw image if filename is not empty                        
                            if (!String.IsNullOrEmpty(saveraw))
                            {
                                try
                                {
                                    image2.Save(saveraw, ImageFormat.Png);
                                }
                                catch (Exception ex)
                                {
                                    // do nothing
                                }
                            }

                            int shrinkleft = 280;
                            int shrinktop = 0;
                            int shrinkright = 0;
                            int shrinkbottom = 0;

                            Color outofrange = Color.Transparent;
                            Color inrange = Color.Transparent;

                            // crop image
                            Rectangle crop = new Rectangle();
                            crop.X = shrinkleft;
                            crop.Y = shrinktop;
                            crop.Width = image2.Width - shrinkleft - shrinkright;
                            crop.Height = image2.Height - shrinktop - shrinkbottom;
                            image3 = new Bitmap(crop.Width, crop.Height);
                            using (Graphics g = Graphics.FromImage(image3))
                            {
                                g.DrawImage(image2, -crop.X, -crop.Y);
                            }

                            // be aware that there are two representations of transparent background:
                            // image.MakeTransparent uses ARGB(0,0,0,0)
                            // Color.Transparent uses ARGB(0,255,255,255)

                            // make out of range background transparent
                            image3.MakeTransparent(outofrange);

                            // convert out of range color into semi-transparent grey
                            ColorMap[] colormap = new ColorMap[1];
                            colormap[0] = new ColorMap();
                            colormap[0].OldColor = Color.FromArgb(0, 0, 0, 0);
                            colormap[0].NewColor = Color.FromArgb(112, 0, 0, 0);
                            ImageAttributes attr = new ImageAttributes();
                            attr.SetRemapTable(colormap);
                            // Draw using the color map
                            using (Graphics g = Graphics.FromImage(image3))
                            {
                                Rectangle rect = new Rectangle(0, 0, image3.Width, image3.Height);
                                g.DrawImage(image3, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
                            }

                            // make in range background transparent
                            image3.MakeTransparent(inrange);
                            // convert in range color into Color.Transparent
                            colormap = new ColorMap[1];
                            colormap[0] = new ColorMap();
                            colormap[0].OldColor = Color.FromArgb(0, 0, 0, 0);
                            colormap[0].NewColor = Color.Transparent;
                            attr = new ImageAttributes();
                            attr.SetRemapTable(colormap);
                            // Draw using the color map & stretch
                            using (Graphics g = Graphics.FromImage(image3))
                            {
                                Rectangle rect = new Rectangle(0, 0, image3.Width, image3.Height);
                                g.DrawImage(image3, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while processing picture: " + ex.Message);
                        }
                    }
                }

                // return if processing fails
                if (image3 == null)
                    return null;


                // extract timestamp from url
                DateTime utc = DateTime.UtcNow;
                try
                {
                    start = imageurl.IndexOf(search) + search.Length;
                    utc = new DateTime(
                        System.Convert.ToInt32(imageurl.Substring(start, 4)),
                        System.Convert.ToInt32(imageurl.Substring(start + 5, 2)),
                        System.Convert.ToInt32(imageurl.Substring(start + 8, 2)),
                        System.Convert.ToInt32(imageurl.Substring(start + 11, 2)),
                        System.Convert.ToInt32(imageurl.Substring(start + 14, 2)),
                        0,
                        DateTimeKind.Utc);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                this.Timestamp = utc;

                return image3;
            }
            catch (Exception ex)
            {
                // do nothing
            }

            return null;
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

        private void GetTemperaturesFromGRIB(DateTime utc)
        {
            try
            {
                string baseurl = "https://opendata.dwd.de/weather/nwp/icon-eu/grib/12/t/";
                // calculate filename for download
                HTTPDirectorySearcher dir = new HTTPDirectorySearcher();
                List<HTTPDirectoryItem> files = dir.GetDirectoryInformation(baseurl);
                // get time of latest forecast run
                HTTPDirectoryItem item = files.Last(file => file.Name.Contains("icon-eu_europe_regular-lat-lon_model-level_"));
                if (item == null)
                    return;
                string fct = item.Name.Substring(item.Name.IndexOf("icon-eu_europe_regular-lat-lon_model-level_") + 43, 10);
                DateTime fctime = DateTime.MinValue;
                if (!DateTime.TryParse(fct.Substring(0, 4) + "-" + fct.Substring(4, 2) + "-" + fct.Substring(6, 2) + "T" + fct.Substring(8, 2) + ":00:00.000Z", 
                        System.Globalization.DateTimeFormatInfo.InvariantInfo, 
                        System.Globalization.DateTimeStyles.AssumeUniversal, 
                        out fctime))
                    return;
                fctime = fctime.ToUniversalTime();
                // calculate forecast offset
                int fchour = (int)(utc - fctime).TotalHours;
                string basefilename = "icon-eu_europe_regular-lat-lon_model-level_" + fct + "_" + fchour.ToString("000");
                // find files in directory
                item = files.First(file => file.Name.Contains(basefilename));
                // return on no files found
                if (item == null)
                    return;
                List<KeyValuePair<int, int>> levels = new List<KeyValuePair<int, int>>();
                levels.Add(new KeyValuePair<int, int>(51, 1000));
                levels.Add(new KeyValuePair<int, int>(47, 2000));
                levels.Add(new KeyValuePair<int, int>(43, 3000));
                levels.Add(new KeyValuePair<int, int>(40, 4000));
                levels.Add(new KeyValuePair<int, int>(37, 5000));
                levels.Add(new KeyValuePair<int, int>(34, 6000));
                levels.Add(new KeyValuePair<int, int>(32, 7000));
                levels.Add(new KeyValuePair<int, int>(29, 8000));
                levels.Add(new KeyValuePair<int, int>(27, 9000));
                levels.Add(new KeyValuePair<int, int>(25, 10000));
                levels.Add(new KeyValuePair<int, int>(22, 11000));
                levels.Add(new KeyValuePair<int, int>(20, 12000));
                levels.Add(new KeyValuePair<int, int>(17, 13000));
                levels.Add(new KeyValuePair<int, int>(14, 14000));
                levels.Add(new KeyValuePair<int, int>(12, 15000));
                levels.Add(new KeyValuePair<int, int>(10, 16000));
                levels.Add(new KeyValuePair<int, int>(8, 17000));
                levels.Add(new KeyValuePair<int, int>(7, 18000));
                levels.Add(new KeyValuePair<int, int>(5, 19000));
                levels.Add(new KeyValuePair<int, int>(4, 20000));
                 foreach (KeyValuePair<int,int> level in levels)
                {
                    NWP[level.Value] = new Dictionary<Coordinate, float?>();
                    string filename = basefilename + "_" + level.Key.ToString() + "_T.grib2.bz2";
                    string url = baseurl + filename;
                    AutoDecompressionWebClient client = new AutoDecompressionWebClient();
                    DOWNLOADFILESTATUS status = client.DownloadFileIfNewer(url, filename, true, true);
                    if ((status == DOWNLOADFILESTATUS.NEWER) || (status == DOWNLOADFILESTATUS.NOTNEWER))
                    {
                        Grib2Reader gr = new Grib2Reader("Z:\\Downloads\\DWD\\icon-eu_europe_regular-lat-lon_model-level_2021052812_000_10_T.grib2");
                        IEnumerable<NGrib.Grib2.Message> messages = gr.ReadMessages();
                        NGrib.Grib2.DataSet dataset = messages.ElementAt(0).DataSets.ElementAt(0);
                        IEnumerable<KeyValuePair<Coordinate, float?>> values = gr.ReadDataSetValues(dataset);
                        foreach (KeyValuePair<Coordinate, float?> value in values)
                        {
                            if ((value.Key.Longitude >= this.Left) && (value.Key.Longitude <= this.Right) && (value.Key.Latitude >= this.Bottom) && (value.Key.Latitude <= this.Top))
                            {
                                NWP[level.Value][value.Key] = value.Value;
                            }
                        }
                        int i = values.Count();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public override Bitmap GetRadarImage()
        {
            // check for last update and get new images if necessary
//            if (Timestamp.AddSeconds(UpdateCycle) < DateTime.UtcNow)
            {
                /*
                Left = -11;
                Right = 45;
                Top = 74;
                Bottom = 32;
                */

                Left = -11;
                Right = 40;
                Top = 71.5;
                Bottom = 33;

                IntensityImage = GetIntensityImage("Intensity_EU.png");
                //            GetTemperaturesFromGRIB(utc);
                CloudTopsImage = GetCloudTopsImage("CloudTops_EU.png");

                GetLightningFromJSON();

            }


            return IntensityImage;
        }

        public override int[,] GetRadarLayer(RADARLAYER layer)
        {
            // check for last update and get new images if necessary
  //          if (Timestamp.AddSeconds(UpdateCycle) < DateTime.UtcNow)
            {
                GetRadarImage();
            }

            switch (layer)
            {
                case RADARLAYER.INTENSITY: return GetValuesFromImage(IntensityImage, IntensityLegend, NEARESTCOLORSTRATEGY.RGB);
                case RADARLAYER.CLOUDTOPS: return GetCloudTopValuesFromImage(CloudTopsImage, CloudTopsLegend, NEARESTCOLORSTRATEGY.RGB);
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

        private int[,] GetCloudTopValuesFromImage(Bitmap image, ValueColorTable legend, NEARESTCOLORSTRATEGY strategy)
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
                            // convert °F into K
                            //                           v = (int)((v + 459.67) * 5.0 / 9.0);
                            v = (int)(((v - 32) * 5.0 / 9.0 - 15.0) / -56.5 * 11000 - 10000);
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
