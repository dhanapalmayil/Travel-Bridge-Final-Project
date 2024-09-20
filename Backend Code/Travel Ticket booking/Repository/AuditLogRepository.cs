using Microsoft.EntityFrameworkCore;
using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Repository
{
    public class AuditLogRepository:IAuditLog
    {
        private readonly TravelTicketingDbContext _context; 

        public AuditLogRepository(TravelTicketingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAuditLogs()
        {
            return await _context.AuditLogs.ToListAsync();
        }

        public async Task<AuditLog> GetAuditLogById(int id)
        {
            return await _context.AuditLogs.FindAsync(id);
        }

        public async Task AddAuditLog(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuditLog(AuditLog auditLog)
        {
            _context.Entry(auditLog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuditLog(AuditLog auditLog)
        {
            _context.AuditLogs.Remove(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AuditLogExists(int id)
        {
            return await _context.AuditLogs.AnyAsync(e => e.AuditLogID == id);
        }
    }
}
