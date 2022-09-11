using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using RESTDentalService.Authorization;
using RESTDentalService.Controllers;
using RESTDentalService.Entity;
using RESTDentalService.Services;

namespace RESTDentalService.Tests.ClassFixtures
{
    public class ClinicControllerFixture
    {
        public ClinicController Controller;
        public IClinicService ClinicService;
        public Mock<DentalRestDbContext> DbMock;
        public Mock<IMapper> MapperMock;
        public Mock<ILogger<ClinicService>> LoggerMock;
        public Mock<IAuthorizationService> AuthServiceMock;
        public Mock<IUserContextService> UserServiceMock;

        public ClinicControllerFixture()
        {
            MapperMock = new Mock<IMapper>();
            LoggerMock = new Mock<ILogger<ClinicService>>();
            DbMock = new Mock<DentalRestDbContext>();
            AuthServiceMock = new Mock<IAuthorizationService>();
            UserServiceMock = new Mock<IUserContextService>();

            ClinicService = new ClinicService(DbMock.Object,
                                              MapperMock.Object,
                                              LoggerMock.Object,
                                              AuthServiceMock.Object,
                                              UserServiceMock.Object);

            Controller = new ClinicController(ClinicService);
        }
    }
}
