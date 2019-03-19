//
// This class provides a base class for the NORAD SGP4/SDP4 orbit models.
//
// Copyright (c) 2003-2011 Michael F. Henry
//
using System;

namespace Zeptomoby.OrbitTools
{
	/// <summary>
   /// This class provides a base class for the NORAD SGP4/SDP4 orbit models.
	/// </summary>
	abstract internal class NoradBase
   {
      #region Properties

      // Orbital parameter variables which need only be calculated one
      // time for a given orbit (ECI position time-independent).
      protected double m_satInc;  // inclination
      protected double m_satEcc;  // eccentricity

      protected double m_cosio;   protected double m_theta2;  protected double m_x3thm1;  protected double m_eosq;  
      protected double m_betao2;  protected double m_betao;   protected double m_aodp;    protected double m_xnodp;
      protected double m_s4;      protected double m_qoms24;  protected double m_perigee; protected double m_tsi;
      protected double m_eta;     protected double m_etasq;   protected double m_eeta;    protected double m_coef;
      protected double m_coef1;   protected double m_c1;      protected double m_c3;      protected double m_c4;
      protected double m_sinio;   protected double m_x1mth2;  protected double m_xmdot;   protected double m_omgdot;  
      protected double m_xnodot;  protected double m_xnodcf;  protected double m_t2cof;   protected double m_xlcof;   
      protected double m_aycof;   protected double m_x7thm1;

      protected Orbit Orbit { get; private set; }

      public abstract EciTime GetPosition(double tsince);

      #endregion

      #region Construction

      public NoradBase(Orbit orbit)
      {
         Orbit = orbit;
         Initialize();
      }

      #endregion

      // ///////////////////////////////////////////////////////////////////////////
      // Initialize()
      // Perform the initialization of member variables, specifically the variables
      // used by derived-class objects to calculate ECI coordinates.
      private void Initialize()
      {
         // Initialize any variables which are time-independent when
         // calculating the ECI coordinates of the satellite.
         m_satInc = Orbit.Inclination;
         m_satEcc = Orbit.Eccentricity;

         m_cosio  = Math.Cos(m_satInc);
         m_theta2 = m_cosio * m_cosio;
         m_x3thm1 = 3.0 * m_theta2 - 1.0;
         m_eosq   = m_satEcc * m_satEcc;
         m_betao2 = 1.0 - m_eosq;
         m_betao  = Math.Sqrt(m_betao2);

         // The "recovered" semimajor axis and mean motion.
         m_aodp  = Orbit.SemiMajor;
         m_xnodp = Orbit.MeanMotion;

         // For perigee below 156 km, the values of Globals.S and Globals.QOMS2T are altered.
         m_perigee = Globals.Xkmper * (m_aodp * (1.0 - m_satEcc) - Globals.Ae);

         m_s4      = Globals.S;
         m_qoms24  = Globals.Qoms2t;

         if (m_perigee < 156.0)
         {
            m_s4 = m_perigee - 78.0;

            if (m_perigee <= 98.0)
            {
               m_s4 = 20.0;
            }

            m_qoms24 = Math.Pow((120.0 - m_s4) * Globals.Ae / Globals.Xkmper, 4.0);
            m_s4 = m_s4 / Globals.Xkmper + Globals.Ae;
         }

         double pinvsq = 1.0 / (m_aodp * m_aodp * m_betao2 * m_betao2);

         m_tsi   = 1.0 / (m_aodp - m_s4);
         m_eta   = m_aodp * m_satEcc * m_tsi;
         m_etasq = m_eta * m_eta;
         m_eeta  = m_satEcc * m_eta;

         double psisq  = Math.Abs(1.0 - m_etasq);

         m_coef  = m_qoms24 * Math.Pow(m_tsi,4.0);
         m_coef1 = m_coef / Math.Pow(psisq,3.5);

         double c2 = m_coef1 * m_xnodp * 
                     (m_aodp * (1.0 + 1.5 * m_etasq + m_eeta * (4.0 + m_etasq)) +
                     0.75 * Globals.Ck2 * m_tsi / psisq * m_x3thm1 * 
                     (8.0 + 3.0 * m_etasq * (8.0 + m_etasq)));

         m_c1    = Orbit.BStar * c2;
         m_sinio = Math.Sin(m_satInc);

         double a3ovk2 = -Globals.Xj3 / Globals.Ck2 * Math.Pow(Globals.Ae,3.0);

         m_c3     = m_coef * m_tsi * a3ovk2 * m_xnodp * Globals.Ae * m_sinio / m_satEcc;
         m_x1mth2 = 1.0 - m_theta2;
         m_c4     = 2.0 * m_xnodp * m_coef1 * m_aodp * m_betao2 * 
                    (m_eta * (2.0 + 0.5 * m_etasq) +
                    m_satEcc * (0.5 + 2.0 * m_etasq) - 
                    2.0 * Globals.Ck2 * m_tsi / (m_aodp * psisq) *
                    (-3.0 * m_x3thm1 * (1.0 - 2.0 * m_eeta + m_etasq * (1.5 - 0.5 * m_eeta)) +
                    0.75 * m_x1mth2 * 
                    (2.0 * m_etasq - m_eeta * (1.0 + m_etasq)) * 
                    Math.Cos(2.0 * Orbit.ArgPerigee)));

         double theta4 = m_theta2 * m_theta2;
         double temp1  = 3.0 * Globals.Ck2 * pinvsq * m_xnodp;
         double temp2  = temp1 * Globals.Ck2 * pinvsq;
         double temp3  = 1.25 * Globals.Ck4 * pinvsq * pinvsq * m_xnodp;

         m_xmdot = m_xnodp + 0.5 * temp1 * m_betao * m_x3thm1 +
                   0.0625 * temp2 * m_betao * 
                   (13.0 - 78.0 * m_theta2 + 137.0 * theta4);

         double x1m5th = 1.0 - 5.0 * m_theta2;

         m_omgdot = -0.5 * temp1 * x1m5th + 0.0625 * temp2 * 
                    (7.0 - 114.0 * m_theta2 + 395.0 * theta4) +
                    temp3 * (3.0 - 36.0 * m_theta2 + 49.0 * theta4);

         double xhdot1 = -temp1 * m_cosio;

         m_xnodot = xhdot1 + (0.5 * temp2 * (4.0 - 19.0 * m_theta2) +
                    2.0 * temp3 * (3.0 - 7.0 * m_theta2)) * m_cosio;
         m_xnodcf = 3.5 * m_betao2 * xhdot1 * m_c1;
         m_t2cof  = 1.5 * m_c1;
         m_xlcof  = 0.125 * a3ovk2 * m_sinio * 
                    (3.0 + 5.0 * m_cosio) / (1.0 + m_cosio);
         m_aycof  = 0.25 * a3ovk2 * m_sinio;
         m_x7thm1 = 7.0 * m_theta2 - 1.0;
      }

      // /////////////////////////////////////////////////////////////////////
      protected EciTime FinalPosition(double  incl, double omega,  double     e,   
                                      double     a, double    xl,  double xnode, 
                                      double    xn, double tsince)
      {
         if ((e * e) > 1.0)
         {
            throw new PropagationException("Error in satellite data");
         }

         double beta = Math.Sqrt(1.0 - e * e);

         // Long period periodics 
         double axn  = e * Math.Cos(omega);
         double temp = 1.0 / (a * beta * beta);
         double xll  = temp * m_xlcof * axn;
         double aynl = temp * m_aycof;
         double xlt  = xl + xll;
         double ayn  = e * Math.Sin(omega) + aynl;

         // Solve Kepler's Equation 
         double capu   = Globals.Fmod2p(xlt - xnode);
         double temp2  = capu;
         double temp3  = 0.0;
         double temp4  = 0.0;
         double temp5  = 0.0;
         double temp6  = 0.0;
         double sinepw = 0.0;
         double cosepw = 0.0;
         bool   fDone  = false;

         for (int i = 1; (i <= 10) && !fDone; i++)
         {
            sinepw = Math.Sin(temp2);
            cosepw = Math.Cos(temp2);
            temp3 = axn * sinepw;
            temp4 = ayn * cosepw;
            temp5 = axn * cosepw;
            temp6 = ayn * sinepw;

            double epw = (capu - temp4 + temp3 - temp2) / 
                         (1.0 - temp5 - temp6) + temp2;

            if (Math.Abs(epw - temp2) <= 1.0e-06)
            {
               fDone = true;
            }
            else
            {
               temp2 = epw;
            }
         }

         // Short period preliminary quantities 
         double ecose = temp5 + temp6;
         double esine = temp3 - temp4;
         double elsq  = axn * axn + ayn * ayn;
         temp  = 1.0 - elsq;
         double pl = a * temp;
         double r  = a * (1.0 - ecose);
         double temp1 = 1.0 / r;
         double rdot  = Globals.Xke * Math.Sqrt(a) * esine * temp1;
         double rfdot = Globals.Xke * Math.Sqrt(pl) * temp1;
         temp2 = a * temp1;
         double betal = Math.Sqrt(temp);
         temp3 = 1.0 / (1.0 + betal);
         double cosu  = temp2 * (cosepw - axn + ayn * esine * temp3);
         double sinu  = temp2 * (sinepw - ayn - axn * esine * temp3);
         double u     = Globals.AcTan(sinu, cosu);
         double sin2u = 2.0 * sinu * cosu;
         double cos2u = 2.0 * cosu * cosu - 1.0;

         temp  = 1.0 / pl;
         temp1 = Globals.Ck2 * temp;
         temp2 = temp1 * temp;

         // Update for short periodics 
         double rk = r * (1.0 - 1.5 * temp2 * betal * m_x3thm1) + 
                     0.5 * temp1 * m_x1mth2 * cos2u;
         double uk = u - 0.25 * temp2 * m_x7thm1 * sin2u;
         double xnodek = xnode + 1.5 * temp2 * m_cosio * sin2u;
         double xinck  = incl + 1.5 * temp2 * m_cosio * m_sinio * cos2u;
         double rdotk  = rdot - xn * temp1 * m_x1mth2 * sin2u;
         double rfdotk = rfdot + xn * temp1 * (m_x1mth2 * cos2u + 1.5 * m_x3thm1);

         // Orientation vectors 
         double sinuk  = Math.Sin(uk);
         double cosuk  = Math.Cos(uk);
         double sinik  = Math.Sin(xinck);
         double cosik  = Math.Cos(xinck);
         double sinnok = Math.Sin(xnodek);
         double cosnok = Math.Cos(xnodek);
         double xmx = -sinnok * cosik;
         double xmy = cosnok * cosik;
         double ux  = xmx * sinuk + cosnok * cosuk;
         double uy  = xmy * sinuk + sinnok * cosuk;
         double uz  = sinik * sinuk;
         double vx  = xmx * cosuk - cosnok * sinuk;
         double vy  = xmy * cosuk - sinnok * sinuk;
         double vz  = sinik * cosuk;

         // Position
         double x = rk * ux;
         double y = rk * uy;
         double z = rk * uz;

         Vector vecPos = new Vector(x, y, z);
         DateTime gmt = Orbit.EpochTime.AddMinutes(tsince);

         // Validate on altitude
         double altKm = (vecPos.Magnitude() * (Globals.Xkmper / Globals.Ae));

         if (altKm < Globals.Xkmper)
         {
            throw new DecayException(gmt, Orbit.SatNameLong);
         }
   
         // Velocity
         double xdot = rdotk * ux + rfdotk * vx;
         double ydot = rdotk * uy + rfdotk * vy;
         double zdot = rdotk * uz + rfdotk * vz;

         Vector vecVel = new Vector(xdot, ydot, zdot);

         return new EciTime(vecPos, vecVel, new Julian(gmt));
      }
   }
}
