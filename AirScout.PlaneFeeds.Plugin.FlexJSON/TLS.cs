using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net
{
    // result of ReadChunkedContent
    public enum ReadResult
    {
        Error = -1,
        MoreData = 0,
        Finished = 0
    }

    public class TlsClient : DefaultTlsClient
    {
        string HostName;

        public TlsClient(string hostname)
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

        private static ReadResult ReadContent(Stream stream, int contentlength, int timeout, ref string response)
        {
            // set stop watch as timout
            Stopwatch st = new Stopwatch();
            st.Start();
            string resp = "";
            int count = 0;
            try
            {
                // assign buffer
                byte[] buff = new byte[1];
                int bytesread = 0;
                // read content bytewise
                while (bytesread < contentlength)
                {
                    bytesread += stream.Read(buff, 0, buff.Length);
                    // add it to response
                    resp += Encoding.ASCII.GetString(buff, 0, buff.Length);
                    if (st.ElapsedMilliseconds > timeout)
                        throw new TimeoutException("Connection timed out.");
                }
                string trailer = "";
                // reassign buffer
                buff = new byte[1];
                // read stream bytewise until CRLFCRLF is detected, should be the next two bytes
                do
                {
                    count = stream.Read(buff, 0, buff.Length);
                    trailer += Encoding.ASCII.GetString(buff, 0, buff.Length);
                    if (st.ElapsedMilliseconds > timeout)
                        throw new TimeoutException("Connection timed out.");
                }
                while (!trailer.Contains("\r\n"));
                //            Console.WriteLine("Reading content [" + contentlength.ToString() + " bytes]: " + resp);
                response += resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading chunked content: " + ex.Message);
                return ReadResult.Error;

            }
            return ReadResult.MoreData;
        }

        private static ReadResult ReadChunkedContent(Stream stream, int timeout, ref string response)
        {
            // set stop watch as timout
            Stopwatch st = new Stopwatch();
            st.Start();
            string resp = "";
            byte[] buff = new byte[1];
            int count = 0;
            string strcontentlength = "";
            int contentlength = 0;
            // chunked transfer, first line should contain content length
            // read stream bytewise until CRLF is detected
            try
            {
                do
                {
                    count = stream.Read(buff, 0, buff.Length);
                    strcontentlength += Encoding.ASCII.GetString(buff, 0, buff.Length);
                    if (st.ElapsedMilliseconds > timeout)
                        throw new TimeoutException("Connection timed out.");
                }
                while (!strcontentlength.Contains("\r\n"));
                strcontentlength = strcontentlength.Replace("\r\n", "");
                contentlength = int.Parse(strcontentlength, System.Globalization.NumberStyles.HexNumber);
                // finished reading all chunks
                if (contentlength == 0)
                {
                    Console.WriteLine("Reading chunked content finished");
                    return ReadResult.Finished;
                }
                int bytesread = 0;
                // read content bytewise
                while (bytesread < contentlength)
                {
                    bytesread += stream.Read(buff, 0, buff.Length);
                    // add it to response
                    resp += Encoding.ASCII.GetString(buff, 0, buff.Length);
                    if (st.ElapsedMilliseconds > timeout)
                        throw new TimeoutException("Connection timed out.");
                }
                string trailer = "";
                // reassign buffer
                buff = new byte[1];
                // read stream bytewise until CRLFCRLF is detected, should be the next two bytes
                do
                {
                    count = stream.Read(buff, 0, buff.Length);
                    trailer += Encoding.ASCII.GetString(buff, 0, buff.Length);
                    if (st.ElapsedMilliseconds > timeout)
                        throw new TimeoutException("Connection timed out.");
                }
                while (!trailer.Contains("\r\n"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading chunked content: " + ex.Message);
                return ReadResult.Error;
            }
            // Console.WriteLine("Reading chunked content [" + contentlength.ToString() + " bytes]: " + resp);
            response += resp;
            return ReadResult.MoreData;
        }

        public static string DownloadFile(string url, int timeout, string apikey = "")
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
                var cl = new TlsClient(uri.Host);
                var protocol = new TlsClientProtocol(client.GetStream(), sr);
                protocol.Connect(cl);

                using (var stream = protocol.Stream)
                {
                    var hdr = new StringBuilder();
                    hdr.AppendLine("GET " + uri.PathAndQuery + " HTTP/1.1");
                    hdr.AppendLine("Host: " + uri.Host);
                    hdr.AppendLine("Content-Type: text/json; charset=utf-8");
                    if (!String.IsNullOrEmpty(apikey))
                    {
                        hdr.AppendLine("api-auth:" + apikey);
                    }
                    hdr.AppendLine("Connection: close");
                    hdr.AppendLine();

                    var dataToSend = Encoding.ASCII.GetBytes(hdr.ToString());

                    stream.Write(dataToSend, 0, dataToSend.Length);

                    byte[] buff;
                    // set stop watch as timout
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    //read header bytewise
                    string header = "";
                    int totalRead = 0;
                    buff = new byte[1];
                    do
                    {
                        totalRead = stream.Read(buff, 0, buff.Length);
                        header += Encoding.ASCII.GetString(buff);
                        if (st.ElapsedMilliseconds > timeout)
                            throw new TimeoutException("Connection to " + url + " timed out.");
                    }
                    while (!header.Contains("\r\n\r\n"));
                    Console.Write(header);
                    int contentlength = 0;
                    if (header.Contains("Transfer-Encoding: chunked"))
                    {
                        // chunked transfer, read all chunks until complete
                        while (ReadChunkedContent(stream, timeout, ref response) == ReadResult.MoreData) 
                        { }
                    }
                    else
                    {
                        // get content length from header
                        Regex rcontentlength = new Regex("(?<=Content-Length:\\s)\\d+", RegexOptions.IgnoreCase);
                        contentlength = int.Parse(rcontentlength.Match(header).Value);
                        ReadContent(stream, contentlength, timeout, ref response);
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
