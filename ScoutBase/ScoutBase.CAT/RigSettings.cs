using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScoutBase.CAT
{
    public class RigSettings
    {
        // rig type name
        public string RigType { get; set; } = "";

        // this is for compatibility with OmniRig under Windows only
        public int Port { 
            get 
            {
                // try to extract COMxx number from PortName
                // should work on Windows
                int port = 0;

                if (Helpers.IsMono)
                    return 0;

                string portnumber = PortName.Replace("COM", "");
                int.TryParse(portnumber, out port);

                return port;
            } 
            set
            {
                // try to extract COM-number from PortName
                // should work on Windows

                // try to set COMxx name from Port
                // should work on Windows

                if (!Helpers.IsMono)
                {
                    PortName = "COM" + Port.ToString();
                }
                    
            }
        }

        // port settings
        public string PortName { get; set; } = "";
        public int Baudrate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public Parity Parity { get; set; } = Parity.ptNone;
        public StopBits StopBits { get; set; } = StopBits.sbOne;
        public FlowControl RtsMode { get; set; } = FlowControl.fcLow;
        public FlowControl DtrMode { get; set; } = FlowControl.fcLow;

        // time settings
        public int PollMs { get; set; } = 1000;
        public int TimeoutMs { get; set; } = 5000;

        public static RigSettings FromRig (Rig rig)
        {
            RigSettings settings = new RigSettings();
            try
            {
                settings.RigType = rig.RigCommands.RigType;
                settings.PortName = rig.ComPort.PortName;
                settings.Baudrate = rig.ComPort.BaudRate;
                settings.DataBits = rig.ComPort.DataBits;
                settings.Parity = rig.ComPort.Parity;
                settings.StopBits = rig.ComPort.StopBits;
                settings.RtsMode = rig.ComPort.RtsMode;
                settings.DtrMode = rig.ComPort.DtrMode;
                settings.PollMs = rig.PollMs;
                settings.TimeoutMs = rig.TimeoutMs;
            }
            catch
            {
                // do nothing
            }

            return settings;
        }

        public void ToRig(Rig rig)
        {
            // do nothing if rig is invalid
            if (rig == null)
                return;

            // get rig enabled/disabled
            bool oldenabled = rig.Enabled;

            // if enabled --> disable rig first
            if (oldenabled)
                rig.Enabled = false;

            try
            {
                // write settings to rig
                rig.RigCommands = OmniRig.CommandsFromRigType(RigType);

                rig.ComPort.PortName = PortName;
                rig.ComPort.BaudRate = Baudrate;
                rig.ComPort.DataBits = DataBits;
                rig.ComPort.Parity = Parity;
                rig.ComPort.StopBits = StopBits;
                rig.ComPort.RtsMode = RtsMode;
                rig.ComPort.DtrMode = DtrMode;

                rig.PollMs = PollMs;
                rig.TimeoutMs = TimeoutMs;
            }
            finally
            {
                // enable the rig if previously enabled
                if (oldenabled)
                    rig.Enabled = true;
            }
        }
    }
}
