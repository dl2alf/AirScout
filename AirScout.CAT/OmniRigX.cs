using Microsoft.Win32;
using ScoutBase.CAT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace AirScout.CAT
{
    // OmniRig enums
    // hopefully constant over all versions

    public enum OMNIRIGSTATUS
    {
        ST_NOTCONFIGURED = 0x00000000,
        ST_DISABLED = 0x00000001,
        ST_PORTBUSY = 0x00000002,
        ST_NOTRESPONDING = 0x00000003,
        ST_ONLINE = 0x00000004
    }

    public enum OMNIRIGPARAM
    {
        PM_UNKNOWN = 0x00000001,
        PM_FREQ = 0x00000002,
        PM_FREQA = 0x00000004,
        PM_FREQB = 0x00000008,
        PM_PITCH = 0x00000010,
        PM_RITOFFSET = 0x00000020,
        PM_RIT0 = 0x00000040,
        PM_VFOAA = 0x00000080,
        PM_VFOAB = 0x00000100,
        PM_VFOBA = 0x00000200,
        PM_VFOBB = 0x00000400,
        PM_VFOA = 0x00000800,
        PM_VFOB = 0x00001000,
        PM_VFOEQUAL = 0x00002000,
        PM_VFOSWAP = 0x00004000,
        PM_SPLITON = 0x00008000,
        PM_SPLITOFF = 0x00010000,
        PM_RITON = 0x00020000,
        PM_RITOFF = 0x00040000,
        PM_XITON = 0x00080000,
        PM_XITOFF = 0x00100000,
        PM_RX = 0x00200000,
        PM_TX = 0x00400000,
        PM_CW_U = 0x00800000,
        PM_CW_L = 0x01000000,
        PM_SSB_U = 0x02000000,
        PM_SSB_L = 0x04000000,
        PM_DIG_U = 0x08000000,
        PM_DIG_L = 0x10000000,
        PM_AM = 0x20000000,
        PM_FM = 0x40000000

    }


    // represents a rig connected via OmniRig ActiveX interface
    public class OmniRigX : IRig
    {
        // holds the OmniRig ActiveX object 
        private static dynamic OmniRigEngine = null;
        private static string OmniRigVersion = "";

        // rig type constants
        private static readonly string RigType1 = "[OmniRig] Rig 1";
        private static readonly string RigType2 = "[OmniRig] Rig 2";
        private static readonly string RigType3 = "[OmniRig] Rig 3";
        private static readonly string RigType4 = "[OmniRig] Rig 4";

        // rig
        private RigSettings Settings = new RigSettings();

        // doppler tracking strategy
        DOPPLERSTRATEGY DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_NONE;

        // save rig settings 
        private RIGMODE SaveRigMode;
        private RIGSPLIT SaveRigSplit;
        private RIGRIT SaveRigRit;
        private long SaveRxFrequency;
        private long SaveTxFrequency;

        private static void StartOmniRig()
        {
            // check for OmniRigEngine is accessible
            try
            {
                if (OmniRigEngine == null)
                {
                    // do nothing
                }
            }
            catch
            {
                OmniRigEngine = null;
            }

            if (OmniRigEngine == null)
            {
                try
                {
                    // try to load recent version of OmniRig
                    // we want OmniRig interface V2.1
                    Type COMType = System.Type.GetTypeFromProgID("OmniRig.OmniRigX");
                    OmniRigVersion = "OmniRig V2.x";

                    // Leagcy support: try to load OmniRig V1.19 instead explicit, if loading of recent version fails
                    if (COMType == null)
                    {
                        COMType = System.Type.GetTypeFromCLSID(new Guid("4FE359C5-A58F-459D-BE95-CA559FB4F270"));
                        OmniRigVersion = "OmniRig V1.19";
                    }

                    // try to create COM-object dynamically
                    if (COMType != null)
                    {
                        OmniRigEngine = Activator.CreateInstance(COMType);
                    }
                    else
                    {
                        OmniRigEngine = null;
                        throw new NotSupportedException("OmniRig is not installed! or has unsupported version!");
                    }

                    // check supported versions
                    if (OmniRigEngine.InterfaceVersion != 0x110)
                    {
                        OmniRigVersion = "OmniRig V1.19";
                    }
                    else if ((OmniRigEngine.InterfaceVersion < 0x210) && (OmniRigEngine.InterfaceVersion > 0x299))
                    {
                        OmniRigVersion = "OmniRig V2.x";
                    }
                    else
                    {
                        OmniRigEngine = null;
                        throw new NotSupportedException("OmniRig is not installed but has unsupported version:" + OmniRigEngine.InterfaceVersion.ToString());
                    }
                }
                catch (Exception ex)
                {
                    OmniRigEngine = null;
                    throw new NotSupportedException("Error while initializing OmniRig: " + ex.ToString());
                }


            }
        }

        // ************************ Interface *********************************
        
        RigSettings IRig.Settings
        {
            get
            {
                if (OmniRigEngine == null)
                    StartOmniRig();

                // refresh Settings with values from OmniRig
                // we can set only rig model as we don't get more info

                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                    Settings.Model = OmniRigX.OmniRigEngine.Rig1.RigType;
                else if (Settings.Type == OmniRigX.RigType2)
                    Settings.Model = OmniRigX.OmniRigEngine.Rig2.RigType;
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                        Settings.Model = OmniRigX.OmniRigEngine.Rig3.RigType;
                    else if (Settings.Type == OmniRigX.RigType4)
                        Settings.Model = OmniRigX.OmniRigEngine.Rig4.RigType;
                }

                // return the current settings
                return Settings;
            }
            set
            {
                // update settings
                Settings = value;

                // we can only transfer rig model to OmniRig
                if (OmniRigEngine == null)
                    StartOmniRig();

                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                    OmniRigX.OmniRigEngine.Rig1.RigType = Settings.Model;
                else if (Settings.Type == OmniRigX.RigType2)
                    OmniRigX.OmniRigEngine.Rig2.RigType = Settings.Model;
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                        OmniRigX.OmniRigEngine.Rig3.RigType = Settings.Model;
                    else if (Settings.Type == OmniRigX.RigType4)
                        OmniRigX.OmniRigEngine.Rig4.RigType = Settings.Model;
                }

            }
        }

        CATENGINE IRig.CatEngine 
        { 
            get 
            {
                if (OmniRigEngine == null)
                    StartOmniRig();
                return CATENGINE.OMNIRIGX; 
            } 
        }

        string IRig.CatVersion
        {
            get
            {
                if (OmniRigEngine == null)
                    StartOmniRig();
                return OmniRigVersion;
            }
        }

        private bool Is2x()
        {
            try
            {
                return (OmniRigEngine.InterfaceVersion < 0x210) && (OmniRigEngine.InterfaceVersion > 0x299);
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        private RIGSTATUS IntGetRigStatus (dynamic rig)
        {
            // translate result to RIGSTATUS values
            switch ((OMNIRIGSTATUS)rig.Status)
            {
                case OMNIRIGSTATUS.ST_PORTBUSY: return RIGSTATUS.NOPORT;
                case OMNIRIGSTATUS.ST_NOTCONFIGURED: return RIGSTATUS.NORIG;
                case OMNIRIGSTATUS.ST_DISABLED: return RIGSTATUS.NORIG;
                case OMNIRIGSTATUS.ST_NOTRESPONDING: return RIGSTATUS.OFFLINE;
            }

            // rig is online
            // check for rig capabilities here
            if (DopplerStrategy == DOPPLERSTRATEGY.DOPPLER_A)
            {
                // rig must support at least split operation
                if ((!rig.IsParamReadable(OMNIRIGPARAM.PM_SPLITOFF)) ||
                    (!rig.IsParamReadable(OMNIRIGPARAM.PM_SPLITON)) ||
                    (!rig.IsParamWriteable(OMNIRIGPARAM.PM_SPLITOFF)) ||
                    (!rig.IsParamWriteable(OMNIRIGPARAM.PM_SPLITON)) ||
                    (!rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOEQUAL))
                    )
                {
                    return RIGSTATUS.NOTSUITABLE;
                }
            }

            // return online
            return RIGSTATUS.ONLINE;
        }

        public RIGSTATUS GetStatus()
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            // return NOCAT if still null
            if (OmniRigEngine == null)
                return RIGSTATUS.NOCAT;

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntGetRigStatus(OmniRigX.OmniRigEngine.Rig1);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntGetRigStatus(OmniRigX.OmniRigEngine.Rig2);
                }

                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntGetRigStatus(OmniRigX.OmniRigEngine.Rig3);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntGetRigStatus(OmniRigX.OmniRigEngine.Rig4);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return RIGSTATUS.NONE;
        }

        private RIGMODE IntGetMode (dynamic rig)
        {
            switch ((OMNIRIGPARAM)rig.Mode)
            {
                case OMNIRIGPARAM.PM_CW_L: return RIGMODE.CW;
                case OMNIRIGPARAM.PM_CW_U: return RIGMODE.CW_R;
                case OMNIRIGPARAM.PM_SSB_L: return RIGMODE.LSB;
                case OMNIRIGPARAM.PM_SSB_U: return RIGMODE.USB;
                case OMNIRIGPARAM.PM_DIG_L: return RIGMODE.DIG;
                case OMNIRIGPARAM.PM_DIG_U: return RIGMODE.DIG_R;
                case OMNIRIGPARAM.PM_AM: return RIGMODE.AM;
                case OMNIRIGPARAM.PM_FM: return RIGMODE.FM;
            }

            return RIGMODE.NONE;
        }

        public RIGMODE GetMode()
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntGetMode(OmniRigX.OmniRigEngine.Rig1);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntGetMode(OmniRigX.OmniRigEngine.Rig2);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntGetMode(OmniRigX.OmniRigEngine.Rig3);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntGetMode(OmniRigX.OmniRigEngine.Rig4);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RIGMODE.NONE;
        }

        public RIGSPLIT IntGetSplit(dynamic rig)
        {
            switch ((OMNIRIGPARAM)rig.Split)
            {
                case OMNIRIGPARAM.PM_SPLITOFF: return RIGSPLIT.SPLITOFF;
                case OMNIRIGPARAM.PM_SPLITON: return RIGSPLIT.SPLITON;
            }

            return RIGSPLIT.NONE;
        }

        public RIGSPLIT GetSplit()
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntGetSplit(OmniRigX.OmniRigEngine.Rig1);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntGetSplit(OmniRigX.OmniRigEngine.Rig2);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntGetSplit(OmniRigX.OmniRigEngine.Rig3);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntGetSplit(OmniRigX.OmniRigEngine.Rig4);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RIGSPLIT.NONE;

        }

        public RIGRIT IntGetRit (dynamic rig)
        {
            switch ((OMNIRIGPARAM)rig.Rit)
            {
                case OMNIRIGPARAM.PM_RITOFF: return RIGRIT.RITOFF;
                case OMNIRIGPARAM.PM_RITON: return RIGRIT.RITON;
            }

            return RIGRIT.NONE;
        }

        public RIGRIT GetRit()
        {

            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntGetRit(OmniRigX.OmniRigEngine.Rig1);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntGetRit(OmniRigX.OmniRigEngine.Rig2);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntGetRit(OmniRigX.OmniRigEngine.Rig3);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntGetRit(OmniRigX.OmniRigEngine.Rig4);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RIGRIT.NONE;

        }

        public RIGVFO IntGetVfo(dynamic rig)
        {
            switch ((OMNIRIGPARAM)rig.Vfo)
            {
                case OMNIRIGPARAM.PM_VFOA: return RIGVFO.A;
                case OMNIRIGPARAM.PM_VFOB: return RIGVFO.B;
            }

            return RIGVFO.NONE;
        }

        public RIGVFO GetVfo()
        {

            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntGetVfo(OmniRigX.OmniRigEngine.Rig1);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntGetVfo(OmniRigX.OmniRigEngine.Rig2);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntGetVfo(OmniRigX.OmniRigEngine.Rig3);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntGetVfo(OmniRigX.OmniRigEngine.Rig4);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RIGVFO.NONE;

        }

        private long IntGetRxFrequency(dynamic rig)
        {
            if ((OMNIRIGPARAM)rig.Split == OMNIRIGPARAM.PM_SPLITOFF)
            {
                // check if pmFreq is readable --> return Freq
                if (rig.Freq > 0)
                    return rig.Freq;
                // check if pmFreqA is readable --> return FreqA instead
                if (rig.FreqA > 0)
                    return rig.FreqA;
            }
            else
            {
                // check if pmFreqA is readable --> return FreqA instead
                if (rig.FreqA > 0)
                    return rig.FreqA;
            }

            return 0;
        }

        public long GetRxFrequency()
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            { 
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntGetRxFrequency(OmniRigX.OmniRigEngine.Rig1);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntGetRxFrequency(OmniRigX.OmniRigEngine.Rig2);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntGetRxFrequency(OmniRigX.OmniRigEngine.Rig3);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntGetRxFrequency(OmniRigX.OmniRigEngine.Rig4);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        private int IntGetTxFrequency(dynamic rig)
        {
            if ((OMNIRIGPARAM)rig.Split == OMNIRIGPARAM.PM_SPLITOFF)
            {
                // check if pmFreq is readable --> return Freq
                if (rig.Freq > 0)
                    return rig.Freq;
                // check if pmFreqA is readable --> return FreqA instead
                if (rig.FreqA > 0)
                    return rig.FreqA;
            }
            else
            {
                // check if pmFreqB is readable --> return FreqB instead
                if (rig.FreqB > 0)
                    return rig.FreqB;
            }

            return 0;
        }

        public long GetTxFrequency()
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntGetTxFrequency(OmniRigX.OmniRigEngine.Rig1);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntGetTxFrequency(OmniRigX.OmniRigEngine.Rig2);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntGetTxFrequency(OmniRigX.OmniRigEngine.Rig3);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntGetTxFrequency(OmniRigX.OmniRigEngine.Rig4);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        // ********************************************************************

        private bool IntSetMode(dynamic rig, RIGMODE mode)
        {
            switch (mode)
            {
                case RIGMODE.CW: rig.Mode = OMNIRIGPARAM.PM_CW_L; return true;
                case RIGMODE.CW_R: rig.Mode = OMNIRIGPARAM.PM_CW_U; return true;
                case RIGMODE.LSB: rig.Mode = OMNIRIGPARAM.PM_SSB_L; return true;
                case RIGMODE.USB: rig.Mode = OMNIRIGPARAM.PM_SSB_U; return true;
                case RIGMODE.DIG: rig.Mode = OMNIRIGPARAM.PM_DIG_L; return true;
                case RIGMODE.DIG_R: rig.Mode = OMNIRIGPARAM.PM_DIG_U; return true;
                case RIGMODE.AM: rig.Mode = OMNIRIGPARAM.PM_AM; return true;
                case RIGMODE.FM: rig.Mode = OMNIRIGPARAM.PM_FM; return true;
            }

            return false;
        }

        public bool SetMode(RIGMODE mode)
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetMode(OmniRigX.OmniRigEngine.Rig1, mode);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetMode(OmniRigX.OmniRigEngine.Rig2, mode);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetMode(OmniRigX.OmniRigEngine.Rig3, mode);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetMode(OmniRigX.OmniRigEngine.Rig4, mode);
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return false;
        }

        private bool IntSetSplit(dynamic rig, RIGSPLIT split)
        {
            switch (split)
            {
                case RIGSPLIT.SPLITOFF: rig.Split = OMNIRIGPARAM.PM_SPLITOFF; return true;
                case RIGSPLIT.SPLITON: rig.Split = OMNIRIGPARAM.PM_SPLITON; return true;
            }

            return false;
        }

        public bool SetSplit (RIGSPLIT split)
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetSplit(OmniRigX.OmniRigEngine.Rig1, split);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetSplit(OmniRigX.OmniRigEngine.Rig2, split);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetSplit(OmniRigX.OmniRigEngine.Rig3, split);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetSplit(OmniRigX.OmniRigEngine.Rig4, split);
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return false;
        }

        private bool IntSetRit(dynamic rig, RIGRIT rit)
        {
            switch (rit)
            {
                case RIGRIT.RITOFF: rig.Rit = OMNIRIGPARAM.PM_RITOFF; return true;
                case RIGRIT.RITON: rig.Rit = OMNIRIGPARAM.PM_RITON; return true;
            }

            return false;
        }

        public bool SetRit(RIGRIT rit)
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetRit(OmniRigX.OmniRigEngine.Rig1, rit);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetRit(OmniRigX.OmniRigEngine.Rig2, rit);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetRit(OmniRigX.OmniRigEngine.Rig3, rit);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetRit(OmniRigX.OmniRigEngine.Rig4, rit);
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return false;
        }

        private bool IntSetVfo(dynamic rig, RIGVFO vfo)
        {
            switch (vfo)
            {
                case RIGVFO.A: rig.Vfo = OMNIRIGPARAM.PM_VFOA; return true;
                case RIGVFO.B: rig.Vfo = OMNIRIGPARAM.PM_VFOB; return true;
            }

            return false;
        }

        public bool SetVfo(RIGVFO vfo)
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetVfo(OmniRigX.OmniRigEngine.Rig1, vfo);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetVfo(OmniRigX.OmniRigEngine.Rig2, vfo);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetVfo(OmniRigX.OmniRigEngine.Rig3, vfo);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetVfo(OmniRigX.OmniRigEngine.Rig4, vfo);
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return false;
        }

        private bool IntSetRxFrequency(dynamic rig, long rx)
        {
            if ((OMNIRIGPARAM)rig.Split == OMNIRIGPARAM.PM_SPLITOFF)
            {
                // check if pmFreq is writeable --> set Freq
                if (rig.IsWritable(OMNIRIGPARAM.PM_FREQ))
                {
                    rig.Freq = (int)rx;
                    return true;
                }
                // check if pmFreqA is writeable --> set FreqA instead
                if (rig.IsWritable(OMNIRIGPARAM.PM_FREQA))
                {
                    rig.FreqA = (int)rx;
                    return true;
                }
            }
            else
            {
                // check if pmFreqA is writeable --> set FreqA instead
                if (rig.IsWritable(OMNIRIGPARAM.PM_FREQA))
                {
                    rig.FreqA = (int)rx;
                    return true;
                }
            }

            return false;
        }

        public bool SetRxFrequency(long rx)
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            // check bounds as OmniRig ActiveX only supperts 32bit values
            if ((rx < Int32.MinValue) || (rx > int.MaxValue))
                return false;

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetRxFrequency(OmniRigX.OmniRigEngine.Rig1, rx);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetRxFrequency(OmniRigX.OmniRigEngine.Rig2, rx);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetRxFrequency(OmniRigX.OmniRigEngine.Rig3, rx);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetRxFrequency(OmniRigX.OmniRigEngine.Rig4, rx);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        private bool IntSetTxFrequency(dynamic rig, long tx)
        {
            if ((OMNIRIGPARAM)rig.Split == OMNIRIGPARAM.PM_SPLITOFF)
            {
                // check if pmFreq is writeable --> set Freq
                if (rig.IsWritable(OMNIRIGPARAM.PM_FREQ))
                {
                    rig.Freq = (int)tx;
                    return true;
                }
                // check if pmFreqA is writeable --> set FreqA instead
                if (rig.IsWritable(OMNIRIGPARAM.PM_FREQA))
                {
                    rig.FreqA = (int)tx;
                    return true;
                }
            }
            else
            {
                // check if pmFreqB is writeable --> set FreqB instead
                if (rig.IsWritable(OMNIRIGPARAM.PM_FREQB))
                {
                    rig.FreqB = (int)tx;
                    return true;
                }
            }

            return false;
        }

        public bool SetTxFrequency(long tx)
        {
            if (OmniRigEngine == null)
                StartOmniRig();

            // check bounds as OmniRig ActiveX only supperts 32bit values
            if ((tx < Int32.MinValue) || (tx > int.MaxValue))
                return false;

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetTxFrequency(OmniRigX.OmniRigEngine.Rig1, tx);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetTxFrequency(OmniRigX.OmniRigEngine.Rig2, tx);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetTxFrequency(OmniRigX.OmniRigEngine.Rig3, tx);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetTxFrequency(OmniRigX.OmniRigEngine.Rig4, tx);
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
            // set frequency and switch split off
            try
            {
                if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOAA))
                {
                    rig.Vfo = OMNIRIGPARAM.PM_VFOAA;
                    rig.FreqA = freq;
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_SPLITOFF))
                {
                    rig.Vfo = OMNIRIGPARAM.PM_VFOA;
                    rig.FreqA = freq;
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOB))
                {
                    rig.Vfo = OMNIRIGPARAM.PM_VFOB;
                    rig.Freq = freq;
                    rig.Vfo = OMNIRIGPARAM.PM_VFOA;
                    rig.Freq = freq;
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOEQUAL))
                {
                    rig.Freq = freq;
                    rig.Vfo = OMNIRIGPARAM.PM_VFOEQUAL;
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOSWAP))
                {
                    rig.Vfo = OMNIRIGPARAM.PM_VFOSWAP;
                    rig.Freq = freq;
                    rig.Vfo = OMNIRIGPARAM.PM_VFOSWAP;
                    rig.Freq = freq;
                }
                // Added by RA6UAZ for Icom Marine Radio NMEA Command
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && !rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQB))
                {
                    rig.Freq = freq;
                    rig.FreqB = freq;
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ))
                {
                    rig.Freq = freq;
                }

                if (rig.IsParamWriteable(OMNIRIGPARAM.PM_SPLITOFF))
                {
                    rig.Split = OMNIRIGPARAM.PM_SPLITOFF;
                }

                // simply set mode on current VFO
                if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                {
                    rig.Mode = mode;
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool SetSimplexMode(long freq, RIGMODE mode = RIGMODE.NONE)
        {

            // we cannot use the "buil-in" function from OmniRig as it affects the Rit/Xit
            if (OmniRigEngine == null)
                StartOmniRig();

            // check bounds as OmniRig ActiveX only supperts 32bit values
            if ((freq < Int32.MinValue) || (freq > int.MaxValue))
                return false;

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetSimplexMode(OmniRigX.OmniRigEngine.Rig1, freq, mode);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetSimplexMode(OmniRigX.OmniRigEngine.Rig2, freq, mode);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetSimplexMode(OmniRigX.OmniRigEngine.Rig3, freq, mode);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetSimplexMode(OmniRigX.OmniRigEngine.Rig4, freq, mode);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        private bool CanWriteMode(dynamic rig)
        {
            // check if modes are writable
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_CW_L))
                return false;
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_CW_U))
                return false;
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_SSB_L))
                return false;
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_SSB_U))
                return false;
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_DIG_L))
                return false;
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_DIG_U))
                return false;
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_AM))
                return false;
            if (!rig.IsParamWriteable(OMNIRIGPARAM.PM_FM))
                return false;

            return true;
        }

        private bool IntSetSplitMode(dynamic rig, long rxfreq, long txfreq, RIGMODE mode = RIGMODE.NONE)
        {
            //set rx and tx frequencies and split
            try
            {
                // try to set mode on both Vfos via VfoEqual which is the easiest way
                if (rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOEQUAL))
                {
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                        rig.Vfo = OMNIRIGPARAM.PM_VFOEQUAL;
                    }
                }

                if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQB) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOAB))
                {
                    // TS-570
                    rig.Vfo = OMNIRIGPARAM.PM_VFOAB;
                    rig.FreqA = rxfreq;
                    rig.FreqB = txfreq;
                    // try to set mode once for both VFOs
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                    }
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOEQUAL))
                {
                    // IC-746
                    rig.Freq = txfreq;
                    // should work fine for both VFOs because of VfoEqual
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                    }
                    rig.Vfo = OMNIRIGPARAM.PM_VFOEQUAL;
                    rig.Freq = rxfreq;
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOB) && rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOA))
                {
                    // FT-100D
                    rig.Vfo = OMNIRIGPARAM.PM_VFOB;
                    rig.Freq = txfreq;
                    // should work fine for both VFOs because of switching to both VFOs
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                    }
                    rig.Vfo = OMNIRIGPARAM.PM_VFOA;
                    rig.Freq = rxfreq;
                    // should work fine for both VFOs because of switching to both VFOs
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                    }
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOSWAP))
                {
                    // FT-817 ?
                    rig.Vfo = OMNIRIGPARAM.PM_VFOSWAP;
                    rig.Freq = txfreq;
                    // should work fine for both VFOs because of VfoSwap
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                    }
                    rig.Vfo = OMNIRIGPARAM.PM_VFOSWAP;
                    rig.Freq = rxfreq;
                    // should work fine for both VFOs because of VfoSwap
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                    }
                }
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQB) && rig.IsParamWriteable(OMNIRIGPARAM.PM_VFOA))
                {
                    //FT-1000 MP, IC-7610
                    rig.Vfo = OMNIRIGPARAM.PM_VFOA;
                    rig.FreqA = rxfreq;
                    rig.FreqB = txfreq;
                    // can set only mode on VFO A
                    if (CanWriteMode(rig) && (mode != RIGMODE.NONE))
                    {
                        rig.Mode = mode;
                    }

                }
                // Added by RA6UAZ for Icom Marine Radio NMEA Command
                else if (rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQ) && !rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQA) && rig.IsParamWriteable(OMNIRIGPARAM.PM_FREQB))
                {
                    rig.Freq = rxfreq;
                    rig.FreqB = txfreq;
                }

                if (rig.IsParamWriteable(OMNIRIGPARAM.PM_SPLITON))
                {
                    rig.Split = OMNIRIGPARAM.PM_SPLITON;
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool SetSplitMode(long rxfreq, long txfreq, RIGMODE mode = RIGMODE.NONE)
        {
            // we cannot use the "buil-in" split function from OmniRig as it affects the Rit/Xit
            if (OmniRigEngine == null)
                StartOmniRig();

            // check bounds as OmniRig ActiveX only supperts 32bit values
            if ((rxfreq < Int32.MinValue) || (rxfreq > int.MaxValue) || (txfreq < Int32.MinValue) || (txfreq > int.MaxValue))
                return false;

            try
            {
                // V1.19
                if (Settings.Type == OmniRigX.RigType1)
                {
                    return IntSetSplitMode(OmniRigX.OmniRigEngine.Rig1, rxfreq, txfreq, mode);
                }
                else if (Settings.Type == OmniRigX.RigType2)
                {
                    return IntSetSplitMode(OmniRigX.OmniRigEngine.Rig2, rxfreq, txfreq, mode);
                }
                // V2.x
                if (Is2x())
                {
                    if (Settings.Type == OmniRigX.RigType3)
                    {
                        return IntSetSplitMode(OmniRigX.OmniRigEngine.Rig3, rxfreq, txfreq, mode);
                    }
                    else if (Settings.Type == OmniRigX.RigType4)
                    {
                        return IntSetSplitMode(OmniRigX.OmniRigEngine.Rig4, rxfreq, txfreq, mode);
                    }
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


        // provides a list of supported rigs
        public static List<SupportedRig> SupportedRigs()
        {
            List<SupportedRig> l = new List<SupportedRig>();

            // check for OmniRigEngine is accessible
            try
            {
                if (OmniRigEngine == null)
                {
                    // do nothing
                }
            }
            catch
            {
                OmniRigEngine = null;
            }

            // start OmniRig engine
            if (OmniRigEngine == null)
            {
                StartOmniRig();
            }

            if (OmniRigEngine != null)
            {
                // setup OmniRig virtual Rigs according to OmniRig version

                // legacy V1.19 --> supports only 2 rigs
                if (OmniRigEngine.InterfaceVersion == 0x101)
                {
                    SupportedRig rig = new SupportedRig();
                    rig.Type = OmniRigX.RigType1;
                    rig.Model = OmniRigX.OmniRigEngine.Rig1.RigType;
                    rig.CATEngine = CATENGINE.OMNIRIGX;
                    l.Add(rig);
                    rig = new SupportedRig();
                    rig.Type = OmniRigX.RigType2;
                    rig.Model = OmniRigX.OmniRigEngine.Rig2.RigType;
                    rig.CATEngine = CATENGINE.OMNIRIGX;
                    l.Add(rig);
                }
                // version 2.xx --> supports 4 rigs
                else if ((OmniRigEngine.InterfaceVersion < 0x210) && (OmniRigEngine.InterfaceVersion > 0x299))
                {
                    SupportedRig rig = new SupportedRig();
                    rig.Type = OmniRigX.RigType1;
                    rig.Model = OmniRigX.OmniRigEngine.Rig1.RigType;
                    rig.CATEngine = CATENGINE.OMNIRIGX;
                    l.Add(rig);
                    rig = new SupportedRig();
                    rig.Type = OmniRigX.RigType2;
                    rig.Model = OmniRigX.OmniRigEngine.Rig2.RigType;
                    rig.CATEngine = CATENGINE.OMNIRIGX;
                    l.Add(rig);
                    rig = new SupportedRig();
                    rig.Type = OmniRigX.RigType3;
                    rig.Model = OmniRigX.OmniRigEngine.Rig3.RigType;
                    rig.CATEngine = CATENGINE.OMNIRIGX;
                    l.Add(rig);
                    rig = new SupportedRig();
                    rig.Type = OmniRigX.RigType4;
                    rig.Model = OmniRigX.OmniRigEngine.Rig4.RigType;
                    rig.CATEngine = CATENGINE.OMNIRIGX;
                    l.Add(rig);
                }

            }

            // return list of rigs
            return l;
        }


    }
}
