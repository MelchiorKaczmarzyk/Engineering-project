using AutoMapper;

namespace MyBackend.Profiles
{
    public class GameSystemProfile : Profile
    {
        public GameSystemProfile()
        {
            CreateMap<Entities.GameSystem, Models.GameSystemModel>();
        }
    }
}
