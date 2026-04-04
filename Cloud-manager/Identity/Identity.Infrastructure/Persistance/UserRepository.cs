using System.Threading.Tasks;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.ValueObjects.Email;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistance;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserExist(Email email)
    {
         return await _context.Users.AnyAsync(x => x.Email == email);
    }
}