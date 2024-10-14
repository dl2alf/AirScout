using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScout
{

    [Serializable]
    public class MapProviders : List<MapProvider>
    {
        public static MapProviders GetAll()
        {
            MapProviders l = new MapProviders();

            l.Add(new MapProvider()
            {
                Name = "OpenStreetMap",
                URL = "https://tile.openstreetmap.org/{z}/{x}/{y}.png",
                Attribution = "Data © <a href=\"https://www.openstreetmap.org/\">OpenStreetMap</a> contributors"
            });

            l.Add(new MapProvider()
            {
                Name = "OpenTopoMap",
                URL = "https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png",
                Attribution = "Map ©  <a href=\"https://www.opentopomap.org/\">OpenStreetMap</a>, Data © <a href=\"https://www.openstreetmap.org/\">OpenStreetMap</a> contributors"
            });

            l.Add(new MapProvider()
            {
                Name = "OCM Landscape",
                URL = "https://{s}.tile.thunderforest.com/landscape/{z}/{x}/{y}.png?apikey={apikey}",
                Attribution = "Maps © <a href=\"https://www.thunderforest.com/\">Thunderforest</a>, Data © <a href=\"https://www.openstreetmap.org/\">OpenStreetMap</a> contributors",
                APIKey = "7d97ea4a50624393bb3e434e396d64bb"
            });

            return l;
        }

    }

    [Serializable]
    public class MapProvider
    {
        public string Name { get; set; } = "";
        public string URL { get; set; } = "";
        public string Attribution { get; set; } = "";

        public int MinZoom { get; set; } = 1;
        public int MaxZoom { get; set; } = 19;

        public string APIKey { get; set; } = null;

    }

}
