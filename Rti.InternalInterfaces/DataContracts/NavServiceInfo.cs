using System;
using System.Runtime.Serialization;


namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class NavServiceInfo
    {
        [DataMember] 
        public string ServiceName { get; set; }
        [DataMember] 
        public int ThreadCount { get; set; }
        [DataMember] 
        public DateTime ServiceStartTime { get; set; }
        [DataMember] 
        public TimeSpan TotalProcessorTime { get; set; }
        [DataMember] 
        public bool Responding { get; set; }
        [DataMember] 
        public string MachineName { get; set; }
        [DataMember] 
        public DateTime ServiceBuildTime { get; set; }
    }
}
