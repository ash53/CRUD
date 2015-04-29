//
// Samia's RAC Test's 
// 

using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rti;
using Rti.AdministrationServer;
using Rti.DataModel;
using Rti.DocumentManagerServer.Servers;
using Rti.EagleIqGatewayServer.Servers;
using Rti.InternalInterfaces.DataContracts;
using Rti.ExternalInterfaces.DataContracts;
using Rti.RemoteProcedureCalls.Embillz;
using Rti.InternalInterfaces.ServiceProxies;

namespace RACServicesUnitTests
{
    public partial class AdministrationServiceUnitTests
    {
        [TestMethod]
        public void IsAlive04()
        {
            var administrationServer = new AdministrationServer();
            Assert.IsTrue(administrationServer.IsAlive());
        }
        //sam4
        [TestMethod]
        public void Login04()
        {
            var domain1 = Rti.CommonFunctions.GetDomainName();
            var machine1 = Rti.CommonFunctions.GetMachineName();
            var user1 = Rti.CommonFunctions.GetUserName();

            Debug.Print("machine:[{0}]", machine1);
            Debug.Print("domain:[{0}]", domain1);
            Debug.Print("username:[{0}]", user1);

            var unixPassword = Rti.CommonFunctions.PromptForInputDialog("Navigator Login", "Enter Unix Password:", "", true);

            var administrationServer1 = new AdministrationServer();
            var credentials1 = new Login.UserCredentials(user1, unixPassword, domain1);
            var permissions1 = administrationServer1.EmWareCollectionLogin(machine1, user1, credentials1);

            Debug.Print("Return value LoginSuccessful:[{0}]", permissions1.LoginSuccessful.ToString());
            Debug.Print("Return value LoginMessage:[{0}]", permissions1.LoginMessage.ToString());

            Debug.Print(Rti.CommonFunctions.ObjectToXml(permissions1));

            Assert.IsTrue(permissions1.LoginSuccessful);
        }

    }
    //sam3
    [TestClass]
    public partial class EagleIqGatewayUnitTests04
    {
        [TestMethod]
        public void IsAlive04()
        {
            var eagleIqGateway = new EagleIqGatewayServer();
            Assert.IsTrue(eagleIqGateway.IsAlive());
        }
        
       //sam2
        [TestMethod]
        public void ApproveMatch04()
        {
            var eagleIqGateway1 = new EagleIqGatewayServer();
            var result = eagleIqGateway1.ApproveMatch(
                new RpcInMessage()
                {
                    Context = ""
                    ,
                    UserName = CommonFunctions.GetUserName()
                    ,
                    Workstation = CommonFunctions.GetMachineName()
                }
                , "samia"
                , 20151201);
            if (result != null)
            {
                if (!result.IsSuccess)
                {
                    Debug.Print(result.OutMessage);
                }
                Assert.IsTrue(result.IsSuccess);
            }
        }
    }
    public partial class RtiDataModelUnitTests
    {
        [TestMethod]
        public void TowerDatabaseQuery04()
        {
            using (var towerContext = new TowerModelContainer())
            {
                var results = towerContext.RTICLIENTs.Where(x => x.RTICLIENT_ID > 0);
                Assert.IsTrue(results.Any());
            }
        }
        //sam05
       [TestMethod]
        public void Cadadmindatabaseconnection04()
        {    using (var cadAdminContext = new CadAdminModelContainer())
            {
                var results = cadAdminContext.HL7_FTP_CONFIG.Where(x => x.HL7_FTP_CONFIG_ID > 0);
                Assert.IsTrue(results.Any());
            }
        } 
    }

    public partial class DocumentManagerServiceIntegrationTests
    {
        [TestMethod]
        public void SearchPostBdRemote04()
        {
            var docType = "CK";
            var docDet = "AD";
            using (
                var docManagerServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var embillzSearchData = docManagerServiceClient.SearchPostBd(Environment.MachineName,
                    Environment.UserName, docType: docType, docDet: docDet);
                VerifyUtils.VerifyDataTable(embillzSearchData);
            }
        }
        //sam1
        [TestMethod]
        public void IsAlive04()
        {
            var docManagerServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                   VerifyUtils.DocMgrRemoteEndpointName);
            Assert.IsTrue(docManagerServiceClient.IsAlive());
        }
    }
}
