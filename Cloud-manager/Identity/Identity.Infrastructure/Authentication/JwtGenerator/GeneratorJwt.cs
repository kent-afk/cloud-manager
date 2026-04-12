using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.Ports.JwtGenerator;
using Identity.Domain.ValueObjects;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Authentication.JwtGenerator;

public class GeneratorJwt : IJwtGenerator
{
    private IConfiguration _configuration;

    public GeneratorJwt(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Email email, HashPassword password)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty)); // 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("email", email.Value),
            new Claim("hash", password.Value),
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