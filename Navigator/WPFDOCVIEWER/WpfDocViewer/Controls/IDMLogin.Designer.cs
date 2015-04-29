namespace WpfDocViewer.Controls
{
    partial class IDMLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDMLogin));
            this.axTTLogin1 = new AxTTLogin.AxTTLogin();
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).BeginInit();
            this.SuspendLayout();
            // 
            // axTTLogin1
            // 
            this.axTTLogin1.Enabled = true;
            this.axTTLogin1.Location = new System.Drawing.Point(33, 23);
            this.axTTLogin1.Name = "axTTLogin1";
            this.axTTLogin1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTTLogin1.OcxState")));
            this.axTTLogin1.Size = new System.Drawing.Size(32, 32);
            this.axTTLogin1.TabIndex = 0;
            // 
            // IDMLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axTTLogin1);
            this.Name = "IDMLogin";
            this.Size = new System.Drawing.Size(91, 106);
            ((System.ComponentModel.ISupportInitialize)(this.axTTLogin1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxTTLogin.AxTTLogin axTTLogin1;

    }
}
