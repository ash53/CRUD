using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class IDGOutMessage
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string OutMessage { get; set; }

        [DataMember]
        public string strTowId { get; set; }

        [DataMember]
        public string strPostdetailId { get; set; }

        [DataMember]
        public string strCourierInstId { get; set; }

        [DataMember]
        public string strWfitemkey { get; set; }

        [DataMember]
        public string strWfthreadkey { get; set; }
    }
}
