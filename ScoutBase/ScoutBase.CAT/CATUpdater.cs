using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ScoutBase.Core;

namespace ScoutBase.CAT
{

    public class CATUpdaterStartOptions
    {
        public string Name;
        public BACKGROUNDUPDATERSTARTOPTIONS Options;
    }

    // Background worker for rig definitions update
    [DefaultPropertyAttribute("Name")]
    public class CATUpdater : BackgroundWorker
    {
        CATUpdaterStartOptions StartOptions;

        // Temp directory to save downloaded files
        string TmpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName, "Tmp", "RigData").TrimEnd(Path.DirectorySeparatorChar);

        // File extension
        string FileExtension = ".ini";

        public CATUpdater() : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            // create temp directory if not exists
            if (!Directory.Exists(TmpDirectory))
                Directory.CreateDirectory(TmpDirectory);
        }

        private DOWNLOADFILESTATUS GetUpdateFromURL(string url, string zipfilename)
        {
            AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
            DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, zipfilename, true, true);
            return status;
        }

        private bool ReadRigsFromURL(string url, string zipfilename)
        {
            try
            {
                DOWNLOADFILESTATUS status = GetUpdateFromURL(url, zipfilename);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.NOTFOUND) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + zipfilename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    var dir = new DirectoryInfo(TmpDirectory);
                    FileInfo[] files = dir.GetFiles("*" + FileExtension);
                    foreach (FileInfo file in files)
                    {
                        string oldfilename = Path.Combine(RigData.Database.DefaultDatabaseDirectory(), file.Name);
                        try
                        {
                            if (!File.Exists(oldfilename))
                            {
                                // file does not exist --> simply copy over
                                File.Copy(file.FullName, oldfilename);
                            }
                            else  if (new FileInfo(oldfilename).LastWriteTimeUtc < file.LastWriteTimeUtc)
                            {
                                // file exists but is older --> delete old file and copy over
                                File.Delete(oldfilename);
                                File.Copy(file.FullName, oldfilename);

                            }

                            if (this.CancellationPending)
                                return false;

                            // reduce CPU load
                            Thread.Sleep(1);
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress(-1, "Error while checking " + oldfilename);
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Error loading database
                this.ReportProgress(-1, "[" + url + "]: " + ex.ToString());
            }

            return false;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            StartOptions = (CATUpdaterStartOptions)e.Argument;
            this.ReportProgress(0, StartOptions.Name + " started.");

            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = nameof(CATUpdater);

            // create directories if not exist
            try
            {
                if (!Directory.Exists(TmpDirectory))
                    Directory.CreateDirectory(TmpDirectory);
                if (!Directory.Exists(RigData.Database.DefaultDatabaseDirectory()))
                    Directory.CreateDirectory(RigData.Database.DefaultDatabaseDirectory());
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, "Error while creating directories: " + ex.ToString());
            }

            this.ReportProgress(0, "Updating rig definitions...");

            // get update interval
            int interval = (int)Properties.Settings.Default.Database_BackgroundUpdate_Period * 60;
            do
            {
                try
                {
                    // check if any kind of update is enabled
                    if ((StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE) || (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY))
                    {
                        // reset database status
                        Stopwatch st = new Stopwatch();
                        st.Start();
                        // clear temporary files
                        try
                        {
                            SupportFunctions.DeleteFilesFromDirectory(TmpDirectory, new string[] { "*" + FileExtension, "*.PendingOverwrite" });
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress(-1, ex.ToString());
                        }
                        // update rig database
                        this.ReportProgress(0, "Updating rig definitions from web database...");
                        // get update from url
                        if (ReadRigsFromURL(Properties.Settings.Default.Rigs_UpdateURL + "rigs.zip", Path.Combine(TmpDirectory, "rigs.zip")))
                        {
                            st.Stop();
                            this.ReportProgress(0, " Rig database update completed: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                        }
                        else
                        {
                            st.Stop();
                            this.ReportProgress(0, " Rig database update completed with errors: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                        }

                        if (this.CancellationPending)
                            break;

                        // sleep once to get all messages to main thread
                        Thread.Sleep(1000);
                        
                        // sleep when running periodically
                        if (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY)
                        {
                            int i = 0;
                            while (!this.CancellationPending && (i < interval))
                            {
                                Thread.Sleep(1000);
                                i++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ReportProgress(-1, ex.ToString());
                }
            }
            while (!this.CancellationPending && (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY));
            if (this.CancellationPending)
                this.ReportProgress(0, "Cancelled.");
            else
                this.ReportProgress(0, "Finished.");
        }

    }
}
