using AutoMapper;
using sakeny.Hubs;

namespace sakeny.Profiles
{
    public class ChatProfile : Profile
    {
       public ChatProfile()
        {
            CreateMap<Entities.UserChatTbl, Models.ChatDtos.MessageForReturnDto>();
            CreateMap<Models.ChatDtos.MessageForCreationDto, Entities.UserChatTbl>();
            CreateMap<ChatDto, Entities.UserChatTbl>().ReverseMap();
        }
    }
}
