using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace ScoutBase.CAT
{
    public class CommandQueue : Collection<QueueItem>
    {
        public ExchangePhase Phase = ExchangePhase.phIdle;

        public CommandQueue()
        {

        }

        public void Delete(int index)
        {
            this.RemoveAt(index);
        }

        public QueueItem AddBeforeStatusCommands(QueueItem item)
        {
            int idx = this.Count;
            // decrease insert index in case of status commands pending
            // item[0] is the command currently worked on, so start with 1
            for (int i = 1; i < this.Count; i++)
            {
                if (this[i].Kind == CommandKind.ckStatus)
                {
                    idx = i;
                    break;
                }
            }

            this.Insert(idx, item);
            return item;
        }

        public bool HasStatusCommands()
        {
            foreach (QueueItem item in this)
            {
                if (item.Kind == CommandKind.ckStatus)
                    return true;
            }

            return false;
        }

        public QueueItem CurrentCmd()
        {
            return this[0];
        }

    }
}
