using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sakeny.Entities;
using sakeny.Models.NotificationDtos;
using sakeny.Services;

namespace sakeny.Controllers
{
    [Route("api/users/{userid}/notifications")]
    [Authorize]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserInfoRepository _userInfoRepository;

        public NotificationsController(IMapper mapper, IUserInfoRepository userInfoRepository)
        {
            _mapper = mapper;
            _userInfoRepository = userInfoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return BadRequest("This User is not exist");
            }

            var notifications = await _userInfoRepository.GetNotificationsForUserAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("{notificationId}", Name = "GetNotification")]
        public async Task<IActionResult> GetNotification(int userId, int notificationId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return BadRequest("This User is not exist");
            }
            if (!await _userInfoRepository.NotificationExistsAsync(userId, notificationId))
            {
                return BadRequest("This Notification is not exist");
            }

            var notification = await _userInfoRepository.GetNotificationForUserAsync(userId, notificationId);
            var notificationToReturn = _mapper.Map<NotificationForReturnDto>(notification);
            return Ok(notificationToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddNotification(int userId, NotificationForCreationDto notification)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return BadRequest("This User is not exist");
            }

            var notificationEntity = _mapper.Map<NotificationTbl>(notification);
            await _userInfoRepository.AddNotificationForUserAsync(userId, notificationEntity);
            await _userInfoRepository.SaveChangesAsync();

            var notificationToReturn = _mapper.Map<NotificationForReturnDto>(notificationEntity);
            return CreatedAtRoute("GetNotification",
                               new { userId, notificationId = notificationEntity.NotificationId },
                                              notificationToReturn);
        }

        [HttpPut("{notificationId}")]
        public async Task<IActionResult> UpdateNotification(int userId, int notificationId, NotificationForUpdateDto notification)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return BadRequest("This User is not exist");
            }
            if (!await _userInfoRepository.NotificationExistsAsync(userId, notificationId))
            {
                return BadRequest("This Notification is not exist");
            }

            var notificationFromRepo = await _userInfoRepository.GetNotificationForUserAsync(userId, notificationId);
            _mapper.Map(notification, notificationFromRepo);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int userId, int notificationId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return BadRequest("This User is not exist");
            }
            if (!await _userInfoRepository.NotificationExistsAsync(userId, notificationId))
            {
                return BadRequest("This Notification is not exist");
            }

            var notification = await _userInfoRepository.GetNotificationForUserAsync(userId, notificationId);
            if (notification != null)
            {
                _userInfoRepository.DeleteNotification(notification);
            }
            await _userInfoRepository.SaveChangesAsync();

            return NoContent();
        }






    }
}
