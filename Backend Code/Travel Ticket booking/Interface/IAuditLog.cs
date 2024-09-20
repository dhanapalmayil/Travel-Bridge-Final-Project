using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Interface
{
    public interface IAuditLog
    {
        Task<IEnumerable<AuditLog>> GetAllAuditLogs();
        Task<AuditLog> GetAuditLogById(int id);
        Task AddAuditLog(AuditLog auditLog);
        Task UpdateAuditLog(AuditLog auditLog);
        Task DeleteAuditLog(AuditLog auditLog);
        Task<bool> AuditLogExists(int id);
    }
}
