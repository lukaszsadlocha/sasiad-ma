using SasiadMa.Application.DTOs.Auth;
using SasiadMa.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SasiadMa.Api.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("User login");

        group.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("User registration");

        group.MapPost("/google-login", GoogleLoginAsync)
            .WithName("GoogleLogin")
            .WithSummary("Google OAuth login");

        group.MapPost("/refresh", RefreshTokenAsync)
            .WithName("RefreshToken")
            .WithSummary("Refresh access token");

        group.MapPost("/confirm-email", ConfirmEmailAsync)
            .WithName("ConfirmEmail")
            .WithSummary("Confirm email address");

        return app;
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        IAuthService authService)
    {
        var result = await authService.LoginAsync(request);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        IAuthService authService)
    {
        var result = await authService.RegisterAsync(request);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> GoogleLoginAsync(
        [FromBody] GoogleLoginRequest request,
        IAuthService authService)
    {
        var result = await authService.GoogleLoginAsync(request);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        IAuthService authService)
    {
        var result = await authService.RefreshTokenAsync(request.RefreshToken);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> ConfirmEmailAsync(
        [FromQuery] string token,
        IAuthService authService)
    {
        var result = await authService.ConfirmEmailAsync(token);
        
        return result.IsSuccess 
            ? Results.Ok(new { message = "Email confirmed successfully" })
            : Results.BadRequest(result.Error);
    }
}
