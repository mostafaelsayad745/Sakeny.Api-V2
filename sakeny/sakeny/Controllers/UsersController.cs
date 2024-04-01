using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sakeny.DbContexts;
using sakeny.Entities;
using sakeny.Models;
using sakeny.Services;
using System.Text.Json;

namespace sakeny.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public UsersController(ILogger<UsersController> logger, IUserInfoRepository userInfoRepository
            , IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<UserForReturnDto>> GetUsers(string? name, string? SearchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (usersFromRepo, pagenationMetadata) = await _userInfoRepository.
                GetUsersAsync(name, SearchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagenationMetadata));

            return Ok(_mapper.Map<IEnumerable<UserForReturnDto>>(usersFromRepo));
        }

        [HttpGet("{userId}", Name = "GetUser")]
        //[Authorize]
        public async Task<IActionResult> GetUser(int userId, bool includePostFeedbacks)
        {
            var userFromRepo = await _userInfoRepository.GetUserAsync(userId, includePostFeedbacks);
            if (userFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserForReturnDto>(userFromRepo));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserForCreationDto userForCreationDto)
        {
            if (userForCreationDto == null)
            {
                return BadRequest("you must give me the user information");
            }
            var postUserEntity = _mapper.Map<UsersTbl>(userForCreationDto);

            var emailCheckd = await _userInfoRepository.checkEmailNotRepated(postUserEntity);
            if (emailCheckd)
            {
                return BadRequest("this email is already used");
            }

            await _userInfoRepository.AddUserAsync(postUserEntity);
            await _userInfoRepository.SaveChangesAsync();
            var postUserToReturn = _mapper.Map<UserForReturnDto>(postUserEntity);
            return CreatedAtRoute("GetUser", new { id = postUserToReturn.UserId }, postUserToReturn);

            //var db = new HOUSE_RENT_DBContext();
            //var maxUserId = db.UsersTbls.Max(u => u.UserId);
            //var userToCreate = new UsersTbl
            //{
            //    UserName = userForCreationDto.UserName,
            //    UserPassword = userForCreationDto.UserPassword,
            //    UserFullName = userForCreationDto.UserFullName,
            //    UserEmail = userForCreationDto.UserEmail,
            //    UserNatId = userForCreationDto.UserNatId,
            //    UserGender = userForCreationDto.UserGender,
            //    UserAge = userForCreationDto.UserAge,
            //    UserInfo = userForCreationDto.UserInfo,
            //    UserAddress = userForCreationDto.UserAddress,
            //    UserAccountType = userForCreationDto.UserAccountType
            //};
            //db.UsersTbls.Add(userToCreate);
            //db.SaveChanges();
            //return CreatedAtRoute("GetUser", new { id = userToCreate.UserId }, userToCreate);
        }

        [HttpPut("{userId}")]
       // [Authorize]
        public async Task<IActionResult> UpdateUser(int userId, UserForUpdateDto userForUpdateDto)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            if (userForUpdateDto == null)
            {
                return BadRequest("you must give me the user information");
            }

            var userEntity = await _userInfoRepository.GetUserAsync(userId, false);
            _mapper.Map(userForUpdateDto, userEntity);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();



            //var db = new HOUSE_RENT_DBContext();
            //var userToUpdate = db.UsersTbls.FirstOrDefault(u => u.UserId == userid);
            //if (userToUpdate == null)
            //{
            //    return NotFound();
            //}
            //userToUpdate.UserName = userForUpdateDto.UserName;
            //userToUpdate.UserPassword = userForUpdateDto.UserPassword;
            //userToUpdate.UserFullName = userForUpdateDto.UserFullName;
            //userToUpdate.UserEmail = userForUpdateDto.UserEmail;
            //userToUpdate.UserNatId = userForUpdateDto.UserNatId;
            //userToUpdate.UserGender = userForUpdateDto.UserGender;
            //userToUpdate.UserAge = userForUpdateDto.UserAge;
            //userToUpdate.UserInfo = userForUpdateDto.UserInfo;
            //userToUpdate.UserAddress = userForUpdateDto.UserAddress;
            //userToUpdate.UserAccountType = userForUpdateDto.UserAccountType;
            //db.SaveChanges();
            //return NoContent();
        }

        [HttpPatch("{userId}")]
       // [Authorize]
        public async Task<IActionResult> PartialUpdateUser(int userId,
            JsonPatchDocument<UserForUpdateDto> patchDocument)
        {

            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var userEnitiy = await _userInfoRepository.GetUserAsync(userId, false);
            if (userEnitiy == null)
            {
                return NotFound();
            }

            var userToPatch = _mapper.Map<UserForUpdateDto>(userEnitiy);

            patchDocument.ApplyTo(userToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, userEnitiy);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();




            //var db = new HOUSE_RENT_DBContext();
            //var userToUpdate = db.UsersTbls.FirstOrDefault(u => u.UserId == userid);
            //if (userToUpdate == null)
            //{
            //    return NotFound();
            //}
            //var userToPatch = new UserForUpdateDto
            //{
            //    UserName = userToUpdate.UserName,
            //    UserPassword = userToUpdate.UserPassword,
            //    UserFullName = userToUpdate.UserFullName,
            //    UserEmail = userToUpdate.UserEmail,
            //    UserNatId = userToUpdate.UserNatId,
            //    UserGender = userToUpdate.UserGender,
            //    UserAge = userToUpdate.UserAge,
            //    UserInfo = userToUpdate.UserInfo,
            //    UserAddress = userToUpdate.UserAddress,
            //    UserAccountType = userToUpdate.UserAccountType
            //};
            //patchDocument.ApplyTo(userToPatch, ModelState);
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if(!TryValidateModel(userToPatch))
            //{
            //    return BadRequest(ModelState);
            //}
            //userToUpdate.UserName = userToPatch.UserName;
            //userToUpdate.UserPassword = userToPatch.UserPassword;
            //userToUpdate.UserFullName = userToPatch.UserFullName;
            //userToUpdate.UserEmail = userToPatch.UserEmail;
            //userToUpdate.UserNatId = userToPatch.UserNatId;
            //userToUpdate.UserGender = userToPatch.UserGender;
            //userToUpdate.UserAge = userToPatch.UserAge;
            //userToUpdate.UserInfo = userToPatch.UserInfo;
            //userToUpdate.UserAddress = userToPatch.UserAddress;
            //userToUpdate.UserAccountType = userToPatch.UserAccountType;

            //db.SaveChanges();
            //return NoContent();
        }

        [HttpDelete("{userId}")]
       // [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var userEntity = await _userInfoRepository.GetUserAsync(userId, false);
            if (userEntity == null)
            {
                return NotFound();
            }
            _userInfoRepository.DeleteUserAsync(userEntity);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
