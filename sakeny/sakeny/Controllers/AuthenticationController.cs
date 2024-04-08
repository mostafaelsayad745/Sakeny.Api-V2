using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sakeny.DbContexts;
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

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public class AuthenticationRequestBody
        {

            //public string? UserName { get; set; }
            public string? UserMail { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("login")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = validateUserCredentials(authenticationRequestBody.Password, authenticationRequestBody.UserMail);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid username or password");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString()));
            //claimsForToken.Add(new Claim("user_name", user.UserName));
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

        private UserInfo validateUserCredentials( string? password , string? userMail)
        {
            HOUSE_RENT_DBContext context = new HOUSE_RENT_DBContext();
            var user = context.UsersTbls.FirstOrDefault(u =>  u.UserEmail == userMail && u.UserPassword == password);
            if (user == null)
            {
                return null;
            }
            return new UserInfo(user.UserId,  user.UserEmail);
        }

        private class UserInfo
        {
            public UserInfo(decimal id, string userEmail)
            {
                Id = id;
                //UserName = userName;
                UserEmail = userEmail;
               
            }

            public decimal Id { get; set; }
           // public string UserName { get; set; }
            public string UserEmail { get; set; }
            

        }


    }

    

    
}
