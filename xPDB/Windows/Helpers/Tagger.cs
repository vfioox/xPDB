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
    public partial class Tagger : Form
    {
        private ConfigManager cm;
        public List<string> tagsChosen = new List<string>();
        public List<string> fileCodes;
        public Tagger(ref ConfigManager cm, List<string> fileCodes)
        {
            this.cm = cm;
            this.fileCodes = fileCodes;
            InitializeComponent();
        }

        private void Tagger_Load(object sender, EventArgs e)
        {
            sfmOListView.FullRowSelect = true;
            sfmOListView.ShowGroups = false;
            var sfd = new List<SuperfamilyDeclarator>();
            foreach (var superfam in cm.cfg.Superfamilies)
            {
                sfd.Add(superfam.Value);
            }
            sfmOListView.SetObjects(sfd);
        }

        private void sfmOListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sfmOListView.SelectedItems.Count == 1)
            {
                var fkd = new Dictionary<string, ListViewGroup>();
                var i = 0;
                tagsView.Items.Clear();
                tagsView.Groups.Clear();
                tagsView.ShowGroups = true;
                tagsView.CheckBoxes = true;
                foreach (var fam in cm.cfg.Families)
                {
                    if (fam.Value.Superfamily == sfmOListView.SelectedItem.Text)
                    {
                        var lvg = new ListViewGroup(fam.Value.Family);
                        lvg.Header = fam.Value.Family;
                        tagsView.Groups.Add(lvg);
                        fkd.Add(fam.Key, lvg);
                        i++;
                    }
                }
                foreach (var item in cm.cfg.Tags)
                {
                    if (fkd.ContainsKey(item.Value.Family))
                    {
                        var row = new ListViewItem();
                        row.Text = item.Value._Tag;
                        foreach (var f in fileCodes)
                        {
                            if (cm.cfg.FileDeclarators[f].TagKeys.Contains(row.Text) && fileCodes.Count == 1)
                            {
                                if (!tagsChosen.Contains(row.Text))
                                {
                                    tagsChosen.Add(row.Text);
                                }
                            }
                        }
                        if (tagsChosen.Contains(row.Text))
                        {
                            row.Checked = true;
                        }
                        row.ToolTipText = item.Value.Description;
                        row.Group = fkd[item.Value.Family];
                        tagsView.Items.Add(row);
                    }
                }
                label2.Text = String.Join(", ", tagsChosen);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            tagsChosen.Clear();
            label2.Text = String.Join(", ", tagsChosen);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var tm = new TagsM(ref cm);
            var dr = tm.ShowDialog();
            if (dr != DialogResult.Cancel)
            {
                sfmOListView_SelectedIndexChanged(null, null);
                cm.saveConfig();
            }
        }

        private void tagsView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void tagsView_Click(object sender, EventArgs e)
        {
        }

        private void tagsView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

            foreach (var item in tagsView.CheckedItems)
            {
                var tag = ((ListViewItem)item).Text;
                if (!tagsChosen.Contains(tag))
                {
                    tagsChosen.Add(tag);
                }
            }
            foreach (var item in tagsView.Items)
            {
                var row = ((ListViewItem)item);
                if (tagsChosen.Contains(row.Text))
                {
                    if (tagsChosen.Contains(row.Text) && !row.Checked)
                    {
                        tagsChosen.Remove(row.Text);
                    }
                }
            }
            label2.Text = String.Join(", ", tagsChosen);
        }
    }
}
