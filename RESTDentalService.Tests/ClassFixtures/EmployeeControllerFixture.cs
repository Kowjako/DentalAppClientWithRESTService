using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RESTDentalService.Controllers;
using RESTDentalService.Entity;
using RESTDentalService.Services;

namespace RESTDentalService.Tests.ClassFixtures
{
    public class EmployeeControllerFixture
    {
        public EmployeeController Controller;
        public IEmployeeService Service;

        public Mock<DentalRestDbContext> DbMock;
        public Mock<IMapper> MapperMock;

        public EmployeeControllerFixture()
        {
            DbMock = new Mock<DentalRestDbContext>();
            MapperMock = new Mock<IMapper>();

            Service = new EmployeeService(DbMock.Object, MapperMock.Object, Mock.Of<ILogger<EmployeeService>>());
            Controller = new EmployeeController(Service);
        }
    }
}
