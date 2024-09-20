using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Interface
{
    public interface IAdminDashboardMetrics
    {
        Task<IEnumerable<AdminDashboardMetrics>> GetAllMetricsAsync();
        Task<AdminDashboardMetrics> GetMetricByIdAsync(int id);
        Task AddMetricAsync(AdminDashboardMetrics metric);
        Task UpdateMetricAsync(AdminDashboardMetrics metric);
        Task DeleteMetricAsync(int id);
        Task<bool> MetricExistsAsync(int id);
    }
}
