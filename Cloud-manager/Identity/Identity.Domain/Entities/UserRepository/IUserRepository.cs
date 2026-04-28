using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Entities.UserRepository;

public interface IUserRepository
{
    Task AddAsync(RegisteredUser user, CancellationToken cancellationToken);
    
    Task<bool> EmailExistAsync(Email email,  CancellationToken cancellationToken);
    
    Task<RegisteredUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    Task UpdateAsync(RegisteredUser user, CancellationToken cancellationToken);
    
    Task<RegisteredUser?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
}