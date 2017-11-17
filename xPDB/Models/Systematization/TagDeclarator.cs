using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPDB.Models.Systematization
{
    public class TagDeclarator
    {
        [JsonProperty("tag")]
        public string _Tag { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
        [JsonProperty("family")]
        public string Family { get; set; }
        [JsonProperty("nsfw")]
        public bool Nsfw { get; set; }
    }
}
