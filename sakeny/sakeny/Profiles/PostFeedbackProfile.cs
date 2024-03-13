using AutoMapper;

namespace sakeny.Profiles
{
    public class PostFeedbackProfile : Profile
    {
        public PostFeedbackProfile()
        {
            CreateMap<Entities.PostFeedbackTbl, Models.PostFeedbackForReturnDto>();
            CreateMap<Models.PostFeedbackForCreationDto, Entities.PostFeedbackTbl>();
            CreateMap<Models.PostFeedbackForUpdateDto, Entities.PostFeedbackTbl>();
            CreateMap<Entities.PostFeedbackTbl, Models.PostFeedbackForUpdateDto>();
            CreateMap<Entities.PostFeedbackTbl, Models.UserForReturnWithPostFeedbacks>();
        }
    }
}
