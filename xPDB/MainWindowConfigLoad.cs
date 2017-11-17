using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xPDB.Models.Objects;
using xPDB.Storage;
using xPDB.Utility;

namespace xPDB
{
    public partial class MainWindow : Form
    {
        private bool multipleInstances = false;

        public void beginConfigLoad()
        {
            configLoaderPanel.Dock = DockStyle.Fill;
            configLoaderPanel.Visible = true;
            configLoaderPanel.BringToFront();
            panel1.Visible = false;
            menuStrip1.Enabled = false;
            statusStrip1.Enabled = false;
            textBox1.Focus();

            spiderWork.Enabled = true;


            string[] instances = ConfigManager.findInstances();
            if (instances.Count() > 1)
            {
                multipleInstances = true;
                moreInstancesBox.Visible = true;
                foreach (string i in instances)
                {
                    listBox1.Items.Add(i);
                }
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                e.Handled = true;
                goConfigAttempt();
                textBox1.Text = "";
            }
        }
        public void setLabelStatus(string status)
        {
            configStatusLabel.Text = status;
        }
        public void goConfigAttempt()
        {
            cm = new Storage.ConfigManager(textBox1.Text);
            ConfigLoadStatus status;
            if (multipleInstances)
            {
                if (listBox1.SelectedItems.Count == 0) return;
                status = cm.loadConfig(UISnippets.getFirstSelectedItem(listBox1));
            }
            else
            {
                status = cm.loadConfig();
            }
            switch (status)
            {
                case ConfigLoadStatus.CreatedNewConfig:
                    setLabelStatus("New config created");
                    break;
                case ConfigLoadStatus.FileLocked:
                    setLabelStatus("Could not access config");
                    break;
                case ConfigLoadStatus.ParsingFailed:
                    setLabelStatus("Parsing failed!");
                    break;
                case ConfigLoadStatus.UnexpectedEncryption:
                    setLabelStatus("Unexpected encryption critical failure");
                    break;
            }
            if(status == ConfigLoadStatus.Success)
            {
                if (multipleInstances) instanceStatusLabel.Text = "Instance: " + listBox1.SelectedItems[0].ToString();
                else instanceStatusLabel.Text = "Instance: " + ConfigManager.defaultInstanceName;
                endConfigLoad();
            }
        }

        private TabPage _DATP;
        public void endConfigLoad()
        {
            if (!Directory.Exists(cm.cfg.ChunkPath))
            {
                Directory.CreateDirectory(cm.cfg.ChunkPath);
            }
            configLoaderPanel.Dock = DockStyle.None;
            configLoaderPanel.Visible = false;
            menuStrip1.Enabled = true;
            statusStrip1.Enabled = true;
            panel1.Visible = true;
            initialLoadStatus.Value = 100;
            ConfigSuccessful();





            //cm.cfg.OStorage.Add("dac", new DACredentials
            //{
            //    ClientId = "5729",
            //    Secret = "291201b1c51865ac59a884bb29d1d34a"
            //});

            if (startup_arg != null)
            {
                actionLabel.Text = "Started with argument: " + startup_arg;
                if (startup_arg == "dabegin")
                {
                    
                }
            }

            spiderWork.Enabled = false;
            spiderStatusLabel.Text = "_";
        }
    }
    public enum ReaderPages
    {
        reddit
    }
}
