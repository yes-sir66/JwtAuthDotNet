using JwtAuthDotNetAPI.Data;
using JwtAuthDotNetAPI.DTOs;
using JwtAuthDotNetAPI.Models;
using JwtAuthDotNetAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthDotNetAPI.Services.Implimentations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<User?> RegisterAsync(UserDto request)
        { var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user !=null)
            {
                throw new Exception("username is already registered.");
            }
            var userSubmit = new User();
            var hashedpassword = new PasswordHasher<User>()
               .HashPassword(userSubmit, request.Passwrod);
            userSubmit.UserName = request.UserName;
            userSubmit.PasswordHash = hashedpassword;
            _context.Users.Add(userSubmit);
            await _context.SaveChangesAsync();
            return userSubmit;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Passwrod)
                == PasswordVerificationResult.Failed)
            {
                throw new Exception("Paswrod is wrong");
            }
            var response = new TokenResponseDto { 
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
                
            };
            
            return response;
        }
        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if(user is null)
            {
                return null;
            }
            return await CreateTokenResponse(user);
        }
        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)

            };
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
        private async Task<User?> ValidateRefreshTokenAsync(int id,string refreshToken)
        {
            var user = await _context.Users.FindAsync(id);
            if(user is null || user.RefreshToken!= refreshToken ||
        user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            // system.IdentityModel.Tokens.Jwt
            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
