using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using sakeny.Services;
using System.Linq;
using sakeny.Models.PicturesDtos;
using sakeny.Entities;

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
            var imageList = new List<string>();
            foreach (var picture in Pictures)
            {
                var base64Image = Convert.ToBase64String(picture.Picture);
                imageList.Add($"data:image/jpeg;base64,{base64Image}");
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
            var base64Image = Convert.ToBase64String(picture.Picture);

            return Ok($"data:image/jpeg;base64,{base64Image}");
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(int postId,
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