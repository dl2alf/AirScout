//
// Coord.cs
//
// Copyright (c) 2003-2012 Michael F. Henry
// Version 10/2012
//
using System;
using System.Globalization;

namespace Zeptomoby.OrbitTools
{
   /// <summary>
   /// Class to encapsulate geocentric coordinates.
   /// </summary>
   public class Geo  
   {
      #region Properties

      /// <summary>
      /// Latitude, in radians. A negative value indicates latitude south.
      /// </summary>
      public double LatitudeRad { get; set; }

      /// <summary>
      /// Longitude, in radians. A negative value indicates longitude west.
      /// </summary>
      public double LongitudeRad { get; set; }

      /// <summary>
      /// Latitude, in degrees. A negative value indicates latitude south.
      /// </summary>
      public double LatitudeDeg { get { return Globals.ToDegrees(LatitudeRad); } }

      /// <summary>
      /// Longitude, in degrees. A negative value indicates longitude west.
      /// </summary>
      public double LongitudeDeg { get { return Globals.ToDegrees(LongitudeRad); } }

      /// <summary>
      /// Altitude, in kilometers, above the ellipsoid model.
      /// </summary>
      public double Altitude { get; set; }

      #endregion

      #region Construction

      /// <summary>
      /// Creates a Geo object from a source Geo object.
      /// </summary>
      /// <param name="geo">The source Geo object.</param>
      public Geo(Geo geo)
      {
         LatitudeRad  = geo.LatitudeRad;
         LongitudeRad = geo.LongitudeRad;
         Altitude     = geo.Altitude;
      }

      /// <summary>
      /// Creates a new instance of the class with the given components.
      /// </summary>
      /// <param name="radLat">Latitude, in radians. Negative values indicate
      /// latitude south.</param>
      /// <param name="radLon">Longitude, in radians. Negative value indicate longitude
      /// west.</param>
      /// <param name="kmAlt">Altitude above the ellipsoid model, in kilometers.</param>
      public Geo(double radLat, double radLon, double kmAlt)
      {
         LatitudeRad  = radLat;
         LongitudeRad = radLon;
         Altitude     = kmAlt;
      }

      /// <summary>
      /// Creates a new instance of the class given ECI coordinates.
      /// </summary>
      /// <param name="eci">The ECI coordinates.</param>
      /// <param name="date">The Julian date.</param>
      public Geo(Eci eci, Julian date)
         :this(eci.Position, 
               (Globals.AcTan(eci.Position.Y, eci.Position.X) - date.ToGmst()) % Globals.TwoPi)
      {
      }

      /// <summary>
      /// Creates a new instance of the class given XYZ coordinates.
      /// </summary>
      private Geo(Vector pos, double theta)
      {
         theta = theta % Globals.TwoPi;

         if (theta < 0.0)
         {
            // "wrap" negative modulo
            theta += Globals.TwoPi;
         }

         double r = Math.Sqrt(Globals.Sqr(pos.X) + Globals.Sqr(pos.Y));
         double e2 = Globals.F * (2.0 - Globals.F);
         double lat = Globals.AcTan(pos.Z, r);

         const double DELTA = 1.0e-07;
         double phi;
         double c;

         do
         {
            phi = lat;
            c = 1.0 / Math.Sqrt(1.0 - e2 * Globals.Sqr(Math.Sin(phi)));
            lat = Globals.AcTan(pos.Z + Globals.Xkmper * c * e2 * Math.Sin(phi), r);
         }
         while (Math.Abs(lat - phi) > DELTA);

         LatitudeRad  = lat;
         LongitudeRad = theta;
         Altitude = (r / Math.Cos(lat)) - Globals.Xkmper * c;
      }

      #endregion

      #region ToString

      /// <summary>
      /// Converts to a string representation of the form "38.0N 045.0W 500m".
      /// </summary>
      /// <returns>The formatted string.</returns>
      public override string ToString()
      {
         bool latNorth = (LatitudeRad  >= 0.0);
         bool lonEast  = (LongitudeRad >= 0.0);

         // latitude in degrees
         string str = string.Format(CultureInfo.CurrentCulture, "{0:00.0}{1} ",
                                    Math.Abs(LatitudeDeg),
                                    (latNorth ? 'N' : 'S'));
         // longitude in degrees
         str += string.Format(CultureInfo.CurrentCulture, "{0:000.0}{1} ",
                              Math.Abs(LongitudeDeg),
                              (lonEast ? 'E' : 'W'));
         // elevation in meters
         str += string.Format(CultureInfo.CurrentCulture, "{0:F0}m", Altitude * 1000.0);

         return str;
      }

      #endregion
   }

   /// <summary>
   /// Class to encapsulate a geocentric coordinate and associated time.
   /// </summary>
   public sealed class GeoTime : Geo
   {
      #region Properties

      /// <summary>
      /// The time associated with the coordinates.
      /// </summary>
      public Julian Date { get; private set; }

      #endregion

      #region Construction

      /// <summary>
      /// Standard constructor.
      /// </summary>
      /// <param name="radLat">Latitude, in radians. Negative values indicate
      /// latitude south.</param>
      /// <param name="radLon">Longitude, in radians. Negative value indicate longitude
      /// west.</param>
      /// <param name="kmAlt">Altitude above the ellipsoid model, in kilometers.</param>
      /// <param name="date">The time associated with the coordinates.</param>
      public GeoTime(double radLat, double radLon, double kmAlt, Julian date)
         :base(radLat, radLon, kmAlt)
      {
         Date = date;
      }

      /// <summary>
      /// Constructor accepting Geo and Julian objects.
      /// </summary>
      /// <param name="geo">The Geo object.</param>
      /// <param name="date">The Julian date.</param>
      public GeoTime(Geo geo, Julian date)
         :base(geo)
      {
         Date = date;
      }

      /// <summary>
      /// Creates a new instance of the class from ECI-time information.
      /// </summary>
      /// <param name="eci">The ECI-time coordinate pair.</param>
      /// <param name="ellipsoid">The earth ellipsoid model.</param>
      public GeoTime(EciTime eci)
         : base(eci, eci.Date)
      {
         Date = eci.Date;
      }

      /// <summary>
      /// Creates a new instance of the class from ECI coordinates.
      /// </summary>
      /// <param name="eci">The ECI coordinates.</param>
      /// <param name="date">The Julian date.</param>
      public GeoTime(Eci eci, Julian date)
         : base(eci, date)
      {
         Date = date;
      }

      #endregion
   }

   /// <summary>
   /// Class to encapsulate topo-centric coordinates.
   /// </summary>
   public class Topo  
   {
      #region Properties

      /// <summary>
      /// The azimuth, in radians.
      /// </summary>
      public double AzimuthRad { get; set; }

      /// <summary>
      /// The elevation, in radians.
      /// </summary>
      public double ElevationRad { get; set; }

      /// <summary>
      /// The azimuth, in degrees.
      /// </summary>
      public double AzimuthDeg { get { return Globals.ToDegrees(AzimuthRad); } }
      
      /// <summary>
      /// The elevation, in degrees.
      /// </summary>
      public double ElevationDeg { get { return Globals.ToDegrees(ElevationRad); } }

      /// <summary>
      /// The range, in kilometers.
      /// </summary>
      public double Range { get; set; }

      /// <summary>
      /// The range rate, in kilometers per second. 
      /// A negative value means "towards observer".
      /// </summary>
      public double RangeRate { get; set; }

      #endregion

      #region Construction

      /// <summary>
      /// Creates a new instance of the class from the given components.
      /// </summary>
      /// <param name="radAz">Azimuth, in radians.</param>
      /// <param name="radEl">Elevation, in radians.</param>
      /// <param name="range">Range, in kilometers.</param>
      /// <param name="rangeRate">Range rate, in kilometers per second. A negative
      /// range rate means "towards the observer".</param>
      public Topo(double radAz, double radEl, double range, double rangeRate)
      {
         AzimuthRad   = radAz;
         ElevationRad = radEl;
         Range     = range;
         RangeRate = rangeRate;
      }

      #endregion
   }

   /// <summary>
   /// Class to encapsulate topo-centric coordinates and a time.
   /// </summary>
   public sealed class TopoTime : Topo
   {
      #region Properties

      /// <summary>
      /// The time associated with the coordinates.
      /// </summary>
      public Julian Date { get; private set; }

      #endregion

      #region Construction

      /// <summary>
      /// Creates an instance of the class from topo and time information.
      /// </summary>
      /// <param name="topo"></param>
      /// <param name="date"></param>
      public TopoTime(Topo topo, Julian date)
         :base(topo.AzimuthRad, topo.ElevationRad, topo.Range, topo.RangeRate)
      {
         Date = date;
      }

      /// <summary>
      /// Creates a new instance of the class from the given components.
      /// </summary>
      /// <param name="radAz">Azimuth, in radians.</param>
      /// <param name="radEl">Elevation, in radians.</param>
      /// <param name="range">Range, in kilometers.</param>
      /// <param name="rangeRate">Range rate, in kilometers per second. A negative
      /// range rate means "towards the observer".</param>
      /// <param name="date">The time associated with the coordinates.</param>
      public TopoTime(double radAz, double radEl, double range, double rangeRate, Julian date)
         :base(radAz, radEl, range, rangeRate)
      {
         Date = date;
      }

      #endregion
   }
}