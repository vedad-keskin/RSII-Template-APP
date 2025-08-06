using System.Threading.Tasks;

namespace CallTaxi.Subscriber.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
