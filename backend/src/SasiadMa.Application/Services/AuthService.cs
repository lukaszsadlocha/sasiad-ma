using SasiadMa.Application.DTOs.Auth;
using SasiadMa.Application.DTOs.User;
using SasiadMa.Application.Interfaces;
using FluentResults;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Interfaces;
using SasiadMa.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace SasiadMa.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var userResult = await _userRepository.GetByEmailAsync(request.Email);
            if (!userResult.IsSuccess)
            {
                return Result.Fail("User with email not found");
            }

            var user = userResult.Value;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Result.Fail("Invalid email or password");
            }

            if (!user.IsActive)
            {
                return Result.Fail("Account is deactivated");
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            var response = new LoginResponse
            {
                User = MapToAuthUser(user),
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            return Result.Ok(response);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred during login");
        }
    }

    public async Task<Result<LoginResponse>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Check if user already exists
            var existingUserResult = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUserResult.IsSuccess)
            {
                return Result.Fail("User with this email already exists");
            }

            // Create user
            var emailResult = Email.Create(request.Email);
            if (!emailResult.IsSuccess)
            {
                return Result.Fail("Invalid email format");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = emailResult.Value,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                EmailConfirmationToken = Guid.NewGuid().ToString(),
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createResult = await _userRepository.CreateAsync(user);
            if (!createResult.IsSuccess)
            {
                return Result.Fail("Failed to create user");
            }

            // Send confirmation email
            await _emailService.SendEmailConfirmationAsync(
                user.Email.Value,
                user.FirstName,
                user.EmailConfirmationToken);

            var token = GenerateJwtToken(createResult.Value);
            var refreshToken = GenerateRefreshToken();

            var response = new LoginResponse
            {
                User = MapToAuthUser(createResult.Value),
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            return Result.Ok(response);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred during registration");
        }
    }

    public Task<Result<LoginResponse>> GoogleLoginAsync(GoogleLoginRequest request)
    {
        // This would integrate with Google OAuth
        // For now, return not implemented
        return Task.FromResult(Result.Fail<LoginResponse>("Google login not yet implemented"));
    }

    public Task<Result<LoginResponse>> RefreshTokenAsync(string refreshToken)
    {
        // This would validate and refresh the token
        // For now, return not implemented
        return Task.FromResult(Result.Fail<LoginResponse>("Token refresh not yet implemented"));
    }

    public async Task<Result<bool>> ConfirmEmailAsync(string token)
    {
        try
        {
            var userResult = await _userRepository.GetByEmailConfirmationTokenAsync(token);
            if (!userResult.IsSuccess)
            {
                return Result.Fail("User with token not found");
            }

            var user = userResult.Value;
            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userRepository.UpdateAsync(user);
            return updateResult.IsSuccess 
                ? Result.Ok(true) 
                : Result.Fail("Failed to update user");
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred during email confirmation");
        }
    }

    public async Task<Result<bool>> ResendConfirmationEmailAsync(string email)
    {
        try
        {
            var userResult = await _userRepository.GetByEmailAsync(email);
            if (!userResult.IsSuccess)
            {
                return Result.Fail("User not found");
            }

            var user = userResult.Value;
            if (user.IsEmailConfirmed)
            {
                return Result.Fail("Email is already confirmed");
            }

            user.EmailConfirmationToken = Guid.NewGuid().ToString();
            await _userRepository.UpdateAsync(user);

            var emailResult = await _emailService.SendEmailConfirmationAsync(
                user.Email.Value,
                user.FirstName,
                user.EmailConfirmationToken);

            return emailResult;
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while resending confirmation email");
        }
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    private UserDto MapToAuthUser(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email.Value,
            ProfileImageUrl = user.ProfileImageUrl,
            Bio = user.Bio,
            PhoneNumber = user.PhoneNumber,
            IsEmailConfirmed = user.IsEmailConfirmed,
            Role = user.Role.ToString(),
            ReputationScore = user.Reputation.Value,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };
    }
}
