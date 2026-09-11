using JwtAuthDotNetAPI.DTOs;
using JwtAuthDotNetAPI.Models;
namespace JwtAuthDotNetAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
