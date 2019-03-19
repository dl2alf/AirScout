
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AirScout.Database.Core;
using AirScout.Database.Aircrafts;
using ScoutBase.Core;

namespace AirScoutPlaneServer
{

    public partial class MainDlg : Form
    {

        Dictionary<string, JArray> JArrays = new Dictionary<string, JArray>();
        Dictionary<string, JProperty> JProperties = new Dictionary<string, JProperty>();

        private void ParseToken(JToken token)
        {
            foreach (var t in token.Children())
            {
                try
                {
                    if (t.GetType() == typeof(JProperty))
                    {
                        string number = JProperties.Count.ToString("00000000");
                        JProperties.Add(number, (JProperty)t);
                    }
                    else if (t.GetType() == typeof(JArray))
                    {
                        // generate a unique key in case of no parent property is found
                        string number = JArrays.Count.ToString("00000000");
                        // try to use parent's property name as a key
                        if (t.Parent.GetType() == typeof(JProperty))
                        {
                            string pval = ((JProperty)t.Parent).Name;
                            JArray a;
                            if (!JArrays.TryGetValue(pval, out a))
                            {
                                JArrays.Add(pval, (JArray)t);
                            }
                            else
                            {
                                // use number as unique key
                                JArrays.Add(number, (JArray)t);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // do nothing
                }
                ParseToken(t);
            }
        }

        private void ReadAircraftsFromURL(string url, string filename)
        {
            string json = "";
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                cl.DownloadFileIfNewer(url, filename, true);
                if (!File.Exists(filename))
                    return;
                // deserialize JSON file
                JObject o = (JObject)JsonConvert.DeserializeObject(json);
                // clear collections
                JArrays.Clear();
                JProperties.Clear();
                // parse all child tokens recursively --> can be either a property or an array
                ParseToken(o);
                // we've got all properties and arrays here
                // store array values in DataTable
                DataTableAircrafts dt = new DataTableAircrafts();
                foreach (KeyValuePair<string, JArray> a in JArrays)
                {
                    DataRow row = dt.NewRow();
                    row[0] = a.Value[0].ToString();
                    row[1] = a.Value[1].ToString();
                    row[2] = a.Value[2].ToString();
                    row[3] = a.Value[6].ToString();
                    dt.Rows.Add(row);
                }
                AircraftDatabase_old.InsertOrUpdateTable(dt);
            }
            catch (Exception ex)
            {
                // Error loading database
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + "[" + url + "]: " + ex.Message);
            }
        }

        private void ReadAircraftTypesFromURL(string url, string filename)
        {

            string json = "";
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                cl.DownloadFileIfNewer(url, filename, true);
                if (!File.Exists(filename))
                    return;
                // deserialize JSON file
                JObject o = (JObject)JsonConvert.DeserializeObject(json);
                // clear collections
                JArrays.Clear();
                JProperties.Clear();
                // parse all child tokens recursively --> can be either a property or an array
                ParseToken(o);
                // we've got all properties and arrays here
                // store array values in DataTable
                DataTableAircraftTypes dt = new DataTableAircraftTypes();
                foreach (KeyValuePair<string, JArray> a in JArrays)
                {
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < a.Value.Count; i++)
                        row[i] = a.Value[i].ToString();
                    dt.Rows.Add(row);
                }
                AircraftDatabase_old.InsertOrUpdateTable(dt);
            }
            catch (Exception ex)
            {
                // Error loading database
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + "[" + url + "]: " + ex.Message);
            }
        }

        private void ReadAircraftRegistrationsFromURL(string url, string filename)
        {

            string json = "";
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                cl.DownloadFileIfNewer(url, filename, true);
                if (!File.Exists(filename))
                    return;
                // deserialize JSON file
                JObject o = (JObject)JsonConvert.DeserializeObject(json);
                // clear collections
                JArrays.Clear();
                JProperties.Clear();
                // parse all child tokens recursively --> can be either a property or an array
                ParseToken(o);
                // we've got all properties and arrays here
                // store array values in DataTable
                DataTableAircraftRegistrations dt = new DataTableAircraftRegistrations();
                foreach (KeyValuePair<string, JArray> a in JArrays)
                {
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < a.Value.Count; i++)
                        row[i] = a.Value[i].ToString();
                    dt.Rows.Add(row);
                }
                AircraftDatabase_old.InsertOrUpdateTable(dt);
            }
            catch (Exception ex)
            {
                // Error loading database
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + "[" + url + "]: " + ex.Message);
            }
        }

        private void ReadAirlinesFromURL(string url, string filename)
        {

            string json = "";
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                cl.DownloadFileIfNewer(url, filename, true);
                if (!File.Exists(filename))
                    return;
                // deserialize JSON file
                JObject o = (JObject)JsonConvert.DeserializeObject(json);
                // clear collections
                JArrays.Clear();
                JProperties.Clear();
                // parse all child tokens recursively --> can be either a property or an array
                ParseToken(o);
                // we've got all properties and arrays here
                // store array values in DataTable
                DataTableAirlines dt = new DataTableAirlines();
                foreach (KeyValuePair<string, JArray> a in JArrays)
                {
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < a.Value.Count; i++)
                        row[i] = a.Value[i].ToString();
                    dt.Rows.Add(row);
                }
                AircraftDatabase_old.InsertOrUpdateTable(dt);
            }
            catch (Exception ex)
            {
                // Error loading database
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + "[" + url + "]: " + ex.Message);
            }
        }

        private void ReadAirportsFromURL(string url, string filename)
        {

            string json = "";
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                cl.DownloadFileIfNewer(url, filename, true);
                if (!File.Exists(filename))
                    return;
                // deserialize JSON file
                JObject o = (JObject)JsonConvert.DeserializeObject(json);
                // clear collections
                JArrays.Clear();
                JProperties.Clear();
                // parse all child tokens recursively --> can be either a property or an array
                ParseToken(o);
                // we've got all properties and arrays here
                // store array values in DataTable
                DataTableAirports dt = new DataTableAirports();
                foreach (KeyValuePair<string, JArray> a in JArrays)
                {
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < a.Value.Count; i++)
                        row[i] = a.Value[i].ToString();
                    dt.Rows.Add(row);
                }
                AircraftDatabase_old.InsertOrUpdateTable(dt);
            }
            catch (Exception ex)
            {
                // Error loading database
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + "[" + url + "]: " + ex.Message);
            }
        }

        private void bw_DatabaseUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bw_DatabaseUpdater.CancellationPending)
            {
                ReadAircraftsFromURL("http://www.airscout.eu/downloads/database/Aircrafts.json",Path.Combine(TmpDirectory,"Aircrafts.json"));
                ReadAircraftTypesFromURL("http://www.airscout.eu/downloads/database/AircraftTypes.json", Path.Combine(TmpDirectory, "AircraftTypes.json"));
                ReadAircraftRegistrationsFromURL("http://www.airscout.eu/downloads/database/AircraftRegistrations.json", Path.Combine(TmpDirectory, "AircraftRegistrations.json"));
                ReadAirlinesFromURL("http://www.airscout.eu/downloads/database/Airlines.json", Path.Combine(TmpDirectory, "Airlines.json"));
                ReadAirportsFromURL("http://www.airscout.eu/downloads/database/Airports.json", Path.Combine(TmpDirectory, "Airports.json"));

                Thread.Sleep(60000);
            }
        }

        private void bw_DatabaseUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bw_DatabaseUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

    }
}
