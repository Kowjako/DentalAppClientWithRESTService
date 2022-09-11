using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RESTDentalService.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RESTDentalService.Authorization
{
    public class AdminOrManagerRequirement : IAuthorizationRequirement { }

    public class AdminOrManagerRequirementHandler : AuthorizationHandler<AdminOrManagerRequirement, Clinic>
    {
        private readonly ILogger<AdminOrManagerRequirementHandler> _logger;

        public AdminOrManagerRequirementHandler(ILogger<AdminOrManagerRequirementHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                                                       AdminOrManagerRequirement requirement,
                                                       Clinic clinic)
        {
            var isAdmin = context.User.FindFirst(c => c.Type == ClaimTypes.Role);
            var isManager = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (isAdmin is null || isManager is null)
            {
                _logger.LogInformation("Próba nieautoryzowanego dostępu");
                return Task.CompletedTask; // jezeli klaim nie istnieje to 401 (Unauthorized)
            }

            if (isAdmin.Value.Equals("Admin"))
            {
                context.Succeed(requirement);
            }
            else
            {
                if(clinic.ManagerId == int.Parse(isManager.Value))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
