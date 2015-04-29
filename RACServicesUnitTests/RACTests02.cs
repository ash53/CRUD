//
// Imran's RAC Test's
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
        public void IsAlive02()
        {
            var administrationServer = new AdministrationServer();
            Assert.IsTrue(administrationServer.IsAlive());
        }
    }

    public partial class RtiDataModelUnitTests
    {
        [TestMethod]
        public void TowerDatabaseQuery02()
        {
            using (var towerContext = new TowerModelContainer())
            {
                var results = towerContext.RTICLIENTs.Where(x => x.RTICLIENT_ID > 0);
                Assert.IsTrue(results.Any());
            }
        }
    }

    public partial class DocumentManagerServiceIntegrationTests
    {
        [TestMethod]
        public void SearchPostBdRemote02()
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
