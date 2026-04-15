using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repository;

public sealed class UserRepository : IUserRepository // adapter to port
{
    private readonly ApplicationDbContext _context;
    
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RegisteredUser user) // async for I/O 
    { 
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(); // commit changes in db
        // no return, background status
    }

    public async Task<bool> EmailExistAsync(Email email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }


    public async Task<RegisteredUser?> GetByIdAsync(Guid userId)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task UpdateAsync(RegisteredUser user)
    { 
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<RegisteredUser?> GetByEmailAsync(Email email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}