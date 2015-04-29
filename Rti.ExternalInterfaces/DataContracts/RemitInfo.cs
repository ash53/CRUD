using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rti.ExternalInterfaces.DataContracts
{
    [DataContract]
    public class RemitInfo
    {
        [DataMember]
        public int Key { get; set; }

        [DataMember]
        public int Matchid { get; set; }

        [DataMember]
        public string Client { get; set; }

        [DataMember]
        public string ExtPaycd { get; set; }

        [DataMember]
        public decimal CheckAmt { get; set; }

        [DataMember]
        public DateTime PrchkDt { get; set; }

        [DataMember]
        public string CheckNum { get; set; }

        [DataMember]
        public string Provid { get; set; }

        [DataMember]
        public string DocType { get; set; }

        [DataMember]
        public string DocDet { get; set; }

        [DataMember]
        public string TransType { get; set; }

        [DataMember]
        public string RStatus { get; set; }

        [DataMember]
        public string MatchStat { get; set; }

        [DataMember]
        public string WfStatus { get; set; }

        [DataMember]
        public string OrigCd { get; set; }

        [DataMember]
        public string OrigNo { get; set; }

        [DataMember]
        public string DocNo { get; set; }
        
        [DataMember]
        public DateTime FileDate { get; set; }

        [DataMember]
        public string NoteFlag { get; set; }

        [DataMember]
        public string MatchUid { get; set; }

        [DataMember]
        public string SvcType { get; set; }
    }
}
