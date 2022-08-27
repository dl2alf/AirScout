using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace AirScout
{
    public class GMapLocatorPolygon : GMapPolygon
    {
        public GMapLocatorPolygon(List<PointLatLng> points, string name) : base(points, name)
        {
        }

        protected GMapLocatorPolygon(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private int PolygonWidth()
        {
            if (this.LocalPoints == null)
                return 0;

            long min = int.MaxValue;
            long max = int.MinValue;
            foreach (GPoint p in this.LocalPoints)
            {
                if (p.X < min)
                    min = p.X;
                if (p.X > max)
                    max = p.X;
            }
            return (int)(max - min);
        }

        private int PolygonHeight()
        {
            if (this.LocalPoints == null)
                return 0;

            long min = int.MaxValue;
            long max = int.MinValue;
            foreach (GPoint p in this.LocalPoints)
            {
                if (p.Y < min)
                    min = p.Y;
                if (p.Y > max)
                    max = p.Y;
            }
            return (int)(max - min);
        }

        private Rectangle PolygonRect()
        {
            if (this.LocalPoints == null)
                return new Rectangle(0,0,0,0);

            long minX = int.MaxValue;
            long maxX = int.MinValue;
            foreach (GPoint p in this.LocalPoints)
            {
                if (p.X < minX)
                    minX = p.X;
                if (p.X > maxX)
                    maxX = p.X;
            }
            long minY = int.MaxValue;
            long maxY = int.MinValue;
            foreach (GPoint p in this.LocalPoints)
            {
                if (p.Y < minY)
                    minY = p.Y;
                if (p.Y > maxY)
                    maxY = p.Y;
            }

            return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }
        public override void OnRender(Graphics g)
        {
            base.OnRender(g);

            // render Tag on top of polygon, if it is a regular rectangle and contains a string as tag
            if (this.Points == null)
                return;
            if (this.Points.Count == 0)
                return;
            if (this.Tag == null)
                return;
            if (this.Tag.GetType() != typeof(string))
                return;

            // keep old graphics settings
            SmoothingMode oldsmoothingmode = g.SmoothingMode;
            TextRenderingHint oldtextrenderinghint = g.TextRenderingHint;

            // get polygon extension in pixels
            Rectangle polyrect = PolygonRect();

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            Font font = new Font("Arial", polyrect.Width / 10);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            g.DrawString((string)this.Tag, font, new SolidBrush(this.Stroke.Color), polyrect, format);
            g.Flush();
            font.Dispose();

            // restore old graphics settings
            g.SmoothingMode = oldsmoothingmode;
            g.TextRenderingHint = oldtextrenderinghint;
        }
    }

}
