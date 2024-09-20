using Microsoft.EntityFrameworkCore;
using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;

namespace Travel_Ticket_booking.Repository
{
    public class BookingRepository : IBooking
    {
        private readonly TravelTicketingDbContext _context;

        public BookingRepository(TravelTicketingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingById(int id)
        {
            var result = await (from booking in _context.Bookings
                                join schedule in _context.TransportSchedules
                                on booking.ScheduleID equals schedule.ScheduleID
                                join seatAlloc in _context.SeatAllocations
                                on booking.BookingID equals seatAlloc.BookingID into seatAllocGroup
                                where booking.UserID == id
                                select new Booking
                                {
                                    BookingID = booking.BookingID,
                                    UserID = booking.UserID,
                                    ScheduleID = booking.ScheduleID,
                                    BookingDate = booking.BookingDate,
                                    NumberOfSeats = booking.NumberOfSeats,
                                    TotalAmount = booking.TotalAmount,
                                    BookingStatus = booking.BookingStatus,
                                    TransportSchedule = new TransportSchedule
                                    {
                                        ScheduleID = schedule.ScheduleID,
                                        ProviderID = schedule.ProviderID,
                                        ModeID = schedule.ModeID,
                                        TransportName = schedule.TransportName,
                                        Origin = schedule.Origin,
                                        Destination = schedule.Destination,
                                        DepartureTime = schedule.DepartureTime,
                                        ArrivalTime = schedule.ArrivalTime,
                                        SeatCapacity = schedule.SeatCapacity,
                                        Price = schedule.Price,
                                        AvailableSeats = schedule.AvailableSeats
                                    },
                                    SeatAllocations = seatAllocGroup.Select(sa => new SeatAllocation
                                    {
                                        SeatID = sa.SeatID,
                                        SeatType = sa.SeatType,
                                        SeatPrice = sa.SeatPrice,
                                        SeatNumber = sa.SeatNumber,
                                        IsBooked = sa.IsBooked,
                                        ScheduleID = sa.ScheduleID,
                                        BookingID = sa.BookingID
                                    }).ToList() // Explicitly load seat allocations
                                }).ToListAsync();

            return result;


        }

        public async Task AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBooking(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SeatAllocation>> GetSelectedSeats(List<int> seatIds)
        {
            return await _context.SeatAllocations
                                 .Where(seat => seatIds.Contains(seat.SeatID))
                                 .ToListAsync();
        }

        public async Task<bool> BookingExists(int id)
        {
            return await _context.Bookings.AnyAsync(e => e.BookingID == id);
        }
    }
}
