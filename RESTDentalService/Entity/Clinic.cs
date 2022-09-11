using System;
using System.Collections.Generic;

#nullable disable

namespace RESTDentalService.Entity
{
    public partial class Clinic
    {
        public Clinic()
        {
            Employees = new HashSet<Employee>();
            Operations = new HashSet<Operation>();
        }

        public int Id { get; set; }
        public string UniqueNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ManagerId { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
