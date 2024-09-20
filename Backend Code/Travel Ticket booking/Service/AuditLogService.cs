using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Repository;

namespace Travel_Ticket_booking.Service
{
    public class AuditLogService
    {
        private readonly IAuditLog _auditLogRepository;
        public AuditLogService(IAuditLog auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAuditLogs()
        {
            return await _auditLogRepository.GetAllAuditLogs();
        }

        public async Task<AuditLog> GetAuditLogById(int id)
        {
            return await _auditLogRepository.GetAuditLogById(id);
        }

        public async Task AddAuditLog(AuditLog auditLog)
        {
            await _auditLogRepository.AddAuditLog(auditLog);
        }

        public async Task UpdateAuditLog(AuditLog auditLog)
        {
            await _auditLogRepository.UpdateAuditLog(auditLog);
        }

        public async Task DeleteAuditLog(int id)
        {
            var auditLog = await _auditLogRepository.GetAuditLogById(id);
            if (auditLog != null)
            {
                await _auditLogRepository.DeleteAuditLog(auditLog);
            }
        }

        public async Task<bool> AuditLogExists(int id)
        {
            return await _auditLogRepository.AuditLogExists(id);
        }
    }
}
