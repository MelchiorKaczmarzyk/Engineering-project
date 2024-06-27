using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    public class SessionForService
    {
        [Required(ErrorMessage = "system error")]
        [MaxLength(50)]
        public string System { get; set; } = string.Empty;

        [Required(ErrorMessage = "title error")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;

        [Required(ErrorMessage = "description error")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public required GmForSessions Gm { get; set; }
        public ICollection<PlayerForSessions> Players { get; set; } = new List<PlayerForSessions>();

        public int CurrentNumberOfPlayers => Players.Count;
        public int MaxNumberOfPlayers { get; set; }

    }
}
