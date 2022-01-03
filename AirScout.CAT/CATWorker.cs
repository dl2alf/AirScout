using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using ScoutBase.CAT;

namespace AirScout.CAT
{

    public class CATWorkerStartOptions
    {
        public string Name;
        public string RigType = "";
        public string PortName = "";
        public int Baudrate = 9600;
        public int DataBits = 8;
        public PARITY Parity = PARITY.NONE;
        public STOPBITS StopBits = STOPBITS.ONE;
        public FLOWCONTROL RTS = FLOWCONTROL.LOW;
        public FLOWCONTROL DTR = FLOWCONTROL.LOW;
        public int Poll = 100;
        public int Timeout = 500;
    }

    public class CATWorker : BackgroundWorker
    {

        public CATWorkerStartOptions StartOptions;
        public IRig Rig = null;

        // doppler tracking
        public DOPPLERSTRATEGY DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_NONE;
        public long RxFrequency = 0;
        public long TxFrequency = 0;

        public CATWorker()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
        }

        #region OmniRig

        #endregion

        // provides a list of supported rigs
        public static List<SupportedRig> SupportedRigs()
        {
            List<SupportedRig> rigs = new List<SupportedRig>();
            // try to get external OmniRig supported rigs
            try
            {
                List<SupportedRig> omnirigs = OmniRigX.SupportedRigs();
                foreach (SupportedRig omnirig in omnirigs)
                {
                    rigs.Add(omnirig);
                }
            }
            catch (Exception ex)
            {
                // add nothing if something goes wrong
            }

            // try to get ScoutBase OmniRig supported rigs
            try
            {
                List<SupportedRig> scoutbaserigs = ScoutBaseRig.SupportedRigs();
                foreach (SupportedRig scoutbaserig in scoutbaserigs)
                {
                    rigs.Add(scoutbaserig);
                }
            }
            catch (Exception ex)
            {
                // add nothing if something goes wrong
            }

            // try to collect other rigs from other CAT interfaces here

            // return supported rigs
            return rigs;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            StartOptions = (CATWorkerStartOptions)e.Argument;
            this.ReportProgress(0, StartOptions.Name + " started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = StartOptions.Name + "_" + this.GetType().Name;

            // check if any CAT is working by getting all supported rigs
            // handle exceptions
            List<SupportedRig> rigs = new List<SupportedRig>();
            try
            {
                rigs = CATWorker.SupportedRigs();
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, "Error while getting supported rigs from CAT: " + ex.Message);
                this.ReportProgress(1, RIGSTATUS.NOCAT);
                return;
            }

            // check if any rig is found
            if (rigs.Count == 0)
            {
                this.ReportProgress(-1, "Error while getting supported rigs from CAT: No available CAT found!");
                this.ReportProgress(1, RIGSTATUS.NORIG);
                return;
            }

            // check if the rig is among the currently supported rigs --> get a new IRig object
            // be careful with ActiveX objects an handle exceptions
            Rig = null;
            try
            {
                foreach (SupportedRig rig in rigs)
                {
                    if (rig.Type == StartOptions.RigType)
                    {
                        switch (rig.CATEngine)
                        {
                            case CATENGINE.OMNIRIGX: Rig = new OmniRigX(); break;
                            case CATENGINE.SCOUTBASE: Rig = new ScoutBaseRig(); break;
                        }

                        // OK, we have a valid CAT and rig --> complete and download settings
                        RigSettings settings = new RigSettings();
                        settings.Type = rig.Type;
                        settings.Model = rig.Model;
                        settings.PortName = StartOptions.PortName;
                        settings.Baudrate = StartOptions.Baudrate;
                        settings.DataBits = StartOptions.DataBits;
                        settings.Parity = StartOptions.Parity;
                        settings.StopBits = StartOptions.StopBits;
                        settings.RtsMode = StartOptions.RTS;
                        settings.DtrMode = StartOptions.DTR;
                        settings.PollMs = StartOptions.Poll;
                        settings.TimeoutMs = StartOptions.Timeout;
                        Rig.Settings = settings;

                        // stop search
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, "Error while trying to get a rig object from CAT: " + ex.ToString());
            }

            // report error if rig is still null
            if (Rig == null)
            {
                this.ReportProgress(-1, "Rig is not supported by any available CAT!");
                this.ReportProgress(1, RIGSTATUS.NORIG);
                return;
            }

            
            this.ReportProgress(0, "Opening CAT: Success!");
            this.ReportProgress(2, Rig);

            // old values for check changes
            DOPPLERSTRATEGY olddoppler = DOPPLERSTRATEGY.DOPPLER_NONE;
            RIGSTATUS oldrigstat = RIGSTATUS.NONE;
            RIGMODE oldrigmode = RIGMODE.NONE;
            RIGSPLIT oldrigsplit = RIGSPLIT.NONE;
            RIGRIT oldrigrit = RIGRIT.NONE;
            long oldrxfreq = -1;
            long oldtxfreq = -1;

            // run polling loop
            while (!this.CancellationPending)
            {
                try
                {
                    RIGSTATUS rigstatus = Rig.GetStatus();
                    if (oldrigstat != rigstatus)
                    {
                        oldrigstat = rigstatus;
                        this.ReportProgress(1, rigstatus);
                    }

                    // get rig infos when rig is online
                    if (rigstatus == RIGSTATUS.ONLINE)
                    {
                        // get mode
                        RIGMODE rigmode = Rig.GetMode();
                        if ( oldrigmode != rigmode)
                        {
                            lock (Rig)
                            {
                                oldrigmode = rigmode;
                                this.ReportProgress(2, Rig);
                            }
                        }

                        // get split
                        RIGSPLIT rigsplit = Rig.GetSplit();
                        if (oldrigsplit != rigsplit)
                        {
                            lock (Rig)
                            {
                                oldrigsplit = rigsplit;
                                this.ReportProgress(2, Rig);
                            }
                        }

                        // get rit
                        RIGRIT rigrit = Rig.GetRit();
                        if (oldrigrit != rigrit)
                        {
                            lock (Rig)
                            {
                                oldrigrit = rigrit;
                                this.ReportProgress(2, Rig);
                            }
                        }

                        // get RX frequency
                        long rxfreq = Rig.GetRxFrequency();
                        if (oldrxfreq != rxfreq)
                        {
                            lock (Rig)
                            {
                                oldrxfreq = rxfreq;
                                this.ReportProgress(2, Rig);
                            }
                        }
                        
                        // get TX frequency
                        long txfreq = Rig.GetTxFrequency();
                        if (oldtxfreq != txfreq)
                        {
                            lock (Rig)
                            {
                                oldtxfreq = txfreq;
                                this.ReportProgress(2, Rig);
                            }
                        }

                    }

                    // check for Doppler settings changes
                    if (olddoppler != DopplerStrategy)
                    {
                        olddoppler = DopplerStrategy;
                        // check for Doppler tracking enabled
                        if ((DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_A) || (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_B) || (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_C) || (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_D))
                        {
                            // enter doppler tracking
                            Rig.SetDopplerStrategy(DopplerStrategy);
                            Rig.EnterDoppler();
                        }
                        else
                        {
                            // leave doppler tracking
                            Rig.SetDopplerStrategy(DOPPLERSTRATEGY.DOPPLER_NONE);
                            Rig.LeaveDoppler();
                        }
                    }

                    // Doppler tracking
                    if ((DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_A) || (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_B) || (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_C) || (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_D))
                    {
                        Rig.SetSplitMode(RxFrequency, TxFrequency);
                    }
                }
                catch (Exception ex)
                {

                }

                Thread.Sleep(Properties.Settings.Default.CAT_Update);
            }

            // reset status
            this.ReportProgress(1, RIGSTATUS.NONE);
        }
    }
}
