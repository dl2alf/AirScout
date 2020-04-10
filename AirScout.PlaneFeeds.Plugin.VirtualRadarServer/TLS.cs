using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net
{
    class VRSTlsClient : DefaultTlsClient
    {
        string HostName;

        public VRSTlsClient(string hostname)
        {
            HostName = hostname;
        }

        public override TlsAuthentication GetAuthentication()
        {
            TlsAuthentication auth = new MyTlsAuthentication();
            return auth;
        }

        public override void NotifyNewSessionTicket(NewSessionTicket newSessionTicket)
        {
            base.NotifyNewSessionTicket(newSessionTicket);
        }

        public override IDictionary GetClientExtensions()
        {
            var clientExtensions = base.GetClientExtensions();
            List<ServerName> servers = new List<ServerName>();
            servers.Add(new ServerName(NameType.host_name, HostName));
            TlsExtensionsUtilities.AddServerNameExtension(clientExtensions, new ServerNameList(servers));
            return clientExtensions;
        }

        public static string DownloadFile (string url, int timeout, string apikey)
        {
            string response = "";
            Uri uri = null;
            // try to pasre url
            try
            {
                uri = new Uri(url);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            // create new TCP-Client
            using (var client = new TcpClient(uri.Host, uri.Port))
            {
                var sr = new SecureRandom();
                var cl = new VRSTlsClient(uri.Host);
                var protocol = new TlsClientProtocol(client.GetStream(), sr);
                protocol.Connect(cl);

                using (var stream = protocol.Stream)
                {
                    var hdr = new StringBuilder();
                    hdr.AppendLine("GET " + uri.PathAndQuery + " HTTP/1.1");
                    hdr.AppendLine("Host: " + uri.Host);
                    hdr.AppendLine("Content-Type: text/json; charset=utf-8");
                    hdr.AppendLine("api-auth:" + apikey);
                    hdr.AppendLine("Connection: close");
                    hdr.AppendLine();

                    var dataToSend = Encoding.ASCII.GetBytes(hdr.ToString());

                    stream.Write(dataToSend, 0, dataToSend.Length);
                    // set stop watch as timout
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    //read header bytewise
                    string header = "";
                    int totalRead = 0;
                    byte[] buff = new byte[1];
                    do
                    {
                        totalRead = stream.Read(buff, 0, buff.Length);
                        header += Encoding.ASCII.GetString(buff);
                        if (st.ElapsedMilliseconds > timeout)
                            throw new TimeoutException("Connection to " + url + " timed out.");
                    }
                    while (!header.Contains("\r\n\r\n"));
                    int contentlength = 0;
                    if (header.Contains("Transfer-Encoding: chunked"))
                    {
                        // chunked transfer, first line should contain content length
                        string strcontentlength = "";
                        do
                        {
                            totalRead = stream.Read(buff, 0, buff.Length);
                            strcontentlength += Encoding.ASCII.GetString(buff, 0, buff.Length);
                            if (st.ElapsedMilliseconds > timeout)
                                throw new TimeoutException("Connection to " + url + " timed out.");
                        }
                        while (!strcontentlength.Contains("\r\n"));
                        strcontentlength = strcontentlength.Replace("\r\n", "");
                        contentlength = int.Parse(strcontentlength, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        // get content length from header
                        Regex strcontentlength = new Regex("(?<=Content-Length:\\s)\\d+", RegexOptions.IgnoreCase);
                        contentlength = int.Parse(strcontentlength.Match(header).Value);
                    }
                    // re-assign buffer
                    // read response
                    buff = new byte[1000];
                    totalRead = 0;
                    do
                    {
                        int bytesRead = stream.Read(buff, 0, buff.Length);
                        string part = Encoding.UTF8.GetString(buff, 0, bytesRead);
                        Console.WriteLine(part);
                        response += part;
                        totalRead += bytesRead;
                        if (st.ElapsedMilliseconds > timeout)
                            throw new TimeoutException("Connection to " + url + " timed out.");
                    }
                    while (totalRead < contentlength);
                    // cut response at the end
                    if (response.Contains("\r\n"))
                    {
                        response = response.Substring(0, response.IndexOf("\r\n"));
                    }
                    st.Stop();
                }
            }
            return response;
        }
    }

    class MyTlsAuthentication : TlsAuthentication
    {
        public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest) 
        { 
            return null; 
        }

        public void NotifyServerCertificate(Certificate serverCertificate) 
        { 
        }
    }

}
