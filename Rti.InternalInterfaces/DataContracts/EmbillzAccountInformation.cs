using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class EmbillzAccountInformation
    {
        [DataMember]
        public string GroupNumber { get; set; }
        
        [DataMember]
        public string PatientName { get; set; }
        
        [DataMember]
        public string ServiceDate { get; set; }
        
        [DataMember]
        public string PolicyNumber { get; set; }
        
        [DataMember]
        public string Practice { get; set; }
        
        [DataMember]
        public string SocialSecurityNumber { get; set; }
        
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string PayorNumber { get; set; }
    }
}
