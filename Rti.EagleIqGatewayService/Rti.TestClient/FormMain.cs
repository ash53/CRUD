using System;

using System.Windows.Forms;
using Rti.InternalInterfaces.ServiceProxies;

namespace Rti.TestClient
{
    public partial class FormMain : Form
    {
        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            // New proxy model using common dynamic proxy
            using (var eiqProxy = new EagleIqGatewayClient("net.tcp://localhost:8044/Rti.EagleIqGatewayService/", "netTcpLocal"))
            {
                if (eiqProxy.IsAlive())
                {
                    richTextBox1.AppendText("True" + Environment.NewLine);
                }
                else
                {
                    richTextBox1.AppendText("False" + Environment.NewLine);
                }
            }
        }



    }
}
