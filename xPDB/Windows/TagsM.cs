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
using xPDB.Utility;
using xPDB.Windows.Helpers;

namespace xPDB.Windows
{
    public partial class TagsM : Form
    {
        protected ConfigManager cm;
        protected Dictionary<string, TagDeclarator> temporaryChanges = new Dictionary<string, TagDeclarator>();
        public TagsM(ref ConfigManager cm)
        {
            this.cm = cm;
            InitializeComponent();
        }
        
        private void Superfamilies_Load(object sender, EventArgs e)
        {
            refreshTags();
        }
        private void refreshTags()
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (KeyValuePair<string, TagDeclarator> sd in cm.cfg.Tags)
            {
                listView1.Items.Add(sd.Key);
            }
            foreach (KeyValuePair<string, TagDeclarator> sd in temporaryChanges)
            {
                listView1.Items.Add(sd.Key);
            }
            listView1.EndUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, TagDeclarator> sd in temporaryChanges)
            {
                cm.cfg.Tags.Add(sd.Key, sd.Value);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TagDeclarator td = new TagDeclarator();
            td._Tag = textBox1.Text;
            td.Family = cm.getFamilyDeclarator(button6.Text).Family;
            td.Description = textBox2.Text;

            if (!temporaryChanges.ContainsKey(td._Tag)) temporaryChanges.Add(td._Tag, td);
            else UISnippets.messageBoxWarning("Tag with that key already exists", "Key exists");
            refreshTags();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void editMode()
        {
            button4.Visible = true;
            button5.Visible = true;
            textBox1.Enabled = false;
            button6.Enabled = false;
            groupBox1.Text = "Edit mode: Tag " + textBox1.Text;
            button4.Text = "Save";
        }
        private void exitEditMode()
        {
            textBox1.Enabled = true;
            textBox1.Text = "";
            button4.Text = "Save";
            textBox2.Text = "";
            button6.Enabled = true;
            groupBox1.Text = "Adding new tag";
            button4.Visible = false;
            button6.Text = "Open selector";
            button5.Visible = false;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                string key = UISnippets.getFirstSelectedItem(listView1);
                TagDeclarator td;
                if (cm.doesTagExist(key))
                {
                    td = cm.getTagDeclarator(key);
                }
                else
                {
                    td = temporaryChanges[key];
                }
                textBox1.Text = td._Tag;
                textBox2.Text = td.Description;
                button6.Text = td.Family;
                editMode();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cm.doesTagExist(textBox1.Text))
            {
                cm.cfg.Families[textBox1.Text].Description = textBox2.Text;
                button4.Text = "Saved";
            }
            else
            {
                temporaryChanges[textBox1.Text].Description = textBox2.Text;
                button4.Text = "Saved temp";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            exitEditMode();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FamilyTreeSelector fts = new FamilyTreeSelector(ref cm);
            fts.ShowDialog();
            var selected = fts.selectedFamily;
            fts.Dispose();
            button6.Text = selected;
        }
    }
}
