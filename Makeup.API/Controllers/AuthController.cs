using Makeup.Services.DTO;
using Makeup.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Makeup.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ApiAuthService _apiAuthService;
        public AuthController(IConfiguration configuration, ApiAuthService
       apiAuthService)
        {
            Configuration = configuration;
            _apiAuthService = apiAuthService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto login)
        {
            var apiUser = await _apiAuthService.Authenticate(login);
            if (apiUser != null)
            {
                var token = GenerateJwtToken(apiUser.Username!, apiUser.Role!);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
        private string GenerateJwtToken(string username, string rolename)
        {
            var securityKey = new
           SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey,
           SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
             new Claim(JwtRegisteredClaimNames.Sub, username),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(ClaimTypes.Role, rolename)
             };
            var token = new JwtSecurityToken(
            Configuration["Jwt:Issuer"],
            Configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
