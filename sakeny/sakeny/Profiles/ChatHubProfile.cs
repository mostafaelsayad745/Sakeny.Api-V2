using AutoMapper;
using sakeny.Hubs;

namespace sakeny.Profiles
{
    public class ChatHubProfile : Profile
    {
        public ChatHubProfile()
        {
            
            CreateMap<ChatDto, Entities.UserChatTbl>().ReverseMap();
        }
    }
}
