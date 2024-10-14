 // JavaScript library for ScoutBase core functions
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// holds spherical earth functions
////////////////////////////////////////////////////////////////////////////////
const LatLon = {

    Earth : { Radius : 6371 },

    // returns the distance between two locations in km
    distance : function(mylat, mylon, lat, lon) {
        var R = LatLon.Earth.Radius;
        var dLat = (mylat - lat);
        var dLon = (mylon - lon);
        var a = Math.sin(dLat / 180 * Math.PI / 2) * Math.sin(dLat / 180 * Math.PI / 2) +
            Math.sin(dLon / 180 * Math.PI / 2) * Math.sin(dLon / 180 * Math.PI / 2) * Math.cos(mylat / 180 * Math.PI) * Math.cos(lat / 180 * Math.PI);
        return R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    },

    // returns the bearing from my location to location in degrees
    bearing : function (mylat, mylon, lat, lon) {
        var dLat = (lat - mylat);
        var dLon = (lon - mylon);
        var y = Math.sin(dLon / 180 * Math.PI) * Math.cos(lat / 180 * Math.PI);
        var x = Math.cos(mylat / 180 * Math.PI) * Math.sin(lat / 180 * Math.PI) - Math.sin(mylat / 180 * Math.PI) * Math.cos(lat / 180 * Math.PI) * Math.cos(dLon / 180 * Math.PI);
        return (Math.atan2(y, x) / Math.PI * 180 + 360) % 360;
    },

    // returns the midpoint between two locations in km
    midPoint : function(mylat, mylon, lat, lon) {
        // more accurate spherical midpoint
        var aslatdiff = (mylat - lat);
        var aslondiff = (mylon - lon);
        var bx = Math.cos(lat / 180 * Math.PI) * Math.cos(-aslondiff / 180 * Math.PI);
        var by = Math.cos(lat / 180 * Math.PI) * Math.sin(-aslondiff / 180 * Math.PI);
        var latm = Math.atan2(Math.sin(mylat / 180 * Math.PI) + Math.sin(lat / 180 * Math.PI), Math.sqrt((Math.cos(mylat / 180 * Math.PI) + bx) * (Math.cos(mylat / 180 * Math.PI) + bx) + by * by)) / Math.PI * 180;
        var lonm = mylon + Math.atan2(by, Math.cos(mylat / 180 * Math.PI) + bx) / Math.PI * 180;
        return new GeographicalPoint(latm, lonm);
    },

    // returns a destination point from my location at given bearing and distance
    destinationPoint : function(mylat, mylon, bearing, distance) {
        var R = LatLon.Earth.Radius;
        var lat = Math.asin(Math.sin(mylat / 180 * Math.PI) * Math.cos(distance / R) + Math.cos(mylat / 180 * Math.PI) * Math.sin(distance / R) * Math.cos(bearing / 180 * Math.PI));
        var t1 = Math.sin(mylat / 180 * Math.PI);
        var t2 = Math.cos(distance / R);
        var t3 = Math.cos(mylat / 180 * Math.PI) * Math.sin(distance / R);
        var t4 = Math.cos(bearing / 180 * Math.PI);
        var lon = mylon / 180 * Math.PI + Math.atan2(Math.sin(bearing / 180 * Math.PI) * Math.sin(distance / R) * Math.cos(mylat / 180 * Math.PI), Math.cos(distance / R) - Math.sin(mylat / 180 * Math.PI) * Math.sin(lat));
        return new GeographicalPoint(lat / Math.PI * 180, lon / Math.PI * 180);
    },

    // returns an intersection point from my location at my bearing and location at bearing, NULL if no intersection
    intersectionPoint : function(mylat, mylon, mybearing, lat, lon, bearing) {
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

        return new GeographicalPoint(lat3 / Math.PI * 180, lon3 / Math.PI * 180);
    }


}



////////////////////////////////////////////////////////////////////////////////
// holds Geographical point functions
////////////////////////////////////////////////////////////////////////////////

class GeographicalPoint {

    static PositionLimits = class {

        // Lower latitude limit.
        static LowerLatitudeLimit = -90;

        // Upper latitude limit.
        static UpperLatitudeLimit = +90;

        // Lower longitude limit.
        static LowerLongitudeLimit = -180;

        // Upper longitude limit.
        static UpperLongitudeLimit = +180;
    }

    // constructor
    constructor(lat, lon) {
        this.lat = lat;
        this.lon = lon;
    }

    // Checks geographical coordinates.
    static check(latitude, longitude) {
        return GeographicalPoint.checkLatitude(latitude) && GeographicalPoint.checkLongitude(longitude);
    }

    // Checks a geographical latitude.
    static checkLatitude(latitude) {
        return (latitude >= GeographicalPoint.PositionLimits.LowerLatitudeLimit) && (latitude <= GeographicalPoint.PositionLimits.UpperLatitudeLimit);
    }

    // Checks a geographical longitude.
    static checkLongitude(longitude) {
        return (longitude >= GeographicalPoint.PositionLimits.LowerLongitudeLimit) && (longitude <= GeographicalPoint.PositionLimits.UpperLongitudeLimit);
    }

    // Shifts a geographical point within a rectangle.
    static shiftPositionInRectangle(point, positionInRectangle, height, width) {

        var latitude = point.lat;
        var longitude = point.lon;

        switch (positionInRectangle) {
            case PositionInRectangle.TopLeft:
            case PositionInRectangle.TopMiddle:
            case PositionInRectangle.TopRight:
                latitude += height;
                break;
        }

        switch (positionInRectangle) {
            case PositionInRectangle.MiddleLeft:
            case PositionInRectangle.MiddleMiddle:
            case PositionInRectangle.MiddleRight:
                latitude += height / 2;
                break;
        }

        switch (positionInRectangle) {
            case PositionInRectangle.TopRight:
            case PositionInRectangle.MiddleRight:
            case PositionInRectangle.BottomRight:
                longitude += width;
                break;
        }

        switch (positionInRectangle) {
            case PositionInRectangle.TopMiddle:
            case PositionInRectangle.MiddleMiddle:
            case PositionInRectangle.BottomMiddle:
                longitude += width / 2;
                break;
        }

        return new GeographicalPoint(latitude, longitude);
    }


    // creates a GeographicalPoint at a distinct position inside a 'Maidenhead Locator'
    static fromLoc(maidenheadLocator, positionInRectangle = PositionInRectangle.MiddleMiddle) {

        if (!MaidenheadLocator.check(maidenheadLocator))
            throw new Error("Not a valid Maidenhead Locator: " + maidenheadLocator);

        // upper cases
        maidenheadLocator = maidenheadLocator.toUpperCase();

        var precision = maidenheadLocator.length / 2;

        var latitude = GeographicalPoint.PositionLimits.LowerLatitudeLimit;
        var longitude = GeographicalPoint.PositionLimits.LowerLongitudeLimit;

        // zone size for step "0"
        var height = GeographicalPoint.PositionLimits.UpperLatitudeLimit - GeographicalPoint.PositionLimits.LowerLatitudeLimit;
        var width = GeographicalPoint.PositionLimits.UpperLongitudeLimit - GeographicalPoint.PositionLimits.LowerLongitudeLimit;

        for (var step = 1; step <= precision; step++) {

            // calculate step values
            var zones;
            var firstCharacter;

            if (step % 2 == 0) {

                // step is even
                zones = MaidenheadLocator.Constants.ZonesEvenSteps;
                firstCharacter = MaidenheadLocator.Constants.FirstEvenStepsCharacter;
            }
            else {

                // step is odd
                zones = (step == 1 ? MaidenheadLocator.Constants.ZonesOddStep1 : MaidenheadLocator.Constants.ZonesOddStepsExcept1);
                firstCharacter = MaidenheadLocator.Constants.FirstOddStep1Character;
            }

            // zone size of current step
            height /= zones;
            width /= zones;

            // retrieve precision specific geographical coordinates
            var longitudeStep = 0;
            var latitudeStep = 0;
            {
                var error = false;
                var position = -1;

                if (!error) {

                    // longitude
                    position = step * 2 - 2;
                    var locatorCharacter = maidenheadLocator[position].charCodeAt(0);
                    var zone = locatorCharacter - firstCharacter.charCodeAt(0);

                    if (zone >= 0 && zone < zones) {
                        longitudeStep = zone * width;
                    }
                    else {
                        error = true;
                    }
                }

                if (!error) {

                    // latitude
                    position = step * 2 - 1;
                    var locatorCharacter = maidenheadLocator[position].charCodeAt(0);
                    var zone = locatorCharacter - firstCharacter.charCodeAt(0);

                    if (zone >= 0 && zone < zones) {
                        latitudeStep = zone * height;
                    }
                    else {
                        error = true;
                    }
                }

                if (error) {
                    throw new Error("Not a valid Maidenhead Locator: " + maidenheadLocator);
                }
            }

            longitude += longitudeStep;
            latitude += latitudeStep;
        }

        //Corrections according argument positionInRectangle
        return GeographicalPoint.shiftPositionInRectangle(new GeographicalPoint(latitude, longitude), positionInRectangle, height, width);
    }
}

// enum for position in rectangle
class PositionInRectangle {

    // TopLeft
    static TopLeft = new PositionInRectangle("TopLeft");
    // TopMiddle
    static TopMiddle = new PositionInRectangle("TopMiddle");
    // TopRight
    static TopRight = new PositionInRectangle("TopRight");
    // BottomLeft
    static BottomLeft = new PositionInRectangle("BottomLeft");
    // BottomMiddle
    static BottomMiddle = new PositionInRectangle("BottomMiddle");
    // BottomRight
    static BottomRight = new PositionInRectangle("BottomRight");
    // MiddleLeft
    static MiddleLeft = new PositionInRectangle("MiddleLeft");
    // MiddleRight
    static MiddleRight = new PositionInRectangle("MiddleRight");
    // MiddleMiddle
    static MiddleMiddle = new PositionInRectangle("MiddleMiddle");

    constructor(name) {
        this.name = name;
    }
    toString() {
        return this.name;
    }
}



////////////////////////////////////////////////////////////////////////////////
// holds Geographical rect functions
////////////////////////////////////////////////////////////////////////////////

class GeographicalRect {

    // constructor
    constructor(minlat, minlon, maxlat, maxlon) {
        this.MinLat = minlat;
        this.MinLon = minlon;
        this.MaxLat = maxlat;
        this.MaxLon = maxlon;
    }

    // creates a GeographicalRect representing the bounds of a 'Maidenhead Locator' (latitudes and longitudes, in degrees).
    static FromLoc(maidenheadLocator) {

        var gmin = GeographicalPoint.fromLoc(maidenheadLocator, PositionInRectangle.BottomLeft);
        var gmax = GeographicalPoint.fromLoc(maidenheadLocator, PositionInRectangle.TopRight);
        return new GeographicalRect(gmin.lat, gmin.lon, gmax.lat, gmax.lon);
    }

}


////////////////////////////////////////////////////////////////////////////////
// holds Maidenhead locator functions
////////////////////////////////////////////////////////////////////////////////

class MaidenheadLocator {

    static Constants = class {

        // Number of zones for 'Field' (Precision step 1).
        static ZonesOddStep1 = 18;

        // Number of zones for 'Subsquare', 'Subsubsubsquare', etc. (Precision steps 3, 5, etc.).
        static ZonesOddStepsExcept1 = 24;

        // Number of zones for 'Square', 'Subsubsquare', etc. (Precision steps 2, 4, etc.).
        static ZonesEvenSteps = 10;

        // The first character for 'Field' (Precision step 1).
        static FirstOddStep1Character = 'A';


        // The first character for 'Subsquare', 'Subsubsubsquare', etc. (Precision steps 3, 5, etc.).
        static FirstOddStepsExcept1Character = 'a';

        // The first character for 'Square', 'Subsubsquare', etc. (Precision steps 2, 4, etc.).
        static FirstEvenStepsCharacter = '0';

        // The lowest allowed precision.
        static MinPrecision = 1;

        // The highest allowed precision.
        static MaxPrecision = 9;
    }

    static Utilities = class {

        // Rounds to a specified interval.
        static Round(value, interval) {

            var y = value / interval;
            var q = parseInt(Math.ceil(y));
            var result = q * interval;

            return result;
        }
    }

    SubGridsFilter = class {

        // All subgrids.
        static All = new MaidenheadLocator.SubGridFilter("All");
        // Top subgrids only.
        static Top = new MaidenheadLocator.SubGridFilter("Top");
        // Bottom subgrids only.
        static Bottom = new MaidenheadLocator.SubGridFilter("Bottom");
        // Left subgrids only.
        static Left = new MaidenheadLocator.SubGridFilter("Left");
        // Right subgrids only.
        static Right = new MaidenheadLocator.SubGridFilter("Right");

        constructor(name) {
            this.name = name;
        }
        toString() {
            return this.name;
        }
    }

    // Checks if a string is a valid Maidenhead Locator.
    static check(maidenheadLocator) {

        // check for empty string
        if (String.isNullOrEmpty(maidenheadLocator))
            return false;

        // make string upper
        maidenheadLocator = maidenheadLocator.toUpperCase();

        // string must have even length
        if (maidenheadLocator.length % 2 != 0)
            return false;

        // string must have MinPrecision
        if (maidenheadLocator.length / 2 < MaidenheadLocator.Constants.MinPrecision)
            return false;

        // string must not have more than MaxPrecision
        if (maidenheadLocator.length / 2 > MaidenheadLocator.Constants.MaxPrecision)
            return false;

        for (var i = 0; i < maidenheadLocator.length; i++) {
            if ((i % 4) <= 1) {
                if ((maidenheadLocator[i] < 'A') || (maidenheadLocator[i] > 'X'))
                    return false;
            }
            else {
                if (!maidenheadLocator[i].isNumber())
                    return false;
            }
        }

        // return OK
        return true;
    }

    // Checks if a given location (by lat/lon) is precise, e.g. is not located at midpoint of a given Maidenhead Locator
    static isPrecise(latitude, longitude, precision = 3) {

        // get locator from lat/lon
        var loc = MaidenheadLocator.fromLatLon(latitude, longitude, false, precision);

        // return false, if locator cannot be calculated
        if (String.isNullOrEmpty(loc))
            return false;

        // get locator midpoint
        var midpoint = GeographicalPoint.fromLoc(loc, PositionInRectangle.MiddleMiddle);

        // return true, if lat/lon are different from midpoint
        return (Math.abs(midpoint.lat - latitude) > 0.00001) || (Math.abs(midpoint.lon - longitude) > 0.00001);
    }

    // gets the zone values
    static _getZone(value, interval) {
        var factor = value / interval;
        var result = parseInt(factor);

        //Numerical corrections
        {
            var roundedValue = MaidenheadLocator.Utilities.Round(value, interval);
            var diff = roundedValue - value;

            if (diff < 0.0000000000001) {
                if (roundedValue > value) {
                    result++;
                }
            }
        }

        return result;
    }

    // Converts a given Maidenhead Locator with format options
    static convert(maidenheadLocator, smallLettersForSubsquares) {

        if (!Check(maidenheadLocator))
            throw new Error("This is not a valid Maidenhead Locator: " + maidenheadLocator);

        var loc = "";
        for (i = 0; i < maidenheadLocator.length; i++) {
            if (i <= 1)
                loc = loc + Char.toUpperCase(maidenheadLocator[i]);
            else if (smallLettersForSubsquares) {
                loc = loc + Char.toLowerCase(maidenheadLocator[i]);
            }
            else {
                loc = loc + Char.toUpperCase(maidenheadLocator[i]);
            }
        }

        // return locator
        return loc;
    }

    // creates a 'Maidenhead Locator' from geographical coordinates (latitude and longitude, in degrees)  until a specific precision.
    static fromLatLon(latitude, longitude, smallLettersForSubsquares = false, precision = 3, autolength = false) {

        // check arguments
        if (!GeographicalPoint.check(latitude, longitude))
            return null;

        // corrections: MinPrecision <= precision <= MaxPrecision
        precision = Math.min(MaidenheadLocator.Constants.MaxPrecision, Math.max(MaidenheadLocator.Constants.MinPrecision, precision));

        var loc = "";

        // autolength = false -- > use p=precision and do one single run
        var p;
        if (!autolength)
            p = precision;
        else
            p = 3;

        do {

            var locatorCharacters = [];

            var latitudeWork = latitude + (-GeographicalPoint.PositionLimits.LowerLatitudeLimit);
            var longitudeWork = longitude + (-GeographicalPoint.PositionLimits.LowerLongitudeLimit);

            // zone size for step "0"
            var height = GeographicalPoint.PositionLimits.UpperLatitudeLimit - GeographicalPoint.PositionLimits.LowerLatitudeLimit;
            var width = GeographicalPoint.PositionLimits.UpperLongitudeLimit - GeographicalPoint.PositionLimits.LowerLongitudeLimit;

            for (var step = MaidenheadLocator.Constants.MinPrecision; step <= precision; step++) {
                // calculate step values
                var zones;
                var firstCharacter;

                if (step % 2 == 0) {

                    // step is even
                    zones = MaidenheadLocator.Constants.ZonesEvenSteps;
                    firstCharacter = MaidenheadLocator.Constants.FirstEvenStepsCharacter;
                }
                else {

                    // step is odd
                    zones = (step == 1 ? MaidenheadLocator.Constants.ZonesOddStep1 : MaidenheadLocator.Constants.ZonesOddStepsExcept1);
                    firstCharacter = ((step >= 3 && smallLettersForSubsquares) ? MaidenheadLocator.Constants.FirstOddStepsExcept1Character : MaidenheadLocator.Constants.FirstOddStep1Character);
                }


                // zone size of current step
                height /= zones;
                width /= zones;

                // retrieve zones and locator characters
                var latitudeZone;
                var longitudeZone;
                {
                    longitudeZone = MaidenheadLocator._getZone(longitudeWork, width);
                    {
                        var locatorCharacter = String.fromCharCode(firstCharacter.charCodeAt(0) + longitudeZone);
                        locatorCharacters.push(locatorCharacter);
                    }

                    latitudeZone = MaidenheadLocator._getZone(latitudeWork, height);
                    {
                        var locatorCharacter = String.fromCharCode(firstCharacter.charCodeAt(0) + latitudeZone);
                        locatorCharacters.push(locatorCharacter);
                    }
                }

                if (step <= MaidenheadLocator.Constants.MaxPrecision - 1) {

                    // prepare the next step
                    {
                        latitudeWork -= latitudeZone * height;
                        longitudeWork -= longitudeZone * width;

                        // numerical corrections
                        {
                            if (latitudeWork < 0) {
                                latitudeWork = 0;
                            }

                            if (longitudeWork < 0) {
                                longitudeWork = 0;
                            }
                        }
                    }
                }
            }

            // build the result (Locator text)
            loc = locatorCharacters.join("");

            if (autolength && MaidenheadLocator.isPrecise(latitude, longitude, p))
                p++;
            else
                break;
        }
        while (p <= precision);

        // return locator
        return loc;
    }
}

////////////////////////////////////////////////////////////////////////////////
// holds call sign functions
////////////////////////////////////////////////////////////////////////////////

class Callsign {

    // cuts callsign from prefix and/or suffix
    static cut(call) {

        if (String.isNullOrEmpty(call))
            return call;

        try {

            call.trim().toUpperCase();
            if (call.indexOf('/') >= 0) {

                // cut suffix
                if (call.lastindexOf('/') >= call.length - 4)
                    call = call.slice(0, call.lastindexOf('/') - 1)

                // cut prefix
                if (call.indexOf('/') >= 0)
                    call = call.slice(call.indexOf('/') + 1);

                // cut suffix again
                if (call.lastindexOf('/') >= call.length - 4)
                    call = call.slice(0, call.lastindexOf('/') - 1)

            }
            return call;
        }
        catch
        {
            // error while cutting call
            return "";
        }
    }

    // checks for valid callsign
    static check(call) {

        try {

            // empty string
            if (String.isNullOrEmpty(call))
                return false;

            call = call.trim();
            call = call.toUpperCase();

            // test for invalid chars
            for (var j = 0; j < call.length; j++)
            {
                if (((call[j] < 'A') || (call[j] > 'Z')) &&
                    ((call[j] < '0') || (call[j] > '9')) &&
                    ((call[j] != '/'))) {
                    return false;
                }
            }

            // check for count and position of slashes
            if (call.startsWith("/"))
                return false;

            if (call.endsWith("/"))
                return false;

            if (call.split('/').Length - 1 > 2)
                return false;

            // cut call from prefix/suffix
            call = Callsign.cut(call);

            // call too short
            if (call.length < 3)
                return false;

            // if first char is a number 
            if (call[0].isNumber()) {

                // second char must be a letter and third char must be a number(e.g. 9A5O)
                if (call[1].isLetter() && call[2].isNumber())
                    return true;
                else
                    return false;
            }
            // first char is a letter
            else {

                // second char is a letter 
                if (call[1].isLetter()) {

                    // third char must be a number(e.g.DL0GTH)
                    if (call[2].isNumber())
                        return true;
                    else
                        return false;
                }
                // second char is a number
                else {

                    // third char must be a letter (e.g. G7RAU)
                    if (call[2].isLetter())
                        return true;

                    // at least forth char must be a letter (e.g. T91CO)
                    if (call[3].isLetter())
                        return true;

                    return false;
                }
            }
        }
        catch
        {
            return false;
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// holds band functions
////////////////////////////////////////////////////////////////////////////////

class Band {

    static toString(band) {

        switch (band) {
            case 0: return "Null"; break;
            case 50: return "50M"; break;
            case 70: return "70M"; break;
            case 144: return "144M"; break;
            case 432: return "432M"; break;
            case 1296: return "1.2G"; break;
            case 2320: return "2.3G"; break;
            case 3400: return "3.4G"; break;
            case 5760: return "5.7G"; break;
            case 10368: return "10G"; break;
            case 24048: return "24G"; break;
            case 47088: return "47G"; break;
            case 76032: return "76G"; break;
            case 999999999: return "All"; break;
            default: return ""; break;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// holds several support functions
////////////////////////////////////////////////////////////////////////////////

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

    // function to get formatted string from number
    // positivesign:    show positive sign
    // leading:         number of places before decimal point (not including sign)
    // trailing:        number of places after decimal point
    static numberToString(number, positivesign, leading, trailing) {

        // calculate length
        var length = positivesign ? 1 + leading + 1 + trailing : (number < 0 ? 1 : 0) + leading + 1 + trailing;

        // check if number exceeds length --> show it anyway
        var s = number.toFixed(trailing);
        if (s.length > length)
            return s;

        // convert absolute value
        s = Math.abs(number).toFixed(trailing);

        // pad with leading zeros
        s = s.padStart(leading + 1 + trailing, "0");

        // add sign
        s = s + ((number < 0) ? "-" : positivesign ? "+" : "");

        return s;
    }

    // copies over all properties of destination found in source
    // can handle different style guides, e.g. lowercase, camelCase
    static copyObject(src, dst) {

        // initialize conversion lists
        var equals = {};
        var camelcases = {};
        var uppercases = {};

        // special array treatment
        if (dst.constructor.name == "Array") {

            if (src.constructor.name == "Array") {

                src.forEach(item => {
                    dst.push(SupportFunctions.convertObject(item));
                });
            }
            else {
                throw new Error("Type mismatch detected (conversion not possible)!");
            }

        }
        else {

            Object.keys(dst).forEach(function (key, index) {

                var srckeys = [];
                // find 1:1 equivalents
                srckeys = Object.keys(src).filter(srckey =>
                    srckey == key
                );
                if (srckeys.length > 1) {
                    throw new Error("Ambigous property names detected (conversion not possible): " + key);
                }
                else if (srckeys.length == 1) {

                    // copy cascaded objects first
                    if (dst[key] === Object(dst[key])) {

                        // special datetime treatment
                        if (dst[key].constructor.name == "Date") {

                            dst[key] = new Date(src[srckeys[0]]);
                        }
                        else {

                            // check if source is null --> set dest = null
                            if (src[srckeys[0]] === null) {
                                dst[key] = null; 
                            }
                            else {
                                if (src[srckeys[0]] === Object(src[srckeys[0]])) {

                                    SupportFunctions.copyObject(src[srckeys[0]], dst[key]);
                                }
                                else {
                                    throw new Error("Ambigous property names detected (conversion not possible): " + key);
                                }
                            }
                        }
                    }
                    else {
                        dst[key] = src[srckeys[0]];
                    }

                    equals[key] = srckeys[0];
                }

                // find camelCase equivalents then
                srckeys = Object.keys(src).filter(srckey =>
                    srckey == (key[0].toUpperCase() + key.substring(1))
                );
                if (srckeys.length > 1) {
                    throw new Error("Ambigous property names detected (conversion not possible): " + key);
                }
                if (srckeys.length == 1) {

                    // check if already in another conversion list
                    if (Object.keys(equals).includes(key))
                        throw new Error("Ambigous property names detected (conversion not possible): " + key);

                    // copy cascaded objects first
                    if (dst[key] === Object(dst[key])) {

                        // special datetime treatment
                        if (dst[key].constructor.name == "Date") {

                            dst[key] = new Date(src[srckeys[0]]);
                        }
                        else {

                            // check if source is null --> set dest = null
                            if (src[srckeys[0]] === null) {
                                dst[key] = null;
                            }
                            else {
                                if (src[srckeys[0]] === Object(src[srckeys[0]])) {

                                    SupportFunctions.copyObject(src[srckeys[0]], dst[key]);
                                }
                                else {
                                    throw new Error("Ambigous property names detected (conversion not possible): " + key);
                                }
                            }
                        }
                    }
                    else {
                        dst[key] = src[srckeys[0]];
                    }

                    camelcases[key] = srckeys[0];
                }

                // find UPPERCASE equivalents then
                srckeys = Object.keys(src).filter(srckey =>
                    srckey == key.toUpperCase()
                );
                if (srckeys.length > 1) {
                    throw new Error("Ambigous property names detected (conversion not possible): " + key);
                }
                if (srckeys.length == 1) {

                    // check if already in another conversion list
                    if (Object.keys(equals).includes(key))
                        throw new Error("Ambigous property names detected (conversion not possible): " + key);
                    if (Object.keys(camelcases).includes(key))
                        throw new Error("Ambigous property names detected (conversion not possible): " + key);

                    // copy cascaded objects first
                    if (dst[key] === Object(dst[key])) {

                        // special datetime treatment
                        if (dst[key].constructor.name == "Date") {

                            dst[key] = new Date(src[srckeys[0]]);
                        }
                        else {

                            // check if source is null --> set dest = null
                            if (src[srckeys[0]] === null) {
                                dst[key] = null;
                            }
                            else {
                                if (src[srckeys[0]] === Object(src[srckeys[0]])) {

                                    SupportFunctions.copyObject(src[srckeys[0]], dst[key]);
                                }
                                else {
                                    throw new Error("Ambigous property names detected (conversion not possible): " + key);
                                }
                            }
                        }
                    }
                    else {
                        dst[key] = src[srckeys[0]];
                    }

                    uppercases[key] = srckeys[0];
                }
            });
        }
    }

    // copies over all properties of destination found in source
    // can handle different style guides, e.g. lowercase, camelCase
    static convertObject(src) {

        // check if object
        if (src === Object(src)) {

            var dst = {};

            Object.keys(src).forEach(function (key, index) {

                // make newkey as 1st char lowerCase
                var newkey = key[0].toLowerCase() + key.substring(1);

                // check if newkey not already exists
                if (!Object.keys(dst).includes(newkey)) {

                    // chekc if cascaded object --> call convert recursively
                    if (src[key] === Object(src[key])) {

                        dst[newkey] = SupportFunctions.convertObject(src[key]);
                    }
                    else {

                        // special datetime treatment from JSON
                        if ((src[key].length == 20)
                            && (src[key][4] == '-')
                            && (src[key][7] == '-')
                            && (src[key][10] == 'T')
                            && (src[key][13] == ':')
                            && (src[key][16] == ':')
                            && (src[key][19] == 'Z')) {

                            dst[newkey] = new Date(src[key]);
                        }
                        else {
                            dst[newkey] = src[key];
                        }
                    }
                }
                else {
                    throw new Error("Ambigous property names detected (conversion not possible): " + key);
                }
            });

            // special array treatment
            if (src.constructor.name == "Array") {
                dst = Object.values(dst);
            }

            return dst;
        }
        else {

            // copy simple values
            return src;
        }
    }
}

// returns true if string is NULL or empty
String.isNullOrEmpty = function (value) {

    var b = typeof value == 'string' && (value == "");
    b = b || typeof value == "undefined";
    b = b || value === null;
    return b;
}


// returns true if string is number
String.prototype.isNumber = function () {

    if (String.isNullOrEmpty(this))
        return false;

    return !isNaN(this - parseFloat(this));
}

// returns true if string contains only letters
String.prototype.isLetter = function () {

    if (String.isNullOrEmpty(this))
        return false;

    var s = this.toUpperCase();
    for (var i = 0; i < s.length;  i++) {
        if ((s[i] < "A") || (s[i] > "Z"))
            return false;
    }

    return true;
}

////////////////////////////////////////////////////////////////////////////////
// holds several unit converters
////////////////////////////////////////////////////////////////////////////////

class UnitConverter {

    static ft_m(feet) {

        return feet / 3.28084;
    }

    static m_ft(m) {

        return m * 3.28084;
    }

    static kts_kmh(kts) {

        return kts * 1.852;
    }

    static kmh_kts(kmh) {

        return kmh / 1.852;
    }

    static km_mi(km) {

        return km * 1.609;
    }

    static mi_km(mi) {

        return mi / 1.609;
    }
}

////////////////////////////////////////////////////////////////////////////////
// holds a variable converter
////////////////////////////////////////////////////////////////////////////////

class VarConverter {

    static get Separator() {
        return '%';
    }

    #dir = null;

    constructor() {
        this.#dir = {};
    }

    // adds/updates an entry in dictionary with its string value
    addVar(name, value) {

        this.#dir[name.toUpperCase()] = value;
    }

    // finds an entry in dictionary and returns its value
    getValue(name) {

        if (this.#dir.hasOwnProperty(name.toUpperCase()))
            return this.#dir[name.toUpperCase()];
        else
            return null;
    }

    // replaces all vars in string and returns the result
    replaceAllVars(s) {

        // check for var separators first
        if (s.includes(VarConverter.Separator)) {
            // OK, string is containing vars --> crack the string first and replace vars
            try {
                var a = s.split(VarConverter.Separator);
                // as we are always using a pair of separators the length of a[] must be odd
                if (a.length % 2 == 0)
                    throw "Number of separators is not an even number.";
                // create new string and replace all vars (on odd indices)
                s = "";
                for (var i = 0; i < a.length; i++)
                {
                    if (i % 2 == 0) {
                        // cannot be not a var on that position
                        s = s + a[i];
                    }
                    else {
                        // var identifier: upper the string and try to convert
                        if (this.#dir.hasOwnProperty(a[i].toUpperCase())) {
                            s = s + this.#dir[a[i].toUpperCase()];
                        }
                        else {
                            throw "Var identifier not found: " + a[i];
                        }
                    }
                }
            }
            catch (e)
            {
                // throw an excecption 
                throw "Error while parsing string for variables [" + e.Message + "]: " + s;
            }
        }
        return s;
    }
}

