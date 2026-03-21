using Microsoft.AspNetCore.Mvc;
using SGCM.Application.DTOs.Patient;
using SGCM.Application.Interfaces;

namespace SGCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientAppService _patientAppService;

        public PatientsController(IPatientAppService patientAppService)
        {
            _patientAppService = patientAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _patientAppService.GetAllAsync();
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _patientAppService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("national-id/{nationalId}")]
        public async Task<IActionResult> GetByNationalId(string nationalId)
        {
            var result = await _patientAppService.GetByNationalIdAsync(nationalId);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPatientDto dto)
        {
            var result = await _patientAppService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
        {
            var result = await _patientAppService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientAppService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}