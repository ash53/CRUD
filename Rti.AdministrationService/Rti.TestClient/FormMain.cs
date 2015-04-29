using System;
using System.Windows.Forms;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.TestClient
{
    public partial class FormMain : Form
    {
        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();
        private readonly string _domainName = CommonFunctions.GetDomainName();
        readonly AdministrationServiceClient _administrationService = new AdministrationServiceClient( "NetTcpBinding_IAdministrationService" );

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonLoginToEmWare_Click(object sender, EventArgs e)
        {
            var permissions = _administrationService.EmWareCollectionLogin(_stationName, _userName,
                new Login.UserCredentials(textBoxUnixUsername.Text, textBoxUnixPassword.Text, _domainName));

            richTextBox1.AppendText("LoginSuccessful:[" + permissions.LoginSuccessful + "]["  + Environment.NewLine);
            richTextBox1.AppendText("IDM.CanAnnotate:[" + permissions.IDM.CanAnnotate + "][" + Environment.NewLine);
            richTextBox1.AppendText("Assembly.CanDelete:[" + permissions.Assembly.CanDelete + "][" + Environment.NewLine);
            richTextBox1.AppendText("WorkFlow.IsSupervisor:[" + permissions.WorkFlow.IsSupervisor + "][" + Environment.NewLine);
        }
    }
}
