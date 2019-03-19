using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace ScoutBase.Core
{
	/// <summary>
	/// Converts and checks callsigns
	/// </summary>
	public static class Callsign
	{
        private static PrefixList Prefixes;

        static Callsign()
        {
            Prefixes = new PrefixList();
        }

        public class PrefixEntry : System.Object
        {
            public string Prefix = "";
            public string Name = "";
            public string Start = "";
            public string Stop = "";

            public PrefixEntry(string APrefix, string AName, string AStart, string AStop)
            {
                Prefix = APrefix;
                Name = AName;
                Start = AStart;
                Stop = AStop;
            }

            public string PREFIX
            {
                get
                {
                    return Prefix;
                }
            }

            public string STOP
            {
                get
                {
                    return Stop;
                }
            }
            public string START
            {
                get
                {
                    return Start;
                }
            }

        }

        public class PrefixList : System.Collections.ArrayList
        {
            public class LengthComparer : IComparer
            {
                int IComparer.Compare(Object x, Object y)
                {
                    if (((PrefixEntry)x).Start.Length < ((PrefixEntry)y).Start.Length)
                        return 1;
                    if (((PrefixEntry)x).Start.Length > ((PrefixEntry)y).Start.Length)
                        return -1;
                    return ((PrefixEntry)x).Start.CompareTo(((PrefixEntry)y).Start);
                }
            }

            public PrefixList()
            {
                this.Add(new PrefixEntry("C3", "Andorra", "C3", "C3"));
                this.Add(new PrefixEntry("OE", "Austria", "OE", "OE"));
                this.Add(new PrefixEntry("ON", "Belgium", "ON", "OT"));
                this.Add(new PrefixEntry("LZ", "Bulgaria", "LZ", "LZ"));
                this.Add(new PrefixEntry("EA8", "Canary Islands", "EA8", "EA8"));
                this.Add(new PrefixEntry("EA8", "Canary Islands", "EH8", "EH8"));
                this.Add(new PrefixEntry("SV9", "Crete", "SV9", "SV9"));
                this.Add(new PrefixEntry("5B", "Cyprus", "5B", "5B"));
                this.Add(new PrefixEntry("5B", "Cyprus", "C4", "C4"));
                this.Add(new PrefixEntry("5B", "Cyprus", "H2", "H2"));
                this.Add(new PrefixEntry("5B", "Cyprus", "P3", "P4"));
                this.Add(new PrefixEntry("OK", "Czechia", "OK", "OL"));
                this.Add(new PrefixEntry("OM", "Slovakia", "OM", "OM"));
                this.Add(new PrefixEntry("OZ", "Denmark", "OU", "OZ"));
                this.Add(new PrefixEntry("EA", "Spain", "AM", "AO"));
                this.Add(new PrefixEntry("EA", "Spain", "EA", "EH"));
                this.Add(new PrefixEntry("DL", "Germany", "DA", "DR"));
                this.Add(new PrefixEntry("EI", "Ireland", "EI", "EJ"));
                this.Add(new PrefixEntry("F", "France", "F", "F"));
                this.Add(new PrefixEntry("F", "France", "TP", "TP"));
                this.Add(new PrefixEntry("F", "France", "TM", "TM"));
                this.Add(new PrefixEntry("G", "England", "G", "G"));
                this.Add(new PrefixEntry("G", "England", "M", "M"));
                this.Add(new PrefixEntry("G", "England", "2E", "2E"));
                this.Add(new PrefixEntry("HA", "Hungaria", "HA", "HA"));
                this.Add(new PrefixEntry("HA", "Hungaria", "HG", "HG"));
                this.Add(new PrefixEntry("HB9", "Switzerland", "HB1", "HB9"));
                this.Add(new PrefixEntry("HB9", "Switzerland", "HE", "HE"));
                this.Add(new PrefixEntry("HB0", "Liechtenstein", "HB0", "HB0"));
                this.Add(new PrefixEntry("SP", "Poland", "HF", "HF"));
                this.Add(new PrefixEntry("SP", "Poland", "SP", "SP"));
                this.Add(new PrefixEntry("SP", "Poland", "SO", "SO"));
                this.Add(new PrefixEntry("SP", "Poland", "SQ", "SQ"));
                this.Add(new PrefixEntry("SP", "Poland", "SN", "SN"));
                this.Add(new PrefixEntry("SV", "Greece", "J4", "J4"));
                this.Add(new PrefixEntry("SV", "Greece", "SV", "SV"));
                this.Add(new PrefixEntry("LA", "Norway", "LA", "LN"));
                this.Add(new PrefixEntry("LX", "Luxembuorg", "LX", "LX"));
                this.Add(new PrefixEntry("PA", "Netherlands", "PA", "PI"));
                this.Add(new PrefixEntry("I", "Italy", "I0", "I9"));
                this.Add(new PrefixEntry("I", "Italy", "IA", "IZ"));
                this.Add(new PrefixEntry("YU", "Yugoslavia", "YU1", "YU1"));
                this.Add(new PrefixEntry("YU", "Yugoslavia", "YU6", "YU9"));
                this.Add(new PrefixEntry("YU", "Yugoslavia", "YT", "YT"));
                this.Add(new PrefixEntry("YU", "Yugoslavia", "YZ", "YZ"));
                this.Add(new PrefixEntry("YU", "Yugoslavia", "4N", "4N"));
                this.Add(new PrefixEntry("9A", "Croatia", "9A", "9A"));
                this.Add(new PrefixEntry("9A", "Croatia", "YU2", "YU2"));
                this.Add(new PrefixEntry("S5", "Slovenia", "S5", "S5"));
                this.Add(new PrefixEntry("S5", "Slovenia", "YU3", "YU3"));
                this.Add(new PrefixEntry("T9", "Bosnia-Herzegovina", "T9", "T9"));
                this.Add(new PrefixEntry("T9", "Bosnia-Herzegovina", "YU4", "YU4"));
                this.Add(new PrefixEntry("Z3", "Mazedonia", "Z3", "Z3"));
                this.Add(new PrefixEntry("Z3", "Mazedonia", "YU5", "YU5"));
                this.Add(new PrefixEntry("SM", "Sweden", "SA", "SM"));
                this.Add(new PrefixEntry("HV", "Vatikan", "HV", "HV"));
                this.Add(new PrefixEntry("EA6", "Balearic Islands", "EA6", "EA6"));
                this.Add(new PrefixEntry("EA6", "Balearic Islands", "EH6", "EH6"));
                this.Add(new PrefixEntry("GD", "Isle Of Man", "GD", "GD"));
                this.Add(new PrefixEntry("GI", "Northern Ireland", "GI", "GI"));
                this.Add(new PrefixEntry("GJ", "Jersey", "GJ", "GJ"));
                this.Add(new PrefixEntry("GM", "Scotland", "GM", "GM"));
                this.Add(new PrefixEntry("GW", "Wales", "GW", "GW"));
                this.Add(new PrefixEntry("IS", "Sardinia", "IS", "IS"));
                this.Add(new PrefixEntry("TK", "Corsica", "TK", "TK"));
                this.Add(new PrefixEntry("LY", "Lithuania", "LY", "LY"));
                this.Add(new PrefixEntry("ER", "Moldavia", "ER", "ER"));
                this.Add(new PrefixEntry("ES", "Estonia", "ES", "ES"));
                this.Add(new PrefixEntry("EV", "Byelorussia", "EV", "EV"));
                this.Add(new PrefixEntry("EV", "Byelorussia", "EW", "EW"));
                this.Add(new PrefixEntry("UA", "European Russia", "UA", "UA"));
                this.Add(new PrefixEntry("UA", "European Russia", "UA", "RA"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "UA2", "UA2"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "RA2", "RA2"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "RN2", "RN2"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "RK2", "RK2"));
                this.Add(new PrefixEntry("YO", "Romania", "YO", "YR"));
                this.Add(new PrefixEntry("ZA", "Albania", "ZA", "ZA"));
                this.Add(new PrefixEntry("ZB4", "Gibraltar", "ZB4", "ZB4"));
                this.Add(new PrefixEntry("9H", "Malta", "9H", "9H"));
                this.Add(new PrefixEntry("TA", "Turkey", "TA", "TA"));
                this.Add(new PrefixEntry("T7", "San Marino", "T7", "T7"));
                // nach Länge sortieren, längste Einträge zuerst
                this.Sort(new LengthComparer());
            }
        }


        public static string Cut(string call)
        {
            try
            {
                call.Trim().ToUpper();
                if (call.IndexOf('/') >= 0)
                {
                    // hinteren Teil abschneiden
                    if (call.IndexOf('/') >= call.Length - 4)
                        call = call.Remove(call.IndexOf('/'), call.Length - call.IndexOf('/'));
                    // evtl noch vorderen Teil abschneiden
                    if (call.IndexOf('/') >= 0)
                        call = call.Remove(0, call.IndexOf('/') + 1);
                    // nochmals hinteren Teil abschneiden
                    if (call.IndexOf('/') >= call.Length - 4)
                        call = call.Remove(call.IndexOf('/'), call.Length - call.IndexOf('/'));
                }
                return call;
            }
            catch
            {
                // Fehler beim Abschneiden des Rufzeichens
                return "";
            }
        }

        public static bool Check(string call)
        {
            // bewertet Übergabestring als Call 
            try
            {
                // empty string
                if (String.IsNullOrEmpty(call))
                    return false;
                call = call.Trim();
                call = call.ToUpper();
                // auf unerlaubte Zeichen testen 
                for (int j = 0; j < call.Length; j++)
                {
                    if (((call[j] < 'A') || (call[j] > 'Z')) &&
                        ((call[j] < '0') || (call[j] > '9')) &&
                        ((call[j] != '/')))
                    {
                        return false;
                    }
                }
                // auf Position und Anzahl Schrägstriche testen
                if (call.StartsWith("/"))
                    return false;
                if (call.EndsWith("/"))
                    return false;
                if (call.Split('/').Length - 1 > 2)
                    return false;
                // Rufzeichen von Zusätzen befreien
                call = Cut(call);
                // Rufzeichen zu kurz
                if (call.Length < 3)
                    return false;
                // eigentlicher Test
                // erstes Zeichen Zahl
                if (Char.IsNumber(call, 0))
                {
                    // zweites Zeichen muss Buchstabe sein
                    // drittes Zeichen muss Zahl sein
                    // Beispiel : 9A5O
                    if (Char.IsLetter(call, 1) && Char.IsNumber(call, 2))
                        return true;
                    return false;
                }
                else
                {
                    // erstes Zeichen Buchstabe
                    // zweites Zeichen Buchstabe
                    if (Char.IsLetter(call, 1))
                    {
                        // drittes Zeichen muss Zahl sein
                        // Beispiel DL0GTH
                        if (Char.IsNumber(call, 2))
                            return true;
                        return false;
                    }
                    else
                    {
                        // zweites Zeichen Zahl
                        // drittes Zeichen Buchstabe
                        // Beispiel G7RAU
                        if (Char.IsLetter(call, 2))
                            return true;
                        // drittes Zeichen Zahl
                        // Beispiel T91CO
                        if (Char.IsLetter(call, 3))
                            return true;
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static string Prefix(string S)
        {
            S = Cut(S).ToUpper();
            string Start = "";
            string Stop = "";
            string Prefix = "???";
            for (int i = 0; i < Prefixes.Count; i++)
            {
                try
                {
                    Start = ((PrefixEntry)Prefixes[i]).Start;
                    Stop = ((PrefixEntry)Prefixes[i]).Stop;
                    Prefix = ((PrefixEntry)Prefixes[i]).Prefix;
                    // Call zurechtstutzen auf Länge des Prefixes
                    S = S.Substring(0, Start.Length);
                    if ((S.CompareTo(Start) >= 0) && (S.CompareTo(Stop) <= 0))
                        return Prefix;
                }
                catch
                {
                    // Fehler beim Suchen des Prefixes, i.allg. ist das Call zu kurz
                    // --> nichts weiter unternehmen
                }

            }
            return "???";
        }

    }
}