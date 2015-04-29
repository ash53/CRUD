using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rti.EncryptionLib;
using System.Configuration;

namespace Rti.CRUDTool
{
    public partial class Credential : Form
    {
        public Credential()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var encryptor = new EncryptionFuncs();
            string uName = ConfigurationManager.AppSettings["setting1"];
            string code = ConfigurationManager.AppSettings["setting2"];
            string decryptedString = encryptor.Decrypt(code);
            if (txtInUser.Text.ToString() == uName && txtInPass.Text.ToString() == decryptedString)
            {
                this.DialogResult = DialogResult.OK;
            }

            else
            {
                txtInPass.Clear();
                MessageBox.Show("User name or password is wrong.");
                txtInPass.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
