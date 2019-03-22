using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using ScoutBase;
using ScoutBase.Core;
using ScoutBase.Elevation;
using ScoutBase.Stations;
using SQLiteDatabase;
using Newtonsoft.Json;
using HtmlAgilityPack;
using AirScout.Aircrafts;
using Newtonsoft.Json.Linq;
using Renci.SshNet.Sftp;
using Renci.SshNet;

namespace AirScoutDatabaseManager
{
    public partial class MainDlg : Form
    {

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Application Directory")]
        public string AppDirectory
        {
            get
            {
                return Application.StartupPath.TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Local Application Data Directory")]
        public string AppDataDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName).TrimEnd(Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Logfile Directory")]
        public string LogDirectory
        {
            get
            {
                // get Property
                string logdir = Properties.Settings.Default.Log_Directory;
                // replace Windows/Linux directory spearator chars
                logdir = logdir.Replace('\\', Path.DirectorySeparatorChar);
                logdir = logdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(logdir))
                    logdir = "Log";
                // replace variables, if any
                logdir = VC.ReplaceAllVars(logdir);
                // remove directory separator chars at begin and end
                logdir = logdir.TrimStart(Path.DirectorySeparatorChar);
                logdir = logdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!logdir.Contains(Path.VolumeSeparatorChar))
                    logdir = Path.Combine(AppDataDirectory, logdir);
                return logdir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Tempfile Directory")]
        public string TmpDirectory
        {
            get
            {
                // get Property
                string tmpdir = Properties.Settings.Default.Tmp_Directory;
                // replace Windows/Linux directory spearator chars
                tmpdir = tmpdir.Replace('\\', Path.DirectorySeparatorChar);
                tmpdir = tmpdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(tmpdir))
                    tmpdir = "Tmp";
                // replace variables, if any
                tmpdir = VC.ReplaceAllVars(tmpdir);
                // remove directory separator chars at begin and end
                tmpdir = tmpdir.TrimStart(Path.DirectorySeparatorChar);
                tmpdir = tmpdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!tmpdir.Contains(Path.VolumeSeparatorChar))
                    tmpdir = Path.Combine(AppDataDirectory, tmpdir);
                return tmpdir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Database Directory")]
        public string DatabaseDirectory
        {
            get
            {
                // get Property
                string databasedir = Properties.Settings.Default.Database_Directory;
                // replace Windows/Linux directory spearator chars
                databasedir = databasedir.Replace('\\', Path.DirectorySeparatorChar);
                databasedir = databasedir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(databasedir))
                    databasedir = "Database";
                // replace variables, if any
                databasedir = VC.ReplaceAllVars(databasedir);
                // remove directory separator chars at begin and end
                databasedir = databasedir.TrimStart(Path.DirectorySeparatorChar);
                databasedir = databasedir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!databasedir.Contains(Path.VolumeSeparatorChar))
                    databasedir = Path.Combine(AppDataDirectory, databasedir);
                return databasedir;
            }
        }


        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Tempfile Directory")]
        public string ExportDirectory
        {
            get
            {
                // get Property
                string expdir = Properties.Settings.Default.Export_Directory;
                // replace Windows/Linux directory spearator chars
                expdir = expdir.Replace('\\', Path.DirectorySeparatorChar);
                expdir = expdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(expdir))
                    expdir = "Tmp";
                // replace variables, if any
                expdir = VC.ReplaceAllVars(expdir);
                // remove directory separator chars at begin and end
                expdir = expdir.TrimStart(Path.DirectorySeparatorChar);
                expdir = expdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!expdir.Contains(Path.VolumeSeparatorChar))
                    expdir = Path.Combine(AppDataDirectory, expdir);
                return expdir;
            }
        }

        GMapOverlay Coverageoverlay = new GMapOverlay("Coveragepolygons");
        GMapOverlay Locationsoverlay = new GMapOverlay("Locations");

        DataSet SB = new DataSet();

        DataTableLocations Locations = new DataTableLocations();
        DataView LocationsView;

        DataTable QRV = new DataTable();
        DataView QRVView; 

        NumberFormatInfo ENprovider;

        public LogWriter Log = LogWriter.Instance;
        public VarConverter VC = new VarConverter();

        private bool IsMarkerDragging = false;
        private bool IsMarkerDragged = false;

        private GMarkerGoogle CurrentMarker = null;
        private double CurrentMarkerLat;
        private double CurrentMarkerLon;

        private double OfsLat, OfsLon;

        public MainDlg()
        {
            InitializeComponent();
            ENprovider = new NumberFormatInfo();
            ENprovider.NumberDecimalSeparator = ".";
            ENprovider.NumberGroupSeparator = ",";
            CheckDirectories();
            // set directories and formats for logfile
            ScoutBase.Core.Properties.Settings.Default.LogWriter_Directory = LogDirectory;
            ScoutBase.Core.Properties.Settings.Default.LogWriter_FileFormat = "Log_{0:yyyy_MM_dd}.log";
            ScoutBase.Core.Properties.Settings.Default.LogWriter_MessageFormat = "{0:u} {1}";
            Locations.RowChanged += new DataRowChangeEventHandler(Locations_Row_Changed);
            QRV.RowChanged += new DataRowChangeEventHandler(QRV_Row_Changed);
            tc_Main.Enabled = false;
        }

        private void MainDlg_Load(object sender, EventArgs e)
        {
            Log.WriteMessage("Starting up.");
            // set initial settings for CoverageMap
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            gm_Coverage.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Coverage.IgnoreMarkerOnMouseWheel = true;
            gm_Coverage.MinZoom = 0;
            gm_Coverage.MaxZoom = 20;
            gm_Coverage.Zoom = 6;
            gm_Coverage.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Coverage.CanDragMap = true;
            gm_Coverage.ScalePen = new Pen(Color.Black, 3);
            gm_Coverage.HelperLinePen = null;
            gm_Coverage.SelectionPen = null;
            gm_Coverage.MapScaleInfoEnabled = true;
            gm_Coverage.Overlays.Add(Coverageoverlay);
            // set initial settings for locations map
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            gm_Locations.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Locations.IgnoreMarkerOnMouseWheel = true;
            gm_Locations.MinZoom = 0;
            gm_Locations.MaxZoom = 20;
            gm_Locations.Zoom = 6;
            gm_Locations.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Locations.CanDragMap = true;
            gm_Locations.ScalePen = new Pen(Color.Black, 3);
            gm_Locations.HelperLinePen = null;
            gm_Locations.SelectionPen = null;
            gm_Locations.MapScaleInfoEnabled = true;
            gm_Locations.Overlays.Add(Locationsoverlay);
            gm_Locations.ShowCenter = false;
            // initialize QRV table
            DataColumn qrv_call = new DataColumn("Call", typeof(string));
            QRV.Columns.Add(qrv_call);
            DataColumn qrv_loc = new DataColumn("Loc", typeof(string));
            QRV.Columns.Add(qrv_loc);
            QRV.PrimaryKey = new DataColumn[2] { qrv_call, qrv_loc };
            string[] bands = Bands.GetStringValuesExceptNoneAndAll();
            foreach (string band in bands)
            {
                if (!band.StartsWith("50M") && !band.StartsWith("70M"))
                {
                    QRV.Columns.Add(band + "_AH", typeof(double));
                    QRV.Columns.Add(band + "_AG", typeof(double));
                    QRV.Columns.Add(band + "_P", typeof(double));
                }
            }
            QRV.Columns.Add("LastUpdated", typeof(DateTime));
            // initilize databases
            AircraftData.Database.GetDBLocation();
            StationData.Database.GetDBLocation();
            bw_DatabaseUpdater.RunWorkerAsync();
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            bw_QRZ.CancelAsync();
            Log.WriteMessage("Closing.");
        }

        public void CheckDirectories()
        {
            // check if directories exist
            if (!Directory.Exists(TmpDirectory))
                Directory.CreateDirectory(TmpDirectory);
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);
            if (!Directory.Exists(ExportDirectory))
                Directory.CreateDirectory(ExportDirectory);
        }


        private void Say(string text)
        {
            if (tsl_Status.Text != text)
            {
                tsl_Status.Text = text;
                ss_Main.Refresh();
            }
        }

        private void SayLocations(string text)
        {
            if (tb_Locations_Status.Text != text)
            {
                tb_Locations_Status.Text = text;
                tb_Locations_Status.Refresh();
                Application.DoEvents();
            }
        }

        private void SayQRV(string text)
        {
            if (tb_QRV_Status.Text != text)
            {
                tb_QRV_Status.Text = text;
                tb_QRV_Status.Refresh();
                Application.DoEvents();
            }
        }

        #region tp_General

        private void tp_General_Enter(object sender, EventArgs e)
        {
            tp_General_Update(this, null);
        }

        private void tp_General_Update(object sender, EventArgs e)
        {
            Coverageoverlay.Clear();
            // add tile to map polygons
            List<PointLatLng> l = new List<PointLatLng>();
            l.Add(new PointLatLng(tb_Coverage_MinLat.Value, tb_Coverage_MinLon.Value));
            l.Add(new PointLatLng(tb_Coverage_MinLat.Value, tb_Coverage_MaxLon.Value));
            l.Add(new PointLatLng(tb_Coverage_MaxLat.Value, tb_Coverage_MaxLon.Value));
            l.Add(new PointLatLng(tb_Coverage_MaxLat.Value, tb_Coverage_MinLon.Value));
            GMapPolygon p = new GMapPolygon(l, "Coverage");
            p.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            p.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            Coverageoverlay.Polygons.Add(p);
            // zoom the map
            gm_Coverage.SetZoomToFitRect(RectLatLng.FromLTRB(tb_Coverage_MinLon.Value - 1, tb_Coverage_MaxLat.Value + 1, tb_Coverage_MaxLon.Value + 1, tb_Coverage_MinLat.Value - 1));
        }


        #endregion

        #region tp_Locations

        private void tp_Locations_Enter(object sender, EventArgs e)
        {
            // clear map
            Locationsoverlay.Clear();
            Locations.Clear();
            Locations.Merge(StationData.Database.LocationToDataTable());
            Locations.AcceptChanges();
            LocationsView = new DataView(Locations);
            BindingSource source = new BindingSource();
            source.DataSource = LocationsView;
            dgv_Locations.DataSource = source;
            dgv_Locations.ShowRowErrors = true;
        }

        private void btn_QRZ_Start_Click(object sender, EventArgs e)
        {
            if (!bw_QRZ.IsBusy)
                bw_QRZ.RunWorkerAsync();
        }

        private void btn_QRZ_Stop_Click(object sender, EventArgs e)
        {
            bw_QRZ.CancelAsync();
        }

        private void btn_Locations_Sort_Click(object sender, EventArgs e)
        {
            // sort data table
            DataTableLocations sorted = (DataTableLocations)Locations.Clone();
            DataRow[] rows = Locations.Select("", "Call ASC");
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                    sorted.ImportRow(row);
            }
            Locations.Clear();
            foreach (DataRow row in sorted.Rows)
                Locations.ImportRow(row);
        }

        private void btn_Locations_Save_Click(object sender, EventArgs e)
        {
            SayLocations("Saving changes to database...");
            foreach (DataRow row in Locations.Rows)
            {
                LocationDesignator ld = new LocationDesignator(row);
                StationData.Database.LocationInsertOrUpdateIfNewer(ld);
            }
            SayLocations("Finished.");
        }

        private void btn_Locations_Export_Click(object sender, EventArgs e)
        {
            string filename = Path.Combine(ExportDirectory, "locations.json");
            SayLocations("Exporting database to " + filename);
            string json = StationData.Database.LocationToJSON();
            SupportFunctions.WriteStringToFile(json, filename);
            SayLocations("Finished.");
        }

        private void btn_Locations_Import_AirScout_Click(object sender, EventArgs e)
        {
        }

        private void btn_Locations_Import_CALL3_Click(object sender, EventArgs e)
        {
        }

        private void btn_Locations_Import_DTB_Click(object sender, EventArgs e)
        {
        }

        private void btn_Locations_Import_CSV_Click(object sender, EventArgs e)
        {
        }

        private void btn_Locations_Import_USR_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            Dlg.ShowNewFolderButton = false;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                DataTableLocations dt = new DataTableLocations();
                string[] files = Directory.GetFiles(Dlg.SelectedPath, "*.usr");
                foreach (string file in files)
                {
                    try
                    {
                        SayLocations("Importing " + file + "...");
                        string s = "";
                        using (StreamReader sr = new StreamReader(File.OpenRead(file)))
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
                                    if (Callsign.Check(call))
                                    {
                                        double lat = System.Convert.ToDouble(a[1], CultureInfo.InvariantCulture);
                                        double lon = System.Convert.ToDouble(a[2], CultureInfo.InvariantCulture);
                                        GEOSOURCE source = (MaidenheadLocator.IsPrecise(lat, lon, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC);
                                        string lastupdated = a[6];
                                        DateTime lu = System.Convert.ToDateTime(lastupdated).ToUniversalTime();
                                        if (GeographicalPoint.Check(lat, lon))
                                        {
                                            row["Call"] = call;
                                            row["Lat"] = lat;
                                            row["Lon"] = lon;
                                            row["Source"] = source;
                                            row["LastUpdated"] = lu;
                                            dt.Rows.Add(row);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString());
                    }
                }
                ImportLocations(dt);
            }
        }

        private void cb_Locations_ChangedOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Locations_ChangedOnly.Checked)
            {
                LocationsView.RowStateFilter = DataViewRowState.ModifiedCurrent | DataViewRowState.Added;
            }
            else
            {
                LocationsView.RowStateFilter = DataViewRowState.CurrentRows;
            }
        }

        private void gm_Locations_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CurrentMarker != null && CurrentMarker.IsMouseOver)
            {
                // get geographic coordinates of mouse pointer and calulate offsets
                PointLatLng p = gm_Locations.FromLocalToLatLng(e.X, e.Y);
                OfsLat = p.Lat - CurrentMarker.Position.Lat;
                OfsLon = p.Lng - CurrentMarker.Position.Lng;
                IsMarkerDragging = true;
                IsMarkerDragged = false;
                CurrentMarkerLat = CurrentMarker.Position.Lat;
                CurrentMarkerLon = CurrentMarker.Position.Lng;
                foreach (DataGridViewRow row in dgv_Locations.Rows)
                {
                    try
                    {
                        string call = row.Cells["Call"].Value.ToString();
                        string markercall = (string)CurrentMarker.Tag;
                        if (String.Equals(call, markercall))
                        {
                            dgv_Locations.ClearSelection();
                            row.Selected = true;
                            dgv_Locations.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void gm_Locations_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMarkerDragging && (CurrentMarker != null))
            {
                // get geographic coordinates of mouse pointer
                PointLatLng p = gm_Locations.FromLocalToLatLng(e.X, e.Y);
                p.Lat = p.Lat - OfsLat;
                p.Lng = p.Lng - OfsLon;
                CurrentMarker.Position = p;
                GPoint c = gm_Locations.FromLatLngToLocal(new PointLatLng(CurrentMarkerLat, CurrentMarkerLon));
                if ((Math.Abs(c.X - e.X) > 20) || (Math.Abs(c.Y - e.Y) > 20))
                {
                    IsMarkerDragged = true;
                }
            }
        }

        private void gm_Locations_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentMarker != null)
            {
                if (IsMarkerDragged)
                {
                    // get geographic coordinates of mouse pointer
                    PointLatLng p = gm_Locations.FromLocalToLatLng(e.X, e.Y);
                    double lat = p.Lat - OfsLat;
                    double lon = p.Lng - OfsLon;
                    string call = CurrentMarker.Tag.ToString();
                    string loc = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
                    GEOSOURCE source = (MaidenheadLocator.IsPrecise(lat, lon, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC);
                    DataRow oldrow = Locations.Rows.Find(new string[] { call, loc });
                    if (oldrow != null)
                    {
                        // call found --> check for update
                        if ((double)oldrow["Lat"] != lat)
                        {
                            oldrow["Lat"] = lat;
                            AddRowError(oldrow, "UPDATED", "Lat", "UpdatedValue", "OldValue:" + ((double)oldrow["Lat"]).ToString("F8", CultureInfo.InvariantCulture));
                        }
                        if ((double)oldrow["Lon"] != lon)
                        {
                            oldrow["Lon"] = lon;
                            AddRowError(oldrow, "UPDATED", "Lon", "UpdatedValue", "OldValue:" + ((double)oldrow["Lon"]).ToString("F8", CultureInfo.InvariantCulture));
                        }
                        oldrow["Source"] = source;
                        AddRowError(oldrow, "UPDATED", "Source", "UpdatedValue", "OldValue:" + oldrow["Source"].ToString());
                        oldrow["LastUpdated"] = DateTime.UtcNow;
                    }
                    else
                    {
                        // marker is mpved beyond old locator
                        // create new line
                        if (MessageBox.Show("Marker is moved to a different locator which is not in database so far. Create new entry?","Create new entry", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataRow row = Locations.NewRow();
                            row["Call"] = call;
                            row["Loc"] = loc;
                            row["Lat"] = lat;
                            row["Lon"] = lon;
                            row["Source"] = source;
                            row["Hits"] = 0;
                            row["LastUpdated"] = DateTime.UtcNow;
                            Locations.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    // restore original marker position
                    CurrentMarker.Position = new PointLatLng(CurrentMarkerLat, CurrentMarkerLon);
                }
            }
            gm_Locations.CanDragMap = true;
            IsMarkerDragging = false;
            IsMarkerDragged = false;
        }

        private void gm_Locations_OnMarkerEnter(GMapMarker item)
        {
            CurrentMarker = (GMarkerGoogle)item;
        }

        private void gm_Locations_OnMarkerLeave(GMapMarker item)
        {
            // CurrentMarker = null;
        }

        private void tb_Locations_Callsign_Filter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_Locations_Callsign_Filter.Text))
            {
                LocationsView.RowFilter = "Call LIKE '*'";
                return;
            }
            string filter = tb_Locations_Callsign_Filter.Text;
            if (!filter.EndsWith("*"))
                filter = filter + "*";
            LocationsView.RowFilter = "Call LIKE '" + filter + "'";
        }

        #endregion

        private void dgv_Locations_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.RowIndex >= dgv_Locations.Rows.Count))
                return;
            DataGridViewRow dgvrow = dgv_Locations.Rows[e.RowIndex];
            if ((e.ColumnIndex < 0) || (e.ColumnIndex >= dgvrow.Cells.Count))
                return;
            DataGridViewCell cell = dgvrow.Cells[e.ColumnIndex];
            if (!cell.Displayed)
                return;
            if (String.IsNullOrEmpty(dgvrow.ErrorText))
                return;
            XElement xml = XElement.Parse(dgvrow.ErrorText);
            LOCATIONSTATE state = LOCATIONSTATE.UNKNOWN;
            try
            {
                state = (LOCATIONSTATE)Enum.Parse(typeof(LOCATIONSTATE), xml.Name.ToString());
            }
            catch
            {

            }
            if (state == LOCATIONSTATE.ERROR)
                cell.Style.BackColor = Color.Red;
            else if (state == LOCATIONSTATE.LOCDIFF)
                cell.Style.BackColor = Color.Khaki;
            else if (state == LOCATIONSTATE.UPTODATE)
                cell.Style.BackColor = Color.LightGreen;
            else if (state == LOCATIONSTATE.ADDED)
                cell.Style.BackColor = Color.LightBlue;
            else if (state == LOCATIONSTATE.UPDATED)
            {
                string s = xml.ToString();
                string propertyname = dgv_Locations.Columns[e.ColumnIndex].DataPropertyName;
                if (s.IndexOf("<" + propertyname + " />") >= 0)
                {
                    cell.Style.BackColor = Color.Bisque;
                }
            }
        }

        private void bw_QRZ_DoWork(object sender, DoWorkEventArgs e)
        {
            // check callsign location against QRZ.com entry
            // name current thread
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "QRZ";
            int callschecked = 0;
            int callsfound = 0;
            int callsnotfound = 0;
            int callsuptodate = 0;
            int callsupdated = 0;
            int callsdiffloc = 0;
            int errors = 0;
            // get session key
            WebRequest myWebRequest = WebRequest.Create(Properties.Settings.Default.QRZ_URL_Login);
            myWebRequest.Timeout = 10000;
            WebResponse myWebResponse = myWebRequest.GetResponse();
            Stream ReceiveStream = myWebResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(ReceiveStream, encode);
            string s = readStream.ReadToEnd();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            var nodes = doc.GetElementsByTagName("Key");
            string sessionkey = nodes[0].InnerText;
            foreach (DataRow row in Locations.Rows)
            {
                try
                {
                    callschecked++;
                    string call = row["Call"].ToString();
                    double lat = (double)row["Lat"];
                    double lon = (double)row["Lon"];
                    GEOSOURCE source = (GEOSOURCE)row["Source"];
                    DateTime lastupdated = (DateTime)row["LastUpdated"];
                    string loc = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
                    string qrzloc = "";
                    double qrzlat = 0;
                    double qrzlon = 0;
                    string geoloc = "";
                    string addr1 = "";
                    string addr2 = "";
                    string zip = "";
                    string country = "";
                    string error = "";
                    // get xml data
                    myWebRequest = WebRequest.Create(Properties.Settings.Default.QRZ_URL_XMLData + "?s=" + sessionkey + ";callsign=" + call);
                    myWebRequest.Timeout = 10000;
                    myWebResponse = myWebRequest.GetResponse();
                    ReceiveStream = myWebResponse.GetResponseStream();
                    encode = System.Text.Encoding.GetEncoding("utf-8");
                    readStream = new StreamReader(ReceiveStream, encode);
                    s = readStream.ReadToEnd();
                    // load xml document
                    doc = new XmlDocument();
                    doc.LoadXml(s);
                    // check for errors
                    nodes = doc.GetElementsByTagName("Error");
                    if (nodes.Count > 0)
                    {
                        error = nodes[0].InnerText;
                        if (error.ToUpper().Contains("NOT FOUND"))
                        {
                            callsnotfound++;
                        }
                        else if (error.ToUpper().Contains("SESSION TIMEOUT"))
                        {
                            // session timeout --> obtain a new session key and try again
                            myWebRequest = WebRequest.Create(Properties.Settings.Default.QRZ_URL_Login);
                            myWebRequest.Timeout = 10000;
                            myWebResponse = myWebRequest.GetResponse();
                            ReceiveStream = myWebResponse.GetResponseStream();
                            encode = System.Text.Encoding.GetEncoding("utf-8");
                            readStream = new StreamReader(ReceiveStream, encode);
                            s = readStream.ReadToEnd();
                            doc = new XmlDocument();
                            doc.LoadXml(s);
                            nodes = doc.GetElementsByTagName("Key");
                            sessionkey = nodes[0].InnerText;
                            bw_QRZ.ReportProgress((int)LOCATIONSTATE.INFO, "Obtained new session key: " + sessionkey);
                            // get xml data
                            myWebRequest = WebRequest.Create(Properties.Settings.Default.QRZ_URL_XMLData + "?s=" + sessionkey + ";callsign=" + call);
                            myWebRequest.Timeout = 10000;
                            myWebResponse = myWebRequest.GetResponse();
                            ReceiveStream = myWebResponse.GetResponseStream();
                            encode = System.Text.Encoding.GetEncoding("utf-8");
                            readStream = new StreamReader(ReceiveStream, encode);
                            s = readStream.ReadToEnd();
                            // load xml document
                            doc = new XmlDocument();
                            doc.LoadXml(s);
                        }
                        else
                        {
                            // report error
                            errors++;
                            bw_QRZ.ReportProgress((int)LOCATIONSTATE.ERROR, error);
                        }
                    }
                    // write xml to file
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(call.Replace("/", "_") + ".xml"))
                        {
                            sw.WriteLine(s);
                        }
                        callsfound++;
                        nodes = doc.GetElementsByTagName("lat");
                        if (nodes.Count > 0)
                            qrzlat = System.Convert.ToDouble(nodes[0].InnerText, CultureInfo.InvariantCulture);
                        nodes = doc.GetElementsByTagName("lon");
                        if (nodes.Count > 0)
                            qrzlon = System.Convert.ToDouble(nodes[0].InnerText, CultureInfo.InvariantCulture);
                        nodes = doc.GetElementsByTagName("grid");
                        if (nodes.Count > 0)
                            qrzloc = nodes[0].InnerText.ToUpper().Trim();
                        nodes = doc.GetElementsByTagName("geoloc");
                        if (nodes.Count > 0)
                            geoloc = nodes[0].InnerText;
                        nodes = doc.GetElementsByTagName("addr1");
                        if (nodes.Count > 0)
                            addr1 = nodes[0].InnerText;
                        nodes = doc.GetElementsByTagName("addr2");
                        if (nodes.Count > 0)
                            addr2 = nodes[0].InnerText;
                        nodes = doc.GetElementsByTagName("zip");
                        if (nodes.Count > 0)
                            zip = nodes[0].InnerText;
                        nodes = doc.GetElementsByTagName("country");
                        if (nodes.Count > 0)
                            country = nodes[0].InnerText;
                        // different loc?
                        if (loc != qrzloc)
                        {
                            Log.WriteMessage("QRZ.COM: Locator is different [" + call + "]: " + loc + " <> " + qrzloc);
                            callsdiffloc++;
                        }
                        // precise location by user or geocode?
                        else if (geoloc.ToUpper().Contains("USER") || geoloc.ToUpper().Contains("GEOCODE"))
                        {
                            if ((qrzlat != lat) || (qrzlon != lon))
                            {
                                Log.WriteMessage("QRZ.COM: Location updated [" + call + "].");
                                callsupdated++;
                                LocationDesignator ld = new LocationDesignator(call, qrzlat, qrzlon, GEOSOURCE.FROMUSER);
                                bw_QRZ.ReportProgress((int)LOCATIONSTATE.UPDATED, ld);
                            }
                            else
                            {
                                // already up to date
                                Log.WriteMessage("QRZ.COM: Location up to date [" + call + "].");
                                callsuptodate++;
                            }
                        }
                        else if (geoloc.ToUpper().Contains("GRID"))
                        {
                            // try to get info by OpenStreetMaps API
                            string url = "https://nominatim.openstreetmap.org/search?q=" + addr1 + "+" + addr2 + "+" + zip + "+" + country + "&format=xml&polygon=1&addressdetails=1";
                            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                            httpWebRequest.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 61.0) Gecko / 20100101 Firefox / 61.0";
                            HttpWebResponse httpWebResponse= (HttpWebResponse)httpWebRequest.GetResponse();
                            ReceiveStream = httpWebResponse.GetResponseStream();
                            // encode = System.Text.Encoding.GetEncoding("utf-8");
                            readStream = new StreamReader(ReceiveStream, encode);
                            s = readStream.ReadToEnd();
                            // load xml document
                            doc = new XmlDocument();
                            doc.LoadXml(s);
                            double glat = 0;
                            double glon = 0;
                            string gloc = "";
                            nodes = doc.GetElementsByTagName("place");
                            if (nodes.Count > 0)
                            {
                                glat = System.Convert.ToDouble(nodes[0].Attributes["lat"].Value.ToString(), CultureInfo.InvariantCulture);
                                glon = System.Convert.ToDouble(nodes[0].Attributes["lon"].Value.ToString(), CultureInfo.InvariantCulture);
                            }
                            gloc = MaidenheadLocator.LocFromLatLon(glat, glon, false, 3);
                            if (gloc == qrzloc)
                            {
                                // precise location from address
                                Log.WriteMessage("QRZ.COM: Location updated from postal address[" + call + "].");
                                callsupdated++;
                                LocationDesignator ld = new LocationDesignator(call, glat, glon, GEOSOURCE.FROMUSER);
                                bw_QRZ.ReportProgress((int)LOCATIONSTATE.UPDATED, ld);
                            }
                            else
                            {
                                Log.WriteMessage("QRZ.COM: Locator is different [" + call + "]: " + loc + " <> " + qrzloc);
                                callsdiffloc++;
                            }
                        }
                        // alreadyup to date?
                        else if ((lat == qrzlat) && (lon == qrzlon))
                        {
                            callsuptodate++;
                        }
                    }
                    string status = "QRZ.COM query is running: " +
                        callschecked.ToString() + " checked, " +
                        callsnotfound.ToString() + " not found, " +
                        callsfound.ToString() + " found, " +
                        callsuptodate.ToString() + " up to date, " +
                        callsupdated.ToString() + " updated, " +
                        callsdiffloc.ToString() + " different loc, " +
                        errors.ToString() + " errors";
                bw_QRZ.ReportProgress((int)LOCATIONSTATE.INFO, status);
                    if (bw_QRZ.CancellationPending)
                        return;
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    errors++;
                    bw_QRZ.ReportProgress((int)LOCATIONSTATE.ERROR, ex.Message);
                    Log.WriteMessage(ex.Message);
                }
            }
        }

        private void bw_QRZ_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LOCATIONSTATE state = (LOCATIONSTATE)e.ProgressPercentage;
            if (state <= 0)
                SayLocations((string)e.UserState);
            else
            {
                LocationDesignator ld = (LocationDesignator)e.UserState;
                // update data table
                try
                {
                    DataRow row = Locations.Rows.Find(new string[2] { ld.Call, ld.Loc });
                    if (row != null)
                    {
                        if (state == LOCATIONSTATE.UPTODATE)
                            AddRowError(row, state.ToString(), "", "", "");
                        else if (state == LOCATIONSTATE.LOCDIFF)
                            AddRowError(row, state.ToString(), "", "", "");
                        else
                        {
                            if ((double)row["Lat"] != ld.Lat)
                            {
                                AddRowError(row, state.ToString(), "Lat", "UpdatedValue", "OldValue:" + ((double)row["Lat"]).ToString("F8", CultureInfo.InvariantCulture));
                                row["Lat"] = ld.Lat;
                            }
                            if ((double)row["Lon"] != ld.Lon)
                            {
                                AddRowError(row, state.ToString(), "Lon", "UpdatedValue", "OldValue:" + ((double)row["Lon"]).ToString("F8", CultureInfo.InvariantCulture));
                                row["Lon"] = ld.Lon;
                            }
                            row["Source"] = ld.Source;
                            row["LastUpdated"] = ld.LastUpdated;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.Message);
                }
            }
        }

        private void bw_QRZ_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SayLocations("Finished.");
        }


        private void AddRowError(DataRow row, string category, string node, string item, string text)
        {
            try
            {
                // create XML document if not already created
                if (String.IsNullOrEmpty(row.RowError))
                {
                    XElement x = new XElement(category);
                    row.RowError = x.ToString();
                }
                // read out Errors as XML from RowError
                XElement xml = XElement.Parse(row.RowError);
                if (String.IsNullOrEmpty(node))
                    return;
                xml.Add(new XElement(node));
                if (String.IsNullOrEmpty(item))
                    return;
                xml.Add(new XElement(item, text));
                row.RowError = xml.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
        }

        private void ImportLocations(DataTable dt)
        {
            if (dt != null)
            {
                int callsimported = dt.Rows.Count;
                int callsupdated = 0;
                int callsadded = 0;
                foreach (DataRow row in dt.Rows)
                {
                    DataRow oldrow = Locations.Rows.Find(row["Call"].ToString());
                    if (oldrow != null)
                    {
                        // call found --> check for update
                        if ((DateTime)row["LastUpdated"] > (DateTime)oldrow["LastUpdated"])
                        {
                            if (oldrow["Lat"] != row["Lat"])
                            {
                                oldrow["Lat"] = row["Lat"];
                                AddRowError(oldrow, "UPDATED", "Lat", "UpdatedValue", "OldValue:" + ((double)oldrow["Lat"]).ToString("F8", CultureInfo.InvariantCulture));
                            }
                            if (oldrow["Lon"] != row["Lon"])
                            {
                                oldrow["Lon"] = row["Lon"];
                                AddRowError(oldrow, "UPDATED", "Lon", "UpdatedValue", "OldValue:" + ((double)oldrow["Lon"]).ToString("F8", CultureInfo.InvariantCulture));
                            }
                            if (oldrow["Source"] != row["Source"])
                            {
                                oldrow["Source"] = row["Source"];
                                AddRowError(oldrow, "UPDATED", "Source", "UpdatedValue", "OldValue:" + oldrow["Source"].ToString());
                            }
                            oldrow["LastUpdated"] = row["LastUpdated"];
                            callsupdated++;
                        }
                    }
                    else
                    {
                        // add new row
                        AddRowError(row, LOCATIONSTATE.ADDED.ToString(), "", "", "");
                        Locations.ImportRow(row);
                        callsadded++;
                    }
                }
                SayLocations("Import of " + dt.TableName + " finished: " + callsimported.ToString() + " calls imported, " + callsadded.ToString() + " calls added, " + callsupdated.ToString() + " calls updated.");
            }
        }

        private void Locations_Row_Changed(object sender, DataRowChangeEventArgs e)
        {
            
            try
            {
                string call = e.Row["Call"].ToString();
                double lat = (double)e.Row["Lat"];
                double lon = (double)e.Row["Lon"];
                GEOSOURCE source = (GEOSOURCE)Enum.Parse(typeof(GEOSOURCE), e.Row["Source"].ToString());
                if (e.Action == DataRowAction.Add)
                {
                    GMarkerGoogle gm = new GMarkerGoogle(new PointLatLng(lat, lon), (source == GEOSOURCE.FROMUSER) ? GMarkerGoogleType.green_small : GMarkerGoogleType.white_small);
                    gm.ToolTipText = call;
                    gm.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    gm.Tag = call;
                    Locationsoverlay.Markers.Add(gm);
                }
                else if (e.Action == DataRowAction.Change)
                {
                    GMarkerGoogle gm = (GMarkerGoogle)Locationsoverlay.Markers.First(c => (string)c.Tag == call);
                    if (gm != null)
                        gm.Position = new PointLatLng(lat, lon);
                }
                else if (e.Action == DataRowAction.Delete)
                {
                    GMarkerGoogle gm = (GMarkerGoogle)Locationsoverlay.Markers.First(c => (string)c.Tag == call);
                    if (gm != null)
                        Locationsoverlay.Markers.Remove(gm);
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
        }

        private void btn_QRV_Import_WinTest_Click(object sender, EventArgs e)
        {
            DataTable dt = QRV.Clone();
            dt.Rows.Clear();
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.DefaultExt = ".xdt";
            Dlg.Filter = "Win-Test database|*.xdt";
            Dlg.CheckFileExists = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int qrv_ok = 0;
                    int qrv_err = 0;
                    using (StreamReader sr = new StreamReader(File.OpenRead(Dlg.FileName)))
                    {
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine().Trim();
                            string[] a = s.Split();
                            DataRow row = dt.NewRow();
                            row["Call"] = a[0];
                            SayQRV("Importing call: " + a[0] + "...");
                            DateTime lastupdated = DateTime.MinValue;
                            try
                            {
                                a[1] = a[1].Replace("[", "").Replace("]", "");
                                lastupdated = DateTime.ParseExact(a[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                                lastupdated = lastupdated.ToUniversalTime();
                            }
                            catch (Exception ex)
                            {
                                Log.WriteMessage(ex.ToString());
                                qrv_err++;
                            }
                            row["LastUpdated"] = lastupdated;
                            for (int i = 2; i < a.Length; i++)
                            {
                                a[i] = a[i].Trim();
                                if (!String.IsNullOrEmpty(a[i]))
                                {
                                    try
                                    {
                                        row[a[i] + "_AH"] = 0;
                                        row[a[i] + "_AG"] = 0;
                                        row[a[i] + "_P"] = 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.WriteMessage(ex.ToString());
                                        qrv_err++;
                                    }
                                }
                            }
                            dt.Rows.Add(row);
                            qrv_ok++;
                        }
                    }
                    QRV.Merge(dt);
                    SayQRV("Importing calls finished: " + qrv_ok + " calls, " + qrv_err + " error(s).");
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString());
                }
            }
        }

        private void tp_QRV_Enter(object sender, EventArgs e)
        {
            QRV.Rows.Clear();
            BAND[] bands = Bands.GetValuesExceptNoneAndAll();
            foreach (BAND band in bands)
            {
                if (band > BAND.B70M)
                {
                    SayQRV("Importing band: " + Bands.GetStringValue(band) + "...");
                    List<QRVDesignator> qrvs = StationData.Database.QRVGetAll(band);
                    string band_ah = Bands.GetStringValue(band) + "_AH";
                    string band_ag = Bands.GetStringValue(band) + "_AG";
                    string band_p = Bands.GetStringValue(band) + "_P";
                    if (qrvs != null)
                    {
                        foreach (QRVDesignator qrv in qrvs)
                        {
                            DataRow row = QRV.Rows.Find(new string[]{ qrv.Call, qrv.Loc});
                            if (row == null)
                            {
                                row = QRV.NewRow();
                                row["Call"] = qrv.Call;
                                row["Loc"] = qrv.Loc;
                                row["LastUpdated"] = DateTime.MinValue;
                                QRV.Rows.Add(row);
                            }
                            row[band_ah] = qrv.AntennaHeight;
                            row[band_ag] = qrv.AntennaGain;
                            row[band_p] = qrv.Power;
                            if ((DateTime)row["LastUpdated"] < qrv.LastUpdated)
                                row["LastUpdated"] = qrv.LastUpdated;
                        }
                    }
                }
            }
            QRV.AcceptChanges();
            QRVView = new DataView(QRV);
            BindingSource source = new BindingSource();
            source.DataSource = QRVView;
            dgv_QRV.DataSource = source;
            dgv_QRV.ShowRowErrors = true;
            dgv_QRV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < dgv_QRV.Columns.Count; i++)
            {
                if (i % 2 == 0)
                    dgv_QRV.Columns[i].DefaultCellStyle.BackColor = Color.LightGray;
            }
            SayQRV("Finished.");
        }

        private void tb_QRV_Callsign_Filter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_QRV_Callsign_Filter.Text))
            {
                QRVView.RowFilter = "Call LIKE '*'";
                return;
            }
            string filter = tb_QRV_Callsign_Filter.Text;
            if (!filter.EndsWith("*"))
                filter = filter + "*";
            QRVView.RowFilter = "Call LIKE '" + filter + "'";

        }

        private void btn_QRV_Sort_Click(object sender, EventArgs e)
        {
            // sort data table
            DataTable sorted = QRV.Clone();
            DataRow[] rows = QRV.Select("", "Call ASC");
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                    sorted.ImportRow(row);
            }
            QRV.Clear();
            foreach (DataRow row in sorted.Rows)
                QRV.ImportRow(row);
        }

        private void btn_QRV_Save_Click(object sender, EventArgs e)
        {
            SayQRV("Saving changes to database...");
            try
            {
                foreach (DataRow row in QRV.Rows)
                {
                    if ((row.RowState == DataRowState.Added) || (row.RowState == DataRowState.Modified))
                    { 
                        BAND[] bands = Bands.GetValuesExceptNoneAndAll();
                        foreach (BAND band in bands)
                        {
                            if (band > BAND.B70M)
                            {
                                string band_ah = Bands.GetStringValue(band) + "_AH";
                                string band_ag = Bands.GetStringValue(band) + "_AG";
                                string band_p = Bands.GetStringValue(band) + "_P";
                                QRVDesignator qrv = new QRVDesignator();
                                qrv.Call = row["Call"].ToString().ToUpper();
                                qrv.Loc = row["Loc"].ToString().ToUpper();
                                qrv.Band = band;
                                if ((row[band_ah].GetType() != typeof(DBNull)) && (row[band_ah].GetType() != typeof(DBNull)) && (row[band_ah].GetType() != typeof(DBNull)))
                                {
                                    qrv.AntennaHeight = (double)row[band_ah];
                                    qrv.AntennaGain = (double)row[band_ag];
                                    qrv.Power = (double)row[band_p];
                                    qrv.LastUpdated = (DateTime)row["LastUpdated"];
                                    SayQRV("Updating " + qrv.Call + ", " + qrv.Loc + "...");
                                    StationData.Database.QRVInsertOrUpdateIfNewer(qrv);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            SayQRV("Finished.");

        }

        private void btn_QRV_Export_Click(object sender, EventArgs e)
        {
            string filename = Path.Combine(ExportDirectory, "qrv.json");
            SayQRV("Exporting database to " + filename);
            string json = StationData.Database.QRVToJSON();
            SupportFunctions.WriteStringToFile(json, filename);
            SayQRV("Finished.");
        }

        private void QRV_Row_Changed(object sender, DataRowChangeEventArgs e)
        {

        }

        private void dgv_QRV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // update LastUpdated column in case of changes
            if (e.ColumnIndex < dgv_QRV.Columns.Count - 1)
            {
                dgv_QRV.Rows[e.RowIndex].Cells["LastUpdated"].Value = DateTime.UtcNow;
            }
        }

        private void dgv_QRV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btn_QRV_Import_EDI_Click(object sender, EventArgs e)
        {

        }

        private void cb_QRV_ChangedOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_QRV_ChangedOnly.Checked)
            {
                QRVView.RowStateFilter = DataViewRowState.ModifiedCurrent | DataViewRowState.Added;
            }
            else
            {
                QRVView.RowStateFilter = DataViewRowState.CurrentRows;
            }
        }

        private void btn_SFTP_GenerateFile_Click(object sender, EventArgs e)
        {
            // generates password file for SFTP and other
            if (String.IsNullOrEmpty(tb_SFTP_URL.Text) || String.IsNullOrEmpty(tb_SFTP_User.Text) || String.IsNullOrEmpty(tb_SFTP_Password.Text))
            {
                MessageBox.Show("Invalid entries for URL, user or password!");
                return;
            }
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.FileName = "airscout.pwd";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(Dlg.FileName,false))
                {
                    string s = tb_SFTP_URL.Text + "\t" + Encryption.EncryptString(tb_SFTP_User.Text) + "\t" + Encryption.EncryptString(tb_SFTP_Password.Text);
                    sw.WriteLine(s);
                }
            }
        }

        private void dgv_QRV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.Value != null) && (e.Value != DBNull.Value))
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }
        }

        private void btn_Update_Airlines_Click(object sender, EventArgs e)
        {
            try
            {
                string json = "";
                using (var client = new WebClient())
                {
                    json = client.DownloadString(Properties.Settings.Default.Airlines_Update_URL);
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.FloatFormatHandling = FloatFormatHandling.String;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                FR24Airlines fr24airlines = (FR24Airlines)JsonConvert.DeserializeObject<FR24Airlines>(json, settings);
                int count = 0;
                int errors = 0;
                foreach (FR24AirlineDesignator fr24ad in fr24airlines.rows)
                {
                    if (!String.IsNullOrEmpty(fr24ad.Code) && !String.IsNullOrEmpty(fr24ad.ICAO))
                    {
                        AirlineDesignator ad = new AirlineDesignator(fr24ad.ICAO, fr24ad.Code, fr24ad.Name, "[unknown]");
                        int result = AircraftData.Database.AirlineInsertOrUpdateIfNewer(ad);
                        if (result >= 0)
                            count += result;
                        else
                            errors++;
                    }
                }
                Say("Airlines updated from " + Properties.Settings.Default.Airlines_Update_URL + ": " + count.ToString() + " updated, " + errors.ToString() + " error(s).");
            }
            catch (Exception ex)
            {
                Say(ex.Message);
            }
        }

        private void btn_Update_Airports_Click(object sender, EventArgs e)
        {
            try
            {
                string json = "";
                using (var client = new WebClient())
                {
                    json = client.DownloadString(Properties.Settings.Default.Airports_Update_URL);
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.FloatFormatHandling = FloatFormatHandling.String;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                FR24Airports fr24airports = (FR24Airports)JsonConvert.DeserializeObject<FR24Airports>(json, settings);
                int count = 0;
                int errors = 0;
                foreach (FR24AirportDesignator fr24ad in fr24airports.rows)
                {
                    if (!String.IsNullOrEmpty(fr24ad.icao) && !String.IsNullOrEmpty(fr24ad.iata))
                    {
                        AirportDesignator ad = new AirportDesignator(fr24ad.icao, fr24ad.iata, fr24ad.lat, fr24ad.lon, fr24ad.alt, fr24ad.name.Replace("\t","").Replace("\r","").Replace("\n",""), fr24ad.country);
                        int result = AircraftData.Database.AirportInsertOrUpdateIfNewer(ad);
                        if (result >= 0)
                            count += result;
                        else
                            errors++;
                    }
                }
                Say("Airports updated from " + Properties.Settings.Default.Airports_Update_URL + ": " + count.ToString() + " updated, " + errors.ToString() + " error(s).");
            }
            catch (Exception ex)
            {
                Say(ex.Message);
            }

        }

        private void bw_AircraftUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            // Divide the earth surface into zones optimized for flights density
            List<string> world_zones = new List<string>(new string[] {
                "90, 70, -180, 180",
                "70, 50, -180, -20",
                "70, 50, -20, 0",
                "70, 50, 0, 20",
                "70, 50, 20, 40",
                "70, 50, 40, 180",
                "50, 30, -180, -120",
                "50, 40, -120, -110",
                "50, 40, -110, -100",
                "40, 30, -120, -110",
                "40, 30, -110, -100",
                "50, 40, -100, -90",
                "50, 40, -90, -80",
                "40, 30, -100, -90",
                "40, 30, -90, -80",
                "50, 30, -80, -60",
                "50, 30, -60, -40",
                "50, 30, -40, -20",
                "50, 30, -20, 0",
                "50, 40, 0, 10",
                "50, 40, 10, 20",
                "40, 30, 0, 10",
                "40, 30, 10, 20",
                "50, 30, 20, 40",
                "50, 30, 40, 60",
                "50, 30, 60, 180",
                "30, 10, -180, -100",
                "30, 10, -100, -80",
                "30, 10, -80, 100",
                "30, 10, 100, 180",
                "10, -10, -180, 180",
                "-10, -30, -180, 180",
                "-30, -90, -180, 180"
             } );
            while (!bw_AircraftUpdater.CancellationPending)
            {
                try
                { 
                    List<AircraftDesignator> aircrafts = new List<AircraftDesignator>();
                    int errors = 0;
                    foreach (string zone in world_zones)
                    {
                        string url = Properties.Settings.Default.Aircrafts_BaseURL + "&bounds=" + zone;
                        bw_AircraftUpdater.ReportProgress(0, "getting aircrafts from: " + url);
                        string json = "";
                        using (var client = new WebClient())
                        {
                            json = client.DownloadString(url);
                        }
                        // modify the JSON string to get a list of aircrafts
                        json = json.Substring(json.IndexOf(",") + 1);
                        json = json.Substring(json.IndexOf(",") + 1);
                        json = "{rows:{" + json;
                        json = json.Remove(json.IndexOf(",\"stats\""));
                        json = json + "}}";
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        settings.FloatFormatHandling = FloatFormatHandling.String;
                        settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        FR24Aircrafts fr24aircrafts = (FR24Aircrafts)JsonConvert.DeserializeObject<FR24Aircrafts>(json, new FR24AircraftConverter());
                        foreach (FR24AircraftDesignator fr24ad in fr24aircrafts.rows.Values)
                        {
                            try
                            {
                                AircraftDesignator ad = new AircraftDesignator();
                                ad.Hex = fr24ad.hex;
                                ad.Call = fr24ad.call;
                                ad.Reg = fr24ad.reg;
                                ad.TypeCode = fr24ad.typecode;
                                ad.LastUpdated = DateTime.UtcNow;
                                if (PlaneInfoChecker.Check_Hex(ad.Hex) && PlaneInfoChecker.Check_Call(ad.Call) && PlaneInfoChecker.Check_Reg(ad.Reg) && PlaneInfoChecker.Check_Type(ad.TypeCode))
                                    aircrafts.Add(ad);
                                else
                                {
//                                    Console.WriteLine("Invalid aircraft data: " + fr24ad.hex + "," + fr24ad.call + "," + fr24ad.reg + "," + fr24ad.typecode);
                                    errors++;
                                }
                            }
                            catch (Exception ex)    
                            {
                                bw_AircraftUpdater.ReportProgress(-1, ex.Message);
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    bw_AircraftUpdater.ReportProgress(0, "Updating aircrafts...");
                    // update aircraft data
                    AircraftData.Database.AircraftBulkInsertOrUpdateIfNewer(aircrafts);
                    bw_AircraftUpdater.ReportProgress(0, "Aircafts updated from " + Properties.Settings.Default.Aircrafts_BaseURL + ": " + aircrafts.Count.ToString() + " updated, " + errors.ToString() + " error(s).");
                    int timeout = 0;
                    while (!bw_AircraftUpdater.CancellationPending && (timeout < 600))
                    {
                        Thread.Sleep(1000);
                        timeout++;
                    }
                }
                catch (Exception ex)
                {
                    bw_AircraftUpdater.ReportProgress(-1, ex.Message);
                }
            }
        }   

        private void bw_AircraftUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
                Say((string)(e.UserState));
            else if (e.ProgressPercentage == 0)
                Say((string)(e.UserState));
            ReportAircraftsStats();
        }

        private void bw_AircraftUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_Update_Aicrafts_Start.Enabled = true;
            btn_Update_Aircrafts_Stop.Enabled = false;
        }

        private void ReportAircraftsStats()
        {
            lbl_Aircrafts_Total.Text = "total: " + AircraftData.Database.AircraftCount().ToString();
            lbl_Aircrafts_UnkownHex.Text = "unknown hex: " + AircraftData.Database.AircraftCountUnknownHex().ToString();
            lbl_Aircrafts_UnkownCall.Text = "unknown call: " + AircraftData.Database.AircraftCountUnknownCall().ToString();
            lbl_Aircrafts_UnkownType.Text = "unknown type: " + AircraftData.Database.AircraftCountUnknownType().ToString();
        }

        private void btn_Update_Aicrafts_Start_Click(object sender, EventArgs e)
        {
            bw_AircraftUpdater.RunWorkerAsync();
            while (!bw_AircraftUpdater.IsBusy)
                Application.DoEvents();
            btn_Update_Aicrafts_Start.Enabled = false;
            btn_Update_Aircrafts_Stop.Enabled = true;
        }

        private void btn_Update_Aircrafts_Stop_Click(object sender, EventArgs e)
        {
            bw_AircraftUpdater.CancelAsync();
        }

        private void tp_Aircrafts_Enter(object sender, EventArgs e)
        {
            if (bw_AircraftUpdater.IsBusy)
            {
                btn_Update_Aicrafts_Start.Enabled = false;
                btn_Update_Aircrafts_Stop.Enabled = true;
            }
            else
            {
                btn_Update_Aicrafts_Start.Enabled = true;
                btn_Update_Aircrafts_Stop.Enabled = false;
            }

            ReportAircraftsStats();
        }

        private void btn_StationDatabase_Export_Click(object sender, EventArgs e)
        {
            // export and upload station database
            if (!SupportFunctions.ValidateDirectoryPath(Properties.Settings.Default.StationDatabase_Export_LocalDir))
            {
                MessageBox.Show("Local Path is not valid: " + Properties.Settings.Default.StationDatabase_Export_LocalDir, " Export Station Database");
                return;
            }
            Say("Getting locations...");
            string locations = StationData.Database.LocationToJSON();
            string locationsfile = Path.Combine(Properties.Settings.Default.StationDatabase_Export_LocalDir, "locations.json");
            string locationszipfile = Path.Combine(Properties.Settings.Default.StationDatabase_Export_LocalDir, "locations.zip");
            SupportFunctions.WriteStringToFile(locations, locationsfile);
            Say("Creating zip file...");
            ZIP.CompressFile(locationsfile, false, 60);
            string qrvs = StationData.Database.QRVToJSON();
            Say("Getting qrv information...");
            string qrvfile = Path.Combine(Properties.Settings.Default.StationDatabase_Export_LocalDir, "qrv.json");
            string qrvzipfile = Path.Combine(Properties.Settings.Default.StationDatabase_Export_LocalDir, "qrv.zip");
            SupportFunctions.WriteStringToFile(qrvs, qrvfile);
            Say("Creating zip file...");
            ZIP.CompressFile(qrvfile, false, 60);
            Say("Upload files...");
            SftpClient client = new SftpClient(Properties.Settings.Default.StationDatabase_Export_RemoteHost, Properties.Settings.Default.StationDatabase_Export_User, Properties.Settings.Default.StationDatabase_Export_Password);
            try
            {
                client.Connect();
                using (FileStream file = new FileStream(locationszipfile, FileMode.Open))
                {
                    string uploadfile = Properties.Settings.Default.StationDatabase_Export_RemoteDir + "/" + "locations.zip";
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(file, uploadfile, true);
                }
                using (FileStream file = new FileStream(qrvzipfile, FileMode.Open))
                {
                    string uploadfile = Properties.Settings.Default.StationDatabase_Export_RemoteDir + "/" + "qrv.zip";
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(file, uploadfile, true);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Say("Station database exported and uploaded.");
        }

        private void btn_AircraftDatabase_Export_Click(object sender, EventArgs e)
        {
            // export and upload aircraftdatabase
            if (!SupportFunctions.ValidateDirectoryPath(Properties.Settings.Default.AircraftDatabase_Export_LocalDir))
            {
                MessageBox.Show("Local Path is not valid: " + Properties.Settings.Default.AircraftDatabase_Export_LocalDir, " Export Aircraft Database");
                return;
            }
            Say("Getting aircraft registrations...");
            string ars = AircraftData.Database.AircraftRegistrationToJSON();
            string arsfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "AircraftRegistrations.json");
            string arszipfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "AircraftRegistrations.zip");
            SupportFunctions.WriteStringToFile(ars, arsfile);
            Say("Creating zip file...");
            ZIP.CompressFile(arsfile, false, 60);
            Say("Getting aircrafts...");
            string acs = AircraftData.Database.AircraftToJSON();
            string acsfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "Aircrafts.json");
            string acszipfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "Aircrafts.zip");
            SupportFunctions.WriteStringToFile(acs, acsfile);
            Say("Creating zip file...");
            ZIP.CompressFile(acsfile, false, 60);
            Say("Getting aircraft typess...");
            string ats = AircraftData.Database.AircraftTypeToJSON();
            string atsfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "AircraftTypes.json");
            string atszipfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "AircraftTypes.zip");
            SupportFunctions.WriteStringToFile(ats, atsfile);
            Say("Creating zip file...");
            ZIP.CompressFile(atsfile, false, 60);
            Say("Getting airlines...");
            string als = AircraftData.Database.AirlineToJSON();
            string alsfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "Airlines.json");
            string alszipfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "Airlines.zip");
            SupportFunctions.WriteStringToFile(als, alsfile);
            Say("Creating zip file...");
            ZIP.CompressFile(alsfile, false, 60);
            Say("Getting airports...");
            string aps = AircraftData.Database.AirportToJSON();
            string apsfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "Airports.json");
            string apszipfile = Path.Combine(Properties.Settings.Default.AircraftDatabase_Export_LocalDir, "Airports.zip");
            SupportFunctions.WriteStringToFile(aps, apsfile);
            Say("Creating zip file...");
            ZIP.CompressFile(apsfile, false, 60);
            Say("Upload files...");
            SftpClient client = new SftpClient(Properties.Settings.Default.AircraftDatabase_Export_RemoteHost, Properties.Settings.Default.AircraftDatabase_Export_User, Properties.Settings.Default.AircraftDatabase_Export_Password);
            try
            {
                client.Connect();
                using (FileStream file = new FileStream(arszipfile, FileMode.Open))
                {
                    string uploadfile = Properties.Settings.Default.AircraftDatabase_Export_RemoteDir + "/" + "AircraftRegistrations.zip";
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(file, uploadfile, true);
                }
                using (FileStream file = new FileStream(acszipfile, FileMode.Open))
                {
                    string uploadfile = Properties.Settings.Default.AircraftDatabase_Export_RemoteDir + "/" + "Aircrafts.zip";
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(file, uploadfile, true);
                }
                using (FileStream file = new FileStream(atszipfile, FileMode.Open))
                {
                    string uploadfile = Properties.Settings.Default.AircraftDatabase_Export_RemoteDir + "/" + "AircraftTypes.zip";
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(file, uploadfile, true);
                }
                using (FileStream file = new FileStream(alszipfile, FileMode.Open))
                {
                    string uploadfile = Properties.Settings.Default.AircraftDatabase_Export_RemoteDir + "/" + "Airlines.zip";
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(file, uploadfile, true);
                }
                using (FileStream file = new FileStream(apszipfile, FileMode.Open))
                {
                    string uploadfile = Properties.Settings.Default.AircraftDatabase_Export_RemoteDir + "/" + "Airports.zip";
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(file, uploadfile, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Say("Aircraft database exported and uploaded.");
        }

        private void dgv_Locations_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = dgv_Locations.SelectedRows;
            if (!IsMarkerDragging && (rows.Count > 0))
            {
                try
                {
                    // clear locations
                    Locationsoverlay.Clear();
                    double minlat = double.MaxValue;
                    double maxlat = double.MinValue;
                    double minlon = double.MaxValue;
                    double maxlon = double.MinValue;
                    foreach (DataGridViewRow row in rows)
                    {
                        // get info
                        string call = row.Cells["Call"].Value.ToString();
                        double lat = (double)row.Cells["Lat"].Value;
                        double lon = (double)row.Cells["Lon"].Value;
                        GEOSOURCE source = (GEOSOURCE)row.Cells["Source"].Value;
                        // add location
                        GMarkerGoogle gm = new GMarkerGoogle(new PointLatLng(lat, lon), (source == GEOSOURCE.FROMUSER) ? GMarkerGoogleType.green_small : GMarkerGoogleType.white_small);
                        gm.ToolTipText = call;
                        gm.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                        gm.Tag = call;
                        Locationsoverlay.Markers.Add(gm);
                        if (minlat > lat)
                            minlat = lat;
                        if (maxlat < lat)
                            maxlat = lat;
                        if (minlon > lon)
                            minlon = lon;
                        if (maxlon < lon)
                            maxlon = lon;
                    }
                    // ensure that all location are visible
                    if (rows.Count > 1)
                    {
                        gm_Locations.SetZoomToFitRect(RectLatLng.FromLTRB(minlon, maxlat, maxlon, minlat));
                    }
                    else
                    {
                        // set standard zoom if only 1 location
                        gm_Locations.Zoom = 15;
                        gm_Locations.Position = new PointLatLng(minlat,minlon);
                    }
                }
                catch (Exception ex)
                {
                    // cannot set position -- > do nothing
                }
            }
        }

    }

    public class FR24Airlines
    {
        public int version;
        public FR24AirlineDesignator[] rows;

        public FR24Airlines()
        {
            version = 0;
            rows = null;
        }
    }

    public class FR24AirlineDesignator
    {
        public string Name;
        public string Code;
        public string ICAO;

        public FR24AirlineDesignator()
        {
            Name = "";
            Code = "";
            ICAO = "";
        }
    }

    public class FR24Airports
    {
        public int version;
        public FR24AirportDesignator[] rows;

        public FR24Airports()
        {
            version = 0;
            rows = null;
        }
    }

    public class FR24AirportDesignator
    {
        public string name;
        public string iata;
        public string icao;
        public string city;
        public double lat;
        public double lon;
        public string country;
        public double alt;
        public double size;

        public FR24AirportDesignator()
        {
            name = "";
            iata = "";
            icao = "";
            city = "";
            lat = 0;
            lon = 0;
            country = "";
            alt = 0;
            size = 0;
        }
    }

    public class FR24Aircrafts
    {
        public Dictionary<string, FR24AircraftDesignator> rows;
        public FR24Aircrafts()
        {
            rows = null;
        }
    }

    public class FR24AircraftDesignator
    {
        public string hex;
        public double lat;
        public double lon;
        public int track;
        public int alt;
        public int speed;
        public string squawk;
        public string radar;
        public string typecode;
        public string reg;
        public long time;
        public string src;
        public string dst;
        public string call;
        public int dummy1;
        public int dummy2;
        public string flight;
        public int dummy3;
        public string airline;

        public FR24AircraftDesignator()
        {
            hex = "";
            lat = 0;
            lon = 0;
            track = 0;
            alt = 0;
            speed = 0;
            squawk = "";
            radar = "";
            typecode = "";
            reg = "";
            time = 0;
            src = "";
            dst = "";
            call = "";
            dummy1 = 0;
            dummy2 = 0;
            flight = "";
            dummy3 = 0;
            airline = "";
        }
    }

    class FR24AircraftConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(FR24AircraftDesignator));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            return new FR24AircraftDesignator
            {
                hex = (string)array[0],
                lat = (double)array[1],
                lon = (double)array[2],
                track = (int)array[3],
                alt = (int)array[4],
                speed = (int)array[5],
                squawk = (string)array[6],
                radar = (string)array[7],
                typecode = (string)array[8],
                reg = (string)array[9],
                time = (long)array[10],
                src = (string)array[11],
                dst = (string)array[12],
                flight = (string)array[13],
                dummy1 = (int)array[14],
                dummy2 = (int)array[15],
                call = (string)array[16],
                dummy3 = (int)array[17],
                airline = (string)array[18]
            };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    public enum DATABASEUPDATERSTARTOPTIONS
    {
        NONE = 0,
        FIRSTRUN = 1,
        RUNONCE = 2,
        RUNPERIODICALLY = 3
    }

    public enum LOCATIONSTATE
    {
        UNKNOWN = -2,
        ERROR = -1,
        INFO = 0,
        UPTODATE = 1,
        UPDATED = 2,
        LOCDIFF = 3,
        ADDED = 4
    }
}
