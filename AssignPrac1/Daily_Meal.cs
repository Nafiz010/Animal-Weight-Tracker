//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AssignPrac1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Daily_Meal
    {
        public int Id { get; set; }
        public Nullable<int> Log_Id { get; set; }
        public Nullable<int> Meal_Id { get; set; }
        public Nullable<decimal> Meal_Intake { get; set; }
    
        public virtual Daily_Log Daily_Log { get; set; }
        public virtual Meal Meal { get; set; }
    }
}
