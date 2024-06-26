using MyBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    //There will also me SessionModelWithoutPlayers likely
    //and SessionModelWithoutGm
    public class SessionModel
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
        public int MaxNumberOfPlayers { get; set; }
        public string GmId { get; set; } = string.Empty;
        public GmModel Gm { get; set; }
        public ICollection<PlayerModel> Players { get; set; } = new List<PlayerModel>();
    }
}
