using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string? UserRole { get; set; } // e.g., 'Customer', 'Provider', 'Admin'

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; }

        // Navigation property for bookings made by the user
        public ICollection<Booking>? Bookings { get; set; }
    }

}
