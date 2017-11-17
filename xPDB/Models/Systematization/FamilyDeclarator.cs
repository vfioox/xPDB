using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPDB.Models.Systematization
{
    public class FamilyDeclarator
    {
        [JsonProperty("family")]
        public string Family { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("superfamily")]
        public string Superfamily { get; set; }
    }
}
