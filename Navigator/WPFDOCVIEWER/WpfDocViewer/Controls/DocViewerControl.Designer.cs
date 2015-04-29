namespace WpfDocViewer.Controls
{
    partial class DocViewerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocViewerControl));
            this.annotationToolStrip = new System.Windows.Forms.ToolStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.axTTDoc = new AxTTDoc.AxTTDoc();
            this.axTTDocOp1 = new AxTTDOCOPLib.AxTTDocOp();
            this.axTTLogin1 = new AxTTLogin.AxTTLogin();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTTDoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTDocOp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).BeginInit();
            this.SuspendLayout();
            // 
            // annotationToolStrip
            // 
            this.annotationToolStrip.Location = new System.Drawing.Point(0, 0);
            this.annotationToolStrip.Name = "annotationToolStrip";
            this.annotationToolStrip.Size = new System.Drawing.Size(151, 25);
            this.annotationToolStrip.TabIndex = 3;
            this.annotationToolStrip.Text = "toolStrip1";
            this.annotationToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.annotationToolStrip_ItemClicked);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.axTTLogin1);
            this.panel1.Controls.Add(this.axTTDocOp1);
            this.panel1.Controls.Add(this.axTTDoc);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(151, 316);
            this.panel1.TabIndex = 4;
            // 
            // axTTDoc
            // 
            this.axTTDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTTDoc.Location = new System.Drawing.Point(0, 0);
            this.axTTDoc.Name = "axTTDoc";
            this.axTTDoc.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTDoc.OcxState")));
            this.axTTDoc.Size = new System.Drawing.Size(151, 316);
            this.axTTDoc.TabIndex = 3;
            // 
            // axTTDocOp1
            // 
            this.axTTDocOp1.Enabled = true;
            this.axTTDocOp1.Location = new System.Drawing.Point(116, 243);
            this.axTTDocOp1.Name = "axTTDocOp1";
            this.axTTDocOp1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTDocOp1.OcxState")));
            this.axTTDocOp1.Size = new System.Drawing.Size(32, 32);
            this.axTTDocOp1.TabIndex = 4;
            // 
            // axTTLogin1
            // 
            this.axTTLogin1.Enabled = true;
            this.axTTLogin1.Location = new System.Drawing.Point(116, 281);
            this.axTTLogin1.Name = "axTTLogin1";
            this.axTTLogin1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTLogin1.OcxState")));
            this.axTTLogin1.Size = new System.Drawing.Size(32, 32);
            this.axTTLogin1.TabIndex = 5;
            // 
            // DocViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.annotationToolStrip);
            this.Name = "DocViewerControl";
            this.Size = new System.Drawing.Size(151, 341);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTTDoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTDocOp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        

        #endregion

        private System.Windows.Forms.ToolStrip annotationToolStrip;
        private System.Windows.Forms.Panel panel1;
        private AxTTDoc.AxTTDoc axTTDoc;
        private AxTTDOCOPLib.AxTTDocOp axTTDocOp1;
        private AxTTLogin.AxTTLogin axTTLogin1;
    }
}
