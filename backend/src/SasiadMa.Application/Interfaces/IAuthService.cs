using SasiadMa.Application.DTOs.Auth;
using SasiadMa.Core.Common;

namespace SasiadMa.Application.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<LoginResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<LoginResponse>> GoogleLoginAsync(GoogleLoginRequest request);
    Task<Result<LoginResponse>> RefreshTokenAsync(string refreshToken);
    Task<Result<bool>> ConfirmEmailAsync(string token);
    Task<Result<bool>> ResendConfirmationEmailAsync(string email);
}
