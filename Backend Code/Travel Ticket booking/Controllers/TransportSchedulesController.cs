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
    public class TransportSchedulesController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;

        public TransportSchedulesController(TravelTicketingDbContext context)
        {
            _context = context;
        }

        // GET: api/TransportSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransportSchedule>>> GetTransportSchedules()
        {
            return await _context.TransportSchedules.ToListAsync();
        }

        // GET: api/TransportSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransportSchedule>> GetTransportSchedule(int id)
        {
            var transportSchedule = await _context.TransportSchedules.FindAsync(id);

            if (transportSchedule == null)
            {
                return NotFound();
            }

            return transportSchedule;
        }

        // PUT: api/TransportSchedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportSchedule(int id, TransportSchedule transportSchedule)
        {
            if (id != transportSchedule.ScheduleID)
            {
                return BadRequest();
            }

            _context.Entry(transportSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportScheduleExists(id))
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

        // POST: api/TransportSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransportSchedule>> PostTransportSchedule(TransportSchedule transportSchedule)
        {
            _context.TransportSchedules.Add(transportSchedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransportSchedule", new { id = transportSchedule.ScheduleID }, transportSchedule);
        }

        // DELETE: api/TransportSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportSchedule(int id)
        {
            var transportSchedule = await _context.TransportSchedules.FindAsync(id);
            if (transportSchedule == null)
            {
                return NotFound();
            }

            _context.TransportSchedules.Remove(transportSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransportScheduleExists(int id)
        {
            return _context.TransportSchedules.Any(e => e.ScheduleID == id);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TransportSchedule>>> SearchTransportSchedules(
           string from, string to, DateTime date, string mode)
        {
            // Map mode string to corresponding ModeID
            int modeID = mode.ToLower() switch
            {
                "bus" => 1,
                "train" => 2,
                "flight" => 3,
                _ => 0 // Default to 0 or handle invalid modes as needed
            };

            // If invalid mode is provided, return a BadRequest
            if (modeID == 0)
            {
                return BadRequest("Invalid mode. Please provide 'Bus', 'Train', or 'Flight'.");
            }

            // Filter schedules by origin, destination, date, and mode
            var schedules = await _context.TransportSchedules
                .Where(s => s.Origin == from
                            && s.Destination == to
                            && s.DepartureTime.Date == date.Date
                            && s.ModeID == modeID)
                .ToListAsync();

            return schedules.Any() ? Ok(schedules) : NotFound("No schedules found for the given criteria.");
        }

        [HttpPost("createschedule")]
        public async Task CreateTransportSchedule(CreateTransportScheduleDTO scheduleDto)
        {
            var transportSchedule = new TransportSchedule
            {
                ProviderID = scheduleDto.ProviderID,
                ModeID = scheduleDto.ModeID,
                TransportName = scheduleDto.TransportName,
                Origin = scheduleDto.Origin,
                Destination = scheduleDto.Destination,
                DepartureTime = scheduleDto.DepartureTime,
                ArrivalTime = scheduleDto.ArrivalTime,
                SeatCapacity = scheduleDto.SeatCapacity,
                Price = scheduleDto.Price,
                AvailableSeats = scheduleDto.SeatCapacity // Initially all seats are available
            };

            _context.TransportSchedules.Add(transportSchedule);
            await _context.SaveChangesAsync();

            var seats = new List<SeatAllocation>();

            foreach (var ticketTypeDto in scheduleDto.TicketTypes)
            {
                // Create seats based on the number of seats for each ticket type
                for (int i = 1; i <= ticketTypeDto.NumberOfSeats; i++)
                {
                    var seat = new SeatAllocation
                    {
                        
                        SeatNumber = $"{GetShortForm(ticketTypeDto.TicketTypeName)}-{i}", // Generating unique seat number
                        SeatType = ticketTypeDto.TicketTypeName,
                        SeatPrice = ticketTypeDto.Price,
                        ScheduleID = transportSchedule.ScheduleID,
                        IsBooked = false // Initial value
                    };

                    seats.Add(seat);
                }
            }

            _context.SeatAllocations.AddRange(seats);
            await _context.SaveChangesAsync();
        }
        public class CreateTransportScheduleDTO
        {
            public int ProviderID { get; set; }
            public int ModeID { get; set; }
            public string TransportName { get; set; } = string.Empty;
            public string Origin { get; set; } = string.Empty;
            public string Destination { get; set; } = string.Empty;
            public DateTime DepartureTime { get; set; }
            public DateTime ArrivalTime { get; set; }
            public int SeatCapacity { get; set; }
            public decimal Price { get; set; }
            public List<TicketTypeDTO> TicketTypes { get; set; } = new List<TicketTypeDTO>();
        }

        public class TicketTypeDTO
        {
            public string TicketTypeName { get; set; } = string.Empty;
            public int NumberOfSeats { get; set; }
            public decimal Price { get; set; }
        }
        private string GetShortForm(string ticketTypeName)
        {
            var words = ticketTypeName.Split(' ');

            // Handle single-word names and multi-word names
            if (words.Length == 1)
            {
                return words[0].Substring(0, 2).ToUpper(); // Take first two letters of the single word
            }
            else
            {
                // Combine first letter of the first word and first letter of the second word
                return (words[0][0].ToString() + words[1][0].ToString()).ToUpper();
            }
        }
        // GET: api/TransportSchedules/GetGroupedDetailsByProvider/{providerId}
        [HttpGet("GetGroupedDetailsByProvider/{providerId}")]
        public async Task<IActionResult> GetGroupedDetailsByProvider(int providerId)
        {
            var result = await (from schedule in _context.TransportSchedules
                                join booking in _context.Bookings
                                on schedule.ScheduleID equals booking.ScheduleID
                                join seatAllocation in _context.SeatAllocations
                                on booking.BookingID equals seatAllocation.BookingID
                                join user in _context.Users
                                on booking.UserID equals user.UserID
                                where schedule.ProviderID == providerId
                                group new { booking, seatAllocation, user } by new
                                {
                                    schedule.ScheduleID,
                                    schedule.TransportName,
                                    schedule.Origin,
                                    schedule.Destination,
                                    schedule.DepartureTime,
                                    schedule.ArrivalTime,
                                    schedule.AvailableSeats,
                                    schedule.SeatCapacity
                                } into scheduleGroup
                                select new
                                {
                                    ScheduleID = scheduleGroup.Key.ScheduleID,
                                    TransportName = scheduleGroup.Key.TransportName,
                                    Origin = scheduleGroup.Key.Origin,
                                    Destination = scheduleGroup.Key.Destination,
                                    DepartureTime = scheduleGroup.Key.DepartureTime,
                                    ArrivalTime = scheduleGroup.Key.ArrivalTime,
                                    TotalSeats = scheduleGroup.Key.SeatCapacity - scheduleGroup.Key.AvailableSeats,

                                    // Bookings grouped by BookingID and UserID
                                    Bookings = scheduleGroup.GroupBy(g => new
                                    {
                                        g.booking.BookingID,
                                        g.booking.BookingStatus,
                                        g.user.UserID,
                                        g.user.UserName,
                                        g.user.Email,
                                        g.booking.NumberOfSeats,
                                        g.booking.TotalAmount
                                    })
                                    .Select(bookingGroup => new
                                    {
                                        BookingID = bookingGroup.Key.BookingID,
                                        BookingStatus = bookingGroup.Key.BookingStatus,
                                        UserID = bookingGroup.Key.UserID,
                                        UserName = bookingGroup.Key.UserName,
                                        Email = bookingGroup.Key.Email,
                                        NumberOfSeats = bookingGroup.Key.NumberOfSeats,
                                        TotalAmount = bookingGroup.Key.TotalAmount,
                                        SeatDetails = bookingGroup.Select(b => new
                                        {
                                            SeatNumber = b.seatAllocation.SeatNumber,
                                            SeatType = b.seatAllocation.SeatType,
                                            SeatPrice = b.seatAllocation.SeatPrice
                                        }).ToList()
                                    }).ToList()
                                }).ToListAsync();

            // Handle the case where no data is found
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No data found for the given provider ID." });
            }

            return Ok(result);
        }


    }
}
