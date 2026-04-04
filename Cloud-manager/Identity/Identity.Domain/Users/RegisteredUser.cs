namespace Identity.Domain.Users;

public class RegisteredUser(Email.Email email, HashPassword.HashPassword hashPassword, Guid userId) : IUserType
{

    public Email.Email Email { get; private set; } = email;
    public HashPassword.HashPassword HashPassword { get; private set; } = hashPassword;
    
    public Guid UserId { get; } = userId;
    
    public void ChangePassword(HashPassword.HashPassword passwordHash)
    {
        if (passwordHash == HashPassword)
        {
            throw new InvalidOperationException("Passwords do not match.");
        }
        HashPassword = passwordHash;
    }
    
    public void ChangeEmail(Email.Email email)
    { 
        Email = email;   
    }
}