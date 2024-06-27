using AutoMapper;

namespace MyBackend.Profiles
{
    public class GmForServiceProfile : Profile
    {
        public GmForServiceProfile() 
        {
            CreateMap<Entities.Gm, Models.GmForService>();
        }
    }
}
