using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sakeny.Entities;
using sakeny.Models;
using sakeny.Models.ChatDtos;
using sakeny.Models.UserFeedbackDtos;
using sakeny.Services;
using System.Security.Claims;
using System.Text.Json;

namespace sakeny.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class UserFeedbackController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;

        public UserFeedbackController(IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
        }

        [HttpGet("userfeedbacks")]
        public async Task<IActionResult> GetFeedbacksOfUser(string? name, string? SearchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            var (receiver, pagenationMetadata) = await _userInfoRepository.GetUsersAsync(name, SearchQuery, pageNumber, pageSize);

            var receiverName = receiver.Where(r => r.UserName == name).FirstOrDefault();
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagenationMetadata));

            if (receiverName == null)
            {
                return NotFound();
            }


           var feedbacks = await _userInfoRepository.GetFeedbacksForUserAsync( receiverName.UserId);
            var users = new List<UsersTbl>();
            var feedToReturn = new List<UserFeedbacktoReturnDto>();
            foreach (var feedback in feedbacks)
            {
                var user = await _userInfoRepository.GetUserAsync(feedback.FeedbackFrom);
                var userFeedbackToReturn = new UserFeedbacktoReturnDto()
                {
                    UserFeedback = feedback,
                    UserWhoMadeTheFeedback = new UserForReturnDto()
                    {
                        UserId = (int)user.UserId,
                        UserName = user.UserName,
                        UserPassword = user.UserPassword,
                        UserFullName = user.UserFullName,
                        UserEmail = user.UserEmail,
                        UserNatId = user.UserNatId,
                        UserGender = user.UserGender,
                        UserAge = user.UserAge,
                        UserInfo = user.UserInfo,
                        UserAddress = user.UserAddress,
                        UserAccountType = user.UserAccountType
                    }
                };
                feedToReturn.Add(userFeedbackToReturn);
            }
           
            if (feedbacks == null)
            {
                return NotFound("there is not feedback for this user .");
            }
           return Ok(feedToReturn);
        }
       
       

        [HttpPost("userfeedback")]
        public async Task<IActionResult> SendFeedbackToUser( UserFeedbackDto userFeedbackDto,
            string? name, string? SearchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var sender = await _userInfoRepository.GetUserAsync(userId, false);
            if (sender == null)
            {
                return NotFound();
            }
            var (receiver, pagenationMetadata) = await _userInfoRepository.GetUsersAsync(name, SearchQuery, pageNumber, pageSize);

            var receiverName = receiver.Where(r => r.UserName == name).FirstOrDefault();
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagenationMetadata));

            if (receiverName == null)
            {
                return NotFound();
            }


            await _userInfoRepository.AddFeedbackToUser(userId, receiverName.UserId, userFeedbackDto.FeedbackText);
            await _userInfoRepository.SaveChangesAsync();
            return Ok("feedback is added successfully");
        }

        [HttpPut("users/{userId}/userfeedback")]
        public async Task<IActionResult> UpdateFeedbackToUser(int userId, UserFeedbackDto userFeedbackDto,
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

            var feedback = await _userInfoRepository.GetFeedbackForUserAsync(userId, receiverName.UserId);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.FeedbackText = userFeedbackDto.FeedbackText;
            await _userInfoRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("users/{userId}/userfeedback")]
        public async Task<IActionResult> DeleteFeedbackToUser(int userId, 
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

            var feedback = await _userInfoRepository.GetFeedbackForUserAsync(userId, receiverName.UserId);
            if (feedback == null)
            {
                return NotFound();
            }

            _userInfoRepository.DeleteFeedback(feedback);
            await _userInfoRepository.SaveChangesAsync();
            return Ok();
        }


    }
}
