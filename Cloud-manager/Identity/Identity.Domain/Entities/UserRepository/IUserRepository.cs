using System.Threading.Tasks;
using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Entities.UserRepository;

public interface IUserRepository
{
    Task AddAsync(RegisteredUser user);
    Task<bool> UserExistAsync(Email email);
}