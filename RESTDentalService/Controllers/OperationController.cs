using Microsoft.AspNetCore.Mvc;
using RESTDentalService.Models;
using RESTDentalService.Services;
using System.Threading.Tasks;

namespace RESTDentalService.Controllers
{
    [ApiController]
    [Route("api/clinic/{clinicId}/operation")]
    public class OperationController : ControllerBase
    {
        private readonly IOperationService _service;

        public OperationController(IOperationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OperationDTO>>> GetOperations([FromRoute]int clinicId, [FromRoute]DentalAdvQuery query)
        {
            return Ok(await _service.GetAll(clinicId, query));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromRoute]int clinicId, [FromBody]CreateOperationDTO dto)
        {
            var operationId = await _service.Create(clinicId, dto);
            return Created($"api/clinic/{clinicId}/operation/{operationId}", null);
        }

        [HttpDelete("{operationId}")]
        public async Task<ActionResult> DeleteOperation([FromRoute]int operationId)
        {
            await _service.DeleteById(operationId);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllOperations([FromRoute]int clinicId)
        {
            await _service.DeleteAllForClinic(clinicId);
            return NoContent();
        }
    }
}
