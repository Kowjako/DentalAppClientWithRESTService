using System;
using System.Collections.Generic;

#nullable disable

namespace RESTDentalService.Entity
{
    public partial class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClinicId { get; set; }
        public decimal Cost { get; set; }
        public DateTime Term { get; set; }
        public int EmployeeId { get; set; }

        public virtual Clinic Clinic { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
