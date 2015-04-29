using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfDocViewer.Model
{
	public class MatchingChecks
	{
        /// <summary>
        /// Author: Mark Lane
        /// used for Matching Cash will be in static ObservableCollection.
        /// 11/26/2014
        /// </summary>
        /// <param name="checkNumber"></param>
        /// <param name="checkDocno"></param>
        /// <param name="checkAmount"></param>
        /// Cash:
/*
1. Match Id - matchid
2. Client - client
3. External Payor - extpaycd
4. Check Amt - checkamt
5. Check Date - checkdt
6. Check Number - checknum
7. Note - noteflag
8. Deposit Date - depdt
9. Susp# - suspendno
10.Docno - docno
11.Doc Detail - docdet
12.Status - cstatus
13.Match Status - matchstat
14.WorkFlow Status - wfstatus
15.Doc Type - doctype
16.Expct EPR? - expectepr
17.Orig Bank  - origcd
18.Orig Bank Acct - origno
19.File Dt - filedate
20.Match User - matchuid
21.SvcType - svctype
 
remit:

1. Match Id - matchid
2. Client - client
3. External Payor - extpaycd
4. Check Amt - checkamt
5. Expected Date - prchkdt
6. Check Number - checknum
7. Note - noteflag
8. ProvID/Grp - provid
9. Doc Type - doctype
10.Doc Detail - docdet
11.Pmt Method - transtype
12.Status - rstatus
13.Match Status - matchstat
14.WorkFlow Status - wfstatus
15.Orig Bank - origcd
16.Orig Bank Acct - origno
17.Docno - docno
18.File Date - filedt
19.Remit File ID - remitfileid
20.Match User - matchuid
21.SvcType - svctype 


Matching Grid:

1. Match Id - matchid
2. Client - client
3. External Payor - extpaycd
4. Amount - checkamt
5. Dep/File Date - depdt/prchkdt
6. Check Date -checkdt
7. Check Number - checknum
8. Susp No -
9. Prov ID - provid
10.Docno - docno
11.Doc Type - doctype
12.Doc Detail - docdet
13.Status - cstatus/rstatus
14.Match Status - matchstat
15.WorkFlow Status - wfstatus
16.Orig Bank - origcd
17.Orig Bank Acct - origno
18.File Dt - filedt
19.Pmt Method - transtype
20.Expct EPR? - expectepr
21.Remit File ID - remitfileid
22.Note - noteflag
23.MatchUid - Matchuid
24.SvcType - serviceid
*/
        /// <param name="practice"></param>
        /// <param name="division"></param>
        /// <param name="depDate"></param>
        /// <param name="docDetail"></param>
        /// <param name="docType"></param>
        /// <param name="description"></param>
        public MatchingChecks(string checkNumber,string checkDocno, double checkAmount, string practice, 
                            string depDate, string docDetail, string docType, string description,
                            int matchID, string externalPayor, string note, string checkDepDate, string suspendNumber,
                            string status, string matchStatus, string workflowStatus, string expectEPR, string origBank,
                            string origBankAccount, string filedate, string matchUser, string serviceType, string rstatus,
                            int key, string srcfileflag, string transtype, string parDocno, string comingledStatus)
        {
             MatchId = matchID;
             CheckNumber = checkNumber;
             CheckAmount = checkAmount;
             Docno = checkDocno;
             Practice = practice;
             CheckDepDate = depDate;
             Docdetail = docDetail;
             Doctype = docType;
             Description = description;
             ExternalPayor = externalPayor;
             CheckDate = CheckDate;
             Note = note;
             SuspendNumber = suspendNumber;
             CStatus = status;
             MatchStatus = matchStatus;
             WorkFlowStatus = workflowStatus;
             ExpectEPR = expectEPR;
             OrigBank = origBank;
             OrigBankAccount = origBankAccount;
             FileDate = filedate;
             MatchUser = matchUser;
             ServiceType = serviceType;
             RStatus = rstatus;
             Key = key;
             SrcFileFlag = srcfileflag;
             TransType = transtype;
             ParDocno = parDocno;
             ComingledStatus = comingledStatus;
        }
         /*
        public MatchingChecks(string checkNumber, string checkDocno, double checkAmount, string practice,
                            string depDate, string docDetail, string docType, string description)
        {
          
            CheckNumber = checkNumber;
            CheckAmount = checkAmount;
            CheckDocno = checkDocno;
            Practice = practice;
            CheckDepDate = depDate;
            CheckDocdetail = docDetail;
            CheckDoctype = docType;
            Description = description;
           

        }
         * * */
        public int MatchId {get;set;}
        public string Practice { get; set; }
        public string ExternalPayor { get; set;}
        public double CheckAmount { get; set; }
        public string CheckNumber { get; set; }
        public string CheckDate { get; set; }
        public string Note { get; set; }
        public string CheckDepDate { get; set; }
        public string SuspendNumber { get; set; }
        public string Docno { get; set; }
        public string Docdetail { get; set; }
        public string Doctype { get; set; }
        public string CStatus { get; set; }
        public string RStatus { get; set; }
        public string MatchStatus { get; set; }
        public string WorkFlowStatus { get; set; }
        public string ExpectEPR { get; set; }
        public string OrigBank { get; set; }
        public string OrigBankAccount { get; set; }
        public string FileDate { get; set; }
        public string MatchUser { get; set; }
        public string ServiceType { get; set; }
		public double PaidAmount { get; set; }
		public string Description { get; set; }
        public int Key { get; set; }
        public string SrcFileFlag { get; set; }
        public string TransType { get; set; }
        public string ParDocno { get; set; }
        public string ComingledStatus { get; set; }
       
/*
  1. match id
  2. Client - client
3. External Payor - extpaycd
4. Check Amt - checkamt
5. check date
6. Check Number - checknum
7. Note - noteflag
8. Deposit Date - depdt
9. Susp# - suspendno
10.Docno - docno
11.Doc Detail - docdet
12.Status - cstatus
13.Match Status - matchstat
14.WorkFlow Status - wfstatus
15.Doc Type - doctype
16.Expct EPR? - expectepr
17.Orig Bank  - origcd
18.Orig Bank Acct - origno
19.File Dt - filedate
20.Match User - matchuid
21.SvcType - svctype
 * */
	}
}
