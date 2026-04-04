namespace Identity.Domain.Users;

public interface IUserType
{
    Email.Email Email { get; }
    HashPassword.HashPassword HashPassword { get; }
}