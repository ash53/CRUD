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
    
    public partial class PERSON
    {
        public PERSON()
        {
            this.PREFERENCES = new HashSet<PREFERENCE>();
        }
    
        public decimal ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string SALT { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string ORGANIZATION { get; set; }
        public string EMAIL { get; set; }
        public string PHONENUMBER { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> LAST_LOGIN { get; set; }
        public string LOGGED_IN { get; set; }
    
        public virtual ICollection<PREFERENCE> PREFERENCES { get; set; }
    }
}
