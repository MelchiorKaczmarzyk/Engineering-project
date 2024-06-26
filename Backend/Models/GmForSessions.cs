using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    public class GmForSessions
    {
        [Required(ErrorMessage = "name error")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
