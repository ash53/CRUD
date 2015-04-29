//
// Main RAC Test's
// 
// If accessing the service through WCF, label it as an Integration Test
// If accessing the server object directly, label it as a Unit Test
// 

using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using log4net.Config;
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
using Rti.RemoteFileTransfer;
using Rti.EncryptionLib;


namespace RACServicesUnitTests
{
    [TestClass]
    public partial class AdministrationServiceUnitTests
    {
        [TestMethod]
        public void IsAlive01()
        {
            var administrationServer = new AdministrationServer();
            Assert.IsTrue(administrationServer.IsAlive());
        }

        [TestMethod]
        public void ServiceInfo()
        {
            var administrationServer = new AdministrationServer();
            var serviceInfo = administrationServer.GetServiceInfo(CommonFunctions.GetMachineName(),
                CommonFunctions.GetUserName());
            Debug.Print( CommonFunctions.ObjectToXml(serviceInfo) );
            Assert.IsTrue(serviceInfo.Responding);
        }

        [TestMethod]
        public void Login()
        {
            var domain = Rti.CommonFunctions.GetDomainName();
            var machine = Rti.CommonFunctions.GetMachineName();
            var user = Rti.CommonFunctions.GetUserName();
            
            Debug.Print("machine:[{0}]", machine);
            Debug.Print("domain:[{0}]", domain);
            Debug.Print("username:[{0}]", user);

            var unixPassword = Rti.CommonFunctions.PromptForInputDialog("Navigator Login", "Enter Unix Password:", "", true);

            var administrationServer = new AdministrationServer();
            var credentials = new Login.UserCredentials(user, unixPassword, domain);
            var permissions = administrationServer.EmWareCollectionLogin(machine, user, credentials);

            Debug.Print("Return value LoginSuccessful:[{0}]", permissions.LoginSuccessful.ToString());
            Debug.Print("Return value LoginMessage:[{0}]", permissions.LoginMessage.ToString());

            Debug.Print(Rti.CommonFunctions.ObjectToXml(permissions));

            Assert.IsTrue(permissions.LoginSuccessful);
        }
    }

    [TestClass]
    public partial class EagleIqGatewayUnitTests
    {
        [TestMethod]
        public void IsAlive()
        {
            var eagleIqGateway = new EagleIqGatewayServer();
            Assert.IsTrue(eagleIqGateway.IsAlive());
        }

        [TestMethod]
        public void ServiceInfo()
        {
            var svr = new EagleIqGatewayServer();
            var serviceInfo = svr.GetServiceInfo(CommonFunctions.GetMachineName(),
                CommonFunctions.GetUserName());
            Debug.Print( CommonFunctions.ObjectToXml(serviceInfo) );
            Assert.IsTrue(serviceInfo.Responding);
        }

        [TestMethod]
        public void ApproveMatch()
        {
            var eagleIqGateway = new EagleIqGatewayServer();
            var result = eagleIqGateway.ApproveMatch(
                new RpcInMessage(){Context = ""
                    ,UserName = CommonFunctions.GetUserName()
                    ,Workstation = CommonFunctions.GetMachineName()}
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
        public void CreateMatch()
        {
            var eagleIqGateway = new EagleIqGatewayServer();
            var dataTable = new DataTable();

            dataTable.Columns.Add("matchid");
            dataTable.Rows.Add(16425177);

            var result = eagleIqGateway.CreateMatch(
                new RpcInMessage(){Context = ""
                    ,UserName = CommonFunctions.GetUserName()
                    ,Workstation = CommonFunctions.GetMachineName()}
                , "mpi"
                , dataTable); 
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
        public void ProcessR2P()
        {
            var eagleIqGateway = new EagleIqGatewayServer();
            string result = eagleIqGateway.ProcessR2P("",
                new UpdR2PInputParams()
                {
                    UserId = CommonFunctions.GetUserName() + ",checkposted"
                     ,
                    InKey = "1253079"
                     ,
                    InDocDet = ""
                     ,
                    InPrCheckDt = DateTime.Now
                     ,
                    InCheckNum = ""
                     ,
                    InExtPayCd = ""
                     ,
                    InPractice = ""
                     ,
                    InDivision = ""
                     ,
                    InProvId = ""

                });
            if (!result.Equals(""))
            {
                Debug.Print(result);
            }
        }
    }

    [TestClass]
    public partial class SearchPostBdTests
    {
        private DocumentManagerServer _documentManagerServer = new DocumentManagerServer();
        private string _machineName = CommonFunctions.GetMachineName();
        private string _userName = CommonFunctions.GetUserName();
        private int startRow = 1;
        private int numRows = 500;

        [TestMethod]
        public void SearchPostBd_001()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                mapName: "",
                actName: "",
                practice: "",
                division: "",
                depDtStart: "",
                embAcct: "",
                depDtEnd: "",
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
                parDocNo: "",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_002()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName, 
                docType: "CK",
                docDet: "PD",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_002count()
        {
            var results = _documentManagerServer.SearchPostBdCount(_machineName, _userName, 
                docType: "CK",
                docDet: "PD");
            Debug.Print("Results:" + results);
            Assert.IsTrue(results > 0);
        }

        [TestMethod]
        public void SearchPostBd_003()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                docType: "CK",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_003count()
        {
            var results = _documentManagerServer.SearchPostBdCount(_machineName, _userName,
                practice: "LTN",
                docType: "CK");
            Debug.Print("Results:" + results);
            Assert.IsTrue(results > 0);
        }

        [TestMethod]
        public void SearchPostBd_004()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                checkAmountMin: "40",
                checkAmountMax: "45",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_004count()
        {
            var results = _documentManagerServer.SearchPostBdCount(_machineName, _userName,
                practice: "LTN",
                checkAmountMin: "40",
                checkAmountMax: "45");
            Debug.Print("Results:" + results);
            Assert.IsTrue(results > 0);
        }

        [TestMethod]
        public void SearchPostBd_005()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                checkAmountMax: "5",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }
        [TestMethod]
        public void SearchPostBd_006()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                checkAmountMin: "500",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }
        [TestMethod]
        public void SearchPostBd_007()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                paidAmountMin: "500.00",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }
        [TestMethod]
        public void SearchPostBd_008()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                paidAmountMax: "5.00",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }
        [TestMethod]
        public void SearchPostBd_009()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                paidAmountMin: "15",
                paidAmountMax: "50",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        //2008-10-29 00:00:00
        [TestMethod]
        public void SearchPostBd_010()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                depDtStart: "20120906",
                depDtEnd: "20120906",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_011()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                mapName: "RTI Bulk Post-Billing",
                actName: "RTI Prep Correction",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_012()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                mapName: "RTI Bulk Post-Billing",
                actName: "RTI Prep Correction",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_013()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                mapName: "RTI Remit Processing           ",
                actName: "RTI Remit Needs Check          ",
                practice: "IOW",
                startRow: startRow,
                numRows: numRows);
            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_014()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                docType: "RM",
                startRow: startRow,
                numRows: numRows);
            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_015()
        {
            var results = _documentManagerServer.SearchPostBd(_machineName, _userName,
                practice: "LTN",
                mapName: "RTI Bulk Post-Billing",
                actName: "RTI Prep Correction",
                startRow: startRow,
                numRows: numRows);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void SearchPostBd_015count()
        {
            var results = _documentManagerServer.SearchPostBdCount(_machineName, _userName,
                practice: "LTN",
                mapName: "RTI Bulk Post-Billing",
                actName: "RTI Prep Correction");

            Debug.Print("Results:" + results);
            Assert.IsTrue(results > 0);
        }
    }

    [TestClass]
    public partial class SearhPostDocTests
    {
        DocumentManagerServer _documentManagerServer = new DocumentManagerServer();
        private string _machineName = CommonFunctions.GetMachineName();
        private string _userName = CommonFunctions.GetUserName();

        [TestMethod]
        public void GetPostDocSearchByDocNo()
        {
            string docNo = "043375000037000";  // Look for a docNo that does exist
            var results = _documentManagerServer.GetPostDocSearchByDocNo(_machineName, _userName, docNo);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count > 0, "No results");
        }

        [TestMethod]
        public void GetPostDocSearchByDocNo_null()
        {
            string docNo = "043375";  // Look for a docNo that does not exist
            var results = _documentManagerServer.GetPostDocSearchByDocNo(_machineName, _userName, docNo);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count == 0, "Should not have returned results");
        }

        [TestMethod]
        public void GetImportedPostDocByTowId()
        {
            var towId = 14915889;  // Look for a parDocNo that exists
            var results = _documentManagerServer.GetImportedPostDocByTowId(_machineName, _userName, towId);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count > 0, "No results");
        }

        [TestMethod]
        public void GetImportedPostDocByTowId_null()
        {
            var towId = 50;  // TowId that doesn't exist in table
            var results = _documentManagerServer.GetImportedPostDocByTowId(_machineName, _userName, towId);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count == 0, "Should not have returned results");
        }

        [TestMethod]
        public void GetImportedPostDocByPracticeAndAccount()
        {
            var practice = "SPR";
            var account = "1078257704";
            
            var results = _documentManagerServer.GetImportedPostDocByPracticeAndAccount(
                _machineName, _userName, practice, account);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count > 0, "No results");
        }

        [TestMethod]
        public void GetImportedPostDocByPracticeAndAccount_null()
        {
            var practice = "SP";  // Not in table
            var account = "7";    // Not in table
            
            var results = _documentManagerServer.GetImportedPostDocByPracticeAndAccount(
                _machineName, _userName, practice, account);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count == 0, "Should not have returned results");
        }

        [TestMethod]
        public void GetPostDocSearchByParDocNo()
        {
            string parDocNo = "092807700039000";
            var results = _documentManagerServer.GetPostDocSearchByParDocNo(_machineName, _userName, parDocNo);
            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count > 0, "No results");
        }

        [TestMethod]
        public void GetDocNoFromPostDocsByParDocNo()
        {
            string parDocNo = "043375000037000";  // Look for a docNo that does exist
            var results = _documentManagerServer.GetDocNoFromPostDocsByParDocNo(_machineName, _userName, parDocNo);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count > 0, "No results");
        }

        [TestMethod]
        public void GetDocNoFromPostDocsByParDocNo_null()
        {
            string parDocNo = "44";  // Look for a docNo that does exist
            var results = _documentManagerServer.GetDocNoFromPostDocsByParDocNo(_machineName, _userName, parDocNo);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsTrue(results.Rows.Count == 0, "Should not have returned results");
        }
    }

    [TestClass]
    public partial class DocumentManagerServiceUnitTests
    {
        DocumentManagerServer _documentManagerServer = new DocumentManagerServer();
        private string _machineName = CommonFunctions.GetMachineName();
        private string _userName = CommonFunctions.GetUserName();

        [TestMethod]
        public void IsAlive()
        {
            Assert.IsTrue(_documentManagerServer.IsAlive());
        }

        [TestMethod]
        public void ServiceInfo()
        {
            var svr = new DocumentManagerServer();
            var serviceInfo = svr.GetServiceInfo(CommonFunctions.GetMachineName(),
                CommonFunctions.GetUserName());
            Debug.Print( CommonFunctions.ObjectToXml(serviceInfo) );
            Assert.IsTrue(serviceInfo.Responding);
        }

        [TestMethod]
        public void GetPostBdWithC2PR2PDataMerged_01()
        {
            var results = _documentManagerServer.GetPostBdWithC2PR2PDataMerged(_machineName, _userName, 
                practice: "LTN",
                depDtStart: "",
                depDtEnd: "",
                checkAmountMin: "",
                checkAmountMax: "",
                docType: "CK",
                docDet: "PD",
                docNo: "",
                parDocNo: "",
                startRow: 3,
                numRows: 20);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void GetPostBdWithC2PR2PDataMerged_02()
        {
            var results = _documentManagerServer.GetPostBdWithC2PR2PDataMerged(_machineName, _userName,
                mapName: "RTI Remit Processing",
                actName: "RTI Remit Needs Check",
                startRow: 1,
                numRows: 500);

            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void GetEiqNavC2PByDocTypeDocDet()
        {
            var results = _documentManagerServer.GetEiqNavC2PByDocTypeDocDet(_machineName, _userName, "BH", "CM");
            VerifyUtils.VerifyDataTable(results);
            
            results = _documentManagerServer.GetEiqNavC2PByDocTypeDocDet(_machineName, _userName, "BH", "");
            VerifyUtils.VerifyDataTable(results);

            results = _documentManagerServer.GetEiqNavC2PByDocTypeDocDet(_machineName, _userName, "", "CM");
            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void GetPostDetailByTowerId()
        {
            var result = _documentManagerServer.GetPostDetailByTowerId(_machineName, _userName, 21337551);
            VerifyUtils.VerifyDataTable(result);
        }

        [TestMethod]
        public void GetPostDocStatusLookup()
        {
            var results = _documentManagerServer.GetPostDocStatusLookup(_machineName, _userName);
            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GetDocNoFromPostDocsByParDocNo()
        {
            string parDocNo = "843375000037000";
            var results = _documentManagerServer.GetDocNoFromPostDocsByParDocNo(_machineName, _userName, parDocNo);

            Debug.Print(CommonFunctions.ObjectToXml(results));
            Assert.IsNotNull(results);
        }


        [TestMethod]
        public void GetPostBdMapInstIdNodeId()
        {
            decimal? courierInstId = 51409040;
            var results = _documentManagerServer.GetPostBdMapInstIdNodeId(_machineName, _userName, courierInstId);
            Assert.IsNotNull(results);
            
            Debug.Print(results.Item1.ToString());
            Debug.Print(results.Item2.ToString());
            
            Assert.IsTrue(results.Item1 > 0);
            Assert.IsTrue(results.Item2 > 0);
        }

        [TestMethod]
        public void ConvertPDFtoTIF()
        {
            string message = "";
            //string sourcePath = @"C:\dev";
            //string sourceFile = "ss5_copy.pdf";
            //string destFile = "dest.pdf";
            //string tifFile = "dest.tif";
            string sourcePath = @"\\rtiedinas.rti.com\FS_RTI_navigator\inbound\live";
            string sourceFile = "7798_20120410_7721874035.pdf";
            // string sourceFile = "2800_20130801_340396.pdf";
            string destFile = "test030615a.pdf";
            string tifFile = "test030615a.tif";

            string source = System.IO.Path.Combine(sourcePath, sourceFile);
            string dest = System.IO.Path.Combine(sourcePath, destFile);
            string tif = System.IO.Path.Combine(sourcePath, tifFile);

            System.IO.File.Delete(tif);
            System.IO.File.Copy(source, dest, true);

            var result = _documentManagerServer.ConvertPDFtoTif(_machineName, _userName, dest, out message);
            Assert.IsNotNull(result);
            Debug.Print("Message:{0}", message);
            Debug.Print("result:{0}", result);
            Assert.IsTrue(result);
            Assert.IsTrue(System.IO.File.Exists(tif), "TIF file not created");
        }

        [TestMethod]
        public void ConvertPDFtoTIF2()
        {
            string message = "";
            string sourcePath = @"C:\dev\pdftest";
            string sourceFile = "7017_20150201_23305595_ED_0.pdf";
            string destFile = "5.pdf";
            string tifFile = "5.tif";

            string source = System.IO.Path.Combine(sourcePath, sourceFile);
            string dest = System.IO.Path.Combine(sourcePath, destFile);
            string tif = System.IO.Path.Combine(sourcePath, tifFile);

            System.IO.File.Delete(tif);
            System.IO.File.Copy(source, dest, true);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = _documentManagerServer.ConvertPDFtoTif(_machineName, _userName, dest, out message);
            sw.Stop();
            Debug.Print("Elapsed conversion time=[{0}]", sw.Elapsed);

            Assert.IsNotNull(result);
            Debug.Print("Message:{0}", message);
            Debug.Print("result:{0}", result);
            Assert.IsTrue(result);
            Assert.IsTrue(System.IO.File.Exists(tif), "TIF file not created");
        }

        [TestMethod]
        public void Ifam_GetMapAndActivityByLoginName()
        {
            var userName = Environment.UserName;
            var result = _documentManagerServer.GetMapAndActivityByLoginName(_machineName, _userName, userName);
            Debug.Print("Total Rows:[{0}] for user:[{1}]", result.Rows.Count, _userName);
            foreach (DataRow dr in result.Rows)
            {
                Debug.Print("Map[{0}] Activity[{1}]", dr[0].ToString(), dr[1].ToString());
            }
            Assert.IsTrue(result.Rows.Count > 0);
        }

        [TestMethod]
        public void GetNavC2PByParDocNo()
        {
            var parDocNo = "092376105390005";
            var result = _documentManagerServer.GetNavC2PByParDocNo(_machineName, _userName, parDocNo);
            VerifyUtils.VerifyDataTable(result);
        }
        
        [TestMethod]
        public void GetNavC2PByParDocNoJoinPostDoc()
        {
            var parDocNo = "092376105390005";
            //var parDocNo = "150132001329919";
//            var parDocNo = "150132001329919";
            var results = _documentManagerServer.GetNavC2PByParDocNoJoinPostDoc(_machineName, _userName, parDocNo);
            VerifyUtils.VerifyDataTable(results);
        }

        [TestMethod]
        public void GetCashItemsByDocNoRPC()
        {
            var docNo = "092376105390005";
            var result = _documentManagerServer.GetCashItemsByDocNo(_machineName, _userName, docNo);
            VerifyUtils.VerifyDataTable(result);
        }

        [TestMethod]
        public void GetRemitItemsByDocNoRPC()
        {
            var docNo = "150132001329919";
            var result = _documentManagerServer.GetRemitItemsByDocNo(_machineName, _userName, docNo);
            VerifyUtils.VerifyDataTable(result);
        }

        [TestMethod]
        public void processNotes()
        {
            var result = _documentManagerServer.ProcessNote(_machineName, _userName, 1253079, "", "C", "READ");
            if (result != null)
            {
                if (!result.Success)
                {
                    Debug.Print(result.OutMsg);
                }
                //Assert.IsTrue(result.Success);
            }

        }    
    }

    [TestClass]
    public partial class RtiDataModelUnitTests
    {
        [TestMethod]
        public void TowerDatabaseConnection()
        {
            using(var towerContext = new TowerModelContainer())
            {
                var results = towerContext.RTICLIENTs.Count(x => x.RTICLIENT_ID > 0);
                Debug.Print(results.ToString());
                Assert.IsTrue(results > 0);
            }
        }

        [TestMethod]
        public void CadAdminDatabaseConnection()
        {
            using (var cadAdminContext = new CadAdminModelContainer())
            {
                var results = cadAdminContext.HL7_FTP_CONFIG.Where(x => x.HL7_FTP_CONFIG_ID > 0);
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        public void IfamDatabaseConnection()
        {
            using (var ifamContext = new IfamModelContainer())
            {
                var results = ifamContext.MAP_NAMES.Count(x => x.MAP_NAME_ID > 0);
                Debug.Print(results.ToString());
                Assert.IsTrue(results > 0);
            }
        }

        [TestMethod]
        public void RtTransbrokerConnection()
        {
            using (var rttransContext = new RtTransBrokerAdminModelContainer())
            {
                var results = rttransContext.ETL_CONFIG.Where(x => x.ETLCONFIG_ID > 0);
                Assert.IsTrue(results.Any());
            }
        }
    }

    [TestClass]
    public partial class RtiRemoteProcedureCallsLibrary
    {
        [TestMethod]
        public void GetR2Ps()
        {
            GetRemitItemsInputParams inputParams = new GetRemitItemsInputParams();
            AssemblyFilter fltr = new AssemblyFilter();

            //assign input parameters
            inputParams.ServerName = "longrpc";
            fltr.Status = "Show only active items";
            fltr.PostingSuspends = "Show all cash items";
            fltr.MatchStatus = "Show all items";

            inputParams.StartRow = "1";
            inputParams.DocNo = "140293300473000,140293300473000"; //",140293300473000"; //",";043421000122000
            //inputParams.DocNo = "043421000122000"; //",140293300473000"; //",";043421000122000

            fltr.Practice = "";
            fltr.ExtPayor = "";
            fltr.ServiceType = "";
            fltr.DocDetail = "";
            fltr.CheckAmountMin = "-1";
            fltr.CheckAmountMax = "-1";
            fltr.DateMin = DateTime.MinValue.ToString();
            inputParams.Filters = fltr;

            GetR2PList a = new GetR2PList(inputParams);
            GetRemitItemsResults1 remitItemsResult = a.CallRPC();

            Assert.IsNotNull(remitItemsResult);

            Debug.Print("Result Success:" + remitItemsResult.Success.ToString());
            Debug.Print("Result OutMsg:" + remitItemsResult.OutMsg);
            Debug.Print("Result Rows Returned: " + remitItemsResult.RemitItems.Rows.Count);

            foreach (DataRow remitItem in remitItemsResult.RemitItems.Rows)
            {
                Debug.Print(CommonFunctions.ObjectToXml(remitItemsResult));
            }

            Assert.IsTrue(remitItemsResult.RemitItems.Rows.Count > 0, "No results returned");
        }

        [TestMethod]
        public void GetC2Ps()
        {
            GetCashItemsInputParams inputParams = new GetCashItemsInputParams();
            AssemblyFilter fltr = new AssemblyFilter();

            //assign input parameters
            inputParams.ServerName = "longrpc";
            fltr.Status = "Show only active items";
            fltr.PostingSuspends = "Show all cash items";
            fltr.MatchStatus = "Show all items";

            inputParams.StartRow = "1";
            inputParams.DocNo =
                "140235004120000,140235004121000,140275001365000,140285001549000,140285001551000,092471000623000";
            //inputParams.DocNo = "043421000122000"; //",140293300473000"; //",";043421000122000

            fltr.Practice = "";
            fltr.ExtPayor = "";
            fltr.ServiceType = "";
            fltr.DocDetail = "";
            fltr.CheckAmountMin = "-1";
            fltr.CheckAmountMax = "-1";
            fltr.DateMin = DateTime.MinValue.ToString();
            inputParams.Filters = fltr;

            GetC2PList a = new GetC2PList(inputParams);
            GetCashItemsResults1 cashItemsResult = a.CallRPC();

            Assert.IsNotNull(cashItemsResult);

            Debug.Print("Result Success:" + cashItemsResult.Success.ToString());
            Debug.Print("Result OutMsg:" + cashItemsResult.OutMsg);
            Debug.Print("Result Rows Returned: " + cashItemsResult.CashItems.Rows.Count);

            foreach (DataRow remitItem in cashItemsResult.CashItems.Rows)
            {
                Debug.Print(CommonFunctions.ObjectToXml(cashItemsResult));
            }

            Assert.IsTrue(cashItemsResult.CashItems.Rows.Count > 0, "No results returned");
        }
    }

    [TestClass]
    public partial class DocumentManagerServiceIntegrationTests
    {
        [TestMethod]
        public void ImagingLibraryIsValid()
        {
            string message = "";
            string sourcePath = @"\\rtiedinas.rti.com\FS_RTI_navigator\inbound";
            string sourceFile = "a.pdf";
            string destFile = "dest.pdf";
            string tifFile = "dest.tif";

            string source = System.IO.Path.Combine(sourcePath, sourceFile);
            string dest = System.IO.Path.Combine(sourcePath, destFile);
            string tif = System.IO.Path.Combine(sourcePath, tifFile);

            System.IO.File.Delete(tif);
            System.IO.File.Copy(source, dest, true);

            using (
                var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL,
                    Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                var result = docManagerServiceClient.ConvertPDFtoTif(CommonFunctions.GetMachineName(), 
                    CommonFunctions.GetUserName(), dest, out message);
                Assert.IsNotNull(result);
                Debug.Print("Message:{0}", message);
                Debug.Print("result:{0}", result.ToString());
                Assert.IsTrue(result);
                Assert.IsTrue(System.IO.File.Exists(tif), "TIF file not created");
            }
        }

        [TestMethod]
        public void GetRemitItemsByDocNoRPC()
        {
            var docNo = "150132001329919";
            using (
                var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL,
                    Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                var embillzSearchData = docManagerServiceClient.GetRemitItemsByDocNo(Environment.MachineName,
                    Environment.UserName, docNo);
                VerifyUtils.VerifyDataTable(embillzSearchData);
            }
        }

        [TestMethod]
        public void SearchPostBd()
        {
            var docType = "CK";
            var docDet = "AD";
            using (
                var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL,
                    Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                var embillzSearchData = docManagerServiceClient.SearchPostBd(Environment.MachineName,
                    Environment.UserName, docType: docType, docDet: docDet);
                VerifyUtils.VerifyDataTable(embillzSearchData);
            }
        }

        [TestMethod]
        public void SearchPostBdRemote()
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
    public partial class AdministrationServiceIntegrationTests
    {
        [TestMethod]
        public void IsAlive01()
        {
            using (
                var adminServiceClient = new AdministrationServiceClient(VerifyUtils.AdminRemoteUrl,
                    VerifyUtils.AdminRemoteEndpointName))
            {
                Assert.IsTrue(adminServiceClient.IsAlive());
            }
        }
    }

    [TestClass]
    public partial class TestRemoteServerConfiguration
    {
        // QA
         private const string RemoteServer = "apps.rti.com";
        // Live
        //private const string RemoteServer = "navigator.rti.com";

        [TestMethod]
        public void AdministrationServiceIsAlive()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var adminServiceClient = new AdministrationServiceClient(VerifyUtils.AdminRemoteUrl,
                    VerifyUtils.AdminRemoteEndpointName))
            {
                Assert.IsTrue(adminServiceClient.IsAlive());
            }
        }

        [TestMethod]
        public void AdministrationServiceInfo()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var adminServiceClient = new AdministrationServiceClient(VerifyUtils.AdminRemoteUrl,
                    VerifyUtils.AdminRemoteEndpointName))
            {
                var serviceInfo = adminServiceClient.GetServiceInfo(CommonFunctions.GetMachineName(),
                    CommonFunctions.GetUserName());
                Debug.Print(CommonFunctions.ObjectToXml(serviceInfo));
                Assert.IsTrue(serviceInfo.Responding);
            }
        }

        [TestMethod]
        public void DocumentManagerServiceIsAlive()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var docMgrServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                Assert.IsTrue(docMgrServiceClient.IsAlive());
            }
        }

        [TestMethod]
        public void DocumentManagerServiceInfo()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var serviceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var serviceInfo = serviceClient.GetServiceInfo(CommonFunctions.GetMachineName(),
                    CommonFunctions.GetUserName());
                Debug.Print(CommonFunctions.ObjectToXml(serviceInfo));
                Assert.IsTrue(serviceInfo.Responding);
            }
        }

        [TestMethod]
        public void GetPostDocStatusLookup()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var serviceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var results = serviceClient.GetPostDocStatusLookup(CommonFunctions.GetMachineName(), CommonFunctions.GetUserName());
                Debug.Print(CommonFunctions.ObjectToXml(results));
                Assert.IsNotNull(results);
            }
        }

        [TestMethod]
        public void EagleIqServiceIsAlive()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var eiqServiceClient = new EagleIqGatewayClient(VerifyUtils.EiqRemoteUrl,
                    VerifyUtils.EiqRemoteEndpointName))
            {
                Assert.IsTrue(eiqServiceClient.IsAlive());
            }
        }

        [TestMethod]
        public void EagleIqServiceInfo()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var serviceClient = new EagleIqGatewayClient(VerifyUtils.EiqRemoteUrl,
                    VerifyUtils.EiqRemoteEndpointName))
            {
                Stopwatch s1 = Stopwatch.StartNew();
                for (int i = 500; i > 0; i--)
                {
                    var serviceInfo = serviceClient.GetServiceInfo(CommonFunctions.GetMachineName(),
                        CommonFunctions.GetUserName());
                    Debug.Print(CommonFunctions.ObjectToXml(serviceInfo));
                    Assert.IsTrue(serviceInfo.Responding);
                }
                s1.Stop();
                Debug.Print("Runtime:[{0}]", s1.ElapsedMilliseconds);
            }
        }

        [TestMethod]
        public void ProgressDatabaseConnectionIsValid()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var docMgrServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var results = docMgrServiceClient.GetPostBdWithC2PR2PDataMerged(CommonFunctions.GetMachineName(),
                    CommonFunctions.GetUserName(), 
                    docType: "CK",
                    depDtStart: "20141207",
                    depDtEnd: "20141215");

                VerifyUtils.VerifyDataTable(results);
            }
        }
        //GetMapAndActivityByLoginName
        [TestMethod]
        public void GetFlowareMapActivityPermissionsFromIfamSchemaTest()
        {
            using (
                var docMgrServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var results = docMgrServiceClient.GetMapAndActivityByLoginName(CommonFunctions.GetMachineName(),
                    CommonFunctions.GetUserName(), CommonFunctions.GetUserName());

                VerifyUtils.VerifyDataTable(results);
            }
        }
        [TestMethod]
        public void EntityFrameworkDatabaseConnectionIsValid()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var docMgrServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var results = docMgrServiceClient.SearchPostBd(CommonFunctions.GetMachineName(),
                    CommonFunctions.GetUserName(),
                    docType: "CK",
                    docDet: "PD");

                VerifyUtils.VerifyDataTable(results);
            }
        }

        [TestMethod]
        public void DocumentManagerGetPracticesTest()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var docMgrServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var results = docMgrServiceClient.ListPractices(CommonFunctions.GetMachineName(),
                    CommonFunctions.GetUserName());

                VerifyUtils.VerifyDataTable(results);
            }
        }

        [TestMethod]
        public void ImagingLibraryIsValid()
        {
            VerifyUtils.RemoteServer = RemoteServer;
            string message = "";
            string sourcePath = @"\\rtiedinas.rti.com\FS_RTI_navigator\inbound\live";
            string sourceFile = "2800_20130801_340396.pdf";
            string destFile = "test030615.pdf";
            string tifFile = "test030615.tif";

            string source = System.IO.Path.Combine(sourcePath, sourceFile);
            string dest = System.IO.Path.Combine(sourcePath, destFile);
            string tif = System.IO.Path.Combine(sourcePath, tifFile);

            System.IO.File.Delete(tif);
            System.IO.File.Copy(source, dest, true);

            using (
                var docMgrServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var result = docMgrServiceClient.ConvertPDFtoTif(CommonFunctions.GetMachineName(), 
                    CommonFunctions.GetUserName(), dest, out message);
                Assert.IsNotNull(result);
                Debug.Print("Message:{0}", message);
                Debug.Print("result:{0}", result.ToString());
                Assert.IsTrue(result);
                Thread.Sleep(1000);
                Assert.IsTrue(System.IO.File.Exists(tif), "TIF file not created");
            }
        }
        
        [TestMethod]
        public void EmBillsRpcConnectionIsValid()        
        {
            VerifyUtils.RemoteServer = RemoteServer;
            using (
                var docMgrServiceClient = new DocumentManagerServiceClient(VerifyUtils.DocMgrRemoteUrl,
                    VerifyUtils.DocMgrRemoteEndpointName))
            {
                var docNo = "140286102700008";
                var result = docMgrServiceClient.GetCashItemsByDocNo(CommonFunctions.GetMachineName(),
                    CommonFunctions.GetUserName(), docNo);
                VerifyUtils.VerifyDataTable(result);
            }
        }
    }

    [TestClass]
    public partial class ProgressTests
    {
        DocumentManagerServer _documentManagerServer = new DocumentManagerServer();
        string _machineName = CommonFunctions.GetMachineName();
        string _userName = CommonFunctions.GetUserName();

        [TestMethod]
        public void GetEiqNavC2PByDocTypeDocDet()
        {
            var docType = "M2";
            var docDet = "CR";

            var result = _documentManagerServer.GetEiqNavC2PByDocTypeDocDet(_machineName,
                    _userName, docType, docDet);
            
            VerifyUtils.VerifyDataTable(result);
        }

        [TestMethod]
        public void GetEiqNavC2PByDocTypeDocDetPractice()
        {
            var docType = "M2";
            var docDet = "CR";
            var practice = "GTA";

            var result = _documentManagerServer.GetEiqNavC2PByDocTypeDocDetPractice(_machineName,
                    _userName, docType, docDet, practice);
            
            VerifyUtils.VerifyDataTable(result);
        }
    }

    [TestClass]
    public partial class EncryptionLibTests
    {
        [TestMethod]
        public void EncryptString()
        {
            var encryptor = new EncryptionFuncs();
            string someString = "SomeImportantString";
            string encryptedString = encryptor.Encrypt(someString);
            Debug.Print(encryptedString);
            string decryptedString = encryptor.Decrypt(encryptedString);
            Debug.Print(decryptedString);
            Assert.AreEqual(someString, decryptedString);
        }
    }

    [TestClass]
    public partial class CoreTests
    {
        [TestMethod]
        public void ExceptionTest()
        {
            String testStr = null;

            VerifyUtils.MyAssert.Throws<NullReferenceException>( () => testStr.ToUpper(  ) );
        }

    }

    [TestClass]
    public class SftpTests
    {
        public string Md5Hash(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","").ToLower();
                }
            }
        }

        [TestMethod]
        public void PutFile()
        {
            string user = CommonFunctions.GetUserName().ToLower();
            // Input dialog only displays when running tests normally (not in debug)
            string password = CommonFunctions.PromptForInputDialog("Navigator Login", "Enter Unix Password:", "", true);
            string file = @"C:\dev\test1600.pdf";
//            string file = @"\\rtiedinas.rti.com\FS_RTI_navigator\inbound\qa\ACV11-18-14c.tif";

            Debug.Print("Md5Hash:[" + Md5Hash(file) + "]");

            var localCheckSum = Md5Hash(file);

            var sftpClient = new SftpClient(Constants.ImportImageSftpHost, user, password);
            
            sftpClient.Put(file, Path.GetFileName(file));
            sftpClient.Get(Path.GetFileName(file), Path.GetDirectoryName(file) + @"\checkme" + Path.GetExtension(file));

            // Point this to a file that exists that is not the first file to test checksum failure
            var remoteCheckSum = Md5Hash(Path.GetDirectoryName(file) + @"\checkme" + Path.GetExtension(file));
            Assert.IsTrue(localCheckSum == remoteCheckSum, "Checksums don't match!");

            File.Delete(Path.GetDirectoryName(file) + @"\checkme" + Path.GetExtension(file));
            sftpClient = null;
        }
    }
}
