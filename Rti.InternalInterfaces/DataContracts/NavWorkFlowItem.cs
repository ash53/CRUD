using System;
using System.Runtime.Serialization;


namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class NavWorkFlowItem
    {
        public NavWorkFlowItem()
        {
            
        }

        [DataMember (Order = 1, IsRequired = true)]
        public string practice { get; set; }
        [DataMember (Order = 2, IsRequired = true)]
        public string division { get; set; }
        [DataMember (Order = 3, IsRequired = true)]
        public DateTime? depDate { get; set; }
        [DataMember (Order = 4, IsRequired = true)]
        public DateTime? recDate { get; set; }
        [DataMember (Order = 5, IsRequired = true)]
        public string docType { get; set; }
        [DataMember (Order = 6, IsRequired = true)]
        public string docNo { get; set; }
        [DataMember (Order = 7, IsRequired = true)]
        public string parDocNo { get; set; }
        [DataMember (Order = 8, IsRequired = true)]
        public string embAcctNo { get; set; }
        [DataMember (Order = 9, IsRequired = true)]
        public string checkNum { get; set; }
        [DataMember (Order = 10, IsRequired = true)]
        public string checkAmt { get; set; }
        [DataMember (Order = 11, IsRequired = true)]
        public string paidAmt { get; set; }
        [DataMember (Order = 12, IsRequired = true)]
        public string extPayorCd { get; set; }
        [DataMember (Order = 13, IsRequired = true)]
        public string depCode { get; set; }
        [DataMember (Order = 14, IsRequired = true)]
        public string docGroup { get; set; }
        [DataMember (Order = 15, IsRequired = true)]
        public string docDetail { get; set; }
        [DataMember (Order = 16, IsRequired = true)]
        public decimal? courier_Inst_Id { get; set; }
        [DataMember (Order = 17, IsRequired = true)]
        public string acctNum { get; set; }
        [DataMember (Order = 18, IsRequired = true)]
        public string act_Name { get; set; }
        [DataMember (Order = 19, IsRequired = true)]
        public string map_Name { get; set; }
        [DataMember (Order = 20, IsRequired = true)]
        public int? act_Node_Id { get; set; }
        [DataMember (Order = 21, IsRequired = true)]
        public int? map_Inst_Id { get; set; }
        [DataMember (Order = 22, IsRequired = true)]
        public DateTime? svcDate { get; set; }
        [DataMember (Order = 23, IsRequired = true)]
        public int? docPage { get; set; }
        [DataMember (Order = 24, IsRequired = true)]
        public decimal? postdetail_Id { get; set; }
        [DataMember (Order = 25, IsRequired = true)]
        public decimal? regionLeft { get; set; }
        [DataMember (Order = 26, IsRequired = true)]
        public decimal? regionTop { get; set; }
        [DataMember (Order = 27, IsRequired = true)]
        public string finClass { get; set; }
        [DataMember (Order = 28, IsRequired = true)]
        public string escReason { get; set; }
        [DataMember (Order = 29, IsRequired = true)]
        public string corrFolder { get; set; }
        [DataMember (Order = 30, IsRequired = true)]
        public string serviceId { get; set; }
        [DataMember (Order = 31, IsRequired = true)]
        public string reason { get; set; }
        [DataMember (Order = 32, IsRequired = true)]
        public string informational { get; set; }
    }
}
