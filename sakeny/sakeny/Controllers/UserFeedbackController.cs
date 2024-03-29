﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sakeny.Models.ChatDtos;
using sakeny.Models.UserFeedbackDtos;
using sakeny.Services;
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

        [HttpGet("user/userfeedbacks")]
        public async Task<IActionResult> GetFeedbacksOfUser(string? name, string? SearchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            var (receiver, pagenationMetadata) = await _userInfoRepository.GetUsersAsync(name, SearchQuery, pageNumber, pageSize);

            var receiverName = receiver.Where(r => r.UserName == SearchQuery).FirstOrDefault();
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagenationMetadata));

            if (receiver == null)
            {
                return NotFound();
            }


           var feedbacks = await _userInfoRepository.GetFeedbacksForUserAsync( receiverName.UserId);
           return Ok(feedbacks);
        }
       
       

        [HttpPost("users/{userid}/userfeedback")]
        public async Task<IActionResult> SendFeedbackToUser(int userid, UserFeedbackDto userFeedbackDto,
            string? name, string? SearchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            var sender = await _userInfoRepository.GetUserAsync(userid, false);
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


            await _userInfoRepository.AddFeedbackToUser(userid, receiverName.UserId, userFeedbackDto.FeedbackText);
            await _userInfoRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("users/{userid}/userfeedback")]
        public async Task<IActionResult> UpdateFeedbackToUser(int userid, UserFeedbackDto userFeedbackDto,
                       string? name, string? SearchQuery,
                                  int pageNumber = 1, int pageSize = 10)
        {
            var sender = await _userInfoRepository.GetUserAsync(userid, false);
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

            var feedback = await _userInfoRepository.GetFeedbackForUserAsync(userid, receiverName.UserId);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.FeedbackText = userFeedbackDto.FeedbackText;
            await _userInfoRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("users/{userid}/userfeedback")]
        public async Task<IActionResult> DeleteFeedbackToUser(int userid, 
                                  string? name, string? SearchQuery,
                                                                   int pageNumber = 1, int pageSize = 10)
        {
            var sender = await _userInfoRepository.GetUserAsync(userid, false);
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

            var feedback = await _userInfoRepository.GetFeedbackForUserAsync(userid, receiverName.UserId);
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
