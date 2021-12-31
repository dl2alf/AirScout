using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RainScout.Core
{
    public enum NEARESTCOLORSTRATEGY
    {
        NONE = 0,
        STRICT = 1,
        HUE = 2,
        RGB = 3,
        HUESATBRIGHT =4

    }

    // translates Integer values to Colors and back
    // caches new calculated values for faster lookup
    public class ValueColorTable
    {
        // dictionaries for lookup
        private Dictionary<int, Color> Values = new Dictionary<int, Color>();
        private Dictionary<Color, int> Colors = new Dictionary<Color, int>();

        // min/max-values for faster lookup
        private int MinValues = 0;
        private int MaxValues = 0;
        private Color MinColors = Color.Transparent;
        private Color MaxColors = Color.Transparent;

        public void Add(int value, Color color)
        {
            try
            {
                Values[value] = color;
                MinValues = Values.Keys.Min();
                MaxValues = Values.Keys.Max();
                Colors[color] = value;
            }
            catch (Exception ex)
            {
                // do nothing if failed
                Console.WriteLine("Failed adding value/color pair: " + ex.Message);
            }
        }

        public Color GetColorFromValue(int value)
        {
            Color c = Color.Transparent;
            try
            {
                if (!Values.TryGetValue(value, out c))
                {
                    // value not found --> interpolate linear between to values

                    // check for out of bounds
                    if (value < MinValues)
                    {
                        c = Values[MinValues];
                        Add(value, c);
                        return c;
                    }

                    if (value > MaxValues)
                    {
                        c = Values[MaxValues];
                        Add(value, c);
                        return c;
                    }

                    // found neighboured entries
                    int minkey = int.MaxValue;
                    int maxkey = int.MinValue;
                    foreach (int key in this.Values.Keys)
                    {
                        if (key <= value)
                            minkey = key;
                        if (key >= value)
                        {
                            maxkey = key;
                            break;
                        }
                    }

                    if (minkey != maxkey)
                    {
                        Color mincolor = this.Values[minkey];
                        Color maxcolor = this.Values[maxkey];

                        double step = (value - minkey) / (maxkey - minkey);

                        int a = mincolor.A + (int)((mincolor.A - maxcolor.A) * step); if (a < 0) a = 0; if (a > 255) a = 255;
                        int r = mincolor.R + (int)((mincolor.R - maxcolor.R) * step); if (r < 0) r = 0; if (r > 255) r = 255;
                        int g = mincolor.G + (int)((mincolor.G - maxcolor.G) * step); if (g < 0) g = 0; if (g > 255) g = 255;
                        int b = mincolor.B + (int)((mincolor.B - maxcolor.B) * step); if (b < 0) b = 0; if (b > 255) b = 255;

                        // create color
                        c = Color.FromArgb(a, r, g, b);

                        // cache in dictionary
                        Add(value, c);
                    }
                    else
                    {
                        // should not happen --> use minkey
                        c = this.Values[minkey];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting color from value (" + value.ToString() + "): " + ex.Message);
            }
            return c;
        }

        // color brightness as perceived:
        private float GetBrightness(Color c)
        { 
            return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f;
        }

        //  weighed only by saturation and brightness(from my trackbars)
       float ColorNum(Color c)
        {
            return c.GetSaturation() * 0.5f +
                        GetBrightness(c) * 0.5f;
        }
        // distance between two hues:
        private float GetHueDistance(float hue1, float hue2)
        {
            float d = Math.Abs(hue1 - hue2); return d > 180 ? 360 - d : d;
        }

        // distance in RGB space
        private int ColorDiff(Color c1, Color c2)
        {
            int diff = 0;
            try
            {
                diff = (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                   + (c1.G - c2.G) * (c1.G - c2.G)
                                   + (c1.B - c2.B) * (c1.B - c2.B));
            }
            catch (Exception ex)
            {

            }

            return diff;
        }

        // closed match for hues only:
        int ClosestColor1(Color target)
        {
            var hue1 = target.GetHue();
            var diffs = Colors.Keys.Select(n => GetHueDistance(n.GetHue(), hue1));
            var diffMin = diffs.Min(n => n);
            int v = Colors[Colors.Keys.ElementAt(diffs.ToList().FindIndex(n => n == diffMin))];
            return v;
        }

        // closed match in RGB space
        int ClosestColor2(Color target)
        {
            var diffs = Colors.Keys.Select(n => ColorDiff(n, target));
            var diffMin = diffs.Min(n => n);
            int v = Colors[Colors.Keys.ElementAt(diffs.ToList().FindIndex(n => n == diffMin))];
            return v;
        }

        // weighed distance using hue, saturation and brightness
        int ClosestColor3(Color target)
        {
            float hue1 = target.GetHue();
            var num1 = ColorNum(target);
            var diffs = Colors.Keys.Select(n => Math.Abs(ColorNum(n) - num1) +
                                           GetHueDistance(n.GetHue(), hue1));
            var diffMin = diffs.Min(x => x);
            int v = Colors[Colors.Keys.ElementAt(diffs.ToList().FindIndex(n => n == diffMin))];
            return v;
        }

        public int GetValueFromColor(Color c, NEARESTCOLORSTRATEGY strategy = NEARESTCOLORSTRATEGY.RGB)
        {
            int v = 0;
            if (!Colors.TryGetValue(c, out v))
            {
                try
                {
                    // value not found --> find nearest value
                    switch (strategy)
                    {
                        case NEARESTCOLORSTRATEGY.HUE: v = ClosestColor1(c); break;
                        case NEARESTCOLORSTRATEGY.RGB: v = ClosestColor2(c); break;
                        case NEARESTCOLORSTRATEGY.HUESATBRIGHT: v = ClosestColor3(c); break;
                    }

                    /*
                    ColorDlg Dlg = new ColorDlg();
                    Dlg.lbl_Color.BackColor = c;
                    Dlg.lbl_Value.Text = v.ToString();
                    Dlg.ShowDialog();
                    */
                    try
                    {
                        Add(v, c);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while getting value from color (" + c.ToString() + "): " + ex.Message);
                }
            }

            return v;
        }
    }
}
