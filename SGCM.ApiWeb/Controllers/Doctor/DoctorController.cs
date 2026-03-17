using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Interfaces;
using SGCM.Application.Interfaces.Availability;

namespace SGCM.ApiWeb.Controllers.Doctor
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorAppService _doctorService;
        private readonly IAvailabilityService _availabilityService;
        public DoctorController(
            IDoctorAppService doctorService,
            IAvailabilityService availabilityService)
        {
            _doctorService = doctorService;
            _availabilityService = availabilityService;
        }

        // Ruta para obtener un doctor por su ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _doctorService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        // Ruta para obtener las disponiblidades de un doctor
        [HttpGet("{id:int}/availability")]
        public async Task<IActionResult> GetDoctorAvailability(int id)
        {
            var result = await _availabilityService.GetByDoctorIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        // Ruta para obtener un doctor por su Especialidad
        [HttpGet]
        public async Task<IActionResult> GetBySpecialty([FromQuery] int specialtyId)
        {
            var result = await _doctorService.GetBySpecialtyIdAsync(specialtyId);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
