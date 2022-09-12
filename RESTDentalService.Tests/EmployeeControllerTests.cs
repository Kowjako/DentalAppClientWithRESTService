using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using RESTDentalService.Tests.ClassFixtures;
using RESTDentalService.Tests.MockData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RESTDentalService.Tests
{
    public class EmployeeControllerTests : IClassFixture<EmployeeControllerFixture>
    {
        private EmployeeControllerFixture _target;

        public EmployeeControllerTests(EmployeeControllerFixture target)
        {
            _target = target;
        }

        [Fact]
        public async Task Controller_GetAll_ShouldReturn200AndData()
        {
            // Arrange
            var data = MockDataGenerator.CreateEmployee();
            var dbSetMock = new List<Employee> { data }.AsQueryable().BuildMockDbSet();

            _target.MapperMock.Setup(p => p.Map<List<EmployeeDTO>>(It.IsAny<List<Employee>>()))
                              .Returns<List<Employee>>(p => new List<EmployeeDTO>(p.Count));

            _target.DbMock.Setup(p => p.Employees).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.GetAll(new DentalAdvQuery());

            // Assert
            Assert.NotNull(result.Result as OkObjectResult);
            Assert.Equal(1, ((result.Result as OkObjectResult).Value as PagedResult<EmployeeDTO>).TotalItemsCount);
        }

        [Fact]
        public async Task Controller_GetById_ShouldReturn200AndData()
        {
            // Arrange
            var data = MockDataGenerator.CreateEmployee();
            var dbSetMock = new List<Employee> { data }.AsQueryable().BuildMockDbSet();

            _target.MapperMock.Setup(p => p.Map<EmployeeDTO>(It.IsAny<Employee>()))
                              .Returns<Employee>(p => new EmployeeDTO { Id = p.Id });

            _target.DbMock.Setup(p => p.Employees).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.Get(data.Id);

            // Assert
            Assert.NotNull(result.Result as OkObjectResult);
            Assert.Equal(data.Id, ((result.Result as OkObjectResult).Value as EmployeeDTO).Id);
        }

        [Fact]
        public async Task Controller_PostData_ShouldReturn201()
        {
            // Arrange
            var data = MockDataGenerator.CreateEmployeeDTO();
            var dbSetMock = new List<Clinic> { MockDataGenerator.CreateClinic() }.AsQueryable().BuildMockDbSet();

            _target.DbMock.Setup(p => p.Clinics).Returns(dbSetMock.Object);
            _target.DbMock.Setup(p => p.Employees.Add(It.IsAny<Employee>()));
            _target.MapperMock.Setup(p => p.Map<Employee>(It.IsAny<CreateEmployeeDTO>()))
                              .Returns(MockDataGenerator.CreateEmployee());

            // Act
            var result = await _target.Controller.CreateEmployee(data);

            // Assert
            _target.DbMock.Verify(p => p.Employees.Add(It.IsAny<Employee>()), Times.Once());
            Assert.NotNull(result as CreatedResult);
            Assert.Equal("/api/employee/1", (result as CreatedResult).Location);
        }

        [Fact]
        public async Task Controller_PutData_ShouldReturn200()
        {
            // Arrange
            var data = MockDataGenerator.CreateEmployee();
            var employeeMock = new List<Employee> { data }.AsQueryable().BuildMockDbSet();

            employeeMock.Setup(p => p.FindAsync(It.IsAny<int>()))
                        .ReturnsAsync(data);

            _target.DbMock.Setup(p => p.Employees).Returns(employeeMock.Object);

            // Act
            var result = await _target.Controller.UpdateEmployee(data.Id, new UpdateEmployeeDTO()
            {
                Name = "NewName"
            });

            // Assert
            Assert.NotNull(result as OkResult);
        }

        [Fact]
        public async Task Controller_DeleteById_ShouldReturn204()
        {
            // Arrange
            var data = MockDataGenerator.CreateEmployee();

            var employeeMock = new List<Employee> { data }.AsQueryable().BuildMockDbSet();
            employeeMock.Setup(p => p.FindAsync(It.IsAny<int>()))
                        .ReturnsAsync(data);

            _target.DbMock.Setup(p => p.Employees.Remove(It.IsAny<Employee>()));
            _target.DbMock.Setup(p => p.Employees).Returns(employeeMock.Object);

            // Act
            var result = await _target.Controller.DeleteEmployee(data.Id);

            // Assert
            _target.DbMock.Verify(p => p.Employees.Remove(It.IsAny<Employee>()), Times.Once());
            Assert.NotNull(result as NoContentResult);
        }
    }
}
