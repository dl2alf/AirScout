using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace LibADSB
{
    public class ExtendedSquitter : ModeSReply
    {

        [Browsable(false)]
        [DescriptionAttribute("Capability/Subtype of ES message")]
        public override byte Capabilities
        {
            get
            {
                return capabilities;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Message of ES message")]
        public byte[] Message
        {
            get
            {
                return message;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Format/Typecode of ES message")]
        public byte FormatTypeCode
        {
            get
            {
                return format_type_code;
            }
        }

	    private byte capabilities;
	    private byte[] message;
	    private byte format_type_code;
	
	    /**
	     * @param raw_message raw extended squitter as hex string
	     * @throws BadFormatException if message is not extended squitter or 
	     * contains wrong values.
	     */
	    public ExtendedSquitter(string raw_message) : base (raw_message)
        {
		
		    if (DownlinkFormat != 17 && DownlinkFormat != 18) 
            {
			    throw new BadFormatException("Message is not an extended squitter: " + raw_message);
		    }
		
		    byte[] payload = Payload;
		    capabilities = (byte) (payload[0] & 0x7);
		
		    // extract ADS-B message
		    message = new byte[7];
		    for (int i=0; i<7; i++)
			    message[i] = payload[i+3];
		
		    format_type_code = (byte) ((message[0] >> 3) & 0x1F);
	    }
	
	    public override string ToString() 
        {
		    return  base.ToString() + "\n" +
                    "Extended Squitter:\n"+
				    "\tFormat type code:\t"+FormatTypeCode+"\n"+
				    "\tCapabilities:\t\t"+Capabilities+"\n"+
				    "\tMessage field:\t\t"+BitConverter.ToString(Message).Replace("-",String.Empty);
	    }
    }
}
