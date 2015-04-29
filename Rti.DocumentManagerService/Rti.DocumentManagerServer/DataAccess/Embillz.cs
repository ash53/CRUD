using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Threading;
using ADODB;
using com.solvepoint.iqrpc.client;
using Rti.InternalInterfaces.DataContracts;
using Rti.Imaging;
using Rti.RemoteProcedureCalls.Embillz;
using Rti.EncryptionLib;
using Rti.RemoteFileTransfer;


namespace Rti.DocumentManagerServer.DataAccess
{

    class Embillz
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        com.solvepoint.iqrpc.client.Connection _iqConn = null;
        Commander _commander = null;
        String _serverName = "mpi";
        IqRpc _rpc = null;
        IqResultData _result = null;
        IqInputData _input = null;
        IqInputSet _inputSet;
        readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory;

        public DataTable InsertDepositLog(string iPractice, string iDepositDate, string iDocDet, string iDocType, string iParDocNo,
            string iCheckDate, double iCheckAmount, string iExtPayor, string iCheckNumber, int iDepSequence)
        {
            Log.Debug("Enter InsertDepositLog()");

            try
            {
                setUp();

                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                String rpcName = "deplog";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                // left side must match up with RPC definition
                // right side is the methods names
                _input.setParam("Practice", iPractice);
                _input.setParam("DepDate", iDepositDate);
                _input.setParam("DocDet", iDocDet);
                _input.setParam("DocType", iDocType);
                _input.setParam("ParDocNo", iParDocNo);
                _input.setParam("CheckDt", iCheckDate);
                _input.setParam("DollarAmt", iCheckAmount);
                _input.setParam("ExtPayor", iExtPayor);
                _input.setParam("CheckNum", iCheckNumber);
                _input.setParam("DepSequence", iDepSequence);

                _result = _commander.executeRpc(_rpc);

                Recordset rs = new RecordsetClass();
                rs.CursorType = CursorTypeEnum.adOpenStatic;
                rs.CursorLocation = CursorLocationEnum.adUseClient;

                rs.Fields.Append("success", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldUnspecified, Type.Missing);
                rs.Fields.Append("DepSeq", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldUnspecified, Type.Missing);
                rs.Fields.Append("Count", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldUnspecified, Type.Missing);

                rs.Open(Type.Missing, Type.Missing, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);

                rs.AddNew(Type.Missing, Type.Missing);

                rs.Fields["success"].Value = _result.getBoolean("success");
                rs.Fields["DepSeq"].Value = _result.getInteger("DepSeq");
                rs.Fields["Count"].Value = _result.getInteger("Count");

                rs.Update(Type.Missing, Type.Missing);

                rs.MoveFirst();

                Log.Debug("Exit InsertDepositLog()");
                return CommonFunctions.RecordSetToDataTable(rs, "deplog", "deplog");
            }
            finally
            {
                tearDown();
            }
        }

        public DataTable InsertDeplog(string iPractice, string iDepositDate, string iDocDet, string iDocType, string iParDocNo,
            string iCheckDate, double iCheckAmount, string iExtPayor, string iCheckNumber, int iDepSequence, string iServiceId)
        {
            Log.Debug("Enter InsertDeplog()");

            try
            {
                setUp();

                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                String rpcName = "deplog";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                // left side must match up with RPC definition
                // right side is the methods names
                _input.setParam("Practice", iPractice);
                _input.setParam("DepDate", iDepositDate);
                _input.setParam("DocDet", iDocDet);
                _input.setParam("DocType", iDocType);
                _input.setParam("ParDocNo", iParDocNo);
                _input.setParam("CheckDt", iCheckDate);
                _input.setParam("DollarAmt", iCheckAmount);
                _input.setParam("ExtPayor", iExtPayor);
                _input.setParam("CheckNum", iCheckNumber);
                _input.setParam("DepSequence", iDepSequence);
                _input.setParam("ServiceId", iServiceId);

                _result = _commander.executeRpc(_rpc);

                Recordset rs = new RecordsetClass();
                rs.CursorType = CursorTypeEnum.adOpenStatic;
                rs.CursorLocation = CursorLocationEnum.adUseClient;

                rs.Fields.Append("success", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldUnspecified, Type.Missing);
                rs.Fields.Append("DepSeq", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldUnspecified, Type.Missing);
                rs.Fields.Append("Count", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldUnspecified, Type.Missing);

                rs.Open(Type.Missing, Type.Missing, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);

                rs.AddNew(Type.Missing, Type.Missing);

                rs.Fields["success"].Value = _result.getBoolean("success");
                rs.Fields["DepSeq"].Value = _result.getInteger("DepSeq");
                rs.Fields["Count"].Value = _result.getInteger("Count");

                rs.Update(Type.Missing, Type.Missing);

                rs.MoveFirst();

                Log.Debug("Exit InsertDepositLog()");
                return CommonFunctions.RecordSetToDataTable(rs, "deplog", "deplog");
            }
            finally
            {
                tearDown();
            }
        }

        public RpcOutMessage CreateWorkflowItem(string workstation, string username, NavWorkFlowItem workFlowItem)
        {
            Log.DebugFormat("Enter CreateWorkflowItem() docNo:[{0}]", workFlowItem.docNo);

            try
            {
                setUp();

                var rpcOutMessage = new RpcOutMessage();

                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                String rpcName = "create_wfitems";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                IqInputSet inputSet;
                inputSet = _input.getInputSet("inSet");
                
                _input.setParam("userId", username);

                inputSet.addRow();               
                inputSet.setField("practice", workFlowItem.practice);
				inputSet.setField("division", workFlowItem.division);
				inputSet.setField("depDate", workFlowItem.depDate == null ? DateTime.MinValue : (DateTime)workFlowItem.depDate);
				inputSet.setField("recDate", workFlowItem.recDate == null ? DateTime.MinValue : (DateTime)workFlowItem.recDate);
				inputSet.setField("docType", workFlowItem.docType);
				inputSet.setField("docNo", workFlowItem.docNo);
				inputSet.setField("parDocNo", workFlowItem.parDocNo);
				inputSet.setField("embAcctNo", workFlowItem.embAcctNo);
				inputSet.setField("checkNum", workFlowItem.checkNum);
				inputSet.setField("checkAmt", workFlowItem.checkAmt);
				inputSet.setField("paidAmt", workFlowItem.paidAmt);
				inputSet.setField("extPayorCd", workFlowItem.extPayorCd);
				inputSet.setField("depCode", workFlowItem.depCode);
				inputSet.setField("docGroup", workFlowItem.docGroup);
				inputSet.setField("docDetail", workFlowItem.docDetail);
				inputSet.setField("courier_Inst_Id", workFlowItem.courier_Inst_Id == null ? 0M : (decimal)workFlowItem.courier_Inst_Id);
				inputSet.setField("acctNum", workFlowItem.acctNum);
				inputSet.setField("act_Name", workFlowItem.act_Name);
				inputSet.setField("map_Name", workFlowItem.map_Name);
				inputSet.setField("act_Node_Id", workFlowItem.act_Node_Id == null ? 0 : (int)workFlowItem.act_Node_Id);
				inputSet.setField("map_Inst_Id", workFlowItem.map_Inst_Id == null ? 0 : (int)workFlowItem.map_Inst_Id);
				inputSet.setField("svcDate", workFlowItem.svcDate == null ? DateTime.MinValue : (DateTime)workFlowItem.svcDate);
				inputSet.setField("docPage", workFlowItem.docPage == null ? 0 : (int)workFlowItem.docPage);
				inputSet.setField("postdetail_Id", workFlowItem.postdetail_Id == null ? 0M : (decimal)workFlowItem.postdetail_Id);
				inputSet.setField("regionLeft", workFlowItem.regionLeft == null ? 0M : (decimal)workFlowItem.regionLeft);
				inputSet.setField("regionTop", workFlowItem.regionTop == null ? 0M : (decimal)workFlowItem.regionTop);
				inputSet.setField("finClass", workFlowItem.finClass);
				inputSet.setField("escReason", workFlowItem.escReason);
				inputSet.setField("corrFolder", workFlowItem.corrFolder);
				inputSet.setField("serviceId", workFlowItem.serviceId);
				inputSet.setField("reason", workFlowItem.reason);
				inputSet.setField("informational", workFlowItem.informational);

                _result = _commander.executeRpc(_rpc);

                IqResultSet resultSet = _result.getResultSet("outSet");
                rpcOutMessage.OutDataTable = CommonFunctions.RecordSetToDataTable(resultSet.convertToRecordset(),
                    "create_wfitems", "create_wfitems");
                rpcOutMessage.OutMessage = _result.getString("outParams");
                rpcOutMessage.IsSuccess = _result.getBoolean("outResult");

                Log.DebugFormat("Exit CreateWorkflowItem() Status:[{0}]", rpcOutMessage.IsSuccess);
                return rpcOutMessage;
            }
            finally
            {
                tearDown();
            }
        }

        public RpcOutMessage CreateWorkflowItems(string workstation, string username, List<NavWorkFlowItem> workFlowItems)
        {
            Log.Debug("Exit CreateWorkflowItems()");

            var outMessage = new RpcOutMessage();

            foreach (var workFlowItem in workFlowItems)
            {
                outMessage = CreateWorkflowItem(workstation, username, workFlowItem);
            }

            Log.Debug("Exit CreateWorkflowItems()");
            return outMessage;
        }

        public RpcOutMessage ImportImages(string workstation, string username, string userpwd, List<NavImage> navImages)
        {
            Log.Debug("Enter ImportImages()");

            EncryptionFuncs Encrypter = null;
            ImageProcessor ImageProc = null;
            RpcOutMessage rpcOutMessage = null;
            SftpClient SFtp = null;
#if LIVE
            string strSFTPserver = "prog-dsvc";
#else
            string strSFTPserver = "rtitsta";
#endif
            try
            {
                int iNumPage = 0;
                string strRet = "";
                string strTemp = "";
                string strPwd = "";
                string strCheckFile = "";

                rpcOutMessage = new RpcOutMessage();

                //decrypt pwd
                Encrypter = new EncryptionFuncs();
                //strPwd = Encrypter.TripleDESEncode(strPwd) on client side;
                strPwd = Encrypter.TripleDESDecode(userpwd);
                Encrypter = null;
                //open SFTP session(prog-dsvc/rtitsta)
                SFtp = new SftpClient(strSFTPserver, username, strPwd);

                //change to the right unix path
                //LL - format file and get page count
                ImageProc = new ImageProcessor();
                foreach (var ImageItem in navImages)
                {
                    if (ImageItem.filename.Split('|')[0].ToUpper().EndsWith(".TIF"))
                    {
                        ImageItem.nPages = ImageProc.getTIFFPageCount(ImageItem.filename.Split('|')[0]);
                        if (ImageItem.nPages < 0)
                        {
                            rpcOutMessage.IsSuccess = false;
                            rpcOutMessage.OutMessage = "Can't get page count for " + ImageItem.filename;
                            return rpcOutMessage;
                        }
                    }
                    else
                    {
                        if (ImageItem.filename.Split('|')[0].ToUpper().EndsWith(".PDF"))
                        {
                            ImageProc.convertPDF2TIF(ImageItem.filename.Split('|')[0],
                                                     ImageItem.filename.Split('|')[0].Substring(0, ImageItem.filename.Split('|')[0].Length - 4) + ".tif",
                                                     1,
                                                     ref iNumPage,
                                                     ref strRet);

                            if (!strRet.Equals(""))
                            {
                                rpcOutMessage.IsSuccess = false;
                                rpcOutMessage.OutMessage = "Error on convert " + ImageItem.filename.Split('|')[0] + " - " + strRet;
                                return rpcOutMessage;
                            }
                            else
                            {
                                ImageItem.nPages = iNumPage;
                                strTemp = ImageItem.filename.Split('|')[0].Substring(0, ImageItem.filename.Split('|')[0].Length - 4) + ".tif" + "|" +
                                                     ImageItem.filename.Split('|')[1].Substring(0, ImageItem.filename.Split('|')[1].Length - 4) + ".tif";
                                ImageItem.filename = strTemp;
                            }
                        }
                    }
                    //DateTime sdt1 = DateTime.Now;
                    var localCheckSum = Rti.CommonFunctions.Md5Hash(ImageItem.filename.Split('|')[0]);
                    strCheckFile = ImageItem.filename.Split('|')[0].Substring(0, ImageItem.filename.Split('|')[0].Length - 4) +
                                       "chk" + ImageItem.filename.Split('|')[0].Substring(ImageItem.filename.Split('|')[0].Length - 4);

                    bool blRet = SFtp.Put(ImageItem.filename.Split('|')[0], ImageItem.filename.Split('|')[1]);
                    if (blRet)
                    {
                        //DateTime sdt2 = DateTime.Now;
                        blRet = SFtp.Get(ImageItem.filename.Split('|')[1], strCheckFile);
                        DateTime sdt3 = DateTime.Now;
                        var remoteCheckSum = Rti.CommonFunctions.Md5Hash(strCheckFile);
                        if (localCheckSum != remoteCheckSum)
                        {
                            //Error for SFTP
                            blRet = false;
                        }
                    }
                    SFtp = null; //close SFTP
                    if (!blRet)
                    {
                        //Error for SFTP
                        rpcOutMessage.OutMessage = "ImportImages() - SFTP Failed on " + ImageItem.filename.Split('|')[0];
                        rpcOutMessage.IsSuccess = false;

                        Log.Debug("ImportImages() - SFTP Failed on " + ImageItem.filename.Split('|')[0]);
                        return rpcOutMessage;
                    }

                    if (System.IO.File.Exists(strCheckFile))
                    {
                        System.IO.File.Delete(strCheckFile);
                    }
                    if (System.IO.File.Exists(ImageItem.filename.Split('|')[0]))
                    {
                        System.IO.File.Delete(ImageItem.filename.Split('|')[0]);
                    }
                    //DateTime sdt4 = DateTime.Now;
                }
                //close SFTP session
                ImageProc = null;
                setUp();

                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                String rpcName = "import_image";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                IqInputSet inputSet;
                inputSet = _input.getInputSet("inSet");
                
                _input.setParam("user_id", username);

                foreach (var navImage in navImages)
                {
                    inputSet.addRow();
                    inputSet.setField("action", navImage.action);
                    inputSet.setField("filename", navImage.filename.Split('|')[1].ToString());
                    inputSet.setField("seq", navImage.seq == null ? 0 : (int) navImage.seq);
                    inputSet.setField("docno", navImage.docNo);
                    inputSet.setField("pardocno", navImage.parDocNo);
                    inputSet.setField("origdocno", navImage.origDocNo);
                    inputSet.setField("doctype", navImage.docType);
                    inputSet.setField("docdet", navImage.docDet);
                    inputSet.setField("docsrc", navImage.docSrc);
                    inputSet.setField("acctno", navImage.acctNo);
                    inputSet.setField("practice", navImage.practice);
                    inputSet.setField("div", navImage.div);
                    inputSet.setField("serviceid", navImage.serviceId);
                    inputSet.setField("deptcode", navImage.deptCode);
                    inputSet.setField("extpayor", navImage.extPayor);
                    inputSet.setField("checkamt", navImage.checkAmt);
                    inputSet.setField("checknum", navImage.checkNum);
                    inputSet.setField("checkdt",
                        navImage.checkDt == null ? DateTime.MinValue.ToShortDateString() : navImage.checkDt);
                    inputSet.setField("npages", navImage.nPages == null ? 0 : (int) navImage.nPages);
                    inputSet.setField("cstatus", navImage.cStatus);
                    inputSet.setField("supervisorid", navImage.supervisorId);
                }

                _result = _commander.executeRpc(_rpc);

                IqResultSet resultSet = _result.getResultSet("outSet");
                rpcOutMessage.OutDataTable = CommonFunctions.RecordSetToDataTable(resultSet.convertToRecordset(),
                    "import_image", "import_image");
                rpcOutMessage.OutMessage = _result.getString("outParams");
                rpcOutMessage.IsSuccess = _result.getBoolean("outResult");

                Log.Debug("Exit ImportImages()");
                return rpcOutMessage;
            }
            finally
            {
                if (ImageProc != null)
                {
                    ImageProc = null;
                }
                if (Encrypter != null)
                {
                    Encrypter = null;
                }
                if (SFtp != null)
                {
                    SFtp = null;
                }
                tearDown();
            }
        }

        public RpcOutMessage ImportImage(string workstation, string username, NavImage navImage)
        {
            Log.Debug("Enter ImportImage()");

            try
            {
                setUp();

                var rpcOutMessage = new RpcOutMessage();

                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                String rpcName = "import_image";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                IqInputSet inputSet;
                inputSet = _input.getInputSet("inSet");
                
                _input.setParam("user_id", username);

                inputSet.addRow();
                inputSet.setField("action", navImage.action);
                inputSet.setField("filename", navImage.filename);
                inputSet.setField("seq", navImage.seq == null ? 0 : (int)navImage.seq);
                inputSet.setField("docno", navImage.docNo);
                inputSet.setField("pardocno", navImage.parDocNo);
                inputSet.setField("origdocno", navImage.origDocNo);
                inputSet.setField("doctype", navImage.docType);
                inputSet.setField("docdet", navImage.docDet);
                inputSet.setField("docsrc", navImage.docSrc);
                inputSet.setField("acctno", navImage.acctNo);
                inputSet.setField("practice", navImage.practice);
                inputSet.setField("div", navImage.div);
                inputSet.setField("serviceid", navImage.serviceId);
                inputSet.setField("deptcode", navImage.deptCode);
                inputSet.setField("extpayor", navImage.extPayor);
                inputSet.setField("checkamt", navImage.checkAmt);
                inputSet.setField("checknum", navImage.checkNum);
                inputSet.setField("checkdt", navImage.checkDt == null ? DateTime.MinValue.ToShortDateString() : navImage.checkDt);
                inputSet.setField("npages", navImage.nPages == null ? 0 : (int)navImage.nPages);
                inputSet.setField("cstatus", navImage.cStatus);
                inputSet.setField("supervisorid", navImage.supervisorId);

                _result = _commander.executeRpc(_rpc);

                IqResultSet resultSet = _result.getResultSet("outSet");
                rpcOutMessage.OutDataTable = CommonFunctions.RecordSetToDataTable(resultSet.convertToRecordset(),
                    "import_image", "import_image");
                rpcOutMessage.OutMessage = _result.getString("outParams");
                rpcOutMessage.IsSuccess = _result.getBoolean("outResult");

                Log.Debug("Exit ImportImage()");
                return rpcOutMessage;
            }
            finally
            {
                tearDown();
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private void setUp()
        {
            if (!ConnectionFactory.isConfigured("default"))
            {
                String configFileName = _basePath + "\\SaIqClientRTI.xml";
                String xmlDir = null;
                ConnectionFactory.configure("default", configFileName, xmlDir);
            }
            //ML reset the serverName to its default. 8/14/2008
            //Added below condition to check for blank.1/13/2015.
            if(_serverName == "")
            _serverName = "mpi";
            if (_iqConn == null)
            {
                _iqConn = ConnectionFactory.getInstance("default").getConnection(_serverName);
            }
        }

        protected void tearDown()
        {
            if (_iqConn != null)
            {
                if (_iqConn.isOpen())
                    _iqConn.close();
                _iqConn = null;
                _commander = null;
            }
        }
    
        public DataTable CreateR2P(string sPractice, string sDocDet, string sDocType, string sParDocNo,
            double dDollarAmt, string sExtPayor, string sCheckNum, string sAction)
        {
            Log.Debug("Enter CreateR2P()");

            try
            {
                this.setUp();

                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                String rpcName = "creater2p";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                // left side must match up with RPC definition
                // right side is the methods names
                _input.setParam("Practice", sPractice);
                _input.setParam("DocDet", sDocDet);
                _input.setParam("DocType", sDocType);
                _input.setParam("ParDocNo", sParDocNo);
                _input.setParam("DollarAmt", dDollarAmt);
                _input.setParam("ExtPayor", sExtPayor);
                _input.setParam("CheckNum", sCheckNum);
                _input.setParam("Action", sAction);

                _result = _commander.executeRpc(_rpc);

                Recordset rs = new RecordsetClass();
                rs.CursorType = CursorTypeEnum.adOpenStatic;
                rs.CursorLocation = CursorLocationEnum.adUseClient;

                rs.Fields.Append("success", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldUnspecified, Type.Missing);

                rs.Open(Type.Missing, Type.Missing, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1);

                rs.AddNew(Type.Missing, Type.Missing);

                rs.Fields["success"].Value = _result.getBoolean("success");

                rs.Update(Type.Missing, Type.Missing);

                rs.MoveFirst();

                Log.Debug("Exit CreateR2P()");
                return CommonFunctions.RecordSetToDataTable(rs, "creater2p", "creater2p");
            }
            finally
            {
                this.tearDown();
            }
        }

        public DataTable GetCashItemsByDocNo(string strWorkstation, string strUser, string docNo)
        {
            Log.DebugFormat("Enter GetCashItemsByDocno() docNo:[{0}]", docNo);
            
            DataTable dt = null;

            try
            {
                DateTime sdt = DateTime.Now;
                _serverName = "longrpc";
                setUp();
                _commander = _iqConn.open();
                _commander.setAutoXA(false);

                string rpcName = "getC2P";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                _inputSet = _input.getInputSet("inttFilter");

                _input.setParam("cstatus_flag", "Show only active items");  // filters.Status
                _input.setParam("suspend_flag", "Show all cash items");  // filters.PostingSuspends
                _input.setParam("match_flag", "Show all items");    // filters.MatchStatus

                _input.setParam("inMaxRows", 500);  // iintMaxRows
                _input.setParam("inStartRow", 1);  // iintStartRow
                _input.setParam("inSortField", "");
                _input.setParam("inSortDir", "");
                _input.setParam("inDocNo", docNo);     // istrDocno

                _result = _commander.executeRpc(_rpc);

                IqResultSet resultSet = _result.getResultSet("outcash2post");

                var outParams = _result.getString("outParams");
                var outResult = _result.getBoolean("outResult");

                DateTime sdt1 = DateTime.Now;
                Recordset rs = resultSet.convertToRecordset();
                DateTime sdt2 = DateTime.Now;
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter();
                DataSet ds = new DataSet("AssemblyInfo");
                da.Fill(ds, rs, "CashInfo");
                Log.Debug("Exit GetCashItems");

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables["CashInfo"];
                }
                DateTime sdt3 = DateTime.Now;

                Log.Debug("Exit GetCashItemsByDocNo()");
                return dt;
            }
            catch
            {
                Log.Debug("Exit GetCashItemsByDocNo()");
                return dt;
            }
            finally
            {
                tearDown();
            }
        }

        public DataTable GetRemitItemsByDocNo(string strWorkstation, string strUser, string docNo)
        {
            Log.DebugFormat("Enter GetRemitItemsByDocno() docNo:[{0}]", docNo);

            DataTable dt = null;
            try
            {
                DateTime sdt = DateTime.Now;
                _serverName = "longrpc";
                setUp();
                _commander = _iqConn.open();
                _commander.setAutoXA(false);

                string rpcName = "getR2P";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();
                _inputSet = _input.getInputSet("inttFilter");

                _input.setParam("rstatus_flag", "Show only active items");  // filters.Status
                _input.setParam("suspend_flag", "Show all Remit items");  // filters.PostingSuspends
                _input.setParam("match_flag", "Show all items");    // filters.MatchStatus

                _input.setParam("inMaxRows", 500);  // iintMaxRows
                _input.setParam("inStartRow", 1);  // iintStartRow
                _input.setParam("inSortField", "");
                _input.setParam("inSortDir", "");
                _input.setParam("inDocNo", docNo);     // istrDocno

                _result = _commander.executeRpc(_rpc);

                IqResultSet resultSet = _result.getResultSet("outremit2post");

                var outParams = _result.getString("outParams");
                var outResult = _result.getBoolean("outResult");

                DateTime sdt1 = DateTime.Now;
                Recordset rs = resultSet.convertToRecordset();
                DateTime sdt2 = DateTime.Now;
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter();
                DataSet ds = new DataSet("AssemblyInfo");
                da.Fill(ds, rs, "RemitInfo");
                Log.Debug("Exit GetRemitItems");

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables["RemitInfo"];
                }
                DateTime sdt3 = DateTime.Now;
    
                Log.Debug("Exit GetRemitItemsByDocno()");
                return dt;
            }
            catch
            {
                Log.Debug("Exit GetRemitItemsByDocno()");
                return dt;
            }
            finally
            {
                tearDown();
            }
        }

        /// <summary>
        /// 1. GetServiceID: 
        ///    PracServiceMode(ServerName, Practice, rtnMessage, rtnSuccess, isERBilling, isERCoding, isFacBilling, isFacCoding)
        ///    GetServiceID = "B"
        ///    If (isERBilling And Not isFacBilling) Then
        ///        GetServiceID = "P"
        ///    End If
        ///    If (isFacBilling And Not isERBilling) Then
        ///        GetServiceID = "F"
        ///    End If
        ///
        ///2.   GetWFSupervisorID: 
        ///     PracServiceMode(ServerName, "USERID=" & istrUserID, rtnMessage, rtnSuccess, isERBilling, isERCoding, isFacBilling, isFacCoding) 
        ///     GetWFSupervisorID = rtnMessage
        /// </summary>
        public bool PracServiceMode(string iServerName, string inPractice, out string rtnMessage, out string rtnSuccess, out bool isERBilling, out bool isERCoding, out bool isFacBilling, out bool isFacCoding)
        {
            Log.Debug("Enter PracServiceMode()");

            try
            {
                if (iServerName != "")						//Check to see if user gave servername
                {											//--if not use default ("mpi")
                    _serverName = iServerName;				//set the servername to inputed name
                }
                setUp();
                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                String rpcName = "PracServiceMode";
                _rpc = IqRpcFactory.getInstance(_serverName).getRpc(rpcName, "1");
                _input = _rpc.getInputData();

                _input.setParam("contextString", "contextString");
                _input.setParam("inPractice", inPractice);

                _result = _commander.executeRpc(_rpc);

                rtnMessage = _result.getString("outMessage");
                isERBilling = _result.getBoolean("outERBilling");
                isERCoding = _result.getBoolean("outERCoding");
                isFacBilling = _result.getBoolean("outFacBilling");
                isFacCoding = _result.getBoolean("outFacCoding");
                rtnSuccess = _result.getBoolean("outSuccess").ToString();

                Log.Debug("Exit PracServiceMode()");
                // this DataTable that is returned is open
                return _result.getBoolean("outSuccess");
            }
            finally
            {
                tearDown();
            }
        }
        public ProcNoteResult ProcessNote(string workstaiton, string strUserId, int intNoteKey, string strNote, string strType, string strAction)
        {
            Log.Debug("Enter ProcessNote()");
            ProcNoteResult rpcRet = new ProcNoteResult();
            ProcNotes procNote = null;

            try
            {

                procNote = new ProcNotes("mpi", strUserId, intNoteKey, strNote, strType, strAction);
                string strRet = procNote.CallRPC();

                procNote = null;
                if (strRet.Equals("") || strRet.Split('|').Length < 7)
                {
                    rpcRet.Success = false;
                    rpcRet.OutMsg = "Error: No data.";
                }
                else
                {
                    rpcRet.Success = (strRet.Split('|')[0].ToString().ToUpper().Equals("TRUE"));
                    rpcRet.NoteTxt = strRet.Split('|')[1].ToString();
                    rpcRet.CreateUId = strRet.Split('|')[2].ToString();
                    rpcRet.CreateDt = strRet.Split('|')[3].ToString();
                    rpcRet.ModUId = strRet.Split('|')[4].ToString();
                    rpcRet.ModDt = strRet.Split('|')[5].ToString();
                    rpcRet.OutMsg = strRet.Split('|')[6].ToString();
                }

                Log.Debug("Exit ProcessNote()");
                return rpcRet;
            }
            catch (EntityException)
            {
                Log.Debug("Exit GetCashItemsByDocNo()");
                rpcRet.Success = false;
                rpcRet.OutMsg = "Error: No data.";
                if (procNote != null)
                {
                    procNote = null;
                }
                return rpcRet;
            }
            finally
            {
            }
        }

        public IDGOutMessage SubmitDocuments(string workstation, string username, string userpwd, string strAction, List<NavImage> navImages, IDGInputParams idgparams)
        {
            Log.Debug("Enter SubmitDocuments()");

            var submitdocRet = new IDGOutMessage();
            var rpcOutMessage = new RpcOutMessage();
            bool blIsChkNeedsPrep = idgparams.IsChkNeedsPrep;
            bool blIsRemitOnly = idgparams.IsRemitOnly;
            string strMapName = idgparams.strMapName;
            string strActName = idgparams.strActName;
            long intTowId = 0;
            string strFileName = "";
            int intFnDs = 0;
            string intFnID = "";
            int postdetail_id = 0;
            string strFinClass = "";
            var selImageItem = new NavImage();
            bool blActionType = (strAction.ToUpper().Equals("ADDDOC"));
            PostDetail postdtl = null;
            PostBd postbd = null;
            EncryptionFuncs Encrypter = null;
            DataTable postdetailTb = null;
          
#if LIVE
            ServiceReferenceFlowareWebLive.FlowareWebServiceManagerClient flowareClient = null;
#else
            ServiceReferenceFlowareWebTest.FlowareWebServiceManagerClient flowareClient = null;
#endif

            try
            {
                //initial outputs
                submitdocRet.IsSuccess = true;
                submitdocRet.OutMessage = "";
                submitdocRet.strTowId = "0";
                submitdocRet.strCourierInstId = "0";
                submitdocRet.strWfitemkey = "0";
                submitdocRet.strWfthreadkey = "0";

                // Start with 6 Steps:
                // 1. ImportImages
                Log.Debug("Enter SubmitDocuments() - " + strAction);
                if (blActionType)
                {
                    rpcOutMessage = ImportImages(workstation, username, userpwd, navImages);
                    if (!rpcOutMessage.IsSuccess)
                    {
                        Log.Debug("SubmitDocuments() - ImportImages " + navImages[0].parDocNo + " Error:" + rpcOutMessage.OutMessage);
                        submitdocRet.IsSuccess = rpcOutMessage.IsSuccess;
                        submitdocRet.OutMessage = "ImportImages " + navImages[0].parDocNo + " Error:" + rpcOutMessage.OutMessage;
                        submitdocRet.strTowId = "0";
                        submitdocRet.strCourierInstId = "0";
                        submitdocRet.strWfitemkey = "0";
                        submitdocRet.strWfthreadkey = "0";
                        return submitdocRet;
                    }
                    //Retrieving  the first row 
                    strFileName = rpcOutMessage.OutDataTable.Rows[0]["filename"].ToString();
                    intTowId = Convert.ToInt64(rpcOutMessage.OutDataTable.Rows[0]["outTowId"]);
                    intFnDs = Convert.ToInt32(rpcOutMessage.OutDataTable.Rows[0]["outIFNDs"]);
                    intFnID = rpcOutMessage.OutDataTable.Rows[0]["outIFNId"].ToString();
                    foreach (var ImageItem in navImages)
                    {
                        if (ImageItem.filename.Contains(strFileName))
                        {
                            selImageItem = ImageItem;
                            break;
                        }
                    }
                    submitdocRet.strTowId = intTowId.ToString();  //LL - ImportImages Ok

                    // 2. Create postdetail 
                    if ((!(blIsChkNeedsPrep)) || (strMapName == "RTI Remit Processing"))
                    {
                        var postDetail = new NavPostDetail();
                        postDetail.TOWID = intTowId;
                        postDetail.SVCDT = DateTime.Now;
                        postDetail.PAIDAMT = Convert.ToDecimal(selImageItem.checkAmt);
                        postDetail.EMBACCT = selImageItem.acctNo;
                        postDetail.ROUTEINFO = "";
                        postDetail.STATUS = "X";
                        postDetail.DOCPAGE = 1;
                        postDetail.MODIFYUID = username;
                        postDetail.MODIFYDATE = DateTime.Now;
                        postDetail.CREATEUID = username;
                        postDetail.CREATEDATE = DateTime.Now;
                        postDetail.TOP = 0;
                        postDetail.LEFT = 0;
                        postDetail.HEIGHT = 0;
                        postDetail.WIDTH = 0;
                        postDetail.XSCALE = 0;
                        postDetail.YSCALE = 0;
                        postDetail.ORIENTATION = 0;
                        postDetail.HSCROLL = 0;
                        postDetail.VSCROLL = 0;
                        postDetail.IFN = intFnID;
                        postDetail.NEXT_ACT_NAME = "";
                        postDetail.NEXT_MAP_NAME = "";
                        postDetail.REASON = idgparams.strReason;
                        postDetail.INFORMATIONAL = idgparams.strInformational;
                        postDetail.PRACTICE = selImageItem.practice;
                        postDetail.IFNDS = intFnDs;
                        postDetail.IFNID = Convert.ToInt32(intFnID);
                        postDetail.ORIG_TOWID = 0;
                        postDetail.ALT_TOWID = 0;
                        postDetail.COMINGLED_STATUS = "N";
                        postDetail.SERVICEID = selImageItem.serviceId;

                        postdtl = new PostDetail();
                        //Log
                        uint postdid = postdtl.InsertIntoPostDetail(workstation, username, postDetail);
                        postdtl = null;

                        postdetail_id = Convert.ToInt32(postdid);

                        if (postdetail_id < 1)
                        {
                            Log.Debug("Enter SubmitDocuments() - Create PostDetail Error: Can't create postdetail for " + intTowId.ToString());

                            submitdocRet.IsSuccess = false;
                            submitdocRet.OutMessage = rpcOutMessage.OutMessage + "; Error - Can't create postdetail for " + intTowId.ToString();

                            return submitdocRet;
                        }
                        submitdocRet.strPostdetailId = postdetail_id.ToString();
                    }

                    //Needs Prep Selected - handled on client side
                    //if (blIsChkNeedsPrep == true)
                    //{
                    //    strMapName = "RTI Bulk Post-Billing";
                    //    strActName = "RTI Prep Documents";
                    //}

                    // 3. if check call deplog
                    if ((selImageItem.docType == "CK" || selImageItem.docType == "CE") && (!selImageItem.docDet.Equals("")))
                    {
                        //log
                        //Is this cash2post
                        var depositLogInsertResults = InsertDeplog(selImageItem.practice, selImageItem.checkDt, selImageItem.docDet,
                                                      selImageItem.docType, selImageItem.parDocNo, Convert.ToString(DateTime.Today),
                                                      Convert.ToDouble(selImageItem.checkAmt), selImageItem.extPayor, selImageItem.checkNum,
                                                      Convert.ToInt32(selImageItem.seq), selImageItem.serviceId);
                        if (depositLogInsertResults.Rows.Count == 0)
                        {
                            Log.Debug("SubmitDocuments() - Create deplog Error: Can't create deplog for " + selImageItem.parDocNo);

                            submitdocRet.IsSuccess = false;
                            submitdocRet.OutMessage = rpcOutMessage.OutMessage + "; Error on creating deplog for " + selImageItem.parDocNo;

                            return submitdocRet;
                        }
                    }
                    // 4. if remit only call CreateR2P
                    else if (selImageItem.docType == "RM" && blIsRemitOnly)
                    {
                        //log
                        var remitToPost = CreateR2P(selImageItem.practice, selImageItem.docDet, selImageItem.docType, selImageItem.parDocNo,
                                                    Convert.ToDouble(selImageItem.checkAmt), selImageItem.extPayor, selImageItem.checkNum, "F2P");
                        if (remitToPost.Rows.Count == 0)
                        {
                            Log.Debug("SubmitDocuments() - Create Remit2post Error: Can't create Remit2post for " + selImageItem.parDocNo);

                            submitdocRet.IsSuccess = false;
                            submitdocRet.OutMessage = rpcOutMessage.OutMessage + "; Error on creating remit2post for " + selImageItem.parDocNo;
                            return submitdocRet;
                        }
                    }
                } //LL - for "AddDoc"
                else
                {
                    selImageItem = navImages[0];
                    intTowId = Convert.ToInt64(selImageItem.action);
                    strFinClass = selImageItem.filename; 
                    postdetailTb = new DataTable();
                    postdtl = new PostDetail();
                    postdetailTb = postdtl.GetPostDetailByTowerId(workstation, username, (int)intTowId);
                    postdtl = null;
                    if ((postdetailTb.Rows.Count != 0))
                    {
                        postdetail_id = Convert.ToInt32(postdetailTb.Rows[0]["POSTDETAIL_ID"]);
                    }
                    else
                    {
                        postdetail_id = 0;
                    }
                    postdetailTb = null;
                }

                // 5. Call floware
                if (intTowId != 0 && (!blIsRemitOnly) && (!strActName.Equals("")))
                {
                    //postdetail and postdoc tables are joined based on ifnds and ifnid with  towerid as input. 
#if LIVE
                    flowareClient = new ServiceReferenceFlowareWebLive.FlowareWebServiceManagerClient();
#else
                    flowareClient = new ServiceReferenceFlowareWebTest.FlowareWebServiceManagerClient();
#endif
                    var courier_inst_id = flowareClient.createWorkItem(strActName, (int)intTowId);
                    flowareClient = null;
                    if (courier_inst_id < 1)
                    {
                        Log.Debug("SubmitDocuments() - Create Floware item Error: Can't create Floware item for Activity:" + strActName + ";TowId:" + intTowId.ToString());

                        submitdocRet.IsSuccess = false;
                        submitdocRet.OutMessage = rpcOutMessage.OutMessage + "; Error on creating Floware item for Activity:" + strActName + ";TowId:" + intTowId.ToString();

                        return submitdocRet;
                    }
                    submitdocRet.strCourierInstId = courier_inst_id.ToString();

                    //To retrieve Act_Node_Id and Map_Inst_Id
                    postbd = new PostBd();
                    //log
                    var results = postbd.GetPostBdMapInstIdNodeId(workstation, username, courier_inst_id);
                    postbd = null;
                    var mapInstId = results.Item1;
                    var actInstId = results.Item2;

                 // 6. Create wfitem/wfthread
                        
                    var workFlowItem = new NavWorkFlowItem();
                    workFlowItem.practice = selImageItem.practice;
                    workFlowItem.division = selImageItem.div;
                    DateTime? depdt = (selImageItem.checkDt != null) ? DateTime.Parse(selImageItem.checkDt) : DateTime.Now;
                    workFlowItem.depDate = depdt;
                    workFlowItem.recDate = DateTime.Now;
                    workFlowItem.docType = selImageItem.docType;
                    workFlowItem.docNo = selImageItem.docNo;
                    workFlowItem.parDocNo = selImageItem.parDocNo;
                    workFlowItem.embAcctNo = selImageItem.acctNo;
                    workFlowItem.checkNum = selImageItem.checkNum;
                    workFlowItem.checkAmt = Convert.ToString(selImageItem.checkAmt);
                    workFlowItem.paidAmt = Convert.ToString(selImageItem.checkAmt); ;
                    workFlowItem.extPayorCd = selImageItem.extPayor;
                    workFlowItem.depCode = selImageItem.deptCode;
                    workFlowItem.docGroup = selImageItem.DocGroup;
                    workFlowItem.docDetail = selImageItem.docDet;
                    workFlowItem.courier_Inst_Id = courier_inst_id;
                    workFlowItem.acctNum = selImageItem.acctNo;
                    workFlowItem.act_Name = strActName;
                    workFlowItem.map_Name = strMapName;
                    workFlowItem.act_Node_Id = (int?)actInstId;
                    workFlowItem.map_Inst_Id = (int?)mapInstId;
                    workFlowItem.svcDate = DateTime.Now;
                    workFlowItem.docPage = 1; // selImageItem.nPages;
                    workFlowItem.postdetail_Id = postdetail_id;
                    workFlowItem.regionLeft = null;
                    workFlowItem.regionTop = null;
                    workFlowItem.finClass = strFinClass;  
                    workFlowItem.escReason = "";
                    workFlowItem.corrFolder = "";
                    workFlowItem.serviceId = selImageItem.serviceId;
                    workFlowItem.reason = "";
                    workFlowItem.informational = idgparams.strInformational;
                    var createWorkflowResult = CreateWorkflowItem(workstation, username, workFlowItem);
                    int workflowitemkey = Convert.ToInt32(createWorkflowResult.OutDataTable.Rows[0]["wfitemkey"]);
                    int workflowthreadkey = Convert.ToInt32(createWorkflowResult.OutDataTable.Rows[0]["wfthreadkey"]);
                    if (workflowitemkey == 0)
                    {
                        Log.Debug("SubmitDocuments() - Create WorkFlow item Error: Map/Act:" + strMapName + "/" + strActName + ";Docno:" + selImageItem.docNo);

                        submitdocRet.IsSuccess = false;
                        submitdocRet.OutMessage = createWorkflowResult.OutMessage + "Create WorkFlow item Error: Map/Act:" + strMapName + "/" + strActName + ";Docno:" + selImageItem.docNo;
                        return submitdocRet;
                    }
                    submitdocRet.strWfitemkey = workflowitemkey.ToString();
                    submitdocRet.strWfthreadkey = workflowthreadkey.ToString();
                }

                Log.Debug("Exit SubmitDocuments() Status:[" + submitdocRet.IsSuccess + "]");

                return submitdocRet;
            }
            catch (Exception exception)
            {
                Log.Error("Exit SubmitDocuments(), FAIL:" + exception.Message);
                if (postdtl != null)
                {
                    postdtl = null;
                }
                if (postbd != null)
                {
                    postbd = null;
                }
                if (flowareClient != null)
                {
                    flowareClient = null;
                }
                if (Encrypter != null)
                {
                    Encrypter = null;
                }
                if (postdetailTb != null)
                {
                    postdetailTb = null;
                }
                submitdocRet.IsSuccess = false;
                submitdocRet.OutMessage = "SubmitDocuments() Exception Error: " + exception.Message;
                return submitdocRet; 
            }
        }
    }
}
