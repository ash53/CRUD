using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DocumentViewer
{
    public partial class IDMTester : Form
    {
        public IDMTester()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            IDMViewer doc1 = new IDMViewer();
            doc1.IdmLogin(textBox1.Text, textBox2.Text, textBox3.Text);

        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            IDMViewer doc1 = new IDMViewer();
            doc1.Visible = true;
            doc1.DrawDoc(textBox4.Text, Convert.ToInt16(textBox5.Text));
        
        }
    }
}
