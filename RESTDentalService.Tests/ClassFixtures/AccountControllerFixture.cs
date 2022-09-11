using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using RESTDentalService.Authentication;
using RESTDentalService.Controllers;
using RESTDentalService.Entity;
using RESTDentalService.Services;

namespace RESTDentalService.Tests.ClassFixtures
{
    public class AccountControllerFixture
    {
        public AccountController Controller;
        public IAccountService AccountService;
        public Mock<DentalRestDbContext> DbMock;
        public Mock<IMapper> MapperMock;
        public Mock<IPasswordHasher<User>> HasherMock;

        public AccountControllerFixture()
        {
            MapperMock = new Mock<IMapper>();
            DbMock = new Mock<DentalRestDbContext>();
            HasherMock = new Mock<IPasswordHasher<User>>();
            AccountService = new AccountService(MapperMock.Object,
                                                DbMock.Object,
                                                HasherMock.Object,
                                                new AuthSettings() { JwtExpireDays = 1, 
                                                                     JwtIssuer = "test",
                                                                     JwtKey = "TEST_TEST_TEST_TEST_TEST" });
            Controller = new AccountController(AccountService);
        }
    }
}
