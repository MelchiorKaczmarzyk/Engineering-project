using AutoMapper;

namespace MyBackend.Profiles
{
    public class GmForSessionProfile : Profile
    {
        public GmForSessionProfile()
        {
            CreateMap<Entities.Gm, Models.GmForSessions>();
        }
    }
}
