using System;
using System.Runtime.Serialization;


namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class NavR2P
    {
        public NavR2P()
        {
        }

        [DataMember]
        public string DOCNO { set; get; }

        [DataMember]
        public string PARDOCNO { set; get; }

        [DataMember]
        public string EXTPAYCD { set; get; }

        [DataMember]
        public DateTime? FILEDT { set; get; }

        [DataMember]
        public int? MATCHID { set; get; }

        [DataMember]
        public string MATCHSTAT { set; get; }

        [DataMember]
        public string MATCHUID { set; get; }

        [DataMember]
        public string ORIGCD { set; get; }

        [DataMember]
        public string ORIGNO { set; get; }

        [DataMember]
        public DateTime? PRCHKDT { set; get; }

        [DataMember]
        public string PROVID { set; get; }

        [DataMember]
        public string RSTATUS { set; get; }

        [DataMember]
        public int? SUSPENDNO { set; get; }

        [DataMember]
        public string REMITFILEID { set; get; }

        [DataMember]
        public decimal? CHECKAMT { set; get; }

        [DataMember]
        public string CHECKNUM { set; get; }

        [DataMember]
        public string DIV { set; get; }

        [DataMember]
        public string DOCDET { set; get; }

        [DataMember]
        public string DOCTYPE { set; get; }

        [DataMember]
        public int? KEY { set; get; }

        [DataMember]
        public string PRACTICE { set; get; }

        [DataMember]
        public string SERVICEID { set; get; }

        [DataMember]
        public string STAT { set; get; }

        [DataMember]
        public string TRANSTYPE { set; get; }

        [DataMember]
        public string NOTETEXT { set; get; }

        [DataMember]
        public double? SECMODDT { set; get; }
    }
}
