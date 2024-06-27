using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    public class PlayerForService
    {
        [Required(ErrorMessage = "name error")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        //Avoid sessions having players which has sessions which has players...
        public ICollection<SessionForPlayers> Sessions { get; set; } = new List<SessionForPlayers>();
    }
}
