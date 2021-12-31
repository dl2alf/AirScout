using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace AirScout.CAT
{

    // can handle different versions dynamically
    public static class ExtOmniRig
    {

        private static dynamic OmniRigEngine = null;
        private static string OmniRigVersion = "";
        private static OMNIRIGSTATUS Status = OMNIRIGSTATUS.ST_NOTCONFIGURED;
        private static int Retries = 0;

        public static RIGSTATUS GetRigStatus(string rig)
        {
            try
            {
                OMNIRIGSTATUS omnistat = OMNIRIGSTATUS.ST_NOTCONFIGURED;

                if (rig == "OmniRig Rig 1")
                {
                    omnistat = (OMNIRIGSTATUS)ExtOmniRig.OmniRigEngine.Rig1.Status;
                }
                else if (rig == "OmniRig Rig 2")
                {
                    omnistat = (OMNIRIGSTATUS)ExtOmniRig.OmniRigEngine.Rig2.Status;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                    omnistat = (OMNIRIGSTATUS)ExtOmniRig.OmniRigEngine.Rig3.Status;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                    omnistat = (OMNIRIGSTATUS)ExtOmniRig.OmniRigEngine.Rig4.Status;
                }

                // suppress sporadic "offline" status
                if (omnistat != OMNIRIGSTATUS.ST_ONLINE)
                {
                    if (Retries < 3)
                    {
                        omnistat = Status;
                        Retries++;
                    }
                }
                else
                {
                    Status = omnistat;
                    Retries = 0;
                }


                // translate OmniRig status to rig status
                switch (omnistat)
                {
                    case OMNIRIGSTATUS.ST_ONLINE:
                        return RIGSTATUS.ONLINE;
                    case OMNIRIGSTATUS.ST_NOTRESPONDING:
                        return RIGSTATUS.OFFLINE;
                    default:
                        return RIGSTATUS.ERROR;
                }

            }
            catch (Exception ex)
            {
                // do nothing
            }

            return RIGSTATUS.ERROR;
        }

        public static RIGRIT GetRit(string rig)
        {
            try
            {
                if (rig == "OmniRig Rig 1")
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig1.Rit)
                    {
                        case OMNIRIGPARAM.PM_RITOFF:
                            return RIGRIT.RITOFF;

                        case OMNIRIGPARAM.PM_RITON:
                            return RIGRIT.RITON;
                        default:
                            return RIGRIT.ERROR;
                    }
                }
                else if (rig == "OmniRig Rig 2")
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig2.Rit)
                    {
                        case OMNIRIGPARAM.PM_RITOFF:
                            return RIGRIT.RITOFF;

                        case OMNIRIGPARAM.PM_RITON:
                            return RIGRIT.RITON;
                        default:
                            return RIGRIT.ERROR;
                    }
                }
                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                switch ((OMNIRIGPARAM)OmniRigEngine.Rig3.Rit)
                {
                    case OMNIRIGPARAM.PM_RITOFF:
                        return RIGRIT.RITOFF;

                    case OMNIRIGPARAM.PM_RITON:
                        return RIGRIT.RITON;
                    default:
                        return RIGRIT.ERROR;
                }
            }
            else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                switch ((OMNIRIGPARAM)OmniRigEngine.Rig4.Rit)
                {
                    case OMNIRIGPARAM.PM_RITOFF:
                        return RIGRIT.RITOFF;

                    case OMNIRIGPARAM.PM_RITON:
                        return RIGRIT.RITON;
                    default:
                        return RIGRIT.ERROR;
                }
            }
        }
            catch (Exception ex)
            {
                // do nothing
            }
            return RIGRIT.ERROR; ;
        }

        public static RIGSPLIT GetSplit(string rig)
        {
            try
            {
                if (rig == "OmniRig Rig 1")
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig1.Split)
                    {
                        case OMNIRIGPARAM.PM_SPLITOFF:
                            return RIGSPLIT.SPLITOFF;
                        case OMNIRIGPARAM.PM_SPLITON:
                            return RIGSPLIT.SPLITON;
                        default:
                            return RIGSPLIT.ERROR;
                    }
                }
                else if (rig == "OmniRig Rig 2")
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig2.Split)
                    {
                        case OMNIRIGPARAM.PM_SPLITOFF:
                            return RIGSPLIT.SPLITOFF;
                        case OMNIRIGPARAM.PM_SPLITON:
                            return RIGSPLIT.SPLITON;
                        default:
                            return RIGSPLIT.ERROR;
                    }
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig3.Split)
                    {
                        case OMNIRIGPARAM.PM_SPLITOFF:
                            return RIGSPLIT.SPLITOFF;
                        case OMNIRIGPARAM.PM_SPLITON:
                            return RIGSPLIT.SPLITON;
                        default:
                            return RIGSPLIT.ERROR;
                    }
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig4.Split)
                    {
                        case OMNIRIGPARAM.PM_SPLITOFF:
                            return RIGSPLIT.SPLITOFF;
                        case OMNIRIGPARAM.PM_SPLITON:
                            return RIGSPLIT.SPLITON;
                        default:
                            return RIGSPLIT.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
            return RIGSPLIT.ERROR; ;
        }

        public static RIGMODE GetMode(string rig)
        {
            try
            {
                if (rig == "OmniRig Rig 1")
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig1.Mode)
                    {
                        case OMNIRIGPARAM.PM_CW_L:
                            return RIGMODE.CW;
                        case OMNIRIGPARAM.PM_CW_U:
                            return RIGMODE.CW_R;
                        case OMNIRIGPARAM.PM_SSB_L:
                            return RIGMODE.LSB;
                        case OMNIRIGPARAM.PM_SSB_U:
                            return RIGMODE.USB;
                        case OMNIRIGPARAM.PM_FM:
                            return RIGMODE.FM;
                        case OMNIRIGPARAM.PM_AM:
                            return RIGMODE.AM;
                        default:
                            return RIGMODE.OTHER;
                    }
                }
                else if (rig == "OmniRig Rig 2")
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig2.Mode)
                    {
                        case OMNIRIGPARAM.PM_CW_L:
                            return RIGMODE.CW;
                        case OMNIRIGPARAM.PM_CW_U:
                            return RIGMODE.CW_R;
                        case OMNIRIGPARAM.PM_SSB_L:
                            return RIGMODE.LSB;
                        case OMNIRIGPARAM.PM_SSB_U:
                            return RIGMODE.USB;
                        case OMNIRIGPARAM.PM_FM:
                            return RIGMODE.FM;
                        case OMNIRIGPARAM.PM_AM:
                            return RIGMODE.AM;
                        default:
                            return RIGMODE.OTHER;
                    }
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig3.Mode)
                    {
                        case OMNIRIGPARAM.PM_CW_L:
                            return RIGMODE.CW;
                        case OMNIRIGPARAM.PM_CW_U:
                            return RIGMODE.CW_R;
                        case OMNIRIGPARAM.PM_SSB_L:
                            return RIGMODE.LSB;
                        case OMNIRIGPARAM.PM_SSB_U:
                            return RIGMODE.USB;
                        case OMNIRIGPARAM.PM_FM:
                            return RIGMODE.FM;
                        case OMNIRIGPARAM.PM_AM:
                            return RIGMODE.AM;
                        default:
                            return RIGMODE.OTHER;
                    }
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                    switch ((OMNIRIGPARAM)OmniRigEngine.Rig4.Mode)
                    {
                        case OMNIRIGPARAM.PM_CW_L:
                            return RIGMODE.CW;
                        case OMNIRIGPARAM.PM_CW_U:
                            return RIGMODE.CW_R;
                        case OMNIRIGPARAM.PM_SSB_L:
                            return RIGMODE.LSB;
                        case OMNIRIGPARAM.PM_SSB_U:
                            return RIGMODE.USB;
                        case OMNIRIGPARAM.PM_FM:
                            return RIGMODE.FM;
                        case OMNIRIGPARAM.PM_AM:
                            return RIGMODE.AM;
                        default:
                            return RIGMODE.OTHER;
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
            return RIGMODE.ERROR; ;
        }

        public static long GetRXFrequency(string rig)
        {
            try
            {
                if (rig == "OmniRig Rig 1")
                {
                    return (long)OmniRigEngine.Rig1.GetRxFrequency();
                }
                else if (rig == "OmniRig Rig 2")
                {
                    return (long)OmniRigEngine.Rig2.GetRxFrequency();
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                    return (long)OmniRigEngine.Rig3.GetRxFrequency();
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                    return (long)OmniRigEngine.Rig4.GetRxFrequency();
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
            return 0;
        }

        public static bool SetSplit(string rig, long rx, long tx)
        {
            try
            {
                if (rig == "OmniRig Rig 1")
                {
                    // OmniRig can handle Int32 only
                    if ((rx > Int32.MaxValue) || (tx > Int32.MaxValue))
                        return false;
                    OmniRigEngine.Rig1.Split = OMNIRIGPARAM.PM_SPLITON;
                    OmniRigEngine.Rig1.FreqA = (int)rx;
                    OmniRigEngine.Rig1.FreqB = (int)tx;
                    return true;
                }
                else if (rig == "OmniRig Rig 2")
                {
                    // OmniRig can handle Int32 only
                    if ((rx > Int32.MaxValue) || (tx > Int32.MaxValue))
                        return false;
                    OmniRigEngine.Rig2.Split = OMNIRIGPARAM.PM_SPLITON;
                    OmniRigEngine.Rig2.FreqA = (int)rx;
                    OmniRigEngine.Rig2.FreqB = (int)tx;
                    return true;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                    // OmniRig can handle Int32 only
                    if ((rx > Int32.MaxValue) || (tx > Int32.MaxValue))
                        return false;
                    OmniRigEngine.Rig3.Split = OMNIRIGPARAM.PM_SPLITON;
                    OmniRigEngine.Rig3.FreqA = (int)rx;
                    OmniRigEngine.Rig3.FreqB = (int)tx;
                    return true;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                    // OmniRig can handle Int32 only
                    if ((rx > Int32.MaxValue) || (tx > Int32.MaxValue))
                        return false;
                    OmniRigEngine.Rig4.Split = OMNIRIGPARAM.PM_SPLITON;
                    OmniRigEngine.Rig4.FreqA = (int)rx;
                    OmniRigEngine.Rig4.FreqB = (int)tx;
                    return true;
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }

            return false;

        }

        public static bool SetSimplex(string rig, long rx)
        {
            try
            {
                if (rig == "OmniRig Rig 1")
                {
                    // OmniRig can handle Int32 only
                    if (rx > Int32.MaxValue)
                        return false;
                    OmniRigEngine.Rig1.Split = OMNIRIGPARAM.PM_SPLITOFF;
                    OmniRigEngine.Rig1.FreqA = (int)rx;
                    return true;
                }
                else if (rig == "OmniRig Rig 2")
                {
                    // OmniRig can handle Int32 only
                    if (rx > Int32.MaxValue)
                        return false;
                    OmniRigEngine.Rig2.Split = OMNIRIGPARAM.PM_SPLITOFF;
                    OmniRigEngine.Rig2.FreqA = (int)rx;
                    return true;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                    // OmniRig can handle Int32 only
                    if (rx > Int32.MaxValue)
                        return false;
                    OmniRigEngine.Rig3.Split = OMNIRIGPARAM.PM_SPLITOFF;
                    OmniRigEngine.Rig3.FreqA = (int)rx;
                    return true;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                    // OmniRig can handle Int32 only
                    if (rx > Int32.MaxValue)
                        return false;
                    OmniRigEngine.Rig4.Split = OMNIRIGPARAM.PM_SPLITOFF;
                    OmniRigEngine.Rig4.FreqA = (int)rx;
                    return true;
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }

            return false;

        }

        public static bool SetRit(string rig, RIGRIT rit)
        {
            try
            {
                if (rig == "OmniRig Rig 1")
                {
                    OmniRigEngine.Rig1.Rit = (rit == RIGRIT.RITON) ? OMNIRIGPARAM.PM_RITON : OMNIRIGPARAM.PM_RITOFF;
                    return true;
                }
                else if (rig == "OmniRig Rig 2")
                {
                    OmniRigEngine.Rig2.Rit = (rit == RIGRIT.RITON) ? OMNIRIGPARAM.PM_RITON : OMNIRIGPARAM.PM_RITOFF;
                    return true;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 3")
                            )
                {
                    OmniRigEngine.Rig3.Rit = (rit == RIGRIT.RITON) ? OMNIRIGPARAM.PM_RITON : OMNIRIGPARAM.PM_RITOFF;
                    return true;
                }

                else if ((OmniRigEngine.InterfaceVersion >= 0x210) &&
                            (OmniRigEngine.InterfaceVersion >= 0x299) &&
                            (rig == "OmniRig 4")
                            )
                {
                    OmniRigEngine.Rig3.Rit = (rit == RIGRIT.RITON) ? OMNIRIGPARAM.PM_RITON : OMNIRIGPARAM.PM_RITOFF;
                    return true;
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }

            return false;

        }

    }

}
