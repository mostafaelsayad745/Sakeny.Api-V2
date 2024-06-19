using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using sakeny.Services;
using System.Linq;
using sakeny.Models.PicturesDtos;
using sakeny.Entities;
using System.Text.Json;

namespace sakeny.Controllers
{
    [Route("api/posts/{postId}/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserInfoRepository _userInfoRepository;

        public ImagesController(IMapper mapper, IUserInfoRepository userInfoRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetImages(int postId)
        {
            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return BadRequest("This Post is not exist");
            }

            var Pictures = await _userInfoRepository.GetPicturesForPostAsync(postId);
            var imageList = new List<object>();
            foreach (var picture in Pictures)
            {
                imageList.Add(new { id = (int)picture.PostPicId, image = picture.PictureString });
            }

            return Ok(imageList);
        }


        [HttpGet("{picId}")]
        public async Task<IActionResult> GetImage(int postId, int picId)
        {
            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return BadRequest("This Post does not exist");
            }
            if (!await _userInfoRepository.PictureExistsAsync(postId, picId))
            {
                return BadRequest("This Picture does not exist");
            }

            var picture = await _userInfoRepository.GetPictureForPostAsync(postId, picId);

            return Ok(new { id = (int)picture.PostPicId, image = picture.PictureString });
        }

        [HttpPost]
        public async Task<IActionResult> PostPictures(int postId,
    [FromForm] PicturesForCreationDto picturesForCreation)
        {
            if (picturesForCreation == null || picturesForCreation.Images == null || picturesForCreation.Images.Count == 0)
            {
                return BadRequest("Invalid image files.");
            }

            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return BadRequest("This Post is not exist");
            }

            var pictures = new List<PostPicTbl>();

            foreach (var image in picturesForCreation.Images)
            {
                if (!string.IsNullOrEmpty(image))
                {
                    PostPicTbl picture = new PostPicTbl
                    {
                        PostId = postId,
                        Post = await _userInfoRepository.GetPostForUserAsync(postId),
                        PictureString = image // Assign the base64 string directly
                    };

                    await _userInfoRepository.AddPictureForPostAsync(picture);
                    pictures.Add(picture);
                }
            }

            await _userInfoRepository.SaveChangesAsync();
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pictures.Select(p => p.PostPicId).ToArray()));

            return NoContent();
        }




        [HttpDelete("{picId}")]
        public async Task<IActionResult> DeleteImage(int postId, int picId)
        {
            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return BadRequest("This Post is not exist");
            }
            if (!await _userInfoRepository.PictureExistsAsync(postId, picId))
            {
                return BadRequest("This Picture is not exist");
            }

            var picture = await _userInfoRepository.GetPictureForPostAsync(postId, picId);
            if (picture != null)
            {
                _userInfoRepository.DeletePicture(picture);
            }

            await _userInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    
    }
}