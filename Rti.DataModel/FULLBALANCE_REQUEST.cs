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
    
    public partial class FULLBALANCE_REQUEST
    {
        public decimal FULLBALANCE_REQUEST_ID { get; set; }
        public string PRACTICE { get; set; }
        public Nullable<System.DateTime> DEPOSITDATE { get; set; }
        public Nullable<decimal> PAIDAMT { get; set; }
        public string ACCTNUM { get; set; }
        public Nullable<decimal> COURIERID { get; set; }
        public string EMBILLZ_DFPAYOR { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> SCHEDULEDTTM { get; set; }
        public Nullable<decimal> ATTEMPTCOUNT { get; set; }
        public Nullable<System.DateTime> COMPLETEDTTM { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATE_USERID { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string UPDATE_USERID { get; set; }
        public string RPCMSG { get; set; }
        public string UNIVERSALFINCLASS { get; set; }
        public string PAYOR { get; set; }
        public Nullable<decimal> ACCTORGBAL { get; set; }
        public string BATCHID { get; set; }
    }
}
