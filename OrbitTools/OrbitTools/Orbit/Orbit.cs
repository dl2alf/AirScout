//
// Orbit.cs
//
// Copyright (c) 2005-2012 Michael F. Henry
// Version 06/2012
//
using System;

namespace Zeptomoby.OrbitTools
{
   /// <summary>
   /// This class accepts a single satellite's NORAD two-line element
   /// set and provides information regarding the satellite's orbit 
   /// such as period, axis length, ECI coordinates, velocity, etc.
   /// </summary>
   public class Orbit
   {
      // Caching variables
      private TimeSpan m_Period = new TimeSpan(0, 0, 0, -1);

      // TLE caching variables
      private double m_Inclination;
      private double m_Eccentricity;
      private double m_RAAN;
      private double m_ArgPerigee;
      private double m_BStar;
      private double m_Drag;
      private double m_MeanAnomaly;
      private double m_TleMeanMotion;

      // Caching variables recovered from the input TLE elements
      private double m_aeAxisSemiMajorRec;  // semimajor axis, in AE units
      private double m_aeAxisSemiMinorRec;  // semiminor axis, in AE units
      private double m_rmMeanMotionRec;     // radians per minute
      private double m_kmPerigeeRec;        // perigee, in km
      private double m_kmApogeeRec;         // apogee, in km

      #region Properties

      private Tle       Tle        { get; set; }
      private NoradBase NoradModel { get; set; }
      public Julian     Epoch      { get; private set; }
      public  DateTime  EpochTime  { get { return Epoch.ToTime(); }}

      // "Recovered" from the input elements
      public double SemiMajor    { get { return m_aeAxisSemiMajorRec; }}
      public double SemiMinor    { get { return m_aeAxisSemiMinorRec; }}
      public double MeanMotion   { get { return m_rmMeanMotionRec;    }}
      public double Major        { get { return 2.0 * SemiMajor;      }}
      public double Minor        { get { return 2.0 * SemiMinor;      }}
      public double Perigee      { get { return m_kmPerigeeRec;       }}
      public double Apogee       { get { return m_kmApogeeRec;        }}

      public double Inclination    { get { return m_Inclination;   }}
      public double Eccentricity   { get { return m_Eccentricity;  }}
      public double RAAN           { get { return m_RAAN;          }}
      public double ArgPerigee     { get { return m_ArgPerigee;    }}
      public double BStar          { get { return m_BStar;         }}
      public double Drag           { get { return m_Drag;          }}
      public double MeanAnomaly    { get { return m_MeanAnomaly;   }}
      private double TleMeanMotion { get { return m_TleMeanMotion; }}

      public string SatNoradId    { get { return Tle.NoradNumber; }}
      public string SatName       { get { return Tle.Name;        }}
      public string SatNameLong   { get { return SatName + " #" + SatNoradId; }}

      public TimeSpan Period 
      {
         get 
         { 
            if (m_Period.TotalSeconds < 0.0)
            {
               // Calculate the period using the recovered mean motion.
               if (MeanMotion == 0)
               {
                  m_Period = new TimeSpan(0, 0, 0);
               }
               else
               {
                  double secs  = (Globals.TwoPi / MeanMotion) * 60.0;
                  int    msecs = (int)((secs - (int)secs) * 1000);

                  m_Period = new TimeSpan(0, 0, 0, (int)secs, msecs);
               }
            }

            return m_Period;
         }
      }

      #endregion

      #region Construction

      /// <summary>
      /// Standard constructor.
      /// </summary>
      /// <param name="tle">Two-line element orbital parameters.</param>
      public Orbit(Tle tle)
      {
         Tle     = tle;
         Epoch = Tle.EpochJulian;

         m_Inclination   = GetRad(Tle.Field.Inclination);
         m_Eccentricity  = Tle.GetField(Tle.Field.Eccentricity);
         m_RAAN          = GetRad(Tle.Field.Raan);              
         m_ArgPerigee    = GetRad(Tle.Field.ArgPerigee);        
         m_BStar         = Tle.GetField(Tle.Field.BStarDrag);   
         m_Drag          = Tle.GetField(Tle.Field.MeanMotionDt);
         m_MeanAnomaly   = GetRad(Tle.Field.MeanAnomaly);
         m_TleMeanMotion = Tle.GetField(Tle.Field.MeanMotion);  

         // Recover the original mean motion and semimajor axis from the
         // input elements.
         double mm     = TleMeanMotion;
         double rpmin  = mm * Globals.TwoPi / Globals.MinPerDay;   // rads per minute

         double a1     = Math.Pow(Globals.Xke / rpmin, 2.0 / 3.0);
         double e      = Eccentricity;
         double i      = Inclination;
         double temp   = (1.5 * Globals.Ck2 * (3.0 * Globals.Sqr(Math.Cos(i)) - 1.0) / 
                         Math.Pow(1.0 - e * e, 1.5));   
         double delta1 = temp / (a1 * a1);
         double a0     = a1 * 
                        (1.0 - delta1 * 
                        ((1.0 / 3.0) + delta1 * 
                        (1.0 + 134.0 / 81.0 * delta1)));

         double delta0 = temp / (a0 * a0);

         m_rmMeanMotionRec    = rpmin / (1.0 + delta0);
         m_aeAxisSemiMajorRec = a0 / (1.0 - delta0);
         m_aeAxisSemiMinorRec = m_aeAxisSemiMajorRec * Math.Sqrt(1.0 - (e * e));
         m_kmPerigeeRec       = Globals.Xkmper * (m_aeAxisSemiMajorRec * (1.0 - e) - Globals.Ae);
         m_kmApogeeRec        = Globals.Xkmper * (m_aeAxisSemiMajorRec * (1.0 + e) - Globals.Ae);

         if (Period.TotalMinutes >= 225.0)
         {
            // SDP4 - period >= 225 minutes.
            NoradModel = new NoradSDP4(this);
         }
         else
         {
            // SGP4 - period < 225 minutes
            NoradModel = new NoradSGP4(this);
         }
      }

      #endregion

      #region Get Position

      /// <summary>
      /// Calculate satellite ECI position/velocity for a given time.
      /// </summary>
      /// <param name="tsince">Target time, in minutes past the TLE epoch.</param>
      /// <returns>Kilometer-based position/velocity ECI coordinates.</returns>
      public EciTime GetPosition(double minutesPastEpoch)
      {
         EciTime eci = NoradModel.GetPosition(minutesPastEpoch);

         // Convert ECI vector units from AU to kilometers
         double radiusAe = Globals.Xkmper / Globals.Ae;

         eci.ScalePosVector(radiusAe);                               // km
         eci.ScaleVelVector(radiusAe * (Globals.MinPerDay / 86400)); // km/sec

         return eci;
      }

      /// <summary>
      /// Calculate ECI position/velocity for a given time.
      /// </summary>
      /// <param name="utc">Target time (UTC).</param>
      /// <returns>Kilometer-based position/velocity ECI coordinates.</returns>
      public EciTime GetPosition(DateTime utc)
      {
         return GetPosition(TPlusEpoch(utc).TotalMinutes);
      }

      #endregion

      // ///////////////////////////////////////////////////////////////////////////
      // Returns elapsed time from epoch to given time.
      // Note: "Predicted" TLEs can have epochs in the future.
      public TimeSpan TPlusEpoch(DateTime utc) 
      {
         return (utc - EpochTime);
      }

      // ///////////////////////////////////////////////////////////////////////////
      // Returns elapsed time from epoch to current time.
      // Note: "Predicted" TLEs can have epochs in the future.
      public TimeSpan TPlusEpoch()
      {
         return TPlusEpoch(DateTime.UtcNow);
      }

      #region Utility

      // ///////////////////////////////////////////////////////////////////
      protected double GetRad(Tle.Field fld) 
      { 
         return Tle.GetField(fld, Tle.Unit.Radians); 
      }

      // ///////////////////////////////////////////////////////////////////
      protected double GetDeg(Tle.Field fld) 
      { 
         return Tle.GetField(fld, Tle.Unit.Degrees); 
      }

      #endregion
   }
}
