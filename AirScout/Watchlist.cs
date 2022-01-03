using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScout
{
    /// <summary>
    /// Holds the watchlist item
    /// </summary>
    public class WatchlistItem : IComparable<WatchlistItem>
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

        public WatchlistItem(string call, string loc, bool oor, bool check = false, bool selected = false)
        {
            Call = call;
            Loc = loc;
            Checked = check;
            Selected = selected;
            OutOfRange = oor;
            Remove = false;
        }

        public int CompareTo(WatchlistItem other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;

            // compare call signs first
            int i = String.Compare(this.Call, other.Call);
            if (i != 0)
                return i;
            // then compare locs when calls are equal
            return String.Compare(this.Loc, other.Loc);
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
