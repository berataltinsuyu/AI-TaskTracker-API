using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    var result = await _authService.RegisterAsync(dto);

    if (result is null)
    {
      return BadRequest("Email is already registered.");
    }

    return Ok(result);
  }

  [HttpPost("Login")]
  public async Task<IActionResult> Login(LoginDto dto)
  {
    var result = await _authService.LoginAsync(dto);

    if (result is null)
    {
      return Unauthorized("Invalid email or password.");
    }

    return Ok(result);
  }
}