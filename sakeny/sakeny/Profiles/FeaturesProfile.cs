using AutoMapper;
using sakeny.Entities;
using sakeny.Models.FeaturesDtos;

namespace sakeny.Profiles
{
    public class FeaturesProfile : Profile
    {
        public FeaturesProfile()
        {
            CreateMap<FeaturesTbl, FeatureForReturnDto>().ReverseMap();
            CreateMap<FeatureForCreationDto, FeaturesTbl>().ReverseMap();
            CreateMap<FeatureForUpdateDto, FeaturesTbl>().ReverseMap();
        }
    }
}
