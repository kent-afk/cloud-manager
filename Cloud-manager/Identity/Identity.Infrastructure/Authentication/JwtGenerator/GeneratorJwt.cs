using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.Ports.JwtGenerator;
using Identity.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Authentication.JwtGenerator;

public sealed class GeneratorJwt : IJwtGenerator
{
    private readonly IConfiguration _configuration;

    public GeneratorJwt(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(RegisteredUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty)); // 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("email", user.Email.ToString()),
            new Claim("hash", user.HashPassword.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? string.Empty,
            audience: _configuration["Jwt:Audience"] ?? string.Empty,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}