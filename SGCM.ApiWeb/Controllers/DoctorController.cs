using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Interfaces;

namespace SGCM.ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorAppService _doctorService;
        public DoctorController(IDoctorAppService doctorService)
        {
            _doctorService = doctorService;
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
