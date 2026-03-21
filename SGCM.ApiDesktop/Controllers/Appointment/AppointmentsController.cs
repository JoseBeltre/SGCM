using Microsoft.AspNetCore.Mvc;
using SGCM.Application.DTOs.Appointment;
using SGCM.Application.Interfaces;

namespace SGCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentAppService _appointmentAppService;

        public AppointmentsController(IAppointmentAppService appointmentAppService)
        {
            _appointmentAppService = appointmentAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _appointmentAppService.GetAllAsync();
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _appointmentAppService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _appointmentAppService.GetByPatientIdAsync(patientId);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var result = await _appointmentAppService.GetByDoctorIdAsync(doctorId);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAppointmentDto dto)
        {
            var result = await _appointmentAppService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            var result = await _appointmentAppService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentAppService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _appointmentAppService.ConfirmAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromBody] RemoveAppointmentDto dto)
        {
            var result = await _appointmentAppService.CancelAsync(id, dto.CancellationReason);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _appointmentAppService.CompleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}