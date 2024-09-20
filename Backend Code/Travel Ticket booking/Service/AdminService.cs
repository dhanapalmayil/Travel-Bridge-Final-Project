using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Repository;
using Travel_Ticket_booking.Interface;

namespace Travel_Ticket_booking.Service
{
    public class AdminService
    {
        private readonly IAdmin _adminRepository;

        public AdminService(IAdmin adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<Admin>> GetAllAdmins()
        {
            return await _adminRepository.GetAllAdmins();
        }

        public async Task<Admin> GetAdminById(int id)
        {
            return await _adminRepository.GetAdminById(id);
        }

        public async Task AddAdmin(Admin admin)
        {
            await _adminRepository.AddAdmin(admin);
        }

        public async Task UpdateAdmin(Admin admin)
        {
            await _adminRepository.UpdateAdmin(admin);
        }

        public async Task DeleteAdmin(int id)
        {
            var admin = await _adminRepository.GetAdminById(id);
            if (admin != null)
            {
                await _adminRepository.DeleteAdmin(admin);
            }
        }

        public async Task<bool> AdminExists(int id)
        {
            return await _adminRepository.AdminExists(id);
        }
    }
}
