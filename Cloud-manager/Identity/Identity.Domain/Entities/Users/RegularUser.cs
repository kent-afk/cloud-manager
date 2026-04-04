using System;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Entities.Users;

public class RegularUser : RegisteredUser
{
    public RegularUser(Email email, HashPassword hashPassword, Guid userId) : base(email, hashPassword, userId)
    {
    }
}