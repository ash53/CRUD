using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class UpdR2PInputParams
    {
        [DataMember]
        public string ServerName { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string InKey { get; set; }

        [DataMember]
        public string InDocDet { get; set; }

        [DataMember]
        public DateTime InPrCheckDt { get; set; }

        [DataMember]
        public string InCheckNum { get; set; }

        [DataMember]
        public string InExtPayCd { get; set; }

        [DataMember]
        public string InPractice { get; set; }

        [DataMember]
        public string InDivision { get; set; }

        [DataMember]
        public string InProvId { get; set; }

    }
}
