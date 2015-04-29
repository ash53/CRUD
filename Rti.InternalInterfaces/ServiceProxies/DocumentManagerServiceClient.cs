using System;
using System.Collections.Generic;
using System.Data;
using Rti.DataModel;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceContracts;

namespace Rti.InternalInterfaces.ServiceProxies
{
    public class DocumentManagerServiceClient : ServiceProxyBase<IDocumentManagerService>
    {
        public DocumentManagerServiceClient(string serviceEndpointUri, string serviceEndpointName)
            : base(serviceEndpointUri, serviceEndpointName)
        {
        }

        public bool IsAlive()
        {
            return Channel.IsAlive();
        }

        public NavServiceInfo GetServiceInfo(string workstation, string username)
        {
            return Channel.GetServiceInfo(workstation, username);
        }


        #region PostBd
        
        public Tuple<decimal?, decimal?> GetPostBdMapInstIdNodeId(string workstation, string userid, decimal? courierInstId)
        {
            return Channel.GetPostBdMapInstIdNodeId(workstation, userid, courierInstId);
        }

        public DataTable GetPostBdWithC2PR2PDataMerged(string workstation, string username,
            string mapName = "", string actName = "", string practice = "", string division = "",
            string depDtStart = "", string depDtEnd = "", string embAcct = "", string reason = "", string checkNum = "",
            string checkAmountMin = "", string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "",
            string docType = "", string docDet = "", string who = "", string docNo = "", string parDocNo = "",
            bool includeMatched = false, int startRow = 1, int numRows = Constants.MAX_RESULTS_LIMIT)
        {
            return Channel.GetPostBdWithC2PR2PDataMerged(workstation, username, mapName, actName, practice,
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
            return Channel.SearchPostBd(workstation, username, mapName, actName, practice,
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
            return Channel.SearchPostBdCount(workstation, username, mapName, actName, practice,
                division, depDtStart, depDtEnd, embAcct, reason, checkNum, checkAmountMin,
                checkAmountMax, paidAmountMin, paidAmountMax, docType, docDet, who, docNo, 
                parDocNo);
        }

        public DataTable GetPostBDByPracticeAndAccount(string workstation, string username,
            string practice, string embAccount)
        {
            return Channel.GetPostBDByPracticeAndAccount(workstation, username, practice, embAccount);
        }

        public DataTable GetPostBDByTowId(string workstation, string username, int towId)
        {
            return Channel.GetPostBDByTowId(workstation, username, towId);
        }

        public DataTable GetPostBDByPostDetailId(string workstation, string username, int postDetailId)
        {
            return Channel.GetPostBDByPostDetailId(workstation, username, postDetailId);
        }

        public DataTable GetPostBDByUsername(string workstation, string username, string searchUsername)
        {
            return Channel.GetPostBDByUsername(workstation, username, searchUsername);
        }

        public DataTable GetPostBDByPracticeAndDepDate(string workstation, string username,
            string practice, string depDate)
        {
            return Channel.GetPostBDByPracticeAndDepDate(workstation, username, practice, depDate);
        }

        public DataTable GetPostBDByCheckNum(string workstation, string username, string checkNum)
        {
            return Channel.GetPostBDByCheckNum(workstation, username, checkNum);
        }

        public DataTable GetPostBDByPracticeAndCheckAmount(string workstation, string username,
            string practice, string checkAmount)
        {
            return Channel.GetPostBDByPracticeAndCheckAmount(workstation, username, practice, checkAmount);
        }

        public DataTable GetPostBDByDocNo(string workstation, string username, string docNo)
        {
            return Channel.GetPostBDByDocNo(workstation, username, docNo);
        }

        public DataTable GetPostBDByParDocNo(string workstation, string username, string parDocNo)
        {
            return Channel.GetPostBDByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetPostBDOrderedByMapAndActivity(string workstation, string username,
            string map, string activity)
        {
            return Channel.GetPostBDOrderedByMapAndActivity(workstation, username, map, activity);
        }

        public DataTable GetPostBDOrderedByMapAndActivityDepDt(string workstation, string username,
            string map, string activity)
        {
            return Channel.GetPostBDOrderedByMapAndActivityDepDt(workstation, username, map, activity);
        }

        public DataTable GetWorkItemsByIFN(string workstation, string username,
            string ifn)
        {
            return Channel.GetWorkItemsByIFN(workstation, username, ifn);
        }

        #endregion  // PostBd

        
        #region PostDoc

        public string GetPostDocExtServiceIdByTowIdInsertIfNull(string workstation, string userid, int towId,
            string serviceId)
        {
            return Channel.GetPostDocExtServiceIdByTowIdInsertIfNull(workstation, userid, towId, serviceId);
        }

        public DataTable GetPostDocByParDocNo(string workstation, string username, string parDocNo)
        {
            return Channel.GetPostDocByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetImportedPostDocByTowId(string workstation, string username, int towId)
        {
            return Channel.GetImportedPostDocByTowId(workstation, username, towId);
        }

        public DataTable GetImportedPostDocByDocNo(string workstation, string username, string docNo)
        {
            return Channel.GetImportedPostDocByDocNo(workstation, username, docNo);
        }

        public DataTable GetImportedPostDocByParDocNo(string workstation, string username, string parDocNo)
        {
            return Channel.GetImportedPostDocByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetImportedPostDocByPracticeAndAccount(string workstation, string username,
            string practice, string account)
        {
            return Channel.GetImportedPostDocByPracticeAndAccount(workstation, username, practice, account);
        }

        public DataTable GetPostDocStatusLookup(string workstation, string username)
        {
            return Channel.GetPostDocStatusLookup(workstation, username);
        }

        public DataTable GetPostDocSearchByParDocNo(string workstation, string username, string parDocNo)
        {
            return Channel.GetPostDocSearchByParDocNo(workstation, username, parDocNo);
        }

        public DataTable GetPostDocSearchByPracticeAndAccount(string workstation, string username,
            string practice, string account)
        {
            return Channel.GetImportedPostDocByPracticeAndAccount(workstation, username, practice, account);
        }

        public DataTable GetPostDocSearchByDocNo(string workstation, string username, string docNo)
        {
            return Channel.GetPostDocSearchByDocNo(workstation, username, docNo);
        }

        public DataTable GetPostDocSearchByCheckNum(string workstation, string username, string checkNum)
        {
            return Channel.GetPostDocSearchByCheckNum(workstation, username, checkNum);
        }

        public DataTable GetPostDocSearchBy(string workstation, string username, DateTime depDt)
        {
            return Channel.GetPostDocSearchBy(workstation, username, depDt);
        }

        public DataTable GetPostDocSearchByPracticeAndDeptDt(string workstation, string username,
            string practice, DateTime depDt)
        {
            return Channel.GetPostDocSearchByPracticeAndDeptDt(workstation, username, practice, depDt);
        }

        public DataTable GetPostDocByDateAndUser(string workstation, string username, DateTime date, 
            string scanuid)
        {
            return Channel.GetPostDocByDateAndUser(workstation, username, date, scanuid);
        }

        public bool UpdatePostDocNPagesByTowId(string workstation, string username, int nPages, int towId)
        {
            return Channel.UpdatePostDocNPagesByTowId(workstation, username, nPages, towId);
        }

        public bool InsertIntoPostDoc(string workstation, string username, POSTDOC postDoc)
        {
            return Channel.InsertIntoPostDoc(workstation, username, postDoc);
        }

        public DataTable GetDocNoFromPostDocsByParDocNo(string workstation, string username, string parDocNo)
        {
            return Channel.GetDocNoFromPostDocsByParDocNo(workstation, username, parDocNo);
        }

        #endregion  // PostDoc


        #region PostDetail

        public DataTable ListPostDetailStatusLookups(string workstation, string username)
        {
            return Channel.ListPostDetailStatusLookups(workstation, username);
        }

        public DataTable GetPostDetailByTowerId(string workstation, string username, int towId)
        {
            return Channel.GetPostDetailByTowerId(workstation, username, towId);
        }

        public DataTable GetPostDetailByPracticeAndAccount(string workstation, string username,
            string practice, string account)
        {
            return Channel.GetPostDetailByPracticeAndAccount(workstation, username, practice, account);
        }

        public DataTable GetPostDetailByPostDetailId(string workstation, string username, int postDetailId)
        {
            return Channel.GetPostDetailByPostDetailId(workstation, username, postDetailId);
        }

        public uint InsertIntoPostDetail(string workstation, string username, NavPostDetail postDetail)
        {
            return Channel.InsertIntoPostDetail(workstation, username, postDetail);
        }

        #endregion  // PostDetail

        
        #region Embillz

        public DataTable InsertDepositLog(string iPractice, string iDepositDate, string iDocDet, 
            string iDocType, string iParDocNo, string iCheckDate, double iCheckAmount, string iExtPayor, 
            string iCheckNumber, int iDepSequence)
        {
            return Channel.InsertDepositLog(iPractice, iDepositDate, iDocDet, iDocType, iParDocNo,
                iCheckDate, iCheckAmount, iExtPayor, iCheckNumber, iDepSequence);
        }

        public DataTable InsertDeplog(string iPractice, string iDepositDate, string iDocDet,
            string iDocType, string iParDocNo, string iCheckDate, double iCheckAmount, string iExtPayor,
            string iCheckNumber, int iDepSequence, string iServiceId)
        {
            return Channel.InsertDeplog(iPractice, iDepositDate, iDocDet, iDocType, iParDocNo,
                iCheckDate, iCheckAmount, iExtPayor, iCheckNumber, iDepSequence, iServiceId);
        }

        public RpcOutMessage CreateWorkflowItem(string workstation, string username, NavWorkFlowItem workFlowItem)
        {
            return Channel.CreateWorkflowItem(workstation, username, workFlowItem);
        }

        public RpcOutMessage CreateWorkflowItems(string workstation, string username,
            List<NavWorkFlowItem> workFlowItems)
        {
            return Channel.CreateWorkflowItems(workstation, username, workFlowItems);
        }

        public RpcOutMessage ImportImages(string workstation, string username, string userpwd, List<NavImage> navImages)
        {
            return Channel.ImportImages(workstation, username, userpwd, navImages);
        }

        public RpcOutMessage ImportImage(string workstation, string username, NavImage navImage)
        {
            return Channel.ImportImage(workstation, username, navImage);
        }

        public DataTable CreateR2P(string sPractice, string sDocDet, string sDocType, string sParDocNo,
            double dDollarAmt, string sExtPayor, string sCheckNum, string sAction)
        {
            return Channel.CreateR2P(sPractice, sDocDet, sDocType, sParDocNo, dDollarAmt, sExtPayor,
                sCheckNum, sAction);
        }

        public DataTable GetCashItemsByDocNo(string strWorkstation, string strUser, string docNo)
        {
            return Channel.GetCashItemsByDocNo(strWorkstation, strUser, docNo);
        }

        public DataTable GetRemitItemsByDocNo(string strWorkstation, string strUser, string docNo)
        {
            return Channel.GetRemitItemsByDocNo(strWorkstation, strUser, docNo);
        }

        public bool PracServiceMode(string iServerName, string inPractice, out string rtnMessage, out string rtnSuccess,
            out bool isERBilling, out bool isERCoding, out bool isFacBilling, out bool isFacCoding)
        {
            return Channel.PracServiceMode(iServerName, inPractice, out rtnMessage, out rtnSuccess, out isERBilling,
                out isERCoding, out isFacBilling, out isFacCoding);
        }

        public ProcNoteResult ProcessNote(string workstaiton, string strUserId, int intNoteKey, string strNote, string strType, string strAction)
        {
             return Channel.ProcessNote( workstaiton,  strUserId,  intNoteKey,  strNote,  strType,  strAction);
        }

        public IDGOutMessage SubmitDocuments(string workstation, string username, string userpwd, string strAction, List<NavImage> navImages, IDGInputParams idgparams)
        {
            return Channel.SubmitDocuments(workstation, username, userpwd, strAction, navImages, idgparams);
        }
        #endregion  // Embillz


        #region DocUtils

        public string CreateDocNo(string workstation, string username)
        {
            return Channel.CreateDocNo(workstation, username);
        }

        public DataTable ListStartRuleLookups(string workstation, string username)
        {
            return Channel.ListStartRuleLookups(workstation, username);
        }

        public DataTable ListRoutingInfo(string workstation, string username)
        {
            return Channel.ListRoutingInfo(workstation, username);
        }

        public DataTable ListPractices(string workstation, string username)
        {
            return Channel.ListPractices(workstation, username);
        }

        public DataTable ListDocTypeLookups(string workstation, string username)
        {
            return Channel.ListDocTypeLookups(workstation, username);
        }

        public int GetTowerSequenceNextValue(string workstation, string username)
        {
            return Channel.GetTowerSequenceNextValue(workstation, username);
        }

        public DataTable GetBillDocByDocNo(string workstation, string username, string docNo)
        {
            return Channel.GetBillDocByDocNo(workstation, username, docNo);
        }

        public DataTable GetPostDocByDocNo(string workstation, string username, string docNo)
        {
            return Channel.GetPostDocByDocNo(workstation, username, docNo);
        }

        public DataTable GetRtiBDByAccountName(string workstation, string username, string accountName)
        {
            return Channel.GetRtiBDByAccountName(workstation, username, accountName);
        }

        public DataTable GetStartActivityJoinedPostDetail(string workstation, string username)
        {
            return Channel.GetStartActivityJoinedPostDetail(workstation, username);
        }

        public DataTable GetSourceTypeLookupByTypeCode(string workstation, string username)
        {
            return Channel.GetSourceTypeLookupByTypeCode(workstation, username);
        }

        public DataTable GetDocTypes(string workstation, string username)
        {
            return Channel.GetDocTypes(workstation, username);
        }

        public DataTable GetLastTenCouriers(string workstation, string username, string loginName)
        {
            return Channel.GetLastTenCouriers(workstation, username, loginName);
        }

        public DataTable GetRtiBDByMapAndActivity(string workstation, string username,
            string map, string activity)
        {
            return Channel.GetRtiBDByMapAndActivity(workstation, username, map, activity);
        }

        public bool ConvertPDFtoTif(string workstation, string username, string pdfFileName, out string message)
        {
            return Channel.ConvertPDFtoTif(workstation, username, pdfFileName, out message);
        }

        public DataTable GetMapAndActivityByLoginName(string workstation, string username, string loginName)
        {
            return Channel.GetMapAndActivityByLoginName(workstation, username, loginName);
        }

        public DataTable GetNavC2PByParDocNoJoinPostDoc(string workstation, string username, string parDocNo)
        {
            return Channel.GetNavC2PByParDocNoJoinPostDoc(workstation, username, parDocNo);
        }

        public DataTable GetEiqNavC2PByDocTypeDocDet(string workstation, string username, string docType, string docDet)
        {
            return Channel.GetEiqNavC2PByDocTypeDocDet(workstation, username, docType, docDet);
        }

        public DataTable GetEiqNavC2PByDocTypeDocDetPractice(string workstation, string username,
            string docType, string docDet, string practice)
        {
            return Channel.GetEiqNavC2PByDocTypeDocDetPractice(workstation, username, docType, docDet, practice);
        }

        public DataTable GetNavC2PByParDocNo(string workstation, string username, string parDocNo)
        {
            return Channel.GetNavC2PByParDocNo(workstation, username, parDocNo);
        }

        #endregion  // DocUtils

    }
}
