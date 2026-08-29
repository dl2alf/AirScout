using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.ComponentModel.Composition;
using System.Globalization;
using AirScout.PlaneFeeds.Plugin.MEFContract;

namespace AirScout.PlaneFeeds.Plugin.PublicBinCraft
{
    public class PublicBinCraftSettings
    {
        [Browsable(false)]
        [DefaultValue("")]
        [XmlIgnore]
        public string DisclaimerAccepted { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Host of a readsb-compatible server, e.g. https://globe.airplanes.live or https://adsb.lol, or a self-hosted readsb/tar1090 server. The /re-api/?binCraft&zstd&box=... path is fixed and appended automatically.")]
        [DefaultValue("https://globe.airplanes.live")]
        public string Host { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("User-Agent header sent with the request.")]
        [DefaultValue("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/151.0.0.0 Safari/537.36")]
        public string UserAgent { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Response is zstd-compressed (the '&zstd' query parameter).")]
        [DefaultValue(true)]
        public bool Compressed { get; set; }

        public PublicBinCraftSettings()
        {
            Default();
            Load(true);
        }

        public void Default()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                try
                {
                    if (Attribute.IsDefined(p, typeof(DefaultValueAttribute)))
                    {
                        p.SetValue(this, ((DefaultValueAttribute)Attribute.GetCustomAttribute(
                            p, typeof(DefaultValueAttribute)))?.Value, null);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[" + this.GetType().Name + "]: Cannot set default value of: " + p.Name + ", " + ex.Message);
                }
            }
        }

        public void Load(bool loadall, string filename = "")
        {
            if (String.IsNullOrEmpty(filename))
                filename = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(".dll", ".cfg").Replace(".DLL", ".CFG")).LocalPath;

            if (!File.Exists(filename))
                return;

            try
            {
                string xml;
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    xml = sr.ReadToEnd();
                }
                XDocument xdoc = XDocument.Parse(xml);
                foreach (PropertyInfo p in this.GetType().GetProperties())
                {
                    if (!loadall && p.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length > 0)
                        continue;
                    try
                    {
                        XElement typenode = xdoc.Element(this.GetType().Name);
                        XElement element = typenode?.Element(p.Name);
                        if (element != null)
                            p.SetValue(this, Convert.ChangeType(element.Value, p.PropertyType), null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[" + this.GetType().Name + "]: Error while loading property[" + p.Name + " from " + filename + ", " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + this.GetType().Name + "]: Cannot load settings from " + filename + ", " + ex.Message);
            }
        }

        public void Save(bool saveall, string filename = "")
        {
            if (String.IsNullOrEmpty(filename))
                filename = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(".dll", ".cfg").Replace(".DLL", ".CFG")).LocalPath;

            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            if (saveall)
            {
                foreach (PropertyInfo p in this.GetType().GetProperties())
                {
                    overrides.Add(this.GetType(), p.Name, new XmlAttributes { XmlIgnore = false });
                }
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Create(filename)))
                {
                    XmlSerializer s = new XmlSerializer(this.GetType(), overrides);
                    s.Serialize(sw, this);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot save settings to " + filename + ", " + ex.Message);
            }
        }
    }

    [Export(typeof(IPlaneFeedPlugin))]
    [ExportMetadata("Name", "PlaneFeedPlugin")]
    public class PublicBinCraftPlugin : IPlaneFeedPlugin
    {
        private PublicBinCraftSettings Settings = new PublicBinCraftSettings();

        // Public re-api deployments (adsb.lol, airplanes.live, ...) pin requests to a backend
        // worker via a "sticky" session cookie (Set-Cookie'd on the first response). Keeping
        // this across polling calls mirrors the browser instead of re-negotiating a new backend
        // on every request.
        private readonly CookieContainer Cookies = new CookieContainer();

        public string Name => "[WebFeed]           Public BinCraft (readsb re-api)";

        public string Info =>
            "Web feed for any readsb/tar1090 server exposing the binCraft\n" +
            "binary /re-api/ endpoint - e.g. adsb.lol, globe.airplanes.live,\n" +
            "or a self-hosted readsb instance.\n\n" +
            "Every such host publishes the same path, so only the host\n" +
            "itself needs configuring: /re-api/?binCraft&zstd&box=... is\n" +
            "fixed and appended automatically, with the box filled in from\n" +
            "the current view. The Referer header is derived automatically\n" +
            "from the configured host too.";

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public bool HasSettings => true;

        public bool CanImport => true;

        public bool CanExport => true;

        public string Disclaimer => "";

        public string DisclaimerAccepted
        {
            get => Settings.DisclaimerAccepted;
            set => Settings.DisclaimerAccepted = value;
        }

        public void ResetSettings()
        {
            Settings.Default();
        }

        public void LoadSettings()
        {
            Settings.Load(true);
        }

        public void SaveSettings()
        {
            Settings.Save(true);
        }

        public object GetSettings()
        {
            return this.Settings;
        }

        public void ImportSettings()
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.FileName = "*.feed";
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds (*.feed)|*.feed";
            Dlg.CheckFileExists = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Settings.Load(false, Dlg.FileName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot import from " + Dlg.FileName + ", " + ex.Message);
                }
            }
        }

        public void ExportSettings()
        {
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds (*.feed)|*.feed";
            Dlg.OverwritePrompt = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Settings.Save(false, Dlg.FileName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot export to " + Dlg.FileName + ", " + ex.Message);
                }
            }
        }

        public void Start(PlaneFeedPluginArgs args)
        {
        }

        // Deliberately does not depend on ScoutBase.Core's Log/LogWriter (kept this project
        // free of that dependency chain), and Console.WriteLine is invisible in a WinForms
        // app with no attached console. args.LogDirectory is already handed to us, so write
        // a plain text log there directly.
        private void LogMsg(PlaneFeedPluginArgs args, string message)
        {
            string line = "[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "] " + message;
            Console.WriteLine("[" + this.GetType().Name + "]: " + message);
            try
            {
                if (!string.IsNullOrEmpty(args?.LogDirectory) && Directory.Exists(args.LogDirectory))
                {
                    string logfile = Path.Combine(args.LogDirectory, "AirScout.PlaneFeeds.Plugin.PublicBinCraft.log");
                    File.AppendAllText(logfile, line + Environment.NewLine);
                }
            }
            catch
            {
                // logging must never break the feed
            }
        }

        public PlaneFeedPluginPlaneInfoList GetPlanes(PlaneFeedPluginArgs args)
        {
            var planes = new PlaneFeedPluginPlaneInfoList();

            // Every readsb-compatible server publishes the same /re-api/ path, so only the
            // host itself needs configuring - build the actual request URL from it here.
            // Uri.GetLeftPart(UriPartial.Authority) also tolerates a stray trailing slash or
            // path the user might paste in by habit, normalizing down to scheme+host.
            string baseUrl = new Uri(Settings.Host).GetLeftPart(UriPartial.Authority);
            string referer = baseUrl + "/";
            string url = baseUrl + "/re-api/?binCraft&zstd&box=" +
                args.MinLat.ToString(CultureInfo.InvariantCulture) + "," +
                args.MaxLat.ToString(CultureInfo.InvariantCulture) + "," +
                args.MinLon.ToString(CultureInfo.InvariantCulture) + "," +
                args.MaxLon.ToString(CultureInfo.InvariantCulture);
            byte[] data;

            try
            {
                LogMsg(args, "Requesting " + url + " (Referer: " + referer + ")");

                // Headers below mirror what Chrome actually sends to these endpoints
                // (captured via devtools against adsb.lol and airplanes.live).
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.CookieContainer = Cookies;
                req.Accept = "*/*";
                if (!string.IsNullOrEmpty(Settings.UserAgent))
                    req.UserAgent = Settings.UserAgent;
                req.Referer = referer;
                req.Headers.Add("Accept-Language", "en-US,en;q=0.9");
                req.Headers.Add("DNT", "1");
                req.Headers.Add("Priority", "u=1, i");
                req.Headers.Add("Sec-CH-UA", "\"Not=A?Brand\";v=\"99\", \"Google Chrome\";v=\"151\", \"Chromium\";v=\"151\"");
                req.Headers.Add("Sec-CH-UA-Mobile", "?0");
                req.Headers.Add("Sec-CH-UA-Platform", "\"macOS\"");
                req.Headers.Add("Sec-Fetch-Dest", "empty");
                req.Headers.Add("Sec-Fetch-Mode", "cors");
                req.Headers.Add("Sec-Fetch-Site", "same-origin");
                req.Headers.Add("X-Requested-With", "XMLHttpRequest");

                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var stream = resp.GetResponseStream())
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    data = ms.ToArray();
                    LogMsg(args, "HTTP " + (int)resp.StatusCode + ", " + data.Length + " bytes, content-type=" + resp.ContentType);
                }
            }
            catch (Exception ex)
            {
                LogMsg(args, "Error while getting data: " + ex);
                return planes;
            }

            try
            {
                if (Settings.Compressed)
                {
                    data = PublicBinCraftParser.Decompress(data);
                    LogMsg(args, "Decompressed to " + data.Length + " bytes");
                }

                BinCraftHeader header = PublicBinCraftParser.ParseHeader(data);
                LogMsg(args, "Header: box=[" + header.South + "," + header.West + "," + header.North + "," + header.East +
                    "] resultCount=" + header.ResultCount + " binCraftVersion=" + header.BinCraftVersion +
                    " elementSize=" + header.ElementSize);

                List<BinCraftAircraft> aircraft = PublicBinCraftParser.ParseAircraft(data, header);
                int skippedNoPosition = 0;

                foreach (BinCraftAircraft ac in aircraft)
                {
                    // a position-less record isn't useful as a position message
                    if (!ac.Lat.HasValue || !ac.Lon.HasValue)
                    {
                        skippedNoPosition++;
                        continue;
                    }

                    var plane = new PlaneFeedPluginPlaneInfo
                    {
                        Hex = ac.Hex,
                        Lat = ac.Lat.Value,
                        Lon = ac.Lon.Value,
                        Alt = ac.BaroAlt ?? ac.GeomAlt ?? double.MinValue,
                        Track = ac.Track ?? double.MinValue,
                        Speed = ac.Gs ?? double.MinValue,
                        Call = ac.Callsign ?? "",
                        Reg = ac.Registration ?? "",
                        Type = ac.TypeCode ?? "",
                        VSpeed = ac.BaroRate.HasValue ? (int)ac.BaroRate.Value : 0,
                        Ground = ac.AirGround == AirGround.Ground,
                        Time = header.Now.AddSeconds(-(ac.SeenPos ?? ac.Seen ?? 0)),
                    };

                    planes.Add(plane);
                }

                LogMsg(args, "Parsed " + aircraft.Count + " records, " + skippedNoPosition + " without a valid position, returning " + planes.Count + " planes");
            }
            catch (Exception ex)
            {
                LogMsg(args, "Error while decoding binCraft data: " + ex);
            }

            return planes;
        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            Settings.Save(true);
        }
    }
}
