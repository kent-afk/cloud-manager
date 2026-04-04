using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Users;

public interface IUserType
{
    Email Email { get; }
    HashPassword HashPassword { get; }
}