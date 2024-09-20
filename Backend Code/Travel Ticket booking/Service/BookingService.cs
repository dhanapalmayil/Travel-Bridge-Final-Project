using static Travel_Ticket_booking.Controllers.BookingsController;
using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Repository;
using Travel_Ticket_booking.Interface;

namespace Travel_Ticket_booking.Service
{
    public class BookingService
    {
        private readonly IBooking _bookingRepository;

        public BookingService(IBooking bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _bookingRepository.GetAllBookings();
        }

        public async Task<IEnumerable<Booking>> GetBookingById(int id)
        {
            return await _bookingRepository.GetBookingById(id);
        }

        public async Task CreateBooking(BookingRequest bookingRequest)
        {
            var booking = new Booking
            {
                UserID = bookingRequest.UserID,
                ScheduleID = bookingRequest.ScheduleID,
                BookingDate = DateTime.Now,
                NumberOfSeats = bookingRequest.NumberOfSeats,
                TotalAmount = bookingRequest.TotalAmount,
                BookingStatus = bookingRequest.BookingStatus
            };

            await _bookingRepository.AddBooking(booking);

            var selectedSeats = await _bookingRepository.GetSelectedSeats(bookingRequest.SeatIds);

            foreach (var seat in selectedSeats)
            {
                seat.BookingID = booking.BookingID;
                seat.IsBooked = true;
            }

            await _bookingRepository.UpdateBooking(booking);
        }

        public async Task UpdateBooking(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                throw new Exception("Invalid booking ID");
            }

            await _bookingRepository.UpdateBooking(booking);
        }

        public async Task DeleteBooking(int id)
        {
            var booking = await _bookingRepository.GetBookingById(id);
            if (booking != null)
            {
                await _bookingRepository.DeleteBooking(booking.First());
            }
        }

        public async Task<bool> BookingExists(int id)
        {
            return await _bookingRepository.BookingExists(id);
        }
    }
}
