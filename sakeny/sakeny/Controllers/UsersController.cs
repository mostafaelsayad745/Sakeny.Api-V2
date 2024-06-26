﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using sakeny.DbContexts;
using sakeny.Entities;
using sakeny.Models;
using sakeny.Services;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sakeny.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        const int maxPageSize = 20;

        public UsersController(ILogger<UsersController> logger, IUserInfoRepository userInfoRepository
            , IMapper mapper , IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetUser(int userId)
        {
            var userFromRepo = await _userInfoRepository.GetUserAsync(userId);
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
            else
            {
                var verificationCode = new Random().Next(100000, 999999).ToString();

                // Send the verification code to the user's email
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
                emailMessage.To.Add(new MailboxAddress("", postUserEntity.UserEmail));
                emailMessage.Subject = "Verification Code";
                emailMessage.Body = new TextPart("plain") { Text = $"Your verification code is {verificationCode}" };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("sandbox.smtp.mailtrap.io", 2525, false);
                    await client.AuthenticateAsync("aa06c94b9d5589", "017d5983e41cb6");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }


            await _userInfoRepository.AddUserAsync(postUserEntity);
            await _userInfoRepository.SaveChangesAsync();
            var token = GenerateTokenForUser(postUserEntity);

            var postUserToReturn = _mapper.Map<UserForReturnDto>(postUserEntity);

            // Return the token along with the user details
            return CreatedAtRoute("GetUser", new { userId = postUserToReturn.UserId }, new { postUserToReturn, token });
        

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

        [HttpPut]
       [Authorize]
        public async Task<IActionResult> UpdateUser( UserForUpdateDto userForUpdateDto)
        {
            if (!await _userInfoRepository.UserExistsAsync((int)userForUpdateDto.UserId))
            {
                return NotFound();
            }

            if (userForUpdateDto == null)
            {
                return BadRequest("you must give me the user information");
            }

            var userEntity = await _userInfoRepository.GetUserAsync((int)userForUpdateDto.UserId, false);
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
       [Authorize]
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
        [Authorize]
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

        private string GenerateTokenForUser(UsersTbl user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("user_email", user.UserEmail));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }
    }
}
