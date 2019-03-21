using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Globalization;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AirScout.Core;
using AirScout.Aircrafts;
using AirScout.PlaneFeeds.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using ScoutBase.Core;

namespace AirScout.PlaneFeeds
{
    public class PlaneFeedSettings_FR : PlaneFeedSettings
    {
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Base URL for website.")]
        [Browsable(false)]
        public virtual string URL
        {
            get
            {
                return "http://arn.data.fr24.com/zones/fcgi/feed.js";
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Update interval for website request [seconds]")]
        public virtual int Interval
        {
            get
            {
                return Properties.Settings.Default.FR_Interval;
            }
            set
            {
                Properties.Settings.Default.FR_Interval = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Save downloaded JSON to file")]
        public virtual bool SaveToFile
        {
            get
            {
                return Properties.Settings.Default.FR_SaveToFile;
            }
            set
            {
                Properties.Settings.Default.FR_SaveToFile = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Login Data")]
        [DescriptionAttribute("Base URL for login.")]
        [Browsable(false)]
        public virtual string Login_URL
        {
            get
            {
                return "https://www.flightradar24.com/premium/ws.php";
            }
        }

        [CategoryAttribute("Login Data")]
        [DescriptionAttribute("User name.")]
        public virtual string Login_User
        {
            get
            {
                string user = "";
                // try to decrypt data
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(Properties.Settings.Default.FR_Login_User);
                    byte[] passwordBytes = ProtectedData.Unprotect(cipherBytes, null, DataProtectionScope.CurrentUser);
                    user = Encoding.Unicode.GetString(passwordBytes);
                }
                catch
                {
                    // do nothing if failed
                }
                return user;
            }
            set
            {
                string user = "";
                // try to encrypt data
                try
                {
                    byte[] passwordBytes = Encoding.Unicode.GetBytes(value);
                    byte[] cipherBytes = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);
                    user = Convert.ToBase64String(cipherBytes); Properties.Settings.Default.Save();
                }
                catch
                {
                }
                Properties.Settings.Default.FR_Login_User = user;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Login Data")]
        [DescriptionAttribute("Password.")]
        public virtual string Login_Password
        {
            get
            {
                string password = "";
                // try to decrypt data
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(Properties.Settings.Default.FR_Login_Password);
                    byte[] passwordBytes = ProtectedData.Unprotect(cipherBytes, null, DataProtectionScope.CurrentUser);
                    password = Encoding.Unicode.GetString(passwordBytes);
                }
                catch
                {
                    // do nothing if failed
                }
                return password;
            }
            set
            {
                string password = "";
                // try to encrypt data
                try
                {
                    byte[] passwordBytes = Encoding.Unicode.GetBytes(value);
                    byte[] cipherBytes = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);
                    password = Convert.ToBase64String(cipherBytes); Properties.Settings.Default.Save();
                }
                catch
                {
                }
                Properties.Settings.Default.FR_Login_Password = password;
                Properties.Settings.Default.Save();
            }
        }

    }

    public class PlaneFeed_FR : PlaneFeed
    {
        [Browsable(false)]
        public override string Name
        {
            get
            {
                return Properties.Settings.Default.FR_Name; ;
            }
            protected set
            {
                Properties.Settings.Default.FR_Name = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Disclaimer
        {
            get
            {
                return Properties.Settings.Default.FR_Disclaimer;
            }
            protected set
            {
                Properties.Settings.Default.FR_Disclaimer = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string DisclaimerAccepted
        {
            get
            {
                return Properties.Settings.Default.FR_Disclaimer_Accepted;
            }
            set
            {
                Properties.Settings.Default.FR_Disclaimer_Accepted = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Info
        {
            get
            {
                return Properties.Settings.Default.FR_Info;
            }
            protected set
            {
                Properties.Settings.Default.FR_Info = value;
                Properties.Settings.Default.Save();
            }
        }

        public new PlaneFeedSettings_FR FeedSettings = new PlaneFeedSettings_FR();

        public PlaneFeed_FR()
            : base()
        {
            HasSettings = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            PlaneFeedWorkEventArgs args = (PlaneFeedWorkEventArgs)e.Argument;
            // set directories
            AppDirectory = args.AppDirectory;
            AppDataDirectory = args.AppDataDirectory;
            LogDirectory = args.LogDirectory;
            TmpDirectory = args.TmpDirectory;
            DatabaseDirectory = args.DatabaseDirectory;
            // set boundaries from arguments
            MaxLat = args.MaxLat;
            MinLon = args.MinLon;
            MinLat = args.MinLat;
            MaxLon = args.MaxLon;
            MyLat = args.MyLat;
            MyLon = args.MyLon;
            DXLat = args.DXLat;
            DXLon = args.DXLon;
            MinAlt = args.MinAlt;
            MaxAlt = args.MaxAlt;
            
            // check boundaries
            if ((MaxLat <= MinLat) || (MaxLon <= MinLon))
            {
                Status = STATUS.ERROR;
                this.ReportProgress((int)PROGRESS.ERROR, "Area boundaries mismatch. Check your Covered Area parameters!");
            }
            else
            {
                Status = STATUS.OK;
                int interval = Properties.Settings.Default.FR_Interval;
                // run loop
                do
                {
                    string login;
                    string json = "";
                    DateTime start = DateTime.UtcNow;
                    // check for empty user data
                    if (String.IsNullOrEmpty(FeedSettings.Login_User) || String.IsNullOrEmpty(FeedSettings.Login_Password))
                    {
                        this.ReportProgress((int)PROGRESS.STATUS, "[" + start.ToString("HH:mm:ss") + "] " +
                            "Login to Flightradar24 failed (Empty user or password). Unable to reed plane feed.");
                        Thread.Sleep(FeedSettings.Interval * 1000);
                        continue;
                    }
                    // initialize a new cookie container
                    CookieContainer cookies = new CookieContainer();
                    // calculate url and get json
                    String url = FeedSettings.URL + "?bounds=" +
                        MaxLat.ToString(CultureInfo.InvariantCulture) + "," +
                        MinLat.ToString(CultureInfo.InvariantCulture) + "," +
                        MinLon.ToString(CultureInfo.InvariantCulture) + "," +
                        MaxLon.ToString(CultureInfo.InvariantCulture);
                    try
                    {
                        // check login status
                        string post_data = "email=" + FeedSettings.Login_User +
                            "&password=" + FeedSettings.Login_Password + 
                             "&remember=false&type=web";
                        // create a POST request
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FeedSettings.Login_URL);
                        request.CookieContainer = cookies;
                        request.KeepAlive = false;
                        request.ProtocolVersion = HttpVersion.Version10;
                        request.Method = "POST";

                        // turn the request string into a byte stream
                        byte[] postBytes = Encoding.ASCII.GetBytes(post_data);

                        // this is important - make sure you specify type this way
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.KeepAlive = true;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:37.0) Gecko/20100101 Firefox/37.0";
                        request.Accept = "*/*";
                        request.Expect = "";
                        request.ContentLength = postBytes.Length;
                        Stream requestStream = request.GetRequestStream();

                        // now send it
                        requestStream.Write(postBytes, 0, postBytes.Length);
                        requestStream.Close();

                        // grab te response
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            login = sr.ReadToEnd();
                        }
                        // check for success
                        if (!login.ToUpper().Contains("SUCCEEDED"))
                        {
                            this.ReportProgress((int)PROGRESS.STATUS, "[" + start.ToString("HH:mm:ss") + "] " +
                                "Login to Flightradar24 failed (Wrong user or password). Unable to reed plane feed.");
                            Thread.Sleep(FeedSettings.Interval * 1000);
                            continue;
                        }
                        // read data if login was successful
                        HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                        webrequest.Accept = "application/json, text/javascript, */*";
                        webrequest.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
                        webrequest.CookieContainer = cookies;
                        webrequest.Referer = "http://www.flightradar24.com/";
                        webrequest.UserAgent = "Mozilla/5.0 (X11; Linux x86_64; rv:17.0) Gecko/20130807 Firefox/17.0";
                        HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                        // get the complete json string
                        using (StreamReader sr = new StreamReader(webresponse.GetResponseStream()))
                        {
                            json = sr.ReadToEnd();
                        }
                    }
                    catch (Exception ex)
                    {
                        // do nothing
                    }
                    if (FeedSettings.SaveToFile)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "fr24.json"))
                            {
                                sw.WriteLine(json);
                            }
                        }
                        catch
                        {
                        }
                    }
                    int total = 0;
                    int count = 0;
                    int errors = 0;
                    List<PlaneInfo> planes = new List<PlaneInfo>();
                    // analyze json string for planes data
                    if (json.Contains(']'))
                    {
                        // clean header
                        json = json.Remove(0, json.IndexOf(",") + 1);
                        json = json.Remove(0, json.IndexOf(",") + 1);
                        // split plane positions
                        string[] items = json.Split(']');
                        string[] par = new string[0];
                        foreach (string item in items)
                        {
                            try
                            {
                                if (item.Length > 11)
                                {
                                    total++;
                                    string d = item.Substring(2).Replace(":", ",").Replace("\"", string.Empty).Replace("[", string.Empty);
                                    par = d.Split(',');

                                    // fill planeinfo with fields from JSON string
                                    PlaneInfo info = new PlaneInfo();
                                    long l = System.Convert.ToInt64(par[11].ToString());
                                    DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                                    timestamp = timestamp.AddSeconds(l);
                                    info.Time = timestamp;
                                    try
                                    {
                                        // fill in the "must have" info
                                        info.Call = par[17];
                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        provider.NumberDecimalSeparator = ".";
                                        provider.NumberGroupSeparator = ",";
                                        provider.NumberGroupSizes = new int[] { 3 };
                                        info.Lat = double.Parse(par[2], NumberStyles.Float, provider);
                                        info.Lon = double.Parse(par[3], NumberStyles.Float, provider);
                                        info.Alt = double.Parse(par[5], NumberStyles.Float, provider);
                                        info.Speed = double.Parse(par[6], NumberStyles.Float, provider);
                                        info.Hex = par[1];
                                        try
                                        {
                                            if (par[4].Length > 0)
                                                info.Track = System.Convert.ToInt32(par[4]);
                                        }
                                        catch
                                        {
                                        }
                                        info.Reg = par[10].ToString();
                                        info.Type = par[9].ToString();
                                        AircraftTypeDesignator type = AircraftData.Database.AircraftTypeFindByICAO(info.Type);
                                        if (type != null)
                                        {
                                            info.Manufacturer = type.Manufacturer;
                                            info.Model = type.Model;
                                            info.Category = type.Category;
                                        }
                                        else
                                        {
                                            info.Manufacturer = "[unknown]";
                                            info.Model = "[unknown]";
                                            info.Category = PLANECATEGORY.NONE;
                                        }
                                        if ((info.Lat >= MinLat) &&
                                            (info.Lat <= MaxLat) &&
                                            (info.Lon >= MinLon) &&
                                            (info.Lon <= MaxLon) &&
                                            (info.Alt_m >= MinAlt) &&
                                            (info.Alt_m <= MaxAlt))
                                        {
                                            planes.Add(info);
                                            count++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // discard data if any exception occured
                                        System.Console.WriteLine("Error in plane data: " + item);
                                        errors++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // discard data if any exception occured
                                System.Console.WriteLine("Error in plane data: " + item);
                                errors++;
                            }
                        }
                    }
                    DateTime stop = DateTime.UtcNow;
                    ReportProgress((int)PROGRESS.PLANES, planes);
                    AircraftData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(planes);
                    string msg = "[" + start.ToString("HH:mm:ss") + "] " +
                        total.ToString() + " Positions updated from " + "www.fligthradar24.com" + ", " +
                        (stop - start).Milliseconds.ToString() + " ms. OK: " + count.ToString() + ", Errors: " + errors.ToString();
                    this.ReportProgress((int)PROGRESS.STATUS, msg);
                    int i = 0;
                    while (!CancellationPending && (i < interval))
                    {
                        Thread.Sleep(1000);
                        i++;
                    }
                }
                while (!this.CancellationPending || (Status != STATUS.OK));
            }
            this.ReportProgress((int)PROGRESS.FINISHED);
        }

        public override Object GetFeedSettings()
        {
            return FeedSettings;
        }

    }
}
