using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class Cancellation
    {
        public int CancellationID { get; set; }

        [Required]
        public int BookingID { get; set; }
        public Booking? Booking { get; set; }

        [Required]
        public DateTime CancelDate { get; set; } = DateTime.Now;

        [Required]
        public decimal CancellationFee { get; set; }

        [Required]
        public decimal RefundAmount { get; set; }
    }

}
