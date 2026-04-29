using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AITaskTracker.API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AITaskTracker.API.Services;

public class TokenService : ITokenService
{
  private readonly IConfiguration _configuration;

  public TokenService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string CreateToken(User user)
  {
      var jwtSettings= _configuration.GetSection("JwtSettings");

      var secretKey = jwtSettings["SecretKey"];
      var issuer = jwtSettings["Issuer"];
      var audience = jwtSettings["Audience"];
      var expirationMinutes = Convert.ToDouble(jwtSettings["ExpirationMinutes"]);

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer : issuer,
        audience : audience,
        claims : claims,
        expires : DateTime.UtcNow.AddMinutes(expirationMinutes),
        signingCredentials : credentials
      );
      
      return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
  

