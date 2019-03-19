//
// Eci.cs
//
// Copyright (c) 2003-2012 Michael F. Henry
// Version 09/2012
//
using System;

namespace Zeptomoby.OrbitTools
{
   /// <summary>
   /// Encapsulates an Earth Centered Inertial coordinate position/velocity.
   /// </summary>
   public class Eci
   {
      #region Properties

      public Vector Position { get; protected set; }
      public Vector Velocity { get; protected set; }

      #endregion

      #region Construction

      /// <summary>
      /// Creates a new instance of the class zero position and zero velocity.
      /// </summary>
      public Eci()
         : this(new Vector(), new Vector())
      {
      }

      /// <summary>
      /// Creates a new instance of the class from XYZ coordinates.
      /// </summary>
      /// <param name="pos">The XYZ coordinates.</param>
      public Eci(Vector pos)
         : this(pos, new Vector())
      {
      }

      /// <summary>
      /// Creates a new instance of the class with the given position and
      /// velocity components.
      /// </summary>
      /// <param name="pos">The position vector.</param>
      /// <param name="vel">The velocity vector.</param>
      public Eci(Vector pos, Vector vel)
      {
         Position = pos;
         Velocity = vel;
      }

      /// <summary>
      /// Creates a new instance of the class from ECI coordinates.
      /// </summary>
      /// <param name="eci">The ECI coordinates.</param>
      public Eci(Eci eci)
      {
         Position = new Vector(eci.Position);
         Velocity = new Vector(eci.Velocity);
      }

      /// <summary>
      /// Creates a instance of the class from geodetic coordinates.
      /// </summary>
      /// <param name="geo">The geocentric coordinates.</param>
      /// <param name="date">The Julian date.</param>
      /// <remarks>
      /// Assumes the Earth is an oblate spheroid.
      /// Reference: The 1992 Astronomical Almanac, page K11
      /// Reference: www.celestrak.com (Dr. T.S. Kelso)
      /// </remarks>
      public Eci(Geo geo, Julian date)
      {
         double lat = geo.LatitudeRad;
         double lon = geo.LongitudeRad;
         double alt = geo.Altitude;

         // Calculate Local Mean Sidereal Time (theta)
         double theta = date.ToLmst(lon);
         double c = 1.0 / Math.Sqrt(1.0 + Globals.F * (Globals.F - 2.0) *
                          Globals.Sqr(Math.Sin(lat)));
         double s = Globals.Sqr(1.0 - Globals.F) * c;
         double achcp = (Globals.Xkmper * c + alt) * Math.Cos(lat);

         Position = new Vector();

         Position.X = achcp * Math.Cos(theta);             // km
         Position.Y = achcp * Math.Sin(theta);             // km
         Position.Z = (Globals.Xkmper * s + alt) * Math.Sin(lat);   // km
         Position.W = Math.Sqrt(Globals.Sqr(Position.X) +
                                Globals.Sqr(Position.Y) +
                                Globals.Sqr(Position.Z));  // range, km

         Velocity = new Vector();
         double mfactor = Globals.TwoPi * (Globals.OmegaE / Globals.SecPerDay);

         Velocity.X = -mfactor * Position.Y;               // km / sec
         Velocity.Y =  mfactor * Position.X;               // km / sec
         Velocity.Z = 0.0;                                 // km / sec
         Velocity.W = Math.Sqrt(Globals.Sqr(Velocity.X) +  // range rate km/sec^2
                                Globals.Sqr(Velocity.Y));
      }
      #endregion

      /// <summary>
      /// Scale the position vector by a factor.
      /// </summary>
      public void ScalePosVector(double factor)
      {
         Position.Mul(factor);
      }

      /// <summary>
      /// Scale the velocity vector by a factor.
      /// </summary>
      public void ScaleVelVector(double factor)
      {
         Velocity.Mul(factor);
      }

      /// <summary>
      /// Returns a string representation of the coordinate and 
      /// velocity XYZ values.
      /// </summary>
      /// <returns>The formatted string.</returns>
      public override string ToString()
      {
         return string.Format("km:({0:F0}, {1:F0}, {2:F0}) km/s:({3:F1}, {4:F1}, {5:F1})",
                              Position.X, Position.Y, Position.Z,
                              Velocity.X, Velocity.Y, Velocity.Z);
      }
   }

   /// <summary>
   /// Encapsulates an Earth Centered Inertial coordinate and 
   /// associated time.
   /// </summary>
   public class EciTime : Eci
   {
      #region Properties

      /// <summary>
      /// The time associated with the ECI coordinates.
      /// </summary>
      public Julian Date { get; protected set; }

      #endregion

      #region Construction

      /// <summary>
      /// Creates an instance of the class with the given position, velocity, and time.
      /// </summary>
      /// <param name="pos">The position vector.</param>
      /// <param name="vel">The velocity vector.</param>
      /// <param name="date">The time associated with the position.</param>
      public EciTime(Vector pos, Vector vel, Julian date)
         : base(pos, vel)
      {
         Date = date;
      }

      /// <summary>
      /// Creates a new instance of the class from ECI-time coordinates.
      /// </summary>
      /// <param name="eci">The ECI coordinates.</param>
      /// <param name="date">The time associated with the ECI coordinates.</param>
      public EciTime(Eci eci, Julian date)
         : this(eci.Position, eci.Velocity, date)
      {
      }

      /// <summary>
      /// Creates a new instance of the class from geodetic coordinates.
      /// </summary>
      /// <param name="geo">The geodetic coordinates.</param>
      /// <param name="date">The time associated with the ECI coordinates.</param>
      public EciTime(Geo geo, Julian date)
         : base(geo, date)
      {
         Date = date;
      }

      /// <summary>
      /// Creates a new instance of the class from geodetic-time coordinates.
      /// </summary>
      /// <param name="geo">The geodetic-time coordinates.</param>
      public EciTime(GeoTime geo)
         :this(geo, geo.Date)
      {
      }

      #endregion
   }
}
