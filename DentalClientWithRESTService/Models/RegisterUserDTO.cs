using System;

namespace DentalClientWithRESTService.Models
{
    public class RegisterUserDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; } = 3; // User
    }
}
