﻿namespace xPDB.Windows
{
    partial class ImageExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignToSuperfamilyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.excludeView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.pbContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 574);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1185, 31);
            this.panel1.TabIndex = 4;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(361, 5);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(60, 21);
            this.button6.TabIndex = 13;
            this.button6.Text = "Clear all";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(308, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(47, 22);
            this.button5.TabIndex = 12;
            this.button5.Text = "Apply";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(224, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 22);
            this.button4.TabIndex = 11;
            this.button4.Text = "Exclude filter";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(140, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 22);
            this.button3.TabIndex = 10;
            this.button3.Text = "Include filter";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox2.BackColor = System.Drawing.SystemColors.MenuText;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox2.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "30",
            "60",
            "120",
            "240",
            "480",
            "Full screen"});
            this.comboBox2.Location = new System.Drawing.Point(3, 5);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(131, 21);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.Text = "240";
            this.comboBox2.TextChanged += new System.EventHandler(this.comboBox2_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.BackColor = System.Drawing.SystemColors.MenuText;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1065, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(53, 21);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(944, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(1067, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1124, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(25, 21);
            this.button2.TabIndex = 5;
            this.button2.Text = "<";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1155, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 21);
            this.button1.TabIndex = 4;
            this.button1.Text = ">";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1185, 574);
            this.panel2.TabIndex = 5;
            // 
            // pbContextMenuStrip
            // 
            this.pbContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tagToolStripMenuItem,
            this.addTagsToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.assignToSuperfamilyToolStripMenuItem});
            this.pbContextMenuStrip.Name = "pbContextMenuStrip";
            this.pbContextMenuStrip.Size = new System.Drawing.Size(189, 92);
            this.pbContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.pbContextMenuStrip_Opening);
            // 
            // tagToolStripMenuItem
            // 
            this.tagToolStripMenuItem.Name = "tagToolStripMenuItem";
            this.tagToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.tagToolStripMenuItem.Text = "Tag";
            this.tagToolStripMenuItem.Click += new System.EventHandler(this.tagToolStripMenuItem_Click);
            // 
            // addTagsToolStripMenuItem
            // 
            this.addTagsToolStripMenuItem.Name = "addTagsToolStripMenuItem";
            this.addTagsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.addTagsToolStripMenuItem.Text = "Add tags";
            this.addTagsToolStripMenuItem.Click += new System.EventHandler(this.addTagsToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // assignToSuperfamilyToolStripMenuItem
            // 
            this.assignToSuperfamilyToolStripMenuItem.Name = "assignToSuperfamilyToolStripMenuItem";
            this.assignToSuperfamilyToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.assignToSuperfamilyToolStripMenuItem.Text = "Assign to superfamily";
            // 
            // includeView
            // 
            this.includeView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.includeView.BackColor = System.Drawing.Color.Black;
            this.includeView.CheckBoxes = true;
            this.includeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.includeView.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.includeView.Location = new System.Drawing.Point(37, 3);
            this.includeView.Name = "includeView";
            this.includeView.Size = new System.Drawing.Size(425, 565);
            this.includeView.TabIndex = 0;
            this.includeView.UseCompatibleStateImageBehavior = false;
            this.includeView.View = System.Windows.Forms.View.Details;
            this.includeView.Visible = false;
            this.includeView.SelectedIndexChanged += new System.EventHandler(this.includeView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Include tags";
            this.columnHeader1.Width = 417;
            // 
            // excludeView
            // 
            this.excludeView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.excludeView.BackColor = System.Drawing.Color.Black;
            this.excludeView.CheckBoxes = true;
            this.excludeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.excludeView.ForeColor = System.Drawing.Color.Red;
            this.excludeView.Location = new System.Drawing.Point(52, 34);
            this.excludeView.Name = "excludeView";
            this.excludeView.Size = new System.Drawing.Size(425, 534);
            this.excludeView.TabIndex = 1;
            this.excludeView.UseCompatibleStateImageBehavior = false;
            this.excludeView.View = System.Windows.Forms.View.Details;
            this.excludeView.Visible = false;
            this.excludeView.SelectedIndexChanged += new System.EventHandler(this.excludeView_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Exclude tags";
            this.columnHeader2.Width = 416;
            // 
            // ImageExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1185, 605);
            this.Controls.Add(this.excludeView);
            this.Controls.Add(this.includeView);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ImageExplorer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageExplorer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageExplorer_FormClosing);
            this.Load += new System.EventHandler(this.ImageExplorer_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pbContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ContextMenuStrip pbContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tagToolStripMenuItem;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListView excludeView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView includeView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ToolStripMenuItem addTagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assignToSuperfamilyToolStripMenuItem;
    }
}