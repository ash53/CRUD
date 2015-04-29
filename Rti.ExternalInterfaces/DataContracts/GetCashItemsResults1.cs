using System.Data;
using System.Runtime.Serialization;

namespace Rti.ExternalInterfaces.DataContracts
{
    [DataContract]
    public class GetCashItemsResults1
    {
        [DataMember]
        public string OutMsg { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public DataTable CashItems { get; set; }
    }
}
