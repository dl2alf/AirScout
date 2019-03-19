//
// Site.cs
//
// Copyright (c) 2003-2012 Michael F. Henry
// Version 06/2012
//
using System;
using System.Globalization;

namespace Zeptomoby.OrbitTools
{
   /// <summary>
   /// The Site class encapsulates a location on earth.
   /// </summary>
   public sealed class Site
   {
      #region Properties

      /// <summary>
      /// Latitude, in radians. A negative value indicates latitude south.
      /// </summary>
      public double LatitudeRad { get { return Geo.LatitudeRad; } }

      /// <summary>
      /// Longitude, in radians. A negative value indicates longitude west.
      /// </summary>
      public double LongitudeRad { get { return Geo.LongitudeRad; } }

      /// <summary>
      /// Latitude, in degrees. A negative value indicates latitude south.
      /// </summary>
      public double LatitudeDeg { get { return Geo.LatitudeDeg; } }

      /// <summary>
      /// Longitude, in degrees. A negative value indicates longitude west.
      /// </summary>
      public double LongitudeDeg { get { return Geo.LongitudeDeg; } }

      /// <summary>
      /// The altitude of the site, in kilometers
      /// </summary>
      public double Altitude { get { return Geo.Altitude; } }
    
      /// <summary>
      /// The contained geodetic coordinates.
      /// </summary>
      public Geo Geo { get; private set; }

      #endregion

      #region Construction

      /// <summary>
      /// Standard constructor.
      /// </summary>
      /// <param name="degLat">Latitude in degrees (negative south).</param>
      /// <param name="degLon">Longitude in degrees (negative west).</param>
      /// <param name="kmAlt">Altitude in kilometers.</param>
      /// <param name="model">The earth ellipsoid model.</param>
      public Site(double degLat, double degLon, double kmAlt)
      {
         Geo = new Geo(Globals.ToRadians(degLat),
                       Globals.ToRadians(degLon),
                       kmAlt);
      }

      /// <summary>
      /// Create a Site object from Geo object.
      /// </summary>
      /// <param name="geo">The Geo object.</param>
      public Site(Geo geo)
      {
         Geo = new Geo(geo);
      }

      #endregion
 
      /// <summary>
      /// Returns the ECI coordinates of the site at the given time.
      /// </summary>
      /// <param name="date">Time of position calculation.</param>
      /// <returns>The site's ECI coordinates.</returns>
      public EciTime GetPosition(Julian date)
      {
         return new EciTime(Geo, date);
      }

      /// <summary>
      /// Returns the topo-centric (azimuth, elevation, etc.) coordinates for
      /// a target object described by the given ECI coordinates.
      /// </summary>
      /// <param name="eci">The ECI coordinates of the target object.</param>
      /// <returns>The look angle to the target object.</returns>
      public TopoTime GetLookAngle(EciTime eci)
      {
         // Calculate the ECI coordinates for this Site object at the time
         // of interest.
         Julian  date     = eci.Date;
         EciTime eciSite  = GetPosition(date);
         Vector vecRgRate = new Vector(eci.Velocity.X - eciSite.Velocity.X,
                                       eci.Velocity.Y - eciSite.Velocity.Y,
                                       eci.Velocity.Z - eciSite.Velocity.Z);

         double x = eci.Position.X - eciSite.Position.X;
         double y = eci.Position.Y - eciSite.Position.Y;
         double z = eci.Position.Z - eciSite.Position.Z;
         double w = Math.Sqrt(Globals.Sqr(x) + Globals.Sqr(y) + Globals.Sqr(z));

         Vector vecRange = new Vector(x, y, z, w);

         // The site's Local Mean Sidereal Time at the time of interest.
         double theta = date.ToLmst(LongitudeRad);

         double sin_lat   = Math.Sin(LatitudeRad);
         double cos_lat   = Math.Cos(LatitudeRad);
         double sin_theta = Math.Sin(theta);
         double cos_theta = Math.Cos(theta);

         double top_s = sin_lat * cos_theta * vecRange.X + 
                        sin_lat * sin_theta * vecRange.Y - 
                        cos_lat * vecRange.Z;
         double top_e = -sin_theta * vecRange.X + 
                         cos_theta * vecRange.Y;
         double top_z = cos_lat * cos_theta * vecRange.X + 
                        cos_lat * sin_theta * vecRange.Y + 
                        sin_lat * vecRange.Z;
         double az    = Math.Atan(-top_e / top_s);

         if (top_s > 0.0)
         {
            az += Globals.Pi;
         }

         if (az < 0.0)
         {
            az += 2.0 * Globals.Pi;
         }

         double el   = Math.Asin(top_z / vecRange.W);
         double rate = (vecRange.X * vecRgRate.X + 
                        vecRange.Y * vecRgRate.Y + 
                        vecRange.Z * vecRgRate.Z) / vecRange.W;

         TopoTime topo = new TopoTime(az,         // azimuth, radians
                                      el,         // elevation, radians
                                      vecRange.W, // range, km
                                      rate,       // rate, km / sec
                                      eci.Date);

#if WANT_ATMOSPHERIC_CORRECTION
      // Elevation correction for atmospheric refraction.
      // Reference:  Astronomical Algorithms by Jean Meeus, pp. 101-104
      // Note:  Correction is meaningless when apparent elevation is below horizon
      topo.m_El += Globals.ToRadians((1.02 / 
                                    Math.Tan(Globals.ToRadians(Globals.ToDegrees(el) + 10.3 / 
                                    (Globals.ToDegrees(el) + 5.11)))) / 60.0);
      if (topo.m_El < 0.0)
      {
         topo.m_El = el;    // Reset to true elevation
      }

      if (topo.m_El > (Globals.PI / 2))
      {
         topo.m_El = (Globals.PI / 2);
      }
#endif
         return topo;
      }

      /// <summary>
      /// Converts to a string representation of the form "120.00N 090.00W 500m".
      /// </summary>
      /// <returns>The formatted string.</returns>
      public override string ToString()
      {
         return Geo.ToString();
      }
   }
}
