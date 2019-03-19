using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.IO;
using AirScout.Aircrafts;
using AirScout.PlaneFeeds.Generic;
using LibADSB;
using ScoutBase.Core;

namespace AirScout.PlaneFeeds
{
    public class RTLMessage
    {
        public string RawMessage = "";
        public DateTime TimeStamp = DateTime.UtcNow;
        public int SignalStrength = 0;
    }

    public class PlaneFeedSettings_RTL
    {
        [Browsable(true)]
        [DescriptionAttribute("Server address for raw ADS-B data.\nUse localhost for running on the same machine.")]
        public virtual string Server
        {
            get
            {
                return Properties.Settings.Default.RTL_Server;
            }
            set
            {
                Properties.Settings.Default.RTL_Server = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(true)]
        [DescriptionAttribute("Server port for raw ADS-B data.\nRTLSharp.exe: Port 47806\nRTL1090: Port 31001")]
        public virtual int Port
        {
            get
            {
                return Properties.Settings.Default.RTL_Port;
            }
            set
            {
                Properties.Settings.Default.RTL_Port = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(true)]
        [DescriptionAttribute("Interval for updating ADS-B data [sec].")]
        public virtual int Interval
        {
            get
            {
                return Properties.Settings.Default.RTL_Interval;
            }
            set
            {
                Properties.Settings.Default.RTL_Interval = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(true)]
        [DescriptionAttribute("Use binary data format for ADS-B data.\nTrue: Use binary format (ADS Beast with MLAT)\nFalse: Use ASCII format (AVR with/without MLAT)")]
        public virtual bool Binary
        {
            get
            {
                return Properties.Settings.Default.RTL_Binary;
            }
            set
            {
                Properties.Settings.Default.RTL_Binary = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(true)]
        [DescriptionAttribute("Report ADS-B messages and show in status line.")]
        public virtual bool ReportMessages
        {
            get
            {
                return Properties.Settings.Default.RTL_Report_Messages;
            }
            set
            {
                Properties.Settings.Default.RTL_Report_Messages = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(true)]
        [DescriptionAttribute("Marks locally received aircrafts by adding '@' to the call sign")]
        public virtual bool MarkLocal
        {
            get
            {
                return Properties.Settings.Default.RTL_MarkLocal;
            }
            set
            {
                Properties.Settings.Default.RTL_MarkLocal = value;
                Properties.Settings.Default.Save();
            }
        }
    }

    public class PlaneFeed_RTL : PlaneFeed
    {
        [Browsable(false)]
        public override string Name
        {
            get
            {
                return Properties.Settings.Default.RTL_Name; ;
            }
            protected set
            {
                Properties.Settings.Default.RTL_Name = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Disclaimer
        {
            get
            {
                return Properties.Settings.Default.RTL_Disclaimer;
            }
            protected set
            {
                Properties.Settings.Default.RTL_Disclaimer = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string DisclaimerAccepted
        {
            get
            {
                return Properties.Settings.Default.RTL_Disclaimer_Accepted;
            }
            set
            {
                Properties.Settings.Default.RTL_Disclaimer_Accepted = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Info
        {
            get
            {
                return Properties.Settings.Default.RTL_Info;
            }
            protected set
            {
                Properties.Settings.Default.RTL_Info = value;
                Properties.Settings.Default.Save();
            }
        }

        public new PlaneFeedSettings_RTL FeedSettings = new PlaneFeedSettings_RTL();

        public PlaneFeed_RTL()
            : base ()
        {
            HasSettings = true;
        }

        private RTLMessage ReceiveBinaryMsg(Stream stream)
        {
            // read Mode-S beast binary input
            string RTL = null;
            int signal_strength = 0;
            long nanosec = 0;
            long daysec = 0;
            DateTime timestamp = DateTime.UtcNow;
            byte[] buffer = new byte[23];
            // wait for escape character
            DateTime start = DateTime.UtcNow;
            DateTime stop = DateTime.Now;
            do 
            {
                stream.Read(buffer,0,1);
//                System.Console.WriteLine(BitConverter.ToString(buffer,0,1));
                if (buffer[0] == 0x1A)
                {
                    // read next character
                    stream.Read(buffer,1,1);
                    switch (buffer[1])
                    {
                        case 0x31:
                            // do not decode
                            RTL = null;
                            break;
                        case 0x32:
                            // 7 byte short frame
                            // read timestamp
                            stream.Read(buffer, 2, 6);
                            nanosec = ((buffer[4] & 0x3f) << 24) | 
                                (buffer[5] << 16) | 
                                (buffer[6] << 8)  | 
                                (buffer[7]);
                            daysec =  (buffer[2] << 10) | 
                                (buffer[3] << 2) | 
                                (buffer[4] >> 6);
                            timestamp = DateTime.Today.AddSeconds(daysec);
                            timestamp = timestamp.AddMilliseconds(nanosec / 1000);
                            // plausibility check
                            if (Math.Abs((DateTime.Now - timestamp).Seconds) > 10)
                            {
                                // time difference > 10sec --> discard timestamp
                                timestamp = DateTime.UtcNow;
                            }
                            // read signal strength
                            stream.Read(buffer,8,1);
                            // plausibility check
                            if (Math.Abs((DateTime.Now - timestamp).Seconds) > 10)
                            {
                                // time difference > 10sec --> discard timestamp
                                timestamp = DateTime.UtcNow;
                            }
                            signal_strength = buffer[8];
                            // read frame
                            stream.Read(buffer,9,7);
                            // convert to AVR string
                            RTL = BitConverter.ToString((byte[])buffer,9,7).Replace("-", String.Empty);
                            RTL = "*" + RTL + ";";
                            break;
                        case 0x33:
                            // 14 byte long frame
                            // read timestamp
                            stream.Read(buffer, 2, 6);
                            nanosec = ((buffer[4] & 0x3f) << 24) | 
                                (buffer[5] << 16) | 
                                (buffer[6] << 8)  | 
                                (buffer[7]);
 
                            daysec =  (buffer[2] << 10) | 
                                (buffer[3] << 2) | 
                                (buffer[4] >> 6);
                            timestamp = DateTime.Today.AddSeconds(daysec);
                            timestamp = timestamp.AddMilliseconds(nanosec / 1000);
                            // plausibility check
                            if (Math.Abs((DateTime.Now - timestamp).Seconds) > 10)
                            {
                                // time difference > 10sec --> discard timestamp
                                timestamp = DateTime.UtcNow;
                            }
                            // read signal strength
                            stream.Read(buffer, 8, 1);
                            signal_strength = buffer[8];
                            // read frame
                            stream.Read(buffer, 9, 14);
                            // convert to AVR string
                            RTL = BitConverter.ToString((byte[])buffer,9,14).Replace("-", String.Empty);
                            RTL = "*" + RTL + ";";
                            break;
                        default: 
                            // false decode
                            RTL = null;
                            break;
                    }
                }
                // check for timeout 10sec
                stop = DateTime.UtcNow;
                if (stop - start > new TimeSpan(0, 0, 10))
                    throw new TimeoutException();
            }
            while ((RTL == null) && !this.CancellationPending);
            if (RTL == null)
                return null;
            RTLMessage msg = new RTLMessage();
            msg.RawMessage = RTL;
            msg.TimeStamp = timestamp;
            msg.SignalStrength = signal_strength;
            return msg;
        }

        private RTLMessage ReceiveAVRMsg ( StreamReader sr)
        {
            RTLMessage msg = new RTLMessage();
            // read AVR format input
            msg.RawMessage = sr.ReadLine();
            if (msg.RawMessage.StartsWith("*"))
            {
                // standard AVR message
                // no timestamp in telegram --> set timestamp after reading
                msg.TimeStamp = DateTime.UtcNow;
                msg.SignalStrength = 0;
            }
            else if (msg.RawMessage.StartsWith("@"))
            {
                // extended AVR message wit MLAT
                // convert into standard message format
                // extract time string
                string time = msg.RawMessage.Substring(1, 12);
                // TODO: interprete the MLAT timestamp!
                msg.TimeStamp = DateTime.UtcNow;
                msg.RawMessage = "*" + msg.RawMessage.Remove(0, 13);
            }
            return msg;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
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
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Status = STATUS.OK;
                ADSBDecoder decoder = new ADSBDecoder();
                DateTime lastreported = DateTime.UtcNow;
                StreamReader sr = null;
                TcpClient client = null;
                RTLMessage msg = null;
                // outer loop
                do
                {
                    try
                    {
                        // setup TCP listener
                        client = new TcpClient();
                        string server = Properties.Settings.Default.RTL_Server;
                       client.Connect(server, Properties.Settings.Default.RTL_Port);
                        sr = new StreamReader(client.GetStream());
                        // inner loop
                        // receive messages in a loop
                        do
                        {
                            if (FeedSettings.Binary)
                                msg = ReceiveBinaryMsg(sr.BaseStream);
                            else
                                msg = ReceiveAVRMsg(sr);
                            // decode the message
                            if (msg.RawMessage.StartsWith("*") && msg.RawMessage.EndsWith(";"))
                            {
                                // try to decode the message
                                string info = "";
                                try
                                {
                                    Console.Write(msg.RawMessage + " --> ");
                                    info = decoder.DecodeMessage(msg.RawMessage, msg.TimeStamp);
                                    // report messages to main window if activated
                                    if (FeedSettings.ReportMessages && (info.StartsWith("[")))
                                        this.ReportProgress((int)PROGRESS.STATUS, info);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                Console.WriteLine(info);
                            }
                            DateTime stop = DateTime.UtcNow;
                            // check if update report is necessary
                            if ((DateTime.UtcNow - lastreported).TotalSeconds > FeedSettings.Interval)
                            {
                                lastreported = DateTime.UtcNow;
                                // time to report planes
                                ArrayList list = decoder.GetPlanes();
                                if (list.Count > 0)
                                {
                                    // convert to plane info list
                                    List<PlaneInfo> planes = new List<PlaneInfo>();
                                    foreach (ADSBInfo info in list)
                                    {
                                        PlaneInfo plane = new PlaneInfo();
                                        plane.Time = info.Timestamp;
                                        plane.Hex = info.ICAO24;
                                        // mark call with "@" if option is enabled
                                        plane.Call = (FeedSettings.MarkLocal) ? "@" + info.Call : info.Call;
                                        plane.Lat = info.Lat;
                                        plane.Lon = info.Lon;
                                        plane.Alt = info.Alt;
                                        plane.Speed = info.Speed;
                                        plane.Track = info.Heading;
                                        plane.Reg = "[unknown]";
                                        plane.Type = "[unknown]";
                                        plane.Manufacturer = "[unknown]";
                                        plane.Model = "[unknown]";
                                        plane.Category = PLANECATEGORY.NONE;
                                        // try to get the registration and type
                                        AircraftDesignator aircraft = AircraftData.Database.AircraftFindByHex(plane.Hex);
                                        if (aircraft != null)
                                        {
                                            plane.Reg = aircraft.Reg;
                                            plane.Type = aircraft.TypeCode;
                                            // try to get the type
                                            AircraftTypeDesignator type = AircraftData.Database.AircraftTypeFindByICAO(plane.Type);
                                            if (plane != null)
                                            {
                                                plane.Manufacturer = type.Manufacturer;
                                                plane.Model = type.Model;
                                                plane.Category = type.Category;
                                            }
                                        }
                                        planes.Add(plane);
                                    }
                                    ReportProgress((int)PROGRESS.PLANES, planes);
                                    AircraftData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(planes);
                                    string message = "[" + lastreported.ToString("HH:mm:ss") + "] " +
                                        decoder.Count.ToString() + " Positions updated from local ADS-B receiver.";
                                    this.ReportProgress((int)PROGRESS.STATUS, message);
                                }
                            }
                        }
                        while ((msg != null) && !CancellationPending);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.Message);
                        this.ReportProgress((int)PROGRESS.ERROR, "Error reading from TCP connection: " + ex.Message);
                        Thread.Sleep(10000);
                    }
                    finally
                    {
                        // try to close the stream and TCP client
                        try
                        {
                            if (sr != null)
                                sr.Close();
                        }
                        catch
                        {
                        }
                        try
                        {
                            if (client != null)
                                client.Close();
                        }
                        catch
                        {
                        }
                    }
                }
                while (!this.CancellationPending || (Status != STATUS.OK));
            }
            this.ReportProgress((int)PROGRESS.FINISHED);
            Log.WriteMessage("Finished.");
        }

        public override Object GetFeedSettings()
        {
            return FeedSettings;
        }

    }
}
