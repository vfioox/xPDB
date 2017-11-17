using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPDB.Models.Storage
{
    public class FileType
    {
        [JsonProperty("user_custom")]
        public bool UserCustom { get; set; }
        [JsonProperty("extensions")]
        public List<string> Extensions { get; set; }
        [JsonProperty("file_type")]
        public FileTypes _FileType { get; set; }
        [JsonProperty("type_name")]
        public string TypeName { get; set; }
        [JsonProperty("type_key")]
        public string TypeKey { get; set; }
    }
}
