using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xPDB.Windows.Helpers
{
    public partial class ImageView : Form
    {
        public ImageView(Image x)
        {
            InitializeComponent();
            pictureBox1.Image = x;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
