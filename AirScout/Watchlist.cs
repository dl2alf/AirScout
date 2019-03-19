using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScout
{
    /// <summary>
    /// Holds the watchlist item
    /// </summary>
    public class WatchlistItem
    {
        public string Call { get; set; }
        public string Loc { get; set; }
        public bool Checked { get; set; }
        public bool Selected { get; set; }
        public bool OutOfRange { get; set; }
        public bool Remove { get; set; }

        public WatchlistItem()
        {
            Call = "";
            Loc = "";
            Checked = false;
            Selected = false;
            OutOfRange = false;
            Remove = false;
        }

        public WatchlistItem(string call, string loc, bool oor) : this(call, loc, oor, false, false) { }
        public WatchlistItem(string call, string loc, bool check, bool oor, bool selected )
        {
            Call = call;
            Loc = loc;
            Checked = check;
            Selected = selected;
            OutOfRange = oor;
            Remove = false;
        }

    }

    /// <summary>
    /// Holds the watchlist, e.g. a list of watchlist items
    /// </summary>
    public class Watchlist : List<WatchlistItem>
    {
        public Watchlist()
        {

        }

        public int IndexOf(string call)
        {
            // TODO: optimize!!!
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Call == call)
                    return i;
            }
            return -1;
        }

        public int IndexOf(string call, string loc)
        {
            // TODO: optimize!!!
            for (int i = 0; i < this.Count; i++)
            {
                if ((this[i].Call == call) && (this[i].Loc == loc))
                    return i;
            }
            return -1;
        }

    }
}
