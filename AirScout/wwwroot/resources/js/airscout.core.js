// JavaScript library for AirScout core functions

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// holds all AirScout specific functions
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
class AirScout {

    // gets the available bands
    static getBands(rooturl) {

        var bands = null;

        $.ajax({
            url: rooturl + "/bands.json",
            method: "GET",
            success: function (response) {
                bands = response;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            },
            async: false
        });

        return bands;
    }

    // gets a location 
    static getLocation(rooturl, call) {

        var location = null;

        $.ajax({
            url: rooturl + "/location.json?call=" + call.toUpperCase(),
            method: "GET",
            success: function (response) {
                location = response;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            },
            async: false
        });

        return location;

    }

    // gets a propagation path
    static getPropagationPath(rooturl, mycall, myloc, dxcall, dxloc) {

        var ppath = null

        $.ajax({
            url: rooturl + "/propagationpath.json?mycall=" + mycall + "&myloc=" + myloc + "&dxcall=" + dxcall + "&dxloc=" + dxloc,
            method: "GET",
            success: function (response) {
                ppath = response;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            },
            async: false
        });

        return ppath;
    }


    // gets the nearestplanes around a path
    static getNearestPlanes(rooturl, mycall, myloc, dxcall, dxloc, band) {

        var planes = null;

        $.ajax({
            url: rooturl + "/nearestplanes.json?mycall=" + mycall + "&myloc=" + myloc + "&dxcall=" + dxcall + "&dxloc=" + dxloc + "&band=1.2G",
            method: "GET",
            success: function (response) {
                planes = response;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            },
            async: false
        });

        return planes;
    }
}

