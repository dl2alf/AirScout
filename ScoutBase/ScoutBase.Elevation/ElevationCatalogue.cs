using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ScoutBase.Core;
using SQLiteDatabase;

namespace ScoutBase.Elevation
{

    /// <summary>
    /// Holds the elevation tiles catalogue
    /// keeps a local copy of elevation catalogue in a local dictionary
    /// provides a list of available loc files
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    [JsonObject]
    public class ElevationCatalogue
    {
        public string BaseDir { get; set; }
        public string BaseURL { get; set; }
        public double MinLat { get; set; }
        public double MinLon { get; set; }
        public double MaxLat { get; set; }
        public double MaxLon { get; set; }
        public DateTime LastModified { get; set; }

        public SortedDictionary<string, DateTime> Files = new SortedDictionary<string, DateTime>();

        public ElevationCatalogue (BackgroundWorker caller, string baseurl, string basedir, double minlat, double minlon, double maxlat, double maxlon)
        {
            BaseURL = baseurl;
            if (!BaseURL.EndsWith("/"))
                BaseURL = BaseURL + "/";
            BaseDir = basedir;
            MinLat = minlat;
            MinLon = minlon;
            MaxLat = maxlat;
            MaxLon = maxlon;
            // build web catalogue first
            // check and download catalogue file
            string url = BaseURL + "files.zip";
            string zipfilename = Path.Combine(BaseDir, "files.zip");
            string filename = Path.Combine(BaseDir, "files.cat");
            // fill a dictionary with needed squares
            List<string> sq = ElevationData.Database.GetLocsFromRect(minlat, minlon, maxlat, maxlon, 2);
            SortedDictionary<string, string> squares = new SortedDictionary<string, string>();
            foreach (string s in sq)
                squares.Add(s, null);
            // report progress
            if (caller != null)
            {
                if (caller.WorkerReportsProgress)
                    caller.ReportProgress(0, "Downloading elevation tile catalogue from web (please wait)...");
            }
            // get locs catalogue from web
            AutoDecompressionWebClient client = new AutoDecompressionWebClient();
            client.DownloadFileIfNewer(url, zipfilename, true,true);
            if (File.Exists(filename))
            {
                // get LastModified from catalogue
                LastModified = File.GetLastWriteTimeUtc(filename);
                // read catalogue and fill LastUpdated timestamp
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    int i = 0;
                    while (!sr.EndOfStream)
                    { 
                        string s = sr.ReadLine();
                        try
                        {
                            if (!String.IsNullOrEmpty(s) && !s.StartsWith("/"))
                            {
                                string[] a = s.Split(';');
                                DateTime lastupdated;
                                string square = a[0].Substring(0, 4).ToUpper();
                                string dummy;
                                if (caller != null)
                                {
                                    if( (caller.WorkerReportsProgress) && (i%1000 == 0))

                                        caller.ReportProgress(0, "Updating elevation tile information [" + i.ToString() + "], please wait...");
                                }
                                if (squares.TryGetValue(square, out dummy))
                                {
                                    if (!this.Files.TryGetValue(a[0], out lastupdated))
                                        this.Files.Add(a[0], DateTime.ParseExact(a[1], "yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime());
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        i++;
                    }
                    st.Stop();
                    Console.WriteLine("Reading catalogue: " + st.ElapsedMilliseconds.ToString() + " ms.");
                }
            }
        }

        public ElevationCatalogue() : base()
        {
           
        }

        public bool LocExists(string loc)
        {
            DateTime dummy;
            return this.Files.TryGetValue(loc, out dummy);
        }

        public DateTime LastUpdated(string loc)
        {
            DateTime lastupdated;
            if (this.Files.TryGetValue(loc, out lastupdated))
                return lastupdated;
            return DateTime.MinValue;
        }

        public string ToJSON()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            string json = JsonConvert.SerializeObject(this, settings);
            return json;
        }

        public void ToJSONFile (string filename)
        {
            string json = this.ToJSON();
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(json);
            }
        }

        public static ElevationCatalogue FromJSON(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.DeserializeObject<ElevationCatalogue>(json, settings);
        }

        public static ElevationCatalogue FromJSONFile(string filename)
        {
            if (!File.Exists(filename))
                return null;
            string json = "";
            using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
            {
                json = sr.ReadToEnd();
            }
            return FromJSON(json);
        }

        public static ElevationCatalogue FromJSONFileCheckBoundsAndLastModified(string filename, double minlat, double minlon, double maxlat, double maxlon)
        {
            // read cache from json file
            ElevationCatalogue cat = FromJSONFile(filename);
            // return null on error
            if (cat == null)
                return null;
            // check last write time from json file --> must be equal to LastModification timestamp of catalogue
            if (File.GetLastWriteTimeUtc(filename) != cat.LastModified)
                return null;
            // check if bounds are equal
            if ((cat.MinLat != minlat) || (cat.MinLon != minlon) || (cat.MaxLat != maxlat) || (cat.MaxLon != maxlon))
                return null;
            // success --> return cached catalogue
            return cat;
        }
    }
}