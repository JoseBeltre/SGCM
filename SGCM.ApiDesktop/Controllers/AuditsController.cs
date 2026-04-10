using Microsoft.AspNetCore.Mvc;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;

namespace SGCM.Api.Controllers
{
    /// <summary>
    /// Controlador para consultar los logs de auditoría del sistema.
    /// </summary>
    [ApiController]
    [Route("api/audits")]
    public class AuditsController : ControllerBase
    {
        private readonly IAuditLogRepository _auditRepo;

        public AuditsController(IAuditLogRepository auditRepo)
        {
            _auditRepo = auditRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Usamos un rango de fechas amplio ya que GetAll no está en el repo.
            var logs = await _auditRepo.GetByDateRangeAsync(new DateTime(2000, 1, 1), DateTime.MaxValue); 
            if (!logs.IsSuccess)
                return BadRequest(logs.Message);
                
            return Ok(logs.Data);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var logs = await _auditRepo.GetByUserIdAsync(userId);
            if (!logs.IsSuccess)
                return BadRequest(logs.Message);
            return Ok(logs.Data);
        }

        [HttpGet("type/{entityType}/entity/{entityId}")]
        public async Task<IActionResult> GetByEntity(EntityType entityType, int entityId)
        {
            var logs = await _auditRepo.GetByEntityAsync(entityType, entityId);
            if (!logs.IsSuccess)
                return BadRequest(logs.Message);
            return Ok(logs.Data);
        }
    }
}
