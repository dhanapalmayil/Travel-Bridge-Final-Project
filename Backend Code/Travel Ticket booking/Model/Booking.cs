using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class Booking
    {
        public int BookingID { get; set; }

        [Required]
        public int UserID { get; set; }
        public User? User { get; set; }

        [Required]
        public int ScheduleID { get; set; }
        public TransportSchedule? TransportSchedule { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        public int NumberOfSeats { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string? BookingStatus { get; set; } // e.g., 'Booked', 'Cancelled', 'Completed'

        // Navigation property for seat allocations
        public List<SeatAllocation>? SeatAllocations { get; set; }

        // Navigation property for payments
        public Payment? Payment { get; set; }

        // Navigation property for cancellations
        public Cancellation? Cancellation { get; set; }
    }

}
