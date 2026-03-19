using System.Threading.Tasks;

namespace Pa301Fiorelle.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
