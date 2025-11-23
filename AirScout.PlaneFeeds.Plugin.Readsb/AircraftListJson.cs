// Copyright © 2025, DL2ALF
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
    /// The list of aircraft that is sent to the browser as a JSON file.
    /// </summary>
    [DataContract]
    public class AircraftListJson
    {
        /// <summary>
        /// Gets the time of generation of this file in UNIX nanoseconds
        /// </summary>
        [DataMember(Name = "now", IsRequired = true)]
        public double? Now { get; private set; }

        /// <summary>
        /// Gets the list of aircraft to show to the user.
        /// </summary>
        [DataMember(Name = "aircraft", IsRequired = true)]
        public List<AircraftJson> Aircraft { get; private set; }

        /// <summary>
        /// Gets or sets a global message from the server
        /// </summary>
        [DataMember(Name = "messages")]
        public string Messages { get; set; }

        /// <summary>
        /// Creates a new object.
        /// </summary>
        public AircraftListJson()
        {
            Aircraft = new List<AircraftJson>();
        }
    }
}
