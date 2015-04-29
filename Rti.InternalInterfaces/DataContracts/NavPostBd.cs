using System.Runtime.Serialization;


namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class NavPostBd
    {
        public NavPostBd()
        {
        }

        [DataMember] 
            public string MAP_NAME { set; get; }
        [DataMember] 
            public string ACT_NAME { set; get; }
        [DataMember] 
            public string WHO { set; get; }
        [DataMember] 
            public string DOCNO { set; get; }
        [DataMember] 
            public string PRACTICE { set; get; }
        [DataMember] 
            public string DIVISION { set; get; }
        [DataMember] 
            public string EMBACCT { set; get; }
        [DataMember] 
            public string REASON { set; get; }
        [DataMember] 
            public string DOCTYPE { set; get; }
        [DataMember] 
            public string DOCDET { set; get; }
        [DataMember] 
            public string DEPTDT { set; get; }
        [DataMember] 
            public string CHECKNUM { set; get; }
        [DataMember] 
            public string CHECKAMT { set; get; }
        [DataMember] 
            public string PAIDAMT { set; get; }
        [DataMember] 
            public string PARDOCNO { set; get; }
        [DataMember] 
            public string RECDATE { set; get; }
        [DataMember] 
            public string EXTPAYOR { set; get; }
        [DataMember] 
            public string DEPCODE { set; get; }
        [DataMember] 
            public int? POSTDETAIL_ID { set; get; }
        [DataMember] 
            public string SERVICEID { set; get; }
        [DataMember] 
            public string CORRECTION_FOLDER { set; get; }
        [DataMember] 
            public string ESCALATION_REASON { set; get; }
        [DataMember] 
            public string USERNAME { set; get; }
        [DataMember] 
            public int? TOWID { set; get; }
        [DataMember] 
            public string PAIDAMT2 { set; get; }
        [DataMember] 
            public string IFN { set; get; }
        [DataMember] 
            public int? NPAGES { set; get; }
        [DataMember] 
            public string DOCGROUP { set; get; }
        [DataMember] 
            public string STATUS { set; get; }
        [DataMember] 
            public int? COURIER_INST_ID { set; get; }
    }
}
