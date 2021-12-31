using ScoutBase.CAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml.Schema;

namespace AirScout.CAT
{
    // represents a rig connected directly via ScoutBase.CAT (OmniRig style)
    public class ScoutBaseRig : ScoutBase.CAT.Rig, IRig
    {

        private RigSettings Settings = new RigSettings();

        // doppler tracking strategy
        DOPPLERSTRATEGY DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_NONE;

        // save rig settings 
        private RIGMODE SaveRigMode;
        private RIGSPLIT SaveRigSplit;
        private RIGRIT SaveRigRit;
        private long SaveRxFrequency;
        private long SaveTxFrequency;


        // ************************ Interface *********************************

        RigSettings IRig.Settings
        {
            get
            {

                return Settings;
            }
            set
            {
                Settings = value;

                this.Enabled = false;

                // copy settings to rig settings
                ScoutBase.CAT.RigSettings settings = new ScoutBase.CAT.RigSettings();
                settings.RigType = Settings.Model;
                settings.PortName = Settings.PortName;
                settings.Baudrate = Settings.Baudrate;
                settings.DataBits = Settings.DataBits;
                switch(Settings.Parity)
                {
                    case PARITY.EVEN: settings.Parity = Parity.ptEven; break;
                    case PARITY.ODD: settings.Parity = Parity.ptOdd; break;
                    case PARITY.MARK: settings.Parity = Parity.ptMark; break;
                    case PARITY.SPACE: settings.Parity = Parity.ptSpace; break;
                    default: settings.Parity = Parity.ptNone; break;
                }
                switch(Settings.StopBits)
                {
                    case STOPBITS.ONE_5: settings.StopBits = StopBits.sbOne_5; break;
                    case STOPBITS.TWO: settings.StopBits = StopBits.sbTwo; break;
                    default: settings.StopBits = StopBits.sbOne; break;
                }
                switch (Settings.RtsMode)
                {
                    case FLOWCONTROL.HIGH: settings.RtsMode = FlowControl.fcHigh; break;
                    case FLOWCONTROL.HANDSHAKE: settings.RtsMode = FlowControl.fcHandShake; break;
                    default: settings.RtsMode = FlowControl.fcLow; break;
                }
                switch (Settings.DtrMode)
                {
                    case FLOWCONTROL.HIGH: settings.DtrMode = FlowControl.fcHigh; break;
                    case FLOWCONTROL.HANDSHAKE: settings.DtrMode = FlowControl.fcHandShake; break;
                    default: settings.DtrMode = FlowControl.fcLow; break;
                }
                settings.PollMs = Settings.PollMs;
                settings.TimeoutMs = Settings.TimeoutMs;

                // transfer settings to rig and restart
                settings.ToRig(this);

                // enable rig
                this.Enabled = true;
            }
        }


        CATENGINE IRig.CatEngine
        {
            get
            {
                return CATENGINE.SCOUTBASE;
            }
        }

        string IRig.CatVersion
        {
            get
            {
                return "ScoutBase.CAT V" + ScoutBase.CAT.OmniRig.InterfaceVersion;
            }
        }

        public RIGSTATUS GetStatus()
        {
            try
            {
                // translate result to RIGSTATUS values
                switch ((RigCtlStatus)this.Status)
                {
                    case RigCtlStatus.stPortBusy: return RIGSTATUS.NOPORT;
                    case RigCtlStatus.stNotConfigured: return RIGSTATUS.NORIG;
                    case RigCtlStatus.stDisabled: return RIGSTATUS.NORIG;
                    case RigCtlStatus.stNotResponding: return RIGSTATUS.OFFLINE;
                }

                // rig is online
                // check for rig capabilities here
                if (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_A)
                {
                    // rig must support at least split operation
                    if ((!this.IsParamReadable(RigParam.pmSplitOff)) ||
                        (!this.IsParamReadable(RigParam.pmSplitOn)) ||
                        (!this.IsParamWriteable(RigParam.pmSplitOff)) ||
                        (!this.IsParamWriteable(RigParam.pmSplitOn)) ||
                        (!this.IsParamWriteable(RigParam.pmVfoEqual))
                        )
                    {
                        return RIGSTATUS.NOTSUITABLE;
                    }
                }

                // return online
                return RIGSTATUS.ONLINE;
            }
            catch (Exception ex)
            {

            }

            return RIGSTATUS.NONE;
        }

        public RIGMODE GetMode()
        {
            try
            {
                switch ((RigParam)this.Mode)
                {
                    case RigParam.pmCW_L: return RIGMODE.CW;
                    case RigParam.pmCW_U: return RIGMODE.CW_R;
                    case RigParam.pmSSB_L: return RIGMODE.LSB;
                    case RigParam.pmSSB_U: return RIGMODE.USB;
                    case RigParam.pmDIG_L: return RIGMODE.DIG;
                    case RigParam.pmDIG_U: return RIGMODE.DIG_R;
                    case RigParam.pmAM: return RIGMODE.AM;
                    case RigParam.pmFM: return RIGMODE.FM;
                }

                return RIGMODE.NONE;
            }
            catch (Exception ex)
            {

            }

            return RIGMODE.NONE;
        }

        public RIGSPLIT GetSplit()
        {
            try
            {
                switch ((RigParam)this.Split)
                {
                    case RigParam.pmSplitOff: return RIGSPLIT.SPLITOFF;
                    case RigParam.pmSplitOn: return RIGSPLIT.SPLITON;
                }
            }
            catch (Exception ex)
            {

            }

            return RIGSPLIT.NONE;

        }

        public RIGRIT GetRit()
        {
            try
            {
                switch ((RigParam)this.Rit)
                {
                    case RigParam.pmRitOff: return RIGRIT.RITOFF;
                    case RigParam.pmRitOn: return RIGRIT.RITON;
                }
            }
            catch (Exception ex)
            {

            }

            return RIGRIT.NONE;

        }

        public RIGVFO GetVfo()
        {
            try
            {
                switch ((RigParam)this.Vfo)
                {
                    case RigParam.pmVfoA: return RIGVFO.A;
                    case RigParam.pmVfoB: return RIGVFO.B;
                }
            }
            catch (Exception ex)
            {

            }

            return RIGVFO.NONE;

        }

        long IRig.GetRxFrequency()
        {
            try
            {
                if ((RigParam)this.Split == RigParam.pmSplitOff)
                {
                    // check if pmFreq is readable --> return Freq
                    if (this.Freq > 0)
                        return this.Freq;
                    // check if pmFreqA is readable --> return FreqA instead
                    if (this.FreqA > 0)
                        return this.FreqA;
                }
                else
                {
                    // check if pmFreqA is readable --> return FreqA instead
                    if (this.FreqA > 0)
                        return this.FreqA;
                }
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        long IRig.GetTxFrequency()
        {
            try
            {
                if ((RigParam)this.Split == RigParam.pmSplitOff)
                {
                    // check if pmFreq is readable --> return Freq
                    if (this.Freq > 0)
                        return this.Freq;
                    // check if pmFreqA is readable --> return FreqA instead
                    if (this.FreqA > 0)
                        return this.FreqA;
                }
                else
                {
                    // check if pmFreqB is readable --> return FreqB instead
                    if (this.FreqB > 0)
                        return this.FreqB;
                }
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        // ********************************************************************

        public bool SetMode(RIGMODE mode)
        {
            try
            {
                switch (mode)
                {
                    case RIGMODE.CW: this.Mode = RigParam.pmCW_L; return true;
                    case RIGMODE.CW_R: this.Mode = RigParam.pmCW_U; return true;
                    case RIGMODE.LSB: this.Mode = RigParam.pmSSB_L; return true;
                    case RIGMODE.USB: this.Mode = RigParam.pmSSB_U; return true;
                    case RIGMODE.DIG: this.Mode = RigParam.pmDIG_L; return true;
                    case RIGMODE.DIG_R: this.Mode = RigParam.pmDIG_U; return true;
                    case RIGMODE.AM: this.Mode = RigParam.pmAM; return true;
                    case RIGMODE.FM: this.Mode = RigParam.pmFM; return true;
                }

            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool SetSplit(RIGSPLIT split)
        {
            try
            {
                switch (split)
                {
                    case RIGSPLIT.SPLITOFF: this.Split = RigParam.pmSplitOff; return true;
                    case RIGSPLIT.SPLITON: this.Split = RigParam.pmSplitOn; return true;
                }
            }
            catch (Exception ex)
            {

            }


            return false;
        }

        public bool SetRit(RIGRIT rit)
        {
            try
            {
                switch (rit)
                {
                    case RIGRIT.RITOFF: this.Rit = RigParam.pmRitOff; return true;
                    case RIGRIT.RITON: this.Rit = RigParam.pmRitOn; return true;
                }
            }
            catch (Exception ex)
            {

            }


            return false;
        }

        public bool SetVfo(RIGVFO vfo)
        {
            try
            {
                switch (vfo)
                {
                    case RIGVFO.A: this.Vfo = RigParam.pmVfoA; return true;
                    case RIGVFO.B: this.Vfo = RigParam.pmVfoB; return true;
                }
            }
            catch (Exception ex)
            {

            }


            return false;
        }

        public bool SetRxFrequency(long rx)
        {
            try
            {
                if ((RigParam)this.Split == RigParam.pmSplitOff)
                {
                    // check if pmFreq is writeable --> set Freq
                    if (this.IsParamWriteable(RigParam.pmFreq))
                    {
                        this.Freq = (int)rx;
                        return true;
                    }
                    // check if pmFreqA is writeable --> set FreqA instead
                    if (this.IsParamWriteable(RigParam.pmFreqA))
                    {
                        this.FreqA = (int)rx;
                        return true;
                    }
                }
                else
                {
                    // check if pmFreqA is writeable --> set FreqA instead
                    if (this.IsParamWriteable(RigParam.pmFreqA))
                    {
                        this.FreqA = (int)rx;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool SetTxFrequency(long tx)
        {
            try
            {
                if ((RigParam)this.Split == RigParam.pmSplitOff)
                {
                    // check if pmFreq is writeable --> set Freq
                    if (this.IsParamWriteable(RigParam.pmFreq))
                    {
                        this.Freq = (int)tx;
                        return true;
                    }
                    // check if pmFreqA is writeable --> set FreqA instead
                    if (this.IsParamWriteable(RigParam.pmFreqA))
                    {
                        this.FreqA = (int)tx;
                        return true;
                    }
                }
                else
                {
                    // check if pmFreqB is writeable --> set FreqB instead
                    if (this.IsParamWriteable(RigParam.pmFreqB))
                    {
                        this.FreqB = (int)tx;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool IntSetSimplexMode(dynamic rig, long freq, RIGMODE mode)
        {
            return false;
        }

        public bool SetSimplexMode(long freq, RIGMODE mode = RIGMODE.NONE)
        {

            // set frequency and switch split off
            try
            {
                if (this.IsParamWriteable(RigParam.pmFreqA) && this.IsParamWriteable(RigParam.pmVfoAA))
                {
                    this.Vfo = RigParam.pmVfoAA;
                    this.FreqA = freq;
                }
                else if (this.IsParamWriteable(RigParam.pmFreqA) && this.IsParamWriteable(RigParam.pmVfoA) && this.IsParamWriteable(RigParam.pmSplitOff))
                {
                    this.Vfo = RigParam.pmVfoA;
                    this.FreqA = freq;
                }
                else if (this.IsParamWriteable(RigParam.pmFreq) && this.IsParamWriteable(RigParam.pmVfoA) && this.IsParamWriteable(RigParam.pmVfoB))
                {
                    this.Vfo = RigParam.pmVfoB;
                    this.Freq = freq;
                    this.Vfo = RigParam.pmVfoA;
                    this.Freq = freq;
                }
                else if (this.IsParamWriteable(RigParam.pmFreq) && this.IsParamWriteable(RigParam.pmVfoEqual))
                {
                    this.Freq = freq;
                    this.Vfo = RigParam.pmVfoEqual;
                }
                else if (this.IsParamWriteable(RigParam.pmFreq) && this.IsParamWriteable(RigParam.pmVfoSwap))
                {
                    this.Vfo = RigParam.pmVfoSwap;
                    this.Freq = freq;
                    this.Vfo = RigParam.pmVfoSwap;
                    this.Freq = freq;
                }
                // Added by RA6UAZ for Icom Marine Radio NMEA Command
                else if (this.IsParamWriteable(RigParam.pmFreq) && !this.IsParamWriteable(RigParam.pmVfoA) && this.IsParamWriteable(RigParam.pmFreqB))
                {
                    this.Freq = freq;
                    this.FreqB = freq;
                }
                else if (this.IsParamWriteable(RigParam.pmFreq))
                {
                    this.Freq = freq;
                }

                if (this.IsParamWriteable(RigParam.pmSplitOff))
                {
                    this.Split = RigParam.pmSplitOff;
                }

                // simply set mode on current VFO
                if (CanWriteMode() && (mode != RIGMODE.NONE))
                {
                    this.SetMode(mode);
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        private bool CanWriteMode()
        {
            // check if modes are writable
            if (!this.IsParamWriteable(RigParam.pmCW_L))
                return false;
            if (!this.IsParamWriteable(RigParam.pmCW_U))
                return false;
            if (!this.IsParamWriteable(RigParam.pmSSB_L))
                return false;
            if (!this.IsParamWriteable(RigParam.pmSSB_U))
                return false;
            if (!this.IsParamWriteable(RigParam.pmDIG_L))
                return false;
            if (!this.IsParamWriteable(RigParam.pmDIG_U))
                return false;
            if (!this.IsParamWriteable(RigParam.pmAM))
                return false;
            if (!this.IsParamWriteable(RigParam.pmFM))
                return false;

            return true;
        }

        public bool SetSplitMode(long rxfreq, long txfreq, RIGMODE mode = RIGMODE.NONE)
        {
            //set rx and tx frequencies and split
            try
            {
                // try to set mode on both Vfos via VfoEqual which is the easiest way
                if (this.IsParamWriteable(RigParam.pmVfoEqual))
                {
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                        this.Vfo = RigParam.pmVfoEqual;
                    }
                }

                if (this.IsParamWriteable(RigParam.pmFreqA) && this.IsParamWriteable(RigParam.pmFreqB) && this.IsParamWriteable(RigParam.pmVfoAB))
                {
                    // TS-570
                    this.Vfo = RigParam.pmVfoAB;
                    this.FreqA = rxfreq;
                    this.FreqB = txfreq;
                    // try to set mode once for both VFOs
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                    }
                }
                else if (this.IsParamWriteable(RigParam.pmFreq) && this.IsParamWriteable(RigParam.pmVfoEqual))
                {
                    // IC-746
                    this.Freq = txfreq;
                    // should work fine for both VFOs because of VfoEqual
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                    }
                    this.Vfo = RigParam.pmVfoEqual;
                    this.Freq = rxfreq;
                }
                else if (this.IsParamWriteable(RigParam.pmVfoB) && this.IsParamWriteable(RigParam.pmFreq) && this.IsParamWriteable(RigParam.pmVfoA))
                {
                    // FT-100D
                    this.Vfo = RigParam.pmVfoB;
                    this.Freq = txfreq;
                    // should work fine for both VFOs because of switching to both VFOs
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                    }
                    this.Vfo = RigParam.pmVfoA;
                    this.Freq = rxfreq;
                    // should work fine for both VFOs because of switching to both VFOs
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                    }
                }
                else if (this.IsParamWriteable(RigParam.pmFreq) && this.IsParamWriteable(RigParam.pmVfoSwap))
                {
                    // FT-817 ?
                    this.Vfo = RigParam.pmVfoSwap;
                    this.Freq = txfreq;
                    // should work fine for both VFOs because of VfoSwap
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                    }
                    this.Vfo = RigParam.pmVfoSwap;
                    this.Freq = rxfreq;
                    // should work fine for both VFOs because of VfoSwap
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                    }
                }
                else if (this.IsParamWriteable(RigParam.pmFreqA) && this.IsParamWriteable(RigParam.pmFreqB) && this.IsParamWriteable(RigParam.pmVfoA))
                {
                    //FT-1000 MP, IC-7610
                    this.Vfo = RigParam.pmVfoA;
                    this.FreqA = rxfreq;
                    this.FreqB = txfreq;
                    // can set only mode on VFO A
                    if (CanWriteMode() && (mode != RIGMODE.NONE))
                    {
                        this.SetMode(mode);
                    }

                }
                // Added by RA6UAZ for Icom Marine Radio NMEA Command
                else if (this.IsParamWriteable(RigParam.pmFreq) && !this.IsParamWriteable(RigParam.pmFreqA) && this.IsParamWriteable(RigParam.pmFreqB))
                {
                    this.Freq = rxfreq;
                    this.FreqB = txfreq;
                }

                if (this.IsParamWriteable(RigParam.pmSplitOn))
                {
                    this.Split = RigParam.pmSplitOn;
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool SetDopplerStrategy(DOPPLERSTRATEGY doppler)
        {
            DopplerStrategy = doppler;
            return true;
        }

        public bool EnterDoppler()
        {
            // return false if we are not online or rig not suitable
            if (GetStatus() != RIGSTATUS.ONLINE)
                return false;

            // save rig settings 
            SaveRigMode = this.GetMode();
            SaveRigSplit = this.GetSplit();
            SaveRigRit = this.GetRit();
            SaveRxFrequency = this.GetRxFrequency();
            SaveTxFrequency = this.GetTxFrequency();

            SetSplitMode(SaveRxFrequency, SaveRxFrequency, SaveRigMode);

            return true;
        }

        public bool LeaveDoppler()
        {
            // restore rig settings 
            if (SaveRigMode != RIGMODE.NONE)
                SetMode(SaveRigMode);
            if (SaveRigSplit != RIGSPLIT.NONE)
                SetSplit(SaveRigSplit);
            if (SaveRigRit != RIGRIT.NONE)
                SetRit(SaveRigRit);
            if (SaveRxFrequency > 0)
                SetRxFrequency(SaveRxFrequency);
            if (SaveTxFrequency > 0)
                SetTxFrequency(SaveTxFrequency);

            SetSimplexMode(SaveRxFrequency, SaveRigMode);

            return true;
        }



        public static List<SupportedRig> SupportedRigs()
        {
            List<SupportedRig> rigs = new List<SupportedRig>();
            List<string> rigtypes = ScoutBase.CAT.OmniRig.SupportedRigs();
            foreach (string rigtype in rigtypes)
            {
                SupportedRig rig = new SupportedRig();
                rig.Type = "[Serial] " + rigtype;
                rig.Model = rigtype;
                rig.CATEngine = CATENGINE.SCOUTBASE;
                rigs.Add(rig);
            }

            // return list of rigs
            return rigs;
        }
    }
}
