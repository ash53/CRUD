using System.Configuration;
using System.Drawing;
using DocumentViewer.Properties;

namespace DocumentViewer
{
    partial class IDMViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Author: Mark Lane
        /// using application settings to store last used locations
        /// to be reused next time they start the application.
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            /*if (Top >= 0)
            {
                Point point = new Point(ActiveForm.Top, ActiveForm.Left);
                Settings.Default.LastLocation = point;
            }
            if ((IDMViewer.ActiveForm.Width > 200) && (IDMViewer.ActiveForm.Height > 500))
            {
                Size szSize = new Size(IDMViewer.ActiveForm.Height, IDMViewer.ActiveForm.Width);
                Settings.Default.StoreSize = szSize;
            }
             * */
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDMViewer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.axTTDoc1 = new AxTTDoc.AxTTDoc();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.annotationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawHighlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.axTTDocOp1 = new AxTTDOCOPLib.AxTTDocOp();
            this.axTTLogin1 = new AxTTLogin.AxTTLogin();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTTDoc1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTTDocOp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.axTTDoc1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(740, 810);
            this.panel1.TabIndex = 0;
            // 
            // axTTDoc1
            // 
            this.axTTDoc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTTDoc1.Location = new System.Drawing.Point(0, 0);
            this.axTTDoc1.Name = "axTTDoc1";
            this.axTTDoc1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTDoc1.OcxState")));
            this.axTTDoc1.Size = new System.Drawing.Size(740, 810);
            this.axTTDoc1.TabIndex = 0;
            this.axTTDoc1.PositionChanged += new System.EventHandler(this.axTTDoc1_PositionChanged);
            this.axTTDoc1.SelectionChanged += new System.EventHandler(this.axTTDoc1_SelectionChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.annotationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(740, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.printToolStripMenuItem.Text = "Print";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // annotationsToolStripMenuItem
            // 
            this.annotationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawHighlightToolStripMenuItem});
            this.annotationsToolStripMenuItem.Name = "annotationsToolStripMenuItem";
            this.annotationsToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.annotationsToolStripMenuItem.Text = "Annotations";
            // 
            // drawHighlightToolStripMenuItem
            // 
            this.drawHighlightToolStripMenuItem.Name = "drawHighlightToolStripMenuItem";
            this.drawHighlightToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.drawHighlightToolStripMenuItem.Text = "Draw Highlight";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.axTTLogin1);
            this.panel2.Controls.Add(this.axTTDocOp1);
            this.panel2.Location = new System.Drawing.Point(21, 769);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(131, 22);
            this.panel2.TabIndex = 1;
            // 
            // axTTDocOp1
            // 
            this.axTTDocOp1.Enabled = true;
            this.axTTDocOp1.Location = new System.Drawing.Point(3, 3);
            this.axTTDocOp1.Name = "axTTDocOp1";
            this.axTTDocOp1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTDocOp1.OcxState")));
            this.axTTDocOp1.Size = new System.Drawing.Size(32, 32);
            this.axTTDocOp1.TabIndex = 0;
            // 
            // axTTLogin1
            // 
            this.axTTLogin1.Enabled = true;
            this.axTTLogin1.Location = new System.Drawing.Point(43, 4);
            this.axTTLogin1.Name = "axTTLogin1";
            this.axTTLogin1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTLogin1.OcxState")));
            this.axTTLogin1.Size = new System.Drawing.Size(32, 32);
            this.axTTLogin1.TabIndex = 1;
            // 
            // IDMViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 834);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("StartPosition", global::DocumentViewer.Properties.Settings.Default, "LastPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::DocumentViewer.Properties.Settings.Default, "LastLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Location = global::DocumentViewer.Properties.Settings.Default.LastLocation;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "IDMViewer";
            this.StartPosition = global::DocumentViewer.Properties.Settings.Default.LastPosition;
            this.Text = "Document Viewer";
            this.Load += new System.EventHandler(this.IDMViewer_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTTDoc1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTTDocOp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void axTTDoc1_SelectionChanged(object sender, System.EventArgs e)
        {
            //convert twips to pixels
        }

        void axTTDoc1_PositionChanged(object sender, System.EventArgs e)
        {
            //code for Prepping.
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private AxTTDoc.AxTTDoc axTTDoc1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem annotationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawHighlightToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private AxTTLogin.AxTTLogin axTTLogin1;
        private AxTTDOCOPLib.AxTTDocOp axTTDocOp1;
    }
}

