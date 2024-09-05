namespace MyBackend.Models
{
    public class ParametersRegisterResponse
    {
        public bool EmailIsUnique { get; set; } = true;
        public bool EmailIsCorrect { get; set; } = true;
        public bool PasswordIsCorrect { get; set; } = true;
        public bool UserNameIsCorrect { get; set; } = true;
    }
}
