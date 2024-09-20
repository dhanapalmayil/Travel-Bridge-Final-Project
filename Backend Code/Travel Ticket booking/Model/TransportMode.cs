using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class TransportMode
    {
        [Key]
        public int ModeID { get; set; }

        [Required]
        [StringLength(50)]
        public string? ModeName { get; set; } // e.g., 'Bus', 'Train', 'Flight'

        // Navigation property for transport schedules
        public ICollection<TransportSchedule>? TransportSchedules { get; set; }
    }

}
