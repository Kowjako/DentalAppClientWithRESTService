using System.Collections.Generic;

namespace RESTDentalService.Entity
{
    public partial class Employee
    {
        public Employee()
        {
            Operations = new HashSet<Operation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? ClinicId { get; set; }

        public virtual Clinic ClinicNavigation { get; set; }
        public virtual Clinic Clinic { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
