namespace WpfDocViewer.Model
{
    /// <summary>
    /// Mark Lane
    /// used to identify the Remit data in a matching cash model.
    /// used in ObservableCollection.
    /// </summary>
    public class RemitData
    {
        public RemitData(string RemitDataName)
        {
            //this.RemitDataName = RemitDataName;
        }
        public int MatchID { get; set; }
        public string Practice { get; set; } //client
        public string Division { get; set; }
        public string ExtPayCD { get; set; }
        public string Checkamt{get;set;}
        public string Prchkdt { get; set; }//expected date
        public string Checknum { get; set; }
        public string Note { get; set; }
        public string Provid { get; set; }
        public string Doctype { get; set; }
        public string Docdet { get; set; }
        public string Transtype { get; set; }
        public string RStatus { get; set; }
        public string Matchstat { get; set; }
        public string Wfstatus { get; set; }
        public string OrigCD { get; set; } //Original Bank
        public string Origno { get; set; }//Original Bank Account
        public string Docno { get; set; }
        public string Filedt { get; set; }//file date
        public string RemitFileId { get; set; }
        public string MatchUID { get; set; }
        public string Svctype { get; set; }
        
        
        
      /*  remit:

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
       * */

    }
}