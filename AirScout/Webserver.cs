//    AirScout Aircraft Scatter Prediction
//    Copyright (C) DL2ALF
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Zeptomoby.OrbitTools;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Configuration;
using WinTest;
using System.Diagnostics;
using AquaControls;
using AirScout.Core;
using AirScout.Aircrafts;
using NDde;
using NDde.Server;
using NDde.Client;
using ScoutBase;
using ScoutBase.Core;
using ScoutBase.Elevation;
using ScoutBase.Stations;
using ScoutBase.Propagation;
using SerializableGenerics;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLiteDatabase;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Data.SQLite;

namespace AirScout
{
    public partial class MapDlg
    {
        #region Webserver

        private void bw_Webserver_DoWork(object sender, DoWorkEventArgs e)
        {
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = this.GetType().Name;

            string tmpdir = Application.StartupPath;
            // get temp directory from arguments
            if (e != null)
            {
                tmpdir = (string)e.Argument;
            }

            Log.WriteMessage("started.");
            // run simple web server
            string hosturl = "http://+:" + Properties.Settings.Default.Webserver_Port.ToString() + "/";
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            HttpListener listener = null;
            while (!bw_Webserver.CancellationPending)
            {
                string[] prefixes = new string[1];
                prefixes[0] = hosturl;
                int id = 0;
                try
                {
                    if (!HttpListener.IsSupported)
                    {
                        Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                        return;
                    }
                    // URI prefixes are required,
                    if (prefixes == null || prefixes.Length == 0)
                        throw new ArgumentException("prefixes");

                    // Create a listener.
                    listener = new HttpListener();
                    // Add the prefixes.
                    foreach (string s in prefixes)
                    {
                        listener.Prefixes.Add(s);
                    }
                    listener.Start();
                    Console.WriteLine("Listening...");
                    while (!bw_Webserver.CancellationPending)
                    {
                        // Note: The GetContext method blocks while waiting for a request. 
                        HttpListenerContext context = listener.GetContext();
                        List<PlaneInfo> allplanes = Planes.GetAll(DateTime.UtcNow, Properties.Settings.Default.Planes_Position_TTL);
                        id++;
                        // send response from a background thread
                        WebServerDelivererArgs args = new WebServerDelivererArgs();
                        args.ID = id;
                        args.TmpDirectory = tmpdir;
                        args.Context = context;
                        args.AllPlanes = allplanes;
                        WebserverDeliver bw = new WebserverDeliver();
                        bw.RunWorkerAsync(args);
                    }
                }
                catch (HttpListenerException ex)
                {
                    if (ex.ErrorCode == 5)
                    {
                        // gain additional access rights for that specific host url
                        // user will be prompted for allow changes
                        // DO NOT USE THE "listener=yes" option as recommended!!!
                        string args = "http add urlacl " + hosturl + " user=" + userName;
                        ProcessStartInfo psi = new ProcessStartInfo("netsh", args);
                        psi.Verb = "runas";
                        psi.CreateNoWindow = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = true;

                        Process.Start(psi).WaitForExit();
                    }
                    // do almost nothing
                    // wait 10 seconds and restart the listener
                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    // do almost nothing
                    // wait 10 seconds and restart the listener
                    Thread.Sleep(10000);
                }
                finally
                {
                }
            }
            if (listener != null)
            {
                lock (listener)
                {
                    listener.Stop();
                }
            }
            Log.WriteMessage("Finished.");
        }

        private void bw_Webserver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        #endregion

    }

    public class WebserverDeliver : BackgroundWorker
    {
        string TmpDirectory = Application.StartupPath;

        public short GetElevation(double lat, double lon)
        {
            if (!GeographicalPoint.Check(lat, lon))
                return 0;
            short elv = ElevationData.Database.ElvMissingFlag;
            // try to get elevation data from distinct elevation model
            // start with detailed one
            if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.SRTM1, false];
            if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.SRTM3, false];
            if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.GLOBE, false];
            // set it to zero if still invalid
            if (elv <= ElevationData.Database.TileMissingFlag)
                elv = 0;
            return elv;
        }

        public LocationDesignator LocationFind(string call, string loc = "")
        {
            // check all parameters
            if (!Callsign.Check(call))
                return null;
            if (!String.IsNullOrEmpty(loc) && !MaidenheadLocator.Check(loc))
                return null;
            // get location info
            LocationDesignator ld = (String.IsNullOrEmpty(loc)) ? StationData.Database.LocationFind(call) : StationData.Database.LocationFind(call, loc);
            // return null if not found
            if (ld == null)
                return null;
            // get elevation
            ld.Elevation = GetElevation(ld.Lat, ld.Lon);
            ld.BestCaseElevation = false;
            // modify location in case of best case elevation is selected --> but do not store in database or settings!
            if (Properties.Settings.Default.Path_BestCaseElevation)
            {
                if (!MaidenheadLocator.IsPrecise(ld.Lat, ld.Lon, 3))
                {
                    ElvMinMaxInfo maxinfo = GetMinMaxElevationLoc(ld.Loc);
                    if (maxinfo != null)
                    {
                        ld.Lat = maxinfo.MaxLat;
                        ld.Lon = maxinfo.MaxLon;
                        ld.Elevation = maxinfo.MaxElv;
                        ld.BestCaseElevation = true;
                    }
                }
            }
            return ld;
        }

        public List<LocationDesignator> LocationFindAll(string call)
        {
            // check all parameters
            if (!Callsign.Check(call))
                return null;
            // get location info
            List<LocationDesignator> l = StationData.Database.LocationFindAll(call);
            // return null if not found
            if (l == null)
                return null;
            foreach (LocationDesignator ld in l)
            {
                // get elevation
                ld.Elevation = GetElevation(ld.Lat, ld.Lon);
                ld.BestCaseElevation = false;
                // modify location in case of best case elevation is selected --> but do not store in database or settings!
                if (Properties.Settings.Default.Path_BestCaseElevation)
                {
                    if (!MaidenheadLocator.IsPrecise(ld.Lat, ld.Lon, 3))
                    {
                        ElvMinMaxInfo maxinfo = GetMinMaxElevationLoc(ld.Loc);
                        if (maxinfo != null)
                        {
                            ld.Lat = maxinfo.MaxLat;
                            ld.Lon = maxinfo.MaxLon;
                            ld.Elevation = maxinfo.MaxElv;
                            ld.BestCaseElevation = true;
                        }
                    }
                }
            }
            return l;
        }

        public ElvMinMaxInfo GetMinMaxElevationLoc(string loc)
        {
            ElvMinMaxInfo elv = new ElvMinMaxInfo();
            // try to get elevation data from distinct elevation model
            // start with detailed one
            if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.SRTM1, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.SRTM3, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.GLOBE, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            // set it to zero if still invalid
            if (elv.MaxElv == ElevationData.Database.ElvMissingFlag)
                elv.MaxElv = 0;
            if (elv.MinElv == ElevationData.Database.ElvMissingFlag)
                elv.MinElv = 0;

            return elv;
        }


        private string DeliverPlanes(string tmpdir)
        {
            string json = "";
            var fs = File.OpenRead(tmpdir + Path.DirectorySeparatorChar + "planes.json");
            using (StreamReader sr = new StreamReader(fs))
            {
                json = sr.ReadToEnd();
            }
            return json;
        }

        private string DeliverVersion(string paramstr)
        {
            string version = Application.ProductVersion;
            string json = "";
            // convert path to json
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json = JsonConvert.SerializeObject(version, settings);
            return json;
        }

        private string DeliverSettings(string paramstr)
        {
            //save settings
            Properties.Settings.Default.Save();
            string json = "";
            // convert path to json
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json = JsonConvert.SerializeObject(Properties.Settings.Default, settings);
            return json;
        }

        private string DeliverLocation(string paramstr)
        {
            string json = "";
            // set default values
            string callstr = "";
            string locstr = "";
            // get parameters
            try
            {
                if (paramstr.Contains("?"))
                {
                    // OK, we have parameters --> cut them out and make all uppercase
                    paramstr = paramstr.Substring(paramstr.IndexOf("?") + 1).ToUpper();
                    var pars = System.Web.HttpUtility.ParseQueryString(paramstr);
                    callstr = pars.Get("CALL");
                    locstr = pars.Get("LOC");
                }
            }
            catch (Exception ex)
            {
                // return error
                return "Error while parsing parameters!";
            }
            // check parameters
            if (!Callsign.Check(callstr))
                return "Error: " + callstr + " is not a valid callsign!";
            LocationDesignator ld = null;
            // locstr == null or empty --> return last recent location
            if (String.IsNullOrEmpty(locstr))
            {
                ld = LocationFind(callstr);
                if (ld == null)
                    return "Error: Location not found in database!";
                json = ld.ToJSON();
                return json;
            }
            if(locstr == "ALL")
            {
                List<LocationDesignator> l = LocationFindAll(callstr);
                if (l == null)
                    return "Error: Location not found in database!";
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.FloatFormatHandling = FloatFormatHandling.String;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                settings.Culture = CultureInfo.InvariantCulture;
                json = JsonConvert.SerializeObject(l, settings);
                return json;
            }
            if (!MaidenheadLocator.Check(locstr))
                return "Error: " + locstr + " is not a valid Maidenhead locator!";
            // search call in station database, return empty string if not found
            ld = LocationFind(callstr, locstr);
            if (ld == null)
                return "Error: Location not found in database!";
            json = ld.ToJSON();
            return json;
        }

        private string DeliverQRV(string paramstr)
        {
            string json = "";
            // set default values
            string callstr = "";
            string locstr = "";
            string bandstr = "";
            BAND band = Properties.Settings.Default.Band;
            // get parameters
            try
            {
                if (paramstr.Contains("?"))
                {
                    // OK, we have parameters --> cut them out and make all uppercase
                    paramstr = paramstr.Substring(paramstr.IndexOf("?") + 1).ToUpper();
                    var pars = System.Web.HttpUtility.ParseQueryString(paramstr);
                    callstr = pars.Get("CALL");
                    locstr = pars.Get("LOC");
                    bandstr = pars.Get("BAND");
                }
            }
            catch (Exception ex)
            {
                // return error
                return "Error while parsing parameters!";
            }
            // check parameters
            if (!Callsign.Check(callstr))
                return "Error: " + callstr + " is not a valid callsign!";
            if (!MaidenheadLocator.Check(locstr))
                return "Error: " + locstr + " is not a valid Maidenhead locator!";
            // set band to currently selected if empty
            if (string.IsNullOrEmpty(bandstr))
                band = Properties.Settings.Default.Band;
            else
                band = Bands.ParseStringValue(bandstr);
            if (band == BAND.BNONE)
                return "Error: " + bandstr + " is not a valid band value!";
            // search call in station database, return empty string if not found
            if (band != BAND.BALL)
            {
                QRVDesignator qrv = StationData.Database.QRVFind(callstr, locstr, band);
                if (qrv == null)
                    return "Error: QRV info not found in database!";
                json = qrv.ToJSON();
                return json;
            }
            List<QRVDesignator> qrvs = StationData.Database.QRVFind(callstr, locstr);
            if ((qrvs == null) || (qrvs.Count() == 0))
                return "Error: QRV info not found in database!";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json = JsonConvert.SerializeObject(qrvs, settings);
            return json;
        }

        private string DeliverElevationPath(string paramstr)
        {
            string json = "";
            // set default values
            string mycallstr = "";
            string mylocstr = "";
            string dxcallstr = "";
            string dxlocstr = "";
            BAND band = Properties.Settings.Default.Band;
            // get parameters
            try
            {
                if (paramstr.Contains("?"))
                {
                    // OK, we have parameters --> cut them out and make all uppercase
                    paramstr = paramstr.Substring(paramstr.IndexOf("?") + 1).ToUpper();
                    var pars = System.Web.HttpUtility.ParseQueryString(paramstr);
                    mycallstr = pars.Get("MYCALL");
                    mylocstr = pars.Get("MYLOC");
                    dxcallstr = pars.Get("DXCALL");
                    dxlocstr = pars.Get("DXLOC");
                }
            }
            catch (Exception ex)
            {
                // return error
                return "Error while parsing parameters!";
            }
            // check parameters
            if (!Callsign.Check(mycallstr))
                return "Error: " + mycallstr + " is not a valid callsign!";
            if (!Callsign.Check(dxcallstr))
                return "Error: " + dxcallstr + " is not a valid callsign!";
            if (!String.IsNullOrEmpty(mylocstr) && !MaidenheadLocator.Check(mylocstr))
                return "Error: " + mylocstr + " is not a valid Maidenhead locator!";
            if (!String.IsNullOrEmpty(dxlocstr) && !MaidenheadLocator.Check(dxlocstr))
                return "Error: " + dxlocstr + " is not a valid Maidenhead locator!";
            // search call in station database, return empty string if not found
            LocationDesignator myloc = LocationFind(mycallstr, mylocstr);
            if (myloc == null)
                return "Error: MyLocation not found in database!";
            LocationDesignator dxloc = LocationFind(dxcallstr, dxlocstr);
            if (dxloc == null)
                return "Error: DXLocation not found in database!";

            // get qrv info or create default
            QRVDesignator myqrv = StationData.Database.QRVFindOrCreateDefault(myloc.Call, myloc.Loc, band);
            // set qrv defaults if zero
            if (myqrv.AntennaHeight == 0)
                myqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(band);
            if (myqrv.AntennaGain == 0)
                myqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(band);
            if (myqrv.Power == 0)
                myqrv.Power = StationData.Database.QRVGetDefaultPower(band);

            // get qrv info or create default
            QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxloc.Call, dxloc.Loc, band);
            // set qrv defaults if zero
            if (dxqrv.AntennaHeight == 0)
                dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(band);
            if (dxqrv.AntennaGain == 0)
                dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(band);
            if (dxqrv.Power == 0)
                dxqrv.Power = StationData.Database.QRVGetDefaultPower(band);

            // get or calculate elevation path
            ElevationPathDesignator epath = ElevationData.Database.ElevationPathFindOrCreateFromLatLon(
                this,
                myloc.Lat,
                myloc.Lon,
                dxloc.Lat,
                dxloc.Lon,
                ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                Properties.Settings.Default.ElevationModel);
            if (epath == null)
                return json;
            // add additional info to ppath
            epath.Location1 = myloc;
            epath.Location2 = dxloc;
            epath.QRV1 = myqrv;
            epath.QRV2 = dxqrv;
            // convert path to json
            json = epath.ToJSON();
            return json;
        }

        private string DeliverPropagationPath(string paramstr)
        {
            string json = "";
            // set default values
            string mycallstr = "";
            string mylocstr = "";
            string dxcallstr = "";
            string dxlocstr = "";
            string bandstr = "";
            BAND band = Properties.Settings.Default.Band;
            // get parameters
            try
            {
                if (paramstr.Contains("?"))
                {
                    // OK, we have parameters --> cut them out and make all uppercase
                    paramstr = paramstr.Substring(paramstr.IndexOf("?") + 1).ToUpper();
                    var pars = System.Web.HttpUtility.ParseQueryString(paramstr);
                    mycallstr = pars.Get("MYCALL");
                    mylocstr = pars.Get("MYLOC");
                    dxcallstr = pars.Get("DXCALL");
                    dxlocstr = pars.Get("DXLOC");
                    bandstr = pars.Get("BAND");
                }
            }
            catch (Exception ex)
            {
                // return error
                return "Error while parsing parameters!";
            }
            // check parameters
            if (!Callsign.Check(mycallstr))
                return "Error: " + mycallstr + " is not a valid callsign!";
            if (!Callsign.Check(dxcallstr))
                return "Error: " + dxcallstr + " is not a valid callsign!";
            if (!String.IsNullOrEmpty(mylocstr) && !MaidenheadLocator.Check(mylocstr))
                return "Error: " + mylocstr + " is not a valid Maidenhead locator!";
            if (!String.IsNullOrEmpty(dxlocstr) && !MaidenheadLocator.Check(dxlocstr))
                return "Error: " + dxlocstr + " is not a valid Maidenhead locator!";
            // set band to currently selected if empty
            if (string.IsNullOrEmpty(bandstr))
                band = Properties.Settings.Default.Band;
            else
                band = Bands.ParseStringValue(bandstr);
            if (band == BAND.BNONE)
                return "Error: " + bandstr + " is not a valid band value!";
            // search call in station database, return empty string if not found
            LocationDesignator myloc = LocationFind(mycallstr, mylocstr);
            if (myloc == null)
                return "Error: MyLocation not found in database!";
            LocationDesignator dxloc = LocationFind(dxcallstr, dxlocstr);
            if (dxloc == null)
                return "Error: DXLocation not found in database!";

            // get qrv info or create default
            QRVDesignator myqrv = StationData.Database.QRVFindOrCreateDefault(myloc.Call, myloc.Loc, band);
            // set qrv defaults if zero
            if (myqrv.AntennaHeight == 0)
                myqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(band);
            if (myqrv.AntennaGain == 0)
                myqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(band);
            if (myqrv.Power == 0)
                myqrv.Power = StationData.Database.QRVGetDefaultPower(band);

            // get qrv info or create default
            QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxloc.Call, dxloc.Loc, band);
            // set qrv defaults if zero
            if (dxqrv.AntennaHeight == 0)
                dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(band);
            if (dxqrv.AntennaGain == 0)
                dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(band);
            if (dxqrv.Power == 0)
                dxqrv.Power = StationData.Database.QRVGetDefaultPower(band);

            // find local obstruction, if any
            LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(myloc.Lat, myloc.Lon, Properties.Settings.Default.ElevationModel);
            double mybearing = LatLon.Bearing(myloc.Lat, myloc.Lon, dxloc.Lat, dxloc.Lon);
            double myobstr = (o != null) ? o.GetObstruction(myqrv.AntennaHeight, mybearing) : double.MinValue;

            // get or calculate propagation path
            PropagationPathDesignator ppath = PropagationData.Database.PropagationPathFindOrCreateFromLatLon(
                this,
                myloc.Lat,
                myloc.Lon,
                GetElevation(myloc.Lat, myloc.Lon) + myqrv.AntennaHeight,
                dxloc.Lat,
                dxloc.Lon,
                GetElevation(dxloc.Lat, dxloc.Lon) + dxqrv.AntennaHeight,
                Bands.ToGHz(band),
                LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[band].K_Factor,
                Properties.Settings.Default.Path_Band_Settings[band].F1_Clearance,
                ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                Properties.Settings.Default.ElevationModel,
                myobstr);
            if (ppath == null)
                return json;
            // add additional info to ppath
            ppath.Location1 = myloc;
            ppath.Location2 = dxloc;
            ppath.QRV1 = myqrv;
            ppath.QRV2 = dxqrv;
            // convert path to json
            json = ppath.ToJSON();
            return json;
        }

        private string DeliverNearestPlanes(string paramstr, List<PlaneInfo> allplanes)
        {
            string json = "";
            // set default values
            string mycallstr = "";
            string mylocstr = "";
            string dxcallstr = "";
            string dxlocstr = "";
            string bandstr = "";
            BAND band = Properties.Settings.Default.Band;
            // get parameters
            try
            {
                if (paramstr.Contains("?"))
                {
                    // OK, we have parameters --> cut them out and make all uppercase
                    paramstr = paramstr.Substring(paramstr.IndexOf("?") + 1).ToUpper();
                    var pars = System.Web.HttpUtility.ParseQueryString(paramstr);
                    mycallstr = pars.Get("MYCALL");
                    mylocstr = pars.Get("MYLOC");
                    dxcallstr = pars.Get("DXCALL");
                    dxlocstr = pars.Get("DXLOC");
                    bandstr = pars.Get("BAND");
                }
            }
            catch (Exception ex)
            {
                // return error
                return "Error while parsing parameters!";
            }
            // check parameters
            if (!Callsign.Check(mycallstr))
                return "Error: " + mycallstr + " is not a valid callsign!";
            if (!Callsign.Check(dxcallstr))
                return "Error: " + dxcallstr + " is not a valid callsign!";
            if (!String.IsNullOrEmpty(mylocstr) && !MaidenheadLocator.Check(mylocstr))
                return "Error: " + mylocstr + " is not a valid Maidenhead locator!";
            if (!String.IsNullOrEmpty(dxlocstr) && !MaidenheadLocator.Check(dxlocstr))
                return "Error: " + dxlocstr + " is not a valid Maidenhead locator!";
            // set band to currently selected if empty
            if (string.IsNullOrEmpty(bandstr))
                band = Properties.Settings.Default.Band;
            else
                band = Bands.ParseStringValue(bandstr);
            if (band == BAND.BNONE)
                return "Error: " + bandstr + " is not a valid band value!";
            // search call in station database, return empty string if not found
            LocationDesignator myloc = LocationFind(mycallstr, mylocstr);
            if (myloc == null)
                return "Error: MyLocation not found in database!";
            LocationDesignator dxloc = LocationFind(dxcallstr, dxlocstr);
            if (dxloc == null)
                return "Error: DXLocation not found in database!";

            // get qrv info or create default
            QRVDesignator myqrv = StationData.Database.QRVFindOrCreateDefault(myloc.Call, myloc.Loc, band);
            // set qrv defaults if zero
            if (myqrv.AntennaHeight == 0)
                myqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(band);
            if (myqrv.AntennaGain == 0)
                myqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(band);
            if (myqrv.Power == 0)
                myqrv.Power = StationData.Database.QRVGetDefaultPower(band);

            // get qrv info or create default
            QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxloc.Call, dxloc.Loc, band);
            // set qrv defaults if zero
            if (dxqrv.AntennaHeight == 0)
                dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(band);
            if (dxqrv.AntennaGain == 0)
                dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(band);
            if (dxqrv.Power == 0)
                dxqrv.Power = StationData.Database.QRVGetDefaultPower(band);

            // find local obstruction, if any
            LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(myloc.Lat, myloc.Lon, Properties.Settings.Default.ElevationModel);
            double mybearing = LatLon.Bearing(myloc.Lat, myloc.Lon, dxloc.Lat, dxloc.Lon);
            double myobstr = (o != null) ? o.GetObstruction(myqrv.AntennaHeight, mybearing) : double.MinValue;

            // get or calculate propagation path
            PropagationPathDesignator ppath = PropagationData.Database.PropagationPathFindOrCreateFromLatLon(
                this,
                myloc.Lat,
                myloc.Lon,
                GetElevation(myloc.Lat, myloc.Lon) + myqrv.AntennaHeight,
                dxloc.Lat,
                dxloc.Lon,
                GetElevation(dxloc.Lat, dxloc.Lon) + dxqrv.AntennaHeight,
                Bands.ToGHz(band),
                LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[band].K_Factor,
                Properties.Settings.Default.Path_Band_Settings[band].F1_Clearance,
                ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                Properties.Settings.Default.ElevationModel,
                myobstr);
            if (ppath == null)
                return json;
            // add additional info to ppath
            ppath.Location1 = myloc;
            ppath.Location2 = dxloc;
            ppath.QRV1 = myqrv;
            ppath.QRV2 = dxqrv;
            /*
            // estimate positions according to time
            DateTime time = DateTime.UtcNow;
            foreach(PlaneInfo plane in allplanes.Planes)
            {
                // change speed to km/h
                double speed = plane.Speed_kmh;
                // calculate distance after timespan
                double dist = speed * (time - allplanes.At).TotalHours;
                LatLon.GPoint newpos = LatLon.DestinationPoint(plane.Lat, plane.Lon, plane.Track, dist);
                plane.Lat = newpos.Lat;
                plane.Lon = newpos.Lon;
                plane.Time = time;
            }
            */
            // get nearest planes
            List<PlaneInfo> nearestplanes = AircraftData.Database.GetNearestPlanes(DateTime.UtcNow, ppath, allplanes, Properties.Settings.Default.Planes_Filter_Max_Circumcircle, Properties.Settings.Default.Path_Band_Settings[band].MaxDistance, Properties.Settings.Default.Planes_MaxAlt);
            // convert nearestplanes to json
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json = JsonConvert.SerializeObject(nearestplanes, settings);
            return json;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            WebServerDelivererArgs args = (WebServerDelivererArgs)e.Argument;
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = this.GetType().Name + "_" + args.ID;
            HttpListenerRequest request = args.Context.Request;
            // Obtain a response object.
            HttpListenerResponse response = args.Context.Response;
            // Construct a default response.
            string responsestring = "<HTML><BODY> Welcome to AirScout!</BODY></HTML>";
            // check for content delivery request
            if (request.RawUrl.ToLower() == "/planes.json")
            {
                responsestring = DeliverPlanes(args.TmpDirectory);
            }
            else if (request.RawUrl.ToLower().StartsWith("/version.json"))
            {
                responsestring = DeliverVersion(request.RawUrl);
            }
            else if (request.RawUrl.ToLower().StartsWith("/settings.json"))
            {
                responsestring = DeliverSettings(request.RawUrl);
            }
            else if (request.RawUrl.ToLower().StartsWith("/location.json"))
            {
                responsestring = DeliverLocation(request.RawUrl);
            }
            else if (request.RawUrl.ToLower().StartsWith("/qrv.json"))
            {
                responsestring = DeliverQRV(request.RawUrl);
            }
            else if (request.RawUrl.ToLower().StartsWith("/elevationpath.json"))
            {
                responsestring = DeliverElevationPath(request.RawUrl);
            }
            else if (request.RawUrl.ToLower().StartsWith("/propagationpath.json"))
            {
                responsestring = DeliverPropagationPath(request.RawUrl);
            }
            else if (request.RawUrl.ToLower().StartsWith("/nearestplanes.json"))
            {
                responsestring = DeliverNearestPlanes(request.RawUrl, args.AllPlanes);
            }
            // copy bytes to buffer
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responsestring);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
//            Thread.Sleep(1000);
            // You must close the output stream.
            output.Close();
        }
    }

    public class WebServerDelivererArgs
    {
        public int ID;
        public string TmpDirectory = "";
        public HttpListenerContext Context;
        public List<PlaneInfo> AllPlanes;
    }
}
