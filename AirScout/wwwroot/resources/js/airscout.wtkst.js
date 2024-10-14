/**
 *
 * @source: airscout.wtkst.js
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
// JavaScript library for AirScout <> wtKST communication
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// holds all basic functions for communication with wtKST via websocket
////////////////////////////////////////////////////////////////////////////////
const wtKST = {

    _socket : null,
    _lastMsg : new Date(),

    start : function() {

        try {

            if (wtKST._socket == null) {

                if (wtKST._socket != null) {
                    wtKST._socket.close();
                }

                // clear websocket anyway
                wtKST._socket = null;

                // create new websocket
                wtKST._socket = new ReconnectingWebSocket(AirScout.Settings.KST_Websocket, null, { debug: true, timeoutInterval: 10000 });
                wtKST._socket.onopen = wtKST._onOpen;
                wtKST._socket.onmessage = wtKST._onMessage;
                wtKST._socket.onerror = wtKST._onError;
                wtKST._socket.onclose = wtKST._onClose;
            }

            var diff = (new Date().getTime() - wtKST._lastMsg.getTime()) / 1000;
            console.log("[" + new Date().toISOString() + "][wtKST] Connection check, last message: " + diff + " sec ago, timeout: " + AirScout.Settings.KST_WebSocket_Timeout + " sec.");
            if ((wtKST._socket != null) && (wtKST._socket.readyState == 1)) {

                // connection check
                var json = JSON.stringify({ task: "test", data: { aircraft_count: Aircrafts.count } });
                wtKST.send(json);

                if (diff > AirScout.Settings.KST_WebSocket_Timeout) {

                    wtKST._socket.refresh();
                }
            }
        }
        catch (e) {

            console.error("[" + new Date().toISOString() + "][wtKST] Error while (re)starting websocket: " + e);
        }

        // restart procedure
        setTimeout(wtKST.start, 30000);
    },

    send : function(msg) {

        if ((wtKST._socket != null) && (wtKST._socket.readyState == WebSocket.OPEN)) {
            wtKST._socket.send(msg);
        }
        else {
            console.error("[" + new Date().toISOString() + "][wtKST] Connection not established!");
        }
    },

    _onOpen : function (event) {
        console.log("[" + new Date().toISOString() + "][wtKST] Connection established to: " + AirScout.Settings.KST_Websocket);
        wtKST._lastMsg = new Date();
    },

    _onMessage : function (event) {
        console.log("[" + new Date().toISOString() + "][wtKST] Data received: " + event.data);

        // store time of last msg
        wtKST._lastMsg = new Date();

        // interpret the message
        try {

            var msg = JSON.parse(event.data);
            wtKST._dispatchMessage(msg);
        }
        catch (e) {

            console.error("[" + new Date().toISOString() + "][wtKST] Error while receiving message: " + e);
        }
    },

    _onClose : function (event) {

        console.log("[" + new Date().toISOString() + "][wtKST] Connection closed.");
        if (wtKST._socket != null) {

            wtKST._socket.refresh();
        }
    },

    _onError: function (error) {

        console.error("[" + new Date().toISOString() + "][wtKST] Connection error!");
        if ((wtKST._socket != null) && (wtKST._socket == WebSocket.OPEN)) {

            wtKST._socket.refresh();
        }
    },

    _dispatchMessage : async function (msg) {

        if (msg.task == "test") {

            // connection check receveived
            await wtKST._dispatchTest(msg);
        }
        else if ((msg.task == "set_watchlist") && AirScout.Settings.Watchlist_SyncWithKST) {

            // set watchlist received
            await wtKST._dispatchSetWatchlist(msg);
        }
        else if (msg.task == "get_nearestplanes") {

            // get nearest planes received
            await wtKST._dispatchGetNearestPlanes(msg);
        }
        else if (msg.task == "set_path") {

            // set path
            await wtKST._dispatchSetPath(msg);
        }
    },

    // connection check
    _dispatchTest : function (msg) {

        var json = JSON.stringify({ task: "test", data: { aircraft_count: Aircrafts.count } });
        wtKST.send(json);
    },

    // set watchlist
    _dispatchSetWatchlist : function (msg) {

        // set all existing watchlist items to "remove" which are not currently checked
        AirScout.Settings.Watchlist.forEach(wlitem => { if (!wlitem.Checked) wlitem.Remove = true; });

        msg.data.forEach(item => {

            var existing = AirScout.Settings.Watchlist.find(wlitem => ((wlitem.call == item.call) && (wlitem.Loc == item.Loc)));

            if (existing != null) {

                // keep existing item
                existing.Remove = false;
            }
            else {

                // add new item
                var newitem = {
                    Call: item.call,
                    Loc: item.loc,
                    Checked: false,
                    Selected: false,
                    OutOfRange: false,
                    Remove: false
                }
                AirScout.Settings.Watchlist.push(newitem);
            }
        });

        // remove all existing watchlist items stiil on state "remove"
        AirScout.Settings.Watchlist = AirScout.Settings.Watchlist.filter(item => !item.Remove);
    },

    // get nearest planes
    _dispatchGetNearestPlanes : function (msg) {

        // ignore message if "to" is not matching
        if (msg.to != AirScout.Settings.Server_Name)
            return;

        var resp = {
            task: "get_nearestplanes",
            from: AirScout.Settings.Server_Name,
            to: msg.from,
            data: []
        };

        msg.data.forEach(item => {

            var myloc = AirScout.getLocation(msg.mycall, msg.myloc);
            if ((myloc == null) || (myloc.lat == null) || (myloc.lon == null)) {

                // create new location from locator if not in database
                var p = GeographicalPoint.fromLoc(msg.myloc);
                myloc = {
                    Elevation: 0,
                    BestCaseElevation: false,
                    Call: msg.mycall,
                    Loc: msg.myloc,
                    Lat: p.lat,
                    Lon: p.lon,
                    Source: GeoSource.fromLoc,
                    Hits: 0,
                    LastUpdated: new Date()
                }
            }

            var dxloc = AirScout.getLocation(item.call, item.loc);
            if ((dxloc == null) || (dxloc.lat == null) || (dxloc.lon == null)) {

                // create new location from locator if not in database
                var p = GeographicalPoint.fromLoc(item.loc);
                dxloc = {
                    Elevation: 0,
                    BestCaseElevation: false,
                    Call: item.call,
                    Loc: item.loc,
                    Lat: p.lat,
                    Lon: p.lon,
                    Source: GeoSource.fromLoc,
                    Hits: 0,
                    LastUpdated: new Date()
                }
            }

            var ppath = AirScout.getPropagationPath(msg.mycall, myloc.lat, myloc.lon, item.call, dxloc.lat, dxloc.lon, msg.qrg);

            if (ppath != null) {

                // clone aircraft list

                var aclist = Aircrafts.cloneAircraftList();

                // reset Nearest property and clear all intersection values at source data point
                Aircrafts.resetNearest(aclist);

                // set nearest planes
                // use "Auto" here for mac circumcircle to limit the amount of nearest planes
                var time = new Date();
                Aircrafts._setNearestPlanes(time, ppath, 0, AirScout.getBandSettings(AirScout.Settings.Band).MAXDISTANCE, AirScout.Settings.Planes_MaxAlt, aclist);

                // filter out nearest planes
                aclist = Object.values(aclist).filter(item => item.nearest == true);

                if (aclist.length > 0) {

                    var nearests = {
                        call: item.call,
                        loc: item.loc,
                        data: []
                    };

                    // create JSON objects
                    aclist.forEach(plane => {
                        if (plane.nearest) {

                            var int_min = Math.round((plane.intTime - new Date()) / 60000);
                            if ((int_min == null) || Number.isNaN(int_min)) {

                                console.error("[wtKST] Error in plane intersection calculation: ");
                                console.dir(plane);
                            }
                            else {

                                var nearest = {
                                    call: plane.call,
                                    cat: AircraftCategory.getShortStringValue(plane.category),
                                    int_qrb: ((plane.intDist == Number.MAX_VALUE) ? 0 : Math.round(plane.intDist)),
                                    int_potential: Math.round(plane.intPotential),
                                    int_min: int_min
                                }
                            }


                            nearests.data.push(nearest);
                        }
                    });

                    resp.data.push(nearests);
                }
            }
        });

        var json = JSON.stringify(resp, null, 4);
        wtKST.send(json);
        console.log("[" + new Date().toISOString() + "][wtKST] 'get_nearestplanes' response send: " + json.length + " chars.");

    },
    // set path
    _dispatchSetPath: function (msg) {

        // ignore message if "to" is not matching
        if (msg.to != AirScout.Settings.Server_Name)
            return;

        pause();

        pathMode = PathMode.SINGLE;

        $("#mycall").val(msg.mycall);
        $("#myloc").val(msg.myloc);
        $("#dxcall").val(msg.dxcall);
        $("#dxloc").val(msg.dxloc);

        checkInputs();

        play();

        Window.opener.focus();

    },


}



