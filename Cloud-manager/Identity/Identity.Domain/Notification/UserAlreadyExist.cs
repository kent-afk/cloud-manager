using System;
using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Notification;

public class UserAlreadyExist : Exception
{
    public UserAlreadyExist(Email email) : base($"This email {email.Value} is already used.") { }
}