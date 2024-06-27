using MyBackend.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBackend.Models
{
    //There will also me PlayerModelWithoutSessions likely
    public class PlayerModel
    {
        [Required(ErrorMessage = "name error")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        //Avoid sessions having players which has sessions which has players...
        public ICollection<SessionModel> Sessions { get; set; } = new List<SessionModel>();
    }
}
