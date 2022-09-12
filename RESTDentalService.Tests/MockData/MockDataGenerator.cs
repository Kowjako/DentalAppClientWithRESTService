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

        public static Operation CreateOperation()
        {
            return new Operation()
            {
                Id = 5,
                Name = "Test"
            };
        }

        public static CreateOperationDTO CreateOperationDTO()
        {
            return new CreateOperationDTO()
            {
                ClinicId = 1,
                Name = "Test",
                DoctorName = "Test",
                DoctorSurname = "Test",
                Cost = 10.0m
            };
        }

        public static Employee CreateEmployee()
        {
            return new Employee()
            {
                Id = 1,
                Name = "Test",
                Surname = "Test",
                Email = "kowjako@gmail.com"
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

        public static CreateEmployeeDTO CreateEmployeeDTO()
        {
            return new CreateEmployeeDTO()
            {
                Name = "Test",
                Surname = "Test",
                Phone = "111-111-111",
                ClinicUniqueNumber = "ZAX1234567"
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

        public static IEnumerable<Operation> CreateOperations()
        {
            yield return new Operation()
            {
                Id = 1,
                ClinicId = 1,
                Cost = 10.0m,
                Term = DateTime.Now,
                Name = "Test"
            };
            yield return new Operation()
            {
                Id = 2,
                ClinicId = 1,
                Cost = 10.0m,
                Term = DateTime.Now,
                Name = "Test"
            };
            yield return new Operation()
            {
                Id = 3,
                ClinicId = 2,
                Cost = 10.0m,
                Term = DateTime.Now,
                Name = "Test"
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
