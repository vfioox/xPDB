using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xPDB.Models.Systematization;
using xPDB.Storage;

namespace xPDB.Windows.Helpers
{
    public partial class FamilyTreeSelector : Form
    {
        protected ConfigManager cm;
        protected bool superfam = false;
        public FamilyTreeSelector(ref ConfigManager cm, bool superfam = false)
        {
            this.cm = cm;
            this.superfam = superfam;
            InitializeComponent();
        }

        private void FamilyTreeSelector_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            foreach (KeyValuePair<string, SuperfamilyDeclarator> sfd in cm.cfg.Superfamilies)
            {
                var tlist = new List<TreeNode>();
                foreach (var fd in cm.cfg.Families)
                {
                    if (fd.Value.Superfamily == sfd.Key)
                    {
                        tlist.Add(new TreeNode(fd.Key));
                    }
                }
                var sfnode = new TreeNode(sfd.Key, tlist.ToArray());
                treeView1.Nodes.Add(sfnode);
            }
        }

        public string selectedFamily = null;
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (superfam)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    selectedFamily = treeView1.SelectedNode.Text;
                    this.Close();
                }
            }
            else
            {
                if (treeView1.SelectedNode.Parent != null)
                {
                    selectedFamily = treeView1.SelectedNode.Text;
                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = new Families(ref cm);
            f.ShowDialog();
            cm.saveConfig();
            FamilyTreeSelector_Load(null, null);
        }
    }
}
