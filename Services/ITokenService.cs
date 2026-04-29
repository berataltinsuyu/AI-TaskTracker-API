using AITaskTracker.API.Entities;

namespace AITaskTracker.API.Services;

public interface ITokenService
{
  string CreateToken(User user);
}