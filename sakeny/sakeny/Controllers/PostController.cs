using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using sakeny.Entities;
using sakeny.Models.PostDtos;
using sakeny.Services;
using System.Text.Json;

namespace sakeny.Controllers
{
    [Route("api/users/{userId}/posts")]
    //[Authorize]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        const int maxPageSize = 20;

        public PostController(IUserInfoRepository userInfoRepository, IMapper mapper,
        IHubContext<NotificationHub> hubContext)
        {
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _hubContext = hubContext;

        }



        [HttpGet]
        public async Task<IActionResult> GetPosts(int userId ,
            string? name , string? SearchQuery, int pageNumber = 1 , int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var (postsForUser,pagnationMetadata) = await _userInfoRepository.GetPostsForUserAsync(userId,
                name ,SearchQuery ,pageNumber ,pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagnationMetadata));

            return Ok(_mapper.Map<IEnumerable<PostForReturnDto>>(postsForUser));
        }

        [HttpGet("{postId}", Name = "GetPost")]
        public async Task<IActionResult> GetPost(int userId, int postId )
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var postForUser = await _userInfoRepository.GetPostForUserAsync(userId, postId);

            if (postForUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostForReturnDto>(postForUser));
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(int userId, PostForCreationDto postForCreationDto)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var postEntity = _mapper.Map<PostsTbl>(postForCreationDto);
            postEntity.PostUserId = userId;
            await _userInfoRepository.AddPostForUserAsync(userId, postEntity);
            await _userInfoRepository.SaveChangesAsync();

            var postToReturn = _mapper.Map<PostForReturnDto>(postEntity);
            return CreatedAtRoute("GetPost",
                               new { userId, postId = postEntity.PostId },
                                              postToReturn);
        }
        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost (int userId , int postId , PostForUpdateDto postForUpdateDto)
        {
            if(! await _userInfoRepository.UserExistsAsync(userId))
            { 
                return NotFound(); 
            }
            if(! await _userInfoRepository.PostExistsAsync(userId,postId))
            {
                return NotFound();
            }
            if(postForUpdateDto == null)
            {
                return BadRequest("you must give me the post information");
            }

            var postEntity = await _userInfoRepository.GetPostForUserAsync(userId, postId);
            _mapper.Map(postForUpdateDto, postEntity);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();

        }
        [HttpPatch("{postId}")]
        public async Task<IActionResult> PartialUpdatePost(int userId, int postId, JsonPatchDocument<PostForUpdateDto> patchDocument)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }


            var postEntity = await _userInfoRepository.GetPostForUserAsync(userId, postId);
            var postToPatch = _mapper.Map<PostForUpdateDto>(postEntity);
            patchDocument.ApplyTo(postToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(postToPatch))
            {
                return ValidationProblem(ModelState);
            }


            _mapper.Map(postToPatch, postEntity);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();

        }




        [HttpPatch("{postId}/statues")]
        public async Task<IActionResult> PartialUpdatePostForStatues(int userId, int postId,JsonPatchDocument< PostForUpdateDto> patchDocument)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
           

            var postEntity = await _userInfoRepository.GetPostForUserAsyncForUpdate(userId, postId);
            var postToPatch = _mapper.Map<PostForUpdateDto>(postEntity);
            patchDocument.ApplyTo(postToPatch, ModelState);

            if(! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (! TryValidateModel(postToPatch))
            {
                return ValidationProblem(ModelState);
            }


            _mapper.Map(postToPatch, postEntity);
            await _userInfoRepository.SaveChangesAsync();

            var users = await _userInfoRepository.GetUsersWhoFavouriatePostAsync(postId);

            foreach (var user in users)
            {
                var message = $"The status of post {postId} has been changed to {postToPatch.PostStatue}";
                await _hubContext.Clients.User(user.UserId.ToString()).SendAsync("ReceiveNotification", message);
            }

            return NoContent();

        }

       

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int userId, int postId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var postForUser = await _userInfoRepository.GetPostForUserAsync(userId, postId);

            if (postForUser == null)
            {
                return NotFound();
            }

            _userInfoRepository.DeletePost(postForUser);
            await _userInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
