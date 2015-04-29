namespace Rti.TestClient
{
    partial class FormMain
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBoxUnixUsername = new System.Windows.Forms.TextBox();
            this.textBoxUnixPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonLoginToEmWare = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(170, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(249, 126);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // textBoxUnixUsername
            // 
            this.textBoxUnixUsername.Location = new System.Drawing.Point(30, 67);
            this.textBoxUnixUsername.Name = "textBoxUnixUsername";
            this.textBoxUnixUsername.Size = new System.Drawing.Size(100, 20);
            this.textBoxUnixUsername.TabIndex = 3;
            // 
            // textBoxUnixPassword
            // 
            this.textBoxUnixPassword.Location = new System.Drawing.Point(30, 118);
            this.textBoxUnixPassword.Name = "textBoxUnixPassword";
            this.textBoxUnixPassword.Size = new System.Drawing.Size(100, 20);
            this.textBoxUnixPassword.TabIndex = 4;
            this.textBoxUnixPassword.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password";
            // 
            // buttonLoginToEmWare
            // 
            this.buttonLoginToEmWare.Location = new System.Drawing.Point(12, 12);
            this.buttonLoginToEmWare.Name = "buttonLoginToEmWare";
            this.buttonLoginToEmWare.Size = new System.Drawing.Size(137, 23);
            this.buttonLoginToEmWare.TabIndex = 7;
            this.buttonLoginToEmWare.Text = "LoginToEmWare";
            this.buttonLoginToEmWare.UseVisualStyleBackColor = true;
            this.buttonLoginToEmWare.Click += new System.EventHandler(this.buttonLoginToEmWare_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 166);
            this.Controls.Add(this.buttonLoginToEmWare);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUnixPassword);
            this.Controls.Add(this.textBoxUnixUsername);
            this.Controls.Add(this.richTextBox1);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBoxUnixUsername;
        private System.Windows.Forms.TextBox textBoxUnixPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonLoginToEmWare;
    }
}

