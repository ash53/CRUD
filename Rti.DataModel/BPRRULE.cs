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
    
    public partial class BPRRULE
    {
        public string NPI { get; set; }
        public string DESCRIPTION { get; set; }
        public string MONITORFOLDER { get; set; }
        public Nullable<System.DateTime> RTIBILLINGSTARTDATE { get; set; }
        public Nullable<System.DateTime> RTIBILLINGENDDATE { get; set; }
        public string THIRDPARTYDROPFOLDER { get; set; }
        public string RENAMEFILEMASK { get; set; }
        public string ENABLED { get; set; }
        public Nullable<System.DateTime> CREATEDTIMESTAMP { get; set; }
        public Nullable<System.DateTime> MODIFIEDTIMESTAMP { get; set; }
    }
}