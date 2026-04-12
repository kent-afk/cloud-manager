using Identity.Domain.ValueObjects;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Application.Ports.JwtGenerator;

public interface IJwtGenerator
{
    string GenerateToken(Email email, HashPassword password);
}