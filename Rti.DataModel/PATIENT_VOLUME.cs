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
    
    public partial class PATIENT_VOLUME
    {
        public decimal PATIENT_VOLUME_ID { get; set; }
        public Nullable<System.DateTime> AS_OF_DATE { get; set; }
        public string PRACTICE { get; set; }
        public string DIVISION { get; set; }
        public Nullable<System.DateTime> SND_DATE { get; set; }
        public Nullable<System.DateTime> SVC_START_DATE { get; set; }
        public Nullable<System.DateTime> SVC_END_DATE { get; set; }
        public Nullable<System.DateTime> REC_DATE { get; set; }
        public Nullable<decimal> TOTAL_ED_LOG { get; set; }
        public Nullable<decimal> NON_BILL_ORIG { get; set; }
        public Nullable<decimal> NON_BILL_CUR { get; set; }
        public Nullable<decimal> MISSING_ORIG { get; set; }
        public Nullable<decimal> MISSING_CUR { get; set; }
        public Nullable<decimal> SUSPEND { get; set; }
        public Nullable<decimal> SEQ { get; set; }
        public string CREATE_USERID { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string UPDATE_USERID { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public Nullable<decimal> RESCAN_COUNT { get; set; }
    }
}
