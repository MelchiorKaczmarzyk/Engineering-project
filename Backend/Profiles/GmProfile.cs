using AutoMapper;

namespace MyBackend.Profiles
{
    public class GmProfile : Profile
    {
        public GmProfile() 
        {
            CreateMap<Entities.Gm, Models.GmModel>();
        }
    }
}
