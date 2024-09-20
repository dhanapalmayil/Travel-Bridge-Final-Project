using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Service;

namespace Travel_Ticket_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController1 : ControllerBase
    {
        private readonly AuditLogService _auditLogService;

        public AuditLogsController1(AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        // GET: api/AuditLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs()
        {
            var auditLogs = await _auditLogService.GetAllAuditLogs();
            return Ok(auditLogs);
        }

        // GET: api/AuditLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditLog>> GetAuditLog(int id)
        {
            var auditLog = await _auditLogService.GetAuditLogById(id);

            if (auditLog == null)
            {
                return NotFound();
            }

            return Ok(auditLog);
        }

        // PUT: api/AuditLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuditLog(int id, AuditLog auditLog)
        {
            if (id != auditLog.AuditLogID)
            {
                return BadRequest();
            }

            await _auditLogService.UpdateAuditLog(auditLog);

            return NoContent();
        }

        // POST: api/AuditLogs
        [HttpPost]
        public async Task<ActionResult<AuditLog>> PostAuditLog(AuditLog auditLog)
        {
            await _auditLogService.AddAuditLog(auditLog);
            return CreatedAtAction(nameof(GetAuditLog), new { id = auditLog.AuditLogID }, auditLog);
        }

        // DELETE: api/AuditLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuditLog(int id)
        {
            var exists = await _auditLogService.AuditLogExists(id);
            if (!exists)
            {
                return NotFound();
            }

            await _auditLogService.DeleteAuditLog(id);

            return NoContent();
        }
    }
}

