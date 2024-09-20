using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Interface
{
    public interface IBooking
    {
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<IEnumerable<Booking>> GetBookingById(int id);
        Task AddBooking(Booking booking);
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(Booking booking);
        Task<List<SeatAllocation>> GetSelectedSeats(List<int> seatIds);
        Task<bool> BookingExists(int id);

    }
}
