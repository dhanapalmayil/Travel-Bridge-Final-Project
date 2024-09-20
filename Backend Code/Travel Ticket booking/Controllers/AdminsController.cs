using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;

        public AdminsController(TravelTicketingDbContext context)
        {
            _context = context;
        }



        // 1. USER MANAGEMENT

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/admin/users/{userId}
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
        {
            // Validate and update user details excluding PasswordHash
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.UserName = updateDto.Username;
            user.Email = updateDto.Email;
            user.UserRole = updateDto.UserRole;

            _context.SaveChanges();

            return Ok();
        }
        
        // DELETE: api/admin/users/{userId}
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 2. PROVIDER MANAGEMENT

        // GET: api/admin/providers
        [HttpGet("providers")]
        public async Task<ActionResult<IEnumerable<TransportProvider>>> GetProviders()
        {
            return Ok(await _context.TransportProviders.ToListAsync());
        }

        // GET: api/admin/providers/{providerId}
        [HttpGet("providers/{providerId}")]
        public async Task<ActionResult<TransportProvider>> GetProvider(int providerId)
        {
            var provider = await _context.TransportProviders.FindAsync(providerId);
            if (provider == null)
            {
                return NotFound();
            }
            return Ok(provider);
        }

        [HttpPut("providers/update/{id}")]
        public async Task<IActionResult> UpdateProvider(int id, [FromBody] ProviderUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var provider = await _context.TransportProviders.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }

            provider.ProviderName = updateDto.ProviderName;
            provider.ContactEmail = updateDto.ContactEmail;
            provider.ContactPhone = updateDto.ContactPhone;
            provider.ProviderType = updateDto.ProviderType;
            provider.TransportName = updateDto.TransportName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok();
        }

        // DELETE: api/admin/providers/{providerId}
        [HttpDelete("providers/{providerId}")]
        public async Task<IActionResult> DeleteProvider(int providerId)
        {
            var provider = await _context.TransportProviders.FindAsync(providerId);
            if (provider == null)
            {
                return NotFound();
            }

            _context.TransportProviders.Remove(provider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 3. BOOKING MANAGEMENT

        // GET: api/admin/bookings
        [HttpGet("bookings")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return Ok(await _context.Bookings.ToListAsync());
        }

        // GET: api/admin/bookings/{bookingId}
        [HttpGet("bookings/{bookingId}")]
        public async Task<ActionResult<Booking>> GetBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        // PUT: api/admin/bookings/{bookingId}
        [HttpPut("bookings/{bookingId}")]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, Booking booking)
        {
            if (bookingId != booking.BookingID)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Bookings.Any(b => b.BookingID == bookingId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // 4. AUDIT LOGS AND SYSTEM MONITORING

        // GET: api/admin/auditlogs
        [HttpGet("auditlogs")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs()
        {
            return Ok(await _context.AuditLogs.ToListAsync());
        }

        // GET: api/admin/auditlogs/{auditLogId}
        [HttpGet("auditlogs/{auditLogId}")]
        public async Task<ActionResult<AuditLog>> GetAuditLog(int auditLogId)
        {
            var auditLog = await _context.AuditLogs.FindAsync(auditLogId);
            if (auditLog == null)
            {
                return NotFound();
            }
            return Ok(auditLog);
        }

        // 5. REPORT GENERATION

        // GET: api/admin/reports/bookings
        [HttpGet("reports/bookings")]
        public async Task<ActionResult<Report>> GetBookingReport(DateTime startDate, DateTime endDate)
        {
            var reportContent = await _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .ToListAsync();

            var report = new Report
            {
                ReportType = "Booking",
                ReportDate = DateTime.Now,
                ReportContent = JsonConvert.SerializeObject(reportContent)
            };

            return Ok(report);
        }

        // GET: api/admin/reports/revenue
        [HttpGet("reports/revenue")]
        public async Task<ActionResult<Report>> GetRevenueReport(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .SumAsync(p => p.AmountPaid);

            var report = new Report
            {
                ReportType = "Revenue",
                ReportDate = DateTime.Now,
                ReportContent = $"Total revenue from {startDate} to {endDate} is {totalRevenue:C}."
            };

            return Ok(report);
        }

       

        // GET: api/admin/dashboard/metrics
        [HttpGet("dashboard/metrics")]
        public async Task<ActionResult<AdminDashboardMetrics>> GetDashboardMetrics()
        {
            var metrics = new AdminDashboardMetrics
            {
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TotalProviders = await _context.TransportProviders.CountAsync(),
                TotalSales = await _context.Payments.SumAsync(p => p.AmountPaid)
            };

            return Ok(metrics);
        }

    }
    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
    }
    public class ProviderUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string ProviderName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string ContactEmail { get; set; }

        [Phone]
        [StringLength(15)]
        public string ContactPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string ProviderType { get; set; }

        [Required]
        [StringLength(100)]
        public string TransportName { get; set; }
    }

}