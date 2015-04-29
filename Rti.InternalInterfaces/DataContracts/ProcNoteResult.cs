using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class ProcNoteResult
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string NoteTxt { get; set; }
        [DataMember]
        public string CreateUId { get; set; }
        [DataMember]
        public string CreateDt { get; set; }
        [DataMember]
        public string ModUId { get; set; }
        [DataMember]
        public string ModDt { get; set; }
        [DataMember]
        public string OutMsg { get; set; }

    }
}
