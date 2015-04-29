using System;
using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class SearchAccountsAttributes
    {
        [DataMember] 
        public int NumRecsReturned { get; set; }
        [DataMember]
        public int BasisPtr { get; set; }
        [DataMember]
        public int DataSetPtr { get; set; }
        [DataMember]
        public string Ssn { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string PolicyNo { get; set; }
        [DataMember]
        public string AcctNo { get; set; }
        [DataMember]
        public string PhoneNo { get; set; }
        [DataMember]
        public string GroupNo { get; set; }
        [DataMember]
        public bool LastNameBegins { get; set; }
        [DataMember]
        public bool FirstNameBegins { get; set; }
        [DataMember]
        public string Practice { get; set; }
        [DataMember]
        public string EnctrNo { get; set; }
        [DataMember]
        public DateTime SvcDt { get; set; }
        [DataMember]
        public string OriginAddr1 { get; set; }
        [DataMember]
        public string OriginZip { get; set; }
    }
}
