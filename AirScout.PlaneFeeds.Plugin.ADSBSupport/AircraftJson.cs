// Copyright © 2010 onwards, Andrew Whewell
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

namespace ADSBSupportServer.Interface.WebSite
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
        [DataMember(Name = "hex", IsRequired = false, EmitDefaultValue = false)]
        public string Hex { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's callsign.
        /// </summary>
        [DataMember(Name = "callsign", IsRequired = false, EmitDefaultValue = false)]
        public string Callsign { get; set; }


        /// <summary>
        /// Gets or sets the aircraft's pressure altitude in feet.
        /// </summary>
        [DataMember(Name = "altitude_barometric", IsRequired = false, EmitDefaultValue = false)]
        public int? BarometricAltitude { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's geometric altitude in feet.
        /// </summary>
        [DataMember(Name = "altitude_gps", IsRequired = false, EmitDefaultValue = false)]
        public int? GeometricAltitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the aircraft.
        /// </summary>
        [DataMember(Name = "latitude", IsRequired = false, EmitDefaultValue = false)]
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the aircraft's longitude.
        /// </summary>
        [DataMember(Name = "longitude", IsRequired = false, EmitDefaultValue = false)]
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the time that the <see cref="Latitude"/> and <see cref="Longitude"/> were
        /// transmitted as a number of .NET ticks.
        /// </summary>
        [DataMember(Name = "receivedAt", IsRequired = false, EmitDefaultValue = false)]
        public long? ReceivedAt { get; set; }

        /// <summary>
        /// Gets or sets the heading that the aircraft is tracking across the ground in degrees from 0° north.
        /// </summary>
        [DataMember(Name = "angle", IsRequired = false, EmitDefaultValue = false)]
        public float? Angle { get; set; }

        /// <summary>
        /// Gets or sets the ground speed of the aircraft in knots.
        /// </summary>
        [DataMember(Name = "speed", IsRequired = false, EmitDefaultValue = false)]
        public float? Speed { get; set; }

        /// <summary>
        /// Gets or sets the squawk currently transmitted by the aircraft.
        /// </summary>
        [DataMember(Name = "squawk", IsRequired = false, EmitDefaultValue = false)]
        public string Squawk { get; set; }

        /// <summary>
        /// Gets or sets the vertical speed in feet per second.
        /// </summary>
        [DataMember(Name = "verticalRate", IsRequired = false, EmitDefaultValue = false)]
        public int? VerticalRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the wake turbulence category of the aircraft (see <see cref="WakeTurbulenceCategory"/>).
        /// </summary>
        [DataMember(Name = "category", IsRequired = false, EmitDefaultValue = false)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the aircraft is on the ground.
        /// </summary>
        [DataMember(Name = "onGround", IsRequired = false, EmitDefaultValue = false)]
        public bool? OnGround { get; set; }
    }
}
