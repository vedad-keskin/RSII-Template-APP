using System.Threading.Tasks;

namespace ManiFest.Subscriber.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
