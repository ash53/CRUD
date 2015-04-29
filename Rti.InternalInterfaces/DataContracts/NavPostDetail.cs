using System;
using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class NavPostDetail
    {
        public NavPostDetail() 
        {
        }

        [DataMember (Order = 0, IsRequired = true)] 
        public Nullable<decimal> TOWID { get; set; }
        [DataMember (Order = 1, IsRequired = true)] 
        public decimal POSTDETAIL_ID { get; set; }
        [DataMember (Order = 2, IsRequired = true)]
        public Nullable<System.DateTime> SVCDT { get; set; }
        [DataMember (Order = 3, IsRequired = true)]
        public Nullable<decimal> PAIDAMT { get; set; }
        [DataMember (Order = 4, IsRequired = true)]
        public string EMBACCT { get; set; }
        [DataMember (Order = 5, IsRequired = true)]
        public string ROUTEINFO { get; set; }
        [DataMember (Order = 6, IsRequired = true)]
        public string STATUS { get; set; }
        [DataMember (Order = 7, IsRequired = true)]
        public Nullable<decimal> DOCPAGE { get; set; }
        [DataMember (Order = 8, IsRequired = true)]
        public string MODIFYUID { get; set; }
        [DataMember (Order = 9, IsRequired = true)]
        public Nullable<System.DateTime> MODIFYDATE { get; set; }
        [DataMember (Order = 10, IsRequired = true)]
        public string CREATEUID { get; set; }
        [DataMember (Order = 11, IsRequired = true)]
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        [DataMember (Order = 12, IsRequired = true)]
        public Nullable<decimal> LEFT { get; set; }
        [DataMember (Order = 13, IsRequired = true)]
        public Nullable<decimal> TOP { get; set; }
        [DataMember (Order = 14, IsRequired = true)]
        public Nullable<decimal> HEIGHT { get; set; }
        [DataMember (Order = 15, IsRequired = true)]
        public Nullable<decimal> WIDTH { get; set; }
        [DataMember (Order = 16, IsRequired = true)]
        public Nullable<decimal> XSCALE { get; set; }
        [DataMember (Order = 17, IsRequired = true)]
        public Nullable<decimal> YSCALE { get; set; }
        [DataMember (Order = 18, IsRequired = true)]
        public Nullable<decimal> ORIENTATION { get; set; }
        [DataMember (Order = 19, IsRequired = true)]
        public Nullable<decimal> HSCROLL { get; set; }
        [DataMember (Order = 20, IsRequired = true)]
        public Nullable<decimal> VSCROLL { get; set; }
        [DataMember (Order = 21, IsRequired = true)]
        public string IFN { get; set; }
        [DataMember (Order = 22, IsRequired = true)]
        public string NEXT_ACT_NAME { get; set; }
        [DataMember (Order = 23, IsRequired = true)]
        public string NEXT_MAP_NAME { get; set; }
        [DataMember (Order = 24, IsRequired = true)]
        public string REASON { get; set; }
        [DataMember (Order = 25, IsRequired = true)]
        public string INFORMATIONAL { get; set; }
        [DataMember (Order = 26, IsRequired = true)]
        public string PRACTICE { get; set; }
        [DataMember (Order = 27, IsRequired = true)]
        public Nullable<int> IFNDS { get; set; }
        [DataMember (Order = 28, IsRequired = true)]
        public Nullable<int> IFNID { get; set; }
        [DataMember (Order = 29, IsRequired = true)]
        public Nullable<decimal> ORIG_TOWID { get; set; }
        [DataMember (Order = 30, IsRequired = true)]
        public Nullable<decimal> ALT_TOWID { get; set; }
        [DataMember (Order = 31, IsRequired = true)]
        public string COMINGLED_STATUS { get; set; }
        [DataMember (Order = 32, IsRequired = true)]
        public string SERVICEID { get; set; }
       
    }
}
