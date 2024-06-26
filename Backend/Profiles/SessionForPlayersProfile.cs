using AutoMapper;

namespace MyBackend.Profiles
{
    public class SessionForPlayersProfile : Profile
    {
        public SessionForPlayersProfile() 
        {
            CreateMap<Entities.Session, Models.SessionForPlayers>();
        }
    }
}
