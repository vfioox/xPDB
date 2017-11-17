using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPDB.Models.ExternalServices;

namespace xPDB.Network.ExternalServices
{
    class IPAPIOperator
    {
        public static IPAPI getIPQuery(string query)
        {
            const string format = "http://ip-api.com/json/{0}";
            return Net.DownloadObject<IPAPI>(string.Format(format, query));
        }
    }
}
