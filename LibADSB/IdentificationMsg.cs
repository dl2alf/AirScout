using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LibADSB
{
    public class IdentificationMsg : ExtendedSquitter
    {
        [Browsable(false)]
        [DescriptionAttribute("Emitter category of identification message")]
        public byte EmitterCategory
        {
            get
            {
                return emitter_category;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Emitter identity of identification message ")]
        public byte[] Identity
        {
            get
            {
                return identity;
            }
        }

	    private byte emitter_category;
	    private byte[] identity;
	
	    /**
	     * Maps ADS-B encoded to readable characters
	     * @param digit encoded digit
	     * @return readable character
	     */
	    private static char mapChar (byte digit) 
        {
		    if (digit>0 && digit<27) return (char) ('A'+digit-1);
		    else if (digit>47 && digit<58) return (char) ('0'+digit-48);
		    else return ' ';
	    }
	
	    /**
	     * Maps ADS-B encoded to readable characters
	     * @param digits array of encoded digits
	     * @return array of decoded characters
	     */
	    private static char[] mapChar (byte[] digits) {
		    char[] result = new char[digits.Length];
		
		    for (int i=0; i<digits.Length; i++)
			    result[i] = mapChar(digits[i]);
		
		    return result;
	    }
	
	    /**
	     * @param raw_message the identification message in hex representation
	     * @throws BadFormatException if message has the wrong typecode
	     */
	    public IdentificationMsg(String raw_message) : base(raw_message)
        {
		
		    if (FormatTypeCode < 1 || FormatTypeCode > 4) 
            {
			    throw new BadFormatException("Identification messages must have typecode of 1-4: " + raw_message);
		    }
		
		    byte[] msg = Message;
		    emitter_category = (byte) (msg[0] & 0x7);
		
		    // extract identity
		    identity = new byte[8];
		    int byte_off, bit_off;
		    for (int i=8; i>=1; i--) 
            {
			    // calculate offsets
			    byte_off = (i*6)/8; bit_off = (i*6)%8;
			
			    // char aligned with byte?
			    if (bit_off == 0) identity[i-1] = (byte) (msg[byte_off]&0x3F);
			    else {
				    ++byte_off;
				    identity[i-1] = (byte) (msg[byte_off]>>(8-bit_off)&(0x3F>>(6-bit_off)));
				    // should we add bits from the next byte?
				    if (bit_off < 6) identity[i-1] |= (byte)(msg[byte_off-1]<<bit_off&0x3F);
			    }
		    }
	    }

        public string getIdentity()
        {
            return new string(mapChar(identity));
        }

		public string getCategoryDescription () 
        {
		    // category descriptions according
		    // to the ADS-B specification
		    string[][] categories =  new string[][]
            {
                new string[] 
                {
			        "No ADS-B Emitter Category Information",
			        "Light (< 15500 lbs)",
			        "Small (15500 to 75000 lbs)",
			        "Large (75000 to 300000 lbs)",
			        "High Vortex Large (aircraft such as B-757)",
			        "Heavy (> 300000 lbs)",
			        "High Performance (> 5g acceleration and 400 kts)",
			        "Rotorcraft"
		        },
                new string[] 
                {
			        "No ADS-B Emitter Category Information",
			        "Glider / sailplane",
			        "Lighter-than-air",
			        "Parachutist / Skydiver",
			        "Ultralight / hang-glider / paraglider",
			        "Reserved",
			        "Unmanned Aerial Vehicle",
			        "Space / Trans-atmospheric vehicle",
		        },
                new string[] 
                {
			        "No ADS-B Emitter Category Information",
			        "Surface Vehicle – Emergency Vehicle",
			        "Surface Vehicle – Service Vehicle",
			        "Point Obstacle (includes tethered balloons)",
			        "Cluster Obstacle",
			        "Line Obstacle",
			        "Reserved",
			        "Reserved"
		        },
                new string[]
                {
			        "Reserved",
			        "Reserved",
			        "Reserved",
			        "Reserved",
			        "Reserved",
			        "Reserved",
			        "Reserved"
		        }
            };
		
		    return categories[4-FormatTypeCode][emitter_category];
	    }

	    public override string ToString() 
        {
		    return base.ToString()+"\n"+
				    "Identification:\n"+
				    "\tEmitter category:\t"+getCategoryDescription()+" ("+EmitterCategory+")\n"+
				    "\tCallsign:\t\t"+getIdentity();
	    }
    }
}
