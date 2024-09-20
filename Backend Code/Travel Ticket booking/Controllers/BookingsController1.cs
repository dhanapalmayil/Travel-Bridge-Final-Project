using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Travel_Ticket_booking.Controllers.BookingsController;
using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Service;

namespace Travel_Ticket_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController1 : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingsController1(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            var bookings = await _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking(int id)
        {
            var booking = await _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            try
            {
                await _bookingService.UpdateBooking(id, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequest bookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookingService.CreateBooking(bookingRequest);
            return Ok(new { message = "Booking created successfully" });
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var exists = await _bookingService.BookingExists(id);
            if (!exists)
            {
                return NotFound();
            }

            await _bookingService.DeleteBooking(id);
            return NoContent();
        }
    }
}

