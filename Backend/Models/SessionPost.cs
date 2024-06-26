using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    //client side validation
    public class SessionPost
    {
        public string System { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int MaxNumberOfPlayers { get; set; }

        //logged in user, will be different in the finished app
        public GmForSessions Gm { get; set; }
    }
}
