using Microsoft.AspNetCore.Mvc;
using SGCM.Application.DTOs.Specialty;
using SGCM.Application.Interfaces.Specialty;

namespace SGCM.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de Especialidades Médicas.
    /// Endpoint: api/specialties
    /// </summary>
    [ApiController]
    [Route("api/specialties")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _specialtyService.GetAllAsync();
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _specialtyService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto dto)
        {
            var result = await _specialtyService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyDto dto)
        {
            var result = await _specialtyService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _specialtyService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _specialtyService.DeactivateAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}
