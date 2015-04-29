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
    
    public partial class RtTransBrokerAdminModelContainer : DbContext
    {
        public RtTransBrokerAdminModelContainer()
            : base("name=RtTransBrokerAdminModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ALERT> ALERTs { get; set; }
        public DbSet<ATTACHMENT> ATTACHMENTs { get; set; }
        public DbSet<CHANNEL> CHANNELs { get; set; }
        public DbSet<CHANNEL_STATISTICS> CHANNEL_STATISTICS { get; set; }
        public DbSet<CODE_TEMPLATE> CODE_TEMPLATE { get; set; }
        public DbSet<CONFIGURATION> CONFIGURATIONs { get; set; }
        public DbSet<CUSTOMIZESEGMENT> CUSTOMIZESEGMENTS { get; set; }
        public DbSet<ETL_CONFIG> ETL_CONFIG { get; set; }
        public DbSet<ETLDRIVER> ETLDRIVERs { get; set; }
        public DbSet<FILTERHL7> FILTERHL7 { get; set; }
        public DbSet<RTTRANS_HL7_FTP_CONFIG> RTTRANS_HL7_FTP_CONFIG { get; set; }
        public DbSet<MESSAGE> MESSAGEs { get; set; }
        public DbSet<ODSACC> ODSACCs { get; set; }
        public DbSet<PERSON> People { get; set; }
        public DbSet<SCRIPT> SCRIPTs { get; set; }
        public DbSet<STGACC> STGACCs { get; set; }
        public DbSet<TEMPLATE> TEMPLATEs { get; set; }
        public DbSet<ALERT_EMAIL> ALERT_EMAIL { get; set; }
        public DbSet<CHANNEL_ALERT> CHANNEL_ALERT { get; set; }
        public DbSet<CHANNEL_RUN_STATUS> CHANNEL_RUN_STATUS { get; set; }
        public DbSet<ENCRYPTION_KEY> ENCRYPTION_KEY { get; set; }
        public DbSet<HCA_CONVERSION_REF> HCA_CONVERSION_REF { get; set; }
        public DbSet<HL7_EDLOG> HL7_EDLOG { get; set; }
        public DbSet<HL7_MSG> HL7_MSG { get; set; }
        public DbSet<ODSEVN> ODSEVNs { get; set; }
        public DbSet<ODSGT1> ODSGT1 { get; set; }
        public DbSet<ODSIN1> ODSIN1 { get; set; }
        public DbSet<ODSMSH> ODSMSHes { get; set; }
        public DbSet<ODSNK1> ODSNK1 { get; set; }
        public DbSet<ODSPID> ODSPIDs { get; set; }
        public DbSet<ODSPV1> ODSPV1 { get; set; }
        public DbSet<PREFERENCE> PREFERENCES { get; set; }
        public DbSet<STGEVN> STGEVNs { get; set; }
        public DbSet<STGGT1> STGGT1 { get; set; }
        public DbSet<STGIN1> STGIN1 { get; set; }
        public DbSet<STGMSH> STGMSHes { get; set; }
        public DbSet<STGNK1> STGNK1 { get; set; }
        public DbSet<STGPID> STGPIDs { get; set; }
        public DbSet<STGPV1> STGPV1 { get; set; }
        public DbSet<TEST_DESTINATION> TEST_DESTINATION { get; set; }
        public DbSet<TEST_IDFILTER> TEST_IDFILTER { get; set; }
        public DbSet<TEST_SOURCE> TEST_SOURCE { get; set; }
        public DbSet<ODS_HL7MSG> ODS_HL7MSG { get; set; }
        public DbSet<ODS_HL7MSG_FLAT> ODS_HL7MSG_FLAT { get; set; }
    }
}
