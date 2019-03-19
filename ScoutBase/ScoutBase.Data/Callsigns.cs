using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScoutBase.Core;
using ScoutBase.Data;
using System.Reflection;

namespace ScoutBase.Data
{

    [System.ComponentModel.DesignerCategory("")]
    public class DataTableCallsigns : DataTable
    {
        public DataTableCallsigns()
            : base()
        {
            // set table name
            TableName = "Callsigns";
            // create all specific columns
            DataColumn Call = this.Columns.Add("Call", typeof(string));
            DataColumn Lat = this.Columns.Add("Lat", typeof(double));
            DataColumn Lon = this.Columns.Add("Lon", typeof(double));
            DataColumn Source = this.Columns.Add("Source", typeof(string));
            DataColumn LastUpdated = this.Columns.Add("LastUpdated", typeof(string));
            // create primary key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = Call;
            this.PrimaryKey = keys;
        }
    }

    [Serializable]
    [System.ComponentModel.DesignerCategory("")]
    public class JSONArrayCallsigns : Object
    {
        public string version = "";
        public int full_count = 0;
        public List<List<string>> calls;

        public JSONArrayCallsigns()
        {
            version = "";
            full_count = 0;
            calls = new List<List<string>>();
        }

        public JSONArrayCallsigns(CallsignDictionary callsigns)
        {
            version = callsigns.Version;
            full_count = callsigns.Calls.Count;
            calls = new List<List<string>>(full_count);
            foreach (CallsignDesignator call in callsigns.Calls.Values)
            {
                List<string> l = new List<string>();
                l.Add(call.Call);
                l.Add(call.Lat.ToString("F8", CultureInfo.InvariantCulture));
                l.Add(call.Lon.ToString("F8", CultureInfo.InvariantCulture));
                l.Add(call.Source.ToString());
                l.Add(call.LastUpdated.ToString("u"));
                calls.Add(l);
            }
        }
    }

    //
    // lookup table for all callsigns
    //

    [Serializable]
    [System.ComponentModel.DesignerCategory("")]
    public class CallsignDictionary
    {
        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetAssembly(typeof(ScoutBaseDatabase)).GetName().Version.ToString();

            }
        }

        public string Name
        {
            get
            {
                return "Callsigns";
            }
        }

        private bool changed = false;
        public bool Changed
        {
            get
            {
                return changed;
            }
        }

        public SortedDictionary<string, CallsignDesignator> Calls = new SortedDictionary<string, CallsignDesignator>();

        public CallsignDictionary()
        {
        }

        public CallsignDictionary(DataTable table)
        {
            // create dictionary from data table
            FromTable(table);
            // reset changed flag
            changed = false;
        }

        public void Clear()
        {
            if (Calls.Count > 0)
                changed = true;
            Calls.Clear();
        }

        public void FromTable(DataTable dt)
        {
            if (dt == null)
                return;
            foreach (DataRow row in dt.Rows)
            {
                string call = row[0].ToString();
                double lat = (double)row[1];
                double lon = (double)row[2];
                GEOSOURCE source = GEOSOURCE.UNKONWN;
                try
                {
                    source = (GEOSOURCE)Enum.Parse(typeof(GEOSOURCE), row["Source"].ToString());
                }
                catch
                {
                }
                DateTime lastupdated = DateTime.UtcNow;
                try
                {
                    try
                    {
                        lastupdated = DateTime.ParseExact(row["LastUpdated"].ToString(),"yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture);
                        lastupdated = lastupdated.ToUniversalTime();
                    }
                    catch
                    {
                    }
                    Update(new CallsignDesignator(call, lat, lon, source, lastupdated));
                }
                catch
                {
                }
            }
                
        }

        public string ToJSON()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Formatting.Indented;
            string json = JsonConvert.SerializeObject(this.Calls.Values, settings);
            return json;
        }

        public string ToJSONArray()
        {
            JSONArrayCallsigns a = new JSONArrayCallsigns(this);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Formatting.Indented;
            string json = JsonConvert.SerializeObject(a, settings);
            return json;
        }

        public DataTable FromJSONArray(string filename)
        {
            if (!File.Exists(filename))
                return new DataTableCallsigns();
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    string json = sr.ReadToEnd();
                    JSONArrayCallsigns a = JsonConvert.DeserializeObject<JSONArrayCallsigns>(json);
                    // check version
                    if (String.Compare(Version, a.version) != 0)
                    {
                        // do upgrade/downgrade stuff here
                    }
                    foreach (List<string> l in a.calls)
                    {
                        string call = l[0];
                        double lat = System.Convert.ToDouble(l[1], CultureInfo.InvariantCulture);
                        double lon = System.Convert.ToDouble(l[2], CultureInfo.InvariantCulture);
                        GEOSOURCE source = GEOSOURCE.UNKONWN;
                        try
                        {
                            source = (GEOSOURCE) Enum.Parse(typeof(GEOSOURCE),l[3]);
                        }
                        catch
                        {
                        }
                        DateTime lastupdated = DateTime.UtcNow;
                        try
                        {
                            lastupdated = System.Convert.ToDateTime(l[4]);
                        }
                        catch
                        {
                        }
                        Update(new CallsignDesignator(call, lat, lon, source, lastupdated));
                    }
                }
                return ToTable();
            }
            catch (Exception ex)
            {
            }
            return new DataTableCallsigns();
        }

        public DataTableCallsigns FromTXT(string filename)
        {
            // imports a TXT with old AirScout data format
            DataTableCallsigns dt = new DataTableCallsigns();
            if (!File.Exists(filename))
                return dt;
            try
            {
                string s = "";
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    while (!sr.EndOfStream)
                    {
                        s = sr.ReadLine();
                        if (!String.IsNullOrEmpty(s) && !s.StartsWith("//"))
                        {
                            string[] a = s.Split(';');
                            // store array values in DataTable
                            DataRow row = dt.NewRow();
                            string call = a[0];
                            double lat = System.Convert.ToDouble(a[1], CultureInfo.InvariantCulture);
                            double lon = System.Convert.ToDouble(a[2], CultureInfo.InvariantCulture);
                            string loc = a[3];
                            GEOSOURCE source = GEOSOURCE.UNKONWN;
                            if (MaidenheadLocator.IsPrecise(lat, lon, loc.Length / 2))
                                source = GEOSOURCE.FROMUSER;
                            else
                                source = GEOSOURCE.FROMLOC;
                            string lastupdated = a[6];
                            if (GeographicalPoint.Check(lat, lon))
                            {
                                row["Call"] = call;
                                row["Lat"] = lat;
                                row["Lon"] = lon;
                                row["Source"] = source.ToString();
                                row["LastUpdated"] = lastupdated;
                                DataRow oldrow = dt.Rows.Find(row["Call"].ToString());
                                if (oldrow != null)
                                {
                                    // call found --> check for update
                                    if (String.Compare(row["LastUpdated"].ToString(), oldrow["LastUpdated"].ToString()) > 0)
                                    {
                                        oldrow["Lat"] = row["Lat"];
                                        oldrow["Lon"] = row["Lon"];
                                        oldrow["Source"] = row["Source"];
                                        oldrow["LastUpdated"] = row["LastUpdated"];
                                    }
                                }
                                else
                                {
                                    // add new row
                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return dt;
        }

        public DataTableCallsigns FromCALL3(string filename)
        {
            // imports CALL3.TXT as used by WSJT
            // fields are 
            // CALLSIGN, LOCATOR, EME FLAG, (these first three fields are used by WSJT)
            // plus optional fields:
            // STATE, FIRST NAME, EMAIL ADDRESS, NOTES, REVISION DATE

            DataTableCallsigns dt = new DataTableCallsigns();
            if (!File.Exists(filename))
                return dt;
            try
            {
                string s = "";
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    while (!sr.EndOfStream)
                    {
                        s = sr.ReadLine();
                        if (!String.IsNullOrEmpty(s) && !s.StartsWith("//"))
                        {
                            string[] a = s.Split(',');
                            // store array values in DataTable
                            string call = a[0];
                            string loc = a[1];
                            double lat = double.NaN;
                            double lon = double.NaN;
                            if (MaidenheadLocator.Check(loc))
                            {
                                MaidenheadLocator.LatLonFromLoc(loc, PositionInRectangle.MiddleMiddle, out lat, out lon);
                            }
                            GEOSOURCE source = GEOSOURCE.FROMLOC;
                            DateTime lastupdated = DateTime.MinValue.ToUniversalTime();
                            if (a.Length >= 7)
                            {
                                // try to get an revision date maybe in various formats
                                // MMMonVHF
                                try
                                {
                                    lastupdated = DateTime.ParseExact(a[7], "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            if (GeographicalPoint.Check(lat, lon))
                            {
                                DataRow row = dt.NewRow();
                                row["Call"] = call;
                                row["Lat"] = lat;
                                row["Lon"] = lon;
                                row["Source"] = source.ToString();
                                row["LastUpdated"] = lastupdated.ToString("u");
                                DataRow oldrow = dt.Rows.Find(row["Call"].ToString());
                                if (oldrow != null)
                                {
                                    // call found --> check for update
                                    if (String.Compare(row["LastUpdated"].ToString(), oldrow["LastUpdated"].ToString()) > 0)
                                    {
                                        oldrow["Lat"] = row["Lat"];
                                        oldrow["Lon"] = row["Lon"];
                                        oldrow["Source"] = row["Source"];
                                        oldrow["LastUpdated"] = row["LastUpdated"];
                                    }
                                }
                                else
                                {
                                    // add new row
                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTableCallsigns FromDTB(string filename)
        {
            // imports DTB database from Win-Test

            DataTableCallsigns dt = new DataTableCallsigns();
            if (!File.Exists(filename))
                return dt;
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    while (!sr.EndOfStream)
                    {
                        char[] buffer = new char[26];
                        sr.ReadBlock(buffer, 0, 14);
                        sr.ReadBlock(buffer, 14, 12);
                        if (!sr.EndOfStream)
                        {
                            string call = "";
                            string loc = "";
                            string lastupdated = "";
                            double lat = double.NaN;
                            double lon = double.NaN;
                            int i = 0;
                            while ((buffer[i] != 0) && (i < 14))
                            {
                                call += (char)buffer[i];
                                i++;
                            }
                            i = 14;
                            while ((i < 21) && (buffer[i] != 0))
                            {
                                loc += (char)buffer[i];
                                i++;
                            }
                            i = 21;
                            while ((i < 26) && (buffer[i] != 0))
                            {
                                lastupdated += (char)buffer[i];
                                i++;
                            }

                            call = call.Trim().ToUpper();
                            loc = loc.Trim().ToUpper();
                            try
                            {
                                if (lastupdated[2] < '5')
                                    lastupdated = "20" + lastupdated[2] + lastupdated[3] + "-" + lastupdated[0] + lastupdated[1] + "-01 00:00:00Z";
                                else
                                    lastupdated = "19" + lastupdated[2] + lastupdated[3] + "-" + lastupdated[0] + lastupdated[1] + "-01 00:00:00Z";
                            }
                            catch
                            {
                            }
                            if (MaidenheadLocator.Check(loc))
                                MaidenheadLocator.LatLonFromLoc(loc, PositionInRectangle.MiddleMiddle, out lat, out lon);
                            GEOSOURCE source = GEOSOURCE.FROMLOC;
                            if (GeographicalPoint.Check(lat, lon))
                            {
                                DataRow row = dt.NewRow();
                                row["Call"] = call;
                                row["Lat"] = lat;
                                row["Lon"] = lon;
                                row["Source"] = source.ToString();
                                row["LastUpdated"] = lastupdated;
                                DataRow oldrow = dt.Rows.Find(row["Call"].ToString());
                                if (oldrow != null)
                                {
                                    // call found --> check for update
                                    if (String.Compare(row["LastUpdated"].ToString(), oldrow["LastUpdated"].ToString()) > 0)
                                    {
                                        oldrow["Lat"] = row["Lat"];
                                        oldrow["Lon"] = row["Lon"];
                                        oldrow["Source"] = row["Source"];
                                        oldrow["LastUpdated"] = row["LastUpdated"];
                                    }
                                }
                                else
                                {
                                    // add new row
                                    dt.Rows.Add(row);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTableCallsigns FromCSV(string filename)
        {
            // imports a variable csv format with autodetect of callsign, loc and timestamp
            DataTableCallsigns dt = new DataTableCallsigns();
            if (!File.Exists(filename))
                return dt;
            try
            {
                string s = "";
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    while (!sr.EndOfStream)
                    {
                        s = sr.ReadLine();
                        if (!String.IsNullOrEmpty(s) && !s.StartsWith("//"))
                        {
                            string[] a = s.Split(';');
                            if (a.Length < 2)
                                a = s.Split(',');
                            string call = "";
                            double lat = double.NaN;
                            double lon = double.NaN;
                            string loc = "";
                            GEOSOURCE source = GEOSOURCE.FROMLOC;
                            // set lastupdated to filetime if no timestamp is found on file
                            string filetime = File.GetCreationTimeUtc(filename).ToString("u");
                            string lastupdated = filetime;
                            foreach (string entry in a)
                            {
                                // search for 1st locator in line
                                if (MaidenheadLocator.Check(entry) && (entry.Length == 6) && String.IsNullOrEmpty(loc))
                                {
                                    MaidenheadLocator.LatLonFromLoc(entry, PositionInRectangle.MiddleMiddle, out lat, out lon);
                                    break;
                                }
                                // search for 1st callsign in line
                                if (Callsign.Check(entry) && String.IsNullOrEmpty(call))
                                    call = entry.Trim().ToUpper();
                                DateTime timestamp;
                                if (DateTime.TryParseExact(entry, "yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out timestamp))
                                    lastupdated = timestamp.ToString("u");
                            }
                            if (GeographicalPoint.Check(lat, lon))
                            {
                                // store array values in DataTable
                                DataRow row = dt.NewRow();
                                row["Call"] = call;
                                row["Lat"] = lat;
                                row["Lon"] = lon;
                                row["Source"] = source.ToString();
                                row["LastUpdated"] = lastupdated;
                                DataRow oldrow = dt.Rows.Find(row["Call"].ToString());
                                if (oldrow != null)
                                {
                                    // call found --> check for update
                                    if (String.Compare(row["LastUpdated"].ToString(), oldrow["LastUpdated"].ToString()) > 0)
                                    {
                                        oldrow["Lat"] = row["Lat"];
                                        oldrow["Lon"] = row["Lon"];
                                        oldrow["Source"] = row["Source"];
                                        oldrow["LastUpdated"] = row["LastUpdated"];
                                    }
                                }
                                else
                                {
                                    // add new row
                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return dt;
        }

        public DataTableCallsigns FromUSR(string filename)
        {
            // imports a USR with AirScout user data format
            DataTableCallsigns dt = new DataTableCallsigns();
            if (!File.Exists(filename))
                return dt;
            try
            {
                string s = "";
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    while (!sr.EndOfStream)
                    {
                        s = sr.ReadLine();
                        if (!String.IsNullOrEmpty(s) && !s.StartsWith("//"))
                        {
                            string[] a = s.Split(';');
                            // store array values in DataTable
                            DataRow row = dt.NewRow();
                            string call = a[0];
                            double lat = System.Convert.ToDouble(a[1], CultureInfo.InvariantCulture);
                            double lon = System.Convert.ToDouble(a[2], CultureInfo.InvariantCulture);
                            GEOSOURCE source = (MaidenheadLocator.IsPrecise(lat, lon, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC);
                            string lastupdated = a[6];
                            if (GeographicalPoint.Check(lat, lon))
                            {
                                row["Call"] = call;
                                row["Lat"] = lat;
                                row["Lon"] = lon;
                                row["Source"] = source.ToString();
                                row["LastUpdated"] = lastupdated;
                                DataRow oldrow = dt.Rows.Find(row["Call"].ToString());
                                if (oldrow != null)
                                {
                                    // call found --> check for update
                                    if (String.Compare(row["LastUpdated"].ToString(), oldrow["LastUpdated"].ToString()) > 0)
                                    {
                                        oldrow["Lat"] = row["Lat"];
                                        oldrow["Lon"] = row["Lon"];
                                        oldrow["Source"] = row["Source"];
                                        oldrow["LastUpdated"] = row["LastUpdated"];
                                    }
                                }
                                else
                                {
                                    // add new row
                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return dt;
        }

        public DataTable ToTable()
        {
            DataTableCallsigns dt = new DataTableCallsigns();
            foreach (KeyValuePair<string, CallsignDesignator> call in Calls)
            {
                DataRow row = dt.NewRow();
                row["Call"] = call.Value.Call;
                row["Lat"] = call.Value.Lat;
                row["Lon"] = call.Value.Lon;
                row["Source"] = call.Value.Source.ToString();
                row["LastUpdated"] = call.Value.LastUpdated.ToString("u");
                dt.Rows.Add(row);
            }
            return dt;
        }

        public void Update(CallsignDesignator call)
        {
            // return on null
            if (call == null)
                return;
            // return on empty hex value
            if (String.IsNullOrEmpty(call.Call))
                return;
            lock (this)
            {
                CallsignDesignator entry;
                if (!Calls.TryGetValue(call.Call, out entry))
                {
                    // key not found --> add new entry
                    Calls.Add(call.Call, new CallsignDesignator(call.Call, call.Lat, call.Lon, call.Source, call.LastUpdated));
                    changed = true;
                }
                else
                {
                    // key found --> check for update
                    if (call.LastUpdated > entry.LastUpdated)
                    {
                        lock (entry)
                        {
                            // new timestamp --> udpate all not empty fields
                            if (!String.IsNullOrEmpty(call.Call))
                                entry.Call = call.Call;
                            entry.Lat = call.Lat;
                            entry.Lon = call.Lon;
                            entry.Source = call.Source;
                            entry.LastUpdated = call.LastUpdated;
                            changed = true;
                        }
                    }
                }
            }
        }

        public CallsignDesignator FindByCall(string call)
        {
            CallsignDesignator info = null;
            if (String.IsNullOrEmpty(call))
                return info;
            Calls.TryGetValue(call, out info);
            return info;
        }
    
        public List<CallsignDesignator> GetCallsigns()
        {
            // don't use Values.ToList() as it is not sorted
            List<CallsignDesignator> l = new List<CallsignDesignator>();
            foreach (string call in Calls.Keys)
            {
                l.Add(Calls[call]);
            }
            return l;
        }

        public List<CallsignDesignator> GetCallsigns(double minlat, double maxlat, double minlon, double maxlon)
        {
            Rect area = new Rect(minlon, minlat, maxlon - minlon, maxlat - minlat);
            List<CallsignDesignator> a = GetCallsigns();
            List<CallsignDesignator> l = new List<CallsignDesignator>();
            foreach (CallsignDesignator call in a)
            {
                if(area.Contains(call.Lon, call.Lat))
                    l.Add(call);
            }
            return l;
        }


    }

    public enum GEOSOURCE
    {
        UNKONWN = 0,
        FROMLOC = 1,
        FROMUSER = 2,
        FROMBEST = 3
    }

    [Serializable]
    public class CallsignDesignator : Object
    {
        // Properties
        public string Call
        { get; set; }
        public double Lat
        { get; set; }
        public double Lon
        { get; set; }
        public GEOSOURCE Source
        { get; set; }
        public DateTime LastUpdated
        { get; set; }

        public CallsignDesignator()
            : this(String.Empty,double.NaN, double.NaN,GEOSOURCE.UNKONWN, DateTime.MinValue) { }
        public CallsignDesignator(string calll, double lat, double lon, GEOSOURCE source)
            : this(calll, lat, lon, source, DateTime.UtcNow) { }
        public CallsignDesignator(string call, double lat, double lon, GEOSOURCE source, DateTime lastupdated)
        {
            Call = call;
            Lat = lat;
            Lon = lon;
            Source = source;
            LastUpdated = lastupdated;
        }

        public CallsignDesignator (DataRow row)
        {
            /*
            // fills property fields from DataRow
            foreach (DataColumn col in row.Table.Columns)
            {
                // find property
                PropertyInfo p = this.GetType().GetProperty(col.ColumnName);

                if (p != null && row[col] != DBNull.Value)
                {
                    if (p.PropertyType.Name == "String")
                        p.SetValue(this, row[col].ToString(), null);
                    else if (p.PropertyType.Name == "Double")
                        p.SetValue(this, (double)row[col], null);
                    else if (p.PropertyType.Name == "DateTime")
                        p.SetValue(this, DateTime.ParseExact(row[col].ToString(), "yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime(), null);
                    else if (p.PropertyType.Name == "Int32")
                        p.SetValue(this, (int)row[col], null);
                    else if (p.PropertyType.Name == "Int64")
                        p.SetValue(this, (long)row[col], null);
                    else if (p.PropertyType.Name == typeof(GEOSOURCE).Name)
                        p.SetValue(this, (GEOSOURCE)Enum.Parse(typeof(GEOSOURCE), row["Source"].ToString()), null);

                    else
                        throw new NotSupportedException("Data type not supported: " + p.PropertyType.Name);
                }
            }
            */
            Call = row[0].ToString();
            Lat = (double)row[1];
            Lon = (double)row[2];
            Source = (GEOSOURCE)Enum.Parse(typeof(GEOSOURCE), row[3].ToString());
            LastUpdated = DateTime.ParseExact(row[4].ToString(), "yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public static string SQLCreateSchema()
        {
            return "CREATE TABLE IF NOT EXISTS `Callsigns` (Call TEXT UNIQUE, Lat REAL, Lon REAL, Source TEXT, LastUpdated TEXT, PRIMARY KEY (Call))";
        }

        public string WriteToJSON()
        {
            // write json string
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            string json = JsonConvert.SerializeObject(this, settings);
            return json;
        }

        public string[] ToArray()
        {
            string[] a = new string[5];
            a[0] = this.Call;
            a[1] = this.Lat.ToString("F8", CultureInfo.InvariantCulture);
            a[2] = this.Lon.ToString("F8", CultureInfo.InvariantCulture);
            a[3] = this.Source.ToString();
            a[4] = this.LastUpdated.ToString("u");
            return a;
        }
    }

    public partial class ScoutBaseDatabase
    {
        public List<CallsignDesignator> GetCallsigns()
        {
            if (callsigns == null)
                return new List<CallsignDesignator>();
            return callsigns.GetCallsigns();
        }

        public List<CallsignDesignator> GetCallsigns(double minlat, double maxlat, double minlon, double maxlon)
        {
            if (callsigns == null)
                return new List<CallsignDesignator>();
            return callsigns.GetCallsigns(minlat, maxlat, minlon, maxlon);
        }

        public CallsignDesignator CallsignFindByCall(string call)
        {
            DataTable dt = db.Select("SELECT * FROM " + Callsigns.TableName + " WHERE Call = '" + call + "'");
            if ((dt != null) && (dt.Rows.Count > 0))
                return new CallsignDesignator(dt.Rows[0]);
            return null;

//            if (callsigns == null)
//                return null;
//            return callsigns.FindByCall(call);
        }

        public void CallsignUpdate(CallsignDesignator call)
        {
            if (call == null)
                return;
            callsigns.Update(call);
        }


    }
}
