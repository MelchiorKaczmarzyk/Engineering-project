using AutoMapper;

namespace MyBackend.Profiles
{
    public class VttForServiceProfile : Profile
    {
        public VttForServiceProfile()
        {
            CreateMap<Entities.Vtt, Models.VttModel>();
        }
    }
}
