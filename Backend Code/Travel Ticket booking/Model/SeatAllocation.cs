using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel_Ticket_booking.Model
{
    public class SeatAllocation
    {
        [Key]
        public int SeatID { get; set; }

        [Required]
        [StringLength(20)]
        public string? SeatType { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SeatPrice { get; set; }

        [Required]
        [StringLength(10)]
        public string? SeatNumber { get; set; }

        [Required]
        public bool IsBooked { get; set; } = false;

        [Required]
        public int ScheduleID { get; set; }  
        public TransportSchedule? TransportSchedule { get; set; }

        public int BookingID { get; set; } = 0;
    }

}
