//
// Shariq's RAC Test's
// 

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rti.AdministrationServer;
using Rti.DataModel;
using Rti.InternalInterfaces.ServiceProxies;

namespace RACServicesUnitTests
{
    public partial class AdministrationServiceUnitTests
    {
        [TestMethod]
        public void IsAlive05()
        {
            var administrationServer = new AdministrationServer();
            Assert.IsTrue(administrationServer.IsAlive());
        }

        //Updated By Shariq -- Remote Server Test              
        private const string RemoteServer = "apps.rti.com";

        [TestMethod]
        public void AdministrationServiceIsAlive051()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var adminServiceClient = new AdministrationServiceClient(VerifyUtils.AdminRemoteUrl,
                    VerifyUtils.AdminRemoteEndpointName))
            {
                Assert.IsTrue(adminServiceClient.IsAlive());
            }
        }

        //Updated By Shariq -- Remote Server Test              
        [TestMethod]
        public void StopService051()
        {
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();
            //var servicename = "RtiAdministrationService";
            //var servicename = "RtiDocumentManagerService";
            var servicename = "RtiEagleIqService";

            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var adminServiceClient = new AdministrationServiceClient(VerifyUtils.AdminRemoteUrl,
                    VerifyUtils.AdminRemoteEndpointName))
            {
                var result = adminServiceClient.StopService(machine, user, servicename);

                Assert.IsTrue(result);
            }
        }

        //Updated By Shariq -- Remote Server Test              
        [TestMethod]
        public void StartService051()
        {
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();
            //var servicename = "RtiAdministrationService";
            //var servicename = "RtiDocumentManagerService";
            var servicename = "RtiEagleIqService";

            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var adminServiceClient = new AdministrationServiceClient(VerifyUtils.AdminRemoteUrl,
                    VerifyUtils.AdminRemoteEndpointName))
            {
                var result = adminServiceClient.StartService(machine, user, servicename);

                Assert.IsTrue(result);
            }
        }

        //Updated By Shariq -- Remote Server Test              
        [TestMethod]
        public void ListServicesStatus051()
        {
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();

            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var adminServiceClient = new AdministrationServiceClient(VerifyUtils.AdminRemoteUrl,
                    VerifyUtils.AdminRemoteEndpointName))
            {
                var results = adminServiceClient.ListServicesStatus(machine, user);

                Assert.IsTrue(results.Rows.Count > 0);
            }
        }

        //Updated By Shariq -- Local PC Test
        [TestMethod]
        public void StopService052()
        {
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();
            var servicename = "RtiAdministrationService";
            //var servicename = "RtiDocumentManagerService";
            //var servicename = "RtiEagleIqService";

            var administrationServer = new AdministrationServer();
            var result = administrationServer.StopService(machine, user, servicename);

            Assert.IsTrue(result);
        }

        //Updated By Shariq -- Local PC Test
        [TestMethod]
        public void StartService052()
        {
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();
            var servicename = "RtiAdministrationService";
            //var servicename = "RtiDocumentManagerService";
            //var servicename = "RtiEagleIqService";

            var administrationServer = new AdministrationServer();
            var result = administrationServer.StartService(machine, user, servicename);
            //administrationServer.StartService
            Assert.IsTrue(result);
        }

        //Updated By Shariq -- Local PC Test
        [TestMethod]
        public void ListServicesStatus052()
        {
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();

            var administrationServer = new AdministrationServer();
            var results = administrationServer.ListServicesStatus(machine, user);

            Assert.IsTrue(results.Rows.Count > 0);
        }
    }

    public partial class RtiDataModelUnitTests
    {
        [TestMethod]
        public void TowerDatabaseQuery05()
        {
            using (var towerContext = new TowerModelContainer())
            {
                var results = towerContext.RTICLIENTs.Where(x => x.RTICLIENT_ID > 0);
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        public void CadAdminDatabaseQuery051()
        {
            using(var CadAdminContext=new CadAdminModelContainer())
            {
                var results = CadAdminContext.TOWERIMPORTCLIENTs.Where(x => x.TOWERIMPORTCLIENT_ID > 0);
                Assert.IsTrue(results.Any());                             
            }
        }
        
        [TestMethod]
        public void CadAdminDatabaseQuery052()
        {
            using(var CadAdminContext=new CadAdminModelContainer())
            {
                var results = CadAdminContext.TOWERIMPORTCLIENTs.Where(x => x.PRACTICE.Contains("GMY18"));
                Assert.IsTrue(results.Any());                
            }             
        }

        [TestMethod]
        public void CadAdminDatabaseQuery053()
        {
            using (var CadAdminContext = new CadAdminModelContainer())
            {
                var results = CadAdminContext.TOWERIMPORTs.Where(x => x.CLIENTKEY.Equals(1899) && x.ISNEW.Equals("F"));
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        public void CadAdminDatabaseQuery054()
        {
            using (var CadAdminContext = new CadAdminModelContainer())
            {
                var results = CadAdminContext.HL7_FTP_CONFIG.Where(x => x.REMOTESERVER.Equals ("rtiops2"));
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        public void CadAdminDatabaseQuery055()
        {
            using (var CadAdminContext = new CadAdminModelContainer())
            {
                var results = CadAdminContext.HL7_FTP_CONFIG.Count();
                Assert.IsTrue(results > 0);
            }
        }
    }

    public partial class DocumentManagerServiceIntegrationTests
    {
        [TestMethod]
        public void SearchPostBdRemote05()
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
    }
}
