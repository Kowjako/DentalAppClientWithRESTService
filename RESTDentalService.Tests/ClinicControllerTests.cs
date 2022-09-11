using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using RESTDentalService.Tests.ClassFixtures;
using RESTDentalService.Tests.MockData;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RESTDentalService.Tests
{
    public class ClinicControllerTests : IClassFixture<ClinicControllerFixture>
    {
        private ClinicControllerFixture _target;

        public ClinicControllerTests(ClinicControllerFixture target)
        {
            _target = target;
        }

        [Fact]
        public async Task Controller_GetAll_ShouldReturnData()
        {
            // Arrange
            var data = MockDataGenerator.GetClinics();
            var dbSetMock = data.ToList().AsQueryable().BuildMockDbSet();

            _target.MapperMock.Setup(p => p.Map<List<ClinicDTO>>(It.IsAny<List<Clinic>>()))
                              .Returns<List<Clinic>>(p => new List<ClinicDTO>(p.Count));

            _target.DbMock.Setup(p => p.Clinics).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.GetAll(new DentalAdvQuery());

            // Assert
            Assert.NotNull(result.Result as OkObjectResult);
            Assert.Equal(2, ((result.Result as OkObjectResult).Value as PagedResult<ClinicDTO>).TotalItemsCount);
        }

        [Fact]
        public async Task Controller_GetById_ShouldReturnData()
        {
            // Arrange
            var data = MockDataGenerator.GetClinics();
            var dbSetMock = data.ToList().AsQueryable().BuildMockDbSet();

            _target.MapperMock.Setup(p => p.Map<ClinicDTO>(It.IsAny<Clinic>()))
                              .Returns<Clinic>(p => new ClinicDTO { Id = p.Id });

            _target.DbMock.Setup(p => p.Clinics).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.Get(data.ToList()[0].Id);

            // Assert
            Assert.NotNull(result.Result as OkObjectResult);
            Assert.Equal(data.ToList()[0].Id, ((result.Result as OkObjectResult).Value as ClinicDTO).Id);
        }

        [Fact]
        public async Task Controller_PostData_ShouldCallDbContext()
        {
            // Arrange
            var data = MockDataGenerator.CreateClinicDTO();
            _target.DbMock.Setup(p => p.Employees).Returns(new List<Employee>().AsQueryable().BuildMockDbSet().Object);
            _target.DbMock.Setup(p => p.Clinics.Add(It.IsAny<Clinic>()));
            _target.MapperMock.Setup(p => p.Map<Clinic>(It.IsAny<CreateClinicDTO>()))
                              .Returns(MockDataGenerator.CreateClinic());

            // Act
            var result = await _target.Controller.CreateClinic(data);

            // Assert
            _target.DbMock.Verify(p => p.Clinics.Add(It.IsAny<Clinic>()), Times.Once());
            Assert.NotNull(result as CreatedResult);
            Assert.Equal("/api/clinic/1", (result as CreatedResult).Location);
        }

        [Fact]
        public async Task Controller_PutData_ShouldCallDbContext()
        {
            // Arrange
            var data = MockDataGenerator.CreateClinic();
            var clinicsMock = new List<Clinic> { data }.AsQueryable().BuildMockDbSet();

            clinicsMock.Setup(p => p.FindAsync(It.IsAny<int>()))
                       .ReturnsAsync(data);

            _target.DbMock.Setup(p => p.Clinics).Returns(clinicsMock.Object);

            // Act
            var result = await _target.Controller.UpdateClinic(data.Id, new UpdateClinicDTO()
            {
                Location = "Loc",
                Name = "NewName"
            });

            // Assert
            Assert.NotNull(result as OkResult);
        }

        [Fact]
        public async Task Controller_ProvidedId_ShouldDeleteEntry()
        {
            // Arrange
            var data = MockDataGenerator.CreateClinic();
            _target.AuthServiceMock.Setup(p => p.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),
                                                                It.IsAny<object>(),
                                                                It.Is<IEnumerable<IAuthorizationRequirement>>(p => p.Any())))
                                   .ReturnsAsync(AuthorizationResult.Success);

            var clinicsMock = new List<Clinic> { data }.AsQueryable().BuildMockDbSet();
            clinicsMock.Setup(p => p.FindAsync(It.IsAny<int>()))
                       .ReturnsAsync(data);

            _target.DbMock.Setup(p => p.Clinics.Remove(It.IsAny<Clinic>()));
            _target.DbMock.Setup(p => p.Clinics).Returns(clinicsMock.Object);

            // Act
            var result = await _target.Controller.DeleteClinic(data.Id);

            // Assert
            _target.DbMock.Verify(p => p.Clinics.Remove(It.IsAny<Clinic>()), Times.Once());
            Assert.NotNull(result as NoContentResult);
        }
    }
}
