using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTDentalService.Models
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; } = 3; // User
    }
}
