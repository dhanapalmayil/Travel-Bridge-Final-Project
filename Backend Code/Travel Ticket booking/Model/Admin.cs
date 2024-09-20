using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class Admin
    {
        public int AdminID { get; set; }

        [Required]
        public int UserID { get; set; }
        public User? User { get; set; }
    }
}
