using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using RainScout.Core;

namespace RainScout
{
    public class RadarMap
    {
        // color value for semi-transparent areas
        public Color ColorSemitransparent { get; set; }  = Color.FromArgb(60, 0, 0, 0);

        // parent control
        GMapControl ParentMap = null;

        // color legends
        ValueColorTable IntensityLegend = new ValueColorTable();
        ValueColorTable CloudTopsLegend = new ValueColorTable();
        ValueColorTable LightningLegend = new ValueColorTable();

        // map bounds
        public double Top { get; internal set; } = 0;
        public double Left { get; internal set; } = 0;
        public double Bottom { get; internal set; } = 0;
        public double Right { get; internal set; } = 0;

        // size
        public Size Size { get { return new Size(this.Width, this.Height); } }

        // dimension in px
        public int Width { get; internal set; } = 4096;
        public int Height { get; internal set; } = 4096;

        // bitmaps
        public Bitmap Intensity { get; internal set; } = null;
        public Bitmap CloudTops { get; internal set; } = null;
        public Bitmap Lightning { get; internal set; } = null;

        // maps containing info?
        public bool HasIntensity { get { return Intensity != null; } }
        public bool HasCloudTops { get { return CloudTops != null; } }
        public bool HasLightning { get { return Lightning != null; } }

        public DateTime Timestamp { get; set; } = DateTime.MinValue;

        public RadarMap(GMapControl parentmap, double left, double top, double right, double bottom, int width, int height)
        {
            ParentMap = parentmap;

            /*
            IntensityLegend.Add(-1, ColorSemitransparent);
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
            */

            // initialize intensity dictionary
            IntensityLegend.Add(-1, Color.FromArgb(112, 0, 0, 0));
            IntensityLegend.Add(0, Color.Transparent);
            IntensityLegend.Add(1, Color.FromArgb(255, 1, 1, 1));
            IntensityLegend.Add(4, Color.FromArgb(255, 55, 1, 117));
            IntensityLegend.Add(8, Color.FromArgb(255, 7, 0, 246));
            IntensityLegend.Add(16, Color.FromArgb(255, 0, 114, 198));
            IntensityLegend.Add(20, Color.FromArgb(255, 1, 162, 1));
            IntensityLegend.Add(24, Color.FromArgb(255, 0, 195, 5));
            IntensityLegend.Add(28, Color.FromArgb(255, 50, 214, 2));
            IntensityLegend.Add(32, Color.FromArgb(255, 155, 229, 7));
            IntensityLegend.Add(36, Color.FromArgb(255, 221, 218, 0));
            IntensityLegend.Add(40, Color.FromArgb(255, 247, 181, 1));
            IntensityLegend.Add(44, Color.FromArgb(255, 254, 129, 3));
            IntensityLegend.Add(48, Color.FromArgb(255, 251, 86, 0));
            IntensityLegend.Add(52, Color.FromArgb(255, 252, 2, 6));
            IntensityLegend.Add(56, Color.FromArgb(255, 150, 6, 10));
            IntensityLegend.Add(60, Color.FromArgb(255, 255, 255, 255));


            // initialize cloud top legend
            CloudTopsLegend.Add(-1, ColorSemitransparent);
            CloudTopsLegend.Add(0, Color.Transparent);
            CloudTopsLegend.Add(1, Color.FromArgb(255, 170, 170, 170));
            CloudTopsLegend.Add(500, Color.FromArgb(255, 226, 255, 255));
            CloudTopsLegend.Add(1000,Color.FromArgb(255, 180, 240, 250));
            CloudTopsLegend.Add(1500,Color.FromArgb(255, 150, 210, 250));
            CloudTopsLegend.Add(2000,Color.FromArgb(255, 120, 185, 250));
            CloudTopsLegend.Add(2500,Color.FromArgb(255, 80, 165, 245));
            CloudTopsLegend.Add(3000,Color.FromArgb(255, 60, 150, 245));
            CloudTopsLegend.Add(3500,Color.FromArgb(255, 45, 155, 150));
            CloudTopsLegend.Add(4000,Color.FromArgb(255, 30, 180, 30));
            CloudTopsLegend.Add(4500,Color.FromArgb(255, 55, 210, 60));
            CloudTopsLegend.Add(5000,Color.FromArgb(255, 80, 240, 80));
            CloudTopsLegend.Add(5500,Color.FromArgb(255, 150, 245, 140));
            CloudTopsLegend.Add(6000,Color.FromArgb(255, 200, 255, 190));
            CloudTopsLegend.Add(6500,Color.FromArgb(255, 255, 250, 170));
            CloudTopsLegend.Add(7000,Color.FromArgb(255, 255, 232, 120));
            CloudTopsLegend.Add(7500,Color.FromArgb(255, 255, 192, 60));
            CloudTopsLegend.Add(8000,Color.FromArgb(255, 255, 160, 0));
            CloudTopsLegend.Add(8500,Color.FromArgb(255, 255, 96, 0));
            CloudTopsLegend.Add(9000,Color.FromArgb(255, 245, 50, 0));
            CloudTopsLegend.Add(9500,Color.FromArgb(255, 225, 20, 0));
            CloudTopsLegend.Add(10000,Color.FromArgb(255, 192, 0, 0));
            CloudTopsLegend.Add(10500,Color.FromArgb(255, 165, 0, 0));
            CloudTopsLegend.Add(11000,Color.FromArgb(255, 169, 0, 186));
            CloudTopsLegend.Add(11500,Color.FromArgb(255, 236, 59, 255));
            CloudTopsLegend.Add(20000,Color.FromArgb(255, 255, 0, 255));

            // initialize lightning dictionary
            LightningLegend.Add(-1, Color.FromArgb(112, 0, 0, 0));
            LightningLegend.Add(0, Color.FromArgb(0, 0, 0, 0));
            LightningLegend.Add(0, Color.FromArgb(255, 0, 0, 0));
            LightningLegend.Add(0, Color.Transparent);
            LightningLegend.Add(1, ColorTranslator.FromHtml("#FFFFFF"));
            LightningLegend.Add(5, ColorTranslator.FromHtml("#FFFE85"));
            LightningLegend.Add(10, ColorTranslator.FromHtml("#FFFE34"));
            LightningLegend.Add(15, ColorTranslator.FromHtml("#FED82E"));
            LightningLegend.Add(20, ColorTranslator.FromHtml("#FDB127"));
            LightningLegend.Add(25, ColorTranslator.FromHtml("#FD8A21"));
            LightningLegend.Add(40, ColorTranslator.FromHtml("#FF5500"));
            LightningLegend.Add(80, ColorTranslator.FromHtml("#FF0000"));
            LightningLegend.Add(120, ColorTranslator.FromHtml("#BF0000"));

            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;

            Width = width;
            Height = height;


            // create bitmaps and fill with transparent color
            Intensity = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(Intensity))
            {
                g.Clear(Color.Transparent);
            }
            CloudTops = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(CloudTops))
            {
                g.Clear(Color.Transparent);
            }
            Lightning = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(Lightning))
            {
                g.Clear(Color.Transparent);
            }
        }

        private void ImportValues(int[,] values, Bitmap dst, ValueColorTable legend, double left, double top, double right, double bottom)
        {
            // return on no array
            if (values == null)
                return;

            // get source array dimensions
            int srcwidth = values.GetLength(0);
            int srcheight = values.GetLength(1);
            GPoint srctl = ParentMap.FromLatLngToLocal(new PointLatLng(top, left));
            GPoint srcbr = ParentMap.FromLatLngToLocal(new PointLatLng(bottom, right));
            double srcscalex = (double)(srcbr.X - srctl.X) / (double)srcwidth;
            double srcscaley = (double)(srcbr.Y - srctl.Y) / (double)srcheight;

            // get destination array dimensions
            double dstwidth = this.Width;
            double dstheight = this.Height;
            GPoint dsttl = ParentMap.FromLatLngToLocal(new PointLatLng(this.Top, this.Left));
            GPoint dstbr = ParentMap.FromLatLngToLocal(new PointLatLng(this.Bottom, this.Right));
            double dstscalex = (double)(dstbr.X - dsttl.X) / (double)dstwidth;
            double dstscaley = (double)(dstbr.Y - dsttl.Y) / (double)dstheight;

            // start mapping
            for (int x = 0; x < dstwidth; x++)
            {
                for (int y = 0; y < dstheight; y++)
                {
                    try
                    {
                        int srcx = (int)(((double)x * (double)dstscalex + (double)dsttl.X - (double)srctl.X) / srcscalex);
                        int srcy = (int)(((double)y * (double)dstscaley + (double)dsttl.Y - (double)srctl.Y) / srcscaley);
                        if ((srcx >= 0) && (srcx < srcwidth) && (srcy >= 0) && (srcy < srcheight))
                        {
                            if (values[srcx, srcy] == -1)
                                dst.SetPixel(x, y, ColorSemitransparent);
                            if (values[srcx, srcy] == 0)
                                dst.SetPixel(x, y, Color.Transparent);
                            else if (values[srcx, srcy] > 0)
                            {
                                Color c = legend.GetColorFromValue(values[srcx, srcy]);
                                dst.SetPixel(x, y, c);
//                                Console.WriteLine(x.ToString() + "," + y.ToString() + ":" + values[srcx, srcy].ToString() + "-->" + c.ToString());

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error while setting color in map: " + ex.Message);
                    }
                }
            }
        }

        public void ImportIntensity(int[,] intensity, double left, double top, double right, double bottom)
        {
            ImportValues(intensity, Intensity, IntensityLegend, left, top, right, bottom);
        }

        public void ImportCloudTops(int[,] cloudtops, double left, double top, double right, double bottom)
        {
            ImportValues(cloudtops, CloudTops, CloudTopsLegend, left, top, right, bottom);
        }

        public void ImportLightning(int[,] lightning, double left, double top, double right, double bottom)
        {
            ImportValues(lightning, Lightning, LightningLegend, left, top, right, bottom);
        }

        public int GetValue(Bitmap src, ValueColorTable legend, int x, int y)
        {
            // check bounds
            if ((x < 0) || (x > this.Width) || (y < 0) || (y > this.Height))
                return -1;

            Color c = src.GetPixel(x, y);

            return legend.GetValueFromColor(c, NEARESTCOLORSTRATEGY.RGB);
        }

        public int GetIntensityValue(int x, int y)
        {
            return GetValue(Intensity, IntensityLegend, x, y);
        }

        public int GetCloudTopValue(int x, int y)
        {
            return GetValue(CloudTops, CloudTopsLegend, x, y);
        }

        public int GetLightningValue(int x, int y)
        {
            return GetValue(Lightning, LightningLegend, x, y);
        }

        public Color GetColor(Bitmap src, int x, int y)
        {
            // check bounds
            if ((x < 0) || (x > this.Width) || (y < 0) || (y > this.Height))
                return Color.Transparent;

            Color c = src.GetPixel(x, y);

            return c;
        }

        public Color GetIntensityColor(int x, int y)
        {
            return GetColor(Intensity, x, y);
        }

        public Color GetCloudTopColor(int x, int y)
        {
            return GetColor(CloudTops, x, y);
        }

        public Color GetLightningColor(int x, int y)
        {
            return GetColor(Lightning, x, y);
        }
    }
}
