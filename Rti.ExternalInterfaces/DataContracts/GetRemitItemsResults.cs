using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rti.ExternalInterfaces.DataContracts
{
    [DataContract]
    public class GetRemitItemsResults
    {
        [DataMember]
        public string OutMsg { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public List<RemitInfo> RemitItems { get; set; }
    }
}
