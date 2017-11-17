using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xPDB.Models.Storage
{
    public class Chunk
    {
        [JsonProperty("chunk_id")]
        public string ChunkId { get; set; }
        [JsonProperty("file_codes")]
        public Dictionary<string, int> FileCodes { get; set; }
        [JsonProperty("file_offsets")]
        public Dictionary<string, int> FileOffsets { get; set; }
        [JsonProperty("file_lengths")]
        public Dictionary<string, int> FileLenghts { get; set; }
        [JsonProperty("vacant")]
        public bool Vacant { get; set; }
        [JsonProperty("possibly_corrupted")]
        public bool PossiblyCorrupted { get; set; }
        [JsonProperty("corrupted")]
        public bool Corrupted { get; set; }
    }
}
