using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class IDGInputParams
    {
        [DataMember]
        public bool IsRemitOnly { get; set; }

        [DataMember]
        public bool IsChkNeedsPrep { get; set; }

        [DataMember]
        public string strMapName { get; set; }

        [DataMember]
        public string strActName { get; set; }

        [DataMember]
        public string strInformational { get; set; }
 
        [DataMember]
        public string strReason { get; set; }

    }
}
