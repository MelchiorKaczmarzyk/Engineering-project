using AutoMapper;

namespace MyBackend.Profiles
{
    public class SessionPostProfile : Profile
    {
        public SessionPostProfile()
        {
            CreateMap<Models.SessionPost, Entities.Session>();
        }
    }
}
