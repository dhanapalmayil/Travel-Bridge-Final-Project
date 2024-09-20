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
    public class TransportProvidersController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;

        public TransportProvidersController(TravelTicketingDbContext context)
        {
            _context = context;
        }

        // GET: api/TransportProviders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransportProvider>>> GetTransportProviders()
        {
            return await _context.TransportProviders.ToListAsync();
        }

        // GET: api/TransportProviders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransportProvider>> GetTransportProvider(int id)
        {
            var transportProvider = await _context.TransportProviders.FindAsync(id);

            if (transportProvider == null)
            {
                return NotFound();
            }

            return transportProvider;
        }

        // PUT: api/TransportProviders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportProvider(int id, TransportProvider transportProvider)
        {
            if (id != transportProvider.ProviderID)
            {
                return BadRequest();
            }

            _context.Entry(transportProvider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }

        // POST: api/TransportProviders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransportProvider>> PostTransportProvider(TransportProvider transportProvider)
        {
            _context.TransportProviders.Add(transportProvider);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransportProvider", new { id = transportProvider.ProviderID }, transportProvider);
        }

        // DELETE: api/TransportProviders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportProvider(int id)
        {
            var transportProvider = await _context.TransportProviders.FindAsync(id);
            if (transportProvider == null)
            {
                return NotFound();
            }

            _context.TransportProviders.Remove(transportProvider);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/TransportProviders/Exists/5
        [HttpGet("Exists/{id}")]
        public async Task<IActionResult> TransportProviderExists(int id)
        {
            var transportProvider = _context.TransportProviders
                                            .FirstOrDefault(e => e.UserID == id);

            if (transportProvider != null)
            {
                return Ok(transportProvider);  // Return the transport provider data if found
            }
            else
            {
                return NotFound(new { message = "Transport provider not found." });
            }
        }
    }
}
