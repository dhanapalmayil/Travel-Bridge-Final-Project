using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class Payment
    {
        public int PaymentID { get; set; }

        [Required]
        public int BookingID { get; set; }
        public Booking? Booking { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string? PaymentMethod { get; set; } // e.g., 'Credit Card', 'Debit Card', 'Net Banking', 'UPI', 'Wallet'

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        [StringLength(50)]
        public string? PaymentStatus { get; set; } // e.g., 'Success', 'Failed', 'Pending'
    }

}
