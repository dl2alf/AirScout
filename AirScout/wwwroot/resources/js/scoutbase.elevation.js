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
// JavaScript library for ScoutBase elevation related classes and functions
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// Holds a single elevation point information
// lat: The latitude
// lon: The longitude
// elv: The elevation at this point
////////////////////////////////////////////////////////////////////////////////

class ElevationPoint {

    constructor(lat, lon, elv) {
        this.lat = lat;
        this.lon = lon;
        this.elv = elv;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Holds a local obstruction information around a location
// lat:         The latitude
// lon:         The longitude
// distance:    The distance to local obstructions around in 1° steps
// height:      The height of local obstructions around in 1° steps
////////////////////////////////////////////////////////////////////////////////

class LocalObstruction {

    constructor() {

        this.lat = 0;
        this.lon = 0;
        this.distance = {};
        this.height = {};
    }
}


////////////////////////////////////////////////////////////////////////////////
// holds a elevation path
////////////////////////////////////////////////////////////////////////////////

class ElevationPath {

    constructor() {

        this.lat1 = 0;
        this.lon1 = 0;

        this.lat2 = 0;
        this.lon2 = 0;

        this.stepWidth = 0;

        this.path = [];


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

    get count() {

        // check for invalid step width
        if (this.stepWidth == 0)
            return 0;

        return parseInt(this.distance * 1000.0 / this.stepWidth) + 1;
    }

    get distance() {

        return LatLon.distance(this.lat1, this.lon1, this.lat2, this.lon2);
    }

    get bearing12() {

        return LatLon.bearing(this.lat1, this.lon1, this.lat2, this.lon2);
    }

    get bearing21() {

        return LatLon.bearing(this.lat2, this.lon2, this.lat1, this.lon1);
    }

    // returns an array of integers to draw a elevation path on the map in 1km steps
    get infoPoints() {

        // define array of infopoints
        var d = [];
        var dist = this.distance;
        var count = this.count;

        // generate 1km-points
        for (var i = 0; i < dist; i++) {
            d.push(this.path[Math.trunc((i * count) / dist)]);
        }

        return d;
    }
}


////////////////////////////////////////////////////////////////////////////////
// defines static elevation functions
////////////////////////////////////////////////////////////////////////////////

class Elevation {

}

