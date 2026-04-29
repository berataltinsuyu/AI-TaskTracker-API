using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;


public interface IAuthService
{
  Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
  Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}