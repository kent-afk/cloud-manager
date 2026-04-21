using Identity.Domain.ValueObjects.Email;

namespace Identity.Domain.Events;

public record Password_Change_Event(Email Email, string Message): IEvent
{
    public string Route => "password.change";
}