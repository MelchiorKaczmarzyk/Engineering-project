using AutoMapper;

namespace MyBackend.Profiles
{
    public class TagForServiceProfile : Profile
    {
        public TagForServiceProfile()
        {
            CreateMap<Entities.Tag, Models.TagModel>();
        }
    }
}
