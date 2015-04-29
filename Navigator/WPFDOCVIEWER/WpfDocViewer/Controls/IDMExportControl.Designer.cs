namespace WpfDocViewer.Controls
{
    partial class IDMExportControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDMExportControl));
            this.axTTLogin1 = new AxTTLogin.AxTTLogin();
            this.axTTDocOp1 = new AxTTDOCOPLib.AxTTDocOp();
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTDocOp1)).BeginInit();
            this.SuspendLayout();
            // 
            // axTTLogin1
            // 
            this.axTTLogin1.Enabled = true;
            this.axTTLogin1.Location = new System.Drawing.Point(12, 13);
            this.axTTLogin1.Name = "axTTLogin1";
            this.axTTLogin1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTLogin1.OcxState")));
            this.axTTLogin1.Size = new System.Drawing.Size(32, 32);
            this.axTTLogin1.TabIndex = 0;
            // 
            // axTTDocOp1
            // 
            this.axTTDocOp1.Enabled = true;
            this.axTTDocOp1.Location = new System.Drawing.Point(9, 57);
            this.axTTDocOp1.Name = "axTTDocOp1";
            this.axTTDocOp1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTDocOp1.OcxState")));
            this.axTTDocOp1.Size = new System.Drawing.Size(32, 32);
            this.axTTDocOp1.TabIndex = 1;
            // 
            // IDMExportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axTTDocOp1);
            this.Controls.Add(this.axTTLogin1);
            this.Name = "IDMExportControl";
            this.Size = new System.Drawing.Size(190, 157);
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTTDocOp1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxTTLogin.AxTTLogin axTTLogin1;
        private AxTTDOCOPLib.AxTTDocOp axTTDocOp1;

    }
}
