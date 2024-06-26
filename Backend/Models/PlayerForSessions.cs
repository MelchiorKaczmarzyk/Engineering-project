using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    //Does not have a list of sessions
    public class PlayerForSessions
    {
        [Required(ErrorMessage = "name error")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
