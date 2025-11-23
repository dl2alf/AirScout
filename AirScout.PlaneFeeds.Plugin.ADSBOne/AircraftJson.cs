// Copyright © 20125, DL2ALF
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtualRadar.Interface.WebSite
{
    /// <summary>
    /// The object that describes an aircraft in JSON files that are sent to the browser.
    /// </summary>
    [DataContract]
    public class AircraftJson
    {
        /// <summary>
        /// Gets or sets the 24-bit Mode-S identifier of the aircraft.
        /// </summary>
        [DataMember(Name = "hex", IsRequired = true, EmitDefaultValue = false)]
        public string Hex { get; set; }

        /// <summary>
        /// Gets or sets the message type code of the transmission.
        /// </summary>
        [DataMember(Name = "type", IsRequired = false, EmitDefaultValue = false)]
        public string MsgType { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's callsign.
        /// </summary>
        [DataMember(Name = "flight", IsRequired = false, EmitDefaultValue = false)]
        public string Callsign { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's registration.
        /// </summary>
        [DataMember(Name = "r", IsRequired = false, EmitDefaultValue = false)]
        public string Registration { get; set; }

        /// <summary>
        /// Gets or sets the ICAO8643 type code of the aircraft.
        /// </summary>
        [DataMember(Name = "t", IsRequired = false, EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the English description of the aircraft model. Usually includes the manufacturer.
        /// </summary>
        [DataMember(Name = "desc", IsRequired = false, EmitDefaultValue = false)]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the operator's name.
        /// </summary>
        [DataMember(Name = "Op", IsRequired = false, EmitDefaultValue = false)]
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the year of construction
        /// </summary>
        [DataMember(Name = "year", IsRequired = false, EmitDefaultValue = false)]
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's pressure altitude in feet. Can be "ground" as well!
        /// </summary>
        [DataMember(Name = "alt_baro", IsRequired = false, EmitDefaultValue = false)]
        public string Altitude { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's ground speed in knots.
        /// </summary>
        [DataMember(Name = "gs", IsRequired = false, EmitDefaultValue = false)]
        public double? GroundSpeed { get; set; }

        /// <summary>
        /// Gets or sets the heading that the aircraft is tracking across the ground in degrees from 0° north.
        /// </summary>
        [DataMember(Name = "track", IsRequired = false, EmitDefaultValue = false)]
        public double? Track { get; set; }

        /// <summary>
        /// Gets or sets the squawk currently transmitted by the aircraft.
        /// </summary>
        [DataMember(Name = "squawk", IsRequired = false, EmitDefaultValue = false)]
        public string Squawk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the wake turbulence category of the aircraft (see <see cref="WakeTurbulenceCategory"/>).
        /// </summary>
        [DataMember(Name = "category", IsRequired = false, EmitDefaultValue = false)]
        public string WakeTurbulenceCategory { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the aircraft.
        /// </summary>
        [DataMember(Name = "lat", IsRequired = false, EmitDefaultValue = false)]
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's longitude.
        /// </summary>
        [DataMember(Name = "lon", IsRequired = false, EmitDefaultValue = false)]
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's Navigation Integrity Category
        /// 
        [DataMember(Name = "nic", IsRequired = false, EmitDefaultValue = false)]
        public double? NIC { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's Radius of Containment, meters; a measure of position integrity derived from NIC & supplementary bits.
        /// </summary>
        [DataMember(Name = "rc", IsRequired = false, EmitDefaultValue = false)]
        public double? RC { get; set; }


        /// <summary>
        /// Gets or sets how long ago (in seconds before “now”) the position was last updated
        /// </summary>
        [DataMember(Name = "seen_pos", IsRequired = false, EmitDefaultValue = false)]
        public double? SeenPosition { get; set; }

    }


}
