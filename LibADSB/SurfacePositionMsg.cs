using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LibADSB
{
    public class SurfacePositionMsg : ExtendedSquitter
    {

        [Browsable(false)]
        [DescriptionAttribute("Position is availble in Surface Position Message")]
        public bool HasValidPosition
        {
            get
            {
                return horizontal_position_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Object has ground speed in Surface Position Message")]
        public bool HasGroundSpeed
        {
            get
            {
    		    return (movement >= 1) && (movement <= 124);
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Object ground speed in Surface Position Message [m/s]")]
        public double GroundSpeed
        {
            get
            {
    		    double speed;
		        if (movement == 1)
			        speed = 0;
		        else if (movement >= 2 && movement <= 8)
			        speed = 0.125+(movement-2)*0.125;
		        else if (movement >= 9 && movement <= 12)
			        speed = 1+(movement-9)*0.25;
		        else if (movement >= 13 && movement <= 38)
			        speed = 2+(movement-13)*0.5;
		        else if (movement >= 39 && movement <= 93)
			        speed = 15+(movement-39);
		        else if (movement >= 94 && movement <= 108)
			        speed = 70+(movement-94)*2;
		        else if (movement >= 109 && movement <= 123)
			        speed = 100+(movement-109)*5;
		        else if (movement == 124)
			        speed = 175;
		        else
			        throw new MissingInformationException("Ground speed info not available!");
		
		        return speed*0.514444;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Object ground speed resolution in Surface Position Message [m/s]")]
        public double GroundSpeedResolution
        {
            get
            {
		        double resolution;
		
		        if (movement >= 1 && movement <= 8)
			        resolution = 0.125;
		        else if (movement >= 9 && movement <= 12)
			        resolution = 0.25;
		        else if (movement >= 13 && movement <= 38)
			        resolution = 0.5;
		        else if (movement >= 39 && movement <= 93)
			        resolution = 1;
		        else if (movement >= 94 && movement <= 108)
			        resolution = 2;
		        else if (movement >= 109 && movement <= 123)
			        resolution = 5;
		        else if (movement == 124)
			        resolution = 175;
		        else
			        throw new MissingInformationException("Ground speed info not available!");
		
		        return resolution*0.514444;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Object has valid heading in Surface Position Message")]
        public bool HasValidHeading
        {
            get
            {
    		    return heading_status;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Object heading in Surface Position Message")]
        public double Heading
        {
            get
            {
    		    return ground_track * 360.0 / 128.0;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Synchronization status of Time of Applicability of Surface Position Message")]
        public bool IsUTCTime
        {
            get
            {
                return time_flag;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Indicates ODD format of Surface Position Message")]
        public bool IsOddFormat
        {
            get
            {
                return cpr_format;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Indicates barometric measuremnet of altitude of Surface Position Message")]
        public bool IsBarometricAltitude
        {
            get
            {
    		    return this.FormatTypeCode < 20;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("CPR-encoded Latitude of Surface Position Message")]
        public int CPREncodedLat
        {
            get
            {
                return cpr_encoded_lat;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("CPR-encoded Longitude of Surface Position Message")]
        public int CPREncodedLon
        {
            get
            {
                return cpr_encoded_lon;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("NIC supplement of Surface Position Message")]
        public byte NICSupplement
        {
            get
            {
                return nic_suppl;
            }
            set
            {
                nic_suppl = value;
            }
        }


        private bool horizontal_position_available;
	    private byte movement;
	    private bool heading_status; // is heading valid?
	    private byte ground_track;
	    private bool time_flag;
	    private bool cpr_format;
	    private int cpr_encoded_lat;
	    private int cpr_encoded_lon;
	    private byte nic_suppl;
	

	    /**
	     * @param raw_message raw ADS-B surface position message as hex string
	     * @throws BadFormatException if message has wrong format
	     */
	    public SurfacePositionMsg(String raw_message) 
            : base(raw_message)
        {
		    if (!(FormatTypeCode == 0 ||
				    (FormatTypeCode >= 5 && FormatTypeCode <= 8)))
			    throw new BadFormatException("This is not a position message! Wrong format type code ("+FormatTypeCode+"): " +raw_message);

		    byte[] msg = Message;

		    horizontal_position_available = FormatTypeCode != 0;

		    movement = (byte) ((((msg[0]&0x7)<<4) | ((msg[1]&0xF0)>>4))&0x7F);
		    heading_status = (msg[1]&0x8) != 0;
		    ground_track = (byte) ((((msg[1]&0x7)<<4) | ((msg[2]&0xF0)>>4))&0x7F);

		    time_flag = ((msg[2]>>3)&0x1) == 1;
		    cpr_format = ((msg[2]>>2)&0x1) == 1;
		    cpr_encoded_lat = (((msg[2]&0x3)<<15) | ((msg[3]&0xFF)<<7) | ((msg[4]>>1)&0x7F)) & 0x1FFFF;
		    cpr_encoded_lon = (((msg[4]&0x1)<<16) | ((msg[5]&0xFF)<<8) | (msg[6]&0xFF)) & 0x1FFFF;
	    }

	    /**
	     * @return horizontal containment radius limit in meters. A return value of 0 means "unkown".
	     *         If NIC supplement is set before, the return value is exactly according to DO-260B.
	     *         Otherwise it can be a little worse than it actually is. 0 means unkown.
	     */
	    public double getHorizontalContainmentRadiusLimit() 
        {
		    switch (FormatTypeCode) 
            {
		        case 0: return 0;
		        case 5: return 7.5;
		        case 6: return 25;
		        case 7:
			        if ((nic_suppl&0x5) == 0x4) return 75;
			        else return 185.2;
		        case 8:
			        if ((nic_suppl&0x5) == 0x5) return 370.4;
			        else if ((nic_suppl&0x5) == 0x4) return 555.6;
			        else return 1111.2;
		        default: return 0;
		    }
	    }

	    /**
	     * @return Navigation integrity category. A NIC of 0 means "unkown".
	     *         If NIC supplement is set before, the return value is exactly according to DO-260B.
	     *         Otherwise it might be a little worse than it actually is.
	     */
	    public byte getNavigationIntegrityCategory() 
        {
		    switch (FormatTypeCode) 
            {
		        case 0: return 0;
		        case 5: return 11;
		        case 6: return 10;
		        case 7:
			        if ((nic_suppl&0x5) == 0x4) return 9;
			        else return 8;
		        case 8:
			        if ((nic_suppl&0x5) == 0x5) return 7;
			        else if ((nic_suppl&0x5) == 0x4) return 6;
			        else if ((nic_suppl&0x5) == 0x1) return 6;
			        else return 0;
		        default: return 0;
		    }
	    }

	    /**
	     * @param Rlat Even or odd Rlat value (CPR internal)
	     * @return the number of even longitude zones at a latitude
	     */
	    private double NL(double Rlat) {
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
	     * @param other position message of the other format (even/odd). Note that the time between
	     *        both messages should be not longer than 50 seconds! 
	     * @return globally unambiguously decoded position tuple (latitude, longitude). The positional
	     *         accuracy maintained by the CPR encoding will be approximately 1.25 meters.
	     *         A message of the other format is needed for global decoding.
	     * @throws MissingInformationException if no position information is available in one of the messages
	     * @throws IllegalArgumentException if input message was emitted from a different transmitter
	     * @throws PositionStraddleError if position messages straddle latitude transition
	     * @throws BadFormatException other has the same format (even/odd)
	     */
	    public double[] getGlobalPosition(SurfacePositionMsg other)
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

		    SurfacePositionMsg even = IsOddFormat?other:this;
		    SurfacePositionMsg odd = IsOddFormat?this:other;

		    // Helper for latitude single(Number of zones NZ is set to 15)
		    double Dlat0 = 90.0/60.0;
		    double Dlat1 = 90.0/59.0;

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
		    double Dlon0 = 90.0/Math.Max(1.0, NL(Rlat0));
		    double Dlon1 = 90.0/Math.Max(1.0, NL(Rlat1)-1);

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
	     * This method uses a locally unambiguous decoding for position messages. It
	     * uses a reference position known to be within 45NM (= 83.34km) of the true target
	     * position. the reference point may be a previously tracked position that has
	     * been confirmed by global decoding (see getGlobalPosition()).
	     * @param ref_lat latitude of reference position
	     *        ref_lon longitude of reference position
	     * @return decoded position as tuple (latitude, longitude). The positional
	     *         accuracy maintained by the CPR encoding will be approximately 5.1 meters.
	     * @throws MissingInformationException if no position information is available
	     */
	    public double[] getLocalPosition(double ref_lat, double ref_lon)
        {
		    if (!horizontal_position_available)
			    throw new MissingInformationException("No position information available!");
		
		    // latitude zone size
		    double Dlat = IsOddFormat ? 90.0/59.0 : 90.0/60.0;
		
		    // latitude zone index
		    double j = Math.Floor(ref_lat/Dlat) + Math.Floor(0.5+(mod(ref_lat, Dlat))/Dlat-CPREncodedLat/((double)(1<<17)));
		
		    // decoded position latitude
		    double Rlat = Dlat*(j+CPREncodedLat/((double)(1<<17)));
		
		    // longitude zone size
		    double Dlon = 90.0/Math.Max(1.0, NL(Rlat)-(IsOddFormat?1.0:0.0));
		
		    // longitude zone coordinate
		    double m =
				    Math.Floor(ref_lon/Dlon) +
				    Math.Floor(0.5+(mod(ref_lon,Dlon))/Dlon-(float)CPREncodedLon/((double)(1<<17)));
		
		    // and finally the longitude
		    double Rlon = Dlon * (m + CPREncodedLon/((double)(1<<17)));
		
    //		System.out.println("Loc: EncLon: "+getCPREncodedLongitude()+
    //				" m: "+m+" Dlon: "+Dlon+ " Rlon2: "+Rlon2);
		
		    return new double[] {Rlat,Rlon}; 
	    }
	
	    public override string ToString() 
        {
		    try {
			    return base.ToString()+"\n"+
					    "Surface Position:\n"+
					    "\tSpeed:\t\t"+(HasGroundSpeed ? GroundSpeed.ToString("F8") : "unkown")+
					    "\n\tSpeed Resolution:\t\t"+(HasGroundSpeed ? GroundSpeedResolution.ToString("F8") : "unkown")+
					    "\n\tHeading:\t\t"+(HasValidHeading ? Heading.ToString("F8") : "unkown")+
					    "\n\tFormat:\t\t"+(IsOddFormat?"odd":"even")+
					    "\n\tHas position:\t"+(HasValidPosition?"yes":"no");
		    } catch (Exception ex) 
            {
			    // should never happen
			    return "An exception " + ex.Message + " was thrown.";
		    }
	    }

    }
}
