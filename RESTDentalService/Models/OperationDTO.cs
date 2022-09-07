using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTDentalService.Models
{
    public class OperationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Clinic { get; set; }
        public string Doctor { get; set; }
        public decimal Cost { get; set; }
        public string Term { get; set; }
    }
}
