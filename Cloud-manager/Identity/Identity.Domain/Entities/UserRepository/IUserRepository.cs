using System.Threading.Tasks;
using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Entities.UserRepository;

public interface IUserRepository
{
    Task AddAsync(RegisteredUser user);
    Task<bool> EmailExistAsync(Email email);
    
    Task<RegisteredUser?> GetByIdAsync(Guid userId);
    
    Task UpdateAsync(RegisteredUser user);
    
    Task<RegisteredUser?> GetByEmailAsync(Email email);
}