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

    public async Task<bool> UserExistAsync(Email email, HashPassword password)
    {
         return await _context.Users.AnyAsync(x => x.Email == email) && 
             await _context.Users.AnyAsync(x => x.HashPassword == password);
    }
}