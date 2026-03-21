using Microsoft.AspNetCore.Mvc;
using SGCM.Application.DTOs.Doctor;
using SGCM.Application.Interfaces;

namespace SGCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorAppService _doctorAppService;

        public DoctorsController(IDoctorAppService doctorAppService)
        {
            _doctorAppService = doctorAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _doctorAppService.GetAllAsync();
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _doctorAppService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("specialty/{specialtyId}")]
        public async Task<IActionResult> GetBySpecialty(int specialtyId)
        {
            var result = await _doctorAppService.GetBySpecialtyIdAsync(specialtyId);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDoctorDto dto)
        {
            var result = await _doctorAppService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
        {
            var result = await _doctorAppService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doctorAppService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _doctorAppService.DeactivateAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}