using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
using Rti.DataModel;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.InternalInterfaces.ServiceContracts
{
    [ServiceContract]
    [ServiceKnownType(typeof(NavPostDetail))]
    [ServiceKnownType(typeof(NavImage))]
    public interface IDocumentManagerService
    {
        [OperationContract]
        bool IsAlive();

        [OperationContract]
        NavServiceInfo GetServiceInfo(string workstation, string username);

        [OperationContract]
        Tuple<decimal?, decimal?> GetPostBdMapInstIdNodeId(string workstation, string userid, decimal? courierInstId);

        [OperationContract]
        string GetPostDocExtServiceIdByTowIdInsertIfNull(string workstation, string userid, int towId, string serviceId);

        [OperationContract]
        DataTable CreateR2P(string sPractice, string sDocDet, string sDocType, string sParDocNo,
            double dDollarAmt, string sExtPayor, string sCheckNum, string sAction);

        [OperationContract]
        DataTable GetEiqNavC2PByDocTypeDocDet(string workstation, string username, string docType, string docDet);

        [OperationContract]
        DataTable GetEiqNavC2PByDocTypeDocDetPractice(string workstation, string username,
            string docType, string docDet, string practice);

        [OperationContract]
        DataTable GetRemitItemsByDocNo(string strWorkstation, string strUser, string docNo);

        [OperationContract]
        DataTable GetCashItemsByDocNo(string strWorkstation, string strUser, string docNo);

        [OperationContract]
        DataTable GetPostBdWithC2PR2PDataMerged(string workstation, string username,
            string mapName = "", string actName = "", string practice = "", string division = "",
            string depDtStart = "", string depDtEnd = "", string embAcct = "", string reason = "", string checkNum = "",
            string checkAmountMin = "", string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "",
            string docType = "", string docDet = "", string who = "", string docNo = "", string parDocNo = "",
            bool includeMatched = false, int startRow = 1, int numRows = Constants.MAX_RESULTS_LIMIT);

        [OperationContract]
        DataTable SearchPostBd(string workstation, string username,
            string mapName = "", string actName = "", string practice = "", string division = "",
            string depDtStart = "", string depDtEnd = "", string embAcct = "", string reason = "", string checkNum = "",
            string checkAmountMin = "", string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "",
            string docType = "", string docDet = "", string who = "", string docNo = "", string parDocNo = "", 
            int startRow = 1, int numRows = Constants.MAX_RESULTS_LIMIT);

        [OperationContract]
        int SearchPostBdCount(string workstation, string username, string mapName = "", string actName = "",
            string practice = "", string division = "", string depDtStart = "", string depDtEnd = "",
            string embAcct = "", string reason = "", string checkNum = "", string checkAmountMin = "",
            string checkAmountMax = "", string paidAmountMin = "", string paidAmountMax = "", string docType = "",
            string docDet = "", string who = "", string docNo = "", string parDocNo = "");

        [OperationContract]
        DataTable GetNavC2PByParDocNo(string workstation, string username, string parDocNo);

        [OperationContract]
        string CreateDocNo(string workstation, string username);

        [OperationContract]
        DataTable GetNavC2PByParDocNoJoinPostDoc(string workstation, string username, string parDocNo);

        [OperationContract]
        DataTable GetPostDocByParDocNo(string workstation, string username, string parDocNo);

        [OperationContract]
        DataTable GetPostDocByDocNo(string workstation, string username, string docNo);

        [OperationContract]
        DataTable ListPractices(string workstation, string username);

        [OperationContract]
        DataTable ListStartRuleLookups(string workstation, string username);

        [OperationContract]
        DataTable ListRoutingInfo(string workstation, string username);

        [OperationContract]
        DataTable ListDocTypeLookups(string workstation, string username);

        [OperationContract]
        DataTable ListPostDetailStatusLookups(string workstation, string username);

        [OperationContract]
        DataTable GetImportedPostDocByTowId(string workstation, string username, int towId);

        [OperationContract]
        DataTable GetImportedPostDocByDocNo(string workstation, string username, string docNo);

        [OperationContract]
        DataTable GetImportedPostDocByParDocNo(string workstation, string username, string parDocNo);

        [OperationContract]
        DataTable GetImportedPostDocByPracticeAndAccount(string workstation, string username
            ,string practice, string account);

        [OperationContract]
        int GetTowerSequenceNextValue(string workstation, string username);

        [OperationContract]
        DataTable GetPostDetailByTowerId(string workstation, string username, int towId);

        [OperationContract]
        DataTable GetPostDetailByPracticeAndAccount(string workstation, string username, 
            string practice, string account);

        [OperationContract]
        DataTable GetPostDetailByPostDetailId(string workstation, string username, int postDetailId);

        [OperationContract]
        DataTable GetPostBDByPracticeAndAccount(string workstation, string username,
            string practice, string account);

        [OperationContract]
        DataTable GetPostBDByUsername(string workstation, string username, string searchUsername);

        [OperationContract]
        DataTable GetPostBDByPracticeAndDepDate(string workstation, string username,
            string practice, string depDate);

        [OperationContract]
        DataTable GetPostBDByCheckNum(string workstation, string username, string checkNum);
        
        [OperationContract]
        DataTable GetPostBDByPracticeAndCheckAmount(string workstation, string username,
            string practice, string checkAmount);
        
        [OperationContract]
        DataTable GetPostBDByDocNo(string workstation, string username, string docNo);
        
        [OperationContract]
        DataTable GetPostBDByParDocNo(string workstation, string username, string parDocNo);

        [OperationContract]
        DataTable GetPostBDOrderedByMapAndActivity(string workstation, string username,
            string map, string activity);

        [OperationContract]
        DataTable GetPostBDOrderedByMapAndActivityDepDt(string workstation, string username,
            string map, string activity);

        [OperationContract]
        DataTable GetBillDocByDocNo(string workstation, string username, string docNo);

        [OperationContract]
        DataTable GetRtiBDByAccountName(string workstation, string username, string accountName);

        [OperationContract]
        DataTable GetStartActivityJoinedPostDetail(string workstation, string username);

        [OperationContract]
        DataTable GetPostDocStatusLookup(string workstation, string username);

        [OperationContract]
        DataTable GetSourceTypeLookupByTypeCode(string workstation, string username);

        [OperationContract]
        bool InsertIntoPostDoc(string workstation, string username, POSTDOC postDoc);

        [OperationContract]
        uint InsertIntoPostDetail(string workstation, string username, NavPostDetail postDetail);

        [OperationContract]
        DataTable GetPostBDByTowId(string workstation, string username, int towId);

        [OperationContract]
        DataTable GetPostBDByPostDetailId(string workstation, string username, int postDetailId);

        [OperationContract]
        bool UpdatePostDocNPagesByTowId(string workstation, string username, int nPages, int towId);

        [OperationContract]
        DataTable GetDocTypes(string workstation, string username);

        [OperationContract]
        DataTable GetDocNoFromPostDocsByParDocNo(string workstation, string username, string parDocNo);

        [OperationContract]
        DataTable GetRtiBDByMapAndActivity(string workstation, string username,
            string map, string activity);

        [OperationContract]
        DataTable GetWorkItemsByIFN(string workstation, string username, string ifn);

        [OperationContract]
        DataTable GetPostDocSearchByParDocNo(string workstation, string username, string parDocNo);        

        [OperationContract]
        DataTable GetPostDocSearchByPracticeAndAccount(string workstation, string username,
            string practice, string account);
        
        [OperationContract]
        DataTable GetPostDocSearchByDocNo(string workstation, string username, string docNo);
        
        [OperationContract]
        DataTable GetPostDocSearchByCheckNum(string workstation, string username, string checkNum);
        
        [OperationContract]
        DataTable GetPostDocSearchBy(string workstation, string username, DateTime depDt);
        
        [OperationContract]
        DataTable GetPostDocSearchByPracticeAndDeptDt(string workstation, string username,
            string practice, DateTime depDt);

        [OperationContract]
        DataTable GetPostDocByDateAndUser(string workstation, string username, DateTime date, string scanuid);

        [OperationContract]
        DataTable GetLastTenCouriers(string workstation, string username, string loginName);

        [OperationContract]
        DataTable GetMapAndActivityByLoginName(string workstation, string username, string loginName);

        [OperationContract]
        DataTable InsertDepositLog(string iPractice, string iDepositDate, string iDocDet, string iDocType,
            string iParDocNo, string iCheckDate, double iCheckAmount, string iExtPayor, string iCheckNumber, 
            int iDepSequence);

        [OperationContract]
        DataTable InsertDeplog(string iPractice, string iDepositDate, string iDocDet, string iDocType,
            string iParDocNo, string iCheckDate, double iCheckAmount, string iExtPayor, string iCheckNumber,
            int iDepSequence, string iServiceId);

        [OperationContract]
        RpcOutMessage CreateWorkflowItem(string workstation, string username, NavWorkFlowItem workFlowItem);

        [OperationContract]
        RpcOutMessage CreateWorkflowItems(string workstation, string username, List<NavWorkFlowItem> workFlowItems);

        [OperationContract]
        bool PracServiceMode(string iServerName, string inPractice, out string rtnMessage, out string rtnSuccess,
            out bool isERBilling, out bool isERCoding, out bool isFacBilling, out bool isFacCoding);

        [OperationContract]
        IDGOutMessage SubmitDocuments(string workstation, string username, string userpwd, string strAction, List<NavImage> navImages, IDGInputParams idgparams);

        #region Imaging

        [OperationContract]
        bool ConvertPDFtoTif(string workstation, string username, string pdfFileName, out string message);

        /*
        Out params:
        <out name="outSet" type="table" seq="1">
        <field name="filename" type="character" seq="1"/>
        <field name="outIFNDs" type="character" seq="2"/>
        <field name="outIFNId" type="character" seq="3"/>
        <field name="outTowId" type="character" seq="4"/>
        </out>

        <out name="outParams" type="character" seq="2"/>
        <out name="outResult" type="logical" seq="3"/>
        */
        [OperationContract]
        RpcOutMessage ImportImages(string workstaiton, string username, string userpwd, List<NavImage> navImages);

        [OperationContract]
        RpcOutMessage ImportImage(string workstaiton, string username, NavImage navImage);

        [OperationContract]
        ProcNoteResult ProcessNote(string workstaiton, string strUserId, int intNoteKey, string strNote, string strType, string strAction);

        #endregion
    }
}

