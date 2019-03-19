using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibADSB
{
    // public class for storing subsequent position messages
    public class AircraftPositions
    {
        public AirbornePositionMsg last_even_airborne;
        public AirbornePositionMsg last_odd_airborne;
        public SurfacePositionMsg last_even_surface;
        public SurfacePositionMsg last_odd_surface;
        public double[] last_position; // lat lon
        public bool supplA;
    }
    
    /**
     * General decoder for ADS-B messages
     * @author Matthias Schäfer <schaefer@sero-systems.de>
     */
    public class Decoder
    {

        /**
         * A top-down ADS-B decoder. Use instanceof to check for the message type.
         * @param raw_message The Mode S message in hex representation
         */
        public static ModeSReply GenericDecoder(String raw_message)
        {
            ModeSReply modes = new ModeSReply(raw_message);
            // check parity; Note: some receivers set parity to 0
            if (modes.IsZeroParity() || modes.CheckParity)
            {
                // check if it is an ADS-B message
                if (modes.DownlinkFormat == 17 || modes.DownlinkFormat == 18) 
                {
                    ExtendedSquitter es1090 = new ExtendedSquitter(raw_message);

                    // what kind of extended squitter?
                    byte ftc = es1090.FormatTypeCode;
                    if (ftc >= 1 && ftc <= 4) // identification message
                        return new IdentificationMsg(raw_message);
                    if (ftc >= 5 && ftc <= 8) // surface position message
                        return new SurfacePositionMsg(raw_message);
                    if ((ftc >= 9 && ftc <= 18) || (ftc >= 20 && ftc <= 22)) // airborne position message
                        return new AirbornePositionMsg(raw_message);
                    if (ftc == 19) { // possible velocity message, check subtype
                        int subtype = es1090.Message[0]&0x7;

                       if (subtype == 1 || subtype == 2) // velocity over ground
                            return new VelocityOverGroundMsg(raw_message);
                        else if (subtype == 3 || subtype == 4) // airspeed & heading
                            return new AirspeedHeadingMsg(raw_message);
                    }
                    /*
                    if (ftc == 28) { // aircraft status message, check subtype
                        int subtype = es1090.getMessage()[0]&0x7;

                        if (subtype == 1) // emergency/priority status
                            return new EmergencyOrPriorityStatusMsg(raw_message);
                        if (subtype == 2) // TCAS resolution advisory report
                            return new TCASResolutionAdvisoryMsg(raw_message);
                    }
				
                    if (ftc == 31) { // operational status message
                        int subtype = es1090.getMessage()[0]&0x7;

                        if (subtype == 0 || subtype == 1) // airborne or surface?
                            return new OperationalStatusMsg(raw_message);
                    }
                    */
                    return es1090; // unknown extended squitter
                }
            }
            return modes; // unknown mode s reply
        }
    }
}