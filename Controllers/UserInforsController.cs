using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeLeapChallengeAPI_06022025.Data.Class;
using CodeLeapChallengeAPI_06022025.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using NuGet.Common;
using Newtonsoft.Json;
using CodeLeapChallengeAPI_06022025.Data.Dto;
using Microsoft.AspNetCore.Identity;

namespace CodeLeapChallengeAPI_06022025.Controllers
{
    /// <summary>
    /// List api for user
    /// </summary>
    [ApiController]
    [Route("userinfor")]
    public class UserInforsController : BaseAPIController
    {
        private readonly CodeDBContext _context;
        private readonly IConfiguration _config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public UserInforsController(CodeDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        /// <summary>
        /// Get Accesstoken
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            ResponseDto<object> r = new ResponseDto<object>();           
            if (String.IsNullOrEmpty(request.Username) || String.IsNullOrEmpty(request.Password) || !IsValidEmail(request.Username))
            {
                r.RespnseStatus.StatusCode = StatusCodes.Status401Unauthorized;
                r.RespnseStatus.ResponseMessage = "Invalid Username/Password";
                return GetRes(r);
            }
            var userInfor = await _context.Users.FirstOrDefaultAsync(m => m.UserName == request.Username && m.Password == request.Password); // Can do with base64 pass but just so little bit lazzy
            
            if (userInfor != null)
            {
                var token = GenerateJwtToken(request.Username);
                r.ResponseData = token;
                return GetRes(r);
            } 

            r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
            r.RespnseStatus.ResponseMessage = "Invalid Username/Password";
            return GetRes(r);
        }
        /// <summary>
        /// Edit information of user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userInfor"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Password,Email,Sex,AccountType")] UserInfor userInfor)
        {
            ResponseDto<object> r = new ResponseDto<object>();
            if (id != userInfor.UserName)
            {
                r.RespnseStatus.StatusCode = StatusCodes.Status400BadRequest;
                r.RespnseStatus.ResponseMessage = "Can not edit another user information";
                return GetRes(r);
            }

            if (ModelState.IsValid)
            {
                _context.Update(userInfor);
                await _context.SaveChangesAsync();
                r.RespnseStatus.ResponseMessage = $"Success update product Id: {userInfor.UserName}";
                return GetRes(r);
            } 

            r.RespnseStatus.StatusCode = StatusCodes.Status405MethodNotAllowed;
            r.RespnseStatus.ResponseMessage = "Can not edit information";
            return GetRes(r);
        }
        /// <summary>
        /// Create new user and save into database
        /// </summary>
        /// <param name="userInfor"></param>
        /// <returns></returns>
        [HttpPost("createuser")]
        public async Task<IActionResult> Create([Bind("UserName,Password,Email,Sex,AccountType")] UserInfor userInfor)
        {
            ResponseDto<object> r = new ResponseDto<object>();
            if (ModelState.IsValid)
            {
                if (!IsValidEmail(userInfor.UserName))
                {
                    r.RespnseStatus.StatusCode = StatusCodes.Status400BadRequest;
                    r.RespnseStatus.ResponseMessage = "Invalid Username (much email format)!";
                    return GetRes(r);
                }
                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == userInfor.UserName); 
                if (user != null)
                {
                    r.RespnseStatus.StatusCode = StatusCodes.Status409Conflict;
                    r.RespnseStatus.ResponseMessage = "Username already exited!";
                    return GetRes(r);
                }

                _context.Add(userInfor);
                await _context.SaveChangesAsync();
                r.RespnseStatus.StatusCode = StatusCodes.Status201Created;
                r.RespnseStatus.ResponseMessage = $"Success created account {userInfor.UserName}";
                return GetRes(r);
            }

            r.RespnseStatus.StatusCode = StatusCodes.Status405MethodNotAllowed;
            r.RespnseStatus.ResponseMessage = "Can not edit information";
            return GetRes(r);
        }
        /// <summary>
        /// Delete exited user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string userName)
        {
            ResponseDto<object> r = new ResponseDto<object>();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == userName);
            if (user == null)
                {
                r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
                r.RespnseStatus.ResponseMessage = "can not found account!";
                return GetRes(r);
                }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return GetRes(r);
        }
        /// <summary>
        /// Get detail of any user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        // GET: UserInfors/Details/5
        [Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> Details(string userName)
        {
            ResponseDto<object> r = new ResponseDto<object>();
            if (String.IsNullOrEmpty(userName) == null)
                {
                    r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
                    r.RespnseStatus.ResponseMessage = "can not found account!";
                    return GetRes(r);
                }

            var user = await _context.Users.Where(m => m.UserName == userName).Select(o => new
            {
                o.UserName,
                o.Email,
                o.Sex
            }).FirstOrDefaultAsync();
            if (user == null)
            {
                r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
                r.RespnseStatus.ResponseMessage = "can not found account!";
                return GetRes(r);
            }
            r.ResponseData = user;
            return GetRes(r);
        }
        /// <summary>
        /// Get detail of current user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("detail")]
        public async Task<IActionResult> Detail()
        {
            ResponseDto<object> r = new ResponseDto<object>();
            var user = await _context.Users.Where(m => m.UserName == User.Identity.Name).Select(o => new
            {
                o.UserName,
                o.Email,
                o.Sex
            }).FirstOrDefaultAsync();
            if (user == null)
            {
                r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
                r.RespnseStatus.ResponseMessage = "can not found account!";
                return GetRes(r);
            }
            r.ResponseData = user;
            return GetRes(r);
        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Invalidate the JWT (add to blacklist if needed)
            ResponseDto<object> r = new ResponseDto<object>();
            r.RespnseStatus.ResponseMessage = "Logged out successfully";
            return GetRes(r);
        }
        [NonAction]
        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim("role", "admin") // Thêm quyền
            }),
                Expires = DateTime.UtcNow.AddHours(1), // Hết hạn sau 1 giờ
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [NonAction]
        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        [NonAction]
        private bool UserInforExists(string id)
        {
            return _context.Users.Any(e => e.UserName == id);
        }
    }
}
