using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackend.Entities
{
    public class Session
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required(ErrorMessage = "system error")]
        [MaxLength(50)]
        public string System { get; set; } = string.Empty;

        [Required(ErrorMessage = "title error")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        //List of strings? Chyba nie, bo nie ma żadnej listy ustalonych
        public string Tags { get; set; } = string.Empty;

        [Required(ErrorMessage = "description error")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int MaxNumberOfPlayers { get; set; }
        public string GmId { get; set; } = string.Empty;
        public Gm Gm { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();

    }
}
