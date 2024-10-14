/**
 *
 * @source: airscout.feed.js
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

// JavaScript library for AirScout plane feeds
class VRSWebServer {

    constructor() {

        this._settings = null;

        this._count1 = 0;
        this._count2 = 0;

        this._mean1 = 1;
        this._mean2 = 1;
        
        this._lastDV1 = "";
        this._lastDV2 = "";

        this._running = false;

    }

    static get name() {
        return "[WebFeed]           VRS Web Server";
    }

    static get running() {
        return this._running;
    }

    start = function(settings) {

        this._settings = settings;

        this._running = true;
        this.getPlanes(settings);
    }

    stop() {
        this._running = false;
    }

    _getPlanesFromURL = function (index) {

        var vc = new VarConverter();

        vc.addVar("MINLAT", AirScout.Settings.MinLat);
        vc.addVar("MAXLAT", AirScout.Settings.MaxLat);
        vc.addVar("MINLON", AirScout.Settings.MinLon);
        vc.addVar("MAXLON", AirScout.Settings.MaxLon);
        vc.addVar("MINALTM", AirScout.Settings.Planes_MinAlt);
        vc.addVar("MAXALTM", AirScout.Settings.Planes_MaxAlt);
        vc.addVar("MINALTFT", UnitConverter.m_ft(AirScout.Settings.Planes_MinAlt));
        vc.addVar("MAXALTFT", UnitConverter.m_ft(AirScout.Settings.Planes_MaxAlt));
        vc.addVar("UNIXTIME", Math.floor(Date.now() / 1000));
        vc.addVar("MYLAT", AirScout.Settings.MyLat);
        vc.addVar("MYLON", AirScout.Settings.MyLon);

        var mindist = 0;
        var maxdist = 0;

        // check the distance between MyLocation and edges of rect and take the maximum
        maxdist = Math.max(maxdist, LatLon.distance(AirScout.Settings.MyLat, AirScout.Settings.MyLon, AirScout.Settings.MinLat, AirScout.Settings.MinLon));
        maxdist = Math.max(maxdist, LatLon.distance(AirScout.Settings.MyLat, AirScout.Settings.MyLon, AirScout.Settings.MaxLat, AirScout.Settings.MinLon));
        maxdist = Math.max(maxdist, LatLon.distance(AirScout.Settings.MyLat, AirScout.Settings.MyLon, AirScout.Settings.MinLat, AirScout.Settings.MaxLon));
        maxdist = Math.max(maxdist, LatLon.distance(AirScout.Settings.MyLat, AirScout.Settings.MyLon, AirScout.Settings.MaxLat, AirScout.Settings.MaxLon));

        vc.addVar("MINDISTKM", mindist);
        vc.addVar("MAXDISTKM", maxdist);

        var lastDV = (index == 1) ? this.lastDV1 : this.lastDV2;
        vc.addVar("LASTDV", lastDV);

        var url = (index == 1) ? vc.replaceAllVars(this._settings.URL) : vc.replaceAllVars(this._settings.URL2);

        console.log("[" + new Date().toISOString() + "][VRSWebServer] Fetching planes from " + url);

        var start = new Date();

        var count = 0;
        var errors = 0;

        $.ajax({
            url: url,
            // Tell jQuery we're expecting JSONP
            dataType: "jsonp",
            context: this,
            // Work with the response
            success: function (response) {
                try {

//                    console.dir(response.acList);

                    for (var i = 0; i < response.acList.length; i++) {

                        try {

                            var time = new Date(0);
                            time.setUTCMilliseconds(response.acList[i].PosTime);

                            var plane = new AircraftDesignator();

                            plane.hex = response.acList[i].Icao;

                            plane.call = (!String.isNullOrEmpty(response.acList[i].Call)) ? response.acList[i].Call : "[unknown]";
                            plane.reg = (!String.isNullOrEmpty(response.acList[i].Reg)) ? response.acList[i].Reg : "[unknown]";

                            plane.type = (!String.isNullOrEmpty(response.acList[i].Type)) ? response.acList[i].Type : "[unknown]";
                            plane.model = (!String.isNullOrEmpty(response.acList[i].Mdl)) ? response.acList[i].Mdl : "[unknown]";

                            // sanitize model as to keep tooltips in map short
                            while (plane.model.length > 20) {
                                if (plane.model.includes("/")) {
                                    plane.model = plane.model.substring(0, plane.model.lastIndexOf("/") - 1);
                                }
                                else if (plane.model.includes(" ")) {
                                    plane.model = plane.model.substring(0, plane.model.lastIndexOf(" ") - 1);
                                }
                                else {
                                    plane.model = plane.model.substring(0, 20);
                                }
                            }

                            plane.manufacturer = (!String.isNullOrEmpty(response.acList[i].Man)) ? response.acList[i].Man : "";
                            if (response.acList[i].WTC != null) {
                                plane.category = response.acList[i].WTC;
                            }

                            // fake the SUPERHEAVY category as not in WTC
                            if (plane.type == "A388") plane.category = AircraftCategory.SUPERHEAVY;

                            plane.from = (response.acList[i].From != null) ? response.acList[i].From : "";
                            plane.to = (response.acList[i].To != null) ? response.acList[i].To : "";

                            var pos = new AircraftPosition();
                            pos.time = time;
                            pos.lat = (response.acList[i].Lat != null) ? response.acList[i].Lat : Number.NaN;
                            pos.lon = (response.acList[i].Long != null) ? response.acList[i].Long : Number.NaN;
                            pos.alt = (response.acList[i].GAlt != null) ? UnitConverter.ft_m(response.acList[i].GAlt) : Number.NaN;
                            pos.track = (response.acList[i].Trak != null) ? response.acList[i].Trak : Number.NaN;
                            pos.speed = (response.acList[i].Spd != null) ? UnitConverter.kts_kmh(response.acList[i].Spd) : Number.NaN;
                            pos.vSpeed = (response.acList[i].Vsi != null) ? UnitConverter.ft_m(response.acList[i].Vsi) : 0;

                            // sanitize data
                            if (isNaN(time) ||
                                (pos.lat == null) || Number.isNaN(pos.lat) || (pos.lat < -90) || (pos.lat > 90) ||
                                (pos.lon == null) || Number.isNaN(pos.lon) || (pos.lon < -180) || (pos.lon > 180) ||
                                (pos.alt == null) || Number.isNaN(pos.alt) || (pos.alt <= 0) || (pos.alt > AirScout.Settings.Planes_MaxAlt) ||
                                (pos.track == null) || Number.isNaN(pos.track) || (pos.track < 0) || (pos.track > 359) ||
                                (pos.speed == null) || Number.isNaN(pos.speed) || (pos.speed <= 10)
                            ) {

                                // error in plane data
                                errors++;
                            }
                            else {
                                plane.trail[time] = pos;
                                Aircrafts.add(plane);
                                count++;
                            }

                        }
                        catch (e) {
                            console.error(e);
                            errors++;
                        }
                    }

                    if (index == 1) {

                        var diff = new Date().getTime() - start.getTime();
                        this._count1 += diff;
                        this._mean1 = (this._mean1 + diff) / 2;
                        this.lastDV1 = response.lastDv;
                    }
                    else if (index == 2) {

                        var diff = new Date().getTime() - start.getTime();
                        this._count2 += diff;
                        this._mean2 = (this._mean2 + diff) / 2;
                        this.lastDV2 = response.lastDv;
                    }

                }
                catch (e) {
                    console.error(e);

                    // maintain counts in case of error
                    // this will result in "bad feed":"good feed" ratio of abt. 1:10
                    if (index == 1) {
                        this._count1 = this._count2 + this._mean2 * 10;
                    }
                    else if (index == 2) {
                        this._count2 = this._count1 + this._mean1 * 10;
                    }

                    errors++;
                }

                console.log("[" + new Date().toISOString() + "][VRSWebServer] " + count + " positions updated, " + errors + " errors, in memory " + Aircrafts.count + " planes.");
            },
            error: function (xhr, ajaxOptions, thrownError) {

                // maintain counts in case of error
                // this will result in "bad feed":"good feed" ratio of abt. 1:10
                if (index == 1) {
                    this._count1 = this._count2 + this._mean2 * 10;
                }
                else if (index == 2) {
                    this._count2 = this._count1 + this._mean1 * 10;
                }
            },
            async: false
        });

    }

    getPlanes = async function() {

        // already stopped?
        if (!this._running)
            return;

        // sanitize outdated planes
        Aircrafts.sanitize();

        if (!String.isNullOrEmpty(this._settings.URL2)) {
            if (!this._settings.LoadShare) {

                // no load sharing --> use URL1 by default, use URL2 if anything goes wrong
                try {

                    console.log("[" + new Date().toISOString() + "][VRSWebServer] Mode=REDUNDANCY,LoadShare=OFF");

                    // get plane data
                    await this._getPlanesFromURL(1);
                }
                catch (e) {

                    try {

                        console.log("[" + new Date().toISOString() + "][VRSWebServer] Mode=REDUNDANCY,LoadShare=OFF");

                        // get plane data
                        await this._getPlanesFromURL(2);

                    }
                    catch (e) {

                        // report error
                        console.error(e);
                    }
                }
            }
            else {

                // load sharing --> use URL with lowest overall loading time first, use the other if anything goes wrong
                if (this._count1 <= this._count2) {

                    console.log("[" + new Date().toISOString() + "][VRSWebServer] Mode=REDUNDANCY,LoadShare=ON,Count1=" + this._count1 + ", Count2=" + this._count2);

                    // get plane data
                    await this._getPlanesFromURL(1);
                }
                else {

                    console.log("[" + new Date().toISOString() + "][VRSWebServer] Mode=REDUNDANCY,LoadShare=ON,Count1=" + this._count1 + ", Count2=" + this._count2);

                    // get plane data
                    await this._getPlanesFromURL(2);
                }
            }
        }
        else {

        // no redundancy --> simple load from URL1
            this._count1 = 0;
            this._count2 = 0;

            console.log("[" + new Date().toISOString() + "][VRSWebServer] Mode=SIMPLE");
            await this._getPlanesFromURL(1);
        }

        // reset counters if before getting too near to overflow
        if ((this._count1 > Number.MAX_SAFE_INTEGER / 2) || (this._count2 > Number.MAX_SAFE_INTEGER / 2)) {
            this._count1 = 0;
            this._count2 = 0;
        }

        if (this._running) {

            // repeatedly call function after timeout
            setTimeout(function () { this.getPlanes(); }.bind(this), parseInt(this._settings.Timeout) * 1000);
        }
    }
}

