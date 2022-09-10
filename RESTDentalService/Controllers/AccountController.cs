using Microsoft.AspNetCore.Mvc;
using RESTDentalService.Models;
using RESTDentalService.Services;
using System.Threading.Tasks;

namespace RESTDentalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            await _service.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("{login}/{password}")]
        public async Task<ActionResult<string>> Login([FromRoute]string login, [FromRoute]string password)
        {
            return Ok(await _service.GenerateJWT(login, password));
        }
    }
}
