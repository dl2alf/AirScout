
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using GMap.NET.WindowsForms;
using System.Linq;

namespace RainScout
{
   public class GMapImage : GMapMarker
   {
      private Image image;
      public Image Image
      {
         get
         {
            return image;
         }
         set
         {
            image = value;
            if(image != null)
            {
               this.Size = new Size(image.Width, image.Height);
            }
         }
      }

      public GMapImage(GMap.NET.PointLatLng p)
         : base(p)
      {
         DisableRegionCheck = true;
         IsHitTestVisible = false;
      }

      public override void OnRender(Graphics g)
      {
         if(image == null)
            return;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rect = new Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height);
            g.DrawImage(image, rect);
      }

    }
}