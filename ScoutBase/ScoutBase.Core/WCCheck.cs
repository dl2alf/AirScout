using System;
using System.Collections;
using ScoutBase.Core;

namespace WCCheck
{
	/// <summary>
	/// 
	/// </summary>
    /// 
    /*
	public class WCCheck : Object
	{
		public static double Radius = 6378.2;
		
		private static PrefixList Prefixes;

		static WCCheck()
		{
			Prefixes = new PrefixList();
		}

		public class PrefixEntry : System.Object
		{
			public string Prefix = "";
			public string Name = "";
			public string Start = "";
			public string Stop = "";

			public PrefixEntry (string APrefix, string AName, string AStart, string AStop)
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
				int IComparer.Compare( Object x, Object y )  
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
				this.Add(new PrefixEntry("C3","Andorra","C3","C3"));				
				this.Add(new PrefixEntry("OE","Austria","OE","OE"));				
				this.Add(new PrefixEntry("ON","Belgium","ON","OT"));				
				this.Add(new PrefixEntry("LZ","Bulgaria","LZ","LZ"));				
				this.Add(new PrefixEntry("EA8","Canary Islands","EA8","EA8"));				
				this.Add(new PrefixEntry("EA8","Canary Islands","EH8","EH8"));				
				this.Add(new PrefixEntry("SV9","Crete","SV9","SV9"));				
				this.Add(new PrefixEntry("5B","Cyprus","5B","5B"));				
				this.Add(new PrefixEntry("5B","Cyprus","C4","C4"));				
				this.Add(new PrefixEntry("5B","Cyprus","H2","H2"));				
				this.Add(new PrefixEntry("5B","Cyprus","P3","P4"));				
				this.Add(new PrefixEntry("OK","Czechia","OK","OL"));				
				this.Add(new PrefixEntry("OM","Slovakia","OM","OM"));				
				this.Add(new PrefixEntry("OZ","Denmark","OU","OZ"));				
				this.Add(new PrefixEntry("EA","Spain","AM","AO"));				
				this.Add(new PrefixEntry("EA","Spain","EA","EH"));				
				this.Add(new PrefixEntry("DL","Germany","DA","DR"));				
				this.Add(new PrefixEntry("EI","Ireland","EI","EJ"));				
				this.Add(new PrefixEntry("F","France","F","F"));				
				this.Add(new PrefixEntry("F","France","TP","TP"));				
				this.Add(new PrefixEntry("F","France","TM","TM"));				
				this.Add(new PrefixEntry("G","England","G","G"));				
				this.Add(new PrefixEntry("G","England","M","M"));				
				this.Add(new PrefixEntry("G","England","2E","2E"));				
				this.Add(new PrefixEntry("HA","Hungaria","HA","HA"));				
				this.Add(new PrefixEntry("HA","Hungaria","HG","HG"));				
				this.Add(new PrefixEntry("HB9","Switzerland","HB1","HB9"));				
				this.Add(new PrefixEntry("HB9","Switzerland","HE","HE"));				
				this.Add(new PrefixEntry("HB0","Liechtenstein","HB0","HB0"));				
				this.Add(new PrefixEntry("SP","Poland","HF","HF"));				
				this.Add(new PrefixEntry("SP","Poland","SP","SP"));				
				this.Add(new PrefixEntry("SP","Poland","SO","SO"));				
				this.Add(new PrefixEntry("SP","Poland","SQ","SQ"));				
				this.Add(new PrefixEntry("SP","Poland","SN","SN"));				
				this.Add(new PrefixEntry("SV","Greece","J4","J4"));				
				this.Add(new PrefixEntry("SV","Greece","SV","SV"));				
				this.Add(new PrefixEntry("LA","Norway","LA","LN"));				
				this.Add(new PrefixEntry("LX","Luxembuorg","LX","LX"));				
				this.Add(new PrefixEntry("PA","Netherlands","PA","PI"));				
				this.Add(new PrefixEntry("I","Italy","I0","I9"));				
				this.Add(new PrefixEntry("I","Italy","IA","IZ"));				
				this.Add(new PrefixEntry("YU","Yugoslavia","YU1","YU1"));				
				this.Add(new PrefixEntry("YU","Yugoslavia","YU6","YU9"));				
				this.Add(new PrefixEntry("YU","Yugoslavia","YT","YT"));				
				this.Add(new PrefixEntry("YU","Yugoslavia","YZ","YZ"));				
				this.Add(new PrefixEntry("YU","Yugoslavia","4N","4N"));				
				this.Add(new PrefixEntry("9A","Croatia","9A","9A"));				
				this.Add(new PrefixEntry("9A","Croatia","YU2","YU2"));				
				this.Add(new PrefixEntry("S5","Slovenia","S5","S5"));				
				this.Add(new PrefixEntry("S5","Slovenia","YU3","YU3"));				
				this.Add(new PrefixEntry("T9","Bosnia-Herzegovina","T9","T9"));				
				this.Add(new PrefixEntry("T9","Bosnia-Herzegovina","YU4","YU4"));				
				this.Add(new PrefixEntry("Z3","Mazedonia","Z3","Z3"));				
				this.Add(new PrefixEntry("Z3","Mazedonia","YU5","YU5"));				
				this.Add(new PrefixEntry("SM","Sweden","SA","SM"));				
				this.Add(new PrefixEntry("HV","Vatikan","HV","HV"));				
				this.Add(new PrefixEntry("EA6","Balearic Islands","EA6","EA6"));				
				this.Add(new PrefixEntry("EA6","Balearic Islands","EH6","EH6"));				
				this.Add(new PrefixEntry("GD","Isle Of Man","GD","GD"));				
				this.Add(new PrefixEntry("GI","Northern Ireland","GI","GI"));				
				this.Add(new PrefixEntry("GJ","Jersey","GJ","GJ"));				
				this.Add(new PrefixEntry("GM","Scotland","GM","GM"));				
				this.Add(new PrefixEntry("GW","Wales","GW","GW"));				
				this.Add(new PrefixEntry("IS","Sardinia","IS","IS"));				
				this.Add(new PrefixEntry("TK","Corsica","TK","TK"));				
				this.Add(new PrefixEntry("LY","Lithuania","LY","LY"));				
				this.Add(new PrefixEntry("ER","Moldavia","ER","ER"));				
				this.Add(new PrefixEntry("ES","Estonia","ES","ES"));				
				this.Add(new PrefixEntry("EV","Byelorussia","EV","EV"));				
				this.Add(new PrefixEntry("EV","Byelorussia","EW","EW"));
                this.Add(new PrefixEntry("UA", "European Russia", "UA", "UA"));
                this.Add(new PrefixEntry("UA", "European Russia", "UA", "RA"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "UA2", "UA2"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "RA2", "RA2"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "RN2", "RN2"));
                this.Add(new PrefixEntry("UA2", "Kaliningrad", "RK2", "RK2"));
                this.Add(new PrefixEntry("YO", "Romania", "YO", "YR"));				
				this.Add(new PrefixEntry("ZA","Albania","ZA","ZA"));				
				this.Add(new PrefixEntry("ZB4","Gibraltar","ZB4","ZB4"));				
				this.Add(new PrefixEntry("9H","Malta","9H","9H"));				
				this.Add(new PrefixEntry("TA","Turkey","TA","TA"));				
				this.Add(new PrefixEntry("T7","San Marino","T7","T7"));				
				// nach Länge sortieren, längste Einträge zuerst
				this.Sort(new LengthComparer());
			}
		}

		// eigentliche Check - Funtionen

		public static double Lat ( string S )
		{
			double StepB1 = 10;
			double StepB2 = StepB1/10;
			double StepB3 = StepB2/24;
			double StartB1 = -90;
			double StartB2 = 0;
			double StartB3 = StepB3/2;
			try
			{
				S = S.ToUpper();
				if ((S[1] < 'A') || (S[1] > 'Z') ||
					(S[3] < '0') || (S[3] > '9') ||
					(S[5] < 'A') || (S[5] > 'X'))
				{
					return -360;
				}
				return	StartB1+StepB1*(System.Convert.ToInt16(S[1])-0x41)+
					StartB2+StepB2*(System.Convert.ToInt16(S[3])-0x30)+
					StartB3+StepB3*(System.Convert.ToInt16(S[5])-0x41);
			}
			catch
			{
				// Fehler bei der Breitenberechnung
				return -360;
			}
		}

		public static double Lon ( string S )
		{
			try
			{
				double StepL1 = 20;
				double StepL2 = StepL1/10;
				double StepL3 = StepL2/24;
				double StartL1 = -180;
				double StartL2 = 0;
				double StartL3 = StepL3/2;
				S = S.ToUpper();
				if ((S[0] < 'A') || (S[0] > 'Z') ||
					(S[2] < '0') || (S[2] > '9') ||
					(S[4] < 'A') || (S[4] > 'X'))
				{
					return  -360;
				}
				return	StartL1+StepL1*(System.Convert.ToInt16(S[0])-0x41)+
					StartL2+StepL2*(System.Convert.ToInt16(S[2])-0x30)+
					StartL3+StepL3*(System.Convert.ToInt16(S[4])-0x41);
			}
			catch
			{
				// Fehler bei der Längenberechnung
				return -360;
			}
		}

		public static string Loc ( double L,double B )
		{
			try
			{
				double StepB1 = 10;
				double StepB2 = StepB1/10;
				double StepB3 = StepB2/24;
				double StartB1 = -90;
				double StartB2 = 0;
				double StartB3 = StepB3/2;
				double StepL1 = 20;
				double StepL2 = StepL1/10;
				double StepL3 = StepL2/24;
				double StartL1 = -180;
				double StartL2 = 0;
				double StartL3 = StepL3/2;
				int i0,i1,i2,i3,i4,i5;
				char S0,S1,S2,S3,S4,S5;
				i0 = System.Convert.ToInt32(Math.Floor((L - StartL1)/StepL1));
				S0 = System.Convert.ToChar(i0+0x41);
				L = L - i0 * StepL1 - StartL1;
				i2 = System.Convert.ToInt32(Math.Floor((L - StartL2)/StepL2));
				S2 = System.Convert.ToChar(i2+0x30);
				L = L - i2 * StepL2 - StartL2;
				i4 = System.Convert.ToInt32((L - StartL3)/StepL3);
				S4 = System.Convert.ToChar(i4+0x41);
				i1 = System.Convert.ToInt32(Math.Floor((B - StartB1)/StepB1));
				S1 = System.Convert.ToChar(i1+0x41);
				B = B - i1 * StepB1 - StartB1;
				i3 = System.Convert.ToInt32(Math.Floor((B - StartB2)/StepB2));
				S3 = System.Convert.ToChar(i3+0x30);
				B = B - i3 * StepB2 - StartB2;
				i5 = System.Convert.ToInt32((B - StartB3)/StepB3);
				S5 = System.Convert.ToChar(i5+0x41);
				string S = System.String.Concat(S0,S1,S2,S3,S4,S5);
				return S;
			}
			catch
			{
				// Fehler bei der Locatorberechnung
				return "";
			}
		}

		public static int QRB ( string MyLoc, string Loc )
        {
            return QRB(Lat(MyLoc), Lon(MyLoc), Lat(Loc), Lon(Loc));
        }
        
        public static int QRB (double MyLat, double MyLon, double Lat, double Lon)
        {
			try
			{
				// double F = 2*Math.PI*Radius/360;
                // acording to IARU Reg1 contest rules
                double F = 111.2;
                double E;
				if ((MyLon<-180) || (MyLon>180) ||
					(MyLat<-90) || (MyLat>90) ||
					(Lon<-180) || (Lon>180) ||
					(Lat<-90) || (Lat>90))
					return -1;
				E  =  Math.Sin(MyLat/180*Math.PI)*Math.Sin(Lat/180*Math.PI)+
					Math.Cos(MyLat/180*Math.PI)*Math.Cos(Lat/180*Math.PI)*Math.Cos((MyLon-Lon)/180*Math.PI);
				if (E == 0) return 0;
				E  =  Math.Sqrt(1-E*E)/E;
				E  =  Math.Atan(E)/Math.PI*180*F+0.5;
				return System.Convert.ToInt32(Math.Round(E,0));
			}
			catch
			{
				// Fehler bei der Entfernungsberechnung
				return -1;
			}
		}

        public static double QTF(string MyLoc, string Loc)
        {
            return QTF(Lat(MyLoc), Lon(MyLoc), Lat(Loc), Lon(Loc));
        }

        public static double QTF (double MyLat, double MyLon, double Lat, double Lon)
		{
			try
			{
				double F = 2*Math.PI*Radius/360;
                double E;
				double CE,DL;
				if ((MyLon<-180) || (MyLon>180) ||
					(MyLat<-90) || (MyLat>90) ||
					(Lon<-180) || (Lon>180) ||
					(Lat<-90) || (Lat>90))
					return -1;
				DL = Lon - MyLon;
				CE = (Math.Tan(Lat/180*Math.PI) * Math.Cos(MyLat/180*Math.PI) - Math.Sin(MyLat/180*Math.PI)) * Math.Cos(DL/180*Math.PI);
				if (CE == 0) 
				{
					if (DL < 0) return 270;
					if (DL > 0) return 90;
					return 0;
				}
				E = Math.Atan(Math.Sin(DL/180*Math.PI)/CE)/Math.PI*180;
				if (CE < 0) 
					E += 180;
				if ((CE > 0) && (E < 0)) 
					E = E + 360;
				return E;
			}
			catch
			{
				// Fehler bei der Winkelberechnung
				return -1;
			}
		}

		public static string Cut ( string S )
		{
			try
			{
				S.Trim().ToUpper();
				if (S.IndexOf('/')>= 0)
				{
					// hinteren Teil abschneiden
					if(S.IndexOf('/') >= S.Length-4)
						S = S.Remove(S.IndexOf('/'),S.Length-S.IndexOf('/'));
					// evtl noch vorderen Teil abschneiden
					if (S.IndexOf('/') >= 0)
						S = S.Remove(0,S.IndexOf('/')+1);
					// nochmals hinteren Teil abschneiden
					if(S.IndexOf('/') >= S.Length-4)
						S = S.Remove(S.IndexOf('/'),S.Length-S.IndexOf('/'));
				}
				return S;
			}
			catch
			{
				// Fehler beim Abschneiden des Rufzeichens
				return "";
			}
		}

		public static string Prefix (string S)
		{
			S = WCCheck.Cut(S).ToUpper();
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

		public static int IsCall (string S)
		{
			// bewertet Übergabestring als Call 
			// Rückgabewerte :  -1 - auf keinen Fall         
			//                   0 - vielleicht              
			//					+1 - wahrscheinlich       
			try 
			{
				S = S.Trim();
				S = S.ToUpper();
				// auf unerlaubte Zeichen testen 
				for (int j = 0;j < S.Length;j++)
				{
					if (((S[j] <'A') || (S[j] > 'Z')) &&
						((S[j] < '0') || (S[j] > '9')) &&
						((S[j] != '/')))
					{
						return -1;
					}			
				}
				// Rufzeichen von Zusätzen befreien
				S = Cut(S);
				// Rufzeichen zu kurz
				if (S.Length < 3)
					return -1;
				// eigentlicher Test
				// erstes Zeichen Zahl
				if (Char.IsNumber(S,0))
				{	
					// zweites Zeichen muss Buchstabe sein
					// drittes Zeichen muss Zahl sein
					// Beispiel : 9A5O
					if (Char.IsLetter(S,1) && Char.IsNumber(S,2))
						return 1;
					return -1;
				}
				else
				{
					// erstes Zeichen Buchstabe
					// zweites Zeichen Buchstabe
					if (Char.IsLetter(S,1))
					{
						// drittes Zeichen muss Zahl sein
						// Beispiel DL0GTH
						if (Char.IsNumber(S,2))
							return 1;
						return -1;
					}
					else
					{
						// zweites Zeichen Zahl
						// drittes Zeichen Buchstabe
						// Beispiel G7RAU
						if (Char.IsLetter(S,2))
							return 1;
						// drittes Zeichen Zahl
						// Beispiel T91CO
						if (Char.IsLetter(S,3))
							return 1;
						return -1;
					}
				}
			}
			catch
			{
				return -1;
			}
		}

		public static int IsLoc (string S)
		{
			// bewertet Übergabestring als Locator 
			// Rückgabewerte :  -1 - auf keinen Fall         
			//                   0 - vielleicht              
			//					+1 - wahrscheinlich          
			S = S.Trim();
			S = S.ToUpper();
			if (S.Length == 6)
			{
				if ((S[0] >= 'A') && (S[0] <= 'X') &&
					(S[1] >= 'A') && (S[1] <= 'X') &&
					(S[2] >= '0') && (S[2] <= '9') &&
					(S[3] >= '0') && (S[3] <= '9') &&
					(S[4] >= 'A') && (S[4] <= 'X') &&
					(S[5] >= 'A') && (S[5] <= 'X'))
				{
					return 1;
				}
			}
			return -1;
		}

		public static bool IsNumeric (string S)
		{
			// bewertet Übergabestring als Ganzzahl
			int i = 0;
			while ((i< S.Length) && (S[i] >= '0') && (S[i] <= '9'))
				i+=1;
			return (i >= S.Length);
		}


        public static bool IsPrecise(string loc, double lat, double lon)
        {
            double mlat = LatLon.Lat(loc);
            double mlon = LatLon.Lon(loc);
            string mloc = LatLon.Loc(lat, lon);
            if ((loc == mloc) && ((Math.Abs(mlat - lat) > 0.00001) || (mlon - lon) > 0.00001))
            {
                return true;
            }
            return false;
        }


	}

	public class WCUtils : Object
	{
		public static void Debug ( string S, int priority )
		{
			System.Console.WriteLine(System.DateTime.UtcNow+"("+priority+") - "+S);
		}
	}
    */
}
