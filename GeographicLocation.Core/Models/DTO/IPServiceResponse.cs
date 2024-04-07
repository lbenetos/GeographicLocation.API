using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public class IPServiceResponse
    {
        public class Root
        {
            public Data? data { get; set; }
        }

        public class Data
        {
            public string? ip { get; set; }
            public Location? location { get; set; }
            public Timezone? timezone { get; set; }
        }

        public class Country
        {
            public string? alpha2 { get; set; }

            public string? name { get; set; }
        }

        public class Location
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public Country? country { get; set; }
        }

        public class Timezone
        {
            public string? id { get; set; }
        }
    }
}
