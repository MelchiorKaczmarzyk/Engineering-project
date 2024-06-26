using AutoMapper;

namespace MyBackend.Profiles
{
    public class SessionForGmsProfile : Profile
    {
        public SessionForGmsProfile() 
        {
            CreateMap<Entities.Session, Models.SessionForGms>();
        }
    }
}
