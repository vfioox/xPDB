using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xPDB
{
    public partial class MainWindow : Form
    {
        public void displayCounters()
        {
            filesStoredLabel.Text = "Files stored: " + cm.cfg.FileDeclarators.Count.ToString();
            familiesCounter.Text = "Families stored: " + cm.cfg.Families.Count.ToString();
            tagsCounter.Text = "Tags stored: " + cm.cfg.Tags.Count.ToString();
            superfamiliesCounter.Text = "Superfamilies stored: " + cm.cfg.Superfamilies.Count.ToString();
            chunksCounterLabel.Text = "Chunks registered: " + cm.cfg.Chunks.Count.ToString();
            cm.saveConfig();

            superfamilyToolStripMenuItem.DropDownItems.Clear();
            foreach (var sf in cm.cfg.Superfamilies)
            {
                superfamilyToolStripMenuItem.DropDownItems.Add(sf.Key, null, everythingToolStripMenuItem_Click);
            }
            superfamilyToolStripMenuItem.DropDownItems.Add("system_aquisition", null, everythingToolStripMenuItem_Click);
            superfamilyToolStripMenuItem.DropDownItems.Add("queue", null, everythingToolStripMenuItem_Click);
            interceptorTimer.Start();
        }

        public void steamIntegration()
        {

            
        }
    }
}
