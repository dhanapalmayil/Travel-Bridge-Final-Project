namespace Travel_Ticket_booking.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, byte[] attachment = null);
    }

}
