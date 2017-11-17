using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using xPDB.Models.Storage;
using xPDB.Network;
using xPDB.Storage;
using xPDB.Utility;
using xPDB.Windows;
using xPDB.Windows.FileAdders;
using xPDB.Windows.Helpers;
using xPDB.Windows.Readers;
using xPDB.Windows.Tools;

namespace xPDB
{
    public partial class MainWindow : Form
    {
        public ConfigManager cm;
        public WindowManager wm;

        public string startup_arg;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region MAIN_EVENTS
        private void MainWindow_Load(object sender, EventArgs e)
        {
            beginConfigLoad();
            wm = new WindowManager();
        }
        private void MainWindow_Shown(object sender, EventArgs e)
        {
            textBox1.Text = "test";
            goConfigAttempt();
            //MessageBox.Show(ChunkOperations.NewChunk(ref cm).ToString());
        }
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void ConfigSuccessful()
        {
            displayCounters();
        }
        #endregion

        #region MENU_OPERATORS
        private void tagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cm.cfg.Families.Count == 0)
            {
                UISnippets.messageBoxWarning("Add some families first", "No families");
                return;
            }
            CreateWindow<TagsM>("tagsm", true, true, cm);
        }
        private void familiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cm.cfg.Superfamilies.Count == 0)
            {
                UISnippets.messageBoxWarning("Add some superfamilies first", "No superfamilies");
                return;
            }
            CreateWindow<Families>("families", true, true, cm);
        }
        private void superfamiliesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateWindow<Superfamilies>("superfamilies", true, true, cm);
        }
        private void iPLookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateWindow<IPLookup>("iplookup");
        }
        private void displayConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewDisplay tvd = new TreeViewDisplay(cm, ref cm);
            tvd.Show();
        }
        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewChunk();
        }
        private async void directoryAquisitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new FolderBrowserDialog();
            var res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                await Task.Run(() =>
                {
                    DirectoryAquisitor da = wm.putWindow("diraq", new DirectoryAquisitor(ofd.SelectedPath, ref cm)) as DirectoryAquisitor;
                    da.ShowDialog();
                    wm.destroyWindow("diraq");
                });

                targetedCBLToolStripMenuItem_Click(null, null);
            }
        }
        private async void calculateBinaryLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            freezeUI();
            spiderWork.Enabled = true;
            initialLoadStatus.Value = 0;
            initialLoadStatus.Maximum = cm.cfg.FileDeclarators.Count + 1;

            var progressHandler = new Progress<int>(value =>
            {
                if (value <= initialLoadStatus.Maximum) initialLoadStatus.Value = value;
                else initialLoadStatus.Value = initialLoadStatus.Maximum;
            });
            var progress = (IProgress<int>)progressHandler;
            await Task.Run(() =>
            {
                var i = 1;
                foreach (KeyValuePair<string, Chunk> kv in cm.cfg.Chunks)
                {
                    kv.Value.FileOffsets = new Dictionary<string, int>();
                    kv.Value.FileLenghts = new Dictionary<string, int>();
                    var totalOffset = 0;
                    foreach (KeyValuePair<string, int> fc in kv.Value.FileCodes)
                    {
                        var b64 = Encoding.UTF8.GetBytes(FileOperations.readStreamLine(ChunkOperations.getChunkPath(kv.Value.ChunkId, cm), fc.Value));
                        kv.Value.FileOffsets[fc.Key] = totalOffset;
                        kv.Value.FileLenghts[fc.Key] = b64.Length;
                        totalOffset += b64.Length + 1;
                        progress.Report(i);
                        i++;
                    }
                }
                cm.saveConfig();
            });
            spiderWork.Enabled = false;
            unfreezeUI();
            initialLoadStatus.Value = 0;
            initialLoadStatus.Maximum = 100;
            initialLoadStatus.Value = 100;
        }
        private async void softCalculateBinaryLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            freezeUI();
            spiderWork.Enabled = true;
            initialLoadStatus.Value = 0;
            initialLoadStatus.Maximum = cm.cfg.FileDeclarators.Count + 1;

            var progressHandler = new Progress<int>(value =>
            {
                initialLoadStatus.Value = value;
            });
            var progress = (IProgress<int>)progressHandler;
            await Task.Run(() =>
            {
                var i = 1;
                foreach (KeyValuePair<string, Chunk> kv in cm.cfg.Chunks)
                {
                    if (kv.Value.Vacant == true)
                    {
                        kv.Value.FileOffsets = new Dictionary<string, int>();
                        kv.Value.FileLenghts = new Dictionary<string, int>();
                        var totalOffset = 0;
                        foreach (KeyValuePair<string, int> fc in kv.Value.FileCodes)
                        {
                            var b64 = Encoding.UTF8.GetBytes(FileOperations.readStreamLine(ChunkOperations.getChunkPath(kv.Value.ChunkId, cm), fc.Value));
                            kv.Value.FileOffsets[fc.Key] = totalOffset;
                            kv.Value.FileLenghts[fc.Key] = b64.Length;
                            totalOffset += b64.Length + 1;
                            progress.Report(i);
                            i++;
                        }
                    }
                }
                cm.saveConfig();
            });
            spiderWork.Enabled = false;
            initialLoadStatus.Value = 0;
            unfreezeUI();
            initialLoadStatus.Maximum = 100;
            initialLoadStatus.Value = 100;
        }
        private async void targetedCBLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spiderWork.Enabled = true;
            freezeUI();
            initialLoadStatus.Value = 0;
            initialLoadStatus.Maximum = cm.cfg.FileDeclarators.Count + 1;

            List<string> chunksToCheck = new List<string>();

            var progressHandler = new Progress<int>(value =>
            {
                if (initialLoadStatus.Maximum > value)
                {
                    initialLoadStatus.Value = value;
                }
                else initialLoadStatus.Value = initialLoadStatus.Maximum;
            });
            var progress = (IProgress<int>)progressHandler;
            await Task.Run(() =>
            {
                var i = 1;
                int kvfo;
                foreach (var kvf in cm.cfg.FileDeclarators)
                {
                    try
                    {
                        kvfo = cm.cfg.Chunks[kvf.Value.ChunkKey].FileOffsets[kvf.Key];
                    }
                    catch
                    {
                        kvfo = -1;
                    }
                    if (kvfo == -1)
                    {
                        chunksToCheck.Add(kvf.Value.ChunkKey);
                    }
                }
                this.Invoke((MethodInvoker)(() =>
                {
                    initialLoadStatus.Maximum = chunksToCheck.Count + 1;
                    initialLoadStatus.Value = 0;
                }));
                foreach (KeyValuePair<string, Chunk> kv in cm.cfg.Chunks)
                {
                    if (chunksToCheck.Contains(kv.Key))
                    {
                        kv.Value.FileOffsets = new Dictionary<string, int>();
                        kv.Value.FileLenghts = new Dictionary<string, int>();
                        var totalOffset = 0;
                        foreach (KeyValuePair<string, int> fc in kv.Value.FileCodes)
                        {
                            var op = FileOperations.readStreamLine(ChunkOperations.getChunkPath(kv.Value.ChunkId, cm),
                                fc.Value);
                            if (op == null) continue;
                            var b64 = Encoding.UTF8.GetBytes(op);
                            kv.Value.FileOffsets[fc.Key] = totalOffset;
                            kv.Value.FileLenghts[fc.Key] = b64.Length;
                            totalOffset += b64.Length + 1;
                            progress.Report(i);
                            i++;
                        }
                    }
                }
                cm.cfg.MRS = false;
                cm.saveConfig();
                cm.saveConfig();
            });
            spiderWork.Enabled = false;
            initialLoadStatus.Value = 0;
            unfreezeUI();
            initialLoadStatus.Maximum = 100;
            initialLoadStatus.Value = 100;
        }
        private void asIsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                OperateFileImport(ofd.FileName);
            }
        }
        private void everythingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var x = sender as ToolStripDropDownItem;
                if (sender != null && x.OwnerItem == superfamilyToolStripMenuItem)
                {
                    ImageExplorer ie =
                            wm.putWindow("imageexplorer", new ImageExplorer(ref cm, IEFilter.Everything, x.Text)) as
                                ImageExplorer;

                    ie.ShowDialog();
                }
                else
                {
                    ImageExplorer ie = wm.putWindow("imageexplorer", new ImageExplorer(ref cm, IEFilter.Everything)) as ImageExplorer;

                    ie.ShowDialog();
                }
                wm.destroyWindow("imageexplorer");
                if (cm.cfg.MRS)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        targetedCBLToolStripMenuItem_Click(null, null);
                    }));
                }
            });
        }
        private void untaggedOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                ImageExplorer ie = wm.putWindow("imageexplorer", new ImageExplorer(ref cm, IEFilter.Untagged)) as ImageExplorer;
                ie.ShowDialog();
                wm.destroyWindow("imageexplorer");
                if (cm.cfg.MRS)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        targetedCBLToolStripMenuItem_Click(null, null);
                    }));
                }
            });
        }
        private void chanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }
        private void deviantartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        private async void exportAllImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var m_path = "export-" + Utility.UnixTimestamp.Get().ToString();
            Directory.CreateDirectory(m_path);

            initialLoadStatus.Value = 0;
            foreach (KeyValuePair<string, FileDeclarator> kv in cm.cfg.FileDeclarators)
            {
                if (kv.Value.FileTypeKey == "def_image")
                {
                    initialLoadStatus.Maximum++;
                }
            }
            initialLoadStatus.Maximum++; // for weird situations...
            spiderWork.Start();

            var progressHandler = new Progress<int>(value =>
            {
                initialLoadStatus.Value = value;
            });
            var progress = progressHandler as IProgress<int>;

            var c_p = 0; // current progress

            await Task.Run(() =>
            {
                foreach (KeyValuePair<string, FileDeclarator> kv in cm.cfg.FileDeclarators)
                {
                    if (kv.Value.FileTypeKey == "def_image")
                    {
                        var chunk = cm.getChunk(kv.Value.ChunkKey);
                        var data = ChunkOperations.readChunkPosition(chunk.ChunkId,
                            kv.Value.FileId, ref cm);

                        c_p++;

                        FileOperations.writeByteArrayViaStream(m_path + "\\" + Path.GetFileName(kv.Value.OriginalPath), data);
                        progress.Report(c_p);
                    }
                }
            });
            initialLoadStatus.Value = initialLoadStatus.Maximum;
            spiderWork.Stop();
        }
        private void superfamilyToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        #endregion

        #region DRAG_DROP
        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            if (cm.cfg.Chunks.Count == 0 || !cm.cfg.Chunks.Last().Value.Vacant)
            {
                createNewToolStripMenuItem_Click(this, null);
            }
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string s in fileList)
            {
                OperateFileImport(s);
            }
        }
        #endregion

        #region TIMERS
        protected int spiderTick = 0;
        private void SpiderTick(object sender, EventArgs e)
        {
            string spider = "";
            if (spiderTick == 4) spiderTick = 0;
            switch (spiderTick)
            {
                case 0:
                    spider = "-";
                    break;
                case 1:
                    spider = "\\";
                    break;
                case 2:
                    spider = "|";
                    break;
                case 3:
                    spider = "/";
                    break;
            }
            spiderTick++;
            spiderStatusLabel.Text = spider;
        }
        private void interceptorTimer_Tick(object sender, EventArgs e)
        {

            if (!Directory.Exists("incerceptor_dir"))
            {
                Directory.CreateDirectory("incerceptor_dir");
            }
            DirectoryInfo d = new DirectoryInfo("incerceptor_dir");
            FileInfo[] Files = d.GetFiles("*");
            foreach (FileInfo fi in Files)
            {
                StoreFile(fi, "Intercepted", "queue");
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public async void CreateWindow<T>(string name, bool save_after_display = false, bool refreshCounters = false, params object[] args) where T : Form
        {
            await Task.Run(() =>
            {
                var instance = Activator.CreateInstance(typeof(T),
                    args) as T;

                var window = (T)wm.putWindow(name, instance);

                window.ShowDialog();
                try
                {
                }
                catch (Exception ex)
                {
                    throw ex;
                    MessageBox.Show(
                        "Window suffered an exception and needs to be disposed");
                    window.Dispose();
                }
                if (save_after_display) cm.saveConfig();
                wm.destroyWindow(name);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            });
            if (refreshCounters) displayCounters();
        }
        public void NewChunk()
        {
            var key = ChunkOperations.newChunk(ref cm);
            displayCounters();
        }
        public bool StoreFile(FileInfo fi, string comment, string superfamily)
        {
            var vac = false;
            foreach (var c in cm.cfg.Chunks)
            {
                if (c.Value.Vacant)
                {
                    vac = true;
                }
            }
            if (!vac)
            {
                NewChunk();
            }
            foreach (var c in cm.cfg.Chunks)
            {
                if (c.Value.Vacant)
                {
                    var filename = fi.Name;
                    var filecode = RandomData.RandomString(14);
                    var ft = cm.determineFileType(Path.GetExtension(filename));
                    if (ft == null) ft = cm.getFileType("def_unkn");
                    if (fi.Name.Contains("crdownload")) continue;
                    var fd = new FileDeclarator
                    {
                        ChunkKey = c.Key,
                        FileTypeKey = ft.TypeKey,
                        Comment = comment,
                        FileId = filecode,
                        OriginalPath = fi.FullName,
                        SuperfamilyKey = superfamily,
                        TagKeys = new List<string>(),
                        Title = filename
                    };
                    var status = ChunkOperations.writeToChunkEnd(c.Key, filecode, FileOperations.readAllBytes(fi.FullName), ref cm);
                    if (status)
                    {
                        cm.cfg.FileDeclarators.Add(fd.FileId, fd);
                        File.Delete(fi.FullName);
                        actionLabel.Text = "Intercepted and deleted " + fi.Name;
                        Console.WriteLine("\a");
                        cm.saveConfig();
                        targetedCBLToolStripMenuItem_Click(null, null);
                        return true;
                    }
                }
            }
            return false;
        }
        public async void StoreFileFromUrl(string url, string comment, string superfamily)
        {
            await Task.Run(() =>
            {
                var data = Net.GetBytes(url);
                var filename = url.Substring(url.LastIndexOf('/') + 1);
                var filecode = RandomData.RandomString(14);
                var ft = cm.determineFileType(Path.GetExtension(filename));
                if (ft == null) return;
                bool vac = false;
                foreach (var c in cm.cfg.Chunks)
                {
                    if (c.Value.Vacant)
                    {
                        vac = true;
                    }
                }
                if (!vac)
                {
                    NewChunk();
                }
                foreach (var c in cm.cfg.Chunks)
                {
                    if (c.Value.Vacant)
                    {
                        var fd = new FileDeclarator
                        {
                            ChunkKey = c.Key,
                            FileTypeKey = ft.TypeKey,
                            Comment = comment,
                            FileId = filecode,
                            OriginalPath = url,
                            SuperfamilyKey = superfamily,
                            TagKeys = new List<string>(),
                            Title = filename
                        };
                        var status = ChunkOperations.writeToChunkEnd(c.Key, filecode, data, ref cm);
                        if (status)
                        {
                            cm.cfg.FileDeclarators.Add(fd.FileId, fd);
                            cm.saveConfig();
                        }
                    }
                }
            });
            Console.WriteLine("\a");
        }
        public static string ScrubHtml(string value)
        {
            if (value != null)
            {
                value = value.Replace("<br>", "\n");
                value = value.Replace("#039;", "'");
                var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
                var step2 = Regex.Replace(step1, @"\s{2,}", " ");
                step2 = step2.Replace("&gt;", ">");
                return step2;
            }
            else return "";
        }
        public void ArchiveToQueue(string url, string comment)
        {
            StoreFileFromUrl(url, comment, "queue");
            runTargetedCBL();
        }
        public byte[] GetDataFromNet(string url)
        {
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(
                url);
            lxRequest.Proxy = GlobalProxySelection.GetEmptyWebProxy();
            Byte[] lsResponse;
            using (HttpWebResponse lxResponse = (HttpWebResponse)lxRequest.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    lsResponse = reader.ReadBytes(1 * 1024 * 1024 * 10);
                }
            }
            return lsResponse;
        }
        #endregion

        #region PUBLIC_UI_METHODS
        public void spinTheSpider()
        {
            spiderWork.Start();
        }
        public void stopTheSpider()
        {
            spiderWork.Stop();
        }
        public void refreshCounters()
        {
            this.Invoke((MethodInvoker)(() =>
            {
                displayCounters();
            }));
        }
        public void runTargetedCBL()
        {
            this.Invoke((MethodInvoker)(() =>
            {
                targetedCBLToolStripMenuItem_Click(null, null);
            }));
        }
        #endregion

        private void freezeUI()
        {
            menuStrip1.Enabled = false;
            openFileBrowserButton.Enabled = false;
        }
        private void unfreezeUI()
        {
            menuStrip1.Enabled = true;
            openFileBrowserButton.Enabled = true;
        }

        private void OperateFileImport(string s) // s = path
        {
            string ext = Path.GetExtension(s);
            var ft = cm.determineFileType(ext);
            if (ft != null)
            {
                Task.Run(() =>
                {
                    var sfai = new StoreFileAsIs(cm, s, ft);
                    var result = sfai.ShowDialog();
                });
                return;
            }
        }
        private void OpenFileBrowserButton_Click(object sender, EventArgs e)
        {
            CreateWindow<FileBrowser>("filebrowser", true, true, this, cm);
        }


        private List<long> memoryStopList = new List<long>();
        private void memoryTimer_Tick(object sender, EventArgs e)
        {

            try
            {
                {
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }













            if (memoryStopList.Count > 50)
            {
                toolStripProgressBar1.Maximum = unchecked((int)memoryStopList.Max() / 1000);
                memoryStopList.Clear();
            }
            long mem = Process.GetCurrentProcess().PrivateMemorySize64;
            memoryStopList.Add(mem);
            memoryStatusLabel.Text = Utility.Humanizer.BytesToString(mem);
            var lastval = unchecked((int)memoryStopList.Last() / 1000);
            if (lastval > toolStripProgressBar1.Maximum) toolStripProgressBar1.Maximum = lastval;
            toolStripProgressBar1.Value = lastval;
        }
    }
}
