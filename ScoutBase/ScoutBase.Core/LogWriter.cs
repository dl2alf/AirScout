using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;


namespace ScoutBase.Core
{

    public static class LogLevel
    {
        public const int Nothing = 0;
        public const int Info = 10;
        public const int Warning = 20;
        public const int Error = 30;
        public const int Panic = 255;
    }

    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically.
    /// It is designed to work in a multithreading environment
    /// To use it simpy add the following to lines to your code
    /// LogWriter writer = LogWriter.Instance;
    /// writer.WriteToLog(message);
    /// </summary>
    public class LogWriter
    {
        private static LogWriter instance;
        private static Queue<Log> logQueue;

        private static DateTime LastFlushed = DateTime.MinValue;    // be sure that the first message is written directly into the log

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter()
        {
        }

        ~LogWriter()
        {
            // flush the log regardless of queue size and age
            FlushLog();
        }

        #region Properties

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter Instance
        {
            get
            {
                // If the instance is null then create one and init the Queue
                if (instance == null)
                {
                    instance = new LogWriter();
                    logQueue = new Queue<Log>();
                }
                return instance;
            }
        }

        /// <summary>
        /// A string that exposes the log file directory
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                // get string from settings
                string logDirectory = Properties.Settings.Default.LogWriter_Directory;
                // validate logfile directory --> use Windows Tmp directory if not
                if (!SupportFunctions.ValidateDirectoryPath(logDirectory))
                {
                    logDirectory = Path.GetTempPath();
                }
                // be sure that the directory exists --> create it if not
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);
                return logDirectory;
            }
        }

        /// <summary>
        /// A string that exposes the log filename format
        /// Default: "log_{0:d}.log" representing a daily changing log file
        /// </summary>
        public static string LogFileFormat
        {
            get
            {
                string logFileFormat = Properties.Settings.Default.LogWriter_FileFormat;
                if (String.IsNullOrEmpty(logFileFormat))
                {
                    logFileFormat = "log_{0:d}.log";
                }
                // be sure that the resulting filename is valid
                string filename = Path.Combine(LogDirectory, String.Format(logFileFormat, DateTime.UtcNow));
                if (!SupportFunctions.ValidateFilePath(filename))
                    logFileFormat = "log_{0:d}.log";
                return logFileFormat;
            }
        }

        /// <summary>
        /// A string that exposes the log message format
        /// Default: ""{0:t}: {1}" representing a "UTC timestamp: Messagetext" format
        /// </summary>
        public static string LogMessageFormat
        {
            get
            {
                string logMessageFormat = Properties.Settings.Default.LogWriter_MessageFormat;
                if (String.IsNullOrEmpty(logMessageFormat))
                {
                    logMessageFormat = "{0:t}: {1}";
                }
                // be sure that the resulting message format is valid
                // creates a test message line
                try
                {
                    string line = String.Format(logMessageFormat, new object[] { DateTime.UtcNow, "Test" });
                }
                catch
                {
                    logMessageFormat = "{0:t}: {1}";
                }
                return logMessageFormat;
            }
        }

        /// <summary>
        /// An integer that exposes the maximum age [seconds] of the que before writing into the logfile.
        /// Minimum: 1s
        /// Maxixum: 3600s
        /// Default 10s
        /// </summary>
        public static int MaxLogAge
        {
            get
            {
                int maxLogAge = Properties.Settings.Default.LogWriter_MaxAge;
                if (maxLogAge < 1)
                    maxLogAge = 1;
                if (maxLogAge > 3600)
                    maxLogAge = 3600;
                return maxLogAge;
            }
        }

        /// <summary>
        /// An integer that exposes the maximum number of queued entries before writing them into the logfile
        /// Minimum: 1
        /// Maxixum: 100
        /// Default 10
        /// </summary>
        public static int QueueSize
        {
            get
            {
                int queueSize = Properties.Settings.Default.LogWriter_QueueSize;
                if (queueSize < 1)
                    queueSize = 1;
                if (queueSize > 100)
                    queueSize = 100;
                return queueSize;
            }
        }

        /// <summary>
        /// An integer that exposes the log verbosity
        /// Messages with error lever lower than verbosity are not written to the log
        /// Minimum: 0
        /// Maxixum: 255
        /// Default 0
        /// </summary>
        public static int Verbosity
        {
            get
            {
                int verbosity = Properties.Settings.Default.LogWriter_Verbosity;
                if (verbosity < 0)
                    verbosity = 0;
                if (verbosity > 255)
                    verbosity = 255;
                return verbosity;
            }
        }

        #endregion

        /// <summary>
        /// The single instance method that writes a message to the log file queue
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteMessage(string message, int errorlevel = LogLevel.Info, bool debugdetails = true)
        {
            // return on errorlevel below verbosity threshold
            if (errorlevel < Verbosity)
                return;
            // build the message
            StackFrame stackFrame = new StackFrame(1, true);
            string fileName = stackFrame.GetFileName();
            string methodName = stackFrame.GetMethod().ToString();
            int lineNumber = stackFrame.GetFileLineNumber();

            string msg = "[" + errorlevel.ToString("000") + "]";
            if (debugdetails)
            {
                try
                {
                    msg = msg + "[" + methodName + " at " + fileName + ":" + lineNumber.ToString() + "]";
                }
                catch
                {
                    msg = msg + "[unknown]";
                }
            }
            msg = msg + ": " + message;
            // force a one liner
            msg = msg.Replace("\n", " ").Replace("\r", " ");
            // Lock the queue while writing to prevent contention for the log file
            lock (logQueue)
            {
                // Create the entry and push to the Queue
                Log logEntry = new Log(msg);
                logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (logQueue.Count >= QueueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }
        }

        private bool DoPeriodicFlush()
        {
            // returns true if max age of the queue is reached
            TimeSpan logAge = DateTime.Now - LastFlushed;
            if (logAge.TotalSeconds >= MaxLogAge)
            {
                LastFlushed = DateTime.Now;
                return true;
            }   
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        public void FlushLog()
        {
            // not very efficient as opens and closes the stream on each entry
            // but can handle queue at date change to ensure that each message goes to the right file
            while (logQueue.Count > 0)
            {
                try
                {
                    Log entry = logQueue.Dequeue();
                    string logfilename = Path.Combine(LogDirectory, String.Format(LogFileFormat, entry.Timestamp));
                    using (FileStream fs = File.Open(logfilename, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter log = new StreamWriter(fs))
                        {
                            // create the line
                            string line = String.Format(LogMessageFormat, new object[] { entry.Timestamp, entry.Message });

                            log.WriteLine(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // PANIC: writes a message to the console if something goes wrong with writing the log
                    Console.WriteLine("Error writing logfile: " + ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    public class Log
    {

        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public Log(string message)
        {
            Timestamp = DateTime.UtcNow;
            Message = message;
        }
    }
}

