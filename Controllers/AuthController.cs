using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }
[HttpPost("register")]
public async Task<IActionResult> Register(RegisterDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.FullName))
    {
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Full name is required."));
    }

    if (string.IsNullOrWhiteSpace(dto.Email))
    {
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Email is required."));
    }

    if (string.IsNullOrWhiteSpace(dto.Password))
    {
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Password is required."));
    }

    var result = await _authService.RegisterAsync(dto);

    if (result is null)
    {
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Email is already registered."));
    }

    return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(
        result,
        "User registered successfully."
    ));
}

[HttpPost("login")]
public async Task<IActionResult> Login(LoginDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Email))
    {
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Email is required."));
    }

    if (string.IsNullOrWhiteSpace(dto.Password))
    {
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Password is required."));
    }

    var result = await _authService.LoginAsync(dto);

    if (result is null)
    {
        return Unauthorized(ApiResponse<AuthResponseDto>.ErrorResponse("Invalid email or password."));
    }

    return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(
        result,
        "Login successful."
    ));
}
}