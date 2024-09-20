using System.Net.Mail;
using System.Net;
using Travel_Ticket_booking.Interface;
using System.IO;
using System.Threading.Tasks;

namespace Travel_Ticket_booking.Service
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "travelbridge7@gmail.com";
        private readonly string _smtpPass = "nsca bjzp rriz ijuh";

        public async Task SendEmailAsync(string to, string subject, string body, byte[] attachment = null)
        {
            var smtpClient = new SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = false, // You can set this to true if your email body contains HTML
            };

            mailMessage.To.Add(to);

            // If there is an attachment, add it to the email
            if (attachment != null)
            {
                using (var memoryStream = new MemoryStream(attachment))
                {
                    // Create attachment with appropriate content type
                    var pdfAttachment = new Attachment(memoryStream, "Ticket.pdf", "application/pdf");
                    mailMessage.Attachments.Add(pdfAttachment);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            else
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
