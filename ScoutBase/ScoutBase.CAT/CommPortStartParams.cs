using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScoutBase.CAT
{
    // CommPort background worker startup commands
    public class CommPortStartParams
    {
        public int RigNumber { get; set; } = 0;
        public RigCommands RigCommands { get; set; } = new RigCommands();
        public int PollMs { get; set; } = 1000;
        public int TimeoutMs { get; set; } = 5000;
        public int COMPollMs { get; set; } = 20;
    }
}
