using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public class IPAddressDTO
    {
        public string IP { get; set; } = null!;

        public string? CountryCode { get; set; }

        public string? CountryName { get; set; }

        public string? TimeZone { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
