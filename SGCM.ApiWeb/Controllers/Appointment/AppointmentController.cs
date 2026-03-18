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

        // Ruta para obtener una cita por su ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _appointmentService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        // Ruta para craer una nueva cita
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAppointmentDto request)
        {
            var result = await _appointmentService.CreateAsync(request);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        // Ruta para cancelar una cita
        [HttpPatch("{id:int}/cancelar")]
        public async Task<IActionResult> Cancel(int id, [FromBody] string reason)
        {
            var result = await _appointmentService.CancelAsync(id, reason);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return NoContent();
        }

        // Ruta para confirmar una cita
        [HttpPatch("{id:int}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _appointmentService.ConfirmAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return NoContent();
        }
    }
}
