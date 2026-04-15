using Identity.Domain.Entities.Users;

namespace Identity.Application.Ports.JwtGenerator;

public interface IJwtGenerator
{
    string GenerateToken(RegisteredUser user);
}