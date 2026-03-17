using Microsoft.AspNetCore.Mvc;
using SGCM.Application.DTOs.Appointment;
using SGCM.Application.Interfaces;

namespace SGCM.ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentAppService _appointmentService;
        public AppointmentController(IAppointmentAppService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // Ruta para obtener un paciente por su ID
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAppointmentDto request)
        {
            var result = await _appointmentService.CreateAsync(request);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
