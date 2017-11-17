using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPDB.Models.ExternalServices
{
    public class IPAPI
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("regionName")]
        public string RegionName { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("zap")]
        public string Zap { get; set; }
        [JsonProperty("lat")]
        public string Lat { get; set; }
        [JsonProperty("lon")]
        public string Lon { get; set; }
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        [JsonProperty("isp")]
        public string Isp { get; set; }
        [JsonProperty("Org")]
        public string Org { get; set; }
        [JsonProperty("as")]
        public string As { get; set; }
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}
