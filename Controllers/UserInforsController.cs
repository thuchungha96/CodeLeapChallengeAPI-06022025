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

namespace CodeLeapChallengeAPI_06022025.Controllers
{
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

        // GET: UserInfors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: UserInfors/Details/5
        [Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfor = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (userInfor == null)
            {
                return NotFound();
            }

            return View(userInfor);
        }

        // GET: UserInfors/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: UserInfors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfor = await _context.Users.FindAsync(id);
            if (userInfor == null)
            {
                return NotFound();
            }
            return View(userInfor);
        }

        // POST: UserInfors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction(nameof(Index));
            }
            return View(userInfor);
        }

        // GET: UserInfors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfor = await _context.Users
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (userInfor == null)
            {
                return NotFound();
            }

            return View(userInfor);
        }

        // POST: UserInfors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userInfor = await _context.Users.FindAsync(id);
            if (userInfor != null)
            {
                _context.Users.Remove(userInfor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInforExists(string id)
        {
            return _context.Users.Any(e => e.UserName == id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (String.IsNullOrEmpty(request.Username) || String.IsNullOrEmpty(request.Password) || Regex.IsMatch(request.Username, "^[A-Za-z]+$"))
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
        [HttpPost("createuser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password,Email,Sex,AccountType")] UserInfor userInfor)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == userInfor.UserName); 
                if (user != null)
                {
                    return ValidationProblem();
                }

                _context.Add(userInfor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInfor);
        }
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
    }
}
