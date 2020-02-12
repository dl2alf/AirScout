using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScout.PlaneFeeds.Plugin
{
    // Contains a set of raw aircraft information reported from the plugin
    // Information is 
    // At least MANDATORY fields are:
    // Timestamp: as the time the information was generated
    // Hex: as a unique aircraft identifier
    // all other fields are optional
    // Essential for a valid position message are Lat, Lon, Alt Speed, Track 
    // but they might be recovered/estimated by previous messages later on by the software
    public class PlaneFeedPluginPlaneInfo
    {
        // Timestamp of information 
        public DateTime Time { get; set; }                              = DateTime.MinValue;

        // 6digit - HEX-Identifier of aircraft. 
        public string Hex { get; set; }                                 = "";

        // Call sign. 
        public string Call { get; set; }                                = "";

        // Registration. 
        public string Reg { get; set; }                                 = "";

        // Latitude [-90..+90 deg]. 
        public double Lat { get; set; }                                 = double.MinValue;

        // Longitude [-180 ..+180 deg]. 
        public double Lon { get; set; }                                 = double.MinValue;

        // Altitude [0..100000 ft asl]. 
        public double Alt { get; set; }                                 = double.MinValue;

        // Track (Heading) [0..360 deg]. 
        public double Track { get; set; }                               = double.MinValue;

        // Speed [0..800 kts]. 
        public double Speed { get; set; }                               = double.MinValue;

        // Type identifier. 
        public string Type { get; set; }                                = "";

        // Manufacturer. 
        public string Manufacturer { get; set; }                        = "";

        // Model. 
        public string Model { get; set; }                               = "";

        // Country of aircraft is registered. 
        public string Country { get; set; }                             = "";

        // Category derived of aircraft's wake turbulence category. 
        // [0: unknwon]
        // [1: Light]
        // [2: Medium]
        // [3: Heavy]
        // [4: Superheavy]
        public int Category { get; set; }                               = 0;

        // Depearture airport. 
        public string From { get; set; }                                = "";

        // Destination airport. 
        public string To { get; set; }                                  = "";

        // Vertical speed [ft/min]. 
        public int VSpeed { get; set; }                                 = 0;

        // Indicates that aircraft is on ground
        public bool Ground { get; set; }                                = false;
    }

    // holds al list of aircraft info
    public class PlaneFeedPluginPlaneInfoList : List<PlaneFeedPluginPlaneInfo>
    {
    }
}
