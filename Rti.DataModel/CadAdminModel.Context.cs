﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CadAdminModelContainer : DbContext
    {
        public CadAdminModelContainer()
            : base("name=CadAdminModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<EMSCAN_BATCH> EMSCAN_BATCH { get; set; }
        public DbSet<HL7_FTP_CONFIG> HL7_FTP_CONFIG { get; set; }
        public DbSet<IMPORT_ACC_DTL> IMPORT_ACC_DTL { get; set; }
        public DbSet<IMPORT_DFT_DTL> IMPORT_DFT_DTL { get; set; }
        public DbSet<IMPORT_DFT_HDR> IMPORT_DFT_HDR { get; set; }
        public DbSet<PLSQL_PROFILER_DATA> PLSQL_PROFILER_DATA { get; set; }
        public DbSet<PLSQL_PROFILER_RUNS> PLSQL_PROFILER_RUNS { get; set; }
        public DbSet<PLSQL_PROFILER_UNITS> PLSQL_PROFILER_UNITS { get; set; }
        public DbSet<PROCESS_STATUS_LOOKUP> PROCESS_STATUS_LOOKUP { get; set; }
        public DbSet<ROSTER_TYPE_LOOKUP> ROSTER_TYPE_LOOKUP { get; set; }
        public DbSet<SQLN_PROF_ANB> SQLN_PROF_ANB { get; set; }
        public DbSet<SQLN_PROF_PROFILES> SQLN_PROF_PROFILES { get; set; }
        public DbSet<SQLN_PROF_RUNS> SQLN_PROF_RUNS { get; set; }
        public DbSet<SQLN_PROF_SESS> SQLN_PROF_SESS { get; set; }
        public DbSet<SQLN_PROF_UNIT_HASH> SQLN_PROF_UNIT_HASH { get; set; }
        public DbSet<SQLN_PROF_UNITS> SQLN_PROF_UNITS { get; set; }
        public DbSet<WORKFLOW_STATUS_LOOKUP> WORKFLOW_STATUS_LOOKUP { get; set; }
        public DbSet<AMRACTIVATIONDETAIL> AMRACTIVATIONDETAILs { get; set; }
        public DbSet<AMRACTIVATIONEXCEPTION> AMRACTIVATIONEXCEPTIONs { get; set; }
        public DbSet<AMRACTIVATIONHDR> AMRACTIVATIONHDRs { get; set; }
        public DbSet<CHARTMOVER_RUNSTATUS> CHARTMOVER_RUNSTATUS { get; set; }
        public DbSet<CONTRACTWIZARDLOGGER> CONTRACTWIZARDLOGGERs { get; set; }
        public DbSet<CONTRACTWIZARDPAYOR> CONTRACTWIZARDPAYORs { get; set; }
        public DbSet<CONTRACTWIZARDQUESTION> CONTRACTWIZARDQUESTIONs { get; set; }
        public DbSet<EMSCAN_BATCH_CHECKOUT> EMSCAN_BATCH_CHECKOUT { get; set; }
        public DbSet<ENCTR> ENCTRs { get; set; }
        public DbSet<ENCTR_HISTORY> ENCTR_HISTORY { get; set; }
        public DbSet<GROUP_DEFINITION> GROUP_DEFINITION { get; set; }
        public DbSet<HCA_GED_INDICATOR> HCA_GED_INDICATOR { get; set; }
        public DbSet<HCA_MISSING_RUN> HCA_MISSING_RUN { get; set; }
        public DbSet<HL7_ATTRIBUTES> HL7_ATTRIBUTES { get; set; }
        public DbSet<HL7_DOCTYPREF> HL7_DOCTYPREF { get; set; }
        public DbSet<HL7_EREGI_ACCTCODE> HL7_EREGI_ACCTCODE { get; set; }
        public DbSet<HL7_FACPROFILE> HL7_FACPROFILE { get; set; }
        public DbSet<HL7_FILELOAD> HL7_FILELOAD { get; set; }
        public DbSet<HL7_FILESPLITINFO> HL7_FILESPLITINFO { get; set; }
        public DbSet<HL7_HCA_CHART_FILTER> HL7_HCA_CHART_FILTER { get; set; }
        public DbSet<HL7_MSGINFO> HL7_MSGINFO { get; set; }
        public DbSet<IMPORT_CLIENT_LKUP> IMPORT_CLIENT_LKUP { get; set; }
        public DbSet<IMPORT_LOGFILE> IMPORT_LOGFILE { get; set; }
        public DbSet<LAST_PROCESSED_EDLOG> LAST_PROCESSED_EDLOG { get; set; }
        public DbSet<OPTION_DEFINITION> OPTION_DEFINITION { get; set; }
        public DbSet<QUEST_SL_COLLECTION_DEF_REPOS> QUEST_SL_COLLECTION_DEF_REPOS { get; set; }
        public DbSet<QUEST_SL_COLLECTION_DEFINITION> QUEST_SL_COLLECTION_DEFINITION { get; set; }
        public DbSet<QUEST_SL_QUERY_DEF_REPOSITORY> QUEST_SL_QUERY_DEF_REPOSITORY { get; set; }
        public DbSet<QUEST_SL_QUERY_DEFINITIONS> QUEST_SL_QUERY_DEFINITIONS { get; set; }
        public DbSet<QUEST_SL_REPOS_BIND_VALUES> QUEST_SL_REPOS_BIND_VALUES { get; set; }
        public DbSet<QUEST_SL_REPOS_LAB_DETAILS> QUEST_SL_REPOS_LAB_DETAILS { get; set; }
        public DbSet<QUEST_SL_REPOS_PICK_DETAILS> QUEST_SL_REPOS_PICK_DETAILS { get; set; }
        public DbSet<QUEST_SL_REPOS_ROOT> QUEST_SL_REPOS_ROOT { get; set; }
        public DbSet<QUEST_SL_REPOS_SGA_DETAILS> QUEST_SL_REPOS_SGA_DETAILS { get; set; }
        public DbSet<QUEST_SL_REPOS_SGA_STATISTICS> QUEST_SL_REPOS_SGA_STATISTICS { get; set; }
        public DbSet<QUEST_SL_REPOSITORY_SQLAREA> QUEST_SL_REPOSITORY_SQLAREA { get; set; }
        public DbSet<QUEST_SL_REPOSITORY_TRANS_INFO> QUEST_SL_REPOSITORY_TRANS_INFO { get; set; }
        public DbSet<QUEST_SL_SQLAREA> QUEST_SL_SQLAREA { get; set; }
        public DbSet<RTI_ACCOUNTABILITY_MAP> RTI_ACCOUNTABILITY_MAP { get; set; }
        public DbSet<RTI_ACCTSCENARIO> RTI_ACCTSCENARIO { get; set; }
        public DbSet<RTI_BATCHINFO> RTI_BATCHINFO { get; set; }
        public DbSet<RTI_BATCHLOG> RTI_BATCHLOG { get; set; }
        public DbSet<RTI_EXCEPTION_CODE> RTI_EXCEPTION_CODE { get; set; }
        public DbSet<RTI_FILETYPE> RTI_FILETYPE { get; set; }
        public DbSet<RTI_IPA_MAP> RTI_IPA_MAP { get; set; }
        public DbSet<RTI_MISSING_CODE> RTI_MISSING_CODE { get; set; }
        public DbSet<RTI_OPTIONS> RTI_OPTIONS { get; set; }
        public DbSet<RTI_RULE> RTI_RULE { get; set; }
        public DbSet<RTI_RULES> RTI_RULES { get; set; }
        public DbSet<TORRGRIDS_TBL> TORRGRIDS_TBL { get; set; }
        public DbSet<TOWERIMPORT> TOWERIMPORTs { get; set; }
        public DbSet<TOWERIMPORTCLIENT> TOWERIMPORTCLIENTs { get; set; }
    }
}