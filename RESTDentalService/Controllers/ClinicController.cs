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

        public async Task<ActionResult<List<ClinicDTO>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }
    }
}
