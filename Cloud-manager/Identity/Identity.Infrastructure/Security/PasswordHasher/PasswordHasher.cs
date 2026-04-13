using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Infrastructure.Security.PasswordHasher;

public sealed class PasswordHasher : IPasswordHasher
{
    public HashPassword Hash(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var pass = BCrypt.Net.BCrypt.HashPassword(password, salt);
        
        return HashPassword.Create(pass);
    }
}