using System.Runtime.Serialization;

namespace Rti.ExternalInterfaces.DataContracts
{
    [DataContract]
    public class GetCashItemsInputParams
    {
        [DataMember]
        public string ServerName { get; set; }

        [DataMember]
        public string MaxRows { get; set; }

        [DataMember]
        public string StartRow { get; set; }

        [DataMember]
        public string DocNo { get; set; }

        [DataMember]
        public AssemblyFilter Filters { get; set; }
    }
}
