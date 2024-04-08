using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sakeny.Entities;
using sakeny.Models.FeaturesDtos;
using sakeny.Services;

namespace sakeny.Controllers
{
    [Route("api/users/{userId}/posts/{postId}/features")]
    [Authorize]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;
        public FeaturesController(IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<IActionResult> GetFeatures(int userId, int postId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var featuresForPost = await _userInfoRepository.GetFeaturesForPostAsync(postId);
            return Ok(_mapper.Map<IEnumerable<FeatureForReturnDto>>(featuresForPost));
        }
        [HttpGet("{featureId}", Name = "GetFeature")]
        public async Task<IActionResult> GetFeature(int userId, int postId, int featureId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var featureForPost = await _userInfoRepository.GetFeatureForPostAsync(postId, featureId);
            if (featureForPost == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FeatureForReturnDto>(featureForPost));
        }
        [HttpPost]
        public async Task<IActionResult> AddFeature(int userId, int postId, FeatureForCreationDto featureForCreationDto)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var feature = _mapper.Map<FeaturesTbl>(featureForCreationDto);
            
            await _userInfoRepository.AddFeatureForPostAsync(postId, feature);
            await _userInfoRepository.SaveChangesAsync();
            var featureToReturn = _mapper.Map<FeatureForReturnDto>(feature);
            return CreatedAtRoute("GetFeature", new { userId, postId, featureId = feature.FeaturesId }, featureToReturn);
        }
        [HttpPut("{featureId}")]
        public async Task<IActionResult> UpdateFeature(int userId, int postId, int featureId, FeatureForUpdateDto featureForUpdateDto)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var feature = await _userInfoRepository.GetFeatureForPostAsync(postId, featureId);
            if (feature == null)
            {
                return NotFound();
            }
            _mapper.Map(featureForUpdateDto, feature);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{featureId}")]
        public async Task<IActionResult> PartiallyUpdateFeature(int userId, int postId, int featureId, JsonPatchDocument<FeatureForUpdateDto> patchDocument)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var feature = await _userInfoRepository.GetFeatureForPostAsync(postId, featureId);
            if (feature == null)
            {
                return NotFound();
            }
            var featureToPatch = _mapper.Map<FeatureForUpdateDto>(feature);
            patchDocument.ApplyTo(featureToPatch, ModelState);
            if (!TryValidateModel(featureToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(featureToPatch, feature);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{featureId}")]
        public async Task<IActionResult> DeleteFeature(int userId, int postId, int featureId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            if (!await _userInfoRepository.PostExistsAsync(userId, postId))
            {
                return NotFound();
            }
            var feature = await _userInfoRepository.GetFeatureForPostAsync(postId, featureId);
            if (feature == null)
            {
                return NotFound();
            }
            _userInfoRepository.DeleteFeature(feature);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
