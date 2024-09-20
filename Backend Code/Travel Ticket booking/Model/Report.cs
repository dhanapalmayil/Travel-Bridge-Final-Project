using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class Report
    {
        public int ReportID { get; set; }

        [Required]
        public string? ReportType { get; set; } // e.g., 'Sales', 'User Activity', 'Provider Performance'

        [Required]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        public string? ReportContent { get; set; } // This could be a serialized JSON or other format of report content

    
    }

}
