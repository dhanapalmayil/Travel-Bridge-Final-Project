using Microsoft.EntityFrameworkCore;
using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Repository
{
    public class AdminDashboardMetricsRepository: IAdminDashboardMetrics
    {
        private readonly TravelTicketingDbContext _context;

        public AdminDashboardMetricsRepository(TravelTicketingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AdminDashboardMetrics>> GetAllMetricsAsync()
        {
            return await _context.AdminDashboardMetrics.ToListAsync();
        }

        public async Task<AdminDashboardMetrics> GetMetricByIdAsync(int id)
        {
            return await _context.AdminDashboardMetrics.FindAsync(id);
        }

        public async Task AddMetricAsync(AdminDashboardMetrics metric)
        {
            await _context.AdminDashboardMetrics.AddAsync(metric);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMetricAsync(AdminDashboardMetrics metric)
        {
            _context.Entry(metric).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMetricAsync(int id)
        {
            var metric = await _context.AdminDashboardMetrics.FindAsync(id);
            if (metric != null)
            {
                _context.AdminDashboardMetrics.Remove(metric);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MetricExistsAsync(int id)
        {
            return await _context.AdminDashboardMetrics.AnyAsync(e => e.MetricID == id);
        }
    }
}
