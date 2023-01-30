// JavaScript library for ScoutBase core functions

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// holds spherical earth functions
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
class LatLon {

    // returns the distance between two locations in km
    static Distance(mylat, mylon, lat, lon) {
        var R = LatLon.Earth.Radius();
        var dLat = (mylat - lat);
        var dLon = (mylon - lon);
        var a = Math.sin(dLat / 180 * Math.PI / 2) * Math.sin(dLat / 180 * Math.PI / 2) +
            Math.sin(dLon / 180 * Math.PI / 2) * Math.sin(dLon / 180 * Math.PI / 2) * Math.cos(mylat / 180 * Math.PI) * Math.cos(lat / 180 * Math.PI);
        return R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    }

    // returns the bearing from my location to location in degrees
    static Bearing(mylat, mylon, lat, lon) {
        var dLat = (lat - mylat);
        var dLon = (lon - mylon);
        var y = Math.sin(dLon / 180 * Math.PI) * Math.cos(lat / 180 * Math.PI);
        var x = Math.cos(mylat / 180 * Math.PI) * Math.sin(lat / 180 * Math.PI) - Math.sin(mylat / 180 * Math.PI) * Math.cos(lat / 180 * Math.PI) * Math.cos(dLon / 180 * Math.PI);
        return (Math.atan2(y, x) / Math.PI * 180 + 360) % 360;
    }

    // returns the midpoint between two locations in km
    static MidPoint(mylat, mylon, lat, lon) {
        // more accurate spherical midpoint
        var aslatdiff = (mylat - lat);
        var aslondiff = (mylon - lon);
        var bx = Math.cos(lat / 180 * Math.PI) * Math.cos(-aslondiff / 180 * Math.PI);
        var by = Math.cos(lat / 180 * Math.PI) * Math.sin(-aslondiff / 180 * Math.PI);
        var latm = Math.atan2(Math.sin(mylat / 180 * Math.PI) + Math.sin(lat / 180 * Math.PI), Math.sqrt((Math.cos(mylat / 180 * Math.PI) + bx) * (Math.cos(mylat / 180 * Math.PI) + bx) + by * by)) / Math.PI * 180;
        var lonm = mylon + Math.atan2(by, Math.cos(mylat / 180 * Math.PI) + bx) / Math.PI * 180;
        return new LatLon.GPoint(latm, lonm);
    }

    // returns a destination point from my location at given bearing and distance
    static DestinationPoint(mylat, mylon, bearing, distance) {
        var R = LatLon.Earth.Radius();
        var lat = Math.asin(Math.sin(mylat / 180 * Math.PI) * Math.cos(distance / R) + Math.cos(mylat / 180 * Math.PI) * Math.sin(distance / R) * Math.cos(bearing / 180 * Math.PI));
        var t1 = Math.sin(mylat / 180 * Math.PI);
        var t2 = Math.cos(distance / R);
        var t3 = Math.cos(mylat / 180 * Math.PI) * Math.sin(distance / R);
        var t4 = Math.cos(bearing / 180 * Math.PI);
        var lon = mylon / 180 * Math.PI + Math.atan2(Math.sin(bearing / 180 * Math.PI) * Math.sin(distance / R) * Math.cos(mylat / 180 * Math.PI), Math.cos(distance / R) - Math.sin(mylat / 180 * Math.PI) * Math.sin(lat));
        return new LatLon.GPoint(lat / Math.PI * 180, lon / Math.PI * 180);
    }

    // returns an intersection point from my location at my bearing and location at bearing, NULL if no intersection
    static IntersectionPoint(mylat, mylon, mybearing, lat, lon, bearing) {
        var dLat = (lat - mylat);
        var dLon = (lon - mylon);
        var brng13 = mybearing / 180 * Math.PI;
        var brng23 = bearing / 180 * Math.PI;
        var brng12 = 0;
        var brng21 = 0;
        var dist12 = 2 * Math.asin(Math.sqrt(Math.sin(dLat / 2 / 180 * Math.PI) * Math.sin(dLat / 2 / 180 * Math.PI) + Math.cos(mylat / 180 * Math.PI) * Math.cos(lat / 180 * Math.PI) * Math.sin(dLon / 2 / 180 * Math.PI) * Math.sin(dLon / 2 / 180 * Math.PI)));
        if (dist12 == 0) return null;

        // initial/final bearings between points
        var brngA = Math.acos((Math.sin(lat / 180 * Math.PI) - Math.sin(mylat / 180 * Math.PI) * Math.cos(dist12)) / (Math.sin(dist12) * Math.cos(mylat / 180 * Math.PI)));
        // protect against rounding
        if (isNaN(brngA))
            brngA = 0;
        var brngB = Math.acos((Math.sin(mylat / 180 * Math.PI) - Math.sin(lat / 180 * Math.PI) * Math.cos(dist12)) / (Math.sin(dist12) * Math.cos(lat / 180 * Math.PI)));
        if (Math.sin(lon / 180 * Math.PI - mylon / 180 * Math.PI) > 0) {
            brng12 = brngA;
            brng21 = 2 * Math.PI - brngB;
        }
        else {
            brng12 = 2 * Math.PI - brngA;
            brng21 = brngB;
        }
        var alpha1 = (brng13 - brng12 + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 2-1-3
        var alpha2 = (brng21 - brng23 + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 1-2-3

        if ((Math.sin(alpha1) == 0) && (Math.sin(alpha2) == 0))
            return null;  // infinite intersections
        if (Math.sin(alpha1) * Math.sin(alpha2) < 0)
            return null;       // ambiguous intersection

        //alpha1 = Math.abs(alpha1);
        //alpha2 = Math.abs(alpha2);
        // ... Ed Williams takes abs of alpha1/alpha2, but seems to break calculation?

        var alpha3 = Math.acos(-Math.cos(alpha1) * Math.cos(alpha2) + Math.sin(alpha1) * Math.sin(alpha2) * Math.cos(dist12));
        var dist13 = Math.atan2(Math.sin(dist12) * Math.sin(alpha1) * Math.sin(alpha2), Math.cos(alpha2) + Math.cos(alpha1) * Math.cos(alpha3));
        var lat3 = Math.asin(Math.sin(mylat / 180 * Math.PI) * Math.cos(dist13) + Math.cos(mylat / 180 * Math.PI) * Math.sin(dist13) * Math.cos(brng13));
        var dLon13 = Math.atan2(Math.sin(brng13) * Math.sin(dist13) * Math.cos(mylat / 180 * Math.PI), Math.cos(dist13) - Math.sin(mylat / 180 * Math.PI) * Math.sin(lat3));
        var lon3 = mylon / 180 * Math.PI + dLon13;
        lon3 = (lon3 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalise to -180..+180º

        return new LatLon.GPoint(lat3 / Math.PI * 180, lon3 / Math.PI * 180);
    }


}

LatLon.Earth = class {

    static Radius() {
        return 6371;
    }
}

LatLon.GPoint = class {

    constructor(lat, lon)
    {
        this.Lat = lat;
        this.Lon = lon;
    }
}





///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Holds a single propagation point information
// Lat: The latitude
// Lon: The longitude
// H1:  The minimuum heigth an object must have at this point to be seen from location 1
// H2:  The minimuum heigth an object must have at this point to be seen from location 2
// F1:  The Fresnel Zone F1 radius at this point
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
class PropagationPoint {

    constructor(lat, lon, h1, h2, f1) {
        this.Lat = lat;
        this.Lon = lon;
        this.H1 = h1;
        this.H2 = h2;
        this.F1 = f1;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// defines static propagation functions
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
class Propagation {

    // returns the angle Alpha between an observer at 0° and an object in a given distance on Earth's surface seen from the Earth's midpoint
    static AlphaFromDistance(dist, re) {
        return dist / re;
    }

    // returns the height of an object at a given distance (dist) an observer(h) would see at an angle Epsilon (eps)
    static HeightFromEpsilon(h, dist, eps, radius) {
        // convert heights into [km]
        h = h / 1000.0;
        var beta = Math.PI / 2.0 + eps;
        var gamma = Math.PI - Propagation.AlphaFromDistance(dist, radius) - beta;
        return ((radius + h) * Math.sin(beta) / Math.sin(gamma) - radius) * 1000;
    }

    //  returns the radius of the F1 Fresnel Zone
    static F1Radius(qrg, dist1, dist2) {
        return Math.sqrt(300.0 / qrg * dist1 * dist2 / (dist1 + dist2));
    }

    // returns an array of propagation points to draw a path on the map in 1km steps
    static GetInfoPoints(ppath) {

        // define array of infopoints
        var d = [];

        // generate 1km-points
        for (var i = 0; i < ppath['Distance']; i++) {
            // check against localobstruction , if any
            var eps1_min = Math.max(ppath['Eps1_Min'], ppath['LocalObstruction']);
            var p = LatLon.DestinationPoint(ppath['Lat1'], ppath['Lon1'], ppath['Bearing12'], i);
            d.push(new PropagationPoint(
                p.Lat,
                p.Lon,
                Propagation.HeightFromEpsilon(ppath['h1'], i, ppath['Eps1_Min'], ppath['Radius']),
                Propagation.HeightFromEpsilon(ppath['h2'], ppath['Distance'] - i, ppath['Eps2_Min'], ppath['Radius']),
                Propagation.F1Radius(ppath['QRG'], ppath['Distance'], ppath['Distance'] - i)));
        }

        // return the array
        return d;

    }

}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// holds several support functions
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

class SupportFunctions {


    // function to get cookie
    static getCookie(c_name) {
        var i, x, y, ARRcookies = document.cookie.split(";");
        for (i = 0; i < ARRcookies.length; i++) {
            x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
            y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
            x = x.replace(/^\s+|\s+$/g, "");
            if (x == c_name) {
                return unescape(y);
            }
        }
}


}

