using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.CAT
{
    public class CommReceived
    {
        public QueueItem Cmd { get; set; } = new QueueItem();
        public byte[] Received { get; set; } = new byte[0];

        public CommReceived()
        {

        }

        public CommReceived(QueueItem sent, byte[] received)
        {
            Cmd = sent;
            Received = received;
        }
    }
}
