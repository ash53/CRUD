using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rti.ExternalInterfaces.DataContracts
{
    [DataContract]
    public class GetRemitItemsResults1
    {
        [DataMember]
        public string OutMsg { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public DataTable RemitItems { get; set; }
    }
}
