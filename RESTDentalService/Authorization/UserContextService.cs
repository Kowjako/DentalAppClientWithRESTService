using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RESTDentalService.Authorization
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _context;

        public UserContextService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public ClaimsPrincipal User => _context.HttpContext?.User;
        public int? UserId => User == null ? null : int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
