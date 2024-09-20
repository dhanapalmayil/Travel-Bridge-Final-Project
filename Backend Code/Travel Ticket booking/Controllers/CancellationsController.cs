using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancellationsController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;

        public CancellationsController(TravelTicketingDbContext context)
        {
            _context = context;
        }

        // GET: api/Cancellations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cancellation>>> GetCancellations()
        {
            return await _context.Cancellations.ToListAsync();
        }

        // GET: api/Cancellations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cancellation>> GetCancellation(int id)
        {
            var cancellation = await _context.Cancellations.FindAsync(id);

            if (cancellation == null)
            {
                return NotFound();
            }

            return cancellation;
        }

        // PUT: api/Cancellations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCancellation(int id, Cancellation cancellation)
        {
            if (id != cancellation.CancellationID)
            {
                return BadRequest();
            }

            _context.Entry(cancellation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CancellationExists(id))
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

        // POST: api/Cancellations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cancellation>> PostCancellation(Cancellation cancellation)
        {
            _context.Cancellations.Add(cancellation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCancellation", new { id = cancellation.CancellationID }, cancellation);
        }

        // DELETE: api/Cancellations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCancellation(int id)
        {
            var cancellation = await _context.Cancellations.FindAsync(id);
            if (cancellation == null)
            {
                return NotFound();
            }

            _context.Cancellations.Remove(cancellation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CancellationExists(int id)
        {
            return _context.Cancellations.Any(e => e.CancellationID == id);
        }
      

     
        [HttpDelete("cancel-booking/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            // Find the booking based on the BookingID
            var booking = await _context.Bookings
                                        .Include(b => b.SeatAllocations) // Include SeatAllocations
                                        .FirstOrDefaultAsync(b => b.BookingID == bookingId);

            if (booking == null)
            {
                return NotFound("Booking not found");
            }

            // Define cancellation fee and refund logic (auto-generated)
            decimal cancellationFee = CalculateCancellationFee(booking);   // Implement your own logic here
            decimal refundAmount = booking.TotalAmount - cancellationFee;

            // Retrieve seat allocations related to the BookingID
            var seatAllocations = await _context.SeatAllocations
                                                .Where(sa => sa.BookingID == bookingId)
                                                .ToListAsync();

            // Update seat allocations: Set IsBooked = false, BookingID = 0
            foreach (var seatAllocation in seatAllocations)
            {
                seatAllocation.IsBooked = false;    // Seat is now available
                seatAllocation.BookingID = 0;       // Reset BookingID so others can book
            }
            var schedule = await _context.TransportSchedules
                                 .FirstOrDefaultAsync(s => s.ScheduleID == booking.ScheduleID);

            if (schedule != null)
            {
                // Update AvailableSeats by adding the canceled seats back
                schedule.AvailableSeats += booking.NumberOfSeats;

                // Save the updated schedule
                _context.TransportSchedules.Update(schedule);
                _context.SaveChanges();
            }
            // Remove payment details related to the BookingID
            var payment = await _context.Payments
                                        .FirstOrDefaultAsync(p => p.BookingID == bookingId);

            if (payment != null)
            {
                _context.Payments.Remove(payment);
                _context.SaveChanges();
            }

            // Auto-generate cancellation details
            var cancellation = new Cancellation
            {
                BookingID = booking.BookingID,
                CancelDate = DateTime.Now,         // Automatically set the cancellation date
                CancellationFee = cancellationFee, // Automatically calculated
                RefundAmount = refundAmount        // Automatically calculated
            };

            // Add the cancellation entry to the Cancellations table
            _context.Cancellations.Add(cancellation);

            // Remove the booking from the Bookings table
            _context.Bookings.Remove(booking);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking cancelled and related data updated", refundAmount });
        }

        private decimal CalculateCancellationFee(Booking booking)
        {
            // Example logic for calculating cancellation fee, e.g., 10% of total amount
            return booking.TotalAmount * 0.10m;
        }


    }
}
