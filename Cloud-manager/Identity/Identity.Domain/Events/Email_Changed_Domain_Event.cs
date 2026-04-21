using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.HashPassword;

namespace Identity.Domain.Events;

public record Email_Changed_Domain_Event(Email NewEmail, HashPassword Password): IEvent
{
    public string Route => "email.change";
}