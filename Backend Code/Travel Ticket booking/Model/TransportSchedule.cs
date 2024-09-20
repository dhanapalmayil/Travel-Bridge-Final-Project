using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class TransportSchedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [Required]
        public int ProviderID { get; set; }
        public TransportProvider? TransportProvider { get; set; }

        [Required]
        public int ModeID { get; set; }
        public TransportMode? TransportMode { get; set; }

        [Required]
        [StringLength(100)]
        public string? TransportName { get; set; }

        [Required]
        [StringLength(100)]
        public string? Origin { get; set; }

        [Required]
        [StringLength(100)]
        public string? Destination { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public int SeatCapacity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int AvailableSeats { get; set; }

        // Navigation property for bookings
        public ICollection<Booking>? Bookings { get; set; }
    }

}
