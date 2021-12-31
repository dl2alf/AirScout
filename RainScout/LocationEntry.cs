using ScoutBase.Propagation;
using ScoutBase.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RainScout
{
    [Serializable]
    public class LocationEntry
    {
        public LocationDesignator Location { get; set; } = new LocationDesignator();
        public QRVDesignator QRV { get; set; } = new QRVDesignator();
        public PropagationHorizonDesignator Horizon { get; set; } = new PropagationHorizonDesignator();
    }
}
