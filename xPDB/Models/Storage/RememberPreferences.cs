using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using xPDB.Storage;

namespace xPDB.Models.Storage
{
    public class RememberPreferences
    {
        [JsonProperty("read_method")]
        public ReadMethods ReadMethod { get; set; }
        [JsonProperty("write_method")]
        public WriteMethods WriteMethod { get; set; }

        [JsonProperty("analyze_during_aquisition")]
        public bool AnalyzeDuringAquisition { get; set; }
        [JsonProperty("visual_aquisition")]
        public bool VisualAquisition { get; set; }
    }
}
