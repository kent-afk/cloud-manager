using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Entities.PasswordHasher;

public interface IPasswordHasher
{
    HashPassword Hash(string password);
    
    bool Verify(string providedPassword, string hashedPassword);
}