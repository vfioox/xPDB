using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xPDB.Models.Objects
{
    public class DACredentials
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("secret")]
        public string Secret { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
