using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using WinTest;

namespace WinTestSimulator
{
    public partial class MainDlg : Form
    {
        public MainDlg()
        {
            InitializeComponent();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            WTMESSAGES msg = WTMESSAGES.NONE;
            Enum.TryParse<WTMESSAGES>(Properties.Settings.Default.Message.ToUpper().Trim(), out msg);
            wtMessage SendMsg = new wtMessage(msg,
            Properties.Settings.Default.From,
            Properties.Settings.Default.To,
            Properties.Settings.Default.Data);
            SendMsg.HasChecksum = true;
            // send message
            UdpClient client = new UdpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            client.Client.ReceiveTimeout = 10000;    // 10s Receive timeout
            IPEndPoint groupEp = new IPEndPoint(IPAddress.Broadcast, (int)Properties.Settings.Default.Port);
            client.Connect(groupEp);
            byte[] b = SendMsg.ToBytes();
            client.Send(b, b.Length);
            client.Close();

        }
    }
}
