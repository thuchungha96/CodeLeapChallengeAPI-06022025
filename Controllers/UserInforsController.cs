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

namespace CodeLeapChallengeAPI_06022025.Controllers
{
    /// <summary>
    /// List api for user
    /// </summary>
    [ApiController]
    [Route("userinfor")]
    public class UserInforsController : Controller
    {
        private readonly CodeDBContext _context;
        private readonly IConfiguration _config;
        public UserInforsController(CodeDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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
            if (id != userInfor.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInforExists(userInfor.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok();
        }
        /// <summary>
        /// Get Accesstoken
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (String.IsNullOrEmpty(request.Username) || String.IsNullOrEmpty(request.Password) || !IsValidEmail(request.Username))
            {
                return Unauthorized();
            }    
            var userInfor = await _context.Users.FirstOrDefaultAsync(m => m.UserName == request.Username && m.Password == request.Password); // Can do with base64 pass but just so little bit lazzy

            if (userInfor != null)
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { token });
            }
            return Unauthorized();
        }
        /// <summary>
        /// Create new user and save into database
        /// </summary>
        /// <param name="userInfor"></param>
        /// <returns></returns>
        [HttpPost("createuser")]
        public async Task<IActionResult> Create([Bind("UserName,Password,Email,Sex,AccountType")] UserInfor userInfor)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == userInfor.UserName); 
                if (user != null || !IsValidEmail(userInfor.UserName))
                {
                    return Unauthorized();
                }

                _context.Add(userInfor);
                await _context.SaveChangesAsync();
                return Ok(new { userInfor.UserName });
            }
            return Ok(new { userInfor.UserName });
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
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == userName);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
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
            if (String.IsNullOrEmpty(userName) == null)
                return NotFound();

            var user = await _context.Users.Where(m => m.UserName == userName).Select(o => new
            {
                o.UserName,
                o.Email,
                o.Sex
            }).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(JsonConvert.SerializeObject(user).ToString());
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
