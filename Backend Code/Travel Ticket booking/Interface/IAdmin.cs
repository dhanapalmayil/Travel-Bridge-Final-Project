using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Interface
{
    public interface IAdmin
    {
        Task<IEnumerable<Admin>> GetAllAdmins();
        Task<Admin> GetAdminById(int id);
        Task AddAdmin(Admin admin);
        Task UpdateAdmin(Admin admin);
        Task DeleteAdmin(Admin admin);
        Task<bool> AdminExists(int id);
    }
}
