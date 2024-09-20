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
    public class SeatAllocationsController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;

        public SeatAllocationsController(TravelTicketingDbContext context)
        {
            _context = context;
        }

        // GET: api/SeatAllocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatAllocation>>> GetSeatAllocations()
        {
            return await _context.SeatAllocations.ToListAsync();
        }

        // GET: api/SeatAllocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeatAllocation>> GetSeatAllocation(int id)
        {
            var seatAllocation = await _context.SeatAllocations.FindAsync(id);

            if (seatAllocation == null)
            {
                return NotFound();
            }

            return seatAllocation;
        }

        // PUT: api/SeatAllocations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeatAllocation(int id, SeatAllocation seatAllocation)
        {
            if (id != seatAllocation.SeatID)
            {
                return BadRequest();
            }

            _context.Entry(seatAllocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeatAllocationExists(id))
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

        // POST: api/SeatAllocations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeatAllocation>> PostSeatAllocation(SeatAllocation seatAllocation)
        {

            _context.SeatAllocations.Add(seatAllocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeatAllocation", new { id = seatAllocation.SeatID }, seatAllocation);
        }

        // DELETE: api/SeatAllocations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeatAllocation(int id)
        {
            var seatAllocation = await _context.SeatAllocations.FindAsync(id);
            if (seatAllocation == null)
            {
                return NotFound();
            }

            _context.SeatAllocations.Remove(seatAllocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeatAllocationExists(int id)
        {
            return _context.SeatAllocations.Any(e => e.SeatID == id);
        }
        [HttpGet("schedule/{scheduleId}")]
        public async Task<IActionResult> GetSeats(int scheduleId)
        {
            var seats = await _context.SeatAllocations
                .Where(sa => sa.ScheduleID == scheduleId)
                .GroupBy(sa => sa.SeatType)
                .Select(g => new
                {
                    type = g.Key,
                    price = g.FirstOrDefault().SeatPrice, // Assuming all seats of a type have the same price
                    seats = g.Select(sa => new
                    {
                        id = sa.SeatID,
                        number = sa.SeatNumber,
                        isBooked = sa.IsBooked

                    }).ToList()
                })
                .ToListAsync();

            return Ok(seats);
        }
    }
}
