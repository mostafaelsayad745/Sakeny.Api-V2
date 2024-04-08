using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sakeny.Models.PostDtos;
using sakeny.Services;

namespace sakeny.Controllers
{
    [Route("api/users/{userId}")]
    [Authorize]
    [ApiController]
    public class PostFavouriateController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;

        public PostFavouriateController(IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
        }



        [HttpGet("postfavouriate")]
        public async Task<IActionResult> GetPostsFavouriates(int userId)
        {
            var user = await _userInfoRepository.GetUserAsync(userId, false);
            if (user == null)
                return NotFound();

            var postFavouriates = await _userInfoRepository.GetPostFavouriatesAsync(userId);
            var postFavouriatesForReturn = _mapper.Map<IEnumerable<PostForReturnDto>>(postFavouriates);
            return Ok(postFavouriates);
        }

    

    [HttpGet("posts/{postId}/postfavouriate")]
    public async Task<IActionResult> GetPostFavouriate(int userId, int postId)
    {
        var user = await _userInfoRepository.GetUserAsync(userId, false);
        if (user == null)
            return NotFound();

        var post = await _userInfoRepository.GetPostForUserAsync(userId, postId);
            if (post == null)
                return NotFound();



        var postFavouriate = await _userInfoRepository.GetPostFavouriateForUserAsync(userId, postId);

        return Ok(_mapper.Map<PostForReturnDto>(postFavouriate.Post));
    }

    [HttpPost("posts/{postId}/postfavouriate")]
    public async Task<IActionResult> PostFavouriate(int userId, int postId)
    {
        var post = await _userInfoRepository.GetPostForUserAsync(postId);
        if (post == null)
            return NotFound();



        var postFavouriate = await _userInfoRepository.GetPostFavouriateForUserAsync(userId, postId);

        if (postFavouriate != null)
            return BadRequest("You already favouriate this post");


        await _userInfoRepository.AddPostToFaviourates(userId , postId);
        await _userInfoRepository.SaveChangesAsync();

        return Ok();

    }

    [HttpDelete("posts/{postId}/postfavouriate")]
    public async Task<IActionResult> DeletePostFavouriate(int userId, int postId)
    {
        var post = await _userInfoRepository.GetPostForUserAsync(postId);
        if (post == null)
            return NotFound();



        var postFavouriate = await _userInfoRepository.GetPostFavouriateForUserAsync(userId, postId);

        if (postFavouriate == null)
            return BadRequest("You did not favouriate this post");

        _userInfoRepository.DeletePostFavouriate(postFavouriate);
        await _userInfoRepository.SaveChangesAsync();

        return Ok();
    }
}
}
