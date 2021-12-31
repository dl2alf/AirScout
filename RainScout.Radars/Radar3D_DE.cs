using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using System.IO;
using System.Net.Cache;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using RainScout.Core;
using GMap.NET;
using Newtonsoft.Json;
using GMap.NET.Projections;

namespace RainScout.Radars
{
    [Serializable]
    public class Radar3D_DE : RainScout.Radars.GenericRadar
    {
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

        public Radar3D_DE()
        {
            Name = "Radar3D DE";
            Source = "https://kachelmannwetter.com/";

            // set bounds
            Left = 1;
            Right = 17;
            Top = 55.9;
            Bottom = 47;

            RadarLayers.Add(RADARLAYER.INTENSITY);
            RadarLayers.Add(RADARLAYER.CLOUDTOPS);
            RadarLayers.Add(RADARLAYER.LIGHTNING);

            // intialize intensity dictionary
            IntensityLegend.Add(-1, Color.FromArgb(112, 0, 0, 0));
            IntensityLegend.Add(0, Color.FromArgb(0,0,0,0));
            IntensityLegend.Add(0, Color.Transparent);
            IntensityLegend.Add(1, ColorTranslator.FromHtml("#828282"));
            IntensityLegend.Add(2, ColorTranslator.FromHtml("#8C8C8C"));
            IntensityLegend.Add(3, ColorTranslator.FromHtml("#A0A0A0"));
            IntensityLegend.Add(4, ColorTranslator.FromHtml("#AFAFAF"));
            IntensityLegend.Add(5, ColorTranslator.FromHtml("#BEBEBE"));
            IntensityLegend.Add(6, ColorTranslator.FromHtml("#CDCDCD"));
            IntensityLegend.Add(7, ColorTranslator.FromHtml("#DCDCDC"));
            IntensityLegend.Add(8, ColorTranslator.FromHtml("#8FE7E1"));
            IntensityLegend.Add(9, ColorTranslator.FromHtml("#51EFE5"));
            IntensityLegend.Add(10, ColorTranslator.FromHtml("#22CFEE"));
            IntensityLegend.Add(11, ColorTranslator.FromHtml("#1FBFF0"));
            IntensityLegend.Add(12, ColorTranslator.FromHtml("#1CB0F2"));
            IntensityLegend.Add(13, ColorTranslator.FromHtml("#19A1F3"));
            IntensityLegend.Add(14, ColorTranslator.FromHtml("#127AF2"));
            IntensityLegend.Add(15, ColorTranslator.FromHtml("#127AF2"));
            IntensityLegend.Add(16, ColorTranslator.FromHtml("#0B53F2"));
            IntensityLegend.Add(17, ColorTranslator.FromHtml("#052EF2"));
            IntensityLegend.Add(18, ColorTranslator.FromHtml("#052EF1"));
            IntensityLegend.Add(19, ColorTranslator.FromHtml("#2BFF2D"));
            IntensityLegend.Add(20, ColorTranslator.FromHtml("#29F62B"));
            IntensityLegend.Add(21, ColorTranslator.FromHtml("#27ED29"));
            IntensityLegend.Add(22, ColorTranslator.FromHtml("#25E427"));
            IntensityLegend.Add(23, ColorTranslator.FromHtml("#23DA25"));
            IntensityLegend.Add(24, ColorTranslator.FromHtml("#21D123"));
            IntensityLegend.Add(25, ColorTranslator.FromHtml("#1FC821"));
            IntensityLegend.Add(26, ColorTranslator.FromHtml("#1FC821"));
            IntensityLegend.Add(27, ColorTranslator.FromHtml("#1EC01F"));
            IntensityLegend.Add(28, ColorTranslator.FromHtml("#1EC01F"));
            IntensityLegend.Add(29, ColorTranslator.FromHtml("#1CB81D"));
            IntensityLegend.Add(30, ColorTranslator.FromHtml("#1AB01C"));
            IntensityLegend.Add(31, ColorTranslator.FromHtml("#19A81A"));
            IntensityLegend.Add(32, ColorTranslator.FromHtml("#17A018"));
            IntensityLegend.Add(33, ColorTranslator.FromHtml("#159816"));
            IntensityLegend.Add(34, ColorTranslator.FromHtml("#139015"));
            IntensityLegend.Add(35, ColorTranslator.FromHtml("#FEFD34"));
            IntensityLegend.Add(36, ColorTranslator.FromHtml("#F9EE31"));
            IntensityLegend.Add(37, ColorTranslator.FromHtml("#F2DF2E"));
            IntensityLegend.Add(38, ColorTranslator.FromHtml("#ECCF2B"));
            IntensityLegend.Add(39, ColorTranslator.FromHtml("#E6BF28"));
            IntensityLegend.Add(40, ColorTranslator.FromHtml("#E6BF28"));
            IntensityLegend.Add(41, ColorTranslator.FromHtml("#ECB326"));
            IntensityLegend.Add(42, ColorTranslator.FromHtml("#F1A724"));
            IntensityLegend.Add(43, ColorTranslator.FromHtml("#F79A23"));
            IntensityLegend.Add(44, ColorTranslator.FromHtml("#FD8E22"));
            IntensityLegend.Add(45, ColorTranslator.FromHtml("#FD8E22"));
            IntensityLegend.Add(46, ColorTranslator.FromHtml("#FD691E"));
            IntensityLegend.Add(47, ColorTranslator.FromHtml("#FC431A"));
            IntensityLegend.Add(48, ColorTranslator.FromHtml("#FC1918"));
            IntensityLegend.Add(49, ColorTranslator.FromHtml("#FC0017"));
            IntensityLegend.Add(50, ColorTranslator.FromHtml("#FC0017"));
            IntensityLegend.Add(51, ColorTranslator.FromHtml("#F20016"));
            IntensityLegend.Add(52, ColorTranslator.FromHtml("#E80015"));
            IntensityLegend.Add(53, ColorTranslator.FromHtml("#DD0013"));
            IntensityLegend.Add(54, ColorTranslator.FromHtml("#D40012"));
            IntensityLegend.Add(55, ColorTranslator.FromHtml("#D40012"));
            IntensityLegend.Add(56, ColorTranslator.FromHtml("#C70010"));
            IntensityLegend.Add(57, ColorTranslator.FromHtml("#B9000E"));
            IntensityLegend.Add(58, ColorTranslator.FromHtml("#AC000C"));
            IntensityLegend.Add(59, ColorTranslator.FromHtml("#9E000A"));
            IntensityLegend.Add(60, ColorTranslator.FromHtml("#FEC8FE"));
            IntensityLegend.Add(61, ColorTranslator.FromHtml("#F3B4F3"));
            IntensityLegend.Add(62, ColorTranslator.FromHtml("#E7A0E7"));
            IntensityLegend.Add(63, ColorTranslator.FromHtml("#DB8CDB"));
            IntensityLegend.Add(64, ColorTranslator.FromHtml("#CF78CF"));
            IntensityLegend.Add(65, ColorTranslator.FromHtml("#C464C4"));
            IntensityLegend.Add(66, ColorTranslator.FromHtml("#B850B8"));
            IntensityLegend.Add(67, ColorTranslator.FromHtml("#AD3CAD"));
            IntensityLegend.Add(68, ColorTranslator.FromHtml("#A128A1"));
            IntensityLegend.Add(69, ColorTranslator.FromHtml("#961396"));
            IntensityLegend.Add(70, ColorTranslator.FromHtml("#8A008A"));
            IntensityLegend.Add(100, ColorTranslator.FromHtml("#FFFFFF"));


            // initialize cloud tops dictionary
            CloudTopsLegend.Add(-1, Color.FromArgb(112, 0, 0, 0));
            CloudTopsLegend.Add(0, Color.FromArgb(0, 0, 0, 0));
            CloudTopsLegend.Add(0, Color.Transparent);
            CloudTopsLegend.Add(1, Color.FromArgb(255, 170, 170, 170));
            CloudTopsLegend.Add(500, Color.FromArgb(255, 226, 255, 255));
            CloudTopsLegend.Add(1000, Color.FromArgb(255, 180, 240, 250));
            CloudTopsLegend.Add(1500, Color.FromArgb(255, 150, 210, 250));
            CloudTopsLegend.Add(2000, Color.FromArgb(255, 120, 185, 250));
            CloudTopsLegend.Add(2500, Color.FromArgb(255, 80, 165, 245));
            CloudTopsLegend.Add(3000, Color.FromArgb(255, 60, 150, 245));
            CloudTopsLegend.Add(3500, Color.FromArgb(255, 45, 155, 150));
            CloudTopsLegend.Add(4000, Color.FromArgb(255, 30, 180, 30));
            CloudTopsLegend.Add(4500, Color.FromArgb(255, 55, 210, 60));
            CloudTopsLegend.Add(5000, Color.FromArgb(255, 80, 240, 80));
            CloudTopsLegend.Add(5500, Color.FromArgb(255, 150, 245, 140));
            CloudTopsLegend.Add(6000, Color.FromArgb(255, 200, 255, 190));
            CloudTopsLegend.Add(6500, Color.FromArgb(255, 255, 250, 170));
            CloudTopsLegend.Add(7000, Color.FromArgb(255, 255, 232, 120));
            CloudTopsLegend.Add(7500, Color.FromArgb(255, 255, 192, 60));
            CloudTopsLegend.Add(8000, Color.FromArgb(255, 255, 160, 0));
            CloudTopsLegend.Add(8500, Color.FromArgb(255, 255, 96, 0));
            CloudTopsLegend.Add(9000, Color.FromArgb(255, 245, 50, 0));
            CloudTopsLegend.Add(9500, Color.FromArgb(255, 225, 20, 0));
            CloudTopsLegend.Add(10000, Color.FromArgb(255, 192, 0, 0));
            CloudTopsLegend.Add(10500, Color.FromArgb(255, 165, 0, 0));
            CloudTopsLegend.Add(11000, Color.FromArgb(255, 169, 0, 186));
            CloudTopsLegend.Add(11500, Color.FromArgb(255, 236, 59, 255));
            CloudTopsLegend.Add(20000, Color.FromArgb(255, 255, 0, 255));

        }

        private Bitmap GetRawImage(string url, string search, Color inrange, Color outofrange, int shrinkleft, int shrinktop, int shrinkright, int shrinkbottom, string saveraw)
        {
            Bitmap image1 = null;
            Bitmap image2 = null;
            Bitmap image3 = null;
            try
            {
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
                        int start = 0;
                        start = content.IndexOf(search, start) - 12;
                        // return on no image found
                        if (start <= 0)
                            return null;
                        int stop = content.IndexOf(".png", start) + 4;
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

                            // Test
                            Color c;
                            c = image2.GetPixel(10, 10);
                            c = image2.GetPixel(600, 350);

                            // be aware that there are two representations of transparent background:
                            // image.MakeTransparent uses ARGB(0,0,0,0)
                            // Color.Transparent uses ARGB(0,255,255,255)

                            // make out of range background transparent
                            image2.MakeTransparent(outofrange);

                            // convert out of range color into semi-transparent grey
                            ColorMap[] colormap = new ColorMap[1];
                            colormap[0] = new ColorMap();
                            colormap[0].OldColor = Color.FromArgb(0, 0, 0, 0);
                            colormap[0].NewColor = Color.FromArgb(112, 0, 0, 0);
                            ImageAttributes attr = new ImageAttributes();
                            attr.SetRemapTable(colormap);
                            // Draw using the color map
                            using (Graphics g = Graphics.FromImage(image2))
                            {
                                Rectangle rect = new Rectangle(0, 0, image2.Width, image2.Height);
                                g.DrawImage(image2, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
                            }

                            // make in range background transparent
                            image2.MakeTransparent(inrange);
                            // convert in range color into Color.Transparent
                            colormap = new ColorMap[1];
                            colormap[0] = new ColorMap();
                            colormap[0].OldColor = Color.FromArgb(0, 0, 0, 0);
                            colormap[0].NewColor = Color.Transparent;
                            attr = new ImageAttributes();
                            attr.SetRemapTable(colormap);
                            // Draw using the color map & stretch
                            using (Graphics g = Graphics.FromImage(image2))
                            {
                                Rectangle rect = new Rectangle(0, 0, image2.Width, image2.Height);
                                g.DrawImage(image2, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while processing picture: " + ex.Message);
                        }
                    }
                }

                // return if processing fails
                if (image2 == null)
                    return null;


                // extract timestamp from url
                DateTime utc = DateTime.UtcNow;
                try
                {
                    utc = new DateTime(
                        System.Convert.ToInt32(imageurl.Substring(68, 4)),
                        System.Convert.ToInt32(imageurl.Substring(73, 2)),
                        System.Convert.ToInt32(imageurl.Substring(76, 2)),
                        System.Convert.ToInt32(imageurl.Substring(79, 2)),
                        System.Convert.ToInt32(imageurl.Substring(82, 2)),
                        0,
                        DateTimeKind.Utc);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                this.Timestamp = utc;

                return image2;
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
            for (int i = 0; i <= 23; i++)
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
                                GPoint p = MercatorProjection.Instance.FromLatLngToPixel(new PointLatLng(lat, lon), zoom);
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

        private void GetRadarImages()
        {
            IntensityImage = GetRawImage(
                "https://kachelmannwetter.com/de/radar-standard",
                ".kachelmannwetter.com/images/data/cache/radar/radar",
                Color.FromArgb(255, 110, 110, 110),
                Color.FromArgb(255, 143, 143, 143),
                0, 0, 0, 3,
                "RawIntensityImage.png");
            CloudTopsImage = GetRawImage(
                "https://kachelmannwetter.com/de/3d-radar-analyse/deutschland/echo-tops-18dbz.html",
                ".kachelmannwetter.com/images/data/cache/radar3d/radar3d",
                Color.FromArgb(255, 170, 170, 170),
                Color.FromArgb(255, 255, 255, 255),
                //                2,2,0,140,
                2, 3, 0, 140,
                "RawCloudTopsImage.png");

            GetLightningFromJSON();

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
                case RADARLAYER.CLOUDTOPS: return GetValuesFromImage(CloudTopsImage, CloudTopsLegend, NEARESTCOLORSTRATEGY.RGB);
                case RADARLAYER.LIGHTNING: return LightningValues;
            }

            return null;
        }

        private void SaveIntensityColorsAsCSV()
        {
            if (IntensityImage == null)
                return;

            Bitmap test = new Bitmap(IntensityImage.Width, IntensityImage.Height);

            using (StreamWriter sw = new StreamWriter(File.OpenWrite("IntensityColors.csv")))
            {
                for (int x = 0; x < IntensityImage.Width; x++)
                {
                    sw.Write(x.ToString() + ";");
                }
                sw.WriteLine();

                for (int x = 0; x < IntensityImage.Height; x++)
                {
                    for (int y = 0; y < IntensityImage.Width; y++)
                    {
                        Color c = IntensityImage.GetPixel(y, x);
                        sw.Write(ColorTranslator.ToHtml(c).Replace("#FF", "#") + ";") ;
                        test.SetPixel(y, x, c);
                    }
                    sw.WriteLine();
                }
            }
            test.Save("Intensity2.png", ImageFormat.Png);
        }

        private void SaveIntensityValuesAsCSV()
        {
            if (IntensityImage == null)
                return;

            using (StreamWriter sw = new StreamWriter(File.OpenWrite("IntensityValues.csv")))
            {
                for (int x = 0; x < IntensityImage.Width; x++)
                {
                    sw.Write(x.ToString() + ";");
                }
                sw.WriteLine();

                for (int x = 0; x < IntensityImage.Height; x++)
                {
                    for (int y = 0; y < IntensityImage.Width; y++)
                    {
                        Color c = IntensityImage.GetPixel(y, x);
                        sw.Write(IntensityLegend.GetValueFromColor(c, NEARESTCOLORSTRATEGY.RGB).ToString() + ";");
                    }
                    sw.WriteLine();
                }
            }
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
                            values[x, y] = legend.GetValueFromColor(c, strategy);
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

    }

}
