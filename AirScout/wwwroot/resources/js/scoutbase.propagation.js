/**
 *
 * @source: scoutbase.elevation.js
 *
 * @licstart  The following is the entire license notice for the
 *  JavaScript code in this page.
 *
 * Copyright (C) 2023  DL2ALF
 *
 *
 * The JavaScript code in this page is free software: you can
 * redistribute it and/or modify it under the terms of the GNU
 * General Public License (GNU GPL) as published by the Free Software
 * Foundation, either version 3 of the License, or (at your option)
 * any later version.  The code is distributed WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU GPL for more details.
 *
 * As additional permission under GNU GPL version 3 section 7, you
 * may distribute non-source (e.g., minimized or compacted) forms of
 * that code without the copy of the GNU GPL normally required by
 * section 4, provided you include this license notice and a URL
 * through which recipients can access the Corresponding Source.
 *
 * @licend  The above is the entire license notice
 * for the JavaScript code in this page.
 *
 */

////////////////////////////////////////////////////////////////////////////////
// JavaScript library for ScoutBase propagation related classes and functions
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// Holds a single propagation point information
// lat: The latitude
// lon: The longitude
// h1:  The minimuum heigth an object must have at this point 
//      to be seen from location 1
// h2:  The minimuum heigth an object must have at this point 
//      to be seen from location 2
// f1:  The Fresnel Zone F1 radius at this point
////////////////////////////////////////////////////////////////////////////////
class PropagationPoint {

    constructor(lat, lon, h1, h2, f1) {
        this.lat = lat;
        this.lon = lon;
        this.h1 = h1;
        this.h2 = h2;
        this.f1 = f1;
    }
}

////////////////////////////////////////////////////////////////////////////////
// holds a propagation path
////////////////////////////////////////////////////////////////////////////////
class PropagationPath {

    constructor() {

        this.lat1 = 0;
        this.lon1 = 0;
        this.h1 = 0;

        this.lat2 = 0;
        this.lon2 = 0;
        this.h2 = 0;

        this.qrg = 0;
        this.radius = LatLon.Earth.radius;
        this.f1_Clearance = 0;
        this.stepWidth = 0;

        this.eps1_Min = 0;
        this.eps2_Min = 0;

        // path status: valid / invalid
        this.valid = false;
        // path status: selected / not selected
        this.selected = false;

        // path status local obstructed / not obstructed
        this.localObstruction = -Number.MAX_VALUE;

        // associated location info
        this.location1 = new LocationDesignator();
        this.location2 = new LocationDesignator();

        // assoicated qrv info
        this.qrv1 = new QRVDesignator();
        this.qrv2 = new QRVDesignator();
    }

    get distance() {

        return LatLon.distance(this.lat1, this.lon1, this.lat2, this.lon2);
    }

    get bearing12() {

        return LatLon.bearing(this.lat1, this.lon1, this.lat2, this.lon2);
    }

    get bearing21() {

        return LatLon.bearing(this.lat2, this.lon2, this.lat1, this.lon1);    }

    get IsLocalObstructed() {

        return this.localObstruction != null;
    }

    // returns an array of propagation points to draw a path on the map in 1km steps
    // as the vector of waypoints does not exist --> calculate waypoints in 1km steps
    get infoPoints() {

        // define array of infopoints
        var d = [];

        // generate 1km-points
        for (var i = 0; i < this.distance; i++) {

            var p = LatLon.destinationPoint(this.lat1, this.lon1, this.bearing12, i);
            d.push(new PropagationPoint(
                p.lat,
                p.lon,
                Propagation.heightFromEpsilon(this.h1, i, this.eps1_Min, this.radius),
                Propagation.heightFromEpsilon(this.h2, this.distance - i, this.eps2_Min, this.radius),
                Propagation.f1Radius(this.qrg, this.distance, this.distance - i)));
        }

        // push the endpoint excatly
        var p = LatLon.destinationPoint(this.lat1, this.lon1, this.bearing12, this.distance);
        d.push(new PropagationPoint(
            p.lat,
            p.lon,
            Propagation.heightFromEpsilon(this.h1, i, this.eps1_Min, this.radius),
            Propagation.heightFromEpsilon(this.h2, this.distance - i, this.eps2_Min, this.radius),
            Propagation.f1Radius(this.qrg, this.distance, this.distance - i)));

        // return the array
        return d;

    }

    getIntersectionPoint (lat, lon, bearing, maxdistance) {

        // get an interscetion point with the propagation path
        var p = LatLon.intersectionPoint(this.lat1, this.lon1, this.bearing12, lat, lon, bearing);

        // if not, return null
        if (p == null)
            return null;

        // get the minimum altitude a plane must have at intersection point
        // get both distances to intersection point
        var dist1 = LatLon.distance(this.lat1, this.lon1, p.lat, p.lon);
        var dist2 = LatLon.distance(this.lat2, this.lon2, p.lat, p.lon);

        // check against localobstruction , if any
        var eps1_min = Math.max(this.eps1_Min, this.localObstruction);

        // get minimal altitude
        var min_H = Math.max(Propagation.heightFromEpsilon(this.h1, dist1, eps1_min, this.radius), Propagation.heightFromEpsilon(this.h2, dist2, this.eps2_Min, this.radius));

        // get distance
        var qrb = LatLon.distance(p.lat, p.lon, lat, lon);
        if (maxdistance < 0)
            return new IntersectionPoint(p.lat, p.lon, qrb, min_H, dist1, dist2);
        if ((maxdistance == 0) && (qrb > this.distance / 2))
            return null;
        if (qrb < maxdistance)
            return null;

        // return intersection point
        return new IntersectionPoint(p.lat, p.lon, qrb, min_H, dist1, dist2);
    }
}


////////////////////////////////////////////////////////////////////////////////
// defines static propagation functions
////////////////////////////////////////////////////////////////////////////////

class Propagation {

    // returns the angle Alpha between an observer at 0° and an object in a given distance on Earth's surface seen from the Earth's midpoint
    static alphaFromDistance(dist, re) {

        return dist / re;
    }

    static slantRangeFromHeights(h, dist, H, radius) {

        // convert heights into [km]
        h = h / 1000.0;
        H = H / 1000.0;

        // calculate alpha angle in [rad]
        var alpha = Propagation.alphaFromDistance(dist, radius);
        var d = Math.sqrt((radius + H) * (radius + H) + (radius + h) * (radius + h) - (2.0 * (radius + H) * (radius + h) * Math.cos(alpha)));

        return d;
    }

    // returns the height of an object at a given distance (dist) an observer(h) would see at an angle Epsilon (eps)
    static heightFromEpsilon(h, dist, eps, radius) {

        // convert heights into [km]
        h = h / 1000.0;

        var beta = Math.PI / 2.0 + eps;
        var gamma = Math.PI - Propagation.alphaFromDistance(dist, radius) - beta;

        return ((radius + h) * Math.sin(beta) / Math.sin(gamma) - radius) * 1000;
    }

    //  returns the radius of the F1 Fresnel Zone
    static f1Radius(qrg, dist1, dist2) {

        return Math.sqrt(300.0 / qrg * dist1 * dist2 / (dist1 + dist2));
    }

    // returns the angle Epsilon from heights
    static epsilonFromHeights(h, dist, H, radius) {

        // calculate alpha angle in [rad]
        var alpha = dist / radius;

        // calculate slant range
        var d = Propagation.slantRangeFromHeights(h, dist, H, radius);

        // convert heights into [km]
        h = h / 1000.0;
        H = H / 1000.0;

        var a = ((radius + H) * (radius + H) - (radius + h) * (radius + h) - d * d) / 2.0 / d;
        var eps = Math.asin(a / (radius + h));

        return eps;
    }

    // returns the angle Theta a wavefront sent by an observer at (h) will meet an object at (bearing, distance, H) based on the object's horizontal line
    static thetaFromHeights(h, dist, H, radius) {

        // calculate alpha angle in [rad]
        var alpha = Propagation.alphaFromDistance(dist, radius);

        // convert heights into [km]
        h = h / 1000.0;
        H = H / 1000.0;

        var d = Math.sqrt((radius + h) * (radius + h) + (radius + H) * (radius + H) - 2.0 * (radius + h) * (radius + H) * Math.cos(alpha));
        var a = ((radius + H) * (radius + H) - (radius + h) * (radius + h) - d * d) / 2.0 / d;

        return Math.asin((a + d) / (radius + H));
    }
}

