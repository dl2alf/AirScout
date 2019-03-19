
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace AirScoutPlaneServer
{

    public partial class MainDlg : Form
    {

        private void bw_Webserver_DoWork(object sender, DoWorkEventArgs e)
        {
            // run simple web server
            string hosturl = "http://+:" + Properties.Settings.Default.Webserver_Port.ToString() + "/";
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            while (!bw_Webserver.CancellationPending)
            {
                string[] prefixes = new string[1];
                prefixes[0] = hosturl;
                try
                {
                    if (!HttpListener.IsSupported)
                    {
                        Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                        return;
                    }
                    // URI prefixes are required,
                    if (prefixes == null || prefixes.Length == 0)
                        throw new ArgumentException("prefixes");

                    // Create a listener.
                    HttpListener listener = new HttpListener();
                    // Add the prefixes.
                    foreach (string s in prefixes)
                    {
                        listener.Prefixes.Add(s);
                    }
                    listener.Start();
                    Console.WriteLine("Listening...");
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a default response.
                    string responseString = Properties.Settings.Default.Webserver_Welcome;
                    if (request.RawUrl == "/planes.json")
                    {
                        try
                        {
                            var fs = File.OpenRead(TmpDirectory + Path.DirectorySeparatorChar + "planes.json");
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                responseString = sr.ReadToEnd();
                            }
                        }
                        catch
                        {
                            // do nothing
                        }
                    }
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                    listener.Stop();
                }
                catch (HttpListenerException ex)
                {
                    if (ex.ErrorCode == 5)
                    {
                        // gain additional access rights for that specific host url
                        // user will be prompted for allow changes
                        // DO NOT USE THE "listener=yes" option as recommended!!!
                        string args = "http add urlacl " + hosturl + " user=" + userName;
                        ProcessStartInfo psi = new ProcessStartInfo("netsh", args);
                        psi.Verb = "runas";
                        psi.CreateNoWindow = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = true;

                        Process.Start(psi).WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    // do almost nothing
                    // wait 10 seconds and restart the listener
                    Thread.Sleep(10000);
                }
                finally
                {
                }
            }
        }

        private void bw_Webserver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

    }
}
