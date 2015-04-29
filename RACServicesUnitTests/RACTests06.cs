//
// Tamim's RAC Test's
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
        public void IsAlive06()
        {
            var administrationServer = new AdministrationServer();
            Assert.IsTrue(administrationServer.IsAlive());
        }
    }

    public partial class RtiDataModelUnitTests
    {
        [TestMethod]
        public void TowerDatabaseQuery06()
        {
            using (var towerContext = new TowerModelContainer())
            {
                var results = towerContext.RTICLIENTs.Where(x => x.RTICLIENT_ID > 0);
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        public void CadDatabaseQuery06()
        {
            using (var cadAdminContext = new CadAdminModelContainer())
            {
                var results = cadAdminContext.TOWERIMPORTCLIENTs.Where(x => x.PRACTICE == "");
                Assert.IsFalse(results.Any());
            }
        }

        [TestMethod]
        public void CadDatabaseQuery2_06()
        {
            using (var cadAdminContext = new CadAdminModelContainer())
            {
                var results = cadAdminContext.HL7_FTP_CONFIG.Where(x => x.HL7_FTP_CONFIG_ID > 0);
                Assert.IsTrue(results.Any());
            }

        }

        [TestMethod]
        public void RttransbrokerDatabaseQuery06()
        {
            using (var rttransbrokerContext = new RtTransBrokerAdminModelContainer())
            {
                var results = rttransbrokerContext.HL7_EDLOG.Where(x => x.ID > 0);
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        public void RttransbrokerDatabaseQuery2_06()
        {
            using (var rttransbrokerContext = new RtTransBrokerAdminModelContainer())
            {
                var results = rttransbrokerContext.STGMSHes.Where(x => x.STGMSH_ID == 0);
                Assert.IsFalse(results.Any());

            }

        }

        [TestMethod]
        public void RttransbrokerDatabaseQuery3_06()
        {
            using (var rttransbrokerContext = new RtTransBrokerAdminModelContainer())
            {
                var results = rttransbrokerContext.HCA_CONVERSION_REF.Where(x => x.ID > 0);
                Assert.IsTrue(results.Any());

            }

        }



    }

    public partial class DocumentManagerServiceIntegrationTests
    {
        [TestMethod]
        public void SearchPostBdRemote06()
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

    [TestClass]
    public partial class RequestManagerServerTests
    {
        [TestMethod]
        public void IsAlive06()
        {

            var administrationServer = new AdministrationServer();
            Assert.IsTrue(administrationServer.IsAlive());

        }


    }

}
