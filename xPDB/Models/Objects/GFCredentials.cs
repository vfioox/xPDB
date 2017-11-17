using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xPDB.Models.Objects
{
    public class GFCredentials
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
