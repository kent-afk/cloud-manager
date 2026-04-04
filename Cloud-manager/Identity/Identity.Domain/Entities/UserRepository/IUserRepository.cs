using System.Threading.Tasks;
using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Entities.UserRepository;

public interface IUserRepository
{
    Task<bool> UserExist(Email email);
}