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
    public class AdminDashboardMetricsController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;

        public AdminDashboardMetricsController(TravelTicketingDbContext context)
        {
            _context = context;
        }

        // GET: api/AdminDashboardMetrics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDashboardMetrics>>> GetAdminDashboardMetrics()
        {
            return await _context.AdminDashboardMetrics.ToListAsync();
        }

        // GET: api/AdminDashboardMetrics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDashboardMetrics>> GetAdminDashboardMetrics(int id)
        {
            var adminDashboardMetrics = await _context.AdminDashboardMetrics.FindAsync(id);

            if (adminDashboardMetrics == null)
            {
                return NotFound();
            }

            return adminDashboardMetrics;
        }

        // PUT: api/AdminDashboardMetrics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminDashboardMetrics(int id, AdminDashboardMetrics adminDashboardMetrics)
        {
            if (id != adminDashboardMetrics.MetricID)
            {
                return BadRequest();
            }

            _context.Entry(adminDashboardMetrics).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminDashboardMetricsExists(id))
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

        // POST: api/AdminDashboardMetrics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminDashboardMetrics>> PostAdminDashboardMetrics(AdminDashboardMetrics adminDashboardMetrics)
        {
            _context.AdminDashboardMetrics.Add(adminDashboardMetrics);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdminDashboardMetrics", new { id = adminDashboardMetrics.MetricID }, adminDashboardMetrics);
        }

        // DELETE: api/AdminDashboardMetrics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminDashboardMetrics(int id)
        {
            var adminDashboardMetrics = await _context.AdminDashboardMetrics.FindAsync(id);
            if (adminDashboardMetrics == null)
            {
                return NotFound();
            }

            _context.AdminDashboardMetrics.Remove(adminDashboardMetrics);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminDashboardMetricsExists(int id)
        {
            return _context.AdminDashboardMetrics.Any(e => e.MetricID == id);
        }
    }
}
