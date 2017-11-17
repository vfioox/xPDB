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
using xPDB.Models.Storage;
using xPDB.Storage;
using xPDB.Utility;

namespace xPDB.Windows
{
    public partial class DirectoryAquisitor : Form
    {
        private ConfigManager cm;
        private string path;
        public DirectoryAquisitor(string path, ref ConfigManager cm)
        {
            this.cm = cm;
            this.path = path;
            InitializeComponent();
        }

        private void DirectoryAquisitor_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, FileType> ft in cm.cfg.FileTypes)
            {
                checkedListBox1.Items.Add(ft.Value.TypeKey);
            }
            label2.Text = path;
        }

        internal List<string> files = new List<string>();
        internal List<string> filesConfirmed = new List<string>();
        internal List<long> sizesConfirmed = new List<long>();
        internal int ToAddCount = 0;
        internal void DirSearch(string sDir)
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                DirSearch(d);
            }
            foreach (string f in Directory.GetFiles(sDir))
            {
                files.Add(f);
            }
        }
        private async void beginAnalyse()
        {
            progressBar2.Style = ProgressBarStyle.Marquee;
            files.Clear();
            filesConfirmed.Clear();
            sizesConfirmed.Clear();
            button1.Enabled = false;

            long total = 0;
            await Task.Run(() =>
            {
                DirSearch(path);
                ToAddCount = 0;
                var ftlist = new List<string>();
                foreach (var i in checkedListBox1.CheckedItems)
                {
                    ftlist.Add(i.ToString());
                }
                foreach (string file in files)
                {
                    string ext = Path.GetExtension(file);
                    var ft = cm.determineFileType(ext);
                    if (ft != null && ftlist.Contains(ft.TypeKey) || (ftlist.Contains("def_unkn")))
                    {
                        ToAddCount++;
                        filesConfirmed.Add(file);
                        sizesConfirmed.Add(new FileInfo(file).Length);
                    }
                }
                long to = 0;
                foreach (long size in sizesConfirmed)
                {
                    to += size;
                }
                total = to;
            });
            analysisLabel.Text = ToAddCount.ToString();
            label3.Text = Humanizer.BytesToString(total);
            label4.Text = total.ToString();
            progressBar2.Style = ProgressBarStyle.Blocks;
            button1.Enabled = true;
        }

        private async void beginAquisition()
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = ToAddCount + 1;
            var progressHandler = new Progress<int>(value =>
            {
                progressLabel.Text = value.ToString();
                progressBar1.Value = value;
            });
            var progress = progressHandler as IProgress<int>;
            await Task.Run(() =>
            {
                int i = 0;
                foreach (string file in filesConfirmed)
                {
                    var data = FileOperations.readAllBytes(file);
                    Chunk c = null;
                    foreach (KeyValuePair<string, Chunk> kv in cm.cfg.Chunks)
                    {
                        if (kv.Value.Vacant && !kv.Value.PossiblyCorrupted && !kv.Value.Corrupted)
                        {
                            c = kv.Value;
                        }
                    }
                    if (c == null)
                    {
                        c = cm.getChunk(ChunkOperations.newChunk(ref cm));
                        cm.saveConfig();
                    }
                    var ft = cm.determineFileType(Path.GetExtension(file));
                    if (ft == null) ft = cm.getFileType("def_unkn");
                    var fd = new FileDeclarator
                    {
                        ChunkKey = c.ChunkId,
                        FileTypeKey = ft.TypeKey,
                        Comment = "",
                        FileId = RandomData.RandomString(14),
                        OriginalPath = file,
                        SuperfamilyKey = "system_aquisition",
                        TagKeys = new List<string>(),
                        Title = "system_aquisition"
                    };
                    var status = false;
                    if (data != null && data.Length > 0)
                    {
                        status = ChunkOperations.writeToChunkEnd(c.ChunkId, fd.FileId, data, ref cm);
                    }
                    if (status)
                    {
                        cm.cfg.FileDeclarators.Add(fd.FileId, fd);
                    }
                    progress.Report(i);
                    i++;
                }
            });
            cm.saveConfig();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            beginAnalyse();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            beginAquisition();
        }
    }
}
