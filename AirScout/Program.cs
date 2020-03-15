using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace AirScout
{
    static class Program
    {

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

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
            try
            {
                // redirect console output to parent process;
                // must be before any calls to Console.WriteLine()
                // this will crash on Linux/Mono for sure --> handle exception
                AttachConsole(ATTACH_PARENT_PROCESS);
            }
            catch (Exception ex)
            {
                // do nothing
            }
                // run program normally
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MapDlg());
        }
    }
}
