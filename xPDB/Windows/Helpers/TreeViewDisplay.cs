using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using xPDB.Models.Storage;
using xPDB.Storage;
using xPDB.Utility;

namespace xPDB.Windows.Helpers
{
    public partial class TreeViewDisplay : Form
    {
        internal object o;
        internal ConfigManager data;
        public TreeViewDisplay(object o, ref ConfigManager data)
        {
            this.o = o;
            InitializeComponent();
        }

        private void TreeViewDisplay_Load(object sender, EventArgs e)
        {
            textBox1.Text = JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        private void TreeViewDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            //data.cfg = JsonConvert.DeserializeObject<Config>(textBox1.Text);
            //data.saveConfig();
        }
    }
}
