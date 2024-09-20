using System;
using System.Collections.Generic;
using System.Drawing; // For Bitmap and Graphics
using System.Drawing.Imaging; // For ImageFormat
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZXing;
using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;
using iTextSharp.text; // For iTextSharp text objects
using iTextSharp.text.pdf; // For PDF generation

namespace Travel_Ticket_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly TravelTicketingDbContext _context;
        private readonly IEmailService _emailService;

        public BookingsController(TravelTicketingDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] PaymentDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create booking
            var booking = new Booking
            {
                UserID = paymentDto.UserId,
                ScheduleID = paymentDto.ScheduleId,
                BookingDate = paymentDto.BookingDate,
                NumberOfSeats = paymentDto.NumberOfSeats,
                TotalAmount = paymentDto.TotalAmount,
                BookingStatus = paymentDto.BookingStatus
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Add payment
            var payment = new Payment
            {
                BookingID = booking.BookingID,
                PaymentDate = paymentDto.PaymentDate,
                PaymentMethod = paymentDto.PaymentMethod,
                AmountPaid = paymentDto.AmountPaid,
                PaymentStatus = paymentDto.PaymentStatus
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Update seat status
            var selectedSeats = await _context.SeatAllocations
                .Where(seat => paymentDto.SeatIds.Contains(seat.SeatID))
                .ToListAsync();

            foreach (var seat in selectedSeats)
            {
                seat.BookingID = booking.BookingID;
                seat.IsBooked = true;
            }

            await _context.SaveChangesAsync();
            var user = await _context.Users
                .Where(u => u.UserID == paymentDto.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var transportSchedule = await _context.TransportSchedules
                .Where(ts => ts.ScheduleID == paymentDto.ScheduleId)
                .FirstOrDefaultAsync();
            if (transportSchedule != null)
            {
                transportSchedule.AvailableSeats -= paymentDto.NumberOfSeats;

                if (transportSchedule.AvailableSeats < 0)
                {
                    return BadRequest("Not enough available seats.");
                }

                _context.TransportSchedules.Update(transportSchedule);
                _context.SaveChanges();
            }

            if (transportSchedule == null)
            {
                return NotFound(new { message = "Transport schedule not found" });
            }

            var transportName = transportSchedule.TransportName;
            var origin = transportSchedule.Origin;
            var destination = transportSchedule.Destination;

            // Generate seat details for the email
            var seatDetails = string.Join("\n", selectedSeats.Select(seat => $"Seat Type: {seat.SeatType}, Seat Number: {seat.SeatNumber}"));

            // Prepare email content
            var emailSubject = "Booking Confirmation";
            var emailBody = $"Dear {user.UserName},\n\n" +
                $"Your booking has been confirmed.\n\n" +
                $"Transport Name: {transportName}\n" +
                $"Booking ID: {booking.BookingID}\n" +
                $"Number of Seats: {booking.NumberOfSeats}\n" +
                $"Total Amount: ₹{booking.TotalAmount}\n" +
                $"Booking Status: {booking.BookingStatus}\n" +
                $"Origin: {origin}\n" +
                $"Destination: {destination}\n\n" +
                $"Seat Details:\n{seatDetails}\n\n" +
                $"Your Email: {user.Email}\n\n" +
                $"For further details, contact: 6381855525";

            // Generate the QR code for the booking details
            var qrCodeData = $"User: {user.UserName}\n" +
                             $"BookingID: {booking.BookingID}\n" +
                             $"Transport: {transportName}\n" +
                             $"Origin: {origin}\n" +
                             $"Destination: {destination}\n" +
                             $"Seats: {seatDetails}\n" +
                             $"Total Amount: ₹{booking.TotalAmount}";

            var qrCodeImage = GenerateQRCode(qrCodeData);

            // Generate the PDF ticket
            var pdfTicket = GeneratePDFTicket(booking, user, transportName, seatDetails, origin, destination, qrCodeImage);

            // Send the email with the PDF ticket attached
            try
            {
                await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody, pdfTicket);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to send email", error = ex.Message });
            }

            return Ok(new { message = "Booking created successfully", bookingId = booking.BookingID });
        }

        [HttpPost("create1")]
        public async Task<IActionResult> CreateBooking1([FromBody] BookingRequest bookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Insert booking details in the Booking table
            var booking = new Booking
            {
                UserID = bookingRequest.UserID,
                ScheduleID = bookingRequest.ScheduleID,
                BookingDate = DateTime.Now,
                NumberOfSeats = bookingRequest.NumberOfSeats,
                TotalAmount = bookingRequest.TotalAmount,
                BookingStatus = bookingRequest.BookingStatus
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Update the SeatAllocation table for the selected seats
            var selectedSeats = await _context.SeatAllocations
                .Where(seat => bookingRequest.SeatIds.Contains(seat.SeatID))
                .ToListAsync();

            foreach (var seat in selectedSeats)
            {
                seat.BookingID = booking.BookingID;
                seat.IsBooked = true;
            }

            await _context.SaveChangesAsync();

            var user = await _context.Users
                .Where(u => u.UserID == bookingRequest.UserID)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var transportSchedule = await _context.TransportSchedules
                .Where(ts => ts.ScheduleID == bookingRequest.ScheduleID)
                .FirstOrDefaultAsync();
            if (transportSchedule != null)
            {
                transportSchedule.AvailableSeats -= bookingRequest.NumberOfSeats;

                if (transportSchedule.AvailableSeats < 0)
                {
                    return BadRequest("Not enough available seats.");
                }

                _context.TransportSchedules.Update(transportSchedule);
                await _context.SaveChangesAsync();
            }

            if (transportSchedule == null)
            {
                return NotFound(new { message = "Transport schedule not found" });
            }

            var transportName = transportSchedule.TransportName;
            var origin = transportSchedule.Origin;
            var destination = transportSchedule.Destination;

            // Generate seat details for the email
            var seatDetails = string.Join("\n", selectedSeats.Select(seat => $"Seat Type: {seat.SeatType}, Seat Number: {seat.SeatNumber}"));

            // Prepare email content
            var emailSubject = "Booking Confirmation";
            var emailBody = $"Dear {user.UserName},\n\n" +
                $"Your booking has been confirmed.\n\n" +
                $"Transport Name: {transportName}\n" +
                $"Booking ID: {booking.BookingID}\n" +
                $"Number of Seats: {booking.NumberOfSeats}\n" +
                $"Total Amount: ₹{booking.TotalAmount}\n" +
                $"Booking Status: {booking.BookingStatus}\n" +
                $"Origin: {origin}\n" +
                $"Destination: {destination}\n\n" +
                $"Seat Details:\n{seatDetails}\n\n" +
                $"Your Email: {user.Email}\n\n" +
                $"For further details, contact: 6381855525";

            // Generate the QR code for the booking details
            var qrCodeData = $"User: {user.UserName}\n" +
                             $"BookingID: {booking.BookingID}\n" +
                             $"Transport: {transportName}\n" +
                             $"Origin: {origin}\n" +
                             $"Destination: {destination}\n" +
                             $"Seats: {seatDetails}\n" +
                             $"Total Amount: ₹{booking.TotalAmount}";

            var qrCodeImage = GenerateQRCode(qrCodeData);

            // Generate the PDF ticket
            var pdfTicket = GeneratePDFTicket(booking, user, transportName, seatDetails, origin, destination, qrCodeImage);

            // Send the email with the PDF ticket attached
            try
            {
                await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody, pdfTicket);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to send email", error = ex.Message });
            }

            return Ok(new { message = "Booking created successfully", bookingId = booking.BookingID });
        }

        // Method to generate QR code
        private byte[] GenerateQRCode(string qrData)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.QrCode.QrCodeEncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    Margin = 1
                }
            };

            var pixelData = writer.Write(qrData);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    bitmap.UnlockBits(bitmapData);

                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        // Method to generate the PDF ticket
        private byte[] GeneratePDFTicket(Booking booking, User user, string transportName, string seatDetails, string origin, string destination, byte[] qrCodeImage)
        {
            using (var ms = new MemoryStream())
            {
                // Create a new PDF document
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Define font styles
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, new BaseColor(0, 0, 255));  // Blue color
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, new BaseColor(0, 0, 0));  // Black color
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, new BaseColor(0, 0, 0));       // Black color

                // Add the title
                var title = new Paragraph("Booking Confirmation\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Create a table for booking details
                var bookingTable = new PdfPTable(2) { WidthPercentage = 100 };
                bookingTable.SetWidths(new float[] { 1, 2 });

                // Add booking details with styling
                bookingTable.AddCell(new PdfPCell(new Phrase("Booking ID:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(booking.BookingID.ToString(), normalFont)) { Border = 0 });

                bookingTable.AddCell(new PdfPCell(new Phrase("User:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(user.UserName, normalFont)) { Border = 0 });

                bookingTable.AddCell(new PdfPCell(new Phrase("Transport:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(transportName, normalFont)) { Border = 0 });

                bookingTable.AddCell(new PdfPCell(new Phrase("Origin:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(origin, normalFont)) { Border = 0 });

                bookingTable.AddCell(new PdfPCell(new Phrase("Destination:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(destination, normalFont)) { Border = 0 });

                bookingTable.AddCell(new PdfPCell(new Phrase("Seats:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(seatDetails, normalFont)) { Border = 0 });

                bookingTable.AddCell(new PdfPCell(new Phrase("Total Amount:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase("₹" + booking.TotalAmount.ToString("N2"), normalFont)) { Border = 0 });

                document.Add(bookingTable);

                // Add space before QR code
                document.Add(new Paragraph("\n\n"));

                // Add the "Scan me" text above the QR code
                var scanMeText = new Paragraph("Scan me to view your booking details", headerFont);
                scanMeText.Alignment = Element.ALIGN_CENTER;
                document.Add(scanMeText);

                // Add the QR code
                var qrCodeImageInstance = iTextSharp.text.Image.GetInstance(qrCodeImage);
                qrCodeImageInstance.ScaleAbsolute(100, 100); // Resize QR code
                qrCodeImageInstance.Alignment = Element.ALIGN_CENTER;
                document.Add(qrCodeImageInstance);

                // Add a simple line separator using a table with a bottom border
                var separatorTable = new PdfPTable(1) { WidthPercentage = 100 };
                var separatorCell = new PdfPCell { BorderWidthBottom = 1f, BorderColorBottom = new BaseColor(128, 128, 128), Padding = 5f };
                separatorTable.AddCell(separatorCell);
                document.Add(separatorTable);

                // Add more spacing and the document close
                document.Add(new Paragraph("\n\nThank you for booking with us!", normalFont));
                document.Add(new Paragraph("\nFor inquiries, contact: 6381855525", normalFont));
                document.Close();

                return ms.ToArray();
            }
        }

        // DTO for the Booking Request
        public class BookingRequest
        {
            public int UserID { get; set; }
            public int ScheduleID { get; set; }
            public int NumberOfSeats { get; set; }
            public decimal TotalAmount { get; set; }
            public string BookingStatus { get; set; }
            public List<int> SeatIds { get; set; } // List of selected Seat IDs
        }

        public class PaymentDTO
        {
            public int UserId { get; set; }
            public int ScheduleId { get; set; }
            public DateTime BookingDate { get; set; }
            public int NumberOfSeats { get; set; }
            public decimal TotalAmount { get; set; }
            public string BookingStatus { get; set; }
            public DateTime PaymentDate { get; set; }
            public string PaymentMethod { get; set; }
            public decimal AmountPaid { get; set; }
            public string PaymentStatus { get; set; }
            public List<int> SeatIds { get; set; }
        }
    }
}
