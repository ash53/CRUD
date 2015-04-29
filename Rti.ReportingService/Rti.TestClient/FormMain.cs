using System;
using System.Windows.Forms;

namespace Rti.TestClient
{
    public partial class FormMain : Form
    {
        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();
        readonly ReportingServiceClient _reportingService = new ReportingServiceClient( "NetTcpBinding_IReportingService" );

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_reportingService.IsAlive())
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
