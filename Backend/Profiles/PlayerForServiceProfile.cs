using AutoMapper;

namespace MyBackend.Profiles
{
    public class PlayerForServiceProfile : Profile
    {
        public PlayerForServiceProfile() 
        {
            CreateMap<Entities.Player, Models.PlayerForService>();
        }
    }
}
