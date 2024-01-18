using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.EF;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Buisness.Services;

using System.Net;


namespace EstateAgentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[EnableCors(""_myAllowSpecificOrigins"")]
    public class UserController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly EstateAgentContext _context;
        IUserService _userService;

        public UserController(IConfiguration config, EstateAgentContext context, IUserService userService)
        {
            _configuration = config;
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(User _userData)
        {
            if (_userData != null && _userData.UserName != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.UserName, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        //new Claim("Role", user.Role),
                        new Claim("UserName", user.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

                    var resp = new AuthorizationResponse
                    {
                        UserId = user.Id.ToString(),
                        AuthorizationToken = encodedJwt,
                        RefreshToken = string.Empty
                    };

                    return Ok(resp);
                }
                else
                {
                    //return BadRequest("Invalid credentials");
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<User> GetUser(string UserName, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == UserName && u.Password == password);
        }

        public class AuthorizationResponse
        {
            public string UserId { get; set; }
            public string AuthorizationToken { get; set; }
            public string RefreshToken { get; set; }
        }


        [HttpGet]
        public IEnumerable<UserDTO> Index()
        {
            var users = _userService.FindAll().ToList();
            return users;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetById(int id)
        {
            var user = _userService.FindById(id);
            return user == null ? NotFound() : user;
        }
        [HttpPost]
        public UserDTO AddUser(UserDTO user)
        {
            user = _userService.Create(user);
            return user;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> UpdateUser(UserDTO user)
        {
            user = _userService.Update(user);
            if (user == null) return NotFound();
            return user;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public HttpStatusCode DeleteUser(int id)
        {
            var user = _userService.FindById(id);
            if (user == null)
                return HttpStatusCode.NotFound;
            _userService.Delete(user);
            return HttpStatusCode.NoContent;
        }

    }
}
