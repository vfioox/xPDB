using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xPDB.Models.ExternalServices;
using xPDB.Network.ExternalServices;

namespace xPDB.Windows.Tools
{
    public partial class IPLookup : Form
    {
        public IPLookup()
        {
            InitializeComponent();
        }

        private void IPLookup_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
        protected IPAPI ipapi;
        private void displayProperties()
        {
            groupBox1.Controls.Clear();
            int cur_y = 20;
            int cur_x = 7;

            if (ipapi != null)
            {
                foreach (PropertyInfo propertyInfo in ipapi.GetType().GetProperties())
                {
                    Label n = new Label();
                    n.Location = new Point(cur_x, cur_y);
                    n.Text = propertyInfo.Name + "    " + propertyInfo.GetValue(ipapi, null);
                    groupBox1.Controls.Add(n);
                    cur_y = cur_y + n.Height;
                }
            }
            else
            {
                Label n = new Label();
                n.Location = new Point(cur_x, cur_y);
                n.Text = "Failure, no server response";
                groupBox1.Controls.Add(n);
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                e.Handled = true;
                textBox1.Enabled = false;
                Task.Run(() =>
                {
                    ipapi = IPAPIOperator.getIPQuery(textBox1.Text);
                }).ContinueWith(t => {
                    this.Invoke((MethodInvoker)(() => {
                        textBox1.Enabled = true;
                        displayProperties();
                        textBox1.Focus();
                    }));
                });
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
