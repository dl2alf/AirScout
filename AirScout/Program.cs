using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace AirScout
{
    static class Program
    {
        // Mutex to ensure that only one instance is running
        static System.Threading.Mutex singleton = new Mutex(true, Application.ProductName);
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            if (!singleton.WaitOne(TimeSpan.Zero, true))
            {
                //there is already another instance running!
                MessageBox.Show("AirScout is already running on this computer.","Application Check");
                Application.Exit();
            }
            else
            */
            {
                // run program normally
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MapDlg());
            }
        }
    }
}
