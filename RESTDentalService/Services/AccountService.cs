using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System.Threading.Tasks;

namespace RESTDentalService.Services
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDTO user);
        Task<string> GenerateJWT(string login, string password);
    }

    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly DentalRestDbContext _context;
        private readonly IPasswordHasher<User> _hasher;

        public AccountService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task RegisterUser(RegisterUserDTO user)
        {
            var userDb = _mapper.Map<User>(user);
            var hashedPassword = _hasher.HashPassword(userDb, user.Password);

            userDb.PasswordHash = hashedPassword;
            _context.Users.Add(userDb);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateJWT(string login, string password) => "To be implemented";
    }
}
