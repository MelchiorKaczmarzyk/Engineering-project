﻿    namespace MyBackend.Models
{
    public class UserModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string Discord { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        //public IFormFile? ProfilePicture { get; set; } 
}
}
