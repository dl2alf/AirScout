using System;
using System.CodeDom;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ScoutBase.CAT
{
    // Base class for a rig to derive from
    public abstract class CustomRig
    {
        // private constant für COM port polling
        private readonly int COMPollMs = 20;

        // private rig command set
        private RigCommands FRigCommands = new RigCommands();

        // private port & rig status, values & parameters
        protected bool FEnabled = false;
        protected bool FPortOpen = false;
        protected bool FOnline  = false;
        protected long FFreq = 0;
        protected long FFreqA = 0;
        protected long FFreqB = 0;
        protected long FRitOffset = 0;
        protected long FPitch = 0;
        protected RigParam FVfo = RigParam.pmNone;
        protected RigParam FSplit = RigParam.pmNone;
        protected RigParam FRit = RigParam.pmNone;
        protected RigParam FXit = RigParam.pmNone;
        protected RigParam FTx = RigParam.pmNone;
        protected RigParam FMode = RigParam.pmNone;
        protected PortBits FPortBits = new PortBits();

        // public members
        public int RigNumber = 0;

        public int PollMs = 0;
        public int TimeoutMs = 0;

        public CommPort ComPort = new CommPort();
        public RigParam LastWrittenMode = RigParam.pmNone;

        // public rig status
        public RigCtlStatus Status { get { return GetStatus(); } }
        public string StatusStr { get { return GetStatusStr(); } }

        // public rig command set
        public RigCommands RigCommands { get { return FRigCommands; } set { SetRigCommands(value); } }
        public bool Enabled { get { return FEnabled; } set { SetEnabled(value); } }
        public bool PortOpen { get { return FPortOpen; } set { SetPortOpen(value); } }
        public bool Online { get { return FOnline; } set { SetOnline(value); } }

        // public properties
        public long Freq { get { return FFreq; } set { SetFreq(value); } }
        public long FreqA { get { return FFreqA; } set { SetFreqA(value); } }
        public long FreqB { get { return FFreqB; } set { SetFreqB(value); } }
        public long Pitch { get { return FPitch; } set { SetPitch(value); } }
        public long RitOffset { get { return FRitOffset; } set { SetRitOffset(value); } }
        public RigParam Vfo { get { return FVfo; } set { SetVfo(value); } }
        public RigParam Split { get { return FSplit; } set { SetSplit(value); } }
        public RigParam Rit { get { return FRit; } set { SetRit(value); } }
        public RigParam Xit { get { return FXit; } set { SetXit(value); } }
        public RigParam Tx { get { return FTx; } set { SetTx(value); } }
        public RigParam Mode { get { return FMode; } set { SetMode(value); } }
        public PortBits PortBits { get { return FPortBits; } }

        // these commands are implemented in the descendant class,
        // just to keep them in a separate file
        protected abstract void AddWriteCommand(RigParam param, long value = 0);
        protected abstract void AddCustomCommand(object sender, byte[] code, int len, string end);

        protected abstract void ProcessInitReply(int number, byte[] data);
        protected abstract void ProcessStatusReply(int number, byte[] data);
        protected abstract void ProcessWriteReply(RigParam param, byte[] data);
        protected abstract void ProcessCustomReply(object sender, byte[] code, byte[] data);


        //------------------------------------------------------------------------------
        // System
        //------------------------------------------------------------------------------

        public CustomRig()
        {
            if (ComPort != null)
            {
                // set PortBits
                if (FPortBits != null)
                {
                    FPortBits.FPort = ComPort;
                }

                // subscribe to COM port event
                ComPort.ProgressChanged += ComPort_ProgressChanged;
            }
        }

        //------------------------------------------------------------------------------
        // Status
        //------------------------------------------------------------------------------

        private RigCtlStatus GetStatus()
        {
            if (RigCommands == null)
                return RigCtlStatus.stNotConfigured;
            else if (!FEnabled)
                return RigCtlStatus.stDisabled;
            else if (!FPortOpen)
                return RigCtlStatus.stPortBusy;
            else if (!FOnline)
                return RigCtlStatus.stNotResponding;
            else return RigCtlStatus.stOnLine;
        }

        private string GetStatusStr()
        {
            string[] statusstr = new string[] { "Rig is not configured", "Rig is disabled", "Port is not available", "Rig is not responding", "Rig is online" };
            return statusstr[(int)GetStatus()];
        }


        //------------------------------------------------------------------------------
        // Rig capabilities
        //------------------------------------------------------------------------------

        public bool IsParamReadable(RigParam param)
        {
            if (RigCommands == null)
                return false;
            if (RigCommands.ReadableParams == null)
                return false;

            return RigCommands.ReadableParams.Contains(param);
        }

        public bool IsParamWriteable(RigParam param)
        {
            if (RigCommands == null)
                return false;
            if (RigCommands.WriteableParams == null)
                return false;

            return RigCommands.WriteableParams.Contains(param);
        }

        //------------------------------------------------------------------------------
        // COM port
        //------------------------------------------------------------------------------

        private void SetEnabled(bool value)
        {
            // do nothing if already enabled
            if (FEnabled == value)
                return;

            // check for valid rig commands
            if (RigCommands == null)
                return;

            // check for valid COM port
            if (ComPort == null)
                return;

            // set new value & do additional stuff
            if (value)
            {
                // log
                OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llInfo, "Starting RIG" + RigNumber.ToString()));

                // start COM
                if (!ComPort.IsBusy)
                {
                    // set parameter
                    CommPortStartParams p = new CommPortStartParams();
                    p.RigNumber = RigNumber;
                    p.RigCommands = RigCommands;
                    p.PollMs = PollMs;
                    p.TimeoutMs = TimeoutMs;
                    p.COMPollMs = COMPollMs;

                    // start background worker
                    DateTime timeout = DateTime.Now.AddMilliseconds(TimeoutMs);
                    ComPort.RunWorkerAsync(p);
                    while (!ComPort.IsBusy)
                    {
                        Application.DoEvents();
                        if (DateTime.Now > timeout)
                        {
                            // report timeout & live with it
                            OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " Timeout while starting ComPortWorker."));
                        }
                    }
                }

                FEnabled = true;
            }
            else
            {

                // log
                OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llInfo, "Stopping RIG" + RigNumber.ToString()));

                // stop background worker
                DateTime timeout = DateTime.Now.AddMilliseconds(TimeoutMs);
                ComPort.CancelAsync();
                while (ComPort.IsBusy)
                {
                    Application.DoEvents();
                    if (DateTime.Now > timeout)
                    {
                        // report timeout
                        OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " Timeout while stopping ComPortWorker."));
                        // kill COM port on timeout to be sure, that the port is released
                        ComPort = null;
                        GC.Collect();
                        ComPort = new CommPort();
                    }
                }

                // reset stati
                FEnabled = false;
                SetPortOpen(false);
            }

            // fire status event
            OmniRig.FireComNotifyStatus(RigNumber);

            LastWrittenMode = RigParam.pmNone;

        }

        private void SetPortOpen(bool value)
        {
            // do nothing if already open
            if (FPortOpen == value)
                return;

            if (value)
            {
                FPortOpen = true;
            }
            else
            {
                FPortOpen = false;
                SetOnline(false);
            }

            // fire status event
            OmniRig.FireComNotifyStatus(RigNumber);
        }

        private void SetOnline (bool value)
        {
            // do nothing if already online
            if (FOnline == value)
                return;

            // set new value & do additional stuff
            if (value)
            {
                FOnline = true;
            }
            else
            {
                FOnline = false;

                // clear all public stati & values & properties
                FFreq = 0;
                FFreqA = 0;
                FFreqB = 0;
                FPitch = 0;
                FRitOffset = 0;
                FVfo = RigParam.pmNone;
                FSplit = RigParam.pmNone;
                FRit = RigParam.pmNone;
                FXit = RigParam.pmNone;
                FTx = RigParam.pmNone;
                FMode = RigParam.pmNone;

            }

            // fire status event
            OmniRig.FireComNotifyStatus(RigNumber);

            // fire param event
            OmniRig.FireComNotifyParams(RigNumber, 6);
        }

        //------------------------------------------------------------------------------
        // Set param
        //------------------------------------------------------------------------------

        private void SetRigCommands(RigCommands value)
        {
            FRigCommands = value;
            OmniRig.FireComNotifyRigType(RigNumber);
        }

        private void SetFreq(long value)
        {
            // return on not enabled or not writeable
            if (!Enabled || !IsParamWriteable(RigParam.pmFreq))
                return;

            AddWriteCommand(RigParam.pmFreq, value);
        }

        private void SetFreqA(long value)
        {
            // return on not enabled or not writeable
            if (!Enabled || !IsParamWriteable(RigParam.pmFreqA))
                return;

            AddWriteCommand(RigParam.pmFreqA, value);
        }

        private void SetFreqB(long value)
        {
            // return on not enabled or not writeable
            if (!Enabled || !IsParamWriteable(RigParam.pmFreqB))
                return;

            AddWriteCommand(RigParam.pmFreqB, value);
        }

        private void SetPitch(long value)
        {
            // return on not enabled or not writeable
            if (!Enabled || IsParamWriteable(RigParam.pmPitch))
                return;

            AddWriteCommand(RigParam.pmPitch, value);

            // remember the pitch that we set if we cannot read it back from the rig
            if (!IsParamReadable(RigParam.pmPitch))
            {
                FPitch = value;

                // fake ParamChange notification
                OmniRig.FireComNotifyParams(RigNumber, 1);
            }
        }

        private void SetRitOffset(long value)
        {
            // return on not enabled or not writeable
            if (!Enabled || !IsParamWriteable(RigParam.pmRitOffset))
                return;

            AddWriteCommand(RigParam.pmRitOffset, value);
        }

        private void SetMode(RigParam param)
        {
            // return on not enabled or not in set or not writeable
            if (!Enabled || !RigParams.ModeParams.Contains(param) || !IsParamWriteable(param))
                return;

            AddWriteCommand(param);
        }

        private void SetRit(RigParam param)
        {
            // return on not enabled or not in set or not writeable
            if (!Enabled || !RigParams.RitOnParams.Contains(param) || !IsParamWriteable(param))
                return;

            AddWriteCommand(param);
        }

        private void SetSplit(RigParam param)
        {
            // return on not enabled or not in set or not writeable
            if (!Enabled || !RigParams.SplitOnParams.Contains(param) || !IsParamWriteable(param))
                return;

            if (IsParamWriteable(param) && (param != Split))
            {
                AddWriteCommand(param);
            }

            // emulate split mode if not writeable
            else if (param == RigParam.pmSplitOn)
            {
                if (Vfo == RigParam.pmVfoAA)
                {
                    Vfo = RigParam.pmVfoAB;
                }
                else if (Vfo == RigParam.pmVfoBB)
                {
                    Vfo = RigParam.pmVfoBA;
                }
            }
            else
            {
                if (Vfo == RigParam.pmVfoAB)
                {
                    Vfo = RigParam.pmVfoAA;
                }
                else if (Vfo == RigParam.pmVfoBA)
                {
                    Vfo = RigParam.pmVfoBB;
                }
            }
        }

        private void SetTx(RigParam param)
        {
            // return on not enabled or not in set or not writeable
            if (!Enabled || !RigParams.TxParams.Contains(param) || !IsParamWriteable(param))
                return;

            AddWriteCommand(param);
        }

        private void SetVfo(RigParam param)
        {
            // return on not enabled or not in set or not writeable
            if (!Enabled || !RigParams.VfoParams.Contains(param) || !IsParamWriteable(param))
                return;

            AddWriteCommand(param);
        }

        public void ForceVfo(RigParam param)
        {
            // return on not enabled or not in set or not writeable
            if (!Enabled || !RigParams.ModeParams.Contains(param) || !IsParamWriteable(param))
                return;

            AddWriteCommand(param);
        }

        private void SetXit(RigParam param)
        {
            // return on not enabled or not in set or not writeable
            if (!Enabled || !RigParams.XitOnParams.Contains(param) || !IsParamWriteable(param))
                return;

            AddWriteCommand(param);
        }

        private RigParam GetSplit()
        {
            if (FSplit != RigParam.pmNone)
                return FSplit;

            // emulate Split from VFO if not readable
            if ((Vfo == RigParam.pmVfoAA) || (Vfo == RigParam.pmVfoBB))
                return RigParam.pmSplitOff;
            else if ((Vfo == RigParam.pmVfoAB) || (Vfo == RigParam.pmVfoBA))
                return RigParam.pmSplitOn;
            return RigParam.pmNone;
        }

        //------------------------------------------------------------------------------
        // Public methods
        //------------------------------------------------------------------------------

        public void ClearRit()
        {
            AddWriteCommand(RigParam.pmRit0);
        }

        public long FrequencyOfTone(int tone)
        {
            long result = tone;

            lock (this)
            {
                if ((Mode == RigParam.pmCW_U) || (Mode == RigParam.pmCW_L))
                {
                    result -= Pitch;
                }
                if ((Mode == RigParam.pmCW_L) || (Mode == RigParam.pmSSB_L))
                {
                    result = -result;
                }

                result += Freq;
            }

            return result;
        }

        public long GetRxFrequency()
        {
            long result = 0;
            lock (this)
            {
                try
                {
                    if (IsParamReadable(RigParam.pmFreqA) && new RigParamSet() { RigParam.pmVfoA, RigParam.pmVfoAA, RigParam.pmVfoAB }.Contains(Vfo))
                    {
                        result = FreqA;
                    }
                    else if (IsParamReadable(RigParam.pmFreqB) && new RigParamSet() { RigParam.pmVfoB, RigParam.pmVfoBA, RigParam.pmVfoBB }.Contains(Vfo))
                    {
                        result = FreqB;
                    }
                    else if ((Tx != RigParam.pmTx) || (Split != RigParam.pmSplitOn))
                    {
                        result = Freq;
                    }
                    else
                    {
                        result = 0;
                    }

                    //include RIT
                    if (Rit == RigParam.pmRitOn)
                    {
                        result += RitOffset;
                    }
                }
                catch (Exception ex)
                {
                    OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llComm, "RIG" + RigNumber.ToString() + " Error while calcluating RX frequency: " + ex.Message));
                }
            }

            return result;
        }

        public long GetTxFrequency()
        {
            long result = 0;
            lock (this)
            {
                try
                {
                    if (IsParamReadable(RigParam.pmFreqA) && new RigParamSet() { RigParam.pmVfoAA, RigParam.pmVfoBA }.Contains(Vfo))
                    {
                        result = FreqA;
                    }
                    else if (IsParamReadable(RigParam.pmFreqB) && new RigParamSet() { RigParam.pmVfoAB, RigParam.pmVfoBB }.Contains(Vfo))
                    {
                        result = FreqB;
                    }
                    else if (Tx != RigParam.pmTx)
                    {
                        result = Freq;
                    }
                    else
                    {
                        result = 0;
                    }

                    //include XIT
                    if (Xit == RigParam.pmXitOn)
                    {
                        result += RitOffset;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }


        // sets the rig to a simplex mode on frequency
        // the disableritxit parameter is for compatibility
        public void SetSimplexMode(long freq, bool disableritxit = true)
        {
            // return on empty command set
            if (RigCommands == null)
                return;

            if (IsParamWriteable(RigParam.pmFreqA) && IsParamWriteable(RigParam.pmVfoAA))
            {
                ForceVfo(RigParam.pmVfoAA);
                FreqA = freq;
            }
            else if (IsParamWriteable(RigParam.pmFreqA) && IsParamWriteable(RigParam.pmVfoA) && IsParamWriteable(RigParam.pmSplitOff))
            {
                ForceVfo(RigParam.pmVfoA);
                FreqA = freq;
            }
            else if (IsParamWriteable(RigParam.pmFreq) && IsParamWriteable(RigParam.pmVfoA) && IsParamWriteable(RigParam.pmVfoB))
            {
                ForceVfo(RigParam.pmVfoB);
                Freq = freq;
                ForceVfo(RigParam.pmVfoA);
                Freq = freq;
            }
            else if (IsParamWriteable(RigParam.pmFreq) && IsParamWriteable(RigParam.pmVfoEqual))
            {
                Freq = freq;
                ForceVfo(RigParam.pmVfoEqual);
            }
            else if (IsParamWriteable(RigParam.pmFreq) && IsParamWriteable(RigParam.pmVfoSwap))
            {
                ForceVfo(RigParam.pmVfoSwap);
                Freq = freq;
                ForceVfo(RigParam.pmVfoSwap);
                Freq = freq;
            }
            // Added by RA6UAZ for Icom Marine Radio NMEA Command
            else if (IsParamWriteable(RigParam.pmFreq) && !IsParamWriteable(RigParam.pmFreqA) && IsParamWriteable(RigParam.pmFreqB))
            {
                Freq = freq;
                FreqB = freq;
            }
            else if (IsParamWriteable(RigParam.pmFreq))
            {
                Freq = freq;
            }

            if (IsParamWriteable(RigParam.pmSplitOff))
            {
                Split = RigParam.pmSplitOff;
            }

            // why?
            if (disableritxit)
            {
                Rit = RigParam.pmRitOff;
                Xit = RigParam.pmXitOff;
            }
        }

        // sets the rig to a duplex mode on two frequencies
        // the disableritxit parameter is for compatibility
        public void SetSplitMode(long rxfreq, long txfreq, bool disableritxit = true)
        {
            // return on empty command set
            if (RigCommands == null)
                return;

            //set rx and tx frequencies and split
            if (IsParamWriteable(RigParam.pmFreqA) && IsParamWriteable(RigParam.pmFreqB) && IsParamWriteable(RigParam.pmVfoAB))
            {
                // TS-570
                ForceVfo(RigParam.pmVfoAB);
                FreqA = rxfreq;
                FreqB = txfreq;
            }
            else if (IsParamWriteable(RigParam.pmFreq) && IsParamWriteable(RigParam.pmVfoEqual))
            {
                // IC-746
                Freq = txfreq;
                ForceVfo(RigParam.pmVfoEqual);
                Freq = rxfreq;
            }
            else if (IsParamWriteable(RigParam.pmVfoB) && IsParamWriteable(RigParam.pmFreq) && IsParamWriteable(RigParam.pmVfoA))
            {
                // FT-100D
                ForceVfo(RigParam.pmVfoB);
                Freq = txfreq;
                ForceVfo(RigParam.pmVfoA);
                Freq = rxfreq;
            }
            else if (IsParamWriteable(RigParam.pmFreq) && IsParamWriteable(RigParam.pmVfoSwap))
            {
                // FT-817 ?
                ForceVfo(RigParam.pmVfoSwap);
                Freq = txfreq;
                ForceVfo(RigParam.pmVfoSwap);
                Freq = rxfreq;
            }
            else if (IsParamWriteable(RigParam.pmFreqA) && IsParamWriteable(RigParam.pmFreqB) && IsParamWriteable(RigParam.pmVfoA))
            {
                //FT-1000 MP, IC-7610
                ForceVfo(RigParam.pmVfoA);
                FreqA = rxfreq;
                FreqB = txfreq;
            }
            // Added by RA6UAZ for Icom Marine Radio NMEA Command
            else if (IsParamWriteable(RigParam.pmFreq) && !IsParamWriteable(RigParam.pmFreqA) && IsParamWriteable(RigParam.pmFreqB))
            {
                Freq = rxfreq;
                FreqB = txfreq;
            }

            if (IsParamWriteable(RigParam.pmSplitOn))
            {
                Split = RigParam.pmSplitOn;
            }

            // why?
            if (disableritxit)
            {
                Rit = RigParam.pmRitOff;
                Xit = RigParam.pmXitOff;
            }
        }

        public void SetBothModes(RigParam value)
        {

        }

        //------------------------------------------------------------------------------
        // Comm event
        //------------------------------------------------------------------------------

        private void ComPort_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            // try to catch all exceptions here so that they do not affect serial communication
            try
            {
                // report error
                if (e.ProgressPercentage == (int)CommWorkerEvent.evErr)
                {
                    OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " COM Error on " + ((ComPort != null)? ComPort.PortName : "[null]") + ": " + (string)e.UserState));

                    // set rig offline and fire event
                    if (FOnline)
                    {
                        FOnline = false;
                        OmniRig.FireComNotifyStatus(RigNumber);
                    }
                }

                // report notify
                else if (e.ProgressPercentage == (int)CommWorkerEvent.evNotify)
                {
                    OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llComm, "RIG" + RigNumber.ToString() + " COM Message: " + (string)e.UserState));
                }

                // report port status
                else if (e.ProgressPercentage == (int)CommWorkerEvent.evPortStat)
                {
                    bool portopen = (bool)e.UserState;
                    if (FPortOpen && !portopen)
                    {
                        FPortOpen = false;
                        FOnline = false;
                        OmniRig.FireComNotifyStatus(RigNumber);
                    }
                    else if (!FPortOpen & portopen)
                    {
                        FPortOpen = true;
                        OmniRig.FireComNotifyStatus(RigNumber);
                    }
                }

                // report online status
                else if (e.ProgressPercentage == (int)CommWorkerEvent.evOnlineStat)
                {
                    bool online = (bool)e.UserState;

                    // set online status and fire event, if necessary
                    if (FOnline & !online)
                    {
                        OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " COM Timeout while receiving."));
                        FOnline = false;
                        OmniRig.FireComNotifyStatus(RigNumber);
                    }
                    else if (!FOnline & online)
                    {
                        FOnline = true;
                        OmniRig.FireComNotifyStatus(RigNumber);
                    }
                }

                // report rx message
                else if (e.ProgressPercentage == (int)CommWorkerEvent.evRcvd)
                {
                    CommReceived rcvd = (CommReceived)e.UserState;

                    OmniRig.Log(new LogNotifyEventArgs(
                        DateTime.UtcNow, 
                        LOGLEVEL.llComm, 
                        "RIG" + RigNumber.ToString() + " COM Reply received (" + ((RigCommands.CmdType == CommandType.ctBinary)? ByteFuns.BytesToHex(rcvd.Received) : "\"" + ByteFuns.BytesToStr(rcvd.Received) + "\"") + ")"));

                    //process data
                    try
                    {
                        switch (rcvd.Cmd.Kind)
                        {
                            case CommandKind.ckInit:
                                ProcessInitReply(rcvd.Cmd.Number, rcvd.Received);
                                break;
                            case CommandKind.ckWrite:
                                ProcessWriteReply(rcvd.Cmd.Param, rcvd.Received);
                                break;
                            case CommandKind.ckStatus:
                                ProcessStatusReply(rcvd.Cmd.Number, rcvd.Received);
                                break;
                            case CommandKind.ckCustom:
                                ProcessCustomReply(rcvd.Cmd.CustSender, rcvd.Cmd.Code, rcvd.Received);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llFatal, "RIG" + RigNumber.ToString() + " Error while processing reply: " + ex.Message));
                    }
                }
                else if (e.ProgressPercentage == (int)CommWorkerEvent.evStatusLine)
                {
                    OmniRig.Log(new LogNotifyEventArgs(
                        DateTime.UtcNow, 
                        LOGLEVEL.llComm, 
                        "RIG" + RigNumber.ToString() +
                        " Status line changed: CTS = " +
                        ((ComPort.CtsBit) ? "ON" : "OFF") + ", DSR=" +
                        ((ComPort.DsrBit) ? "ON" : "OFF")));

                    // no event to fire so far?
                    // fire ComStatus instead
                    OmniRig.FireComNotifyStatus(RigNumber);
                }

                // report receiving timeout
                else if (e.ProgressPercentage == (int)CommWorkerEvent.evTimeout)
                {
                    // get last command from UserState
                    // should not be null, but we want to be sure
                    string bytes = "[null]";
                    if (e.UserState != null)
                    {
                        bytes = (RigCommands.CmdType == CommandType.ctBinary)? ByteFuns.BytesToHex(((QueueItem)e.UserState).Code) : "\"" + ByteFuns.BytesToStr(((QueueItem)e.UserState).Code) + "\"";
                    }
                    OmniRig.Log(new LogNotifyEventArgs(
                        DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " COM Timeout: Missing answer from rig (" + bytes + ")"));

                    // set rig offline and fire event
                    if (FOnline)
                    {
                        FOnline = false;
                        OmniRig.FireComNotifyStatus(RigNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llFatal, "RIG" + RigNumber.ToString() + " Error while processing message from COM: " + ex.Message));
            }
        }
    }
}
