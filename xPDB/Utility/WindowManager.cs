using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xPDB.Utility
{
    public class WindowManager
    {
        public Dictionary<string, Form> helpers = new Dictionary<string, Form>();
        public Form putWindow(string name, Form window)
        {
            if (helpers.ContainsKey(name))
            {
                if (helpers[name].GetType() == window.GetType())
                {
                    helpers[name] = window;
                }
                else
                {
                    helpers.Remove(name);
                }
            }
            else
            {
                helpers.Add(name, window);
            }
            return window;
        }
        public T getWindow<T>(string name) where T : Form // AWESOME!
        {
            if (!helpers.ContainsKey(name))
            {
                throw new Exception("Tried to open a helper window before creating it");
            }
            return (T)helpers[name];
        }
        public void destroyWindow(string name)
        {
            if (helpers.ContainsKey(name))
            {
                helpers[name].Dispose();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
