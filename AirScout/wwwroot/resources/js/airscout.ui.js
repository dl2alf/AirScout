/**
 *
 * @source: airscout.ui.js
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
// JavaScript library for AirScout UI functions
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// holds all AirScout specific functions for user interface
////////////////////////////////////////////////////////////////////////////////

// holds the current mouse position in lat/long
var MousePosition = { lat: 0, lng: 0 };

// AirScout's map area
class MapArea {

    constructor(id, lat, lon, zoom) {

        // save id of container
        this.container = id;

        // create the base map with initial position and zoom
        this.map = L.map(id, {
            gestureHandling: true,
        }).setView([lat, lon], zoom);

        // disables the scroll wheel zoom
        //    this.map.scrollWheelZoom.disable();

        const resizeObserver = new ResizeObserver(() => {
            this.map.invalidateSize();
        });
        resizeObserver.observe(document.getElementById(id));

        this.map.on('mousemove', function (ev) {
            MousePosition = ev.latlng;
        });

        // add airport layer to map
        this.airports = L.layerGroup([]).addTo(this.map);
        this.airports.setZIndex(1);

        // add path layer to map
        this.paths = L.layerGroup([]).addTo(this.map);
        this.paths.setZIndex(10);

        // add tooltip layer to map
        this.tooltips = L.layerGroup([]).addTo(this.map);
        this.tooltips.setZIndex(30);

        // add planes layer to map
        this.planes = L.layerGroup([]).addTo(this.map);
        this.planes.setZIndex(20);

        // set up layer control
        this.baseMaps = {
        };

        this.overlayMaps = {
            "Airports": this.airports,
            "Paths": this.paths,
            "Planes": this.planes,
        };

        // create layer control
        this.layerControl = L.control.layers(this.baseMaps, this.overlayMaps, { position: 'bottomleft' }).addTo(this. map);

        // get all available map providers and add layers
        AirScout.Settings.Map_Providers.forEach(provider => {

            // replace API key if any is necessary and available
            var url = String.isNullOrEmpty(provider.APIKey) ? provider.URL : provider.URL.replace("{apikey}", provider.APIKey);

            // create tile layer
            var layer = L.tileLayer(url, {
                minZoom : provider.MinZoom,
                maxZoom: provider.MaxZoom,
                attribution: provider.Attribution
            });

            // add layer to layer control
            this.layerControl.addBaseLayer(layer, provider.Name);

            // select layer by settings
            if (provider.Name == AirScout.Settings.Map_Provider) {
                layer.addTo(this.map);
            }

        });

        /*
        var cont = this.map.getContainer();
        cont.style.filter = "saturate(30%)";
        */
    }

    // define clearPaths method
    clearAirports = function () {
        if (this.airports == null)
            return;

        this.airports.clearLayers();
    }

    // define clearPaths method
    clearPaths = function () {
        if (this.paths == null)
            return;

        this.paths.clearLayers();
    }

    // define clearPlanes method
    clearPlanes = function () {

        // remove all map objects & selections from aircraft infos
        Aircrafts.getAircraftList().forEach(val => {
            val.sprite = null;
            val.tooltip = null;
            val.ttIcon = null;
            val.ttLine = null;

            val.selected = false;
            val.mouseOver = false;
        });

        // clear layers
        if (this.planes != null) this.planes.clearLayers();
        if (this.tooltips != null) this.tooltips.clearLayers();

    }

    getElementHeight = function(el) {
        var clone = el.cloneNode(true);
        clone.className = "leaflet-plane-tooltip-gray";
        var width = el.getBoundingClientRect().width;
        clone.style.cssText = 'position: fixed; top: -10000; left: -10000; overflow: hidden; display: block; pointer-events: none; height: 0; max-height: unset; width: ' + width + 'px';
        document.body.append(clone);
//        var height = clone.getBoundingClientRect().height;
        var height = clone.scrollHeight;
        clone.remove();
        return height;
    }

    // define draw airports function
    drawAirports = function () {

        // do nothing if layer is not visible
        if ((this.airports == null) || !this.map.hasLayer(this.airports))
            return;

        try {

            // clear airports
            this.clearAirports();

            // create new icon according to category
            var size = [16, 16];
            var icon = new L.Icon({
                iconUrl: "/resources/icons/markers/airport.png",
                iconSize: size,
                iconAnchor: [size[0] / 2, size[1] / 2],
            });

            // draw airports
            Object.values(AirScout.Airports).forEach(airport => {

                if ((airport.lat >= AirScout.Settings.MinLat) && (airport.lat <= AirScout.Settings.MaxLat) && (airport.lon >= AirScout.Settings.MinLon) && (airport.lon <= AirScout.Settings.MaxLon)) {
                    L.marker([airport.lat, airport.lon], { icon: icon }).addTo(this.airports);
                }
            });
        }
        catch (e) {
            console.error(e.message);
        }

    }

    // define drawPath method
    drawPaths = function(ppaths) {

        // do nothing if layer is not visible
        if ((this.paths == null) || !this.map.hasLayer(this.paths))
            return;

        // do nothing if no paths available
        if ((ppaths == null) || (ppaths.length == 0))
            return;

        // calculate the area to show in map
        // initially set to my location
        var minlat = AirScout.Settings.MyLat;
        var minlon = AirScout.Settings.MyLon;
        var maxlat = AirScout.Settings.MyLat;
        var maxlon = AirScout.Settings.MyLon;

        var centerlat = AirScout.Settings.MyLat;
        var centerlon = AirScout.Settings.MyLon;


        ppaths.forEach(ppath => {

            try {

                // get path midpoint
                var midpoint = LatLon.midPoint(ppath.lat1, ppath.lon1, ppath.lat2, ppath.lon2);

                // create leaflet icons
                if (!AirScout.Settings.Map_SmallMarkers) {

                    var iconurl = '/resources/icons/markers/xxx_small.png';
                    var shadowurl = '/resources/icons/markers/shadow_small.png';
                    var iconsize = [12, 20];
                    var iconanchor = [6, 20];
                    var popupanchor = [1, -4];
                    var shadowsize = [20, 20];
                    var shadowanchor = [6, 20];

                }
                else {
                    var iconurl = "/resources/icons/markers/xxx-dot.png";
                    var shadowurl = "/resources/icons/markers/shadow-dot.png";
                    var iconsize = [32, 32];
                    var iconanchor = [16, 32];
                    var popupanchor = [1, -24];
                    var shadowsize = [64, 32];
                    var shadowanchor = [20, 32];

                }
                var redIcon = new L.Icon({
                    iconUrl: iconurl.replace("xxx", "red"),
                    shadowUrl: shadowurl,
                    iconSize: iconsize,
                    iconAnchor: iconanchor,
                    popupAnchor: popupanchor,
                    shadowSize: shadowsize,
                    shadowAnchor: shadowanchor,
                });
                var yellowIcon = new L.Icon({
                    iconUrl: iconurl.replace("xxx", "yellow"),
                    shadowUrl: shadowurl,
                    iconSize: iconsize,
                    iconAnchor: iconanchor,
                    popupAnchor: popupanchor,
                    shadowSize: shadowsize,
                    shadowAnchor: shadowanchor,
                });
                var blueIcon = new L.Icon({
                    iconUrl: iconurl.replace("xxx", "blue"),
                    shadowUrl: shadowurl,
                    iconSize: iconsize,
                    iconAnchor: iconanchor,
                    popupAnchor: popupanchor,
                    shadowSize: shadowsize,
                    shadowAnchor: shadowanchor,
                });

                // create leaflet markers
                var mymarker = L.marker([ppath.lat1, ppath.lon1], { icon: redIcon }).addTo(this.paths);
                mymarker.bindTooltip(ppath.qrv1.call);
                var dxmarker = L.marker([ppath.lat2, ppath.lon2], { icon: yellowIcon }).addTo(this.paths);
                dxmarker.bindTooltip(ppath.qrv2.call);
                var midmarker = L.marker([midpoint.lat, midpoint.lon], { icon: blueIcon }).addTo(this.paths);
                dxmarker.bindTooltip(ppath.qrv2.call);

                // get 1km-waypoints from from path
                var infopoints = ppath.infoPoints;

                // create path
                var ppoints = [];
                for (var i = 0; i < infopoints.length; i++) {

                    ppoints.push(new Array(infopoints[i].lat, infopoints[i].lon));
                }
                L.polyline(ppoints, { color: "black" }).addTo(this.paths);

                // create visible path
                var vpoints = [];
                for (var i = 0; i < infopoints.length; i++) {

                    if ((Math.max(infopoints[i].h1, infopoints[i].h2) > 0) && (Math.max(infopoints[i].h1, infopoints[i].h2) < 12000)) {
                        vpoints.push(new Array(infopoints[i].lat, infopoints[i].lon));
                    }
                }
                L.polyline(vpoints, { color: "magenta" }).addTo(this.paths);

                // maintain Min/Max values
                minlat = Math.min(minlat, ppath.lat2);
                minlon = Math.min(minlon, ppath.lon2);
                maxlat = Math.max(maxlat, ppath.lat2);
                maxlon = Math.max(maxlon, ppath.lon2);
            }
            catch (e) {
                console.error(e);
            }
        });

        // zoom map to fit rect
        this.map.fitBounds([[minlat, minlon], [maxlat, maxlon]]);
    }

    drawPlanes = function() {

        // do nothing if layer is not visible
        if ((this.planes == null) || !this.map.hasLayer(this.planes))
            return;

        // update time
        var now = new Date();
        var today = now.getUTCHours().toString().padStart(2, '0') + ":" +
            now.getUTCMinutes().toString().padStart(2, '0') + ":" +
            now.getUTCSeconds().toString().padStart(2, '0');
        $("#utc").text(today);

        // center map if auto center = true
        if (AirScout.Settings.Map_AutoCenter) {

            // get midpoint
            var midpoint = LatLon.midPoint(AirScout.Settings.MyLat, AirScout.Settings.MyLon, AirScout.Settings.DXLat, AirScout.Settings.DXLon);
            this.map.setView([midpoint.lat, midpoint.lon]);
        }

        // get map visible bounds
        var bounds = this.map.getBounds();

        // draw planes
        Aircrafts.getAircraftList().forEach(val => {
            try {

                // remove map objects and skip, if 
                // out of range
                // out of visible bounds
                // filtered by category
                if (!val.nearest || !bounds.contains([val.lat, val.lon]) || (val.category < AirScout.Settings.Planes_Filter_Min_Category)) {

                    if (val.sprite != null) {
                        this.map.removeLayer(val.sprite);
                        val.sprite = null;
                    }

                    if (val.tooltip != null) {
                        this.map.removeLayer(val.tooltip);
                        val.tooltip = null;
                    }

                    if (val.ttIcon != null) {
                        this.map.removeLayer(val.ttIcon);
                        val.ttIcon = null;
                    }

                    if (val.ttLine != null) {
                        this.map.removeLayer(val.ttLine);
                        val.ttLine = null;
                    }

                    if (val.intLine != null) {
                        this.map.removeLayer(val.intLine);
                        val.intLine = null;
                    }

                    return;
                }

                // toltip offset
                const ttAnchor = [-40, 40];

                // create all variables
                var color =
                    (val.intPotential == 100) ? "magenta" :
                        (val.intPotential == 75) ? "red" :
                            (val.intPotential == 50) ? "darkorange" : "gray";
                var size =
                    (val.category == AircraftCategory.SUPERHEAVY) ? [48, 48] :
                        (val.category == AircraftCategory.HEAVY) ? [32, 32] :
                            (val.category == AircraftCategory.MEDIUM) ? [24, 24] :
                                (val.category == AircraftCategory.LIGHT) ? [16, 16] : [24, 24];
                var cat =
                    (val.category == AircraftCategory.SUPERHEAVY) ? "S" :
                        (val.category == AircraftCategory.HEAVY) ? "H" :
                            (val.category == AircraftCategory.MEDIUM) ? "M" :
                                (val.category == AircraftCategory.LIGHT) ? "L" : "N";

                // create show tooltip, if hovered or selected
                if (val.mouseOver || val.selected) {

                    // create tooltip text
                    if ((val.intPoint != null) && (val.intPotential > 0)) {

                        // get all intersection values and show detailed information
                        var lat = (lat >= 0) ? val.lat.toFixed(2) + "°N" : val.lat.toFixed(2) + "°S";
                        var lon = (lon >= 0) ? val.lon.toFixed(2) + "°E" : val.lon.toFixed(2) + "°W";
                        var alt = AirScout.Settings.InfoWin_Metric ? val.alt.toFixed(0) + "m [" + val.intAltDiff.toFixed(0) + "m]" : UnitConverter.m_ft(val.alt) + "ft [" + UnitConverter.m_ft(val.intAltDiff).toFixed(0) + "m]";
                        var track = val.track.toFixed(0) + "°";
                        var track = val.track.toFixed(0) + "°";
                        var speed = AirScout.Settings.InfoWin_Metric ? val.speed.toFixed(0) + "km/h" : UnitConverter.kmh_kts(val.alt) + "kts";
                        var dist = AirScout.Settings.InfoWin_Metric ? val.intDist.toFixed(0) + "km" : UnitConverter.km_mi(val.intDist) + "mi";
                        var time = val.intTime.toTimeString("hh:mm").substring(0, 5) + " [" + ((val.intTime - new Date()) / 60000).toFixed(0) + "min]";
                        var intangle = (val.intAngle / Math.PI * 180.0).toFixed(0) + "°";
                        var eps = (val.eps1 / Math.PI * 180.0).toFixed(2) + "° <> " + (val.eps2 / Math.PI * 180.0).toFixed(2) + "°";
                        var squint = (val.squint / Math.PI * 180).toFixed(2) + "°";
                        var tooltip = val.call +
                            "<br>-----------" +
                            (AirScout.Settings.InfoWin_Position ? "<br>Pos: " + lat + " , " + lon : "") +
                            (AirScout.Settings.InfoWin_Alt ? "<br>Alt: " + alt : "") +
                            (AirScout.Settings.InfoWin_Track ? "<br>Track: " + track : "") +
                            (AirScout.Settings.InfoWin_Speed ? "<br>Speed: " + speed : "") +
                            (AirScout.Settings.InfoWin_Type ? "<br>Type: " + val.model + "[" + cat + "]" : "") +
                            (AirScout.Settings.InfoWin_Dist ? "<br>Dist: " + dist : "") +
                            (AirScout.Settings.InfoWin_Time ? "<br>Time: " + time : "") +
                            (AirScout.Settings.InfoWin_Angle ? "<br>Angle: " + intangle : "") +
                            (AirScout.Settings.InfoWin_Epsilon ? "<br>Eps: " + eps : "") +
                            (AirScout.Settings.InfoWin_Squint ? "<br>Squint: " + squint : "");
                    }
                    else {

                        // show minimum information
                        var lat = (lat >= 0) ? val.lat.toFixed(2) + "°N" : val.lat.toFixed(2) + "°S";
                        var lon = (lon >= 0) ? val.lon.toFixed(2) + "°E" : val.lon.toFixed(2) + "°W";
                        var alt = AirScout.Settings.InfoWin_Metric ? val.alt.toFixed(0) + "m" : UnitConverter.m_ft(val.alt) + "ft";
                        var track = val.track.toFixed(0) + "°";
                        var speed = AirScout.Settings.InfoWin_Metric ? val.speed.toFixed(0) + "km/h" : UnitConverter.kmh_kts(val.alt) + "kts";

                        var tooltip = val.call +
                            "<br>-----------" +
                            (AirScout.Settings.InfoWin_Position ? "<br>Pos: " + lat + " , " + lon : "") +
                            (AirScout.Settings.InfoWin_Alt ? "<br>Alt: " + alt : "") +
                            (AirScout.Settings.InfoWin_Track ? "<br>Track: " + track : "") +
                            (AirScout.Settings.InfoWin_Speed ? "<br>Speed: " + speed : "") +
                            (AirScout.Settings.InfoWin_Type ? "<br>Type: " + val.model + "[" + cat + "]" : "");
                    }

                    // create new tooltip marker, if necessary
                    if (val.tooltip == null) {

                        // create tooltip icon
                        var tticon = L.divIcon(
                            {
                                className: "leaflet-plane-tooltip-" + color,
                                html: "<center>" + tooltip + "</center>",
                                iconSize: null,
                                iconAnchor: [0, 0]
                            });


                        var ttmarker = L.marker([val.lat, val.lon], {
                            icon: tticon
                        }).addTo(this.tooltips);

                        ttmarker._icon.id = "tt" + val.hex;

                        val.tooltip = ttmarker;
                        val.ttIcon = tticon;


                    }
                    val.ttIcon.options.className = "leaflet-plane-tooltip-" + color;

                    // show/update tooltip
                    var ttheight = this.getElementHeight(val.tooltip._icon);
                    val.ttIcon.options.iconAnchor[0] = ttAnchor[0];
                    val.ttIcon.options.iconAnchor[1] = ttAnchor[1] + ttheight;
                    val.tooltip.setIcon(val.ttIcon);
                    val.tooltip._icon.innerHTML = "<center>" + tooltip + "</center>";

                    // (re-)create tooltip line
                    if (val.ttLine != null) {
                        map.map.removeLayer(val.ttLine);
                    }

                    var ttpoints = Array();
                    ttpoints.push(val.sprite.getLatLng());
                    var x = val.tooltip._icon._leaflet_pos.x - ttAnchor[0];
                    var y = val.tooltip._icon._leaflet_pos.y - ttAnchor[1];
                    ttpoints.push(map.map.layerPointToLatLng({ x: x, y: y }));
                    var linecolor = "rgba(0, 0, 76, 0.55)";

                    var ttline = new L.Polyline(ttpoints, {
                        color: linecolor,
                        weight: 2,
                    }).addTo(map.tooltips);

                    val.ttLine = ttline;

                    // (re-)create intersection line
                    if (val.intLine != null) {
                        map.map.removeLayer(val.intLine);
                    }

                    if (val.intPoint != null) {
                        var intpoints = Array();
                        intpoints.push(val.sprite.getLatLng());
                        intpoints.push([val.intPoint.lat, val.intPoint.lon]);

                        var intline = new L.Polyline(intpoints, {
                            color: 'black',
                            weight: 1,
                        }).addTo(map.tooltips);

                        val.intLine = intline;
                    }

                    // update position and icon
                    val.tooltip.setLatLng({ lat: val.lat, lng: val.lon });

                }
                else {

                    // delete intersection line
                    if (val.intLine != null) {
                        this.map.removeLayer(val.intLine);
                        val.intLine = null;
                    }

                    // delete tooltip line
                    if (val.ttLine != null) {
                        this.map.removeLayer(val.ttLine);
                        val.ttLine = null;
                    }

                    // delete icon
                    if (val.ttIcon != null) {
                        this.map.removeLayer(val.ttIcon);
                        val.ttIcon = null;
                    }

                    // delete marker
                    if (val.tooltip != null) {
                        this.map.removeLayer(val.tooltip);
                        val.tooltip = null;
                    }
                }

             

                // create new icon according to category
                var spricon = new L.Icon({
                    iconUrl: "/resources/icons/aircrafts/" + color + ".png",
                    iconSize: size,
                    iconAnchor: [size[0] / 2, size[1] / 2],
                });

                // create new marker, if necessarey
                if (val.sprite == null) {

                    var sprmarker = L.marker([val.lat, val.lon], { icon: spricon, rotationAngle: val.track }).addTo(this.planes);

                    sprmarker._icon.id = val.hex;

                    sprmarker.on('mouseover', function (ev) {
                        val.mouseOver = true;
                    });

                    sprmarker.on('mouseout', function (ev) {
                        val.mouseOver = false;
                    });

                    sprmarker.on('click', function (ev) {
                        val.selected = !val.selected;
                    });

                    val.sprite = sprmarker;
                }
                else {

                    // update position, icon and rotation angle
                    val.sprite.setLatLng({ lat: val.lat, lng: val.lon });
                    if (val.sprite.options.icon.options.iconUrl != spricon.options.iconUrl) {
                        val.sprite.setIcon(spricon);
                    }
                    if (val.sprite.options.rotationAngle != val.track) {
                        val.sprite.setRotationAngle(val.track);
                    }

                    // set icon anchor again to place it at proper position, this seems to be a bug in leaflet.js
                    spricon.options.iconAnchor[0] = spricon.options.iconSize[0] / 2;
                    spricon.options.iconAnchor[1] = spricon.options.iconSize[1] / 2;
                }

            }
            catch (e) {
                console.error(e.message);
            }
        });
    }
}

// AirScout's Chart area
function ChartArea(idPath, idElev, maxalt) {

    // callsigns
    this.call1 = "";
    this.call2 = "";

    // path chart
    this.path = new Chart(document.getElementById(idPath), {
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
            },
            scales: {
                x: {
                    type: "linear",
                    grid: {
                        display: true,
                        borderDash: function (context) {

                            // show grid lines each 50kms
                            if (context.tick.value % 50 == 0) {
                                return undefined;
                            } else {
                                return [1, 1];
                            }
                        },
                    },
                    ticks: {
                        stepSize: 10,
                        autoSkip: false,
                        font: {
                            family: 'Arial',
                            size: 8,
                        },
                        maxRotation: 90,
                        minRotation: 90,
                        callback: function (value, index, ticks) {

                            // show ticks each 50kms
                            if (value % 50 == 0)
                                return value;
                            return "";
                        },
                    },
                },
                y: {
                    beginAtZero: true,
                    min: 0,
                    max: 20000,
                    grid: {
                        display: true,
                        borderDash: [1, 1]
                    },
                    ticks: {
                        font: {
                            family: 'Arial',
                            size: 8,
                        },
                        major: {
                            enabled: true,
                        },
                    },
                    min: 0,
                }
            }
        },
        plugins: [
            {
                afterDraw: chart => {

                    // draw callsigns on top of chart
                    let ctx = chart.ctx;
                    let x1 = chart.chartArea.left;
                    let x2 = -chart.chartArea.right + 130;
                    let y = chart.chartArea.height * maxalt / 20000;
                    ctx.save();
                    ctx.textAlign = 'center';
                    ctx.fillStyle = 'black';
                    ctx.font = "bold 14px 'Arial', Arial, Arial, sans-serif";
                    ctx.translate(100, y);
                    ctx.rotate(Math.PI / 2);
                    ctx.fillText(this.call1, 0, x1);
                    ctx.fillText(this.call2, 0, x2);
                    ctx.restore();

                },
            }
        ],

        data: {
            datasets: [
                {
                    // elevation
                    type: "line",
                    label: "",
                    lineTension: 0,
                    data: [],
                    borderColor: '#4e9a05',
                    backgroundColor: '#4e9a05',
                    fill: true,
                    pointRadius: 0,
                    order: 6,
                },
                {
                    // eps_min1
                    type: "line",
                    label: "",
                    lineTension: 0,
                    data: [],
                    borderColor: "red",
                    fill: false,
                    pointRadius: 0,
                    order: 5,
                },
                {
                    // eps_min2
                    type: "line",
                    label: "",
                    lineTension: 0,
                    data: [],
                    borderColor: "yellow",
                    fill: false,
                    pointRadius: 0,
                    order: 4,
                },
                {
                    // planes_maxalt
                    type: "line",
                    label: "",
                    lineTension: 0,
                    data: [],
                    borderColor: "blue",
                    backgroundColor: "rgba(255, 0, 255, 0.25)",
                    fill: "+1",
                    pointRadius: 0,
                    borderDash: [2, 4],
                    borderWidth: 2,
                    order: 3,
                },
                {
                    // fill hot area
                    type: "line",
                    label: "",
                    lineTension: 0,
                    data: [],
                    borderColor: "magenta",
                    fill: false,
                    pointRadius: 0,
                    borderDash: [2, 4],
                    order: 2,
               },
                {
                    // planes_lo
                    type: "scatter",
                    label: "",
                    lineTension: 0,
                    data: [],
                    borderColor: "gray",
                    borderWidth: 4,
                    fill: true,
                    pointStyle: "rect",
                    pointRadius: 1,
                    order: 1,
                },
                {
                    //planes_hi
                    type: "scatter",
                    label: "",
                    lineTension: 0,
                    data: [],
                    borderColor: "magenta",
                    borderWidth: 4,
                    fill: true,
                    pointStyle: "rect",
                    pointRadius: 1,
                    order: 0,
                },
            ],
            labels: []
        },
    });

    // define drawPath method
    this.drawPath = function (epath, ppath) {

        var epoints = epath.infoPoints;
        var ppoints = ppath.infoPoints;

        for (var i = 0; i < epoints.length; i++) {
            this.path.data.labels.push(i);
            this.path.data.datasets[0].data[i] = epoints[i];
            this.path.data.datasets[1].data[i] = ppoints[i].h1;
            this.path.data.datasets[2].data[i] = ppoints[i].h2;
            this.path.data.datasets[3].data[i] = maxalt;
            this.path.data.datasets[4].data[i] = Math.min(maxalt, Math.max(ppoints[i].h1, ppoints[i].h2));
        }

        this.call1 = ppath.qrv1.call;
        this.call2 = ppath.qrv2.call;

        this.path.update();
    }

    // elevation chart
    this.elev = new Chart(document.getElementById(idElev), {
        type: 'line',
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
            },
            scales: {
                x: {
                    type: "linear",
                    grid: {
                        display: true,
                        borderDash: function (context) {
                            if (context.tick.value % 50 == 0) {
                                return undefined;
                            } else {
                                return [1, 1];
                            }
                        },
                    },
                    ticks: {
                        stepSize: 10,
                        autoSkip: false,
                        font: {
                            family: 'Arial',
                            size: 8,
                        },
                        maxRotation: 90,
                        minRotation: 90,
                        callback: function (value, index, ticks) {
                            if (value % 50 == 0)
                                return value;
                            return "";
                        },
                    },
                },
                y: {
                    beginAtZero: true,
                    min: 0,
                    grid: {
                        display: true,
                        borderDash: [1, 1]
                    },
                    ticks: {
                        font: {
                            family: 'Arial',
                            size: 8,
                        },
                        major: {
                            enabled: true,
                        },
                    },
                    min: 0,
                }
            }
        },
        data: {
            datasets: [{
                label: "",
                lineTension: 0,
                data: [],
                borderColor: '#4e9a05',
                backgroundColor: 'rgba(78, 154, 5, 0.5)',
                fill: true,
                pointRadius: 0,
            }],
            labels: []
        },
    });

    // define drawElev method
    this.drawElev = function (epath) {

        var epoints = epath.infoPoints;
        for (var i = 0; i < epoints.length; i++) {
            this.elev.data.labels.push(i);
            this.elev.data.datasets[0].data[i] = epoints[i];
        }

        this.elev.update();
    }

    // define clearPlanes method
    this.clearPlanes = function () {

        this.path.data.datasets[5].data = [];
        this.path.data.datasets[6].data = [];
    }


    // define drawPlanes method
    this.drawPlanes = function (planes) {

        this.clearPlanes();

        // draw planes
        Aircrafts.getAircraftList().forEach(val => {
            try {

                if ((val.intPoint != null) && (val.intDist <= AirScout.getBandSettings(AirScout.Settings.Band).MAXDISTANCE)) {

                    // calculate distance from mylat/mylon
                    var dist = LatLon.distance(AirScout.Settings.MyLat, AirScout.Settings.MyLon, val.intPoint.lat, val.intPoint.lon);

                    // add point to dataset
                    var p = { x: dist, y: val.alt };

                    if (val.intAltDiff <= 0) {
                        this.path.data.datasets[5].data.push(p);
                    }
                    else {
                        this.path.data.datasets[6].data.push(p);
                    }


                }
            }
            catch (e) {
                console.error(e.message);
            }
        });

        this.path.update();
    }


}


