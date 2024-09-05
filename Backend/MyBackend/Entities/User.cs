using Microsoft.AspNetCore.Identity;

namespace MyBackend.Entities
{
    public class User : IdentityUser
    {
        public string ProfilePicturePath { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string GmOrPlayerId { get; set; } = string.Empty;
        public string Discord { get; set; } = string.Empty;
    }
}
