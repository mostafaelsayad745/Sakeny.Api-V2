using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sakeny.Models.ChatDtos;
using sakeny.Services;
using System.Text.Json;

namespace sakeny.Controllers
{
    [Route("api/users/{userId}/chats")]
    //[Authorize]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;

        public ChatController(IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatHistory(int userId, string? name, string? SearchQuery,
                       int pageNumber = 1, int pageSize = 10)
        {
            var sender = await _userInfoRepository.GetUserAsync(userId, false);
            if (sender == null)
            {
                return NotFound();
            }
            var (receiver, pagenationMetadata) = await _userInfoRepository.GetUsersAsync(name, SearchQuery, pageNumber, pageSize);
            var receiverName = receiver.Where(r => r.UserName == SearchQuery).FirstOrDefault();

            Response.Headers.Add("X-Pagination",
                               JsonSerializer.Serialize(pagenationMetadata));
           var messages = await _userInfoRepository.GetChatHistoryAsync(sender.UserName, receiverName.UserName);
            //var messagesToReturn = _mapper.Map<IEnumerable<MessageForReturnDto>>(messages);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int userId, MessageForCreationDto messageForCreationDto,
            string? name, string? SearchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            var sender = await _userInfoRepository.GetUserAsync(userId, false);
            if (sender == null)
            {
                return NotFound();
            }
            var (receiver, pagenationMetadata) = await _userInfoRepository.GetUsersAsync(name, SearchQuery, pageNumber, pageSize);

            var receiverName = receiver.Where(r => r.UserName == SearchQuery).FirstOrDefault();
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagenationMetadata));

            if (receiver == null)
            {
                return NotFound();
            }


            await _userInfoRepository.SendMessageAsync(userId, receiverName.UserId, messageForCreationDto.Message);
            await _userInfoRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
