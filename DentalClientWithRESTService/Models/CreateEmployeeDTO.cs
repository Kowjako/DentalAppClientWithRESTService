using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinicWithRestService.Models
{
    public class CreateEmployeeDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ClinicUniqueNumber { get; set; }
    }
}
