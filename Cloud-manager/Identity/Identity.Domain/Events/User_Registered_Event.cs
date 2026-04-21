using System;

namespace Identity.Domain.Events;

public record User_Registered_Event(Guid ClientId, string Email): IEvent
{
    public string Route => "user.register";
}
