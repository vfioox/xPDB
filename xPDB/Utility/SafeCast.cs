using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xPDB.Utility
{
    public static class SafeCast
    {
        public static T Cast<T>(object subject)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(subject));
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
