using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    public class GmForService
    {
        [Required(ErrorMessage = "name error")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<SessionForGms> Sessions { get; set; } = new List<SessionForGms>();
    }
}
