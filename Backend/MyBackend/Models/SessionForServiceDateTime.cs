using MyBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    public class SessionForServiceDateTime
    {
        public GameSystemModel System { get; set; }

        [Required(ErrorMessage = "title error")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;
        public string Triggers { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public bool IsRemote { get; set; }
        //Like city or town, locale - (napisz to na froncie)
        public string Location { get; set; } = string.Empty;
        public string Vtt { get; set; } = string.Empty;

        [Required(ErrorMessage = "description error")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public required GmForSessions Gm { get; set; }
        public ICollection<PlayerForSessions> Players { get; set; } = new List<PlayerForSessions>();

        public int CurrentNumberOfPlayers => Players.Count;
        public int MaxNumberOfPlayers { get; set; }

    }
}
