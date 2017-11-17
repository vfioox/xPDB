using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using xPDB.Utility;

namespace xPDB
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.AddMessageFilter(new KeyboardMessageFilter());
            var w = new MainWindow();
            if (args.Length > 0 && args[0].Contains("xpdb://"))
            {
                var startupArg = "";
                startupArg = args[0].Substring(6, args[0].Length - 6);
                startupArg = startupArg.Replace("/", "");
                w.startup_arg = startupArg;
            }
            Application.Run(w);

        }
    }
}
