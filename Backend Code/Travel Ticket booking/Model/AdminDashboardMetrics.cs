using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class AdminDashboardMetrics
    {
        [Key]
        public int MetricID { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int TotalBookings { get; set; }

        public int TotalUsers { get; set; }

        public int TotalProviders { get; set; }

        public decimal TotalSales { get; set; }

        // Add more metrics as needed
    }

}
