using System;
using System.Data.SQLite;
using System.Drawing;

namespace AirScout
{

    public class DatabaseStatus
    {
        public static Color GetDatabaseStatusColor(DATABASESTATUS status)
        {
            Color color = Color.Plum;
            if ((status & DATABASESTATUS.ERROR) > 0)
                color = Color.Red;
            else if ((status & DATABASESTATUS.UPTODATE) > 0)
                color = Color.Green;
            else if ((status & DATABASESTATUS.COMPLETE) > 0)
                color = Color.Blue;
            else if ((status & DATABASESTATUS.UPDATING) > 0)
                color = Color.Gold;
            else if ((status & DATABASESTATUS.EMPTY) > 0)
                color = Color.Black;
            return color;
        }

        public static string GetDatabaseStatusText(DATABASESTATUS status)
        {
            string s = "";
            if ((status & DATABASESTATUS.UNDEFINED) > 0)
                s = "Database status is unknown.";
            if ((status & DATABASESTATUS.ERROR) > 0)
            {
                if (s.Length > 0)
                    s = s + "\n";
               s = s + "Database has errors.";
            }
            if ((status & DATABASESTATUS.UPTODATE) > 0)
            {
                if (s.Length > 0)
                    s = s + "\n";
                s = s + "Database is up to date.";
            }
            if ((status & DATABASESTATUS.COMPLETE) > 0)
            {
                if (s.Length > 0)
                    s = s + "\n";
                s = s + "Database is complete.";
            }
            if ((status & DATABASESTATUS.UPDATING) > 0)
            {
                if (s.Length > 0)
                    s = s + "\n";
                s = s + "Database is updating.";
            }
            if ((status & DATABASESTATUS.EMPTY) > 0)
            {
                if (s.Length > 0)
                    s = s + "\n";
                s = s + "Database is empty.";
            }
            return s;
        }

    }

}