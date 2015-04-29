using System.Collections.Generic;
using System;
using System.Data;
using System.ServiceModel;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceContracts;
using Rti.DataModel;

namespace Rti.DocumentManagerServer.Servers
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    [System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
    public class DocumentManagerServer : IDocumentManagerService
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DataAccess.PostBd _postBd;
        private static DataAccess.PostDoc _postDoc;
        private static DataAccess.Embillz _embillz;
        private static DataAccess.PostDetail _postDetail;
        private static DataAccess.DocUtils _docUtils;

        public DocumentManagerServer()
        {
            _postBd = new DataAccess.PostBd();
            _postDoc = new DataAccess.PostDoc();
            _embillz = new DataAccess.Embillz();
            _postDetail = new DataAccess.PostDetail();
            _docUtils = new DataAccess.DocUtils();
        }

        public bool IsAlive()
        {
            Log.Debug("Alive!!!");
            return true;
        }

        public NavServiceInfo GetServiceInfo(string workstation, string username)
        {
            return Rti.ServiceInfo.GetServiceInfo();
        }

       
        #region PostBd
        
        public Tuple<decimal?, decimal?> GetPostBdMapInstIdNodeId(string workstation, string userid, decimal? courierInstId)
        {
            return _postBd.GetPostBdMapInstIdNodeId(workstation, userid, courierInstId);
        }

        public DataTable GetPostBdWithC2PR2PDataMerged(string workstation, string username,
            string mapName = "", string actName = "", string practice = "", string division = "",
            string depDtStart = "", string depDtEnd = "", string embAcct = "", string reason = "", string checkNum = "",
            string checkAmountMin = "", string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "",
            string docType = "", string docDet = "", string who = "", string docNo = "", string parDocNo = "",
            bool includeMatched = false, int startRow = 1, int numRows = Constants.MAX_RESULTS_LIMIT)
        {
            return _postBd.GetPostBdWithC2PR2PDataMerged(workstation, username, mapName, actName, practice,
                division, depDtStart, depDtEnd, embAcct, reason, checkNum, checkAmountMin, checkAmountMax, 
                paidAmountMin, paidAmountMax, docType, docDet, who, docNo, parDocNo, includeMatched,
                startRow, numRows);
        }

        public DataTable SearchPostBd(string workstation, string username,
            string mapName = "", string actName = "", string practice = "", string division = "",
            string depDtStart = "", string depDtEnd = "", string embAcct = "", string reason = "", string checkNum = "",
            string checkAmountMin = "", string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "",
            string docType = "", string docDet = "", string who = "", string docNo = "", string parDocNo = "",
            int startRow = 1, int numRows = Constants.MAX_RESULTS_LIMIT)
        {
            return _postBd.SearchPostBd(workstation, username, mapName, actName, practice,
                division, depDtStart, depDtEnd, embAcct, reason, checkNum, checkAmountMin,
                checkAmountMax, paidAmountMin, paidAmountMax, docType, docDet, who, docNo, 
                parDocNo, startRow, numRows);
        }

        public int SearchPostBdCount(string workstation, string username, string mapName = "", string actName = "",
            string practice = "", string division = "", string depDtStart = "", string depDtEnd = "",
            string embAcct = "", string reason = "", string checkNum = "", string checkAmountMin = "",
            string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "", string docType = "",
            string docDet = "", string who = "", string docNo = "", string parDocNo = "")
        {
            return _postBd.SearchPostBdCount(workstation, username, mapName, actName, practice,
                division, depDtStart, depDtEnd, embAcct, reason, checkNum, checkAmountMin,
                checkAmountMax, paidAmountMin, paidAmountMax, docType, docDet, who, docNo, 
                parDocNo);
        }

        public DataTable GetPostBDByPracticeAndAccount(string workstation, string username,
            string practice, string embAccount)
        {
            return _postBd.GetPostBDByPracticeAndAccount(workstation, username, practice, embAccount);
        }

        public DataTable GetPostBDByTowId(string workstation, string username, int towId)
        {
            return _postBd.GetPostBDByTowId(workstation, username, towId);
        }

        public DataTable GetPostBDByPostDetailId(string workstation, string username, int postDetailId)
        {
            return _postBd.GetPostBDByPostDetailId(workstation, username, postDetailId);
        }

        public DataTable GetPostBDByUsername(string workstation, string username, string searchUsername)
        {
            return _postBd.GetPostBDByUsername(workstation, username, searchUsername);
        }

        public DataTable GetPostBDByPracticeAndDepDate(string workstation, string username,
            string practice, string depDate)
        {
            return _postBd.GetPostBDByPracticeAndDepDate(workstation, username, practice, depDate);
        }

        public DataTable GetPostBDByCheckNum(string workstation, string username, string checkNum)
        {
            return _postBd.GetPostBDByCheckNum(workstation, username, checkNum);
        }

        public DataTable GetPostBDByPracticeAndCheckAmount(string workstation, string username,
            string practice, string checkAmount)
        {
            return _postBd.GetPostBDByPracticeAndCheckAmount(workstation, username, practice, checkAmount);
        }

        public DataTable GetPostBDByDocNo(string workstation, string username, string docNo)
        {
            return _postBd.GetPostBDByDocNo(workstation, username, docNo);
        }

        public DataTable GetPostBDByParDocNo(string workstation, string username, string parDocNo)
        {
            return _postBd.GetPostBDByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetPostBDOrderedByMapAndActivity(string workstation, string username,
            string map, string activity)
        {
            return _postBd.GetPostBDOrderedByMapAndActivity(workstation, username, map, activity);
        }

        public DataTable GetPostBDOrderedByMapAndActivityDepDt(string workstation, string username,
            string map, string activity)
        {
            return _postBd.GetPostBDOrderedByMapAndActivityDepDt(workstation, username, map, activity);
        }

        public DataTable GetWorkItemsByIFN(string workstation, string username,
            string ifn)
        {
            return _postBd.GetWorkItemsByIFN(workstation, username, ifn);
        }

        #endregion  // PostBd

        
        #region PostDoc

        public string GetPostDocExtServiceIdByTowIdInsertIfNull(string workstation, string userid, int towId,
            string serviceId)
        {
            return _postDoc.GetPostDocExtServiceIdByTowIdInsertIfNull(workstation, userid, towId, serviceId);
        }

        public DataTable GetPostDocByParDocNo(string workstation, string username, string parDocNo)
        {
            return _postDoc.GetPostDocByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetImportedPostDocByTowId(string workstation, string username, int towId)
        {
            return _postDoc.GetImportedPostDocByTowId(workstation, username, towId);
        }

        public DataTable GetImportedPostDocByDocNo(string workstation, string username, string docNo)
        {
            return _postDoc.GetImportedPostDocByDocNo(workstation, username, docNo);
        }

        public DataTable GetImportedPostDocByParDocNo(string workstation, string username, string parDocNo)
        {
            return _postDoc.GetImportedPostDocByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetImportedPostDocByPracticeAndAccount(string workstation, string username,
            string practice, string account)
        {
            return _postDoc.GetImportedPostDocByPracticeAndAccount(workstation, username, practice, account);
        }

        public DataTable GetPostDocStatusLookup(string workstation, string username)
        {
            return _postDoc.GetPostDocStatusLookup(workstation, username);
        }

        public DataTable GetPostDocSearchByParDocNo(string workstation, string username, string parDocNo)
        {
            return _postDoc.GetPostDocSearchByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetPostDocSearchByPracticeAndAccount(string workstation, string username,
            string practice, string account)
        {
            return _postDoc.GetImportedPostDocByPracticeAndAccount(workstation, username, practice, account);
        }

        public DataTable GetPostDocSearchByDocNo(string workstation, string username, string docNo)
        {
            return _postDoc.GetPostDocSearchByDocNo(workstation, username, docNo);
        }

        public DataTable GetPostDocSearchByCheckNum(string workstation, string username, string checkNum)
        {
            return _postDoc.GetPostDocSearchByCheckNum(workstation, username, checkNum);
        }

        public DataTable GetPostDocSearchBy(string workstation, string username, DateTime depDt)
        {
            return _postDoc.GetPostDocSearchBy(workstation, username, depDt);
        }

        public DataTable GetPostDocSearchByPracticeAndDeptDt(string workstation, string username,
            string practice, DateTime depDt)
        {
            return _postDoc.GetPostDocSearchByPracticeAndDeptDt(workstation, username, practice, depDt);
        }

        public DataTable GetPostDocByDateAndUser(string workstation, string username, DateTime date, 
            string scanuid)
        {
            return _postDoc.GetPostDocByDateAndUser(workstation, username, date, scanuid);
        }

        public bool UpdatePostDocNPagesByTowId(string workstation, string username, int nPages, int towId)
        {
            return _postDoc.UpdatePostDocNPagesByTowId(workstation, username, nPages, towId);
        }

        public bool InsertIntoPostDoc(string workstation, string username, POSTDOC postDoc)
        {
            return _postDoc.InsertIntoPostDoc(workstation, username, postDoc);
        }

        public DataTable GetDocNoFromPostDocsByParDocNo(string workstation, string username, string parDocNo)
        {
            return _postDoc.GetDocNoFromPostDocsByParDocNo(workstation, username, parDocNo);
        }

        #endregion  // PostDoc


        #region PostDetail

        public DataTable ListPostDetailStatusLookups(string workstation, string username)
        {
            return _postDetail.ListPostDetailStatusLookups(workstation, username);
        }

        public DataTable GetPostDetailByTowerId(string workstation, string username, int towId)
        {
            return _postDetail.GetPostDetailByTowerId(workstation, username, towId);
        }

        public DataTable GetPostDetailByPracticeAndAccount(string workstation, string username,
            string practice, string account)
        {
            return _postDetail.GetPostDetailByPracticeAndAccount(workstation, username, practice, account);
        }

        public DataTable GetPostDetailByPostDetailId(string workstation, string username, int postDetailId)
        {
            return _postDetail.GetPostDetailByPostDetailId(workstation, username, postDetailId);
        }

        public uint InsertIntoPostDetail(string workstation, string username, NavPostDetail postDetail)
        {
            return _postDetail.InsertIntoPostDetail(workstation, username, postDetail);
        }

        #endregion  // PostDetail

        
        #region Embillz

        public DataTable InsertDepositLog(string iPractice, string iDepositDate, string iDocDet, 
            string iDocType, string iParDocNo, string iCheckDate, double iCheckAmount, string iExtPayor, 
            string iCheckNumber, int iDepSequence)
        {
            return _embillz.InsertDepositLog(iPractice, iDepositDate, iDocDet, iDocType, iParDocNo,
                iCheckDate, iCheckAmount, iExtPayor, iCheckNumber, iDepSequence);
        }

        public DataTable InsertDeplog(string iPractice, string iDepositDate, string iDocDet,
            string iDocType, string iParDocNo, string iCheckDate, double iCheckAmount, string iExtPayor,
            string iCheckNumber, int iDepSequence, string iServiceId)
        {
            return _embillz.InsertDeplog(iPractice, iDepositDate, iDocDet, iDocType, iParDocNo,
                iCheckDate, iCheckAmount, iExtPayor, iCheckNumber, iDepSequence, iServiceId);
        }

        public RpcOutMessage CreateWorkflowItem(string workstation, string username, NavWorkFlowItem workFlowItem)
        {
            return _embillz.CreateWorkflowItem(workstation, username, workFlowItem);
        }

        public RpcOutMessage CreateWorkflowItems(string workstation, string username,
            List<NavWorkFlowItem> workFlowItems)
        {
            return _embillz.CreateWorkflowItems(workstation, username, workFlowItems);
        }

        public RpcOutMessage ImportImages(string workstation, string username, string userpwd, List<NavImage> navImages)
        {
            return _embillz.ImportImages(workstation, username, userpwd, navImages);
        }

        public RpcOutMessage ImportImage(string workstation, string username, NavImage navImage)
        {
            return _embillz.ImportImage(workstation, username, navImage);
        }

        public DataTable CreateR2P(string sPractice, string sDocDet, string sDocType, string sParDocNo,
            double dDollarAmt, string sExtPayor, string sCheckNum, string sAction)
        {
            return _embillz.CreateR2P(sPractice, sDocDet, sDocType, sParDocNo, dDollarAmt, sExtPayor,
                sCheckNum, sAction);
        }

        public DataTable GetCashItemsByDocNo(string strWorkstation, string strUser, string docNo)
        {
            return _embillz.GetCashItemsByDocNo(strWorkstation, strUser, docNo);
        }

        public DataTable GetRemitItemsByDocNo(string strWorkstation, string strUser, string docNo)
        {
            return _embillz.GetRemitItemsByDocNo(strWorkstation, strUser, docNo);
        }

        public bool PracServiceMode(string iServerName, string inPractice, out string rtnMessage, out string rtnSuccess,
            out bool isERBilling, out bool isERCoding, out bool isFacBilling, out bool isFacCoding)
        {
            return _embillz.PracServiceMode(iServerName, inPractice, out rtnMessage, out rtnSuccess, out isERBilling,
                out isERCoding, out isFacBilling, out isFacCoding);
        }

        #endregion  // Embillz


        #region DocUtils

        public string CreateDocNo(string workstation, string username)
        {
            return _docUtils.CreateDocNo(workstation, username);
        }

        public DataTable ListStartRuleLookups(string workstation, string username)
        {
            return _docUtils.ListStartRuleLookups(workstation, username);
        }

        public DataTable ListRoutingInfo(string workstation, string username)
        {
            return _docUtils.ListRoutingInfo(workstation, username);
        }

        public DataTable ListPractices(string workstation, string username)
        {
            return _docUtils.ListPractices(workstation, username);
        }

        public DataTable ListDocTypeLookups(string workstation, string username)
        {
            return _docUtils.ListDocTypeLookups(workstation, username);
        }

        public int GetTowerSequenceNextValue(string workstation, string username)
        {
            return _docUtils.GetTowerSequenceNextValue(workstation, username);
        }

        public DataTable GetBillDocByDocNo(string workstation, string username, string docNo)
        {
            return _docUtils.GetBillDocByDocNo(workstation, username, docNo);
        }

        public DataTable GetPostDocByDocNo(string workstation, string username, string docNo)
        {
            return _docUtils.GetPostDocByDocNo(workstation, username, docNo);
        }

        public DataTable GetRtiBDByAccountName(string workstation, string username, string accountName)
        {
            return _docUtils.GetRtiBDByAccountName(workstation, username, accountName);
        }

        public DataTable GetStartActivityJoinedPostDetail(string workstation, string username)
        {
            return _docUtils.GetStartActivityJoinedPostDetail(workstation, username);
        }

        public DataTable GetSourceTypeLookupByTypeCode(string workstation, string username)
        {
            return _docUtils.GetSourceTypeLookupByTypeCode(workstation, username);
        }

        public DataTable GetDocTypes(string workstation, string username)
        {
            return _docUtils.GetDocTypes(workstation, username);
        }

        public DataTable GetLastTenCouriers(string workstation, string username, string loginName)
        {
            return _docUtils.GetLastTenCouriers(workstation, username, loginName);
        }

        public DataTable GetRtiBDByMapAndActivity(string workstation, string username,
            string map, string activity)
        {
            return _docUtils.GetRtiBDByMapAndActivity(workstation, username, map, activity);
        }

        public bool ConvertPDFtoTif(string workstation, string username, string pdfFileName, out string message)
        {
            return _docUtils.ConvertPDFtoTif(workstation, username, pdfFileName, out message);
        }

        public DataTable GetMapAndActivityByLoginName(string workstation, string username, string loginName)
        {
            return _docUtils.GetMapAndActivityByLoginName(workstation, username, loginName);
        }

        public DataTable GetNavC2PByParDocNoJoinPostDoc(string workstation, string username, string parDocNo)
        {
            return _docUtils.GetNavC2PByParDocNoJoinPostDoc(workstation, username, parDocNo);
        }

        public DataTable GetEiqNavC2PByDocTypeDocDet(string workstation, string username, string docType, string docDet)
        {
            return _docUtils.GetEiqNavC2PByDocTypeDocDet(workstation, username, docType, docDet);
        }

        public DataTable GetEiqNavC2PByDocTypeDocDetPractice(string workstation, string username, string docType, 
            string docDet, string practice)
        {
            return _docUtils.GetEiqNavC2PByDocTypeDocDetPractice(workstation, username, docType, docDet, practice);
        }

        public DataTable GetNavC2PByParDocNo(string workstation, string username, string parDocNo)
        {
            return _docUtils.GetNavC2PByParDocNo(workstation, username, parDocNo);
        }

        public ProcNoteResult ProcessNote(string workstaiton, string strUserId, int intNoteKey, string strNote, string strType, string strAction)
        {
            return _embillz.ProcessNote(workstaiton, strUserId, intNoteKey, strNote, strType, strAction);
        }

        public IDGOutMessage SubmitDocuments(string workstation, string username, string userpwd, string strAction, List<NavImage> navImages, IDGInputParams idgparams)
        {
            return _embillz.SubmitDocuments(workstation, username, userpwd, strAction, navImages, idgparams);
        }
        #endregion  // DocUtils
    }
}
