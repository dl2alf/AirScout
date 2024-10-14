using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScoutBase.CAT
{
    public static class OmniRig
    {

        // Rig settings directory
        public static string RigDefinitionsDirectory = RigData.Database.DefaultDatabaseDirectory();
        public static string RigDefinitionsFileExtension = ".ini";

        // Log
        public static string LogFileName { get; set; } = "CAT.log";
        public static void Log(LogNotifyEventArgs args)
        {
            try
            {
                Console.WriteLine(args.Message);
//                File.AppendAllText(LogFileName, "[" + args.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff") + "][" + args.LogLevel.ToString() + "]: " + args.Message + Environment.NewLine) ;
                if (LogNotify != null)
                {
                    LogNotify.Invoke(args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to write to logfile [" + LogFileName + "]:" + ex.ToString());
            }
        }

        // Rigs
        public static Rig Rig1 = new Rig(1);
        public static Rig Rig2 = new Rig(2);
        public static Rig Rig3 = new Rig(3);
        public static Rig Rig4 = new Rig(4);

        // public properties
        public static bool DialogVisible { get; } = false;   // not supported so far
        public static int SoftwareVersion 
        { 
            get
            {
                int version = 0;
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                version = ((info.FileMajorPart & 0x0000FFFF) << 16) + (info.FileMinorPart & 0x0000FFFF);
                return version;
            }
        }
        public static int InterfaceVersion
        {
            get
            {
                int version = 0;
                Version ver = Assembly.GetExecutingAssembly().GetName().Version;
                version = ((ver.Major & 0x0000FFFF) << 16) + (ver.Minor & 0x0000FFFF);
                return version;
            }
        }

        // public events
        public delegate void RigTypeChangeEventHandler(int rignumber);
        public static event RigTypeChangeEventHandler RigTypeChange;

        public delegate void StatusChangeEventHandler(int rignumber);
        public static event StatusChangeEventHandler StatusChange;

        public delegate void ParamsChangeEventHandler(int rignumber, int Params);
        public static event ParamsChangeEventHandler ParamsChange;

        public delegate void CustomReplyEventHandler(int rignumber, object sender, byte[] code, byte[] data);
        public static event CustomReplyEventHandler CustomReply;

        // never fired, for compatibility only
        public delegate void VisibleChangeEventHandler();
        public static event VisibleChangeEventHandler VisibleChange;

        public delegate void LogNotifyEventHandler(LogNotifyEventArgs args);
        public static event LogNotifyEventHandler LogNotify;

        public static void FireComNotifyRigType(int rignumber)
        {
            if (OmniRig.RigTypeChange != null)
            {
                OmniRig.RigTypeChange.Invoke(rignumber);
            }
        }

        public static void FireComNotifyStatus(int rignumber)
        {
            if (OmniRig.StatusChange != null)
            {
                OmniRig.StatusChange.Invoke(rignumber);
            }
        }

        public static void FireComNotifyParams(int rignumber, int Params)
        {
            if (OmniRig.ParamsChange != null)
            {
                OmniRig.ParamsChange.Invoke(rignumber, Params);
            }
        }

        // different implementation as there is no ActiveX object
        public static void FireComNotifyCustom(int rignumber, object sender, byte[] code, byte[] data)
        {
            if (OmniRig.CustomReply != null)
            {
                OmniRig.CustomReply.Invoke(rignumber, sender, code, data);
            }
        }

        // gets a rig command set from settings file using rig type name
        public static RigCommands CommandsFromRigType (string rigtype)
        {
            // return empty command set on empty rig type name
            if (String.IsNullOrEmpty(rigtype))
                return new RigCommands();

            // check for invalid directory name --> set application executing directory instead
            string settingsdir = OmniRig.RigDefinitionsDirectory;
            if (!Directory.Exists(settingsdir))
            {
                settingsdir = Application.StartupPath;
            }

            // check for invalid settings extension --> change to standard extension
            string settingsextension = OmniRig.RigDefinitionsFileExtension;
            if (String.IsNullOrEmpty(settingsextension) || !settingsextension.StartsWith("."))
            {
                settingsextension = ".ini";
            }
            return RigCommands.FromIni(Path.Combine(settingsdir, rigtype + settingsextension));
        }

        // list all supported rigs from rig settings files found in RigDefinitionsDirectory
        public static List<string> SupportedRigs()
        {
            List<string> rigs = new List<string>();

            // return empty list, if directory does not extist
            if (!Directory.Exists(RigDefinitionsDirectory))
                return rigs;
            try
            {
                Console.WriteLine("Getting rig definition files (*" + RigDefinitionsFileExtension + ") from " + RigDefinitionsDirectory);
                // get all configuration files from directory
                string[] configs = Directory.GetFiles(RigDefinitionsDirectory, "*" + RigDefinitionsFileExtension);
                Console.WriteLine("Found " + configs.Length + " file(s).");
                // get straight filename without extension
                for (int i = 0; i < configs.Length; i++)
                {
                    string rig = Path.GetFileNameWithoutExtension(configs[i]);
                    rigs.Add(rig);
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }

            // return a list of rigs
            return rigs;
        }
    }
}
