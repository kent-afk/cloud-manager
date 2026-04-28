using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;
using Identity.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repository;

public sealed class UserRepository : IUserRepository // adapter to port
{
    private readonly ApplicationDbContext _context;
    
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RegisteredUser user, CancellationToken cancellationToken) // async for I/O 
    { 
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken); // commit changes in db
        // no return, background status
    }

    public async Task<bool> EmailExistAsync(Email email, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Email == email,
            cancellationToken: cancellationToken);
    }


    public async Task<RegisteredUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId, 
            cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(RegisteredUser user, CancellationToken cancellationToken)
    { 
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RegisteredUser?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email, 
            cancellationToken: cancellationToken);
    }
}