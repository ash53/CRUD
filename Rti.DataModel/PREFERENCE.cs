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
    
    public partial class PREFERENCE
    {
        public decimal PERSON_ID { get; set; }
        public string NAME { get; set; }
        public string VALUE { get; set; }
    
        public virtual PERSON PERSON { get; set; }
    }
}