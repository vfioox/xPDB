using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace xPDB.Utility
{
    public class UISnippets
    {
        public static string getFirstSelectedItem(ListBox c)
        {
            if (c.SelectedItems.Count == 0) return null;
            return c.SelectedItems[0].ToString();
        }
        public static string getFirstSelectedItem(ListView c)
        {
            if (c.SelectedItems.Count == 0) return null;
            return c.SelectedItems[0].Text;
        }
        public static void messageBoxWarning(string text, string title)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void DisplayObject(object obj, string title = "Object view")
        {
            MessageBox.Show(JsonConvert.SerializeObject(obj), title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
