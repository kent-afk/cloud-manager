using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Entities.Users;

public abstract class RegisteredUser
{
    public Email Email { get; private set; }
    public HashPassword HashPassword { get; private set; }
    
    public System.Guid UserId { get; private set; }

    public RegisteredUser(Email email, HashPassword hashPassword, System.Guid userId)
    {
        Email = email;
        HashPassword = hashPassword;
        UserId = userId;
    }
    
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