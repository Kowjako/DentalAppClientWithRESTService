using Microsoft.AspNetCore.Mvc;
using RESTDentalService.Models;
using RESTDentalService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTDentalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _service;

        public ClinicController(IClinicService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClinicDTO>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ClinicDTO>>> Get([FromRoute]int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult> CreateClinic([FromBody]CreateClinicDTO dto)
        {
            var id = await _service.Create(dto);
            return Created($"/api/clinic/{id}", null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClinic([FromRoute]int id, [FromBody]UpdateClinicDTO dto)
        {
            await _service.Update(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClinic([FromRoute]int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
