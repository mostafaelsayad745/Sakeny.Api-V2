//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
//using sakeny.Services;
//using System.Text.Json;

//namespace sakeny.Controllers
//{
//    [Route("api/userBan")]
//    [ApiController]
//    public class UserBanController : ControllerBase
//    {
//        private readonly IUserInfoRepository _userInfoRepository;
//        private readonly IMapper _mapper;

//        public UserBanController(IUserInfoRepository userInfoRepository , IMapper mapper)
//        {
//            _userInfoRepository = userInfoRepository;
//            _mapper = mapper;
//        }


//        [HttpGet]
//        public async Task<IActionResult> GetBannedUsers()
//        {
//            return Ok(await _userInfoRepository.GetBannedUsersAsync());
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetUnBannedUser(string? SearchQuery)
//        {
//            var bannedUsers = await _userInfoRepository.GetBannedUserAsync(SearchQuery);
//            if (bannedUsers == null)
//            {
//                return NotFound();
//            }
//            return Ok(bannedUsers);
//        }


//        [HttpPost("ban")]
//        public async Task<IActionResult> BanUser(string? name, string? SearchQuery,
//            int pageNumber = 1, int pageSize = 10)
//        {
//            var (users, pagenationMetadata) = await _userInfoRepository.GetUsersAsync(name, SearchQuery, pageNumber, pageSize);
//            var user = users.Where(r => r.UserNatId == SearchQuery.Trim()).FirstOrDefault();
//            Response.Headers.Add("X-Pagination",
//                               JsonSerializer.Serialize(pagenationMetadata));
//            if (user == null)
//            {
//                return NotFound();
//            }
//            await _userInfoRepository.BanUserAsync(user.UserNatId);
//            return Ok("user is banned successfully");
//        }

//        [HttpPost("unban")]
//        public async Task<IActionResult> UnBanUser(string? name, string? SearchQuery,
//                       int pageNumber = 1, int pageSize = 10)
//        {
//            var (users, pagenationMetadata) = await _userInfoRepository.GetUsersAsync(name, SearchQuery, pageNumber, pageSize);
//            var user = users.Where(r => r.UserNatId == SearchQuery).FirstOrDefault();
//            Response.Headers.Add("X-Pagination",
//                                              JsonSerializer.Serialize(pagenationMetadata));
//            if (user == null)
//            {
//                return NotFound();
//            }
//            await _userInfoRepository.UnBanUserAsync(user.UserNatId);
//            return Ok("user is unbanned successfully");
//        }
//    }
//}
