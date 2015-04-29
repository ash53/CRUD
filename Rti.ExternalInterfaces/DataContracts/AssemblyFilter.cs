using System.Runtime.Serialization;

namespace Rti.ExternalInterfaces.DataContracts
{
    [DataContract]
    public class AssemblyFilter
    {
        [DataMember]
        public string Practice { get; set; }

        [DataMember]
        public string ExtPayor { get; set; }

        [DataMember]
        public string DocDetail { get; set; }

        [DataMember]
        public string PmtMethod { get; set; }

        [DataMember]
        public string CheckAmountMin { get; set; }

        [DataMember]
        public string CheckAmountMax { get; set; }

        [DataMember]
        public string DateMin { get; set; }

        [DataMember]
        public string DateMax { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string PostingSuspends { get; set; }

        [DataMember]
        public string MatchStatus { get; set; }

        [DataMember]
        public string ServiceType { get; set; }
    }
}
