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
    
    public partial class CLAUS
    {
        public long CLAUSE_ID { get; set; }
        public long SEQUENCE { get; set; }
        public long CLAUSE_STATUS { get; set; }
        public long MAP_NAME_ID { get; set; }
        public long ACT_NAME_ID { get; set; }
        public string ACCESS_NAME { get; set; }
        public string RETURN_STATE { get; set; }
        public string CONDITION { get; set; }
        public string DESCRIPTION { get; set; }
        public long MEMBER_TYPE { get; set; }
        public long LOCK_FLAG { get; set; }
        public long ACCESS_TYPE { get; set; }
        public long ACCESS_OPERATION { get; set; }
        public long ACCESS_NAME_STATUS { get; set; }
        public long RETURN_STATE_USAGE { get; set; }
        public Nullable<decimal> ACTION_TYPE { get; set; }
        public string ACTION_CMD { get; set; }
        public string ACTION_DATA { get; set; }
    }
}
