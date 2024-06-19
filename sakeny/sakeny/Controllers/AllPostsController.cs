using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sakeny.Services.FaviourateRepo;
using sakeny.Services.NewPostRepo;
using sakeny.Services;
using sakeny.Entities;
using System.Security.Claims;

namespace sakeny.Controllers
{
	[Route("api/appposts")]
	[ApiController]
	public class AllPostsController : ControllerBase
	{
		private readonly IpostRepo _postRepo;
		private readonly IMapper _mapper;
		private readonly IFaviourateRepo _favouriteRepo;
		private readonly IUserInfoRepository _userInfoRepository;

		public AllPostsController(IpostRepo postRepo, IMapper mapper, IFaviourateRepo favouriteRepo, IUserInfoRepository userInfoRepository)
        {
			_postRepo = postRepo;
			_mapper = mapper;
			_favouriteRepo = favouriteRepo;
			_userInfoRepository = userInfoRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsAsynce()
		{
			var posts = await _postRepo.GetAllPostsAsync();
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var favouritePostIds = await _favouriteRepo.GetFavoritePosts(int.Parse(userId));

			var postDtos = posts.Select(async post => new PostDto
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
				IsFavourited = favouritePostIds.Contains((int)post.PostId),
				PostOwnerName = await _userInfoRepository.GetUserNameAsync((int)post.PostUserId),
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


		// GET: api/Posts/5
		[HttpGet("{id}")]
		public async Task<ActionResult<PostsTbl>> GetPost(decimal id)
		{
			var post = await _postRepo.GetPostByIdAsync(id);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var favouritePostIds = await _favouriteRepo.GetFavoritePosts(int.Parse(userId));


			if (post == null)
			{
				return NotFound("the post is not avaiable was deleted");
			}

			var postDto = new PostDto
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
				IsFavourited = favouritePostIds.Contains((int)post.PostId),
				PostOwnerName = _userInfoRepository.GetUserNameAsync((int)post.PostUserId).Result,
				PostPicTbls = post.PostPicTbls.Select(pic => new PostPicTblDto
				{
					PictureString = pic.PictureString
				}).ToList(),

				Features = post.Features.Select(feature => new FeaturesTblDto
				{
					FeaturesName = feature.FeaturesName
				}).ToList()
			};

			return Ok(postDto);
		}

		// GET: api/Posts/user/5
		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByUserId(decimal userId)
		{
			var posts = await _postRepo.GetAllPostsByUserIdAsync(userId);
			var favouritePostIds = await _favouriteRepo.GetFavoritePosts((int)userId);
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
				IsFavourited = favouritePostIds.Contains((int)post.PostId),
				PostOwnerName = _userInfoRepository.GetUserNameAsync((int)post.PostUserId).Result,
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

		// POST: api/Posts
		[HttpPost]
		public async Task<ActionResult<PostsTbl>> AddPost(PostDto postDto)
		{
			await _postRepo.AddPostAsync(postDto);
			await _postRepo.SaveChangesAsync();

			return Ok(postDto);
		}

		// PUT: api/Posts/5
		[HttpPut("{id}")]
		public async Task<ActionResult<PostDto>> UpdatePost(decimal id, PostDto postDto)
		{
			var updatedPost = await _postRepo.UpdatePost(id, postDto);
			if (updatedPost == null)
			{
				return NotFound($"Post with ID {id} not found.");
			}

			// Manually map the updatedPost to PostDto
			var updatedPostDto = new PostDto
			{
				PostId = updatedPost.PostId,
				// Assuming PostDto has similar properties to PostsTbl. Adjust according to your actual PostDto structure.
				PostDate = updatedPost.PostDate,
				PostTime = updatedPost.PostTime,
				PostCategory = updatedPost.PostCategory,
				PostTitle = updatedPost.PostTitle,
				PostBody = updatedPost.PostBody,
				PostArea = updatedPost.PostArea,
				PostKitchens = updatedPost.PostKitchens,
				PostBedrooms = updatedPost.PostBedrooms,
				PostBathrooms = updatedPost.PostBathrooms,
				PostLookSea = updatedPost.PostLookSea,
				PostPetsAllow = updatedPost.PostPetsAllow,
				PostCurrency = updatedPost.PostCurrency,
				PostPriceAi = updatedPost.PostPriceAi,
				PostPriceDisplay = updatedPost.PostPriceDisplay,
				PostPriceType = updatedPost.PostPriceType,
				PostAddress = updatedPost.PostAddress,
				PostCity = updatedPost.PostCity,
				PostState = updatedPost.PostState,
				PostFloor = updatedPost.PostFloor,
				PostLatitude = updatedPost.PostLatitude,
				PostLongitude = updatedPost.PostLongitude,
				PostStatue = updatedPost.PostStatue,
				PostUserId = updatedPost.PostUserId,
				PostOwnerName = await _userInfoRepository.GetUserNameAsync((int)updatedPost.PostUserId),
				// For collections like PostPicTbls and Features, you would need to map them as well, possibly with a loop or LINQ.
				PostPicTbls = updatedPost.PostPicTbls.Select(pic => new PostPicTblDto
				{
					// Assuming PostPicDto structure. Adjust according to your actual PostPicDto.
					PictureString = pic.PictureString
				}).ToList(),
				Features = updatedPost.Features.Select(f => new FeaturesTblDto
				{
					// Assuming FeatureDto structure. Adjust according to your actual FeatureDto.
					FeaturesName = f.FeaturesName
				}).ToList()
			};

			return Ok(updatedPostDto);
		}


		// DELETE: api/Posts/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePost(decimal id)
		{
			var post = await _postRepo.GetPostByIdAsync(id);
			if (post == null)
			{
				return NotFound("this post is not found !");
			}
			await _postRepo.DeletePost(id);


			return NoContent();
		}

		[HttpPost("search")]
		public async Task<ActionResult<IEnumerable<PostDto>>> SearchPostsAsynce(PostDto? searchCriteria
			  )
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var favouritePostIds = await _favouriteRepo.GetFavoritePosts(int.Parse(userId));
			var posts = await _postRepo.SearchPostsAsync(searchCriteria);
			
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
				IsFavourited = favouritePostIds.Contains((int)post.PostId),
				PostOwnerName = _userInfoRepository.GetUserNameAsync((int)post.PostUserId).Result,
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

	}
}
