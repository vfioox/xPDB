using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using xPDB.Models.Storage;
using xPDB.Models.Systematization;
using xPDB.Storage;
using xPDB.Utility;
using xPDB.Windows.Helpers;

namespace xPDB.Windows.FileAdders
{
    public partial class StoreFileAsIs : Form
    {
        private ConfigManager cm;
        private string path;
        private FileType ft;
        public StoreFileAsIs(ConfigManager cm, string path, FileType ft)
        {
            this.cm = cm;
            this.path = path;
            this.ft = ft;
            InitializeComponent();
        }

        private void StoreFileAsIs_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Enabled = false;

        }

        private void refreshTags()
        {
            Dictionary<string, int> fam = new Dictionary<string, int>();
            listView1.Groups.Clear();
            int c = 0;

            listView1.Columns.Add(new ColumnHeader().Text = "Tag text");

            foreach (KeyValuePair<string, FamilyDeclarator> fd in cm.cfg.Families)
            {
                if (fd.Value.Superfamily == textBox5.Text)
                {
                    fam.Add(fd.Key, c);
                    var group = new ListViewGroup(fd.Key);
                    listView1.Groups.Add(group);
                    c++;
                }
            }

            foreach (KeyValuePair<string, TagDeclarator> td in cm.cfg.Tags)
            {
                if (cm.doesFamilyExist(td.Value.Family))
                {
                    var fd = cm.getFamilyDeclarator(td.Value.Family);
                    if (fd.Superfamily == textBox5.Text)
                    {
                        var tagRow = new ListViewItem(td.Key);
                        tagRow.Group = listView1.Groups[fam[td.Value.Family]];
                        listView1.Items.Add(tagRow);
                    }
                }
                else
                {
                    UISnippets.messageBoxWarning("Family doesn't exist! Weird error", "No family key exists");
                }
            }
        }
        
        private async void button1_Click(object sender, EventArgs e)
        {
            string selfam = "", t2 = "", t3 = "", t4 = "", preview = "";
            var fts = new FamilyTreeSelector(ref cm, true);
            fts.ShowDialog();
            selfam = fts.selectedFamily;
            fts.Dispose();
            loadingBox.Visible = true;
            await Task.Run(() =>
            {
                if (selfam != "")
                {
                    t2 = RandomData.RandomString(14);
                    foreach (KeyValuePair<string, Chunk> c in cm.cfg.Chunks)
                    {
                        if (c.Value.Vacant && !c.Value.Corrupted)
                        {
                            calculateSystemImpact(path, c.Value);
                            t3 = c.Key;
                            break;
                        }
                    }
                    t4 = path;
                    var read = FileOperations.readStreamLine(path, 1);
                    if (read == null)
                    {
                        UISnippets.messageBoxWarning("We cannot fully access this file because it's used by another process. Wizard will quit.", "File locked");
                        this.Invoke((MethodInvoker)(() =>
                        {
                            this.Dispose();
                        }));
                        return;
                    }

                    if (read.Length < 500) preview = read;
                    else preview = read.Substring(0, 500);
                }
            }).ContinueWith(t => {
                if (this.ActiveControl != null && !this.IsDisposed)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        splitContainer1.Panel2.Enabled = true;
                        textBox2.Text = t2;
                        button1.Text = selfam;
                        textBox1.Text = preview;
                        textBox3.Text = t3;
                        textBox4.Text = t4;
                        textBox5.Text = selfam;

                        listView1.Enabled = true;
                        refreshTags();
                        loadingBox.Visible = false;
                        if (ft._FileType == FileTypes.Image)
                        {
                            pictureBox1.Image = Image.FromFile(path);
                        }
                    }));
                }
                else
                {
                    return;
                }
            });

        }

        private void calculateSystemImpact(string path, Chunk c)
        {
            var inf = new FileInfo(path);
            var size = inf.Length;
            var ro = inf.IsReadOnly;
            if (ro) panel2.Enabled = false;
            var cpath = ChunkOperations.getChunkPath(c.ChunkId, cm);
            var cinf = new FileInfo(cpath);
            var csize = cinf.Length;
            var added = csize + size;
            var nofiles = c.FileCodes.Count + 1;

            this.Invoke((MethodInvoker)(() =>
            {
                systemImpactLabel.Text = string.Format("New file size: {0}\n" +
                                                       "Current chunk size: {1}\n" +
                                                       "After op: {2}\n" +
                                                       "NO of files after: {3}\n",
                size, csize, added, nofiles);
            }));
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            bool status = false;
            var seltag = new List<string>();
            foreach (ListViewItem val in listView1.CheckedItems)
            {
                seltag.Add(val.Text);
            }
            await Task.Run(() =>
            {
                var data = FileOperations.readAllBytes(path);
                var c = cm.getChunk(textBox3.Text);
                if (!c.PossiblyCorrupted)
                {
                    var fd = new FileDeclarator
                    {
                        ChunkKey = c.ChunkId,
                        FileTypeKey = ft.TypeKey,
                        Comment = textBox7.Text,
                        FileId = textBox2.Text,
                        OriginalPath = textBox4.Text,
                        SuperfamilyKey = button1.Text,
                        TagKeys = seltag,
                        Title = textBox6.Text
                    };
                    status = ChunkOperations.writeToChunkEnd(c.ChunkId, textBox2.Text, data , ref cm);
                    if (status)
                    {
                        cm.cfg.FileDeclarators.Add(fd.FileId, fd);
                        cm.saveConfig();
                        this.Invoke((MethodInvoker)(() =>
                        {
                            this.Dispose();
                        }));
                        return;
                    }
                }
            });
        }
    }
}
