using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTDentalService.Models;
using RESTDentalService.Services;
using System.Threading.Tasks;

namespace RESTDentalService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<EmployeeDTO>>> GetAll([FromQuery]DentalAdvQuery query)
        {
            return Ok(await _service.GetAll(query));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ClinicDTO>> Get([FromRoute] int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateEmployee([FromBody] CreateEmployeeDTO dto)
        {
            var id = await _service.Create(dto);
            return Created($"/api/employee/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> UpdateEmployee([FromRoute] int id, [FromBody] UpdateEmployeeDTO dto)
        {
            await _service.Update(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return NoContent();
        }

    }
}
