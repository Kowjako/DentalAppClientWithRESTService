using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.Models
{
    public class CreateOperationDTO
    {
        public int ClinicId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Date { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
    }
}
