using Microsoft.EntityFrameworkCore;
using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Repository
{
    public class AdminRepository: IAdmin
    {

        private readonly TravelTicketingDbContext _context;

        public AdminRepository(TravelTicketingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Admin>> GetAllAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admin> GetAdminById(int id)
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task AddAdmin(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdmin(Admin admin)
        {
            _context.Entry(admin).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdmin(Admin admin)
        {
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AdminExists(int id)
        {
            return await _context.Admins.AnyAsync(e => e.AdminID == id);
        }
    }
}
