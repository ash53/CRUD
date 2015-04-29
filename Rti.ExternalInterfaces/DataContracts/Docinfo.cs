using System.Runtime.Serialization;

namespace Rti.ExternalInterfaces.DataContracts
{
    [DataContract]
    public class Docinfo
    {
        [DataMember]
        public string strDocno { get; set; }
    }
}
