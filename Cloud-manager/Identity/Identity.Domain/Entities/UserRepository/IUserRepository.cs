using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Entities.UserRepository;

public interface IUserRepository
{
    Task AddAsync(RegisteredUser user, CancellationToken cancellationToken);
    
    Task<bool> EmailExistAsync(Email email);
    
    Task<RegisteredUser?> GetByIdAsync(Guid userId);
    
    Task UpdateAsync(RegisteredUser user, CancellationToken cancellationToken);
    
    Task<RegisteredUser?> GetByEmailAsync(Email email);
}