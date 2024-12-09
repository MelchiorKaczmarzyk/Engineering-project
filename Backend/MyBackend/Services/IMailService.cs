using MyBackend.Models;

namespace MyBackend.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailModel mailModel);
    }
}
