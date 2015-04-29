using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.TestClient
{
    public partial class FormMain : Form
    {
        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();
        readonly DocumentManagerServiceClient _documentManagerService = 
            new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME);

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_documentManagerService.IsAlive())
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

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _documentManagerService.GetPostDocByParDocNo(_stationName, _userName, textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var client = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                dataGridView1.DataSource = client.ListPractices(_stationName, _userName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string outMessage = "";
            if (_documentManagerService.ConvertPDFtoTif(_stationName,
                _userName,
                textBoxPDF.Text,
                out outMessage))
            {
                MessageBox.Show("Tif Created");
            }
            else
            {
                // Outmessage will have error message on false
                MessageBox.Show("[" + outMessage + "]");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _documentManagerService.GetMapAndActivityByLoginName(_stationName, _userName, textBoxLoginName.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var formattedDocNo = _documentManagerService.CreateDocNo(_stationName, _userName);
            MessageBox.Show(formattedDocNo);
        }

        private void PostDetailInsert_Click(object sender, EventArgs e)
        {
            var postDetail = new NavPostDetail();

            postDetail.CREATEUID = "cbunders";
            postDetail.TOWID = 9980;
            
            if (_documentManagerService.InsertIntoPostDetail(_stationName, _userName,
                postDetail) > 0)
            {
                MessageBox.Show("Record inserted");
            }
            else
            {
                MessageBox.Show("Fail");
            }
        }

    }
}
