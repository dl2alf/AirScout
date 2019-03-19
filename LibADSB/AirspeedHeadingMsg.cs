using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LibADSB
{
    public class AirspeedHeadingMsg : ExtendedSquitter
    {
        [Browsable(false)]
        [DescriptionAttribute("Heading is availble in Airspeed Heading Message")]
        public bool HasValidHeading
        {
            get
            {
                return heading_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Heading in Airspeed Heading Message [kts]")]
        public int Heading
        {
            get
            {
                return heading;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Airspeed is availble in Airspeed Heading Message")]
        public bool HasValidAirspeed
        {
            get
            {
                return airspeed_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Airspeed is true airspeed in Airspeed Heading Message")]
        public bool IsTrueAirspeed
        {
            get
            {
                return true_airspeed;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Airspeed in Airspeed Heading Message [kts]")]
        public int Airspeed
        {
            get
            {
                return airspeed;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Heading is availble in Airspeed Heading Message")]
        public bool IsBarometricVerticalSpeed
        {
            get
            {
                return vertical_source;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Vertical rate is availble in Airspeed Heading Message")]
        public bool HasValidVerticalRate
        {
            get
            {
                return vertical_rate_info_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Vertical rate in Airspeed Heading Message")]
        public int VerticalRate
        {
            get
            {
                return vertical_rate;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Vertical source in Airspeed Heading Message")]
        public bool VerticalSource
        {
            get
            {
                return vertical_source;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Vertical rate DOWN in Airspeed Heading Message")]
        public bool VerticalRateDown
        {
            get
            {
                return vertical_rate_down;
            }
        }

        private byte subtype;
	    private bool intent_change;
	    private bool ifr_capability;
	    private byte navigation_accuracy_category;
	    private bool heading_available;
	    private int heading; // in degrees
	    private bool true_airspeed; // 0 = indicated AS, 1 = true AS
	    private int airspeed; // in knots
	    private bool airspeed_available;
	    private bool vertical_source; // 0 = geometric, 1 = barometric
	    private bool vertical_rate_down; // 0 = up, 1 = down
	    private int vertical_rate; // in ft/s
	    private bool vertical_rate_info_available;
	    private int geo_minus_baro; // in ft
	    private bool geo_minus_baro_available;
	
	    /**
	     * @param raw_message raw ADS-B airspeed and heading message as hex string
	     * @throws BadFormatException if message has wrong format
	     */
	    public AirspeedHeadingMsg(string raw_message) 
            : base(raw_message)
		{
		    if (this.FormatTypeCode != 19) 
            {
			    throw new BadFormatException("Airspeed and heading messages must have typecode 19: " + raw_message);
		    }
		
		    byte[] msg = this.Message;
		
		    subtype = (byte) (msg[0]&0x7);
		    if (subtype != 3 && subtype != 4) 
            {
			    throw new BadFormatException("Airspeed and heading messages have subtype 3 or 4: " + raw_message);
		    }
		
		    intent_change = (msg[1]&0x80)>0;
		    ifr_capability = (msg[1]&0x40)>0;
		    navigation_accuracy_category = (byte) ((msg[1]>>3)&0x7);
		
		    // check this later
		    vertical_rate_info_available = true;
		    geo_minus_baro_available = true;
		
		    heading_available = (msg[1]&0x4)>0;
		    heading = (int)(((msg[1]&0x3)<<8 | msg[2]&0xFF) * 360.0/1024.0);
		
		    true_airspeed = (msg[3]&0x80)>0;
		    airspeed = (short) (((msg[3]&0x7F)<<3 | msg[4]>>5&0x07)-1);
		    if (airspeed == -1) airspeed_available = false;
		    if (subtype == 2) airspeed<<=2;

		    vertical_source = (msg[4]&0x10)>0;
		    vertical_rate_down = (msg[4]&0x08)>0;
		    vertical_rate = (short) ((((msg[4]&0x07)<<6 | msg[5]>>2&0x3F)-1)<<6);
		    if (vertical_rate == -1) vertical_rate_info_available = false;
		
		    geo_minus_baro = (short) (((msg[6]&0x7F)-1)*25);
		    if (geo_minus_baro == -1) geo_minus_baro_available = false;
		    if ((msg[6]&0x80)>0) geo_minus_baro *= -1;
	    }

	    /**
	     * Must be checked before accessing geo minus baro!
	     * 
	     * @return whether geo-baro difference info is available
	     */
	    public bool hasGeoMinusBaroInfo() {
		    return geo_minus_baro_available;
	    }

	    /**
	     * @return If supersonic, velocity has only 4 kts accuracy, otherwise 1 kt
	     */
	    public bool isSupersonic() {
		    return subtype == 4;
	    }
	
	    /**
	     * @return true, if aircraft wants to change altitude for instance
	     */
	    public bool hasChangeIntent() {
		    return intent_change;
	    }

	    /**
	     * Note: only in ADS-B version 1 transponders!!
	     * @return true, iff aircraft has equipage class A1 or higher
	     */
	    public bool hasIFRCapability() {
		    return ifr_capability;
	    }


	    /**
	     * @return NAC according to RTCA DO-260A
	     */
	    public byte getNavigationAccuracyCategory() {
		    return navigation_accuracy_category;
	    }


	    /**
	     * @return difference between barometric and geometric altitude in m
	     * @throws MissingInformationException  if no geo/baro difference info is available
	     */
	    public double getGeoMinusBaro()
        {
		    if (!geo_minus_baro_available) throw new MissingInformationException("No geo/baro difference info available!");
		    return geo_minus_baro * 0.3048;
	    }
	
	    public override string ToString() {
		    string ret = base.ToString()+"\n"+
				    "Airspeed and heading:\n";
		    try { ret += "\tAirspeed:\t"+Airspeed+" kts\n"; }
		    catch (Exception e) { ret += "\tAirspeed:\t\tnot available\n"; }
		    ret += "\tAirspeed Type:\t\t"+(IsTrueAirspeed ? "true" : "indicated")+"\n";
		    try { ret += "\tHeading\t\t\t\t"+Heading+"\n"; }
		    catch (Exception e) { ret += "\tHeading\t\t\t\tnot available\n"; }
		    try { ret += "\tVertical rate:\t\t\t"+VerticalRate; }
		    catch (Exception e) { ret += "\tVertical rate:\t\t\tnot available"; }
		
		    return ret;
	    }
    }
}
