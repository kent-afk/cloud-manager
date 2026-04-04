using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Users;

public class RegisteredUser(Email email, HashPassword hashPassword, System.Guid userId) : IUserType
{

    public Email Email { get; private set; } = email;
    public HashPassword HashPassword { get; private set; } = hashPassword;
    
    public System.Guid UserId { get; } = userId;

    private RegisteredUser() : this(default!,  default!, default!) {}
    
    public void ChangePassword(HashPassword passwordHash)
    {
        if (passwordHash == HashPassword)
        {
            throw new System.InvalidOperationException("Passwords do not match.");
        }
        HashPassword = passwordHash;
    }
    
    public void ChangeEmail(Email email)
    { 
        Email = email;   
    }
}