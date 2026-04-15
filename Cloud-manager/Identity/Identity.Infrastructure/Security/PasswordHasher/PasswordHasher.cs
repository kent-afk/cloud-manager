using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Infrastructure.Security.PasswordHasher;

public sealed class PasswordHasher : IPasswordHasher
{
    public HashPassword Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));
        
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var pass = BCrypt.Net.BCrypt.HashPassword(password, salt);
        
        return HashPassword.Create(pass);
    }

    public bool Verify(string providedPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}