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
using BrightIdeasSoftware;
using xPDB.Models.Storage;
using xPDB.Storage;
using xPDB.Windows.Helpers;

namespace xPDB.Windows
{
    public partial class FileBrowser : Form
    {
        private ConfigManager cm;

        internal FileDeclarator CurrentFile;
        internal MainWindow ParentWindow;

        public FileBrowser(MainWindow parent, ref ConfigManager cm)
        {
            this.cm = cm;
            this.ParentWindow = parent;
            InitializeComponent();
        }

        private void FileBrowser_Load(object sender, EventArgs e)
        {
            fileListView.SetObjects(cm.cfg.FileDeclarators.Values);
        }

        private void fileListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count == 1)
            {
                CurrentFile = fileListView.SelectedObject as FileDeclarator;
                displayFileDeclarator();
            }
        }

        private void displayFileDeclarator()
        {
            textBox1.Text = CurrentFile.FileId;
            textBox2.Text = CurrentFile.ChunkKey;
            textBox3.Text = CurrentFile.FileTypeKey;
            textBox4.Text = CurrentFile.OriginalPath;
            textBox5.Text = CurrentFile.Title;
            textBox6.Text = CurrentFile.Comment;
            textBox7.Text = CurrentFile.SuperfamilyKey;
            if (cm.doesFileTypeExist(CurrentFile.FileTypeKey))
            {
                var ft = cm.getFileType(CurrentFile.FileTypeKey);
                if (ft._FileType == FileTypes.Image)
                {
                    var chunk = cm.getChunk(CurrentFile.ChunkKey);
                    if (!chunk.FileOffsets.ContainsKey(CurrentFile.FileId))
                    {
                        ParentWindow.runTargetedCBL();
                        MessageBox.Show("Please wait a second, recalculation is needed. Wait for the spider on the main window to stop before clicking OK");
                    }
                    var s = new MemoryStream(ChunkOperations.readChunkPosition(chunk.ChunkId,
                        CurrentFile.FileId, ref cm));

                    previewBox.Image = Image.FromStream(s);
                }
            }
        }

        private void previewBox_Click(object sender, EventArgs e)
        {
            var iv = new ImageView(previewBox.Image);
            iv.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("temp"))
            {
                Directory.CreateDirectory("temp");
            }

        }
    }
}
