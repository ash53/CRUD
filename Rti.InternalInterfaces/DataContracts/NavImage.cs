using System.Runtime.Serialization;


namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class NavImage
    {
        public NavImage()
        {
        }

        [DataMember (Order = 1, IsRequired = true)]
        public string action { get; set; }
        [DataMember (Order = 2, IsRequired = true)]
        public string filename { get; set; }
        [DataMember (Order = 3, IsRequired = true)]
        public int? seq { get; set; }
        [DataMember (Order = 4, IsRequired = true)]
        public string docNo { get; set; }
        [DataMember (Order = 5, IsRequired = true)]
        public string parDocNo { get; set; }
        [DataMember (Order = 6, IsRequired = true)]
        public string origDocNo { get; set; }
        [DataMember (Order = 7, IsRequired = true)]
        public string docType { get; set; }
        [DataMember (Order = 8, IsRequired = true)]
        public string docDet{ get; set; }
        [DataMember (Order = 9, IsRequired = true)]
        public string docSrc { get; set; }
        [DataMember (Order = 10, IsRequired = true)]
        public string acctNo { get; set; }
        [DataMember (Order = 11, IsRequired = true)]
        public string practice { get; set; }
        [DataMember (Order = 12, IsRequired = true)]
        public string div { get; set; }
        [DataMember (Order = 13, IsRequired = true)]
        public string serviceId{ get; set; }
        [DataMember (Order = 14, IsRequired = true)]
        public string deptCode { get; set; }
        [DataMember (Order = 15, IsRequired = true)]
        public string extPayor { get; set; }
        [DataMember (Order = 16, IsRequired = true)]
        public string checkAmt { get; set; }
        [DataMember (Order = 17, IsRequired = true)]
        public string checkNum { get; set; }
        [DataMember (Order = 18, IsRequired = true)]
        public string checkDt{ get; set; }
        [DataMember (Order = 19, IsRequired = true)]
        public int? nPages { get; set; }
        [DataMember (Order = 20, IsRequired = true)]
        public string cStatus { get; set; }
        [DataMember (Order = 21, IsRequired = true)]
        public string supervisorId{ get; set; }
        [DataMember(Order = 22, IsRequired = true)]
        public string DocGroup { get; set; }

    }
}
