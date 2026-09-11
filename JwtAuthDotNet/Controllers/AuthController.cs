using JwtAuthDotNetAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using JwtAuthDotNetAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using JwtAuthDotNetAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthDotNetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if(user is null)
            {
                return BadRequest("User already exists");
            }
            return Ok(user);
          

        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);
            if (result is null || result.AccessToken is null ||
                result.RefreshToken is null)
            {
                return Unauthorized("invalid refresh token");
            }
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if(result is null)
            {
                return BadRequest("invalid username or password");
            }
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("youare auth admin");
        }
        

    }
}
