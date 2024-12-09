using MailKit;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MyBackend.Models
{
    public class MailModel
    {
        public string EmailTo { get; set; } = string.Empty;
        public string EmailSubcject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
    }
}
