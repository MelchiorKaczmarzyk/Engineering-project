using AutoMapper;

namespace MyBackend.Profiles
{
    public class PlayerForSessionProfile : Profile
    {
        public PlayerForSessionProfile() 
        {
            CreateMap<Entities.Player, Models.PlayerForSessions>();
        }
    }
}
