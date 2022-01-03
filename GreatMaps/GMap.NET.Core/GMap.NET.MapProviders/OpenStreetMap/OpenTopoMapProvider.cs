
namespace GMap.NET.MapProviders
{
   using System;

   /// <summary>
   /// OpenTopoMap provider - http://www.opentopomap.org
   /// </summary>
   public class OpenTopoMapProvider : OpenStreetMapProviderBase
   {
      public static readonly OpenTopoMapProvider Instance;

      OpenTopoMapProvider()
      {
         RefererUrl = "";
         Copyright = string.Format("© OpenTopoMap - Map data ©{0} OpenTopoMap. Licence CC-BY-SA. See www.opentopomap.org", DateTime.Today.Year);

        }

        static OpenTopoMapProvider()
      {
         Instance = new OpenTopoMapProvider();
      }

      #region GMapProvider Members

      readonly Guid id = new Guid("B8A6A7B4-3034-495D-BC83-96F6034748B1");
      public override Guid Id
      {
         get
         {
            return id;
         }
      }

      readonly string name = "OpenTopoMap";
      public override string Name
      {
         get
         {
            return name;
         }
      }

      GMapProvider[] overlays;
      public override GMapProvider[] Overlays
      {
         get
         {
            if(overlays == null)
            {
               overlays = new GMapProvider[] { this };
            }
            return overlays;
         }
      }

      public override PureImage GetTileImage(GPoint pos, int zoom)
      {
         string url = MakeTileImageUrl(pos, zoom, string.Empty);

         return GetTileImageUsingHttp(url);
      }

      #endregion

      string MakeTileImageUrl(GPoint pos, int zoom, string language)
      {
         char letter = ServerLetters[GMapProvider.GetServerNum(pos, 3)];
         return string.Format(UrlFormat, letter, zoom, pos.X, pos.Y);
      }

      static readonly string UrlFormat = "https://{0}.tile.opentopomap.org/{1}/{2}/{3}.png";
    }
}
