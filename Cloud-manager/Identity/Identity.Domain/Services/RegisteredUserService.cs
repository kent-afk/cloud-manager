using System;
using System.Threading.Tasks;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Entities.Users;
using Identity.Domain.Notification;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Services;

public class RegisteredUserService(IUserRepository userRepository)
{
    public async Task<RegisteredUser> RegisterAsync(Email email, HashPassword hashPassword)
    {

        if (await userRepository.UserExist(email))
        {
            throw new UserAlreadyExist(email);
        }

        return new RegisteredUser(email, hashPassword, Guid.NewGuid());
    }
}