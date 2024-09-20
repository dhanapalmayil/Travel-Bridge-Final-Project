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
    public class TransportModesController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;

        public TransportModesController(TravelTicketingDbContext context)
        {
            _context = context;
        }

        // GET: api/TransportModes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransportMode>>> GetTransportModes()
        {
            return await _context.TransportModes.ToListAsync();
        }

        // GET: api/TransportModes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransportMode>> GetTransportMode(int id)
        {
            var transportMode = await _context.TransportModes.FindAsync(id);

            if (transportMode == null)
            {
                return NotFound();
            }

            return transportMode;
        }

        // PUT: api/TransportModes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportMode(int id, TransportMode transportMode)
        {
            if (id != transportMode.ModeID)
            {
                return BadRequest();
            }

            _context.Entry(transportMode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportModeExists(id))
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

        // POST: api/TransportModes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransportMode>> PostTransportMode(TransportMode transportMode)
        {
            _context.TransportModes.Add(transportMode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransportMode", new { id = transportMode.ModeID }, transportMode);
        }

        // DELETE: api/TransportModes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportMode(int id)
        {
            var transportMode = await _context.TransportModes.FindAsync(id);
            if (transportMode == null)
            {
                return NotFound();
            }

            _context.TransportModes.Remove(transportMode);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransportModeExists(int id)
        {
            return _context.TransportModes.Any(e => e.ModeID == id);
        }
    }
}
