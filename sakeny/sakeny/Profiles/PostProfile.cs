using AutoMapper;
using sakeny.Entities;
using sakeny.Models.PicturesDtos;
using sakeny.Models.PostDtos;

namespace sakeny.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostsTbl, PostForReturnDto>().ReverseMap();
            CreateMap<PostForUpdateDto, PostsTbl>().ReverseMap();
            CreateMap<PostForCreationDto, PostsTbl>().ReverseMap();

           



            //CreateMap<PostForCreationDto, PostsTbl>().ForMember(dest => dest.PostUserId,
            //   opt => opt.MapFrom(src => src.));
        }
    }
}
