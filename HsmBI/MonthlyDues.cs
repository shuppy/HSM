//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HsmBI
{
    using System;
    using System.Collections.Generic;
    
    public partial class MonthlyDues
    {
        public int id { get; set; }
        public string DateId { get; set; }
    
        public virtual Transactions Transactions { get; set; }
    }
}
