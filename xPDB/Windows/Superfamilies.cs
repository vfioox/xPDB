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
    public partial class Superfamilies : Form
    {
        public ConfigManager cm;
        public Dictionary<string, SuperfamilyDeclarator> temporaryChanges = new Dictionary<string, SuperfamilyDeclarator>();
        public Superfamilies(ref ConfigManager cm)
        {
            this.cm = cm;
            InitializeComponent();
        }
        
        private void Superfamilies_Load(object sender, EventArgs e)
        {
            refreshFileTypes();
            refreshSuperfamilies();
        }
        private void refreshFileTypes()
        {
            foreach (string key in cm.cfg.FileTypes.Keys)
            {
                comboBox1.Items.Add(key);
            }
        }
        private void refreshSuperfamilies()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach (KeyValuePair<string, SuperfamilyDeclarator> sd in cm.cfg.Superfamilies)
            {
                listBox1.Items.Add(sd.Key);
            }
            foreach (KeyValuePair<string, SuperfamilyDeclarator> sd in temporaryChanges)
            {
                listBox1.Items.Add(sd.Key);
            }
            listBox1.EndUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, SuperfamilyDeclarator> sd in temporaryChanges)
            {
                cm.cfg.Superfamilies.Add(sd.Key, sd.Value);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SuperfamilyDeclarator sfd = new SuperfamilyDeclarator();
            sfd.SuperFamily = textBox1.Text;
            sfd.FileTypeKey = comboBox1.Text;
            sfd.Description = textBox2.Text;

            if (!temporaryChanges.ContainsKey(sfd.SuperFamily)) temporaryChanges.Add(sfd.SuperFamily, sfd);
            else UISnippets.messageBoxWarning("Superfamily with that key already exists", "Key exists");
            refreshSuperfamilies();
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
            groupBox1.Text = "Edit mode: Superfamily " + textBox1.Text;
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
            groupBox1.Text = "Adding new superfamily";
            button4.Visible = false;
            button5.Visible = false;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                string key = UISnippets.getFirstSelectedItem(listBox1);
                SuperfamilyDeclarator sfd;
                if (cm.doesSuperfamilyExist(key))
                {
                    sfd = cm.getSuperfamilyDeclarator(key);
                }
                else
                {
                    sfd = temporaryChanges[key];
                }
                textBox1.Text = sfd.SuperFamily;
                textBox2.Text = sfd.Description;
                comboBox1.Text = cm.getFileType(sfd.FileTypeKey).TypeName;
                editMode();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cm.doesSuperfamilyExist(textBox1.Text))
            {
                cm.cfg.Superfamilies[textBox1.Text].Description = textBox2.Text;
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
