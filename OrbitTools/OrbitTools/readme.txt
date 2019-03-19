
C# NORAD SGP4/SDP4 Implementation
Developed by Michael F. Henry

Copyright © 2003-2011 Michael F. Henry. All rights reserved.
Permission to use for non-commercial purposes only.
All other uses contact author at mfh@zeptomoby.com

The files in this directory are compiled to make the two OrbitTools assemblies:
	Zeptomoby.OrbitTools.Core.dll
	Zeptomoby.OrbitTools.Orbit.dll

The "Core" assembly contains several utility classes:

   Tle  – This class encapsulates a single set of NORAD two-line elements.
   Site – Describes a location on the earth. Given the ECI coordinates of a 
          satellite, this class can generate Azimuth/Elevation look angles to 
          the satellite.
   Eci  – This class encapsulates Earth-Centered Inertial coordinates and 
          velocity for a given moment in time.
   Julian - Encapsulates a julian date/time system.
      
The "Orbit" assembly contains the main SGP4/SDP4 implementation:

   Orbit – Given a Tle object, this class provides information about the orbit
           of the described satellite, including inclination, perigee, 
           eccentricity, etc. Most importantly, it provides ECI coordinates and 
           velocity for the satellite.
   NoradBase, NoradSGP4, NoradSDP4 – These classes implement the NORAD 
           SGP4/SDP4 algorithms. They are used by class Orbit to calculate the
           ECI coordinates and velocity of its associated satellite.

All classes are contained within the Zeptomoby.OrbitTools namespace.

Michael F. Henry
September, 2011