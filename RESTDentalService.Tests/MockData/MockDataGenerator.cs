using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;
using System.Collections.Generic;

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

        public static CreateClinicDTO CreateClinicDTO()
        {
            return new CreateClinicDTO()
            {
                Name = "TEST",
                UniqueNumber = "ZAX1234567",
                ManagerName = "TEST",
                ManagerSurname = "TEST",
                Location = "TEST"
            };
        }

        public static Clinic CreateClinic()
        {
            return new Clinic()
            {
                Id = 1,
                Name = "TEST",
                UniqueNumber = "ZAX1234567",
                Location = "TEST"
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

        public static IEnumerable<Clinic> GetClinics()
        {
            yield return new Clinic()
            {
                Name = "Test",
                Id = 1,
                Location = "Test Loc",
                Manager = new Employee()
                {
                    Name = "TestName",
                    Surname = "TestSurname"
                }
            };
            yield return new Clinic()
            {
                Name = "Test",
                Id = 2,
                Location = "Test Loc",
                Manager = new Employee()
                {
                    Name = "TestName",
                    Surname = "TestSurname"
                }
            };
        }
    }
}
