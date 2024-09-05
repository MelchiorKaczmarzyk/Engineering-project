namespace MyBackend.Models
{
    public class ParametersLogInResponse
    {
        public bool PasswordIsCorrect { get; set; } = true;
        public bool EmailIsCorrect { get; set; } = true;
    }
}
