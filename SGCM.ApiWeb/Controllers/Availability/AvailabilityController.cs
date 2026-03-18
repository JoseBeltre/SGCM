using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Interfaces.Availability;

namespace SGCM.ApiWeb.Controllers.Availability
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;
        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }
        // Ruta para obtener todas las disponibilidades
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _availabilityService.GetAllAsync();
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data); 
        }
    }
}
