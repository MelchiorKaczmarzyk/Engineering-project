using MyBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    //There will also me SessionModelWithoutPlayers likely
    //and SessionModelWithoutGm
    public class SessionModel
    {
        public GameSystemModel System { get; set; }

        [Required(ErrorMessage = "title error")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;
        public string Triggers { get; set; } = string.Empty;

        public DateTime? Date { get; set; }
        public bool IsRemote { get; set; }
        //Like city or town, locale - (napisz to na froncie)
        public string Location { get; set; } = string.Empty;
        public string Vtt { get; set; } = string.Empty;

        [Required(ErrorMessage = "description error")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int MaxNumberOfPlayers { get; set; }
        public GmModel Gm { get; set; }
        public ICollection<PlayerModel> Players { get; set; } = new List<PlayerModel>();
    }
}
