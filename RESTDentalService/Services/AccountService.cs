using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RESTDentalService.Authentication;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly AuthSettings _auth;

        public AccountService(IMapper mapper, DentalRestDbContext context, IPasswordHasher<User> hasher, AuthSettings auth)
        {
            _mapper = mapper;
            _hasher = hasher;
            _context = context;
            _auth = auth;
        }

        public async Task RegisterUser(RegisterUserDTO user)
        {
            var userDb = _mapper.Map<User>(user);
            var hashedPassword = _hasher.HashPassword(userDb, user.Password);

            userDb.PasswordHash = hashedPassword;
            _context.Users.Add(userDb);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateJWT(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == login);
            await _context.Entry(user).Reference(u => u.Role).LoadAsync();

            if (user == null) throw new ArgumentNullException("Takiego użytkownika nie istnieje");

            var isPassCorrect = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (isPassCorrect == PasswordVerificationResult.Failed) throw new ArgumentNullException("Niepoprawne hasło");

            /* Generacja JWT tokenu */

            /* 1 - uzupelnienie claim'ow */
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };

            /* 2 - szyforwanie prywatnego klucza tokenu */
            var authPrivateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.JwtKey));
            var credentials = new SigningCredentials(authPrivateKey, SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.Now.AddDays(_auth.JwtExpireDays);

            /* 3 - generowanie tokenu uzytkownika */
            var token = new JwtSecurityToken(_auth.JwtIssuer, _auth.JwtIssuer, claims,
                                             expires: expireDate,
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
