//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rti.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHANNEL
    {
        public CHANNEL()
        {
            this.CHANNEL_STATISTICS = new HashSet<CHANNEL_STATISTICS>();
            this.MESSAGEs = new HashSet<MESSAGE>();
        }
    
        public string ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string IS_ENABLED { get; set; }
        public string VERSION { get; set; }
        public Nullable<decimal> REVISION { get; set; }
        public Nullable<System.DateTime> LAST_MODIFIED { get; set; }
        public string SOURCE_CONNECTOR { get; set; }
        public string DESTINATION_CONNECTORS { get; set; }
        public string PROPERTIES { get; set; }
        public string PREPROCESSING_SCRIPT { get; set; }
        public string POSTPROCESSING_SCRIPT { get; set; }
        public string DEPLOY_SCRIPT { get; set; }
        public string SHUTDOWN_SCRIPT { get; set; }
    
        public virtual ICollection<CHANNEL_STATISTICS> CHANNEL_STATISTICS { get; set; }
        public virtual ICollection<MESSAGE> MESSAGEs { get; set; }
    }
}
