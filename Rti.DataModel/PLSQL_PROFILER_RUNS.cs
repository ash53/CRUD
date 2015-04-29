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
    
    public partial class PLSQL_PROFILER_RUNS
    {
        public PLSQL_PROFILER_RUNS()
        {
            this.PLSQL_PROFILER_UNITS = new HashSet<PLSQL_PROFILER_UNITS>();
            this.SQLN_PROF_SESS = new HashSet<SQLN_PROF_SESS>();
        }
    
        public decimal RUNID { get; set; }
        public Nullable<decimal> RELATED_RUN { get; set; }
        public string RUN_OWNER { get; set; }
        public Nullable<System.DateTime> RUN_DATE { get; set; }
        public string RUN_COMMENT { get; set; }
        public Nullable<decimal> RUN_TOTAL_TIME { get; set; }
        public string RUN_SYSTEM_INFO { get; set; }
        public string RUN_COMMENT1 { get; set; }
        public string SPARE1 { get; set; }
    
        public virtual ICollection<PLSQL_PROFILER_UNITS> PLSQL_PROFILER_UNITS { get; set; }
        public virtual ICollection<SQLN_PROF_SESS> SQLN_PROF_SESS { get; set; }
    }
}