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
    
    public partial class IMPORT_DFT_HDR
    {
        public IMPORT_DFT_HDR()
        {
            this.IMPORT_ACC_DTL = new HashSet<IMPORT_ACC_DTL>();
            this.IMPORT_DFT_DTL = new HashSet<IMPORT_DFT_DTL>();
        }
    
        public decimal IMPORT_DFT_HDR_ID { get; set; }
        public string PRACTICE { get; set; }
        public string DIVISION { get; set; }
        public string ACSTATUS { get; set; }
        public string EMBACCTSTATUS { get; set; }
        public string MSG_CTL_ID { get; set; }
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string REF_PROV_NO { get; set; }
        public Nullable<System.DateTime> ADMITDT { get; set; }
        public Nullable<System.DateTime> DISCHARGEDT { get; set; }
        public string MEDRECNO { get; set; }
        public string EMB_ACCT_NO { get; set; }
        public string DOC_NO { get; set; }
        public Nullable<System.DateTime> BATCHDT { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATE_USERID { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string UPDATE_USERID { get; set; }
        public string FILENAME { get; set; }
        public string HOSP_ACCT_NO { get; set; }
        public string PROC_ERRMSG { get; set; }
        public string REF_PROV_NAME { get; set; }
        public string ACTYPE { get; set; }
        public string CM_ACCT_NO { get; set; }
    
        public virtual ICollection<IMPORT_ACC_DTL> IMPORT_ACC_DTL { get; set; }
        public virtual ICollection<IMPORT_DFT_DTL> IMPORT_DFT_DTL { get; set; }
    }
}