using Microsoft.AspNetCore.Mvc;
using RESTDentalService.Models;
using RESTDentalService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTDentalService.Controllers
{
    [ApiController]
    [Route("api/clinic/{clinicId}/operation")]
    public class OperationController : ControllerBase
    {
        private readonly IClinicService _service;

        public OperationController(IClinicService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OperationDTO>>> GetOperations([FromRoute]DentalAdvQuery query)
        {

        }
    }
}
