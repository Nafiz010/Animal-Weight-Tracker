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

    public abstract class Person
    {
        public int Staff_Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Designation { get; set; }
    }
    public partial class Staff:Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Staff()
        {
            this.Courses = new HashSet<Course>();
        }
    
        
        public Nullable<int> Org_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
