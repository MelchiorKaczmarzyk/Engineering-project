using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    public class SessionForPlayers
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

        //Hmm...
        //public string GmId { get; set; } = string.Empty;
        //public GmModel Gm { get; set; }
    }
}
