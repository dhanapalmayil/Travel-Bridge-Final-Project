using System.ComponentModel.DataAnnotations;

namespace Travel_Ticket_booking.Model
{
    public class AuditLog
    {
        public int AuditLogID { get; set; }

        [Required]
        public int AdminID { get; set; }
        public Admin? Admin { get; set; }

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string? ActionType { get; set; } // e.g., 'Update', 'Delete', 'Create'

        [Required]
        [StringLength(1000)]
        public string? Details { get; set; } // Description of the action
    }

}
