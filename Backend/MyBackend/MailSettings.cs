namespace MyBackend
{
    public class MailSettings
    {
        public MailSettings() { }

        public string Host { get; set; } = "sandbox.smtp.mailtrap.io";
        public int Port { get; set; } = 587;
        public string DisplayName { get; set; } = "MyBackend";
        public string SenderAddress { get; set; } = "my@backend.com";
        public string UserName { get; set; } = "085ae38cc56942";
        public string Password { get; set; } = "cf859c9bf87463";


    }
}
