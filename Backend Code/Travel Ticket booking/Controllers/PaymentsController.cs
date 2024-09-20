using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;
using static Travel_Ticket_booking.Controllers.BookingsController;
using ZXing;
using Travel_Ticket_booking.Service;

namespace Travel_Ticket_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;
        private readonly SmtpEmailService _emailService;

        public PaymentsController(TravelTicketingDbContext context, SmtpEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        

       
    }
}
