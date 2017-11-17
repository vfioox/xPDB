using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPDB.Models.Storage;

namespace xPDB.Models.Systematization
{
    public class SuperfamilyDeclarator
    {
        [JsonProperty("superfamily")]
        public string SuperFamily { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("file_type_key")]
        public string FileTypeKey { get; set; }
    }
}
