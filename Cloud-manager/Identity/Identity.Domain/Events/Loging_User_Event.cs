using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Events;

public record Loging_User_Event(Email Email, string HashPassword);