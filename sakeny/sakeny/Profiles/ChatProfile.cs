using AutoMapper;

namespace sakeny.Profiles
{
    public class ChatProfile : Profile
    {
       public ChatProfile()
        {
            CreateMap<Entities.UserChatTbl, Models.ChatDtos.MessageForReturnDto>();
            CreateMap<Models.ChatDtos.MessageForCreationDto, Entities.UserChatTbl>();
        }
    }
}
