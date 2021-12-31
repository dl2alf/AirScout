using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ScoutBase.CAT
{

    // enums

    public enum CommWorkerEvent
    {
        evErr = -1,
        evNotify,
        evPortStat,
        evOnlineStat,
        evSent,
        evRcvd,
        evStatusLine,
        evTimeout
        // not supported: ,evRlsd
    }

    [Description("Stopbits")]
    public enum StopBits 
    {
        [Description("1")]
        sbOne,
        [Description("1,5")]
        sbOne_5,
        [Description("2")]
        sbTwo
    }

    [Description("Parity")]
    public enum Parity 
    {
        [Description("None")]
        ptNone,
        [Description("Odd")]
        ptOdd,
        [Description("Even")]
        ptEven,
        [Description("Mark")]
        ptMark,
        [Description("Space")]
        ptSpace
    }

    [Description("Flow Control")]
    public enum FlowControl 
    {
        [Description("Low")]
        fcLow,
        [Description("High")]
        fcHigh,
        [Description("Handshake")]
        fcHandShake
    }

    public enum RxBlockMode { rbChar, rbBlockSize, rbTerminator }


    public class CommPort : BackgroundWorker
    {

        public readonly int BufSize = 1024;

        private DateTime FNextStatusTime = DateTime.MinValue;
        private DateTime FDeadLineTime = DateTime.MinValue;

        // command queue
        public CommandQueue FQueue = new CommandQueue();

        // serial port
        private SerialPort COM = new SerialPort();

        private FlowControl FRtsMode = FlowControl.fcLow;
        private FlowControl FDtrMode = FlowControl.fcLow;

        // public properties
        public string PortName { get { return GetPortName(); } set { SetPortName(value); } }
        public int BaudRate { get { return GetBaudRate(); } set { SetBaudRate(value); } }
        public int DataBits { get { return GetDataBits(); } set { SetDataBits(value); } }
        public StopBits StopBits { get { return GetStopBits(); } set { SetStopBits(value); } }
        public Parity Parity { get { return GetParity(); } set { SetParity(value); } }
        public FlowControl DtrMode { get { return FDtrMode; } set { SetDtrMode(value); } }
        public FlowControl RtsMode { get { return FRtsMode; } set { SetRtsMode(value); } }
        public bool Open { get { return GetOpen(); } }
        public bool RtsBit { get { return GetRtsBit(); } set { SetRtsBit(value); } }
        public bool DtrBit { get { return GetDtrBit(); } set { SetDtrBit(value); } }
        public bool CtsBit { get; internal set; } 
        public bool DsrBit { get; internal set; }
        public bool RlsdBit { get { return GetRlsdBit(); } }        //(receive-line-signal-detect, not supported yet

        // public events
        public delegate void CommNotifyEventHandler(object sender);

        public event CommNotifyEventHandler OnError;
        public event CommNotifyEventHandler OnReceived;
        public event CommNotifyEventHandler OnSent;
        public event CommNotifyEventHandler OnCtsDsr;

        //------------------------------------------------------------------------------
        // Initialization
        //------------------------------------------------------------------------------

        public CommPort()
        {
            // enable progress report & cancellation
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;

            // create serial port object
            SerialPort COM = new SerialPort();

            COM.ReadBufferSize = BufSize;

            //set default comm paraams
            PortName = "COM1";
            BaudRate = 19200;
            DataBits = 8;
            StopBits = StopBits.sbOne;
            Parity = Parity.ptNone;

            FDtrMode = FlowControl.fcLow;
            RtsMode = FlowControl.fcLow;

            // do not use xon/off so far
            /*
            FDcb.XonLim := BUF_SIZE div 2;
            FDcb.XoffLim := MulDiv(BUF_SIZE, 3, 4);
            FDcb.XonChar := #17;  //$11
            FDcb.XoffChar := #19; //$13
            */

        }

        //------------------------------------------------------------------------------
        // Get/set
        //------------------------------------------------------------------------------

        private bool GetOpen()
        {
            if (COM != null)
            {
                return COM.IsOpen;
            }

            return false;
        }

        private string GetPortName()
        {
            if (COM != null)
            {
                return COM.PortName;
            }

            return "";
        }

        private int GetBaudRate()
        {
            if (COM != null)
            {
                return COM.BaudRate;
            }

            return 0;
        }

        private int GetDataBits()
        {
            if (COM != null)
            {
                return COM.DataBits;
            }

            return 0;
        }

        private Parity GetParity()
        {
            if (COM != null)
            {
                switch (COM.Parity)
                {
                    case System.IO.Ports.Parity.None:
                        return Parity.ptNone;
                    case System.IO.Ports.Parity.Odd:
                        return Parity.ptOdd;
                    case System.IO.Ports.Parity.Even:
                        return Parity.ptEven;
                    case System.IO.Ports.Parity.Mark:
                        return Parity.ptMark;
                    case System.IO.Ports.Parity.Space:
                        return Parity.ptSpace;
                }
            }

            return Parity.ptNone;
        }

        private StopBits GetStopBits()
        {
            if (COM != null)
            {
                switch(COM.StopBits)
                {
                    case System.IO.Ports.StopBits.One:
                        return StopBits.sbOne;
                    case System.IO.Ports.StopBits.OnePointFive:
                        return StopBits.sbOne_5;
                    case System.IO.Ports.StopBits.Two:
                        return StopBits.sbTwo;
                }
            }

            return StopBits.sbOne;
        }

        private void SetPortName(string value)
        {
            if (COM != null)
            {
                // check for empty values --> not allowed!
                if (!String.IsNullOrEmpty(value))
                {
                    COM.PortName = value;
                }

            }
        }

        private void SetBaudRate(int value)
        {
            if (COM != null)
            {
                COM.BaudRate = value;
            }
        }

        private void SetDataBits(int value)
        {
            if (COM != null)
            {
                COM.DataBits = Math.Max(5, Math.Min(8, value));
            }
        }

        private void SetParity(Parity value)
        {
            if (COM != null)
            {
                switch(value)
                {
                    case Parity.ptNone:
                        COM.Parity = System.IO.Ports.Parity.None;
                        break;
                    case Parity.ptOdd:
                        COM.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case Parity.ptEven:
                        COM.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case Parity.ptMark:
                        COM.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case Parity.ptSpace:
                        COM.Parity = System.IO.Ports.Parity.Space;
                        break;
                }
            }
        }

        private void SetStopBits(StopBits value)
        {
            if (COM != null)
            {
                switch (value)
                {
                    case StopBits.sbOne:
                        COM.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case StopBits.sbOne_5:
                        COM.StopBits = System.IO.Ports.StopBits.OnePointFive;
                        break;
                    case StopBits.sbTwo:
                        COM.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                }
            }
        }


        //------------------------------------------------------------------------------
        // Read/write
        //------------------------------------------------------------------------------

        public void PurgeRx()
        {
            if (COM == null)
                return;

            COM.DiscardInBuffer();
        }

        public void PurgeTx()
        {
            if (COM == null)
                return;
 
            COM.DiscardOutBuffer();
        }

        //------------------------------------------------------------------------------
        // Fire events
        //------------------------------------------------------------------------------

        private void FireErrEvent()
        {
            if (OnError != null)
            {
                OnError.Invoke(this);
            }
        }

        private void FireTxEvent()
        {
            if (OnSent != null)
            {
                OnSent.Invoke(this);
            }
        }

        private void FireRxEvent()
        {
            if (OnReceived != null)
            {
                OnReceived.Invoke(this);
            }
        }

        private void FireCtsDsrEvent()
        {
            if (OnCtsDsr != null)
            {
                OnCtsDsr.Invoke(this);
            }
        }


        //------------------------------------------------------------------------------
        // Control bits
        //------------------------------------------------------------------------------

        private bool GetCtsBit()
        {
            if (COM != null)
            {
                return COM.CtsHolding;
            }

            return false;
        }

        private bool GetDsrBit()
        {
            if (COM != null)
            {
                return COM.DsrHolding;
            }

            return false;
        }

        private bool GetDtrBit()
        {
            return COM.DtrEnable;
        }

        private bool GetRtsBit()
        {
            return COM.RtsEnable;
        }

        private bool GetRlsdBit()
        {
            // not implemented
            return false;
        }

        private void SetDtrBit(bool value)
        {
            if (COM != null)
            {
                COM.DtrEnable = value;
            }
        }

        private void SetRtsBit(bool value)
        {
            if (COM != null)
            {
                COM.RtsEnable = value;
            }
        }

        private void SetDtrMode(FlowControl value)
        {
            if (COM != null)
            {
                switch (value)
                {
                    case FlowControl.fcLow:
                        COM.Handshake = Handshake.None;
                        COM.DtrEnable = false;
                        break;
                    case FlowControl.fcHigh:
                        COM.Handshake = Handshake.None;
                        COM.DtrEnable = true;
                        break;
                    case FlowControl.fcHandShake:
                        COM.Handshake = Handshake.RequestToSend;
                        break;
                }
            }
        }

        private void SetRtsMode(FlowControl value)
        {
            if (COM != null)
            {
                switch (value)
                {
                    case FlowControl.fcLow:
                        COM.Handshake = Handshake.None;
                        COM.RtsEnable = false;
                        break;
                    case FlowControl.fcHigh:
                        COM.Handshake = Handshake.None;
                        COM.RtsEnable = true;
                        break;
                    case FlowControl.fcHandShake:
                        COM.Handshake = Handshake.RequestToSend;
                        break;
                }
            }
        }

        //------------------------------------------------------------------------------
        // Queue
        //------------------------------------------------------------------------------

        public void AddCommands(List<RigCommand> cmds, CommandKind kind)
        {
            lock (FQueue)
            {
                for (int i = 0; i < cmds.Count; i++)
                {
                    QueueItem item = new QueueItem();
                    item.Code = cmds[i].Code;
                    item.Number = i;
                    item.ReplyLength = cmds[i].ReplyLength;
                    item.ReplyEnd = ByteFuns.BytesToStr(cmds[i].ReplyEnd);
                    item.Kind = kind;
                    FQueue.Add(item);
                }
            }
        }


        //------------------------------------------------------------------------------
        // Background worker
        //------------------------------------------------------------------------------

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            // get the startup parameters
            CommPortStartParams StartParams = (CommPortStartParams)e.Argument;

            byte[] RxBuffer = null;
            RxBlockMode RxBlockMode = RxBlockMode.rbChar;
            int RxBlockSize = 0;
            string RxBlockTerminator = "";

            bool PortInitialized = true;    // set to true to get a possible error message once
            bool PortOpen = false;
            bool Online = false;

            // remember old values for check on changes
            int OldRxChars = 0;
            int OldTxChars = 0;
            bool oldrts = false;
            bool olddtr = false;
            bool oldcts = false;
            bool olddsr = false;
        
        Thread.CurrentThread.Priority = ThreadPriority.Highest;
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                Thread.CurrentThread.Name = "CommWorker";
            }

            // .NET SerialPort implementation does not have full functionality 
            // so we try to poll the interface for all information needed and fire according events

            while (!this.CancellationPending)
            {
                try
                {
                    // hopefully we got a valid SerialPort object
                    // skip, if not
                    if (COM == null)
                    {
                        throw new NullReferenceException("COM port is null.");
                    }

                    // try to start communication, if not started
                    if (!COM.IsOpen)
                    {
                        try
                        {
                            // try to open port, if not open
                            COM.Open();
                            OldRxChars = COM.BytesToRead;
                            OldTxChars = COM.BytesToWrite;
                            CtsBit = COM.CtsHolding;
                            DsrBit = COM.DsrHolding;

                            PortInitialized = true;

                            // clear buffers
                            PurgeRx();
                            PurgeTx();
                            RxBuffer = null;

                            // set initial queue and phase, status
                            lock (FQueue)
                            {
                                FQueue.Clear();
                                FQueue.Phase = ExchangePhase.phIdle;
                            }

                            Online = false;

                            // queue initial commands
                            this.ReportProgress((int)CommWorkerEvent.evNotify, "Adding init commands to queue.");
                            AddCommands(StartParams.RigCommands.InitCmd, CommandKind.ckInit);
                            AddCommands(StartParams.RigCommands.StatusCmd, CommandKind.ckStatus);
                        }
                        catch (Exception ex)
                        {
                            // report port status on change
                            if (PortOpen)
                            {
                                PortOpen = false;
                                this.ReportProgress((int)CommWorkerEvent.evPortStat, false);
                            }

                            if (PortInitialized)
                            {
                                PortInitialized = false;
                                throw new InvalidOperationException("Cannot start communication (" + ex.Message + ")");
                            }
                        }
                    }

                    // skip if COM is not open
                    if (COM.IsOpen)
                    {
                        // report port status on change
                        if (!PortOpen)
                        {
                            PortOpen = true;
                            this.ReportProgress((int)CommWorkerEvent.evPortStat, true);
                        }
                    }
                    else
                    {
                        // report port status on change
                        if (PortOpen)
                        {
                            PortOpen = false;
                            this.ReportProgress((int)CommWorkerEvent.evPortStat, false);
                        }
                        Thread.Sleep(1000);
                        continue;
                    }

                    // check if status refresh is necessary
                    if (Online && (DateTime.Now > FNextStatusTime))
                    {
                        // are there already status commands in queue?
                        // add, if not
                        if (FQueue.HasStatusCommands())
                        {
                            this.ReportProgress((int)CommWorkerEvent.evNotify, "Status commands already in queue.");
                        }
                        else
                        {
                            this.ReportProgress((int)CommWorkerEvent.evNotify, "Adding status commands to queue.");
                            AddCommands(StartParams.RigCommands.StatusCmd, CommandKind.ckStatus);
                        }

                        // set next status refresh time
                        FNextStatusTime = DateTime.Now.AddMilliseconds(StartParams.PollMs);
                    }

                    // are we in phIdle phase?
                    if (FQueue.Phase == ExchangePhase.phIdle)
                    {
                        // rx chars available? --> read them
                        int rxcount = COM.BytesToRead;
                        if (rxcount > 0)
                        {
                            byte[] buf = new byte[rxcount];
                            COM.Read(buf, 0, rxcount);

                            // report unexpected bytes
                            this.ReportProgress((int)CommWorkerEvent.evErr, 
                                "Unexpected bytes in RX buffer: " + ((StartParams.RigCommands.CmdType == CommandType.ctBinary)? ByteFuns.BytesToHex(buf) : "\"" + ByteFuns.BytesToStr(buf) + "\""));
                            PurgeRx();
                            RxBuffer = null;
                        }

                        // check queue and send next command if any
                        lock (FQueue)
                        {
                            if (FQueue.Count > 0)
                            {
                                // clear all buffers
                                PurgeRx();
                                PurgeTx();
                                RxBuffer = null;

                                // prepare for receiving reply
                                RxBlockSize = FQueue[0].ReplyLength;
                                RxBlockTerminator = FQueue[0].ReplyEnd;
                                if (!String.IsNullOrEmpty(FQueue[0].ReplyEnd))
                                    RxBlockMode = RxBlockMode.rbTerminator;
                                else if (FQueue[0].ReplyLength > 0)
                                    RxBlockMode = RxBlockMode.rbBlockSize;
                                else RxBlockMode = RxBlockMode.rbChar;

                                // log
                                string s = "";
                                switch (FQueue[0].Kind)
                                {
                                    case CommandKind.ckInit:
                                        s = "init";
                                        break;
                                    case CommandKind.ckWrite:
                                        s = StartParams.RigCommands.ParamToStr(FQueue[0].Param);
                                        break;
                                    case CommandKind.ckStatus:
                                        s = "status";
                                        break;
                                    case CommandKind.ckCustom:
                                        s = "custom";
                                        break;
                                }
                                this.ReportProgress((int)CommWorkerEvent.evNotify, "Sending " + s + " command (" + 
                                    ((StartParams.RigCommands.CmdType == CommandType.ctBinary)? ByteFuns.BytesToHex(FQueue[0].Code) : "\"" + ByteFuns.BytesToStr(FQueue[0].Code) + "\"") + ")");

                                // send command
                                byte[] buf = FQueue[0].Code;
                                COM.Write(buf, 0, buf.Length);

                                FQueue.Phase = ExchangePhase.phSending;
                            }
                        }
                    }

                    // are we in phSending phase?
                    else if (FQueue.Phase == ExchangePhase.phSending)
                    {
                        // still bytes to send?
                        int txcount = COM.BytesToWrite;
                        if (txcount > 0)
                        {
                            // do nothing and wait until all bytes are sent
                        }
                        else
                        {
                            // report finish
                            this.ReportProgress((int)CommWorkerEvent.evSent);

                            // do we need reply?
                            if (FQueue.CurrentCmd().NeedsReply())
                            {
                                // set receiving phase and deadline time
                                FQueue.Phase = ExchangePhase.phReceiving;
                                FDeadLineTime = DateTime.Now.AddMilliseconds(StartParams.TimeoutMs);
                            }
                            else
                            {
                                // delete command and set idle phase
                                lock (FQueue)
                                {
                                    FQueue.Delete(0);
                                    FDeadLineTime = DateTime.MaxValue;
                                    FQueue.Phase = ExchangePhase.phIdle;
                                }
                            }
                        }

                    }

                    // are we in receiving phase?
                    else if (FQueue.Phase == ExchangePhase.phReceiving)
                    {
                        // rx chars available? --> read them
                        int rxcount = COM.BytesToRead;
                        if (rxcount > 0)
                        {
                            byte[] buf = new byte[rxcount];
                            COM.Read(buf, 0, rxcount);

                            // initally copy buffer, else resize and append
                            if (RxBuffer == null)
                            {
                                RxBuffer = buf;
                            }
                            else
                            {
                                int oldlen = RxBuffer.Length;
                                Array.Resize(ref RxBuffer, RxBuffer.Length + rxcount);
                                Array.Copy(buf, 0, RxBuffer, oldlen, rxcount);
                            }

                            // fire rx event if rx complete (according to RxBlockMode)
                            bool fire = false;
                            switch (RxBlockMode)
                            {
                                case RxBlockMode.rbBlockSize:
                                    fire = RxBuffer.Length >= RxBlockSize;
                                    break;
                                case RxBlockMode.rbTerminator:
                                    // convert RXBuffer to string
                                    // be sure to use a converter with full 8bit conversion
                                    var MyEncoding = Encoding.GetEncoding("Windows-1252");
                                    string rxstring = MyEncoding.GetString(RxBuffer);
                                    fire = rxstring.EndsWith(RxBlockTerminator);
                                    break;
                                // rbChar
                                default:
                                    fire = true;
                                    break;
                            }
                            if (fire)
                            {
                                Online = true;

                                // report that we are online
                                this.ReportProgress((int)CommWorkerEvent.evOnlineStat, true);

                                // report received
                                // put the current command in the message, so that it won't get lost during processing
                                this.ReportProgress((int)CommWorkerEvent.evRcvd, new CommReceived(FQueue.CurrentCmd(), RxBuffer));

                                RxBuffer = null;

                                // still might be a wrong answer 
                                // remove the command and set idle state anyway
                                lock (FQueue)
                                {
                                    FQueue.Delete(0);
                                    FQueue.Phase = ExchangePhase.phIdle;
                                }
                            }
                        }

                        // check for Timeout
                        if (DateTime.Now > FDeadLineTime)
                        {
                            if (PortOpen)
                            {
                                // report timeout
                                Online = false;
                                this.ReportProgress((int)CommWorkerEvent.evTimeout, FQueue.CurrentCmd());
                                this.ReportProgress((int)CommWorkerEvent.evOnlineStat, false);
                            }

                            // clear all buffers
                            PurgeRx();
                            PurgeTx();
                            lock(FQueue)
                            {
                                FQueue.Clear();
                                FQueue.Phase = ExchangePhase.phIdle;
                            }

                            // queue initial commands
                            this.ReportProgress((int)CommWorkerEvent.evNotify, "Adding init commands to queue.");
                            AddCommands(StartParams.RigCommands.InitCmd, CommandKind.ckInit);
                            AddCommands(StartParams.RigCommands.StatusCmd, CommandKind.ckStatus);
                        }
                    }

                    // status line changed?
                    bool cts = COM.CtsHolding;
                    bool dsr = COM.DsrHolding;
                    bool rts = COM.RtsEnable;
                    bool dtr = COM.DtrEnable;
                    if (oldrts != rts)
                    {
                        // report status line change
                        oldrts = rts;
                        this.ReportProgress((int)CommWorkerEvent.evStatusLine);
                    }
                    if (olddtr != dtr)
                    {
                        // report status line change
                        olddtr = dtr;
                        this.ReportProgress((int)CommWorkerEvent.evStatusLine);
                    }
                    if (oldcts != cts)
                    {
                        // report status line change
                        oldcts = cts;
                        this.ReportProgress((int)CommWorkerEvent.evStatusLine);
                    }
                    if (olddsr != dsr)
                    {
                        // report status line change
                        olddsr = dsr;
                        this.ReportProgress((int)CommWorkerEvent.evStatusLine);
                    }

                    Thread.Sleep(StartParams.COMPollMs);
                }
                catch (Exception ex)
                {
                    // report error
                    this.ReportProgress((int)CommWorkerEvent.evErr, ex.ToString());

                    // try to close COM
                    if ((COM != null) && (COM.IsOpen))
                    {
                        try
                        {
                            COM.Close();

                        }
                        catch
                        {
                            // do nothing
                        }
                    }

                    // sleep a while 
                    Thread.Sleep(1000);
                }
            }

            // try to close COM
            if ((COM != null) && (COM.IsOpen))
            {
                try
                {
                    COM.Close();
                }
                catch
                {
                    // do nothing
                }
            }
        }


    }
}