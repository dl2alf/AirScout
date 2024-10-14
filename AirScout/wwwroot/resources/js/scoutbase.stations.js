/**
 *
 * @source: scoutbase.stations.js
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
// JavaScript library for ScoutBase station related functions
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// Enum that holds the geosource of a station location
// UNKNOWN:    The source is unknown
// FROMLOC:    The source is taken from the midpoint of the locator
// FROMUSER:   The source is taken from a user input (precise location)
// FROMBEST:   The source is taken from the best case calculation (highest point inside the locator)
////////////////////////////////////////////////////////////////////////////////
const GeoSource = {

    UNKNOWN : 0,
    FROMLOC : 1,
    FROMUSER : 2,
    FROMBEST: 3,

    getStringValue: function (value) {

        if (value == 3) return "FromBest";
        if (value == 2) return "FromUser";
        if (value == 1) return "FromLoc";

        return "Unknown";
    },
}

////////////////////////////////////////////////////////////////////////////////
// Holds a single station location information
// call:        The call
// loc:         The 6-digit Maidenhead locator
// lat:         The latitude
// lon:         The longitude
// source:      The source of information
// hits:        The hit count of thi location in the station database
// elevation:   The elevation at this point
// lastUpdated: The timestamp of last update
////////////////////////////////////////////////////////////////////////////////
class LocationDesignator {

    constructor() {

        this.call = "";
        this.loc = "";
        this.lat = 0;
        this.lon = 0;

        this.elevation = 0;

        this.source = GeoSource.UNKNOWN;
        
        this.hits = 0;

        this.lastUpdated = new Date(0);

    }

    get location() {

        return new GeographicalPoint(this.lat, this.lon);
    }

    get isBestCase() {

        return this.source == GeoSource.FROMBEST;
    }
}

////////////////////////////////////////////////////////////////////////////////
// Holds a single station QRV information
// call:            The call
// loc:             The 6-digit Maidenhead locator
// band:            The band
// antennaHeight:   The antenna height above ground [m]
// antennaGain:     The antenna gain [dBd]
// power:           The power output [W]
// lastUpdated:     The timestamp of last update
////////////////////////////////////////////////////////////////////////////////
class QRVDesignator {

    constructor() {

        this.call = "";
        this.loc = "";
        this.band = 0;
        this.antennaHeight = 0;
        this.antennaGain = 0;
        this.power = 0;

        this.lastUpdated = new Date(0);
    }
}
        



