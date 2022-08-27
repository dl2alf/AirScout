// (c) 2016 DL2ALF
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using Ionic.Zip;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace ScoutBase.Core
{

    [System.ComponentModel.DesignerCategory("")]
    public static class SupportFunctions : Object
    {
        public static double TodB(double value)
        {
            // calulate dB from linear value
            return 10.0f * Math.Log10(value);
        }

        public static double ToLinear(double value)
        {
            // calculate linear value from dB
            return Math.Pow(10, value / 10.0f);
        }

        /// <summary>
        /// Returns true if running under Linux/Mono
        /// </summary>
        public static bool IsMono
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        /// <summary>
        /// Returns true if running under a 64bit configuration
        /// </summary>
        public static bool Is64BitConfiguration()
        {
            return System.IntPtr.Size == 8;
        }

        [System.ComponentModel.DesignerCategory("")]
        public static class CPUCounter
        {
            static PerformanceCounter cpucounter;

            static CPUCounter()
            {
                cpucounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }

            public static double GetLoad()
            {
                // returns the current overall CPU load
                return cpucounter.NextValue();
            }
        }

        [System.ComponentModel.DesignerCategory("")]
        public static class MemoryCounter
        {
            static PerformanceCounter available;

            static MemoryCounter()
            {
                available = new PerformanceCounter("Memory", "Available MBytes");
            }

            /// <summary>
            /// Get the amount of available memory in MBytes
            /// </summary>
            /// <returns>The amount of memory available in MBytes.</returns>
            public static double GetAvailable()
            {
                // returns the current overall CPU load
                return available.NextValue();
            }
        }

        /// <summary>
        /// Checks whether a given path would be a vaild path to a directory.
        /// The path must be fully qualified.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True if the path is valid.</returns>
        public static bool ValidateDirectoryPath(string path)
        {
            // return false on empty or whitespaced path
            if (String.IsNullOrEmpty(path))
                return false;
            if (String.IsNullOrWhiteSpace(path))
                return false;
            string root = null; ;
            string directory = null;
            try
            {
                //throw ArgumentException   - The path parameter contains invalid characters, is empty, or contains only white spaces.
                root = Path.GetPathRoot(path);
                //throw ArgumentException   - path contains one or more of the invalid characters defined in GetInvalidPathChars.
                //    -or- String.Empty was passed to path.
                directory = Path.GetDirectoryName(path);
            }
            catch (ArgumentException)
            {
                return false;
            }
            //null if path is null, or an empty string if path does not contain root directory information
            if (String.IsNullOrEmpty(root))
                return false;
            //null if path denotes a root directory or is null. Returns String.Empty if path does not contain directory information
            if (String.IsNullOrEmpty(directory))
                return false;
            // path is valid at the end
            return true;
        }

        /// <summary>
        /// Checks whether a given path would be a vaild path to a file.
        /// The path must be fully qualified.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True if the path is valid.</returns>
        public static bool ValidateFilePath(string path)
        {
            // return false on empty or whitespaced path
            if (String.IsNullOrEmpty(path))
                return false;
            if (String.IsNullOrWhiteSpace(path))
                return false;
            string root = null; ;
            string directory = null;
            string filename = null;
            try
            {
                //throw ArgumentException   - The path parameter contains invalid characters, is empty, or contains only white spaces.
                root = Path.GetPathRoot(path);
                //throw ArgumentException   - path contains one or more of the invalid characters defined in GetInvalidPathChars.
                //    -or- String.Empty was passed to path.
                directory = Path.GetDirectoryName(path);
                //path contains one or more of the invalid characters defined in GetInvalidPathChars
                filename = Path.GetFileName(path);
            }
            catch (ArgumentException)
            {
                return false;
            }
            //null if path is null, or an empty string if path does not contain root directory information
            if (String.IsNullOrEmpty(root))
                return false;
            //null if path denotes a root directory or is null. Returns String.Empty if path does not contain directory information
            if (String.IsNullOrEmpty(directory))
                return false;
            // path is valid at the end
            return true;
        }

        /// <summary>
        /// Retrieves drive information about a specific drive
        /// </summary>
        /// <param name="drive">The drive letter, e.g. "C:\" (Windows) or "\" (Linux).</param>
        /// <returns>The DriveInfo object if drive was found, null if not.</returns>
        public static DriveInfo GetDriveInfo(string drive)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo d in drives)
            {
                if (d.IsReady)
                    Console.WriteLine("DriveInfo\n=============\nName=\"" + d.Name + "\"\nFormat=\"" + d.DriveFormat + "\"\nFree=" + d.AvailableFreeSpace);
                if (d.Name == drive)
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves file system of a specific drive
        /// </summary>
        /// <param name="drive">The drive letter, e.g. "C:\" (Windows) or "\" (Linux).</param>
        /// <returns>The file system identifier if drive was found, empty string if not.</returns>
        public static string GetDriveFileSystem(string drive)
        {
            DriveInfo d = GetDriveInfo(drive);
            // drive not found --> return 0
            if (d == null)
                return string.Empty;
            return d.DriveFormat;
        }

        /// <summary>
        /// Retrieves available free space of a specific drive
        /// </summary>
        /// <param name="drive">The drive letter, e.g. "C:\" (Windows) or "\" (Linux).</param>
        /// <returns>The available frees space [bytes] if drive was found, 0 if not.</returns>
        public static long GetDriveAvailableFreeSpace(string drive)
        {
            DriveInfo d = GetDriveInfo(drive);
            // drive not found --> return 0
            if (d == null)
                return 0;
            return d.AvailableFreeSpace;
        }

        /// <summary>
        /// Retrieves maxixum allowed file size on a specific file system
        /// </summary>
        /// <param name="filesystem">The file system identifier.</param>
        /// <returns>The available maximum file size [bytes] if filesystem is found on the list, 0 if not.</returns>
        public static long GetFileSystemGetMaxFileSize(string filesystem)
        {
            filesystem = filesystem.ToUpper().Trim();
            if (filesystem == "FAT16")
                return (long)2 * System.Convert.ToInt64(Math.Pow(2, 30));
            if (filesystem == "FAT32")
                return (long)4 * System.Convert.ToInt64(Math.Pow(2, 30)) - (long)1;
            if (filesystem == "NTFS")
                return (long)16 * System.Convert.ToInt64(Math.Pow(2, 40));
            if (filesystem == "EXT2")
                return (long)2 * System.Convert.ToInt64(Math.Pow(2, 40));
            if (filesystem == "EXT3")
                return (long)2 * System.Convert.ToInt64(Math.Pow(2, 40));
            if (filesystem == "EXT4")
                return (long)1 * System.Convert.ToInt64(Math.Pow(2, 60));
            if (filesystem == "BTRFS")
                return long.MaxValue;
            return 0;
        }

        /// <summary>
        /// Retrieves maxixum allowed file size on a specific file system
        /// </summary>
        /// <param name="drive">The drive letter, e.g. "C:\" (Windows) or "\" (Linux).</param>
        /// <returns>The available maximum file size [bytes] if drive and file system is found, 0 if not.</returns>
        public static long GetDriveMaxFileSize(string drive)
        {
            string filesystem = GetDriveFileSystem(drive).ToUpper().Trim();
            if (String.IsNullOrEmpty(filesystem))
                return 0;
            if (filesystem == "FAT16")
                return (long)2 * System.Convert.ToInt64(Math.Pow(2, 30));
            if (filesystem == "FAT32")
                return (long)4 * System.Convert.ToInt64(Math.Pow(2, 30)) - (long)1;
            if (filesystem == "NTFS")
                return (long)16 * System.Convert.ToInt64(Math.Pow(2, 40));
            if (filesystem == "EXT2")
                return (long)2 * System.Convert.ToInt64(Math.Pow(2, 40));
            if (filesystem == "EXT3")
                return (long)2 * System.Convert.ToInt64(Math.Pow(2, 40));
            if (filesystem == "EXT4")
                return (long)1 * System.Convert.ToInt64(Math.Pow(2, 60));
            if (filesystem == "BTRFS")
                return long.MaxValue;
            return 0;
        }

        /// <summary>
        /// Checks whether a given filename would be valid.
        /// The filename must not contain path information.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>True if the filename is valid.</returns>
        public static bool ValidateFileName(string filename)
        {
            // return false on empty or whitespaced path
            if (String.IsNullOrEmpty(filename))
                return false;
            if (String.IsNullOrWhiteSpace(filename))
                return false;
            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return false;
            return true;
        }

        /// <summary>
        /// Deletes files from a given directory which are matching a single filter pattern
        /// </summary>
        /// <param name="dir">The directory.</param>
        /// <param name="filter">The filter pattern.</param>
        /// <returns>Nothing.</returns>
        public static void DeleteFilesFromDirectory(string dir, string filter)
        {
            string[] files = Directory.GetFiles(dir, filter);
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeleteFilesFromDirectory: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Deletes files from a given directory which are matching multiple filter patterns
        /// </summary>
        /// <param name="dir">The directory.</param>
        /// <param name="filter">The filter patterns.</param>
        /// <returns>Nothing.</returns>
        public static void DeleteFilesFromDirectory(string dir, string[] filters)
        {
            string[] files = filters.SelectMany(f => Directory.GetFiles(dir, f)).ToArray();
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeleteFilesFromDirectory: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Writes a string into a file
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>Nothing.</returns>
        public static void WriteStringToFile(string str, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(str);
            }
        }

        /// <summary>
        /// Reads a string from a file
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The string.</returns>
        public static string ReadStringFromFile(string filename)
        {
            string s = String.Empty;
            using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
            {
                s = sr.ReadToEnd();
            }
            return s;
        }
        
        /// <summary>
        /// Converts an object to an array of bytes.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Array of bytes representing the object.</returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts an array of bytes into an object.
        /// </summary>
        /// <param name="arr">The array of bytes representing the object.</param>
        /// <returns>The object.</returns>
        public static Object ByteArrayToObject(byte[] arr)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arr, 0, arr.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        /// <summary>
        /// Converts a DateTime into UNIX Epoch time
        /// Handles MinValue and MaxValue correctly
        /// </summary>
        /// <param name="dt">The DateTime to be converted.</param>
        /// <returns>The UNIX Epoch time. Fractional seconds will be lost.</returns>
        public static int DateTimeToUNIXTime(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return int.MinValue;
            else if (dt == DateTime.MaxValue)
                return int.MaxValue;
            return (Int32)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// Converts a UNIX Epoch time into DateTime
        /// Handles MinValue and MaxValue correctly
        /// </summary>
        /// <param name="dt">The UNIX Epoch time to be converted.</param>
        /// <returns>The DateTime (in UTC).</returns>
        public static DateTime UNIXTimeToDateTime(int ut)
        {
            if (ut == int.MinValue)
                return DateTime.MinValue;
            else if (ut == int.MaxValue)
                return DateTime.MaxValue;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(ut);
        }

    }


    [System.ComponentModel.DesignerCategory("")]
    public static class UnitConverter
    {
        public static double ft_m(double feet)
        {
            return feet / 3.28084;
        }

        public static double m_ft(double m)
        {
            return m * 3.28084;
        }

        public static double kts_kmh(double kts)
        {
            return kts * 1.852;
        }

        public static double kmh_kts(double kmh)
        {
            return kmh / 1.852;
        }

        public static double km_mi(double km)
        {
            return km * 1.609;
        }

        public static double mi_km(double mi)
        {
            return mi / 1.609;
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    public class VarConverter : Dictionary<string, object>
    {
        public readonly char VarSeparator = '%';

        public void AddVar(string var, object value)
        {
            // adds a new var<>value pair to dictionary
            object o;
            if (this.TryGetValue(var, out o))
            {
                // item found --> update value
                o = value;
            }
            else
            {
                // item not found --> add new
                this.Add(var, value);
            }
        }

        public object GetValue(string var)
        {
            // finds a var in dictionary and returns its value
            object o;
            if (this.TryGetValue(var, out o))
            {
                // item found --> return value
                return o;
            }
            // item not found --> return null
            return null;
        }

        public string ReplaceAllVars(string s)
        {
            // check for var separotors first
            if (s.Contains(VarSeparator))
            {
                // OK, string is containing vars --> crack the string first and replace vars
                try
                {
                    string[] a = s.Split(VarSeparator);
                    // as we are always using a pair of separators the length of a[] must be odd
                    if (a.Length % 2 == 0)
                        throw new ArgumentException("Number of separators is not an even number.");
                    // create new string and replace all vars (on odd indices)
                    s = "";
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            // cannot be not a var on that position
                            s = s + a[i];
                        }
                        else
                        {
                            // var identifier: upper the string and try to convert
                            a[i] = a[i].ToUpper();
                            object o;
                            if (this.TryGetValue(a[i], out o))
                            {
                                // convert floating points with invariant culture info
                                if (o.GetType() == typeof(double))
                                    s = s + ((double)o).ToString(CultureInfo.InvariantCulture);
                                else if (o.GetType() == typeof(float))
                                    s = s + ((float)o).ToString(CultureInfo.InvariantCulture);
                                else
                                    s = s + o.ToString();
                            }
                            else
                            {
                                throw new ArgumentException("Var identifier not found: " + a[i]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // throw an excecption 
                    throw new ArgumentException("Error while parsing string for variables [" + ex.Message + "]: " + s);
                }
            }
            return s;
        }


    }

    [System.ComponentModel.DesignerCategory("")]
    public static class ZIP
    {
        public static bool UncompressFile(string filename, int timeout, string password = "")
        {
            // unzips a zip file content to the same directory
            string downloaddir = String.Empty;
            // get the directory correct under Windows & Linux
            // don't use Path.GetDirectory under Linux
            if (filename.IndexOf("\\") >= 0)
                downloaddir = filename.Substring(0, filename.LastIndexOf("\\"));
            else if (filename.IndexOf("/") >= 0)
                downloaddir = filename.Substring(0, filename.LastIndexOf("/"));
            // set path to calling assembly's path if not otherwise specified
            if (String.IsNullOrEmpty(downloaddir))
                downloaddir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            try
            {
                Console.WriteLine("[UnzipFile: Trying to unzip file: " + filename);
                // open the zip file
                using (ZipFile zip = ZipFile.Read(filename))
                {
                    if (!String.IsNullOrEmpty(password))
                        zip.Password = password;
                    // here, we extract every entry, but we could extract conditionally
                    // based on entry name, size, date, checkbox status, etc.  
                    foreach (ZipEntry ze in zip)
                    {
                        Console.WriteLine("[UnzipFile: unzipping entry: " + ze.FileName + " to: " + downloaddir);
                        ze.Extract(downloaddir, ExtractExistingFileAction.OverwriteSilently);
                        string fname = Path.Combine(downloaddir, ze.FileName);
                        // wait for extraction to finish
                        int i = 0;
                        while (!File.Exists(fname))
                        {
                            Thread.Sleep(1000);
                            i++;
                            if (i > timeout)
                            {
                                throw new TimeoutException("Timeout while waiting for extraction is complete: " + fname);
                            }
                        }
                        File.SetLastWriteTime(fname, ze.LastModified);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public static bool CompressFile(string filename, bool storedirectoryinarchive, int timeout, string password = "")
        {
            // unzips a zip file content to the same directory
            string directory = String.Empty;
            // get the directory correct under Windows & Linux
            // don't use Path.GetDirectory under Linux
            if (filename.IndexOf("\\") >= 0)
                directory = filename.Substring(0, filename.LastIndexOf("\\"));
            else if (filename.IndexOf("/") >= 0)
                directory = filename.Substring(0, filename.LastIndexOf("/"));
            // set path to calling assembly's path if not otherwise specified
            if (String.IsNullOrEmpty(directory))
                directory = Assembly.GetCallingAssembly().Location;
            try
            {
                Console.WriteLine("[ZipFile: Trying to zip file: " + filename);
                string zipfilename = Path.GetFileNameWithoutExtension(filename) + ".zip";
                // create the zip file
                using (ZipFile zip = new ZipFile())
                {
                    if (!String.IsNullOrEmpty(password))
                        zip.Password = password;
                    zip.AddFile(filename, storedirectoryinarchive ? directory : "");
                    zip.Save(Path.Combine(directory, zipfilename));
                }
                // wait for compression to finish
                int i = 0;
                while (!File.Exists(Path.Combine(directory, zipfilename)))
                {
                    Thread.Sleep(1000);
                    i++;
                    if (i > timeout)
                        throw new TimeoutException("Timeout while waiting for compression is complete: " + zipfilename);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }

    /// <summary>
    /// Derived WebClient class for automatic compression handling.
    /// Can download files from web resources with compressed content delivery.
    /// Tries to download various zipped versions from a given <filename.ext>, e.g. <filename.zip> or <filename.ext.zip>
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class AutoDecompressionWebClient : WebClient
    {
        public AutoDecompressionWebClient()
        {
            // add website compression request
            Headers.Add("Accept-Encoding: gzip, deflate");
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            // ovverides GetWebRequest to add automatic decompression
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            // allow redirect 2017/12/06 DL2ALF
            request.AllowAutoRedirect = true;
            return request;
        }

        /// <summary>
        /// Gets the creation time of a file
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns>The creation time in UTC if file found. DateTime.MinValue if not.</returns>
        public DateTime GetFileCreationTimeUtc(string filename)
        {
            if (File.Exists(filename))
                return File.GetCreationTimeUtc(filename);
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// Gets the creation time of a web resource.
        /// </summary>
        /// <param name="address">The address string of web resource.</param>
        /// <param name="allowredirect">Allows redirection of requested source.</param>
        /// <returns>The creation time in UTC if successful. DateTime.MinValue if not.</returns>
        public DateTime GetWebCreationTimeUtc(string address, bool allowredirect = true)
        {
            return GetWebCreationTimeUtc(new Uri(address), allowredirect);
        }

        /// <summary>
        /// Gets the creation time of a web resource.
        /// </summary>
        /// <param name="address">The address URI of web/file resource.</param>
        /// <param name="allowredirect">Allows redirection of requested source.</param>
        /// <returns>The creation time in UTC if successful. DateTime.MinValue if not.</returns>
        public DateTime GetWebCreationTimeUtc(Uri address, bool allowredirect = true)
        {
            // get the last modified time of the website/file
            // returns DateTime.MinValue if address not found
            try
            {

                DateTime webcreationtime = DateTime.MinValue;
                if (address.IsFile)
                {
                    if (File.Exists(address.LocalPath))
                    {
                        webcreationtime = File.GetLastWriteTimeUtc(address.LocalPath);
                    }
                }
                else
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(address);
                    // allow redirect 2017/12/06 DL2ALF
                    req.AllowAutoRedirect = allowredirect;
                    Console.WriteLine("[GetWebCreationTime] Waiting for response from address: " + address);
                    using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                    {
                        webcreationtime = res.LastModified.ToUniversalTime();
                        Console.WriteLine("[GetWebCreationTime] Getting web creation time from address: " + address + " = " + webcreationtime.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                 return webcreationtime;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GetWebCreationTime] Error while reading address: " + address + "\n" + ex.ToString());
            }
            return DateTime.MinValue;
        }

        private bool DownloadFileFromWeb(string address, string filename, bool allowredirect, bool autounzip, string password = "")
        {
            // donwloads file from a web/file resource
            try
            {
                Uri uri = new Uri(address);
                if (uri.IsFile)
                {
                    if (File.Exists(uri.LocalPath))
                    {
                        File.Copy(uri.LocalPath, filename, true);
                        if (autounzip && Path.GetExtension(filename).ToLower() == ".zip")
                        {
                            Console.WriteLine("[DownloadFileFromWeb] Trying to unzip downloaded file: " + filename);
                            return ZIP.UncompressFile(filename, 60, password);
                        }
                        Console.WriteLine("[DownloadFileFromWeb] Downloading file from address finished: " + address);
                        return true;
                    }
                }
                else
                {
                    // get web cration time
                    DateTime webcreationtime = GetWebCreationTimeUtc(address, allowredirect);
                    // download file and check for errors and uri identical to request
                    // do not use WebClient.Download for this!
                    var request = (HttpWebRequest)WebRequest.Create(address);
                    // allow redirect 2017/12/06 DL2ALF
                    request.AllowAutoRedirect = allowredirect;
                    request.Method = "GET";
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        HttpStatusCode status = response.StatusCode;
                        if ((status == HttpStatusCode.OK) && (response.ResponseUri == new Uri(address)))
                        {
                            using (var responseStream = response.GetResponseStream())
                            {
                                using (var fileToDownload = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                                {
                                    responseStream.CopyTo(fileToDownload);
                                }
                            }
                        }
                    }
                    // set creation time
                    if (File.Exists(filename))
                    {
                        File.SetCreationTime(filename, webcreationtime);
                        File.SetLastWriteTime(filename, webcreationtime);
                        // unzip the file content if enabled
                        if (autounzip && (Path.GetExtension(filename).ToLower() == ".zip"))
                        {
                            Console.WriteLine("[DownloadFileFromWeb] Trying to unzip downloaded file: " + filename);
                            return ZIP.UncompressFile(filename, 60, password);
                        }
                        Console.WriteLine("[DownloadFileFromWeb] Downloading file from address finished: " + address);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException )
                    Console.WriteLine("[DownloadFileFromWeb] WebException while reading address: " + address + "\n" + "URI of orginal request=" + address + "\n" + "URI of responding server=" + "\n" + ex.ToString());
                else
                    Console.WriteLine("[DownloadFileFromWeb] Error while reading address: " + address + "\n" + ex.ToString());
            }
            return false;
        }

        private DOWNLOADFILESTATUS DownloadFileFromWebIfNewer(string address, string filename, bool allowredirect, bool autounzip, string password = "")
        {
            DateTime filecreationtime;
            DateTime webcreationtime;
            if (File.Exists(filename))
                filecreationtime = File.GetCreationTimeUtc(filename);
            else
                filecreationtime = DateTime.MinValue;
            webcreationtime = GetWebCreationTimeUtc(address, allowredirect);
            // nothing found on web
            if (webcreationtime == DateTime.MinValue)
                return DOWNLOADFILESTATUS.NOTFOUND;     
            if (webcreationtime > filecreationtime)
            {
                //  web content is newer --> download and do not unzip
                if (!DownloadFileFromWeb(address, filename, allowredirect, false, password))
                    return DOWNLOADFILESTATUS.ERROR;
            }
            // unzip the file if enabled and content is a ZIP-file
            if (autounzip && (Path.GetExtension(filename).ToLower() == ".zip"))
            {
                if (!ZIP.UncompressFile(filename,60, password))
                    return DOWNLOADFILESTATUS.ERROR;
            }
            // set the return value
            if (webcreationtime > filecreationtime)
                return DOWNLOADFILESTATUS.NEWER;
            else
                return DOWNLOADFILESTATUS.NOTNEWER;
        }

        /// <summary>
        /// Downloads a file from a web resource.
        /// Sets local file time stamps according to original after download.
        /// </summary>
        /// <param name="address">The address of web resource.</param>
        /// <param name="filename">The filename for local store.</param>
        /// <param name="allowredirect">Allows redirection of requested source.</param>
        /// <param name="autounzip">Try to download a zipped version first.</param>
        /// <returns>True if download was successful.</returns>
        public bool DownloadFile(string address, string filename, bool allowredirect, bool autounzip, string password = "")
        {
            string downloadfilename;
            string downloadaddress;
            if (autounzip)
            {
                // try to download <filename.ext.zip> first
                downloadfilename = filename + ".zip";
                downloadaddress = address + ".zip";
                if (GetWebCreationTimeUtc(downloadaddress, allowredirect) != DateTime.MinValue)
                {
                    // resource found --> download the zip file and extract it
                    if (DownloadFileFromWeb(downloadaddress, downloadfilename, true,autounzip, password))
                        return true;
                }
                // try to download <filename.zip>
                downloadfilename = filename.Replace(Path.GetExtension(filename), ".zip");
                downloadaddress = address.Substring(0, address.LastIndexOf(".")) + ".zip";
                if (GetWebCreationTimeUtc(downloadaddress, allowredirect) != DateTime.MinValue)
                {
                    // resource found --> download the zip file and extract it
                    if (DownloadFileFromWeb(downloadaddress, downloadfilename, true, autounzip, password))
                        return true;
                }
            }
            // try to download <filename.ext>
            downloadfilename = filename;
            downloadaddress = address;
            if (GetWebCreationTimeUtc(downloadaddress, allowredirect) != DateTime.MinValue)
            {
                // resource found --> download the file
                if (DownloadFileFromWeb(downloadaddress, downloadfilename, allowredirect, true, password))
                    return true;
            }
            // nothing found
            return false;
        }

        /// <summary>
        /// Downloads a file from a web resource only if it is newer or not found locally.
        /// </summary>
        /// <param name="address">The address of web resource.</param>
        /// <param name="filename">The filename for local store.</param>
        /// <param name="allowredirect">Allows redirection of requested source.</param>
        /// <param name="autounzip">Try to download a zipped version first.</param>
        /// <returns>A DOWNLOADFILESTATUS object containing status information.</returns>
        public DOWNLOADFILESTATUS DownloadFileIfNewer(string address, string filename, bool allowredirect, bool autounzip, string password = "")
        {
            string downloadaddress;
            string downloadfilename;
            DOWNLOADFILESTATUS ds = DOWNLOADFILESTATUS.UNKNOWN;
            try
            {
                // <filename.ext>
                downloadaddress = address;
                downloadfilename = filename;
                ds = DownloadFileFromWebIfNewer(downloadaddress, downloadfilename, allowredirect, autounzip, password);
                // return here if successful
                if ((ds == DOWNLOADFILESTATUS.NEWER) || (ds == DOWNLOADFILESTATUS.NOTNEWER))
                    return ds;
                // try to download other extensions 
                if (autounzip)
                {
                    // <filename.zip>
                    downloadaddress = address.Substring(0, address.LastIndexOf(".")) + ".zip";
                    downloadfilename = filename.Replace(Path.GetExtension(filename), ".zip");
                    ds = DownloadFileFromWebIfNewer(downloadaddress, downloadfilename, allowredirect, autounzip, password);
                    // return here if successful
                    if ((ds == DOWNLOADFILESTATUS.NEWER) || (ds == DOWNLOADFILESTATUS.NOTNEWER))
                        return ds;
                    // <filename.ext.zip>
                    downloadaddress = address + ".zip";
                    downloadfilename = filename + ".zip";
                    ds = DownloadFileFromWebIfNewer(downloadaddress, downloadfilename, allowredirect, autounzip, password);
                    // return here if successful
                    if ((ds == DOWNLOADFILESTATUS.NEWER) || (ds == DOWNLOADFILESTATUS.NOTNEWER))
                        return ds;
                }
                return DOWNLOADFILESTATUS.NOTFOUND;
            }
            catch (Exception ex)
            {
                if (ex is WebException)
                    Console.WriteLine("[DownloadFileIfNewer] WebException while reading address: " + address + "\n" + "URI of orginal request=" + address + "\n" + "URI of responding server=" + ((WebException)ex).Response.ResponseUri + "\n" + ex.ToString());
                else
                    Console.WriteLine("[DownloadFileIfNewer] Error while reading address: " + address + "\n" + ex.ToString());

            }
            return DOWNLOADFILESTATUS.ERROR;
        }
    }

    public class FTPDirectoryItem
    {
        public Uri BaseUri;

        public string AbsolutePath
        {
            get
            {
                return string.Format("{0}/{1}", BaseUri, Name);
            }
        }

        public DateTime DateCreated;
        public bool IsDirectory;
        public string Name;
        public List<FTPDirectoryItem> Items;
    }

    public class FTPDirectorySearcher
    {
        public  List<FTPDirectoryItem> GetDirectoryInformation(string address, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(address);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            List<FTPDirectoryItem> returnValue = new List<FTPDirectoryItem>();
            string[] list = null;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                list = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (string line in list)
            {
                // Windows FTP Server Response Format
                // DateCreated    IsDirectory    Name
                string data = line;

                // Parse date
                string date = data.Substring(0, 17);
                DateTime dateTime = DateTime.Parse(date);
                data = data.Remove(0, 24);

                // Parse <DIR>
                string dir = data.Substring(0, 5);
                bool isDirectory = dir.Equals("<dir>", StringComparison.InvariantCultureIgnoreCase);
                data = data.Remove(0, 5);
                data = data.Remove(0, 10);

                // Parse name
                string name = data;

                // Create directory info
                FTPDirectoryItem item = new FTPDirectoryItem();
                item.BaseUri = new Uri(address);
                item.DateCreated = dateTime;
                item.IsDirectory = isDirectory;
                item.Name = name;

                Debug.WriteLine(item.AbsolutePath);
                item.Items = item.IsDirectory ? GetDirectoryInformation(item.AbsolutePath, username, password) : null;

                returnValue.Add(item);
            }

            return returnValue;
        }
    }

    public class HTTPDirectoryItem
    {
        public Uri BaseUri;

        public string AbsolutePath
        {
            get
            {
                return string.Format("{0}/{1}", BaseUri, Name);
            }
        }

        public string Name;
    }

    public class HTTPDirectorySearcher
    {
        public List<HTTPDirectoryItem> GetDirectoryInformation(string address)
        {
            if (String.IsNullOrEmpty(address))
                return new List<HTTPDirectoryItem>();
            try
            {
                string content;
                /*
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                // request.KeepAlive = false;
                // request.ProtocolVersion = HttpVersion.Version10;
                // request.Timeout = 10000;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                 */
                WebClient client = new WebClient();
                // fake a up-to-date user agent
                client.Headers["User-Agent"] = "Mozilla / 5.0(Windows NT 10.0; WOW64; rv: 50.0) Gecko / 20100101 Firefox / 50.0";
                content = client.DownloadString(address);
                Regex regex = new Regex("<a href=\".*\">(?<name>.*)</a>");
                List<HTTPDirectoryItem> files = new List<HTTPDirectoryItem>();
                MatchCollection matches = regex.Matches(content);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            string[] matchData = match.Groups[0].ToString().Split('\"');
                            // sort out all weblinks and comments
                            HTTPDirectoryItem item = new HTTPDirectoryItem();
                            item.Name = matchData[1];
                            item.BaseUri = new Uri(address);
                            files.Add(item);
                        }
                    }
                }
                return files;
            }
            catch(Exception ex)
            {
            }
            return new List<HTTPDirectoryItem>();
        }


    }

    public enum DOWNLOADFILESTATUS
    {
        UNKNOWN = 0,
        NOTFOUND = 1,
        NEWER = 2,
        NOTNEWER = 4,
        ERROR = 8
    }

    public static class Password
    {
        public static string GetSFTPURL(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            if (!s.Contains('\t'))
                return "";
            string[] a = s.Split('\t');
            return a[0].Trim();
        }

        public static string GetSFTPUser(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            if (!s.Contains('\t'))
                return "";
            string[] a = s.Split('\t');
            string user = a[1].Trim();
            return Encryption.SimpleDecryptString(user);
        }

        public static string GetSFTPPassword(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            if (!s.Contains('\t'))
                return "";
            string[] a = s.Split('\t');
            string pwd = a[2].Trim();
            return Encryption.SimpleDecryptString(pwd);
        }
    }

}
