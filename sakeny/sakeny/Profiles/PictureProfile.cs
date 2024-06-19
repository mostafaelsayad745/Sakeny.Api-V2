using AutoMapper;
using sakeny.Entities;
using sakeny.Models.PicturesDtos;
using sakeny.Models.PostDtos;

namespace sakeny.Profiles
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<PostPicTbl, PicturesForReturnDto>();
            CreateMap<PostsTbl, PostForReturnDto>()
             .ForMember(dest => dest.PostPics, opt => opt.MapFrom(src => src.PostPicTbls));
        }
    }
}
