using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using xPDB.Models.Systematization;

namespace xPDB.Models.Storage
{
    public class Config
    {
        [JsonProperty("last_launch")]
        public int LastLaunch { get; set; }
        [JsonProperty("chunk_path")]
        public string ChunkPath { get; set; }
        [JsonProperty("chunk_size")]
        public int ChunkSize { get; set; }
        [JsonProperty("no_of_launches")]
        public int NoOfLaunches { get; set; }
        [JsonProperty("use_encryption")]
        public bool UseEncryption { get; set; }
        [JsonProperty("log_password")]
        public string LogPassword { get; set; }

        [JsonProperty("mandatory_recalculation_scheduled")]
        public bool MRS { get; set; }

        [JsonProperty("instance")]
        public string Instance { get; set; }
        [JsonProperty("instance_chunk_dir")]
        public string InstanceChunkDir { get; set; }

        [JsonProperty("chunks")]
        public Dictionary<string, Chunk> Chunks { get; set; }
        [JsonProperty("file_declarators")]
        public Dictionary<string, FileDeclarator> FileDeclarators { get; set; }

        [JsonProperty("deletion_queue")]
        public Dictionary<string, FileDeclarator> DeletionQueue { get; set; }

        [JsonProperty("file_types")]
        public Dictionary<string, FileType> FileTypes { get; set; }
        [JsonProperty("superfamilies")]
        public Dictionary<string, SuperfamilyDeclarator> Superfamilies { get; set; }
        [JsonProperty("families")]
        public Dictionary<string, FamilyDeclarator> Families { get; set; }
        [JsonProperty("tags")]
        public Dictionary<string, TagDeclarator> Tags { get; set; }

        [JsonProperty("object_storage")]
        public Dictionary<string, object> OStorage { get; set; }


        [JsonProperty("preferences")]
        public RememberPreferences Preferences { get; set; }

    }
}
