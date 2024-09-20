using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class TransportProvider
    {
        [Key]
        public int ProviderID { get; set; }

        [Required]
        [StringLength(100)]
        public string? ProviderName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? ContactEmail { get; set; }

        [Phone]
        [StringLength(15)]
        public string? ContactPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string? ProviderType { get; set; } // e.g., 'Bus', 'Train', 'Flight'

        // Foreign key to the User who manages this provider
        public int UserID { get; set; }
        public User? User { get; set; }

        [Required]
        [StringLength(100)]
        public string? TransportName { get; set; }

        // Navigation property for transport schedules
        public ICollection<TransportSchedule>? TransportSchedules { get; set; }
    }

}
