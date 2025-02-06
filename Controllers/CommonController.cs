using CodeLeapChallengeAPI_06022025.Data.Class;
using CodeLeapChallengeAPI_06022025.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeLeapChallengeAPI_06022025.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommonController : Controller
    {
        private readonly CodeDBContext _context;
        private readonly IConfiguration _config;
        public CommonController(CodeDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }  


    }
}
