using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.CAT
{
    // set of possible rig params
    public enum RigParam
    {
        pmNone,
        pmFreq, pmFreqA, pmFreqB, pmPitch, pmRitOffset, pmRit0,
        pmVfoAA, pmVfoAB, pmVfoBA, pmVfoBB, pmVfoA, pmVfoB, pmVfoEqual, pmVfoSwap,
        pmSplitOn, pmSplitOff,
        pmRitOn, pmRitOff,
        pmXitOn, pmXitOff,
        pmRx, pmTx,
        pmCW_U, pmCW_L, pmSSB_U, pmSSB_L, pmDIG_U, pmDIG_L, pmAM, pmFM
    }

    // provide different sets of RigParam values (static and readonly)
    // for Check use xxx.Contains(param)
    public static class RigParams
    {
        public static HashSet<RigParam> AllParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>();
                Array values = Enum.GetValues(typeof(RigParam));
                foreach (RigParam p in values)
                {
                    parms.Add(p);
                }
                return parms;
            }
        }

        public static HashSet<RigParam> NumericParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>() { RigParam.pmFreq, RigParam.pmFreqA, RigParam.pmFreqB, RigParam.pmPitch, RigParam.pmRitOffset }; 
                return parms;
            }
        }

        public static HashSet<RigParam> VfoParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>() { RigParam.pmVfoAA, RigParam.pmVfoAB, RigParam.pmVfoBA, RigParam.pmVfoBB, RigParam.pmVfoA, RigParam.pmVfoB, RigParam.pmVfoEqual, RigParam.pmVfoSwap };
                return parms;
            }
        }

        public static HashSet<RigParam> SplitOnParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>() { RigParam.pmSplitOn, RigParam.pmSplitOff };
                return parms;
            }
        }

        public static HashSet<RigParam> RitOnParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>() { RigParam.pmRitOn, RigParam.pmRitOff };
                return parms;
            }
        }

        public static HashSet<RigParam> XitOnParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>() { RigParam.pmXitOn, RigParam.pmXitOff };
                return parms;
            }
        }

        public static HashSet<RigParam> TxParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>() { RigParam.pmRx, RigParam.pmTx };
                return parms;
            }
        }

        public static HashSet<RigParam> ModeParams
        {
            get
            {
                HashSet<RigParam> parms = new HashSet<RigParam>() { RigParam.pmCW_U, RigParam.pmCW_L, RigParam.pmSSB_U, RigParam.pmSSB_L, RigParam.pmDIG_U, RigParam.pmDIG_L, RigParam.pmAM, RigParam.pmFM };
                return parms;
            }
        }
    }

    public class RigParamSet : HashSet<RigParam>
    {

    }

}
