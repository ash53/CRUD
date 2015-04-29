//
// Sadia's RAC Test's
// 

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rti.AdministrationServer;
using Rti.DataModel;
using Rti.InternalInterfaces.ServiceProxies;
using System.Diagnostics;
using Rti.InternalInterfaces.DataContracts;
using Rti.EagleIqGatewayServer.Servers;
using Rti.EagleIqGatewayServer.RPCs;
using Rti;
using System.Data;
using Rti.EagleIqGatewayServer.StoredProcedures;
using Rti.EagleIqGatewayServer.Threads;


namespace RACServicesUnitTests
{
    public partial class AdministrationServiceUnitTests
    {
        [TestMethod]
        public void login01()
        {
            //var domain = " ";
            //var machine =" ";
            //var user = " ";
            //var user = "12345";
            var domain = Rti.CommonFunctions.GetDomainName();
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();
            var unixPassword = " ";
            //var unixPassword = Rti.CommonFunctions.PromptForInputDialog("Navigator Login", "Enter Unix Password:", "", true);
            var administrationServer = new AdministrationServer();
            var credentials = new Login.UserCredentials(user, unixPassword, domain);
            var permissions = administrationServer.EmWareCollectionLogin(machine, user, credentials);

            Debug.Print("Return value LoginSuccessful:[{0}]", permissions.LoginSuccessful.ToString());
            Debug.Print("Return value LoginMessage:[{0}]", permissions.LoginMessage.ToString());

            Debug.Print(Rti.CommonFunctions.ObjectToXml(permissions));

            //Assert.IsTrue(permissions.LoginSuccessful);
            Assert.IsFalse(permissions.LoginSuccessful);
        }
    }

    public partial class RtiDataModelUnitTests
    {
        [TestMethod]
        public void TowerDatabasequery01()
        {
            using(var towerContext=new TowerModelContainer())
            {
                var items = (from a in towerContext.RTICLIENTs where a.PRACTICE == "KKK" select a.DIVISION);
                Assert.AreEqual(3, items.Count());
                //this test should fail as their is no practice named 'KKK'
            }
        }
        [TestMethod]
        public void CadadminDatabaseQuery01()
        {
            var cadContext = new CadAdminModelContainer();
            var items = (from b in cadContext.EMSCAN_BATCH where b.PRACTICE != null select b.PRACTICE).Distinct().OrderBy(x => x);
            Assert.IsTrue(items.Any());
        }
        [TestMethod]
        public void RtTransBrokerDatabaseQuery01()
        {
            var rtContext = new RtTransBrokerAdminModelContainer();
            var items = rtContext.ETL_CONFIG.Where(x => x.EIQ_ENABLED == "Y").Select(y => y.ETLCONFIG_ID);
            Assert.IsTrue(items.Any());
        }

        [TestMethod]
        public void IfAmDatabaseQuery01()
        {
            var IfAmContext = new IfamModelContainer();
            var items = IfAmContext.USERS.Where(x => x.USER_ID == null).Select(y => y.LOGIN_NAME);
            Assert.IsFalse(items.Any()); //this will pass
            //Assert.IsTrue(items.Any()); //this won't pass as USER_ID being the primary key, there shouldn't be any null value
        }

    }

    public partial class EagleIqGatewayUnitTests
    {
        private string _machineName = CommonFunctions.GetMachineName();
        private string _userName = CommonFunctions.GetUserName();
            
        [TestMethod]
        public void createMatch()
        {

            var navMatch = new NavMatching();
            var dataTable = new DataTable("postbd");
            dataTable.Columns.Add("MAP_NAME");
            dataTable.Columns.Add("ACT_NAME");
            dataTable.Columns.Add("ASSIGNED_TO");
            dataTable.Columns.Add("DOCNO");
            dataTable.Columns.Add("PRACTICE");
            dataTable.Columns.Add("DIVISION");
            dataTable.Columns.Add("EMBACCT");
            dataTable.Columns.Add("REASON");
            dataTable.Columns.Add("DOCTYPE");
            dataTable.Columns.Add("DOCDET");
            dataTable.Columns.Add("DEPDT");
            dataTable.Columns.Add("CHECKNUM");
            dataTable.Columns.Add("CHECKAMT");
            var result=navMatch.CreateMatch(
                new RpcInMessage() { Context = "", UserName = _userName, Workstation = _machineName },
                "mpi", dataTable);
            if (result != null)
            {
                if (!result.IsSuccess)
                {
                    Debug.Print(result.OutMessage);
                }
                Assert.IsTrue(result.IsSuccess);
            }
        }
        
        [TestMethod]
        public void ApproveMatch01()
        {
            var navMatch = new NavMatching();
            var result = navMatch.ApproveMatch(
                new RpcInMessage()
                {
                    Context = ""
                    ,
                    UserName = _userName
                    ,
                    Workstation = _machineName
                }
                , "mpi"
                , 20146832);
            if (result != null)
            {
                if (!result.IsSuccess)
                {
                    Debug.Print(result.OutMessage);
                }
                Assert.IsTrue(result.IsSuccess);
            }
        }

        [TestMethod]
        public void getPostBd()
        {
            var navProcedure = new NavProcedures();
            var result = navProcedure.GetPostBd(_machineName, _userName,
                mapName: "",
                actName: "",
                practice: "",
                division: "",
                depDtStart: "",
                depDtEnd: "",
                embAcct: "",
                reason: "",
                checkNum: "",
                checkAmountMin: "",
                checkAmountMax: "",
                paidAmountMin: "",
                paidAmountMax: "",
                docType: "CK",
                docDet: "",
                who: "",
                docNo: "",
                parDocNo: "");
            VerifyUtils.VerifyDataTable(result);
        }
    }

    

}
