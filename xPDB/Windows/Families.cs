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

namespace xPDB.Windows
{
    public partial class Families : Form
    {
        protected ConfigManager cm;
        protected Dictionary<string, FamilyDeclarator> temporaryChanges = new Dictionary<string, FamilyDeclarator>();
        public Families(ref ConfigManager cm)
        {
            this.cm = cm;
            InitializeComponent();
        }
        
        private void Superfamilies_Load(object sender, EventArgs e)
        {
            refreshFamilies();
            refreshSuperfamilies();
        }
        private void refreshSuperfamilies()
        {
            foreach (var key in cm.cfg.Superfamilies.Keys)
            {
                comboBox1.Items.Add(key);
            }
        }
        private void refreshFamilies()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach (KeyValuePair<string, FamilyDeclarator> sd in cm.cfg.Families)
            {
                listBox1.Items.Add(sd.Key);
            }
            foreach (KeyValuePair<string, FamilyDeclarator> sd in temporaryChanges)
            {
                listBox1.Items.Add(sd.Key);
            }
            listBox1.EndUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, FamilyDeclarator> sd in temporaryChanges)
            {
                cm.cfg.Families.Add(sd.Key, sd.Value);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FamilyDeclarator fd = new FamilyDeclarator();
            fd.Family = textBox1.Text;
            fd.Superfamily = cm.getSuperfamilyDeclarator(comboBox1.Text).SuperFamily;
            fd.Description = textBox2.Text;

            if (!temporaryChanges.ContainsKey(fd.Family)) temporaryChanges.Add(fd.Family, fd);
            else UISnippets.messageBoxWarning("Family with that key already exists", "Key exists");
            refreshFamilies();
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
            comboBox1.Enabled = false;
            groupBox1.Text = "Edit mode: Family " + textBox1.Text;
            button4.Text = "Save";
        }
        private void exitEditMode()
        {
            textBox1.Enabled = true;
            textBox1.Text = "";
            button4.Text = "Save";
            comboBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Enabled = true;
            groupBox1.Text = "Adding new family";
            button4.Visible = false;
            button5.Visible = false;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                string key = UISnippets.getFirstSelectedItem(listBox1);
                FamilyDeclarator fd;
                if (cm.doesFamilyExist(key))
                {
                    fd = cm.getFamilyDeclarator(key);
                }
                else
                {
                    fd = temporaryChanges[key];
                }
                textBox1.Text = fd.Family;
                textBox2.Text = fd.Description;
                comboBox1.Text = fd.Superfamily;
                editMode();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cm.doesFamilyExist(textBox1.Text))
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
    }
}
