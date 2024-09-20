using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Model
{
    public class TravelTicketingDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TransportProvider> TransportProviders { get; set; }
        public DbSet<TransportMode> TransportModes { get; set; }
        public DbSet<TransportSchedule> TransportSchedules { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<SeatAllocation> SeatAllocations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdminDashboardMetrics> AdminDashboardMetrics { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public TravelTicketingDbContext(DbContextOptions<TravelTicketingDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }

}
