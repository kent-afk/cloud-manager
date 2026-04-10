using System.Threading.Tasks;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects.Email;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistance.Repository;

public class UserRepository : IUserRepository // adapter to port
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

    public async Task<bool> UserExistAsync(Email email)
    {
         return await _context.Users.AnyAsync(x => x.Email == email);
    }
}