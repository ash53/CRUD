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
    
    public partial class FWSYS_CONFIG
    {
        public long DOMAIN_ID { get; set; }
        public long MAX_MIFAM_CNT { get; set; }
        public long MAX_CIFAM_CNT { get; set; }
        public long MAX_PROC_CNT { get; set; }
        public long MAX_DDLN_CNT { get; set; }
        public long MAX_SYSMON_CNT { get; set; }
        public long MAX_STAT_CNT { get; set; }
        public long MAX_SRC_CNT { get; set; }
        public long TRANS_MEM_KEY { get; set; }
        public long TRANS_SEM_KEY { get; set; }
        public long SYS_STATE { get; set; }
        public long CURR_MIFAM_CNT { get; set; }
        public long CURR_CIFAM_CNT { get; set; }
        public long CURR_PROC_CNT { get; set; }
        public long CURR_DDLN_CNT { get; set; }
        public long CURR_SYSMON_CNT { get; set; }
        public long CURR_STAT_CNT { get; set; }
        public long CURR_SRC_CNT { get; set; }
        public string DAP_FORMULA { get; set; }
        public long DAP_PRIOR_INC { get; set; }
        public long DAP_COUR_THRESH { get; set; }
        public string CONFIG_FILE { get; set; }
        public string FW_DBNAME { get; set; }
        public string IFSM_LOG_FILE { get; set; }
        public long DEF_AGENT_PORT { get; set; }
        public string JDBC_URL { get; set; }
        public string JDBC_DRIVER { get; set; }
        public string MAX_NIMBED_STR { get; set; }
        public string FMK_STR { get; set; }
        public string SERIAL_STR { get; set; }
        public string VERSION { get; set; }
        public Nullable<long> CTLINFO_INTVAL { get; set; }
    }
}
