using System.Net;
using System.Net.Mail;
using ManiFest.Subscriber.Interfaces;

namespace ManiFest.Subscriber.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly string _gmailMail = "calltaxi.sender@gmail.com";
        private readonly string _gmailPass = "tlkv xshf zbaw oaab";

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_gmailMail, _gmailPass)
            };

            return client.SendMailAsync(
                new MailMessage(from: _gmailMail,
                              to: email,
                              subject,
                              message
                              ));
        }
    }
}