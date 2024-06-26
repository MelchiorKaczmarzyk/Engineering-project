using AutoMapper;

namespace MyBackend.Profiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile() 
        {
            CreateMap<Entities.Player, Models.PlayerModel>();
        }
    }
}
