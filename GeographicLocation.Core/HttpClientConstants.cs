using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public static class HttpClientConstants
    {
        public const string GeoLocationApiUrl = "https://api.ipbase.com/v2/info";

        public const string ApiKey = "ipb_live_d3MZROuWO4Ws4FETvSm4pDNm9E1c69B207FZd7Gu";

        public const string FailedToCreateBatchJobMessage = "Failed to create batch job";

        public const string InvalidIP = "Invalid IP Addrress";

        public const string InvalidListIP = "Invalid List of IP Addresses";
    }
}
