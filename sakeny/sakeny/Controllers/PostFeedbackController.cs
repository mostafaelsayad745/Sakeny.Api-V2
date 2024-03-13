using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Models;
using sakeny.Services;

namespace sakeny.Controllers
{
    [Route("api/users/{userid}/posts/{postid}/postfeedbacks")]
    [Authorize]
    [ApiController]
    public class PostFeedbackController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;

        public PostFeedbackController(IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetPostFeedbacks(int userId ,int postId)
        {
            if (!await _userInfoRepository.PostExistsAsync(userId ,postId))
            {
                return NotFound();
            }
            var postFeedbacksForUser = await _userInfoRepository.GetPostFeedbacksForUserAsync( postId);
            return Ok(_mapper.Map<IEnumerable<PostFeedbackForReturnDto>>(postFeedbacksForUser));
        }

        [HttpGet("{feedbackid}", Name = "GetPostFeedback")]
        public async Task<IActionResult> GetPostFeedback(int userId,int postId, int feedbackId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var postFeedbackForUser = await _userInfoRepository.GetPostFeedbackForUserAsync(userId,postId, feedbackId);

            if (postFeedbackForUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostFeedbackForReturnDto>(postFeedbackForUser));
        }


        [HttpPost]
        public async Task<IActionResult> AddPostFeedback(int userId,int postId
            , PostFeedbackForCreationDto postFeedbackForCreationDto)
        {
            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return NotFound();
            }
            var postFeedbackEntity = _mapper.Map<Entities.PostFeedbackTbl>(postFeedbackForCreationDto);
            postFeedbackEntity.PostId = postId;

            postFeedbackEntity.UserId = userId;
           
            await _userInfoRepository.AddPostFeedbackForUserAsync(userId,postId, postFeedbackEntity);

            try
            {
                await _userInfoRepository.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, "YOU CAN MAKE ONE FEEDBACK TO THE POST");
            }
            var postFeedbackToReturn = _mapper.Map<PostFeedbackForReturnDto>(postFeedbackEntity);
            return CreatedAtRoute("GetPostFeedback", new { userId = userId, postId = postId, feedbackId = postFeedbackEntity.PostFeedId }, postFeedbackToReturn);
        }

        [HttpPut("{feedbackid}")]
        public async Task<ActionResult> UpdatePostFeedback(int userId,int postId, int feedbackId, PostFeedbackForUpdateDto postFeedbackForUpdateDto)
        {
            if (!await _userInfoRepository.PostExistsAsync(userId,postId))
            {
                return NotFound();
            }
            var postFeedbackFromRepo = await _userInfoRepository.GetPostFeedbackForUserAsync(userId,postId, feedbackId);
            if (postFeedbackFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(postFeedbackForUpdateDto, postFeedbackFromRepo);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{feedbackId}")]
        public async Task<IActionResult> PartiallyUpdatePostFeedback(int userId,int postId, int feedbackId, JsonPatchDocument<PostFeedbackForUpdateDto> patchDocument)
        {
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var postFeedbackFromRepo = await _userInfoRepository.GetPostFeedbackForUserAsync(userId,postId, feedbackId);
            if (postFeedbackFromRepo == null)
            {
                return NotFound();
            }
            var postFeedbackToPatch = _mapper.Map<PostFeedbackForUpdateDto>(postFeedbackFromRepo);
            patchDocument.ApplyTo(postFeedbackToPatch, ModelState);
            if (!TryValidateModel(postFeedbackToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(postFeedbackToPatch, postFeedbackFromRepo);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> DeletePostFeedback(int userId,int postId, int feedbackId)
        {
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var postFeedbackFromRepo = await _userInfoRepository.GetPostFeedbackForUserAsync(userId,postId, feedbackId);
            if (postFeedbackFromRepo == null)
            {
                return NotFound();
            }
            _userInfoRepository.DeletePostFeedback(postFeedbackFromRepo);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
