using Microsoft.Extensions.Options;
using MyBackend.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MyBackend.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService() 
        {
            _mailSettings = new MailSettings();
        }
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMailAsync(MailModel mailModel)
        {
            MailMessage mailMessage = new MailMessage
            {
                Subject = mailModel.EmailSubcject,
                Body = mailModel.EmailBody,
                From = new MailAddress(_mailSettings.SenderAddress, _mailSettings.DisplayName),
                IsBodyHtml = true
            };
            mailMessage.To.Add(mailModel.EmailTo);

            NetworkCredential networkCredential = new NetworkCredential(_mailSettings.UserName, 
                _mailSettings.Password);

            SmtpClient smtpClient = new SmtpClient 
            {
                Host = _mailSettings.Host,
                Port = _mailSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = networkCredential,
            };
            mailMessage.BodyEncoding = Encoding.Default;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {
                Console.WriteLine("lol");
            }


        }
    }
}
