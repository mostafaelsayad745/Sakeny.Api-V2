using AutoMapper;
using sakeny.Entities;
using sakeny.Models.PicturesDtos;
using sakeny.Models.PostDtos;
using sakeny.Services.NewPostRepo;

namespace sakeny.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostsTbl, PostForReturnDto>().ReverseMap();
            CreateMap<PostForUpdateDto, PostsTbl>().ReverseMap();
            CreateMap<PostForCreationDto, PostsTbl>().ReverseMap();
            CreateMap<PostsTbl, PostDto>();




            //CreateMap<PostForCreationDto, PostsTbl>().ForMember(dest => dest.PostUserId,
            //   opt => opt.MapFrom(src => src.));
        }
    }
}
