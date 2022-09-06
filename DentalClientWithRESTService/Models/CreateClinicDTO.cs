using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinicWithRestService.Models
{
    public class CreateClinicDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string UniqueNumber { get; set; }

        public string ManagerName { get; set; }
        public string ManagerSurname { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerEmail { get; set; }
    }
}
