using AutoMapper;

namespace MyBackend.Profiles
{
    public class SessionProfile : Profile
    {
        public SessionProfile() 
        {
            CreateMap<Entities.Session, Models.SessionModel>();
        }
    }
}
