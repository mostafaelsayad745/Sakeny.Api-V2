using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sakeny.Entities;
using sakeny.Models;
using sakeny.Models.PostFeedbackDto;
using sakeny.Services.NewPostRepo;
using sakeny.Services.PostFeedbackRepo;
using System.Security.Claims;

namespace sakeny.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsFeedbackController : ControllerBase
    {
        private readonly IPostFeedbackRepo _postFeedbackRepo;
        private readonly IpostRepo _postRepo;

        public PostsFeedbackController(IPostFeedbackRepo postFeedbackRepo , IpostRepo postRepo)
        {
            _postFeedbackRepo = postFeedbackRepo;
            _postRepo = postRepo;
        }

        // GET: api/PostFeedback/post/5
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<PostFeedbackTbl>>> GetFeedbacksForPost(decimal postId)
        {
            var feedbacks = await _postFeedbackRepo.GetFeedbacksForPost(postId);

            if (feedbacks == null || feedbacks.Count() < 1)
            {
                return NotFound("there isn't feedbacks for this post ");
            }

            // Map the feedbacks to FeedbackResponseDto
            var response = feedbacks.Select(f => new FeedbackResponseDto
            {
                TimeCreated = f.PostFeedDate,
                FeedbackText = f.PostFeedText,
                User = new UserForReturnDto // Add this block
                {
                    UserId = (int)f.UserId,
                    UserName = f.User.UserName,
                    UserPassword = f.User.UserPassword,
                    UserFullName = f.User.UserFullName,
                    UserEmail = f.User.UserEmail,
                    UserNatId = f.User.UserNatId,
                    UserGender = f.User.UserGender,
                    UserAge = f.User.UserAge,
                    UserInfo = f.User.UserInfo,
                    UserAddress = f.User.UserAddress,
                    UserAccountType = f.User.UserAccountType
                }
            }).ToList();

            return Ok(response);
        }

        // POST: api/PostFeedback
        [HttpPost]
        public async Task<ActionResult<PostFeedbackTbl>> AddFeedback(PostFeedbackTblDto feedbackdot)
        {
            var post = await _postRepo.PostExistsAsync(feedbackdot.PostId.Value);
            if (post == false)
            {
                return NotFound("Post not found");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingFeedback = await _postFeedbackRepo.GetFeedbackByUserAndPost(int.Parse(userId), feedbackdot.PostId.Value);
            if (existingFeedback != null)
            {
                return BadRequest("You can only make one feedback to the post.");
            }

            var feedback = new PostFeedbackTbl
            {
                PostFeedText = feedbackdot.PostFeedText,
                UserId = int.Parse(userId),
                PostId = feedbackdot.PostId
            };
            
            await _postFeedbackRepo.AddFeedback(feedback);
            await _postFeedbackRepo.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedbacksForPost), new { postId = feedback.PostId }, feedback);
        }


        // PUT: api/PostFeedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(decimal id, PostFeedbackTbl feedback)
        {
            if (id != feedback.PostFeedId)
            {
                return BadRequest();
            }

            _postFeedbackRepo.UpdateFeedback(feedback);
            await _postFeedbackRepo.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/PostFeedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(decimal id)
        {
            _postFeedbackRepo.DeleteOneFeedback(id);
            

            return NoContent();
        }
    }

}
