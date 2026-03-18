using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Interfaces;

namespace SGCM.ApiWeb.Controllers.Patient
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientAppService _patientService;
        private readonly IAppointmentAppService _appointmentService;
        public PatientController(
            IPatientAppService patientService,
            IAppointmentAppService appointmentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        // Ruta para obtener un paciente por su ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _patientService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        // Ruta para obtener las citas de un paciente
        [HttpGet("{id:int}/appointments")]
        public async Task<IActionResult> GetDoctorAppointments(int id)
        {
            var result = await _appointmentService.GetByDoctorIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
