using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xPDB.Models.Storage;
using xPDB.Storage;
using xPDB.Windows.Helpers;

namespace xPDB.Windows
{
    public enum IEFilter
    {
        Everything,
        Untagged
    }
    public partial class ImageExplorer : Form
    {
        internal ConfigManager cm;
        internal IEFilter IeFilter;
        internal string superfamilyKey = null;
        public ImageExplorer(ref ConfigManager cm, IEFilter Ifilter, string superfamilyKey = null)
        {
            this.cm = cm;
            this.IeFilter = Ifilter;
            InitializeComponent();
            if (superfamilyKey != null) this.superfamilyKey = superfamilyKey;
        }

        private int current_page = 1;
        private int pages = 1;

        private Dictionary<string, string> hashesForDuplicates = new Dictionary<string, string>();

        private int box_def_h = 240;
        private int box_def_w = 240;

        private Dictionary<int, List<string>> keypages;

        private void displayTags(ListView lv)
        {
            lv.BringToFront();
            var fkd = new Dictionary<string, ListViewGroup>();
            var i = 0;
            lv.Items.Clear();
            lv.Groups.Clear();
            lv.ShowGroups = true;
            lv.CheckBoxes = true;
            foreach (var fam in cm.cfg.Families)
            {
                if (superfamilyKey != null && fam.Value.Superfamily != superfamilyKey) continue;
                var lvg = new ListViewGroup(fam.Value.Family);
                lvg.Header = fam.Value.Family;
                lv.Groups.Add(lvg);
                fkd.Add(fam.Key, lvg);
                i++;
            }
            foreach (var item in cm.cfg.Tags)
            {
                if (fkd.ContainsKey(item.Value.Family))
                {
                    var row = new ListViewItem();
                    row.Text = item.Value._Tag;
                    row.ToolTipText = item.Value.Description;
                    row.Group = fkd[item.Value.Family];
                    lv.Items.Add(row);
                }
            }
        }
        private void ImageExplorer_Load(object sender, EventArgs e)
        {
            loadPage(current_page);
            displayTags(includeView);
            displayTags(excludeView);
            foreach (var sf in cm.cfg.Superfamilies)
            {
                assignToSuperfamilyToolStripMenuItem.DropDownItems.Add(sf.Key, null, assignToSF);
            }
        }

        private void assignToSF(object sender, EventArgs e)
        {
            ToolStripDropDownItem menuItem = sender as ToolStripDropDownItem;
            if (menuItem != null)
            {
                ToolStripDropDownMenu subOwner = menuItem.Owner as ToolStripDropDownMenu;
                if (subOwner != null)
                {
                    ContextMenuStrip owner = subOwner.OwnerItem.Owner as ContextMenuStrip;
                    if (owner != null)
                    {
                        var pb = openedOn;
                        var file = cm.cfg.FileDeclarators[pb.Tag.ToString()];
                        if (toTagList.Count == 0) toTagList.Add(file.FileId);
                        foreach (var f in toTagList)
                        {
                            cm.cfg.FileDeclarators[f].SuperfamilyKey = menuItem.Text;
                        }
                        cm.cfg.MRS = true;
                        cm.saveConfig();
                        if (cm.cfg.FileDeclarators[toTagList[0]].SuperfamilyKey == menuItem.Text)
                        {
                            MessageBox.Show("Done!");
                        }
                        else
                        {
                            MessageBox.Show("Something went wrong");
                        }
                    }
                }
            }
        }

        private List<string> includeTags = new List<string>();
        private List<string> excludeTags = new List<string>();
        private bool validateFile(FileDeclarator fd)
        {
            var a = cm.getFileType(fd.FileTypeKey)._FileType == FileTypes.Image;
            if (superfamilyKey != null)
            {
                if (fd.SuperfamilyKey != superfamilyKey) a = false;
            }
            foreach (var tag in includeTags)
            {
                if (!fd.TagKeys.Contains(tag))
                {
                    a = false;
                }
            }
            foreach (var tag in fd.TagKeys)
            {
                if (excludeTags.Contains(tag))
                {
                    a = false;
                }
            }
            if (fd.TagKeys.Count == 0 && includeTags.Count > 0) a = false;
            if (IeFilter != IEFilter.Everything)
            {
                switch (IeFilter)
                {
                    case IEFilter.Untagged:
                        if (fd.TagKeys.Count != 0)
                        {
                            a = false;
                        }
                        break;
                }
            }
            return a;
        }
        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
        private string calculateHash(byte[] data)
        {
            string hash;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                hash = Convert.ToBase64String(sha1.ComputeHash(data));
            }
            return hash;
        }
        private async void loadPage(int page)
        {
            foreach (Control c in panel2.Controls)
            {
                c.Dispose();
            }
            panel2.Controls.Clear();
            comboBox1.Text = current_page.ToString();
            button1.Enabled = false;
            button2.Enabled = false;
            var list = new List<string>();
            var height = panel2.Height;
            var width = panel2.Width;

            int h_boxes = height / box_def_h;
            int w_boxes = width / box_def_w;

            int total_boxes = h_boxes * w_boxes;
            var img = 0;
            foreach (var kvf in cm.cfg.FileDeclarators)
            {
                if (validateFile(kvf.Value))
                {
                    img++;
                }
            }
            decimal math = img / total_boxes;

            var progressHandler = new Progress<PictureBox>(value =>
            {
                panel2.Controls.Add(value);
            });
            var progress = progressHandler as IProgress<PictureBox>;
            await Task.Run(() =>
            {
                if (keypages == null)
                {
                    keypages = new Dictionary<int, List<string>>();
                    var k = 0;
                    var p = 1;
                    foreach (var kvf in cm.cfg.FileDeclarators)
                    {
                        if (validateFile(kvf.Value))
                        {
                            list.Add(kvf.Key);
                            k++;
                            if (k > total_boxes + 2)
                            {
                                var lis = new List<string>();
                                foreach (var e in list)
                                {
                                    lis.Add(e);
                                }
                                keypages.Add(p, lis);
                                list.Clear();
                                p++;
                                k = 0;
                            }
                        }
                    }
                    if (list.Count != 0)
                    {
                        var lisx = new List<string>();
                        foreach (var e in list)
                        {
                            lisx.Add(e);
                        }
                        keypages.Add(p, lisx);
                        list.Clear();
                        if (includeTags.Contains("max"))
                        {
                            ;
                        }
                    }
                    else
                    {
                        return;
                    }
                    pages = keypages.Count;
                    if (current_page > pages)
                    {
                        current_page = pages;
                    }
                }
                var total = 0;
                for (var i = 0; i < h_boxes; i++)
                {
                    for (var j = 0; j < w_boxes; j++)
                    {
                        if (keypages.ContainsKey(current_page) && keypages[current_page].Count > total)
                        {
                            var pb = new PictureBox();
                            pb.SizeMode = PictureBoxSizeMode.Zoom;
                            var file = cm.cfg.FileDeclarators[keypages[current_page].ElementAt(total)];
                            var chunk = cm.getChunk(file.ChunkKey);
                            var d = ChunkOperations.readChunkPosition(chunk.ChunkId,
                                file.FileId, ref cm);
                            if (d == null)
                            {
                                pb.Image = null;
                            }
                            else
                            {
                                var s = new MemoryStream(d);
                                pb.Image = Image.FromStream(s);
                            }

                            pb.Tag = file.FileId;
                            pb.Click += Pb_Click;
                            pb.MouseEnter += Pb_MouseHover;
                            pb.ContextMenuStrip = pbContextMenuStrip;

                            if (file.TagKeys.Count == 0)
                            {
                                pb.BackColor = Color.Indigo;
                            }


                            pb.Size = new Size(box_def_w, box_def_h);
                            pb.Location = new Point(box_def_w * j, box_def_h * i);

                            if (d != null)
                            {
                                var hash = calculateHash(ImageToByteArray(pb.Image));
                                if (hashesForDuplicates.ContainsKey(hash))
                                {
                                    if (hashesForDuplicates[hash] != file.FileId)
                                        pb.BackColor = Color.OrangeRed;

                                }
                                else hashesForDuplicates.Add(hash, file.FileId);
                            }
                            else
                            {
                                pb.BackColor = Color.Aqua;
                            }

                            progress.Report(pb);

                            total++;
                        }
                    }
                }
                return;
            });

            if (pages != 1)
            {
                label1.Text = current_page + "/" + pages.ToString();
                comboBox1.Items.Clear();
                for (var i = 1; i <= pages; i++)
                {
                    comboBox1.Items.Add(i.ToString());
                }
            }
            button1.Enabled = true;
            button2.Enabled = true;
            return;
        }

        private List<string> toTagList = new List<string>();
        private void Pb_Click(object sender, EventArgs e)
        {
            var pb = ((PictureBox) sender);
            if (Control.ModifierKeys == Keys.Shift)
            {
                pb.BackColor = Color.Blue;
                toTagList.Add(pb.Tag.ToString());
            }
            else
            {
                foreach (var control in panel2.Controls)
                {
                    var c = control as PictureBox;
                    if (c != null && c.BackColor == Color.Blue)
                    {
                        c.BackColor = Color.Black;
                    }
                }
                toTagList.Clear();
                var iv = new ImageView(pb.Image);
                iv.Show();
            }
        }

        private void Pb_MouseHover(object sender, EventArgs e)
        {
            label2.Text = ((PictureBox) sender).Tag.ToString();

        }

        private void button1_Click(object sender, EventArgs e) // forward
        {
            if (current_page + 1 <= pages)
            {
                current_page++;
                loadPage(current_page);
            }
        }

        private void button2_Click(object sender, EventArgs e) // back
        {
            if (1 <= current_page - 1)
            {
                current_page--;
                loadPage(current_page);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != current_page.ToString())
            {
                current_page = int.Parse(comboBox1.Text);
                loadPage(current_page);
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            keypages = new Dictionary<int, List<string>>();
            if (comboBox2.Text == "Full screen")
            {
                box_def_h = panel2.Height - 2;
                box_def_w = panel2.Width - 2;
                var i = 1;
                foreach (var fd in cm.cfg.FileDeclarators)
                {
                    if (validateFile(fd.Value))
                    {
                        var l = new List<string>();
                        l.Add(fd.Value.FileId);
                        keypages.Add(i, l);
                        i++;
                    }
                }
                pages = keypages.Count;
                if (current_page > pages)
                {
                    current_page = pages;
                }
                loadPage(current_page);
            }
            else
            {
                box_def_h = int.Parse(comboBox2.Text);
                box_def_w = box_def_h;
                loadPage(current_page);
            }
        }

        private void tagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    var pb = owner.SourceControl as PictureBox;
                    var file = cm.cfg.FileDeclarators[pb.Tag.ToString()];
                    if (toTagList.Count == 0) toTagList.Add(file.FileId);
                    var tg = new Tagger(ref cm, toTagList);
                    var dr = tg.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        if (tg.tagsChosen.Count > 0)
                        {
                            foreach (var f in toTagList)
                            {
                                cm.cfg.FileDeclarators[f].TagKeys = tg.tagsChosen;
                            }
                            cm.saveConfig();
                            if (IeFilter != IEFilter.Everything)
                            {
                                keypages = null;
                                loadPage(current_page);
                            }
                        }
                    }
                    foreach (var control in panel2.Controls)
                    {
                        var c = control as PictureBox;
                        if (c != null && c.BackColor == Color.Blue)
                        {
                            c.BackColor = Color.Black;
                        }
                    }
                    toTagList.Clear();
                    tg.Dispose();
                }
            }
        }

        private PictureBox openedOn;
        private void pbContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (toTagList.Count > 0)
            {
                tagToolStripMenuItem.Text = "Tag selection";
                deleteToolStripMenuItem.Text = "Delete those";
            }
            else
            {
                tagToolStripMenuItem.Text = "Tag";
                deleteToolStripMenuItem.Text = "Delete this";
            }
            openedOn = pbContextMenuStrip.SourceControl as PictureBox;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            includeView.Visible = !includeView.Visible;
            includeView.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            excludeView.Visible = !excludeView.Visible;
            excludeView.BringToFront();
        }

        private void excludeView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void includeView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            toTagList.Clear();
            excludeTags.Clear();
            foreach (var i in excludeView.CheckedItems)
            {
                var x = i as ListViewItem;
                excludeTags.Add(x.Text);
            }
            includeTags.Clear();
            foreach (var i in includeView.CheckedItems)
            {
                var x = i as ListViewItem;
                includeTags.Add(x.Text);
            }
            keypages = null;
            loadPage(current_page);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            toTagList.Clear();
            excludeTags.Clear();
            var ix = 0;
            foreach (var ci in excludeView.CheckedItems)
            {
                excludeView.Items[ix].Checked = false;
                ix++;
            }
            includeTags.Clear();
            ix = 0;
            foreach (var ci in includeView.CheckedItems)
            {
                includeView.Items[ix].Checked = false;
                ix++;
            }
            keypages = null;
            loadPage(current_page);
        }

        private void addTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    var pb = owner.SourceControl as PictureBox;
                    var file = cm.cfg.FileDeclarators[pb.Tag.ToString()];
                    if (toTagList.Count == 0) toTagList.Add(file.FileId);
                    var tg = new Tagger(ref cm, toTagList);
                    var dr = tg.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        if (tg.tagsChosen.Count > 0)
                        {
                            foreach (var f in toTagList)
                            {
                                foreach (var t in tg.tagsChosen)
                                {
                                    if (!cm.cfg.FileDeclarators[f].TagKeys.Contains(t))
                                    {
                                        cm.cfg.FileDeclarators[f].TagKeys.Add(t);
                                    }
                                }
                            }
                            cm.saveConfig();
                            if (IeFilter != IEFilter.Everything)
                            {
                                keypages = null;
                                loadPage(current_page);
                            }
                        }
                    }
                    foreach (var control in panel2.Controls)
                    {
                        var c = control as PictureBox;
                        if (c != null && c.BackColor == Color.Blue)
                        {
                            c.BackColor = Color.Black;
                        }
                    }
                    toTagList.Clear();
                    tg.Dispose();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    var pb = owner.SourceControl as PictureBox;
                    pb.Image = null;
                    var file = cm.cfg.FileDeclarators[pb.Tag.ToString()];
                    if (toTagList.Count == 0) toTagList.Add(file.FileId);
                    foreach (var f in toTagList)
                    {
                        var b = ChunkOperations.emptyChunkPosition(cm.cfg.FileDeclarators[f].FileId, cm.cfg.FileDeclarators[f].ChunkKey, ref cm);
                    }
                    toTagList.Clear();
                    keypages = null;
                    cm.cfg.MRS = true;
                    cm.saveConfig();
                    if(toTagList.Count > 1)
                        this.Dispose();
                }
            }
        }

        private void ImageExplorer_FormClosing(object sender, FormClosingEventArgs e)
        {
            hashesForDuplicates.Clear();
            keypages.Clear();
            panel2.Controls.Clear();
        }
    }
}
