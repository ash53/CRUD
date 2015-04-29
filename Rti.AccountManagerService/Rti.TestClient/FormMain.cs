using System;
using System.Windows.Forms;

namespace Rti.TestClient
{
    public partial class FormMain : Form
    {
        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();
        readonly AccountManagerServiceClient _accountManagerService = new AccountManagerServiceClient( "NetTcpBinding_IAccountManagerService" );

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_accountManagerService.IsAlive())
                {
                    richTextBox1.AppendText("True" + Environment.NewLine);
                }
                else
                {
                    richTextBox1.AppendText("False" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText("False" + Environment.NewLine);
                richTextBox1.AppendText(ex.ToString() + Environment.NewLine);
            }
        }
    }
}
