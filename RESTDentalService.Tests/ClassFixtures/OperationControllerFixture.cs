using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RESTDentalService.Controllers;
using RESTDentalService.Entity;
using RESTDentalService.Services;

namespace RESTDentalService.Tests.ClassFixtures
{
    public class OperationControllerFixture
    {
        public OperationController Controller;
        public IOperationService Service;

        public Mock<DentalRestDbContext> DbMock;
        public Mock<IMapper> MapperMock;

        public OperationControllerFixture()
        {
            DbMock = new Mock<DentalRestDbContext>();
            MapperMock = new Mock<IMapper>();

            Service = new OperationService(DbMock.Object, MapperMock.Object, Mock.Of<ILogger<OperationService>>());
            Controller = new OperationController(Service);
        }
    }
}
