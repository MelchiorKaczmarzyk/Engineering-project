using MyBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    //client side validation
    public class SessionPost
    {
        public GameSystemModel? System { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Picture { get; set; } = string.Empty;
        //public IFormFile? Picture { get; set; }

        public int MaxNumberOfPlayers { get; set; }

        //logged in user, will be different in the finished app
        public string GmUserName { get; set; } = string.Empty;
    }
}
