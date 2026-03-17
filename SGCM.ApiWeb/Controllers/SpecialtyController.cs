using Microsoft.AspNetCore.Mvc;
using SGCM.Application.DTOs.Specialty;
using SGCM.Application.Interfaces.Specialty;

namespace SGCM.ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtyController : Controller
    {
        private readonly ISpecialtyService _specialtyService;
        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }
        // Ruta para obtener todas las especialidades
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? active = null)
        {
            if (active.HasValue)
            {
                var result = await _specialtyService.GetByStatusAsync(active.Value);
                if (!result.IsSuccess)
                    return NotFound(result.Message);
                return Ok(result.Data);
            } else
            {
                var result = await _specialtyService.GetAllAsync();
                if (!result.IsSuccess)
                    return NotFound(result.Message);
                return Ok(result.Data);
            }     
        }

        // Ruta para obtener una especialidad por su ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _specialtyService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        // Ruta para crear una nueva especialidad
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto request) {
            var result = await _specialtyService.CreateAsync(request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Data.Id},
                    result.Data
                );
        }

        // Ruta para actualizar por COMPLETO una especialdad existente
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyDto request) {
            var result = await _specialtyService.UpdateAsync(id, request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return NoContent();
        }

        // Ruta para desactivar una especialidad por su ID
        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _specialtyService.DeactivateAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return NoContent();
        }

        // Ruta para eliminar una especialidad por su ID
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _specialtyService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return NoContent();
        }
    }
}
