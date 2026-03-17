using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Interfaces;

namespace SGCM.ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientAppService _patientService;
        public PatientController(IPatientAppService patientService)
        {
            _patientService = patientService;
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
    }
}
