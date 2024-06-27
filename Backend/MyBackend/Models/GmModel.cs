using MyBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    //There will also me GmModelWithoutSessions likely
    public class GmModel
    {
        [Required(ErrorMessage = "name error")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<SessionModel> Sessions { get; set; } = new List<SessionModel>();
    }
}
