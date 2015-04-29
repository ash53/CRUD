using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class RpcInMessage
    {
        [DataMember] 
        public string Workstation { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Context { get; set; }
    }
}
