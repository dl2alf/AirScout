
C# NORAD SGP4/SDP4 Implementation
Developed by Michael F. Henry

Copyright © 2003-2011 Michael F. Henry. All rights reserved.
Permission to use for non-commercial purposes only. 
All other uses contact author at mfh@zeptomoby.com

The files in this package provide a C# implementation of the SGP4 and SDP4
algorithms described in the December, 1980 NORAD document "Space Track 
Report No. 3". These two orbital models, one for "near-earth" objects and
one for "deep space" objects, are widely used in satellite tracking
software and can produce very accurate results when used with current
NORAD two-line element data.

The files are contained in two directories: OrbitTools and OrbitToolsDemo.

The files in the OrbitTools directory are compiled to make the OrbitTools
assemblies, which contain the NORAD SGP4/SDP4 implementations and 
miscellaneous supporting classes. See the "readme.txt" file in the
OrbitTools directory for more information.

The files in the OrbitToolsDemo directory are compiled to make a 
sample executable program.  The program demonstrates how to use the 
OrbitTools assemblies to calculate the ECI position of a satellite, as 
well as determine the look angle from a location on the Earth to the 
satellite. See the "readme.txt" file in the OrbitToolsDemo directory
for more information.

The project files were compiled using Microsoft Visual Studio 2010.

For excellent information on the underlying physics of orbits, visible 
satellite observations, current NORAD TLE data, and other related material,
see http://www.celestrak.com which is maintained by Dr. T.S. Kelso.

Michael F. Henry
December, 2003
(Updated September, 2011)