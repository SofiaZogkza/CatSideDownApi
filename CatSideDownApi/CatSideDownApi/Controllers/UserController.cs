using CatSideDownApi.Contracts;
using CatSideDownApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CatSideDownApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {        
        private readonly IUserServices _service;
        private IConfiguration _config;
        private readonly IPrincipal _user;

        public UserController(IUserServices service, IConfiguration config, IPrincipal user)
        {
            _service = service;
            _config = config;
            _user = user;
        }

        [HttpGet]
        [Route("get")]
        [AllowAnonymous]
        public string Get()
        {
            return "Cat-Side-Down API is Running . . .";
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInRequest req)
        {
            var result =  _service.LogIn(req);
            var response = new LogInResponse();

            if (result.Response.Success == true)
            {
                var tokenStr = GenerateJSONWebToken(result);
                response = new LogInResponse()
                {
                    Response = new Response()
                    {
                        Success = true,
                        Message = "You have the Token"
                    },
                    JwtToken = tokenStr
                };
            }
            else
            {
                response = result;
            }
            return await Task.FromResult(response);
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<ActionResult<Response>> LogOut(LogOutRequest req)
        {
            var result = _service.LogOut(req);
            return await Task.FromResult(result);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<Response>> Register([FromBody] RegisterRequest req)
        {
            var result = await _service.Register(req);
            return result;
        }


        [HttpGet]
        [Route("flip")]
        [Authorize]
        public async Task<IActionResult> GetCatPictureFromCaatas()
        {
            var result = await _service.GetCatPictureFromCaatas();
            return File(result, "image/jpeg");
            
        }

        [HttpGet]
        [Route("getuser")]
        [Authorize]
        public async Task<User> GetUserInformation()
        {
            var email = _user?.Identity?.Name;
            var result = await _service.GetUserInformation(email);
            return result;
        }


        private string GenerateJSONWebToken(LogInResponse user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            if (user.Response.Success == true)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.User.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.User.Email)
                };

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: credentials
                );

                var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
                return encodeToken;
            }          
                        
            return user.Response.Message;
        }
    }
}
