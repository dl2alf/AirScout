using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScoutBase.CAT
{
    /*
    public enum CommWorkerEvent
    {
        evErr,
        evTxEmpty,
        exRxChar,
        evCts,
        evDsr,
        evRlsd
    }

    public class CommWorker : BackgroundWorker
    {

        private int OldRxChars = 0;
        private int OldTxChars = 0;
        private bool OldCts = false;
        private bool OldDsr = false;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                Thread.CurrentThread.Name = "CommPort Worker Thread";
            }

            SerialPort COM = (SerialPort)e.Argument;

            // .NET SerialPort implementation does not have full functionality 
            // so we try to poll the interface for all information needed and fire according events

            while (!this.CancellationPending)
            {
                // hopefully we got a valid SerialPort object
                if (COM != null)
                {
                    // try to open port, if not open
                    if (!COM.IsOpen)
                    {
                        try
                        {
                            COM.Open();
                            OldRxChars = COM.BytesToRead;
                            OldTxChars = COM.BytesToWrite;
                            OldCts = COM.CtsHolding;
                            OldDsr = COM.DsrHolding;
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress((int)CommWorkerEvent.evErr, ex.ToString());
                            Thread.Sleep(1000);
                        }
                    }

                    // refresh status
                    if (DateTime.Now > FNextStatusTime)
                    {
                        if (FQueue.HasStatusCommands())
                        {
                            OmniRig.Log("RIG" + RigNumber.ToString() + " Status commands already in queue");
                        }
                        else
                        {
                            OmniRig.Log("RIG" + RigNumber.ToString() + " Adding status commands to queue");
                            AddCommands(RigCommands.StatusCmd, CommandKind.ckStatus);
                        }

                        FNextStatusTime = DateTime.Now.AddMilliseconds(PollMs);
                    }

                    // port is open --> get status info
                    if (COM.IsOpen)
                    {
                        try
                        {
                            // rx chars available?
                            int rxchars = COM.BytesToRead;
                            if (OldRxChars != rxchars)
                            {
                                OldRxChars = rxchars;
                                this.ReportProgress((int)CommWorkerEvent.exRxChar, rxchars);
                            }

                            // tx empty?
                            int txchars = COM.BytesToWrite;
                            if (OldTxChars != txchars)
                            {
                                OldTxChars = txchars;
                                if (txchars == 0)
                                {
                                    this.ReportProgress((int)CommWorkerEvent.evTxEmpty, txchars);
                                }
                            }

                            // CTS changed?
                            bool cts = COM.CtsHolding;
                            if (OldCts != cts)
                            {
                                OldCts = cts;
                                this.ReportProgress((int)CommWorkerEvent.evCts, cts);
                            }

                            // DSR changed?
                            bool dsr = COM.DsrHolding;
                            if (OldDsr != dsr)
                            {
                                OldDsr = dsr;
                                this.ReportProgress((int)CommWorkerEvent.evDsr, dsr);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress((int)CommWorkerEvent.evErr, ex.ToString());
                            Thread.Sleep(1000);
                        }
                    }

                }

                Thread.Sleep(10);
            }

            // try to close COM
            if ((COM != null) && (COM.IsOpen))
            {
                COM.Close();
            }
        }

        public CommWorker()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
        }
    }
    */
}
