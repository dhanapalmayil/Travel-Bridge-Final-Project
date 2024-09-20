using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Repository;
using Travel_Ticket_booking.Interface;

namespace Travel_Ticket_booking.Service
{
    public class AdminDashboardMetricsService
    {
        private readonly IAdminDashboardMetrics _repository;

        public AdminDashboardMetricsService(IAdminDashboardMetrics repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AdminDashboardMetrics>> GetAllMetricsAsync()
        {
            return await _repository.GetAllMetricsAsync();
        }

        public async Task<AdminDashboardMetrics> GetMetricByIdAsync(int id)
        {
            return await _repository.GetMetricByIdAsync(id);
        }

        public async Task AddMetricAsync(AdminDashboardMetrics metric)
        {
            await _repository.AddMetricAsync(metric);
        }

        public async Task UpdateMetricAsync(int id, AdminDashboardMetrics metric)
        {
            if (id != metric.MetricID)
            {
                throw new ArgumentException("Metric ID mismatch");
            }

            await _repository.UpdateMetricAsync(metric);
        }

        public async Task DeleteMetricAsync(int id)
        {
            await _repository.DeleteMetricAsync(id);
        }
    }
}
