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
    public class OperationControllerTests : IClassFixture<OperationControllerFixture>
    {
        private OperationControllerFixture _target;

        public OperationControllerTests(OperationControllerFixture target)
        {
            _target = target;
        }

        [Fact]
        public async Task Controller_GetOpreations_ShouldReturn200AndData()
        {
            // Arrange
            var data = MockDataGenerator.CreateOperations();
            var dbSetMock = data.ToList().AsQueryable().BuildMockDbSet();

            _target.MapperMock.Setup(p => p.Map<List<OperationDTO>>(It.IsAny<List<Operation>>()))
                              .Returns<List<Operation>>(p => new List<OperationDTO>(p.Count) { new OperationDTO(), new OperationDTO() });

            _target.DbMock.Setup(p => p.Operations).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.GetOperations(1, new DentalAdvQuery());

            // Assert
            Assert.NotNull(result.Result as OkObjectResult);
            Assert.Equal(2, ((result.Result as OkObjectResult).Value as List<OperationDTO>).Count);
        }

        [Fact]
        public async Task Controller_PostOpetaion_ShouldReturn201()
        {
            // Arrange
            var data = MockDataGenerator.CreateOperationDTO();
            var dbSetClinicMock = new List<Clinic> { MockDataGenerator.CreateClinic() }.AsQueryable().BuildMockDbSet();
            var dbSetEmployeeMock = new List<Employee> { MockDataGenerator.CreateEmployee() }.AsQueryable().BuildMockDbSet();

            _target.DbMock.Setup(p => p.Employees).Returns(dbSetEmployeeMock.Object);
            _target.DbMock.Setup(p => p.Clinics).Returns(dbSetClinicMock.Object);
            _target.DbMock.Setup(p => p.Operations.Add(It.IsAny<Operation>()));
            _target.MapperMock.Setup(p => p.Map<Operation>(It.IsAny<CreateOperationDTO>()))
                              .Returns(MockDataGenerator.CreateOperation());

            // Act
            var result = await _target.Controller.Post(1, data);

            // Assert
            _target.DbMock.Verify(p => p.Operations.Add(It.IsAny<Operation>()), Times.Once());
            Assert.NotNull(result as CreatedResult);
            Assert.Equal("api/clinic/1/operation/5", (result as CreatedResult).Location);
        }

        [Fact]
        public async Task Controller_DeleteAll_ShouldReturn204()
        {
            // Arrange
            var data = MockDataGenerator.CreateOperations();
            var dbSetMock = data.ToList().AsQueryable().BuildMockDbSet();

            _target.DbMock.Setup(p => p.Clinics).Returns(new List<Clinic> { MockDataGenerator.CreateClinic() }.AsQueryable().BuildMockDbSet().Object);
            _target.DbMock.Setup(p => p.Operations.RemoveRange(It.IsAny<IEnumerable<Operation>>()));
            _target.DbMock.Setup(p => p.Operations).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.DeleteAllOperations(1);

            // Assert
            _target.DbMock.Verify(p => p.Operations.RemoveRange(It.IsAny<IEnumerable<Operation>>()), Times.Once());
            Assert.NotNull(result as NoContentResult);
        }

        [Fact]
        public async Task Controller_DeleteEntry_ShouldReturn204()
        {
            // Arrange
            var data = MockDataGenerator.CreateOperations();
            var dbSetMock = data.ToList().AsQueryable().BuildMockDbSet();

            _target.DbMock.Setup(p => p.Operations.Remove(It.IsAny<Operation>()));
            _target.DbMock.Setup(p => p.Operations).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.DeleteOperation(data.ToList()[0].Id);

            // Assert
            _target.DbMock.Verify(p => p.Operations.Remove(It.IsAny<Operation>()), Times.Once());
            Assert.NotNull(result as NoContentResult);
        }
    }
}
