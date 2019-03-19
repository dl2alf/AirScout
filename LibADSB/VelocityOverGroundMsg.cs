using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LibADSB
{
    public class VelocityOverGroundMsg : ExtendedSquitter
    {
        [Browsable(false)]
        [DescriptionAttribute("IFR capabilities in Velocity Over Ground Message")]
        public bool IFRCapability
        {
            get
            {
                return ifr_capability;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute(" Velocity is availble in Velocity Over Ground Message")]
        public bool HasValidVelocity
        {
            get
            {
                return velocity_info_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute(" Velocity in Velocity Over Ground Message [kts]")]
        public int Velocity
        {
            get
            {
    		    return (int)Math.Sqrt(north_south_velocity*north_south_velocity + east_west_velocity * east_west_velocity);
            }
        }

        [Browsable(false)]
        [DescriptionAttribute(" Direction is WEST in Velocity Over Ground Message")]
        public bool DirectionWest
        {
            get
            {
                return direction_west;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute(" East to West velocity in Velocity Over Ground Message [kts]")]
        public int EastToWestVelocity
        {
            get
            {
    		    return (int)((direction_west ? east_west_velocity : -east_west_velocity) + 0.5);
            }
        }

        [Browsable(false)]
        [DescriptionAttribute(" Direction is SOUTH in Velocity Over Ground Message")]
        public bool DirectionSouth
        {
            get
            {
                return direction_south;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute(" North to South velocity in Velocity Over Ground Message [kts]")]
        public int NorthToSouthVelocity
        {
            get
            {
    		    return (int)((direction_south ? north_south_velocity : -north_south_velocity) + 0.5);
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Vertical rate is availble in Velocity Over Ground Message")]
        public bool HasValidVerticalRate
        {
            get
            {
                return vertical_rate_info_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Vertical rate in Velocity Over Ground Message [ft/s]")]
        public int VerticalRate
        {
            get
            {
    		    return (int)((vertical_rate_down ? -vertical_rate : vertical_rate) * 0.00508);
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Heading is availble in Airspeed Heading Message")]
        public bool HasValidHeading
        {
            get
            {
                // heading can only calculated when velocity info is available!
                return velocity_info_available;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Heading in Airspeed Heading Message [deg]")]
        public int Heading
        {
            get
            {
		        int angle = (int) (Math.Atan2(
				        -this.EastToWestVelocity,
				        -this.NorthToSouthVelocity) * 180.0 / Math.PI);
		
		        // if negative => clockwise
		        if (angle < 0) 
                    return 360 + angle;
		        else 
                    return angle;
            }
        }


        private byte subtype;
	    private bool intent_change;
	    private bool ifr_capability;
	    private byte navigation_accuracy_category;
	    private bool direction_west; // 0 = east, 1 = west
	    private int east_west_velocity; // in kn
	    private bool velocity_info_available;
	    private bool direction_south; // 0 = north, 1 = south
	    private int north_south_velocity; // in kn
	    private bool vertical_source; // 0 = geometric, 1 = barometric
	    private bool vertical_rate_down; // 0 = up, 1 = down
	    private int vertical_rate; // in ft/s
	    private bool vertical_rate_info_available;
	    private int geo_minus_baro; // in ft
	    private bool geo_minus_baro_available;
	
	    /**
	        * @param raw_message raw ADS-B velocity-over-ground message as hex string
	        * @throws BadFormatException if message has wrong format
	        */
	    public VelocityOverGroundMsg(string raw_message)
            : base(raw_message)
        {
		
		    if (this.FormatTypeCode != 19) 
            {
			    throw new BadFormatException("Velocity messages must have typecode 19: " + raw_message);
		    }
		
		    byte[] msg = Message;
		
		    subtype = (byte) (msg[0]&0x7);
		    if (subtype != 1 && subtype != 2) 
            {
			    throw new BadFormatException("Ground speed messages have subtype 1 or 2: " + raw_message);
		    }
		
		    intent_change = (msg[1]&0x80)>0;
		    ifr_capability = (msg[1]&0x40)>0;
		    navigation_accuracy_category = (byte) ((msg[1]>>3)&0x7);
		
		    // check this later
		    velocity_info_available = true;
		    vertical_rate_info_available = true;
		    geo_minus_baro_available = true;
		
		    direction_west = (msg[1]&0x4)>0;
		    east_west_velocity = (short) (((msg[1]&0x3)<<8 | msg[2]&0xFF)-1);
		    if (east_west_velocity == -1) velocity_info_available = false;
		    if (subtype == 2) east_west_velocity<<=2;
		
		    direction_south = (msg[3]&0x80)>0;
		    north_south_velocity = (short) (((msg[3]&0x7F)<<3 | msg[4]>>5&0x07)-1);
		    if (north_south_velocity == -1) velocity_info_available = false;
		    if (subtype == 2) north_south_velocity<<=2;

		    vertical_source = (msg[4]&0x10)>0;
		    vertical_rate_down = (msg[4]&0x08)>0;
		    vertical_rate = (short) ((((msg[4]&0x07)<<6 | msg[5]>>2&0x3F)-1)<<6);
		    if (vertical_rate == -1) vertical_rate_info_available = false;
		
		    geo_minus_baro = (short) (((msg[6]&0x7F)-1)*25);
		    if (geo_minus_baro == -1) geo_minus_baro_available = false;
		    if ((msg[6]&0x80)>0) geo_minus_baro *= -1;
	    }

	    public bool hasGeoMinusBaroInfo() {
		    return geo_minus_baro_available;
	    }

	    /**
	        * @return If supersonic, velocity has only 4 kts accuracy, otherwise 1 kt
	        */
	    public bool isSupersonic() {
		    return subtype == 2;
	    }
	
	    /**
	        * @return true, if aircraft wants to change altitude for instance
	        */
	    public bool hasChangeIntent() {
		    return intent_change;
	    }


	    /**
	        * @return NAC according to RTCA DO-260A
	        */
	    public byte getNavigationAccuracyCategory() {
		    return navigation_accuracy_category;
	    }


	    /**
	        * @return whether altitude is derived by barometric sensor or GNSS
	        */
	    public bool isBarometricVerticalSpeed() {
		    return vertical_source;
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
	    public override string ToString() 
        {
		    string ret = base.ToString()+"\n"+
				    "Velocity over ground:\n";
		    try { ret += "\tNorth to south velocity:\t"+NorthToSouthVelocity+"\n"; }
		    catch (Exception e) { ret += "\tNorth to south velocity:\t\tnot available\n"; }
		    try { ret += "\tEast to west velocity:\t\t"+EastToWestVelocity+"\n"; }
		    catch (Exception e) { ret += "\tEast to west velocity:\t\tnot available\n"; }
		    try { ret += "\tVelocity:\t\t\t"+Velocity+"\n"; }
		    catch (Exception e) { ret += "\tVelocity:\t\t\tnot available\n"; }
		    try { ret += "\tHeading\t\t\t\t"+Heading+"\n"; }
		    catch (Exception e) { ret += "\tHeading\t\t\t\tnot available\n"; }
		    try { ret += "\tVertical rate:\t\t\t"+VerticalRate; }
		    catch (Exception e) { ret += "\tVertical rate:\t\t\tnot available"; }
		
		    return ret;
	    }
    }
}
