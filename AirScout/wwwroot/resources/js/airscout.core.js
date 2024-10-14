/**
 *
 * @source: airscout.core.js
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
// JavaScript library for AirScout core functions
////////////////////////////////////////////////////////////////////////////////


////////////////////////////////////////////////////////////////////////////////
// Enum for AirScout path mode
////////////////////////////////////////////////////////////////////////////////

const PathMode = {
    NONE : 0,
    SINGLE : 1,
    MULTI : 2,

    getStringValue: function (value) {

        if (value == 2) return "Multi";
        if (value == 1) return "Single";

        return "None";
    },

    getShortStringValue: function (value) {

        return AircraftCategory.getStringValue(value)[0];
    }
}


////////////////////////////////////////////////////////////////////////////////
// Enum for aircraft category
////////////////////////////////////////////////////////////////////////////////

const AircraftCategory = {
    NONE: 0,
    LIGHT: 1,
    MEDIUM: 2,
    HEAVY: 3,
    SUPERHEAVY: 4,

    getStringValue : function(value) {

        if (value == 4) return "Superheavy";
        if (value == 3) return "Heavy";
        if (value == 2) return "Medium";
        if (value == 1) return "Light";

        return "None";
    },

    getShortStringValue : function(value) {

        return AircraftCategory.getStringValue(value)[0];
    }
}


////////////////////////////////////////////////////////////////////////////////
// holds a possible intersection point
////////////////////////////////////////////////////////////////////////////////

class IntersectionPoint {

    constructor(lat, lon, qrb, min_h, dist1, dist2) {

        this.lat = lat;
        this.lon = lon;
        this.qrb = qrb;
        this.min_H = min_h;
        this.dist1 = dist1;
        this.dist2 = dist2;
    }
}


////////////////////////////////////////////////////////////////////////////////
// holds aircraft position at a distinct time
////////////////////////////////////////////////////////////////////////////////

class AircraftPosition {

    constructor(time, lat, lon, alt, speed, vspeed, track) {

        this.time = time;
        this.lat = 0;
        this.lon = 0;
        this.alt = 0;
        this.speed = 0;
        this.vSpeed = 0;
        this.track = 0;
    }
}


////////////////////////////////////////////////////////////////////////////////
// holds complete aircraft info
////////////////////////////////////////////////////////////////////////////////

class AircraftDesignator {

    constructor() {

        this.hex = "";
        this.call = "";
        this.reg = "";

        this.time = new Date(0);
        this.lat = 0;
        this.lon = 0;
        this.alt = 0;
        this.speed = 0;
        this.vSpeed = 0;
        this.track = 0;

        this.trail = {};

        this.type = "";
        this.manufacturer = "";
        this.model = "";
        this.category = AircraftCategory.NONE;
        this.from = "";
        this.to = "";

        // is nearest plane
        this.nearest = false;

        // is mouse over plane
        this.mouseOver = false;

        // is slected plane
        this.selected = false;

        // possible interscection point with a path
        this.intPotential = 0;
        this.intPoint = null;
        this.intDist = Number.MAX_VALUE;
        this.intAltDiff = 0;
        this.intTime = new Date(0);
        this.intAngle = 0;

        // possible angles from path endpoints
        this.eps1 = 0;
        this.eps2 = 0;
        this.theta1 = 0;
        this.theta2 = 0;

        // map objects 
        this.sprite = null;
        this.intLine = null;
        this.tooltip = null;
        this.ttIcon = null;
        this.ttLine = null;

        // time of latest position
        this.lastPos = new Date(0);
    }

    at = function (at, ttl = 5) {

        // check if any usable position is stored and time to live is not over
        // and all relevant properties are filled
        // estimate position at a distinct time using trail sprite,
        if ((this.trail == null) || (Object.keys(this.trail).length == 0)) return null;

        // get the latest entry from trail
        var times = Object.keys(this.trail);
        times.sort();
        var latest = this.trail[times[times.length - 1]];

        // check if TTL is over
        var diff = at.getTime() - latest.time.getTime();
        if (diff > ttl * 60000) return null;

        if (latest.speed < 10) {
            console.error("[Arcraft.at] Invalid speed!");
            console.dir(this);
        }

        // estimate position from latest position
        var dist = latest.speed * diff / 3600000;
        var newpos = LatLon.destinationPoint(latest.lat, latest.lon, latest.track, dist);
        this.time = new Date(at.getTime());
        this.lat = newpos.lat;
        this.lon = newpos.lon;
        this.alt = latest.alt;
        this.speed = latest.speed;
        this.vSpeed = latest.vSpeed;
        this.track = latest.track;

        // return sprite
        return this;
    }
}


////////////////////////////////////////////////////////////////////////////////
// holds a list with all aircraft informations
////////////////////////////////////////////////////////////////////////////////

const Aircrafts = {

    _aircrafts : {},

    // clears all aircraft info
    clear : function () {

        Aircrafts._aircrafts = {};
    },

    // sanitizes all aircraft info --> removes all outdated aircraft datasets
    sanitize : function () {

        if ((Aircrafts._aircrafts == null) || (Object.keys(Aircrafts._aircrafts).length == 0))
            return;

        Object.values(Aircrafts._aircrafts).forEach(ac => {

            if ((new Date().getTime() - ac.lastPos.getTime()) / 60000 > AirScout.Settings.Planes_Position_TTL) {

                // delete map objects

                Aircrafts._aircrafts[ac.hex].sprite = null;
                Aircrafts._aircrafts[ac.hex].intLine = null;
                Aircrafts._aircrafts[ac.hex].tooltip = null;
                Aircrafts._aircrafts[ac.hex].ttIcon = null;
                Aircrafts._aircrafts[ac.hex].ttLine = null;

                // delete aircrafts
                delete Aircrafts._aircrafts[ac.hex];
            }

        });
    },

    // gets a list of all aircrafts
    getAircraftList: function () {

        return Object.values(this._aircrafts);
    },

    // adds a new aircraft/aircraft position to the list
    add : function (ac) {

        // check for mandatory properties
        if (String.isNullOrEmpty(ac.hex)) return null;

        var idx = String(ac.hex);

        if (Aircrafts._aircrafts[idx] == null) {

            // add new aircraft to list
            Aircrafts._aircrafts[idx] = new AircraftDesignator();
        }

        // update aircraft info and add position to trail
        if (!String.isNullOrEmpty(idx)) Aircrafts._aircrafts[idx].hex = idx;
        if (!String.isNullOrEmpty(ac.call)) Aircrafts._aircrafts[idx].call = ac.call;
        if (!String.isNullOrEmpty(ac.reg)) Aircrafts._aircrafts[idx].reg = ac.reg;

        if (!String.isNullOrEmpty(ac.model)) Aircrafts._aircrafts[idx].model = ac.model;
        if (!String.isNullOrEmpty(ac.manufacturer)) Aircrafts._aircrafts[idx].manufacturer = ac.manufacturer;
        if (!String.isNullOrEmpty(ac.type)) Aircrafts._aircrafts[idx].type = ac.type;
        if (ac.category != AircraftCategory.NONE) Aircrafts._aircrafts[idx].category = ac.category;
        if (!String.isNullOrEmpty(ac.from)) Aircrafts._aircrafts[idx].from = ac.from;
        if (!String.isNullOrEmpty(ac.to)) Aircrafts._aircrafts[idx].to = ac.to;

        for (const time in ac.trail) {
            // key: the name of the object key
            // index: the ordinal position of the key within the object 

            if ((ac.trail[time].lat == null) || Number.isNaN(ac.trail[time].lat) || (ac.trail[time].lat < -90) || (ac.trail[time].lat > 90) ||
                (ac.trail[time].lon == null) || Number.isNaN(ac.trail[time].lon) || (ac.trail[time].lon < -180) || (ac.trail[time].lon > 180) ||
                (ac.trail[time].alt == null) || Number.isNaN(ac.trail[time].alt) || (ac.trail[time].alt <= 0) || (ac.trail[time].alt > AirScout.Settings.Planes_MaxAlt) ||
                (ac.trail[time].track == null) || Number.isNaN(ac.trail[time].track) || (ac.trail[time].track < 0) || (ac.trail[time].track > 359) ||
                (ac.trail[time].speed == null) || Number.isNaN(ac.trail[time].speed) || (ac.trail[time].speed <= 10)
            ) {

                // invalid track entry!
                // console.error("[Arcraft.at] Invalid track entry!");
                // console.dir(ac.trail[time]);
            }
            else {
                Aircrafts._aircrafts[idx].trail[time] = ac.trail[time];
                Aircrafts._aircrafts[idx].lastPos = new Date(Math.max.apply(null, Object.values(ac.trail).map(item => item.time)));

            }
        };

    },

    getAircraftInfo : function (key) {

        return Aircrafts._aircrafts[key];
    },

    clearMouseHovers : function () {

        for (const key of Object.keys(Aircrafts._aircrafts)) {
            Aircrafts._aircrafts[key].MouseHover = false;
        }

    },

    clearSelections : function () {

        for (const key of Object.keys(Aircrafts._aircrafts)) {
            Aircrafts._aircrafts[key].selected = false;
        }

    },

    get count() {
        return Object.keys(Aircrafts._aircrafts).length;
    },

    cloneAircraftList : function() {

        var aclist = [];
        Object.values(Aircrafts._aircrafts).forEach(plane => {

            var ac = Object.assign({}, plane);
            aclist.push(ac);
        });

        return aclist;
    },

    resetNearest : function(aircrafts) {

        Object.values(Aircrafts._aircrafts).forEach(val => {
            try {

                val.nearest = false;

                val.intPoint = null;
                val.intDist = Number.MAX_VALUE;
                val.intAltDiff = 0;
                val.intTime = new Date(0);
                val.intPotential = 0;
                val.eps1 = 0;
                val.eps2 = 0;
                val.theta1 = 0;
                val.theta2 = 0;
                val.squint = 0;
            }
            catch (e) {
            }
        });
    },

    // sets the nearest planes meeting nearestPlanes conditions in relation to one path
    _setNearestPlanes : function(at, ppath, maxradius, maxdist, maxalt, aclist) {

        var count = 0;
        var maxrad = maxradius;

      // adjust maxradius when automatic calculation is required
        if (maxradius < 0)
            maxrad = Number.MAX_VALUE;
        if (maxradius == 0)
            maxrad = ppath.distance / 2;

        // get midpoint value
        var midlat = LatLon.midPoint(ppath.lat1, ppath.lon1, ppath.lat2, ppath.lon2).lat;
        var midlon = LatLon.midPoint(ppath.lat1, ppath.lon1, ppath.lat2, ppath.lon2).lon;

        // update intersection info for each plane in list
        Object.values(aclist).forEach(val => {
            try {

                try {
                    // update plane with estimated new position
                    var sprite = val.at(at);
                }
                catch (e) {
                    console.error(val.hex + ": " + e);
                }

                // skip if plane is too old
                if (sprite == null) return;

                var dist = LatLon.distance(midlat, midlon, sprite.lat, sprite.lon);

                // skip if plane is out of range
                if (dist > maxrad) return;

                count++;

                // calculate four possible intersections
                // i1 -->       plane heading
                // i2 -->       plane heading +90°
                // i3 -->       plane heading - 90°
                // i4 -->       opposite plane heading
                // imin -->     intpoint with shortest distance

                var imin = null;
                var i1 = null;
                var i2 = null;
                var i3 = null;
                var i4 = null;

                i1 = ppath.getIntersectionPoint(sprite.lat, sprite.lon, sprite.track, 0);
                i2 = ppath.getIntersectionPoint(sprite.lat, sprite.lon, ppath.bearing12 - 90, 0);
                // calculate right opposite direction only if no left intersection was found
                if (i2 == null)
                    i3 = ppath.getIntersectionPoint(sprite.lat, sprite.lon, ppath.bearing12 + 90, 0);
                // calcalute opposite direction only if no forward intersection was found
                if (i1 == null)
                    i4 = ppath.getIntersectionPoint(sprite.lat, sprite.lon, sprite.track - 180, 0);
                // find the minimum distance first
                if (i1 != null)
                    imin = i1;
                if ((i2 != null) && ((imin == null) || (i2.qrb < imin.qrb)))
                    imin = i2;
                if ((i3 != null) && ((imin == null) || (i3.qrb < imin.qrb)))
                    imin = i3;
                if ((i4 != null) && ((imin == null) || (i4.qrb < imin.qrb)))
                    imin = i4;

                // check hot planes which are very near the path first
                if ((imin != null) && (imin.qrb <= maxdist) && (imin.dist1 <= ppath.distance)) {
                    // plane is near path
                    // use the minimum qrb info
                    sprite.intPoint = new GeographicalPoint(imin.lat, imin.lon);
                    sprite.intDist = imin.qrb;
                    sprite.intAltDiff = sprite.alt - imin.min_H;
                    sprite.intTime = new Date(at.getTime());
                    sprite.intTime.setSeconds(sprite.intDist / sprite.speed * 3600);
                    var c1 = LatLon.bearing(sprite.intPoint.lat, sprite.intPoint.lon, ppath.lat2, ppath.lon2);
                    var c2 = sprite.track;
                    var ca = c1 - c2;
                    if (ca < 0)
                        ca = ca + 360;
                    if ((ca > 180) && (ca < 360))
                        ca = 360 - ca;
                    // save in rad 
                    sprite.intAngle = ca / 180.0 * Math.PI;
                    sprite.eps1 = Propagation.epsilonFromHeights(ppath.h1, imin.dist1, sprite.alt, ppath.radius);
                    sprite.eps2 = Propagation.epsilonFromHeights(ppath.h2, imin.dist2, sprite.alt, ppath.radius);
                    sprite.theta1 = Propagation.thetaFromHeights(ppath.h1, imin.dist1, sprite.alt, ppath.radius);
                    sprite.theta2 = Propagation.thetaFromHeights(ppath.h2, imin.dist2, sprite.alt, ppath.radius);
                    sprite.squint = Math.abs(Propagation.thetaFromHeights(ppath.h1, imin.dist1, sprite.alt, ppath.radius) - Propagation.thetaFromHeights(ppath.h2, imin.dist2, sprite.alt, ppath.radius));

                    if (sprite.intAltDiff > 0) {
                        // plane is high enough
                        sprite.intPotential = 100;
                    }
                    else if (imin.min_H <= maxalt) {
                        // plane is not high enough yet but might be in the future
                        sprite.intPotential = 50;
                    }
                }
                else {
                    // plane is far from path --> check only intersection i1 = planes moves towards path
                    if ((i1 != null) && (i1.min_H <= maxalt) && (i1.dist1 <= ppath.distance)) {
                        sprite.intPoint = new GeographicalPoint(i1.lat, i1.lon);
                        sprite.intDist = i1.qrb;
                        sprite.intAltDiff = sprite.alt - i1.min_H;
                        sprite.intTime = new Date(at.getTime());
                        sprite.intTime.setSeconds(sprite.intDist / sprite.speed * 3600);
                        sprite.eps1 = Propagation.epsilonFromHeights(ppath.h1, i1.dist1, sprite.alt, ppath.radius);
                        sprite.eps2 = Propagation.epsilonFromHeights(ppath.h2, i1.dist2, sprite.alt, ppath.radius);
                        sprite.squint = Math.abs(Propagation.thetaFromHeights(ppath.h1, imin.dist1, sprite.alt, ppath.radius) - Propagation.thetaFromHeights(ppath.h2, imin.dist2, sprite.alt, ppath.radius));
                        if (sprite.intAltDiff > 0) {
                            // plane wil cross path in a suitable altitude
                            sprite.intPotential = 75;
                        }
                        else {
                            // plane wil cross path not in a suitable altitude
                            sprite.intPotential = 50;
                        }
                    }
                }

                // set flag
                val.nearest = true;

            }
            catch (e) {
                console.error(e);
            }
        });

    },


    // sets the Nearest property for all planes meeting nearestPlanes conditions
    setNearestPlanes : function(at, ppaths, maxradius, maxdist, maxalt) {

        try {


            // reset Nearest property and clear all intersection values at source data point
            Aircrafts.resetNearest(Aircrafts.aircrafts);

            if ((ppaths == null) || (ppaths.length == 0))
                return;

            ppaths.forEach(ppath => {

                this._setNearestPlanes(at, ppath, maxradius, maxdist, maxalt, Aircrafts._aircrafts);
            });
        }
        catch (e) {
            console.error(e);
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// holds complete airport info
////////////////////////////////////////////////////////////////////////////////
class AirportDesignator {

    constructor() {

        this.icao = "";
        this.iata = "";
        this.lat = 0;
        this.lon = 0;
        this.alt = 0;
        this.airport = "";
        this.country = "";
    }
}

////////////////////////////////////////////////////////////////////////////////
// holds all AirScout specific functions
// must be a class definition due to ability to write properties
////////////////////////////////////////////////////////////////////////////////
class AirScout {

    // holds the root url
    static _rootURL = "http://10.0.2.143:9880";

    static Settings = AirScout._getSettings();

    // gets the settings
    static _getSettings() {

        var _settings = null;

        // fetch settings from URL
        $.ajax({
            url: AirScout._rootURL + "/settings.json",
            method: "GET",
            success: function (response) {
                _settings = response;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getSettings] Error while getting settings: " + thrownError);
                _settings = null;
           },
            async: false
        });

        return _settings;
    }

    static AircraftCategories = AirScout._getAircraftCategories();

    // gets the aircraft categories
    static _getAircraftCategories() {

        var _aircraftCategories = [];
        $.ajax({
            url: AirScout._rootURL + "/aircraftcategories.json",
            method: "GET",
            success: function (response) {
                if (response !== null) {
                    response.forEach(item => {
                        _aircraftCategories.push(SupportFunctions.convertObject(item));
                    });
                }
                else {
                    _aircraftCategories = null;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getAircraftCategories] Error while getting categories: " + thrownError);
                _aircraftCategories = null;
            },
            async: false
        });

        return _aircraftCategories;
    };

    static PlaneFeedSettings = AirScout._getPlaneFeedSettings();
    
    // gets the plane feed settings
    static _getPlaneFeedSettings () {

        var _planefeedsettings = [];

        // fetch settings from URL
        $.ajax({
            url: AirScout._rootURL + "/planefeedsettings.json",
            method: "GET",
            success: function (response) {
                _planefeedsettings = response;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getPlaneFeedSettings] Error while getting settings: " + thrownError);
                _planefeedsettings = null;
            },
            async: false
        });

        return _planefeedsettings;
    };

    static Bands = AirScout._getBands();
    
    // gets the available bands
    static _getBands() {

        var _bands = [];

        $.ajax({
            url: AirScout._rootURL + "/bandvalues.json",
            method: "GET",
            success: function (response) {
                if (response !== null) {
                    response.forEach(item => {
                        _bands.push(SupportFunctions.convertObject(item));
                    });
                }
                else {
                    _bands = null;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getBands] Error while getting bands: " + thrownError);
                _bands = null;
           },
            async: false
        });

        return _bands;
    };

    // gets settings for a band
    static getBandSettings = function (band) {
        var bandstr = Band.toString(band);
        var settings = AirScout.Settings.Path_Band_Settings.find(b => b.BAND == bandstr);

        return settings;
    };

    static Airports = AirScout._getAirports();
    
    // gets the airports
    static _getAirports() {

        var _airports = [];

        $.ajax({
            url: AirScout._rootURL + "/airports.json",
            method: "GET",
            success: function (response) {
                if (response !== null) {
                    response.forEach(item => {
                        var airport = new AirportDesignator();
                        SupportFunctions.copyObject(item, airport);
                        _airports.push(airport);
                    });
                }
                else {
                    _airports = null;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getAirports] Error while getting airports: " + thrownError);
                _airports = null;
            },
            async: false
        });

        return _airports;
    };

    // gets a location 
    static getLocation(call, loc) {

        var location = new LocationDesignator();
        var url = (loc == null) ?
            AirScout._rootURL + "/location.json?call=" + call.toUpperCase() + "&bestcaseelevation=" + AirScout.Settings.Path_BestCaseElevation :
            AirScout._rootURL + "/location.json?call=" + call.toUpperCase() + "&loc=" + loc.toUpperCase() + "&bestcaseelevation=" + AirScout.Settings.Path_BestCaseElevation;
        $.ajax({
            url: url,
            method: "GET",
            success: function (response) {
                if (response !== null) {
                    SupportFunctions.copyObject(response, location);
                }
                else {
                    location = null;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getLocation] Error while getting location: " + thrownError);
                location = null;
            },
            async: false
        });

        return location;

    };

    static getLocations(call) {

        var locations = [];
        var url = AirScout._rootURL + "/location.json?call=" + call.toUpperCase() + "&loc=ALL";
        $.ajax({
            url: url,
            method: "GET",
            success: function (response) {
                if (response !== null) {

                    response.forEach(item => {
                        var location = new LocationDesignator();
                        SupportFunctions.copyObject(item, location);
                        locations.push(location);
                    });
                }
                else {
                    locations = null;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getLocations] Error while getting locations: " + thrownError);
                locations = null;
            },
            async: false
        });

        return locations;

    };

    // gets a elevation path
    static getElevationPath(mycall, mylat, mylon, dxcall, dxlat, dxlon) {

        var epath = new ElevationPath();
        var url = AirScout._rootURL + "/elevationpath.json?mycall=" + mycall + "&mylat=" + mylat + "&mylon=" + mylon + "&dxcall=" + dxcall + "&dxlat=" + dxlat + "&dxlon=" + dxlon;
        $.ajax({
            url: url,
            method: "GET",
            success: function (response) {
                SupportFunctions.copyObject(response, epath);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getElevationPath] Error while getting path: " + thrownError);
                epath = null;
            },
            async: false
        });

        return epath;
    };

    // gets a propagation path
    static getPropagationPath(mycall, mylat, mylon, dxcall, dxlat, dxlon, band) {

        var ppath = new PropagationPath();
        var url = AirScout._rootURL + "/propagationpath.json?mycall=" + mycall + "&mylat=" + mylat + "&mylon=" + mylon + "&dxcall=" + dxcall + "&dxlat=" + dxlat + "&dxlon=" + dxlon + "&band=" + band;
        $.ajax({
            url: url,
            method: "GET",
            success: function (response) {
                SupportFunctions.copyObject(response, ppath);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("[" + new Date().toISOString() + "][AirScout.getPropagationPath] Error while getting path: " + thrownError);
                ppath = null;
           },
            async: false
        });

        return ppath;
    }
}

