using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class RpcOutMessage
    {
        [DataMember] 
        public bool IsSuccess { get; set; }

        [DataMember]
        public string OutMessage { get; set; }

        [DataMember]
        public DataTable OutDataTable { get; set; }

        [DataMember]
        public Dictionary<string, string> OutDictionary { get; set; }
    }
}
