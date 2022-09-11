using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;

namespace RESTDentalService.Tests.MockData
{
    public static class MockDataGenerator
    {
        public static RegisterUserDTO GetRegisterUserDTO()
        {
            return new RegisterUserDTO()
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "test",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Password = "12345"
            };
        }

        public static User GetUser()
        {
            return new User()
            {
                Id = 1,
                Email = "test@test.com",
                FirstName = "test",
                LastName = "test",
                DateOfBirth = DateTime.Now.AddYears(-20),
                PasswordHash = "12345",
                RoleId = 1,
                Role = new Role() { Id = 1, Name = "Admin"}
            };
        }
    }
}
