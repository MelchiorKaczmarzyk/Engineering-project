using MyBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    public class SessionForGms
    {
        public GameSystemModel System { get; set; }

        [Required(ErrorMessage = "title error")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;

        [Required(ErrorMessage = "description error")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int MaxNumberOfPlayers { get; set; }

        public IEnumerable<PlayerForSessions> PlayerSessions { get; set; } = new List<PlayerForSessions>();
    }
}
