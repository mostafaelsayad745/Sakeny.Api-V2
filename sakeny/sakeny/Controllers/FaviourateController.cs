using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sakeny.Entities;
using sakeny.Services.FaviourateRepo;
using sakeny.Services.NewPostRepo;
using sakeny.Services.NotificationRepo;
using System.Security.Claims;

namespace sakeny.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FaviourateController : ControllerBase
    {
        private readonly IFaviourateRepo _faviourateRepo;
        private readonly INotificationRepo _notificationRepo;
        private readonly IpostRepo _postRepo;

        public FaviourateController(IFaviourateRepo faviourateRepo , INotificationRepo notificationRepo , IpostRepo postRepo)
        {
            _faviourateRepo = faviourateRepo;
            _notificationRepo = notificationRepo;
            _postRepo = postRepo;
        }

        // GET: api/Faviourate/favorites
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoritePosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postIds = await _faviourateRepo.GetFavoritePosts(int.Parse(userId));

            var posts = new List<PostsTbl>();
            foreach (var postId in postIds)
            {
                var post = await _postRepo.GetPostByIdAsync(postId);
                if (post != null)
                {
                    posts.Add(post);
                }
            }

            var postDtos = posts.Select(post => new PostDto
            {
                PostId = post.PostId,
                PostDate = post.PostDate,
                PostTime = post.PostTime,
                PostCategory = post.PostCategory,
                PostTitle = post.PostTitle,
                PostBody = post.PostBody,
                PostArea = post.PostArea,
                PostKitchens = post.PostKitchens,
                PostBedrooms = post.PostBedrooms,
                PostBathrooms = post.PostBathrooms,
                PostLookSea = post.PostLookSea,
                PostPetsAllow = post.PostPetsAllow,
                PostCurrency = post.PostCurrency,
                PostPriceAi = post.PostPriceAi,
                PostPriceDisplay = post.PostPriceDisplay,
                PostPriceType = post.PostPriceType,
                PostAddress = post.PostAddress,
                PostCity = post.PostCity,
                PostState = post.PostState,
                PostFloor = post.PostFloor,
                PostLatitude = post.PostLatitude,
                PostLongitude = post.PostLongitude,
                PostStatue = post.PostStatue,
                PostUserId = post.PostUserId,
                IsFavourited = postIds.Contains((int)post.PostId),
                PostPicTbls = post.PostPicTbls.Select(pic => new PostPicTblDto
                {
                    PictureString = pic.PictureString
                }).ToList(),

                Features = post.Features.Select(feature => new FeaturesTblDto
                {
                    FeaturesName = feature.FeaturesName
                }).ToList()
            }).ToList();

            return Ok(postDtos);
        }


        // POST: api/Faviourate/5/favorite
        [HttpPost("{postid}/favorite")]
        public async Task<IActionResult> AddPostToFavorites(int postid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var postIds = await _faviourateRepo.GetFavoritePosts(int.Parse(userId));
            if (postIds.Contains(postid))
            {
                return BadRequest("The post is already in your favorites.");
            }


            var favoritePost = new PostFaviourateTbl
            {
                PostId = postid,
                UserId = decimal.Parse(userId)
            };

           await _faviourateRepo.AddPostToFaviourate(favoritePost);

            var notification = new NotificationTbl
            {
                UserId = favoritePost.UserId,
                Message = $"You have added post {postid} to your favorites.",
                Timestamp = DateTime.Now
            };

            await _notificationRepo.AddNotification(notification);

            return Ok("The post added to faviourates");
        }

        // DELETE: api/Faviourate/5/favorite
        [HttpDelete("{id}/favorite")]
        public async Task<IActionResult> RemovePostFromFavorites(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postIds = await _faviourateRepo.GetFavoritePosts(int.Parse(userId));
            if (!postIds.Contains(id))
            {
                return BadRequest("The post does not exist in your favorites.");
            }
            await _faviourateRepo.RemovePostFromFaviourate(id);

            var notification = new NotificationTbl
            {
                UserId = decimal.Parse(userId),
                Message = $"You have removed post {id} from your favorites.",
                Timestamp = DateTime.Now
            };

            await _notificationRepo.AddNotification(notification);

            return NoContent();
        }

        
        [HttpGet("notifications")]
        public async Task<ActionResult<IEnumerable<NotificationTbl>>> GetNotificationsForUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _notificationRepo.GetNotificationsForUser(int.Parse(userId));
            
            if (notifications == null)
            {
                return NotFound();
            }

            var notificationDtos = notifications.Select(notification => new NotificationDto
            {
                Message = notification.Message,
                Timestamp = notification.Timestamp
            }).ToList();

            return Ok(notificationDtos);
        }

    }

    public class NotificationDto
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
