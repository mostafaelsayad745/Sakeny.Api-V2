using AutoMapper;
using sakeny.Models;

namespace sakeny.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.UsersTbl, Models.UserForReturnDto>();
           // CreateMap<Entities.UsersTbl, Models.UserForReturnWithPostFeedbacks>();
            CreateMap<Models.UserForCreationDto, Entities.UsersTbl>();
            CreateMap<Models.UserForUpdateDto, Entities.UsersTbl>();
            CreateMap<Entities.UsersTbl, Models.UserForUpdateDto>();
            CreateMap<Entities.UsersTbl, UserForReturnWithPostFeedbacks>()
            .ForMember(dest => dest.PostFeedbacks, opt => opt.MapFrom(src => src.PostFeedbackTbls));
        
    }
    }
}
