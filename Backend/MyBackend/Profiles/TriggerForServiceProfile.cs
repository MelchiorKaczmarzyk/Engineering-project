using AutoMapper;

namespace MyBackend.Profiles
{
    public class TriggerForServiceProfile : Profile
    {
        public TriggerForServiceProfile()
        {
            CreateMap<Entities.Trigger, Models.TriggerModel>();
        }
    }
}
