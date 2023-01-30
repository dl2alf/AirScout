using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LibADSB
{
    public class AirbornePositionMsg : ExtendedSquitter
    {

        [Browsable(false)]
        [DescriptionAttribute("Position is availble in Airborne Position Message")]
        public bool HasValidPosition
        {
            get
            {
                return horizontal_position_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Altitude is availble in Airborne Position Message")]
        public bool HasValidAltitude
        {
            get
            {
                return altitude_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Altitude in Airborne Position Message [ft]")]
        public int Altitude
        {
            get
            {
				if (this.FormatTypeCode < 20)
				{ 
					// barometric altitude
					bool Qbit = (altitude_encoded&0x10)!=0;
					int N;
					if (Qbit)
					{ // altitude reported in 25ft increments
						N = (altitude_encoded & 0xF) | ((altitude_encoded & 0xFE0) >> 1);
						return (int)((25 * N - 1000) + 0.5);
					}
					else
					{ // altitude is above 50175ft, so we use 100ft increments

						// it's decoded using the Gillham code
						int C1 = (0x800 & altitude_encoded) >> 11;
						int A1 = (0x400 & altitude_encoded) >> 10;
						int C2 = (0x200 & altitude_encoded) >> 9;
						int A2 = (0x100 & altitude_encoded) >> 8;
						int C4 = (0x080 & altitude_encoded) >> 7;
						int A4 = (0x040 & altitude_encoded) >> 6;
						int B1 = (0x020 & altitude_encoded) >> 5;
						int B2 = (0x008 & altitude_encoded) >> 3;
						int D2 = (0x004 & altitude_encoded) >> 2;
						int B4 = (0x002 & altitude_encoded) >> 1;
						int D4 = (0x001 & altitude_encoded);

						// this is standard gray code
						int N500 = grayToBin(D2 << 7 | D4 << 6 | A1 << 5 | A2 << 4 | A4 << 3 | B1 << 2 | B2 << 1 | B4, 8);

						// 100-ft steps must be converted
						int N100 = grayToBin(C1 << 2 | C2 << 1 | C4, 3) - 1;
						if (N100 == 6) N100 = 4;
						if (N500 % 2 != 0) N100 = 4 - N100; // invert it

						return (int)((-1200 + N500 * 500 + N100 * 100) + 0.5);
					}
		        }
				else
                {
					// GNSS altitude, not implemented yet
					return 0;
                }
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Surveillance status of Airborne Position Message")]
        public byte SurveillanceStatus
        {
            get
            {
                return surveillance_status;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Synchronization status of Time of Applicability of Airborne Position Message")]
        public bool IsUTCTime
        {
            get
            {
                return time_flag;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Indicates ODD format of Airborne Position Message")]
        public bool IsOddFormat
        {
            get
            {
                return cpr_format;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Indicates barometric measuremnet of altitude of Airborne Position Message")]
        public bool IsBarometricAltitude
        {
            get
            {
    		    return this.FormatTypeCode < 20;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("CPR-encoded Latitude of Airborne Position Message")]
        public int CPREncodedLat
        {
            get
            {
                return cpr_encoded_lat;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("CPR-encoded Longitude of Airborne Position Message")]
        public int CPREncodedLon
        {
            get
            {
                return cpr_encoded_lon;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Horizontal containment radius limit of Airborne Position Message")]
        public double HorizontalContainmentRadiusLimit
        {
            get
            {
		        switch (FormatTypeCode) 
                {
		            case 0: case 18: case 22: return -1;
		            case 9: case 20: return 7.5;
		            case 10: case 21: return 25;
		            case 11:
			            if (nic_suppl_a) return 75;
			            else return 185.2;
		            case 12: return 370.4;
		            case 13:
			            if (!nic_suppl_b) return 926;
			            else if (nic_suppl_a) return 1111.2;
			            else return 555.6;
		            case 14: return 1852;
		            case 15: return 3704;
		            case 16:
			            if (nic_suppl_a) return 7408;
			            else return 14816;
		            case 17: return 37040;
		            default: return 0;
		        }
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("")]
        public byte NavigationIntegrityCategory
        {
            get
            {
 		        switch (FormatTypeCode) 
                {
		            case 0: case 18: case 22: return 0;
		            case 9: case 20: return 11;
		            case 10: case 21: return 10;
		            case 11:
			            if (nic_suppl_a) return 9;
			            else return 8;
		            case 12: return 7;
		            case 13: return 6;
		            case 14: return 5;
		            case 15: return 4;
		            case 16:
			            if (nic_suppl_a) return 3;
			            else return 2;
		            case 17: return 1;
		            default: return 0;
		        }
           }
        }

        [Browsable(false)]
        [DescriptionAttribute("NIC supplement A of Airborne Position Message")]
        public bool NICSupplementA
        {
            get
            {
                return nic_suppl_a;
            }
            set
            {
                nic_suppl_a = value;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("NIC supplement B of Airborne Position Message")]
        public bool NICSupplementB
        {
            get
            {
                return nic_suppl_b;
            }
            set
            {
                nic_suppl_b = value;
            }
        }

	    private bool horizontal_position_available;
	    private bool altitude_available;
	    private byte surveillance_status;
	    private bool nic_suppl_b;
	    private short altitude_encoded;
	    private bool time_flag;
	    private bool cpr_format;
	    private int cpr_encoded_lat;
	    private int cpr_encoded_lon;
	    private bool nic_suppl_a;

	    /**
	     * @param raw_message raw ADS-B airborne position message as hex string
	     * @throws BadFormatException if message has wrong format
	     */
	    public AirbornePositionMsg(String raw_message)
            : base(raw_message)
        {

		    if (!(FormatTypeCode == 0 ||
				    (FormatTypeCode >= 9 && FormatTypeCode <= 18) || (FormatTypeCode >= 20 && FormatTypeCode <= 22))) 
			    throw new BadFormatException("This is not a position message! Wrong format type code ("+FormatTypeCode+"): " + raw_message);

		    byte[] msg = Message;

		    horizontal_position_available = FormatTypeCode != 0;

		    surveillance_status = (byte) ((msg[0]>>1)&0x3);
		    nic_suppl_b = (msg[0]&0x1) == 1;

		    altitude_encoded = (short) (((msg[1]<<4)|((msg[2]>>4)&0xF))&0xFFF);
		    altitude_available = altitude_encoded != 0;

		    time_flag = ((msg[2]>>3)&0x1) == 1;
		    cpr_format = ((msg[2]>>2)&0x1) == 1;
		    cpr_encoded_lat = (((msg[2]&0x3)<<15) | ((msg[3]&0xFF)<<7) | ((msg[4]>>1)&0x7F)) & 0x1FFFF;
		    cpr_encoded_lon = (((msg[4]&0x1)<<16) | ((msg[5]&0xFF)<<8) | (msg[6]&0xFF)) & 0x1FFFF;
	    }

	    /**
	     * This is a function of the surveillance status field in the position
	     * message.
	     * 
	     * @return surveillance status description as defines in DO-260B
	     */
	    public string getSurveillanceStatusDescription() 
        {
		    string[] desc = 
            {
				    "No condition information",
				    "Permanent alert (emergency condition)",
				    "Temporary alert (change in Mode A identity code oter than emergency condition)",
				    "SPI condition"
		    };

		    return desc[surveillance_status];
	    }

	    /**
	     * @param Rlat Even or odd Rlat value (CPR internal)
	     * @return the number of even longitude zones at a latitude
	     */
	    private double NL(double Rlat) 
        {
		    if (Rlat == 0) return 59;
		    else if (Math.Abs(Rlat) == 87) return 2;
		    else if (Math.Abs(Rlat) > 87) return 1;

		    double tmp = 1-(1-Math.Cos(Math.PI/(2.0*15.0)))/Math.Pow(Math.Cos(Math.PI/180.0*Math.Abs(Rlat)), 2);
		    return Math.Floor(2*Math.PI/Math.Acos(tmp));
	    }
	
	    /**
	     * Modulo operator in java has stupid behavior
	     */
	    private static double mod(double a, double b) {
		    return ((a%b)+b)%b;
	    }

	    /**
	     * This method can only be used if another position report with a different format (even/odd) is available
	     * and set with msg.setOtherFormatMsg(other).
	     * @param other airborne position message of the other format (even/odd). Note that the time between
	     *        both messages should be not longer than 10 seconds! 
	     * @return globally unambiguously decoded position tuple (latitude, longitude). The positional
	     *         accuracy maintained by the Airborne CPR encoding will be approximately 5.1 meters.
	     *         A message of the other format is needed for global decoding.
	     * @throws MissingInformationException if no position information is available in one of the messages
	     * @throws IllegalArgumentException if input message was emitted from a different transmitter
	     * @throws PositionStraddleError if position messages straddle latitude transition
	     * @throws BadFormatException other has the same format (even/odd)
	     */
	    public double[] getGlobalPosition(AirbornePositionMsg other) 
        {
			    if (!other.ICAO24.SequenceEqual(ICAO24))
				    throw new IllegalArgumentException(
						    string.Format("Transmitter of other message (%s) not equal to this (%s):",
						    BitConverter.ToString(other.ICAO24).Replace("-",String.Empty), BitConverter.ToString(ICAO24).Replace("-",String.Empty)));
		
		    if (other.IsOddFormat == IsOddFormat)
			    throw new BadFormatException("Expected "+ (IsOddFormat? "even":"odd") + " message format:" + other.ToString());

		    if (!horizontal_position_available)
			    throw new MissingInformationException("No position information available!");
		    if (!other.HasValidPosition)
			    throw new MissingInformationException("Other message has no position information.");

		    AirbornePositionMsg even = IsOddFormat?other:this;
		    AirbornePositionMsg odd = IsOddFormat?this:other;

		    // Helper for latitude single(Number of zones NZ is set to 15)
		    double Dlat0 = 360.0/60.0;
		    double Dlat1 = 360.0/59.0;

		    // latitude index
		    double j = Math.Floor((59.0*even.CPREncodedLat-60.0*odd.CPREncodedLat)/((double)(1<<17))+0.5);

		    // global latitudes
		    double Rlat0 = Dlat0 * (mod(j,60)+even.CPREncodedLat/((double)(1<<17)));
		    double Rlat1 = Dlat1 * (mod(j,59)+odd.CPREncodedLat/((double)(1<<17)));

		    // Southern hemisphere?
		    if (Rlat0 >= 270 && Rlat0 <= 360) Rlat0 -= 360;
		    if (Rlat1 >= 270 && Rlat1 <= 360) Rlat1 -= 360;

		    // Northern hemisphere?
		    if (Rlat0 <= -270 && Rlat0 >= -360) Rlat0 += 360;
		    if (Rlat1 <= -270 && Rlat1 >= -360) Rlat1 += 360;

		    // ensure that the number of even longitude zones are equal
		    if (NL(Rlat0) != NL(Rlat1))
			    throw new PositionStraddleException(
				    "The two given position straddle a transition latitude "+
				    "and cannot be decoded. Wait for positions where they are equal.");

		    // Helper for longitude
		    double Dlon0 = 360.0/Math.Max(1.0, NL(Rlat0));
		    double Dlon1 = 360.0/Math.Max(1.0, NL(Rlat1)-1);

		    // longitude index
		    double NL_helper = NL(IsOddFormat?Rlat1:Rlat0); // assuming that this is the newer message
		    double m = Math.Floor((even.CPREncodedLon*(NL_helper-1)-odd.CPREncodedLon*NL_helper)/((double)(1<<17))+0.5);

		    // global longitude
		    double Rlon0 = Dlon0 * (mod(m,Math.Max(1.0, NL(Rlat0))) + even.CPREncodedLon/((double)(1<<17)));
		    double Rlon1 = Dlon1 * (mod(m,Math.Max(1.0, NL(Rlat1)-1)) + odd.CPREncodedLon/((double)(1<<17)));

		    // correct longitude
		    if (Rlon0 < -180 && Rlon0 > -360) Rlon0 += 360;
		    if (Rlon1 < -180 && Rlon1 > -360) Rlon1 += 360;
		    if (Rlon0 > 180 && Rlon0 < 360) Rlon0 -= 360;
		    if (Rlon1 > 180 && Rlon1 < 360) Rlon1 -= 360;
		
		    return new double[] {IsOddFormat?Rlat1:Rlat0, IsOddFormat?Rlon1:Rlon0};
	    }
	
	    /**
	     * This method uses a locally unambiguous decoding for airborne position messages. It
	     * uses a reference position known to be within 180NM (= 333.36km) of the true target
	     * airborne position. the reference point may be a previously tracked position that has
	     * been confirmed by global decoding (see getGlobalPosition()).
	     * @param ref_lat latitude of reference position
	     *        ref_lon longitude of reference position
	     * @return decoded position as tuple (latitude, longitude). The positional
	     *         accuracy maintained by the Airborne CPR encoding will be approximately 5.1 meters.
	     * @throws MissingInformationException if no position information is available
	     */
	    public double[] getLocalPosition(double ref_lat, double ref_lon) 
        {
		    if (!horizontal_position_available)
			    throw new MissingInformationException("No position information available!");
		
		    // latitude zone size
		    double Dlat = IsOddFormat ? 360.0/59.0 : 360.0/60.0;
		
		    // latitude zone index
		    double j = Math.Floor(ref_lat/Dlat) + Math.Floor(0.5+(mod(ref_lat, Dlat))/Dlat-CPREncodedLat/((double)(1<<17)));
		
		    // decoded position latitude
		    double Rlat = Dlat*(j+CPREncodedLat/((double)(1<<17)));
		
		    // longitude zone size
		    double Dlon = 360.0/Math.Max(1.0, NL(Rlat)-(IsOddFormat?1.0:0.0));
		
		    // longitude zone coordinate
		    double m =
				    Math.Floor(ref_lon/Dlon) +
				    Math.Floor(0.5+(mod(ref_lon,Dlon))/Dlon-(float)CPREncodedLon/((double)(1<<17)));
		
		    // and finally the longitude
		    double Rlon = Dlon * (m + CPREncodedLon/((double)(1<<17)));
		
    //		System.out.println("Loc: EncLon: "+CPREncodedLon+
    //				" m: "+m+" Dlon: "+Dlon+ " Rlon2: "+Rlon2);
		
		    return new double[] {Rlat,Rlon}; 
	    }

	    /**
	     * This method converts a gray code encoded int to a standard decimal int
	     * @param gray gray code encoded int of length bitlength
	     *        bitlength bitlength of gray code
	     * @return radix 2 encoded integer
	     */
	    private static int grayToBin(int gray, int bitlength) {
		    int result = 0;
		    for (int i = bitlength-1; i >= 0; --i)
			    result = result|((((0x1<<(i+1))&result)>>1)^((1<<i)&gray));
		    return result;
	    }

	
	    public override string ToString() 
        {
		    return base.ToString()+"\n"+
				    "Position:\n"+
				    "\tFormat:\t\t"+(IsOddFormat?"odd":"even")+
				    "\tHas position:\t"+(HasValidPosition? "[Lat=" + CPREncodedLat.ToString("F8") + ",Lon=" + CPREncodedLon.ToString("F8") + "]" :"no")+
				    "\tHas altitude:\t"+(HasValidAltitude? "[Alt=" + Altitude.ToString("F8") + "]" :"no");
	    }
    }
}
