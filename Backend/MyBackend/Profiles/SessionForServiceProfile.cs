using AutoMapper;

namespace MyBackend.Profiles
{
    public class SessionForServiceProfile : Profile
    {
        public SessionForServiceProfile()
        {
            CreateMap<Entities.Session, Models.SessionForServiceDateTime>();
        }
    }
}
