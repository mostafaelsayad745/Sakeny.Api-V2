using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sakeny.Models.PostDtos;
using sakeny.Services;

namespace sakeny.Controllers
{
    [Route("api/users/{userid}")]
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
        public async Task<IActionResult> GetPostsFavouriates(int userid)
        {
            var user = await _userInfoRepository.GetUserAsync(userid, false);
            if (user == null)
                return NotFound();

            var postFavouriates = await _userInfoRepository.GetPostFavouriatesAsync(userid);
            var postFavouriatesForReturn = _mapper.Map<IEnumerable<PostForReturnDto>>(postFavouriates);
            return Ok(postFavouriates);
        }

    

    [HttpGet("posts/{postid}/postfavouriate")]
    public async Task<IActionResult> GetPostFavouriate(int userid, int postid)
    {
        var user = await _userInfoRepository.GetUserAsync(userid, false);
        if (user == null)
            return NotFound();

        var post = await _userInfoRepository.GetPostForUserAsync(userid, postid);
            if (post == null)
                return NotFound();



        var postFavouriate = await _userInfoRepository.GetPostFavouriateForUserAsync(userid, postid);

        return Ok(_mapper.Map<PostForReturnDto>(postFavouriate.Post));
    }

    [HttpPost("posts/{postid}/postfavouriate")]
    public async Task<IActionResult> PostFavouriate(int userid, int postid)
    {
        var post = await _userInfoRepository.GetPostForUserAsync(postid);
        if (post == null)
            return NotFound();



        var postFavouriate = await _userInfoRepository.GetPostFavouriateForUserAsync(userid, postid);

        if (postFavouriate != null)
            return BadRequest("You already favouriate this post");


        await _userInfoRepository.AddPostToFaviourates(userid , postid);
        await _userInfoRepository.SaveChangesAsync();

        return Ok();

    }

    [HttpDelete("posts/{postid}/postfavouriate")]
    public async Task<IActionResult> DeletePostFavouriate(int userid, int postid)
    {
        var post = await _userInfoRepository.GetPostForUserAsync(postid);
        if (post == null)
            return NotFound();



        var postFavouriate = await _userInfoRepository.GetPostFavouriateForUserAsync(userid, postid);

        if (postFavouriate == null)
            return BadRequest("You did not favouriate this post");

        _userInfoRepository.DeletePostFavouriate(postFavouriate);
        await _userInfoRepository.SaveChangesAsync();

        return Ok();
    }
}
}
