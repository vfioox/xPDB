using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xPDB.Models.Storage
{
    public class FileDeclarator
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
        [JsonProperty("chunk_key")]
        public string ChunkKey { get; set; }
        [JsonProperty("file_type_key")]
        public string FileTypeKey { get; set; }
        [JsonProperty("original_path")]
        public string OriginalPath { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("tag_keys")]
        public List<string> TagKeys { get; set; }
        [JsonProperty("superfamily_key")]
        public string SuperfamilyKey { get; set; }
    }
}
