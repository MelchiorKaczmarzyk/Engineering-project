using Microsoft.AspNetCore.Mvc;

namespace MyBackend.Models
{
    public class UserLogInResponse
    { 
            public string Email { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;

            public string Discord {  get; set; } = string.Empty;
            public FileContentResult? ProfilePicture { get; set; } = null;

            public bool PasswordIsCorrect { get; set; } = true;
            public bool EmailIsCorrect { get; set; } = true;
    }
}
