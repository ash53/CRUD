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
    
    public partial class HL7_FTP_CONFIG
    {
        public decimal HL7_FTP_CONFIG_ID { get; set; }
        public string REMOTESERVER { get; set; }
        public string REMOTEUSER { get; set; }
        public string REMOTEPWD { get; set; }
        public string REMOTEFOLDER { get; set; }
        public string FOLDERCATEGORY { get; set; }
        public Nullable<decimal> RTICLIENTCODE { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATE_USERID { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string UPDATE_USERID { get; set; }
        public string LOCALFOLDER { get; set; }
        public string CLIENTGROUP { get; set; }
        public string ISACTIVE { get; set; }
        public string NCFTPGETRMSRVR { get; set; }
        public string NCFTPGETRMUSR { get; set; }
        public string NCFTPGETRMPWD { get; set; }
        public string NCFTPGETRMFOLDER { get; set; }
        public string NCFTPGETRMLOCALFOLDER { get; set; }
        public string FILEMASK { get; set; }
    }
}