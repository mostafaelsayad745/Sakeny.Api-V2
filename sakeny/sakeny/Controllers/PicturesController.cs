using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sakeny.Entities;
using sakeny.Models.PicturesDtos;
using sakeny.Services;
using System.Net.Mime;
using System.Text.Json;

namespace sakeny.Controllers
{
    [Route("api/posts/{postId}/pictures")]
    //[Authorize]
    [ApiController]
    public class PicutresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserInfoRepository _userInfoRepository;

        public PicutresController(IMapper mapper, IUserInfoRepository userInfoRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
        }


        [HttpGet(Name = "GetPictures")]
        public async Task<IActionResult> GetPictures(int postId)
        {
            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return BadRequest("This Post is not exist");
            }

            var Pictures = await _userInfoRepository.GetPicturesForPostAsync(postId);
            var imageList = new List<byte[]>();
            foreach (var picture in Pictures)
            {
                imageList.Add(picture.Picture);
            }

            var combinedImageData = imageList.SelectMany(p => p).ToArray();

            return File(combinedImageData, MediaTypeNames.Image.Jpeg);

        }

        [HttpGet("{picId}")]
        public async Task<IActionResult> GetPicture(int postId, int picId)
        {
            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return BadRequest("This Post is not exist");
            }
            if (!await _userInfoRepository.PictureExistsAsync(postId, picId))
            {
                return BadRequest("This Picture is not exist");
            }

            var Pictures = await _userInfoRepository.GetPictureForPostAsync(postId, picId);
            var combinedImageData = Pictures.Picture;

            return File(combinedImageData, MediaTypeNames.Image.Jpeg);

        }

        [HttpPost]
        public async Task<IActionResult> PostPictures(int postId,
            [FromForm(Name = "Data")] PicturesForCreationDto picturesForCreation)
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
                if (image.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        PostPicTbl picture = new PostPicTbl
                        {
                            PostId = postId,
                            Post = await _userInfoRepository.GetPostForUserAsync(postId),
                            Picture = ms.ToArray()
                        };

                        await _userInfoRepository.AddPictureForPostAsync(picture);
                        pictures.Add(picture);
                    }
                }
            }

            await _userInfoRepository.SaveChangesAsync();
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pictures.Select(p => p.PostPicId).ToArray()));

            return NoContent();
        }


        [HttpDelete]
        public async Task<IActionResult> DeletePictures(int postId)
        {
            if (!await _userInfoRepository.PostExistsAsync(postId))
            {
                return BadRequest("This Post is not exist");
            }

            var pictures = await _userInfoRepository.GetPicturesForPostAsync(postId);
            if (pictures != null)
            {
                foreach (var picture in pictures)
                {
                    _userInfoRepository.DeletePicture(picture);
                }
            }

            await _userInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{picId}")]
        public async Task<IActionResult> DeletePicture(int postId, int picId)
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

