using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Service;

namespace Travel_Ticket_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController1 : ControllerBase
    {
        
            private readonly AdminService _adminService;

            public AdminsController1(AdminService adminService)
            {
                _adminService = adminService;
            }

            // GET: api/Admins
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
            {
                var admins = await _adminService.GetAllAdmins();
                return Ok(admins);
            }

            // GET: api/Admins/5
            [HttpGet("{id}")]
            public async Task<ActionResult<Admin>> GetAdmin(int id)
            {
                var admin = await _adminService.GetAdminById(id);

                if (admin == null)
                {
                    return NotFound();
                }

                return Ok(admin);
            }

            // PUT: api/Admins/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutAdmin(int id, Admin admin)
            {
                if (id != admin.AdminID)
                {
                    return BadRequest();
                }

                await _adminService.UpdateAdmin(admin);

                return NoContent();
            }

            // POST: api/Admins
            [HttpPost]
            public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
            {
                await _adminService.AddAdmin(admin);
                return CreatedAtAction(nameof(GetAdmin), new { id = admin.AdminID }, admin);
            }

            // DELETE: api/Admins/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteAdmin(int id)
            {
                var exists = await _adminService.AdminExists(id);
                if (!exists)
                {
                    return NotFound();
                }

                await _adminService.DeleteAdmin(id);

                return NoContent();
            }
        }
    }
