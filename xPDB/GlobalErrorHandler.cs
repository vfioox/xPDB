using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xPDB
{
    class GlobalErrorHandler
    {
        public static void logError(Exception ex, string password)
        {
            throw ex;
        }
    }
}
