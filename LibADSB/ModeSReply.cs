using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

// LibADSB project is based on the java-adsb project (https://github.com/openskynetwork/java-adsb)   

namespace LibADSB
{
    public class ModeSReply
    {

        // Public properties

        [Browsable(false)]
        [DescriptionAttribute("Downlink format of ADS-B message (0..31)")]
        public byte DownlinkFormat
        {
            get
            {
                return downlink_format;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Capability/Subtype of ADS-B message (0..7)")]
        public virtual byte Capabilities
        {
            get
            {
                return capabilities;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("ICAO number of aircraft")]
        public byte[] ICAO24
        {
            get
            {
                return icao24;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Payload of ADS-B message")]
        public byte[] Payload
        {
            get
            {
                return payload;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Parity of ADS-B message")]
        public byte[] Parity
        {
            get
            {
                return parity;
            }
        }

        [Browsable(false)]
        [DescriptionAttribute("Recalculated parity of ADS-B message")]
        public byte[] RecalculatedParity
        {
            get
            {
                byte[] message = new byte[payload.Length + 1];

                message[0] = (byte)(downlink_format << 3 | capabilities);
                for (byte b = 0; b < payload.Length; ++b)
                    message[b + 1] = payload[b];

                return calcParity(message);
            }
        }

        [DescriptionAttribute("Parity check of ADS-B message")]
        public bool CheckParity
        {
            get
            {
                return RecalculatedParity.SequenceEqual(Parity);
            }
        }

        // Private properties
        private byte downlink_format; // 0-31
	    private byte capabilities; // three bits after the downlink format
	    private byte[] icao24; // 3 bytes
	    private byte[] payload; // 3 or 10 bytes
        private byte[] parity; // 3 bytes

	     // Static fields and functions

	     // polynomial for the cyclic redundancy check<br />
	     // Note: we assume that the degree of the polynomial
	     // is divisible by 8 (holds for Mode S) and the msb is left out
	     
	    public static byte[] CRC_polynomial = 
        {
		    (byte)0xFF,
		    (byte)0xF4,
		    (byte)0x09 // according to Annex 10 V4
	    };

	     // @return calculated parity field as 3-byte array. We used the implementation from<br />
	     //         http://www.eurocontrol.int/eec/gallery/content/public/document/eec/report/1994/022_CRC_calculations_for_Mode_S.pdf

        public static byte[] calcParity (byte[] msg) 
        {
		    byte[] pi = new byte[CRC_polynomial.Length];
            Array.Copy(msg, 0, pi, 0, CRC_polynomial.Length);

		    bool invert;
		    int byteidx, bitshift;
		    for (int i = 0; i < msg.Length*8; ++i) 
            {
			    invert = ((pi[0] & 0x80) != 0);
			    // shift left
			    pi[0] <<= 1;
			    for (int b = 1; b < CRC_polynomial.Length; ++b) 
                {
				    pi[b-1] |= (byte) ((pi[b] >> 7) & 0x1);
				    pi[b] <<= 1;
			    }

			    // get next bit from message
			    byteidx = ((CRC_polynomial.Length*8)+i) / 8;
			    bitshift = 7-(i%8);
			    if (byteidx < msg.Length)
				    pi[pi.Length-1] |= (byte) ((msg[byteidx] >> bitshift) & 0x1);

			    // xor
			    if (invert)
                {
				    for (int b = 0; b < CRC_polynomial.Length; ++b)
					    pi[b] ^= CRC_polynomial[b];
                }
		    }
            byte[] result = new byte[CRC_polynomial.Length];
		    Array.Copy(pi, 0, result, 0, CRC_polynomial.Length);
            return result;
	    }

	     // Constructors

	    /**
	     * We assume the following message format:
	     * | DF | CA | Payload | PI/AP |
	     *   5    3    3/10      3
	     * 
	     * @param raw_message Mode S message in hex representation
	     * @throws BadFormatException if message has invalid length or payload does
	     * not match specification or parity has invalid length
	     */
	    public ModeSReply (string raw_message)
        {
		    // check format invariants
		    int length = raw_message.Length;
		    if (length != 14 && length != 28)
			    throw new BadFormatException("Raw message has invalid length: " + raw_message);

		    downlink_format = System.Convert.ToByte(raw_message.Substring(0, 2), 16);
		    capabilities = (byte) (downlink_format & 0x7);
		    downlink_format = (byte) (downlink_format >> 3 & 0x1F);

		    byte[] payload = new byte[(length-8)/2];
		    byte[] icao24 = new byte[3];
		    byte[] parity = new byte[3];

		    // decode based on format
		    // TODO
		    switch (downlink_format) 
            {
		        case 0: // Short air-air (ACAS)
		        case 4: // Short altitude reply
		        case 5: // Short identity reply
		        case 16: // Long air-air (ACAS)
		        case 20: // Long Comm-B, altitude reply
		        case 21: // Long Comm-B, identity reply
		        case 24: // Long Comm-D (ELM)
			        // here we assume that AP is already the icao24
			        // i.e. parity is extracted. Therefore we leave
			        // parity 0
			        for (int i=length-6; i<length; i+=2)
				        icao24[(i-length+6)/2] = System.Convert.ToByte(raw_message.Substring(i, 2),16);

			        // extract payload (little endian)
			        for (int i=2; i<length-6; i+=2)
				        payload[(i-2)/2] = System.Convert.ToByte(raw_message.Substring(i, 2),16);
			        break;
    		    case 11: // Short all-call reply
	    	    case 17: case 18: // Extended squitter
			        // extract payload (little endian)
			        for (int i=2; i<length-6; i+=2)
                    {
                        try
                        {
                            string c = raw_message.Substring(i, 2);
                            payload[(i - 2) / 2] = System.Convert.ToByte(c,16);
                        }
                        catch (Exception ex)
                        {
                        }

                    }
			        for (int i=0; i<3; i++)
				        icao24[i] = payload[i];
			        for (int i=length-6; i<length; i+=2)
				        parity[(i-length+6)/2] = System.Convert.ToByte(raw_message.Substring(i, 2),16);
			        break;
    		    default: // unkown downlink format
	    		    // leave everything 0
                    break;
		    }

		    // check format invariants
		    if (icao24.Length != 3)
			    throw new BadFormatException("ICAO address too short/long: " + raw_message);
		    if (payload.Length != 3 && payload.Length != 10)
			    throw new BadFormatException("Payload length does not match specification: " + raw_message);
		    if (parity.Length != 3)
			    throw new BadFormatException("Parity too short/long: " + raw_message);

		    this.icao24 = icao24;
		    this.payload = payload;
		    this.parity = parity;
	    }

        public bool IsZeroParity()
        {
            for (int i = 0; i < parity.Length; i++)
            {
                if (parity[i] != 0)
                    return false;    
            }
            return true;
        }

	    public override string ToString() 
        {
		    return  "Mode S Reply:\n"+
				    "\tDownlink format:\t"+DownlinkFormat+"\n"+
				    "\tCapabilities:\t\t"+Capabilities+"\n"+
				    "\tICAO 24-bit address:\t"+BitConverter.ToString(ICAO24).Replace("-",String.Empty)+"\n"+
				    "\tPayload:\t\t"+BitConverter.ToString(Payload).Replace("-",String.Empty)+"\n"+
                    "\tParity:\t\t\t" + BitConverter.ToString(Parity).Replace("-",String.Empty) + "\n" +
                    "\tCalculated Parity:\t" + BitConverter.ToString(RecalculatedParity).Replace("-",String.Empty);
	    }
    }
}