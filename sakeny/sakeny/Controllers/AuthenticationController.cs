using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sakeny.DbContexts;
using sakeny.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sakeny.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserInfoRepository _userInfoRepository;

        public AuthenticationController(IConfiguration configuration, IUserInfoRepository userInfoRepository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
        }

        public class AuthenticationRequestBody
        {
            public string? UserMail { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = await validateUserCredentials(authenticationRequestBody.Password, authenticationRequestBody.UserMail);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid username or password");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString()));
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

            Response.Headers.Add("UserId", user.Id.ToString());

            return Ok(tokenToReturn);
        }

        private async Task<UserInfo> validateUserCredentials(string? password, string? userMail)
        {
            var user = await _userInfoRepository.validateUser(userMail, password);

            if (user == null)
            {
                return null;
            }

            var userToReturn = new UserInfo()
            {
                Id = user.UserId,
                UserEmail = user.UserEmail ?? string.Empty
            };

            return userToReturn;
        }

        private class UserInfo
        {
            public decimal Id { get; set; }
            public string UserEmail { get; set; } = string.Empty;
        }
    }

    

    
}
