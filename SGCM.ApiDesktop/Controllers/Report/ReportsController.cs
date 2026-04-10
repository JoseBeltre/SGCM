using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SGCM.Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IAppointmentAppService _appointmentAppService;

        public ReportsController(IAppointmentAppService appointmentAppService)
        {
            _appointmentAppService = appointmentAppService;
        }

        [HttpGet("appointments-stats")]
        public async Task<IActionResult> GetAppointmentStats()
        {
            var result = await _appointmentAppService.GetAllAsync();
            if (!result.IsSuccess) return BadRequest(result.Message);

            var appointments = result.Data;

            var stats = new
            {
                TotalAppointments = appointments.Count,
                ConfirmedAppointments = appointments.Count(a => a.Status == "Confirmada"),
                CancelledAppointments = appointments.Count(a => a.Status == "Cancelada"),
                CompletedAppointments = appointments.Count(a => a.Status == "Completada")
            };

            return Ok(stats);
        }
    }
}
