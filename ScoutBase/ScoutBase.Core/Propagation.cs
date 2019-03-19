using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoutBase.Core;


namespace ScoutBase.Core
{
    public static class Propagation
    {
        /// <summary>
        /// Returns the angle Alpha between an observer at 0° and an object in a given distance on Earth's surface seen from the Earth's midpoint
        /// </summary>
        /// <param name="dist">The distance of the object [km].</param>
        /// <param name="re">The equivalent earth radius [km].</param>
        /// <returns>The angle Alpha [rad].</returns>
        public static double AlphaFromDistance(double dist, double re)
        {
            return dist / re;
        }

        /// <summary>
        /// Returns the elevation angle Epsilon an object at (bearing, distance, H) is seen from an observer(h)
        /// </summary>
        /// <param name="h">The height of the observer [m].</param>
        /// <param name="dist">The distance of the object [km].</param>
        /// <param name="H">The height of the object [m].</param>
        /// <param name="radius">The equivalent earth radius [km].</param>
        /// <returns>The elevation angle Epsilon [rad].</returns>
        public static double EpsilonFromHeights(double h, double dist, double H, double radius)
        {
            // convert heights into [km]
            h = h / 1000.0;
            H = H / 1000.0;
            // calculate alpha angle in [rad]
            double alpha = dist / radius;
            double d = Math.Sqrt((radius + h) * (radius + h) + (radius + H) * (radius + H) - 2.0 * (radius + h) * (radius + H) * Math.Cos(alpha));
            double a = ((radius + H) * (radius + H) - (radius + h) * (radius + h) - d * d) / 2.0 / d;
            return Math.Asin(a / (radius + h));
        }

        /// <summary>
        /// Returns the angle a wavefront sent by an observer at (h) will meet an object at (bearing, distance, H) based on the object's horizontal line
        /// </summary>
        /// <param name="h">The height of the observer [m].</param>
        /// <param name="dist">The distance of the object [km].</param>
        /// <param name="H">The height of the object [m].</param>
        /// <param name="radius">The equivalent earth radius [km].</param>
        /// <returns>The angle Theta [rad].</returns>
        public static double ThetaFromHeights(double h, double dist, double H, double radius)
        {
            // convert heights into [km]
            h = h / 1000.0;
            H = H / 1000.0;
            // calculate alpha angle in [rad]
            double alpha = dist / radius;
            double d = Math.Sqrt((radius + h) * (radius + h) + (radius + H) * (radius + H) - 2.0 * (radius + h) * (radius + H) * Math.Cos(alpha));
            double a = ((radius + H) * (radius + H) - (radius + h) * (radius + h) - d * d) / 2.0 / d;
            return Math.Asin((a + d) / (radius + H));
        }

        /// <summary>
        /// Returns the height of an object at a given distance (dist) an observer(h) would see at an angle Epsilon (eps)
        /// </summary>
        /// <param name="h">The height of the observer [m].</param>
        /// <param name="dist">The distance of the object [km].</param>
        /// <param name="eps">The angle Epsilon [rad].</param>
        /// <param name="radius">The equivalent earth radius [km].</param>
        /// <returns>The height of the object [m].</returns>
        public static double HeightFromEpsilon(double h, double dist, double eps, double radius)
        {
            // convert heights into [km]
            h = h / 1000.0;
            double beta = Math.PI / 2.0 + eps;
            double gamma = Math.PI - AlphaFromDistance(dist,radius) - beta;
            return ((radius + h) * Math.Sin(beta) / Math.Sin(gamma) - radius) * 1000;
        }

        /// <summary>
        /// Returns the radius of the F1 Fresnel Zone
        /// </summary>
        /// <param name="qrg">The frequency in [GHz].</param>
        /// <param name="dist1">The distance from one end [km].</param>
        /// <param name="dist2">The distance from the other end [km].</param>
        /// <returns>The radius of the F1 Fresnel Zone in [m].</returns>
        public static double F1Radius(double qrg, double dist1, double dist2)
        {
            return Math.Sqrt(300.0 / qrg * dist1 * dist2 / (dist1 + dist2));
        }

        /// <summary>
        /// Returns the maximum distance an object (h2) can have to be seen from an observer (h1) --> visiblity distance
        /// </summary>
        /// <param name="h1">The height of the observer [m].</param>
        /// <param name="h2">The height of the object [m].</param>
        /// <param name="re">The equivalent earth radius [km].</param>
        /// <returns>The visibility distance in [km].</returns>
        public static double VisibilityDistance(double h1, double h2, double re)
        {
            // convert heights into [km]
            h1 = h1 / 1000.0;
            h2 = h2 / 1000.0;
            return Math.Sqrt(2 * re) * (Math.Sqrt(h1) + Math.Sqrt(h2));
        }


    }
}
