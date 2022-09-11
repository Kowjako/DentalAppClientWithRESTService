using Microsoft.AspNetCore.Identity;
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
    public class AccountControllerTests : IClassFixture<AccountControllerFixture>
    {
        private AccountControllerFixture _target;

        public AccountControllerTests(AccountControllerFixture target)
        {
            _target = target;
        }

        [Fact]
        public async Task Controller_Register_ShouldReturn200Status()
        {
            // Arrange 
            var userDTO = MockDataGenerator.GetRegisterUserDTO();
            _target.DbMock.Setup(p => p.Users.Add(It.IsAny<User>())).Verifiable();
            _target.MapperMock.Setup(p => p.Map<User>(It.IsAny<RegisterUserDTO>())).Returns(MockDataGenerator.GetUser());

            // Act
            var result = await _target.Controller.RegisterUser(userDTO);

            // Assert
            _target.DbMock.Verify(p => p.Users.Add(It.IsAny<User>()), Times.Once());
            Assert.NotNull(result as OkResult);
        }

        [Fact]
        public async Task Controller_Login_ShouldReturn200StatusAndToken()
        {
            // Arrange
            var userDTO = MockDataGenerator.GetUser();
            var dbSetMock = new List<User>() { MockDataGenerator.GetUser() }.AsQueryable().BuildMockDbSet();

            _target.HasherMock.Setup(p => p.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(PasswordVerificationResult.Success);

            _target.DbMock.Setup(p => p.Users).Returns(dbSetMock.Object);

            // Act
            var result = await _target.Controller.Login(userDTO.Email, userDTO.PasswordHash);

            // Assert
            Assert.NotNull(result.Result as OkObjectResult);
            Assert.NotNull(((OkObjectResult)result.Result).Value);
        }
    }
}
