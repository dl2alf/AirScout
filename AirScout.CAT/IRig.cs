using ScoutBase.CAT;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace AirScout.CAT
{
    public class RigSettings
    {
        // rig type name
        public string Type { get; set; } = "";
        public string Model { get; set; } = "";

        // port settings
        public string PortName { get; set; } = "";
        public int Baudrate { get; set; } = 0;
        public int DataBits { get; set; } = 0;
        public PARITY Parity { get; set; } = PARITY.NONE;
        public STOPBITS StopBits { get; set; } = STOPBITS.ONE;
        public FLOWCONTROL RtsMode { get; set; } = FLOWCONTROL.LOW;
        public FLOWCONTROL DtrMode { get; set; } = FLOWCONTROL.LOW;

        // time settings
        public int PollMs { get; set; } = 1000;
        public int TimeoutMs { get; set; } = 5000;


    }

    //  repesents an interface to a rig connected via CAT interface
    // supports variuos kinds of connections
    public interface IRig
    {

        // Rig
        RigSettings Settings { get; set; }
        CATENGINE CatEngine { get; }
        string CatVersion { get; }

        // rig status, read-only
        RIGSTATUS GetStatus();

        // get rig values
        RIGMODE GetMode();
        RIGSPLIT GetSplit();
        RIGRIT GetRit();
        RIGVFO GetVfo();
        long GetRxFrequency();
        long GetTxFrequency();

        // set rig values
        bool SetMode(RIGMODE mode);
        bool SetSplit(RIGSPLIT split);
        bool SetRit(RIGRIT rit);
        bool SetVfo(RIGVFO vfo);
        bool SetRxFrequency(long rx);
        bool SetTxFrequency(long tx);

        // rig capabilities
        bool SetSimplexMode(long freq, RIGMODE mode = RIGMODE.NONE);
        bool SetSplitMode(long rxfreq, long txfreq, RIGMODE mode = RIGMODE.NONE);

        // doppler tracking
        bool SetDopplerStrategy(DOPPLERSTRATEGY doppler);
        bool EnterDoppler();
        bool LeaveDoppler();

    }
}
